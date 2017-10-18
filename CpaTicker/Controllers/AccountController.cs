using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using CpaTicker.Filters;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Models;
using CpaTicker.Areas.admin.Classes.SecurityLib;
using CpaTicker.Areas.admin.Classes.LimeLightLib;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Text;
using CpaTicker.Areas.admin.Classes.Helpers;
using System.Collections.Specialized;


namespace CpaTicker.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private CpaTickerDb db = new CpaTickerDb();
        private ICpaTickerRepository repo;

        public AccountController()
            : this(new EFCpatickerRepository())
        {

        }

        public AccountController(ICpaTickerRepository repo)
        {
            this.repo = repo;
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //if (WebSecurity.IsAuthenticated)
            //{
            //    return RedirectToLocal(returnUrl);
            //}

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IsDomainExist = repo.GetCustomerByURL(Request.Url);

            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            ViewBag.IsDomainExist = repo.GetCustomerByURL(Request.Url);
            // grab the customer id from username
            if (ModelState.IsValid)
            {
                if (WebSecurity.UserExists(model.UserName))
                {
                    int customerid = CPAHelper.GetCustomerIdFromUsername(model.UserName);
                    int declines = CPAHelper.GetCustomerDeclines(customerid);

                    if (declines > 2)
                    {
                        ModelState.AddModelError("", "This account had been suspended because of payments declines");
                        return View(model);
                    }
                }
                if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    loginhistory lgnobj = new loginhistory();
                    lgnobj.Date = DateTime.UtcNow;
                    lgnobj.IPAddress = Request.UserHostAddress;
                    lgnobj.UserAgent = Request.UserAgent;
                    lgnobj.UserName = model.UserName;
                    db.loginhistory.Add(lgnobj);
                    db.SaveChanges();
                    return RedirectToLocal(returnUrl);
                }
                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(Convert.ToString(Session["username"])))
                {
                    var username = Convert.ToString(Session["username"]);
                    UserProfile userProfile = db.UserProfiles.FirstOrDefault(u => u.UserName == username);
                    if (userProfile != null)
                    {

                        WebSecurity.Logout();
                        Session.Abandon();
                        FormsAuthentication.SetAuthCookie(username, false);
                        return RedirectToAction("Dashboard", "Home", new { area = "admin" });
                    }
                }
            }
            catch
            {


            }




            WebSecurity.Logout();

            //return RedirectToAction("Index", "Home");

            return RedirectToAction("Login", new { ReturnUrl = "/admin" }); // logout redirect to login page
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Register()
        {
            RegisterModel rm = new RegisterModel();
            rm.SelectedCountry = db.Countries.Single(c => c.CountryAbbreviation == "US").Id;
            if (Request.QueryString["level"] != null)
            {
                switch (Request.QueryString["level"])
                {
                    //case "Gold":
                    //    rm.Level = Level.Gold;
                    //    break;
                    case "Platinum":
                        rm.Level = Level.Platinum;
                        break;
                    //case "Diamond":
                    //    rm.Level = Level.Diamond;
                    //    break;
                    default:
                        rm.Level = Level.Gold;
                        break;
                }
            }
            return View(rm);
        }

        [AllowAnonymous]
        public JsonResult GetStates(int id)
        {
            var result = db.States.Where(s => s.CountryId == id).OrderBy(s => s.StateName).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [RequireHttps]
        //[ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //using (var scope = new TransactionScope())
                    //{

                    var guid = Guid.NewGuid();
                    SecureCard sc = new SecureCard(model.CreditCardNumber, model.CreditCardExpMonth, model.CreditCardExpYear, model.CVV);

                    Customer customer = new Customer();
                    customer.AccountId = model.AccountId;
                    customer.FirstName = model.FirstName;
                    customer.LastName = model.LastName;
                    customer.Phone = model.Phone;
                    customer.Address = model.Address;
                    customer.TimeZone = model.TimeZone;
                    customer.APIKey = guid;
                    customer.MemberSince = System.DateTime.Today;
                    customer.Zip = model.Zip;
                    customer.City = model.City;
                    customer.State = model.SelectedState;

                    //customer.Email = model.Email;

                    customer.CompanyName = model.CompanyName;
                    customer.Level = model.Level;
                    customer.CreditCardData = sc.EncryptedData;
                    customer.CountryId = model.SelectedCountry;
                    //customer.IPAddress = Request.UserHostAddress;



                    // place a new order 
                    //customer.Country = repo.GetCountries().Single(c => c.Id == customer.CountryId); 
                    customer.Country = repo.FindCountry(customer.CountryId);
                    var response = LimeLightHelper.NewLimeLightOrder(repo, customer
                        , model.Email
                        , CpaTickerConfiguration.LimeLightProductId(customer.Level)
                        , Request.UserHostAddress);

                    // if there is any error with limelight an exception is in place
                    repo.AddCustomer(customer);

                    var order = new Order() { CustomerId = customer.CustomerId, OrderId = response.OrderId() };
                    repo.AddOrder(order);

                    int? apiorderid = null;
                    if (model.HasAPIKey)
                    {
                        response = LimeLightHelper.NewLimeLightOrder(repo, customer, model.Email
                            , CpaTickerConfiguration.LimeLightAPIKeyProductId
                            , Request.UserHostAddress
                            , order.OrderId);
                        apiorderid = response.OrderId();

                        // this is already try catched
                        CPAHelper.SendAPIKeyMail(model.UserName, model.Email, guid.ToString());
                    }

                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password,
                        propertyValues: new
                        {
                            CustomerId = customer.CustomerId,
                            APIKey = guid,
                            OrderId = apiorderid,
                            Email = model.Email

                        });

                    Roles.AddUserToRole(model.UserName, "Administrator");

                    //    scope.Complete();
                    //}
                    var ipaddress = Request.UserHostAddress;
                    CPAHelper.SendEmailNewRegisterEmail("noreply@clickticker.com", model);
                    CPAHelper.SendEmailClickTickerSupport("noreply@clickticker.com", model, ipaddress);
                    WebSecurity.Login(model.UserName, model.Password);
                    //return RedirectToAction("index", "admin");

                    // send welcome email
                    //CPAHelper.SendMail("noreply@clickticker.com", model.Email, "Welcome to www.ClickTicker.com", "");

                    return RedirectToAction("thanks");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
                catch (LimeLightException e)
                {
                    ModelState.AddModelError("", e.Message);
                }

            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Please correct the errors below.");
            return View(model);
        }


        public ActionResult Thanks()
        {
            return View();
        }

        private void RecordLimeLightTransaction(CpaTicker.Areas.admin.Classes.Transaction transaction, string product_id_csv)
        {
            string[] productid = product_id_csv.Split(',');
            if (productid[0] == CpaTickerConfiguration.LimeLightAPIKeyProductId.ToString())
            {
                // this a transaction from an apikeproduct

                if (!transaction.Status)
                {
                    // is a decline so revoke the apikey and cancel the apikey suscription for that user
                    var user = repo.UserProfile().FirstOrDefault(u => u.OrderId == transaction.OrderId);
                    if (user != null)
                    {
                        user.OrderId = null;
                        LimeLightHelper.CancelLimeLightOrder(repo, transaction.OrderId);
                        repo.EditUserProfile(user);
                    }
                }
            }
            else
            {
                // this a transaction from an customer package
                this.repo.AddTransaction(transaction);

                // if there is a decline record it 1 For Approvals, 0 For Declines status = true if order_status=1

                var order = this.repo.FindOrder(transaction.OrderId);

                if (!transaction.Status)
                {
                    // add a decline to the order

                    //this.repo.AddOrderDecline(transaction.OrderId);
                    order.Declines++;

                    if (order.Declines > 2)
                    {
                        // cancel the order subscription
                        LimeLightHelper.CancelLimeLightOrder(repo, order.OrderId);
                    }
                }
                else
                {
                    // success
                    // reset the order declines to 0
                    order.Declines = 0;
                }

                // save the order
                repo.EditOrder(order);
            }
        }

        [AllowAnonymous]
        public ActionResult LimeLightResponse(string recurring_date = "", string order_status = "", string order_id = "",
            string customer_id = "", string first_name = "", string last_name = "", string email = "", string phone = "",
            string shipping_address = "", string shipping_city = "", string shipping_zip = "", string shipping_state_desc = "",
            string shipping_state_id = "", string shipping_country = "", string billing_address = "", string billing_city = "",
            string billing_zip = "", string billing_state_desc = "", string billing_state_id = "", string billing_country = "",
            string campaign_id = "", string rebill_depth = "", string parent_order_id = "", string is_recurring = "",
            string gateway_id = "", string payment_method = "", string transaction_id = "", string authorization_id = "",
            string affiliate = "", string sub_affiliate = "", string click_id = "", string was_reprocessed = "",
            string is_test_cc = "", string is_fraud = "", string is_shippable = "", string shipping_method = "",
            string shipping_id = "", string shipping_group_name = "", string product_names_csv = "", string product_id_csv = "",
            string product_skus_csv = "", string ip_address = "", string currency_code = "", string order_total = "",
            string shipping_total = "", string non_taxable_amount = "", string taxable_amount = "", string tax_factor = "",
            string sales_tax_percent = "", string digital_delivery_username = "", string digital_delivery_password = "",
            string decline_reason = "", string order_date = "", string order_date_time = "", string total_no_shipping = "",
            string post_back_action = "", string subscription_active_csv = "", string campaign_name = "", string campaign_desc = "",
            string subscription_id_csv = "", string prepaid_match = "", string void_refund_amount = "")
        {
            CpaTicker.Areas.admin.Classes.Transaction transaction = new CpaTicker.Areas.admin.Classes.Transaction()
            {
                TransactionId = int.Parse(order_id),
                OrderId = int.Parse(parent_order_id),
                Status = order_status == "1"
            };

            RecordLimeLightTransaction(transaction, product_id_csv);

            //this.repo.AddTransaction(transaction);

            //// if there is a decline record it 1 For Approvals, 0 For Declines status = true if order_status=1
            //if (!transaction.Status)
            //{
            //    this.repo.AddOrderDecline(transaction.OrderId);
            //}

            return Content(transaction.Status.ToString());
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult LimeLightResponse()
        {
            LimeLightStatus status = new LimeLightStatus(Request.InputStream);

            CpaTicker.Areas.admin.Classes.Transaction transaction = new CpaTicker.Areas.admin.Classes.Transaction()
            {
                TransactionId = status.OrderId,
                OrderId = status.ParentOrderId,
                Status = status.OrderStatus
            };

            this.RecordLimeLightTransaction(transaction, status.Allfields["product_id_csv"]);

            return Content(status.OrderStatus.ToString());
        }



        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (var db = new CpaTickerDb())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        [AllowAnonymous]
        public ActionResult Affiliate()
        {
            ViewBag.Countries = repo.GetCountries();
            ViewBag.States = repo.GetCountryStates(228);
            ViewBag.Message = "";
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Affiliate(Affiliate affiliate)
        {
            string Message = "";
            ViewBag.Countries = repo.GetCountries();
            ViewBag.States = repo.GetCountryStates(228);
            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        affiliate.CustomerId = repo.GetCustomerId(Request.Url);
                    }
                    catch (Exception ex)
                    {
                        Message = "Customer not found.";
                        return Json(Message);
                    }

                    affiliate.Status = AffiliateStatus.Pending;
                    using (TransactionScope scope = new TransactionScope())
                    {
                        int maxAffiliateId = db.Affiliates.Where(a => a.CustomerId == affiliate.CustomerId && a.AffiliateId < 9000)
                            .Max(a => (int?)a.AffiliateId) ?? 1000;
                        affiliate.AffiliateId = maxAffiliateId + 1;
                        db.Affiliates.Add(affiliate);
                        db.SaveChanges();
                       
                        affiliate.Country = db.Countries.Single(u => u.Id == affiliate.CountryId);
                        var AdminList = db.UserProfiles.Where(u => u.CustomerId == affiliate.CustomerId && u.AffiliateId == null && u.Email != null).Select(u => u.Email).ToArray();
                        string toemail = string.Join(",", AdminList);
                        CPAHelper.SendEmailForAffiliateSignup("noreply@clickticker.com", affiliate, toemail);
                        scope.Complete();
                        Message = "Your Request sent to administrator..";
                    }
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }


            }
            return Json(Message);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("index", "admin");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }


}
