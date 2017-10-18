using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes;
using WebMatrix.WebData;
using CpaTicker.Areas.admin.Classes.Helpers;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using CpaTicker.Areas.admin.Classes.SecurityLib;
using CpaTicker.Areas.admin.Classes.LimeLightLib;
using System.Data.Entity.Infrastructure;
using System.Collections.Specialized;
using CpaTicker.Models;
using CpaTicker.Areas.admin.Models;
using System.Transactions;
using System.Threading.Tasks;

namespace CpaTicker.Areas.admin.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        private CpaTickerDb db = new CpaTickerDb();
        private ICpaTickerRepository repo;

        public SettingsController()
        {
            this.repo = new EFCpatickerRepository();
        }

        private Customer CurrentCustomer
        {
            get
            {
                int customerid = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
                return db.Customers.Find(customerid);
            }
        }

        //
        // GET: /Settings/// njhones changes here

        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [CacheControl(HttpCacheability.NoCache), HttpGet]
        public ActionResult Index()
        {
            // get the current logged customer
            //var customer = repo.GetCurrentCustomer();
            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);
            return View();
        }

        //
        // GET: /Settings/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    Customer customer = db.Customers.Find(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(customer);
        //}

        //
        // GET: /Settings/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /Settings/Create

        //[HttpPost]
        //public ActionResult Create(Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Customers.Add(customer);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(customer);
        //}



        public ActionResult EditUser(int id)
        {
            var user = repo.FindUserProfile(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.Affiliates = repo.GetCustomerActiveAffiliates(user.CustomerId).OrderBy(u => u.AffiliateId);
            ViewBag.User = user;

            ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(user.CustomerId).AsEnumerable()
                                          join s in repo.GetUserAffiliates(user.UserId).Select(a => a.AffiliateId) on a.AffiliateId equals s into sa
                                          from s in sa.DefaultIfEmpty()
                                          select new SelectListItem
                                          {
                                              Value = a.AffiliateId.ToString(),
                                              Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                              Selected = s != 0
                                          }).OrderBy(u => u.Value);


            return View(new EditUserVM()
            {
                HasAPIKey = user.OrderId.HasValue,
                UserName = user.UserName,
                UserId = user.UserId,
                Email = user.Email,
                AffiliateId = user.AffiliateId
            });
        }

        public ActionResult Email(int id)
        {
            var user = repo.FindUserProfile(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            try
            {
                CPAHelper.SendAPIKeyMail(user);
            }
            catch
            {
                ViewBag.Error = true;
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(EditUserVM model, int[] aff = null)
        {
            int customerid = 0;
            var user = repo.FindUserProfile(model.UserId);
            if (ModelState.IsValid)
            {
                try
                {


                    customerid = user.CustomerId;
                    model.HasAPIKey = Request["hasapi"] == "on";
                    model.SendEmail = !string.IsNullOrEmpty(Request["send"]);

                    if (model.HasAPIKey)
                    {
                        // set the limelight call and generate the API

                        if (!user.OrderId.HasValue) // if the user don't have an order then place one
                        {
                            var customer = repo.GetCurrentCustomer(user.CustomerId);
                            var order = repo.Orders().Single(o => o.CustomerId == customer.CustomerId);
                            var response = LimeLightHelper.NewLimeLightOrder(repo, customer, user.Email, CpaTickerConfiguration.LimeLightAPIKeyProductId
                                , Request.UserHostAddress
                                , order.OrderId);

                            //var tmp = response.errorFound(); //false; //

                            //if (tmp)
                            //{
                            //    throw new LimeLightException("Sorry, your credict card have been decline. {0}", response.errorMessage());
                            //}

                            user.OrderId = response.OrderId();

                        }

                        if (model.SendEmail)
                        {
                            CPAHelper.SendAPIKeyMail(user);
                        }
                    }

                    else if (user.OrderId.HasValue) // manully cancel the order
                    {
                        // cancel the subscription 
                        //user.APIKey = null;
                        LimeLightHelper.CancelLimeLightOrder(repo, user.OrderId.Value);

                        user.OrderId = null;
                    }

                    if (model.AffiliateId.HasValue && !user.AffiliateId.HasValue) // turning user into affiliate
                    {
                        user.Permissions = user.Permissions | CpaTickerConfiguration.DefaultAffiliateDynamicRestrictions;
                        Roles.RemoveUserFromRole(user.UserName, "Administrator");
                        Roles.AddUserToRole(user.UserName, "Affiliate");
                    }
                    else if (!model.AffiliateId.HasValue && user.AffiliateId.HasValue) // turn into admin
                    {
                        user.Permissions = 0;
                        user.Permissions1 = 0; // grant all privileges
                        Roles.RemoveUserFromRole(user.UserName, "Affiliate");
                        Roles.AddUserToRole(model.UserName, "Administrator");
                    }

                    user.AffiliateId = model.AffiliateId;
                    user.UserName = model.UserName;
                    user.Email = model.Email;

                    // update the user
                    repo.EditUserProfile(user);


                    //UserAffiliates
                    try
                    {
                        db.UserAffiliate.RemoveRange(db.UserAffiliate.Where(c => c.UserId == user.UserId));
                        db.SaveChanges();
                        for (int a = 0; a < aff.Length; a++)
                        {
                            UserAffiliate objUseraff = new UserAffiliate();
                            objUseraff.AffiliateId = aff[a];
                            objUseraff.CustomerId = user.CustomerId;
                            objUseraff.UserId = user.UserId;
                            db.UserAffiliate.Add(objUseraff);
                            if (a == aff.Length - 1)
                            { db.SaveChanges(); }
                        }
                    }
                    catch
                    {

                    }
                    //

                    //var Userprofile = repo.UserProfile().SingleOrDefault(u => u.UserName == model.UserName);
                    return RedirectToAction("SetPermissions", new { id = model.UserId });
                    //return RedirectToAction("index");
                }
                catch (LimeLightException e)
                {
                    ModelState.AddModelError("", e.Message);
                }

            }

            ViewBag.Affiliates = repo.GetCustomerActiveAffiliates(customerid).OrderBy(u => u.AffiliateId);
            ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(user.CustomerId).AsEnumerable()
                                          join s in repo.GetUserAffiliates(user.UserId).Select(a => a.AffiliateId) on a.AffiliateId equals s into sa
                                          from s in sa.DefaultIfEmpty()
                                          select new SelectListItem
                                          {
                                              Value = a.AffiliateId.ToString(),
                                              Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                              Selected = s != 0
                                          }).OrderBy(u => u.Value);
            return View(model);
        }


        //
        // GET: /Settings/Edit/5
        //[AuthorizeUser(RequiredPermission = ViewPermission.EditCustomer)]
        public ActionResult Edit()
        {
            var user = repo.GetCurrentUser();
            Customer customer = repo.GetCurrentCustomer(user.CustomerId);
            SecureCard sc = new SecureCard(customer.CreditCardData);
            //ViewBag.CardNumber = sc.CardNumber;

            EditModel rm = new EditModel();
            rm.Address = customer.Address;

            rm.FirstName = customer.FirstName;
            rm.LastName = customer.LastName;
            rm.SelectedState = customer.State;
            rm.City = customer.City;
            rm.Zip = customer.Zip;
            rm.TimeZone = customer.TimeZone;
            rm.Phone = customer.Phone;
            rm.CompanyName = customer.CompanyName;
            rm.Email = user.Email;
            rm.CreditCardExpMonth = sc.ExpiryMonth;
            rm.CreditCardExpYear = sc.ExpiryYear;
            rm.CreditCardNumber = sc.CardNumber;
            rm.Level = customer.Level;
            rm.HasAPIKey = user.OrderId.HasValue;
            rm.TransactionIdType = customer.TransactionIdType;
            rm.SelectedCountry = customer.CountryId;

            // assing values to 
            return View(rm);
        }



        //// POST: /Settings/Edit/5
        [HttpPost]
        public ActionResult Edit(EditModel model)
        {
            model.HasAPIKey = Request["hasapi"] == "on";
            model.SendEmail = !string.IsNullOrEmpty(Request["send"]);
            try
            {
                if (ModelState.IsValid)
                {
                    var user = repo.GetCurrentUser();
                    var customer = repo.GetCurrentCustomer(user.CustomerId);

                    customer.CompanyName = model.CompanyName;
                    user.Email = model.Email;
                    customer.FirstName = model.FirstName;
                    customer.LastName = model.LastName;
                    customer.TimeZone = model.TimeZone;
                    customer.Address = model.Address;
                    customer.State = model.SelectedState; //Request.Form["SelectedState"];
                    customer.City = model.City;
                    customer.Phone = model.Phone;
                    customer.Zip = model.Zip;
                    customer.Level = model.Level;
                    //customer.CreditCardData = customer.CreditCardData;
                    customer.CountryId = model.SelectedCountry;
                    customer.Country = repo.FindCountry(customer.CountryId);
                    customer.TransactionIdType = model.TransactionIdType;

                    var actualsc = new SecureCard(customer.CreditCardData); // this line is just for extract the CVV ???
                    SecureCard sc = new SecureCard(model.CreditCardNumber, model.CreditCardExpMonth, model.CreditCardExpYear, actualsc.CVV);

                    customer.CreditCardData = sc.EncryptedData;

                    // update the order
                    var order = repo.Orders().Single(o => o.CustomerId == customer.CustomerId);
                    LimeLightHelper.UpdateLimeLightOrder(repo, order.OrderId, customer, user.Email, sc, CpaTickerConfiguration.LimeLightProductId(customer.Level));
                    repo.EditCustomer(customer);

                    // place a new apikeyorder if the customer doesn't have an api and it is requested or cancel the apikeyorder if requested
                    if (model.HasAPIKey)
                    {
                        if (!user.OrderId.HasValue)
                        {
                            // place new apikey order api
                            var apikeyrequest = LimeLightHelper.NewLimeLightOrder(repo, customer, user.Email, CpaTickerConfiguration.LimeLightAPIKeyProductId
                                , Request.UserHostAddress
                                , order.OrderId);
                            user.OrderId = apikeyrequest.OrderId();
                            //repo.EditUserProfile(user);
                        }

                        if (model.SendEmail)
                        {
                            CPAHelper.SendAPIKeyMail(user);
                        }
                    }
                    else
                    {
                        //cancel the apikey subscription if present
                        if (user.OrderId.HasValue)
                        {
                            LimeLightHelper.CancelLimeLightOrder(repo, user.OrderId.Value);
                            user.OrderId = null;
                            //repo.EditUserProfile(user);
                        }
                    }

                    repo.EditUserProfile(user);

                    // if we reach this point is because everything goes well so update all Apikeys orders associated with this customer
                    // no more than 30 request / min

                    var apiorders = repo.UserProfile()
                        .Where(u => u.CustomerId == user.CustomerId)
                        .Select(u => u.OrderId).Where(u => u.HasValue)
                        .ToList();

                    System.Threading.Tasks.Task.Factory.StartNew(() => UpdateAllUserApiKeyOrders(apiorders, customer, user.Email, sc, repo));

                    //foreach (var orderid in apiorders)
                    //{
                    //    // must not give error except the too many call error
                    //    //await LimeLightHelper.UpdateLimeLightOrderAsync(orderid, customer, sc, CpaTickerConfiguration.LimeLightAPIKeyProductId);

                    //    // what happened if the an exception is found what happened if during the execution the customer get updated
                    //    // or just the for example a user get an order suspended should i wait for this method to return yet
                    //    LimeLightHelper.UpdateLimeLightOrder(orderid.Value, customer, sc, CpaTickerConfiguration.LimeLightAPIKeyProductId);

                    //    //LimeLightHelper.UpdateLimeLightOrderAsync(orderid.Value, customer, sc, CpaTickerConfiguration.LimeLightAPIKeyProductId);
                    //}


                    return RedirectToAction("Index");

                }

            }
            catch (LimeLightException e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(model);
        }

        private void UpdateAllUserApiKeyOrders(List<int?> orders, Customer customer, string customer_email, SecureCard sc, ICpaTickerRepository repo)
        {
            for (int i = 0; i < orders.Count(); i++)
            {
                // must not give error except the too many call error
                //await LimeLightHelper.UpdateLimeLightOrderAsync(orderid, customer, sc, CpaTickerConfiguration.LimeLightAPIKeyProductId);

                // what happened if the an exception is found what happened if during the execution the customer get updated
                // or just the for example a user get an order suspended should i wait for this method to return yet

                try
                {
                    LimeLightHelper.UpdateLimeLightOrder(repo, orders[i].Value, customer, customer_email, sc, CpaTickerConfiguration.LimeLightAPIKeyProductId);
                }
                catch (LimeLightException e)
                {

                    if (e.Message.Equals("410"))
                    {
                        // retry in 15s
                        System.Threading.Thread.Sleep(15000);
                        i--;
                    }
                }

                //LimeLightHelper.UpdateLimeLightOrderAsync(orderid.Value, customer, sc, CpaTickerConfiguration.LimeLightAPIKeyProductId);
            }
        }

        //[HttpPost] // Async version
        //[ActionName("Edit")]
        //public async Task<ActionResult> Edit(EditModel model)
        //{
        //    model.HasAPIKey = Request["hasapi"] == "on";
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = repo.GetCurrentUser();
        //            var customer = repo.GetCurrentCustomer(user.CustomerId);

        //            customer.CompanyName = model.CompanyName;
        //            customer.Email = model.Email;
        //            customer.FirstName = model.FirstName;
        //            customer.LastName = model.LastName;
        //            customer.TimeZone = model.TimeZone;
        //            customer.Address = model.Address;
        //            customer.State = model.SelectedState; //Request.Form["SelectedState"];
        //            customer.City = model.City;
        //            customer.Phone = model.Phone;
        //            customer.Zip = model.Zip;
        //            customer.Level = model.Level;
        //            //customer.CreditCardData = customer.CreditCardData;
        //            customer.CountryId = model.SelectedCountry;
        //            customer.Country = repo.FindCountry(customer.CountryId);

        //            var actualsc = new SecureCard(customer.CreditCardData); // this line is just for extract the CVV ???
        //            SecureCard sc = new SecureCard(model.CreditCardNumber, model.CreditCardExpMonth, model.CreditCardExpYear, actualsc.CVV);

        //            customer.CreditCardData = sc.EncryptedData;

        //            // update the order
        //            var order = repo.Orders().Single(o => o.CustomerId == customer.CustomerId);
        //            await LimeLightHelper.UpdateLimeLightOrderAsync(order.OrderId, customer, sc, CpaTickerConfiguration.LimeLightProductId(customer.Level));


        //            // place a new apikeyorder if the customer doesn't have an api and it is requested or cancel the apikeyorder if requested
        //            if (model.HasAPIKey)
        //            {
        //                if (!user.OrderId.HasValue)
        //                {
        //                    // place new apikey order api
        //                    var apikeyrequest = await LimeLightHelper.NewLimeLightOrderAsync(customer, CpaTickerConfiguration.LimeLightAPIKeyProductId);
        //                    user.OrderId = apikeyrequest.OrderId();
        //                    repo.EditUserProfile(user);
        //                }
        //            }
        //            else
        //            {
        //                //cancel the apikey subscription if present
        //                if (user.OrderId.HasValue)
        //                {
        //                    LimeLightHelper.CancelLimeLightOrderAsync(user.OrderId.Value);
        //                    user.OrderId = null;
        //                    repo.EditUserProfile(user);
        //                }
        //            }

        //            repo.EditCustomer(customer);

        //            // if we reach this point is because everything goes well so update all Apikeys orders associated with this customer
        //            // no more than 30 request / min

        //            var apiorders = repo.UserProfile()
        //                .Where(u => u.CustomerId == user.CustomerId)
        //                .Select(u => u.OrderId).Where(u => u.HasValue)
        //                .ToList();
        //            System.Threading.Tasks.Task.Factory.StartNew(() => UpdateAllUserApiKeyOrders(apiorders, customer, sc));

        //            return RedirectToAction("Index");

        //        }
        //    }
        //    catch (LimeLightException e)
        //    {
        //        ModelState.AddModelError("", e.Message);
        //    }
        //    return View(model);
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            repo.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult AddUser(int id = 0)
        {
            AddUserModel aum = new AddUserModel();
            return View(aum);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddUser(AddUserModel model, int[] aff = null)
        {
            model.HasAPIKey = Request["hasapi"] == "on";
            model.SendEmail = !string.IsNullOrEmpty(Request["send"]);
            model.SendUserEmail = !string.IsNullOrEmpty(Request["sendpass"]);
            if (ModelState.IsValid)
            {
                try
                {
                    using (var scope = new TransactionScope())
                    {
                        var customer = repo.GetCurrentCustomer();
                        var userprofile = new UserProfile();
                        userprofile.APIKey = Guid.NewGuid();
                        userprofile.UserName = model.UserName;
                        userprofile.Email = model.Email;

                        if (model.SelectedAffiliateId.HasValue)
                        {
                            userprofile.AffiliateId = model.SelectedAffiliateId;
                            userprofile.Permissions = CpaTickerConfiguration.DefaultAffiliateDynamicRestrictions;
                        }

                        if (model.HasAPIKey)
                        {
                            var order = repo.Orders().Single(o => o.CustomerId == customer.CustomerId);
                            var response = LimeLightHelper.NewLimeLightOrder(repo, customer, model.Email, CpaTickerConfiguration.LimeLightAPIKeyProductId
                                , Request.UserHostAddress
                                , order.OrderId);
                            userprofile.OrderId = response.OrderId();
                            if (model.SendEmail)
                            {
                                // send email
                                CPAHelper.SendAPIKeyMail(userprofile);
                            }
                        }

                        WebSecurity.CreateUserAndAccount(model.UserName, model.Password, propertyValues: new
                        {
                            CustomerId = customer.CustomerId,
                            APIKey = userprofile.APIKey,
                            OrderId = userprofile.OrderId,
                            AffiliateId = userprofile.AffiliateId,
                            Permissions = userprofile.Permissions,
                            Email = userprofile.Email,
                        });


                        Roles.AddUserToRole(model.UserName, userprofile.AffiliateId.HasValue ? "Affiliate" : "Administrator");

                        if (model.SendUserEmail)
                        {
                            // send username and password email
                            CPAHelper.SendUserOrPassMail(userprofile, model.Password);
                        }

                        scope.Complete();
                    }

                    var Userprofile = repo.UserProfile().SingleOrDefault(u => u.UserName == model.UserName);

                    try
                    {
                        for (int a = 0; a < aff.Length; a++)
                        {
                            UserAffiliate objUseraff = new UserAffiliate();
                            objUseraff.AffiliateId = aff[a];
                            objUseraff.CustomerId = Userprofile.CustomerId;
                            objUseraff.UserId = Userprofile.UserId;
                            db.UserAffiliate.Add(objUseraff);
                            if (a == aff.Length - 1)
                            { db.SaveChanges(); }
                        }
                    }
                    catch
                    {

                    }



                    return RedirectToAction("SetPermissions", new { id = Userprofile.UserId });

                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("Username", CPAHelper.ErrorCodeToString(e.StatusCode));
                }
                catch (LimeLightException e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return View(model);
        }

        public ActionResult SetPermissions(int id)
        {

            var user = repo.UserProfile().SingleOrDefault(u => u.UserId == id);
            if (user == null)
                throw new HttpException(404, "Not Found");

            ViewBag.Campaigns = from c in repo.Campaigns().Where(c => c.CustomerId == user.CustomerId && c.Status == Status.Active).AsEnumerable()
                                join hc in user.HiddenCampaigns on c.Id equals hc.CampaignId into uc
                                from hc in uc.DefaultIfEmpty()
                                select new SelectListItem
                                {
                                    Value = c.Id.ToString(),
                                    Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
                                    Selected = hc != null
                                };

            return View(user);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SetPermissions(int id, string[] views, string[] views1, int[] campaigns)
        {
            //UserProfile up = CPAHelper.GetCurrentUserProfile(id);
            //var up = repo.GetCurrentUser();
            var up = repo.UserProfile().SingleOrDefault(u => u.UserId == id);

            using (var scope = new TransactionScope())
            {
                //var hcampaigns = repo.UserHiddenCampaigns().Where(u => u.UserId == up.UserId);
                repo.RemoveUserHiddenCampaigns(up);
                //repo.SaveChanges();

                //up.HiddenCampaigns.Clear();
                if (campaigns != null)
                {
                    foreach (var item in campaigns)
                    {
                        var hc = new UserHiddenCampaign()
                        {
                            CampaignId = item,
                            UserId = up.UserId,
                        };
                        up.HiddenCampaigns.Add(hc);
                        //repo.AddUserHiddenCampaign(hc);
                    }
                }

                #region viewpermissions
                //// every time there is an post request to this page we must reset the permitions
                up.Permissions = up.AffiliateId != null ? CpaTickerConfiguration.DefaultAffiliateDynamicRestrictions : 0;
                up.Permissions1 = (up.AffiliateId != null) ? CpaTickerConfiguration.DefaultAffiliateDynamicRestrictionsCopy("ViewPermission") : 0;

                var assembly = System.Reflection.Assembly.Load("EnumAssembly");


                if (views != null)
                {
                    Type enumtype = assembly.GetType("ViewPermissiond");

                    foreach (var page in views)
                    {
                        System.Reflection.FieldInfo enumItem = enumtype.GetField(page);
                        long value = (long)enumItem.GetValue(enumtype);
                        up.Permissions = up.Permissions | value;
                    }
                }

                if (views1 != null)
                {
                    Type enumtype = assembly.GetType("ViewPermission");

                    foreach (var page in views1)
                    {
                        System.Reflection.FieldInfo enumItem = enumtype.GetField(page);
                        long value = (long)enumItem.GetValue(enumtype);
                        up.Permissions1 = up.Permissions1 | value;
                    }
                }
                #endregion

                repo.EditUserProfile(up);
                scope.Complete();
            }


            //db.UserProfiles.Attach(up);
            //DbEntityEntry<UserProfile> entry = db.Entry(up);
            //entry.Property(e => e.Permissions).IsModified = true;
            //entry.Property(e => e.Permissions1).IsModified = true;
            //entry.Property(e => e.HiddenCampaigns).IsModified = true;


            //db.SaveChanges();
            //db.Dispose();
            ViewBag.Message = "The permissions have been set! Go back to ";

            ViewBag.Campaigns = from c in repo.Campaigns().Where(c => c.CustomerId == up.CustomerId && c.Status == Status.Active).AsEnumerable()
                                join hc in up.HiddenCampaigns on c.Id equals hc.CampaignId into uc
                                from hc in uc.DefaultIfEmpty()
                                select new SelectListItem
                                {
                                    Value = c.Id.ToString(),
                                    Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
                                    Selected = hc != null
                                };

            return View(up);
        }

        //[AuthorizeUser(RequiredPermission = ViewPermission.addcustomerdomainSettings)]
        public ActionResult AddCustomerDomain()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AuthorizeUser(RequiredPermission = ViewPermission.AddDomain)]
        public ActionResult AddCustomerDomain(Domain dom)
        {
            if (ModelState.IsValid)
            {
                db.Domains.Add(dom);
                CustomerDomain cd = new CustomerDomain();
                db.SaveChanges();
                cd.DomainId = dom.Id;
                cd.CustomerId = this.CurrentCustomer.CustomerId;
                db.CustomerDomains.Add(cd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dom);
        }


        public ActionResult addEmployeeIp()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult addEmployeeIp(EmployeeIP eip)
        {
            if (ModelState.IsValid)
            {
                eip.CustomerId = repo.GetCurrentUser().CustomerId;
                repo.AddEmployeeIp(eip);
                return RedirectToAction("Index");
            }

            return View(eip);
        }

        public ActionResult ChangePwd()
        {
            return View(new ChangePwdModel());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangePwd(ChangePwdModel model)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.ChangePassword(WebSecurity.CurrentUserName, model.CurrentPassword, model.Password))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "The current password is not correct");
                }
            }
            return View(model);
        }

        public ActionResult ResetPwd(int id)
        {
            ViewBag.UserName = CPAHelper.GetCurrentUserProfile(id).UserName;

            return View(new ResetPwdModel());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ResetPwd(int id, ResetPwdModel model)
        {
            ViewBag.UserName = CPAHelper.GetCurrentUserProfile(id).UserName;
            if (ModelState.IsValid)
            {
                string token = WebSecurity.GeneratePasswordResetToken(ViewBag.UserName);
                WebSecurity.ResetPassword(token, model.Password);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult AddCustomField()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddCustomField(CustomField cf)
        {
            if (ModelState.IsValid)
            {
                cf.CustomerId = repo.GetCurrentCustomerId();
                repo.AddCustomField(cf);
                return RedirectToAction("index");
            }
            return View(cf);
        }

        public ActionResult EditCustomField(int id)
        {
            var cf = repo.FindCustomField(id);
            if (cf == null)
            {
                return HttpNotFound();
            }
            return View(cf);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditCustomField(CustomField cf)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cf).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cf);
        }

        public ActionResult admin_master_login(string id)
        {
            UserProfile userProfile = db.UserProfiles.FirstOrDefault(u => u.UserName == id);
            if (userProfile != null)
            {
                if (String.IsNullOrWhiteSpace(Convert.ToString(Session["username"])))
                { Session["username"] = repo.GetCurrentUser().UserName; }

                FormsAuthentication.SetAuthCookie(id, false);
                return RedirectToAction("Dashboard", "Home");
            }
            return null;
        }
        public ActionResult AddCustomTimeZone()
        {
            AddCustomTimeZone model = new AddCustomTimeZone();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddCustomTimeZone(AddCustomTimeZone _model)
        {
            CustomTimeZone model = new CustomTimeZone();
            model.DisplayID = _model.DisplayID;
            model.DisplayName = _model.DisplayName;
            model.dstEnd = _model.dstEnd;
            model.offset = _model.offsetHour * 60 + _model.offsetMinute;
            model.dstoffset = _model.dstoffsetHour * 60 + _model.dstoffsetMinute;
            model.dstStart = _model.dstStart;
            model.ID = _model.ID;
            model.IsdstSupport = _model.IsdstSupport;
            model.UserID = repo.GetCurrentUserId();
            var Timezone = repo.AddCustomTimezone(model);
            return RedirectToAction("Index");
        }
        public ActionResult EditCustomTimeZone(int id)
        {
            CustomTimeZone _model = repo.GetTimezoneById(id);
            AddCustomTimeZone model = new AddCustomTimeZone();
            model.DisplayID = _model.DisplayID;
            model.DisplayName = _model.DisplayName;
            model.dstEnd = _model.dstEnd;
            model.offsetHour = _model.offset / 60;
            model.offsetMinute = _model.offset - model.offsetHour * 60;
            model.dstoffsetHour = _model.dstoffset / 60;
            model.dstoffsetMinute = _model.dstoffset - model.dstoffsetHour * 60;
            model.dstStart = _model.dstStart;
            model.ID = _model.ID;
            model.IsdstSupport = _model.IsdstSupport;
            model.UserID = _model.UserID;
            return View(model);
        }
        [HttpPost]
        public ActionResult EditCustomTimeZone(AddCustomTimeZone _model)
        {
            CustomTimeZone model = new CustomTimeZone();
            model.ID = _model.ID;
            model.DisplayID = _model.DisplayID;
            model.DisplayName = _model.DisplayName;
            model.dstEnd = _model.dstEnd;
            model.offset = _model.offsetHour * 60 + _model.offsetMinute;
            model.dstoffset = _model.dstoffsetHour * 60 + _model.dstoffsetMinute;
            model.dstStart = _model.dstStart;
            model.IsdstSupport = _model.IsdstSupport;
            model.UserID = _model.UserID;
            var Timezone = repo.EditCustomTimezone(model);
            return RedirectToAction("Index");
        }
    }

    public class ChangePwdModel : ResetPwdModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
    }

    public class ResetPwdModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class EditUserVM
    {
        [Required]
        [Display(Name = "User name")]
        [RemoteWithServerSideAttribute("CheckUserNameForEdit", "Validation", "admin", AdditionalFields = "UserId", ErrorMessage = "This username is already taken.")]
        public string UserName { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "The e-mail adress isn't in a correct format")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [RemoteWithServerSideAttribute("CheckUserEmailForEdit", "Validation", "admin", AdditionalFields = "UserId", ErrorMessage = "This email is already taken.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        public bool HasAPIKey { get; set; }

        public bool SendEmail { get; set; }

        [Display(Name = "Affiliate")]
        public int? AffiliateId { get; set; }

        public int UserId { get; set; }
        public bool Hassendpassword { get; set; }

        public bool SendUserEmail { get; set; }
    }

    public class AddUserModel
    {
        private IEnumerable<UserAffiliateModel> _afflist;

        [Display(Name = "Select an Affiliate")]
        public IEnumerable<UserAffiliateModel> AffiliateList
        {
            get
            {
                if (_afflist == null)
                {

                    var repo = new EFCpatickerRepository();
                    _afflist = repo.GetCustomerActiveAffiliates().AsEnumerable().Select(a => new UserAffiliateModel
                    {
                        AffiliateId = a.AffiliateId,
                        Company = string.Format("{0} - {1}", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.AffiliateId);


                    //CpaTickerDb db = new CpaTickerDb();
                    //int customerid = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
                    //_afflist = db.Affiliates.Where(a => a.CustomerId == customerid).Select(a => new UserAffiliateModel 
                    //{ 
                    //    AffiliateId = a.AffiliateId, 
                    //    Company = string.Format("{0} - {1}", a.AffiliateId, a.Company)
                    //}).ToList();
                }
                return _afflist;
            }
        }

        [Required]
        [Display(Name = "User name")]
        [RemoteWithServerSideAttribute("CheckUserName", "Validation", "admin", ErrorMessage = "This username is already taken.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "The e-mail address isn't in a correct format")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [RemoteWithServerSideAttribute("CheckEmailExists", "Validation", "admin", ErrorMessage = "This email is already taken.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        public int? SelectedAffiliateId
        {
            get;
            set;
        }

        public bool HasAPIKey { get; set; }

        public bool SendEmail { get; set; }

        public string UserAffiliates { get; set; }

        public bool Hassendpassword { get; set; }

        public bool SendUserEmail { get; set; }
    }

    public class UserAffiliateModel
    {
        public int AffiliateId { get; set; }
        public string Company { get; set; }
    }

}