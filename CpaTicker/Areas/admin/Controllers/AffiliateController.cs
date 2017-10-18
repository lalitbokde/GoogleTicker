using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes;
using WebMatrix.WebData;
using System.Transactions;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Models;
using System.Data.Entity.SqlServer;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace CpaTicker.Areas.admin.Controllers
{
    [Authorize]
    public class AffiliateController : BaseController
    {
        private CpaTickerDb db = new CpaTickerDb();
        private ICpaTickerRepository repo;

        public AffiliateController()
        {
            this.repo = new EFCpatickerRepository();
        }

        //
        // GET: /Affiliate/

        public ActionResult Index()
        {
            UserProfile up = CPAHelper.GetCurrentUserProfile(WebSecurity.CurrentUserId);
            bool IsAdmin = Roles.IsUserInRole("Administrator");
            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);
            IEnumerable<Affiliate> afflist = null;

            if (IsAdmin)
            {
                afflist = db.Affiliates.Where(a => a.CustomerId == up.CustomerId);
            }
            else
            {
                afflist = db.Affiliates.Where(a => a.CustomerId == up.CustomerId && a.AffiliateId == up.AffiliateId);
            }

            var filter = AffiliateFilter.active_pending;
            try
            {
                filter = Request.QueryString.GetValue<AffiliateFilter>("filter");
            }
            catch { }
            var affiliates = afflist; // not hidden campaigns from the current customer

            switch (filter)
            {
                case AffiliateFilter.active_pending:
                    affiliates = affiliates.Where(c => c.Status == AffiliateStatus.Active || c.Status == AffiliateStatus.Pending);
                    break;
                case AffiliateFilter.active:
                    affiliates = affiliates.Where(c => c.Status == AffiliateStatus.Active);
                    break;

                case AffiliateFilter.pending:
                    affiliates = affiliates.Where(c => c.Status == AffiliateStatus.Pending);
                    break;

                case AffiliateFilter.rejected:
                    affiliates = affiliates.Where(c => c.Status == AffiliateStatus.Rejected);
                    break;

                case AffiliateFilter.blocked:
                    affiliates = affiliates.Where(c => c.Status == AffiliateStatus.Blocked);
                    break;

                case AffiliateFilter.deleted:
                    affiliates = affiliates.Where(c => c.Status == AffiliateStatus.Deleted);
                    break;

                case AffiliateFilter.all: // show hidden campaigns here marked as inactived
                    ViewBag.Useraffiliates = affiliates; // save user allowed campaigns
                    affiliates = repo.Affiliates().Where(c => c.CustomerId == up.CustomerId); // bring all customer campaigns                    
                    break;
            }


            ViewBag.filter = filter;

            ViewBag.IsAdmin = IsAdmin;
            return View(affiliates);
        }

        //
        // GET: /Affiliate/Details/5
        [ValidateInputAttribute(false)]
        public ActionResult Details(int id)
        {
            int customerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
            Affiliate affiliate = db.Affiliates.Single(a => a.CustomerId == customerId && a.AffiliateId == id);
            ViewBag.AffiliateId = id;
            ViewBag.Company = affiliate.Company;
            ViewBag.Address1 = affiliate.Address1;
            ViewBag.Address2 = affiliate.Address2;
            ViewBag.Phone = affiliate.Phone;
            ViewBag.Status = affiliate.Status;
            //var cpvm = from cp in db.ConversionPixels
            //           from c in db.Campaigns
            //           where cp.CustomerId == customerId && cp.AffiliateId == id && c.CustomerId == customerId && c.CampaignId == cp.CampaignId
            //           select new { cp.CampaignId, c.CampaignName, cp.TrackingType, cp.PixelCode };
            List<ConversionPixelViewModel> ConversionPixels = new List<ConversionPixelViewModel>();
            //foreach (var vm in cpvm)
            //{
            //    ConversionPixelViewModel c = new ConversionPixelViewModel();
            //    c.CampaignId = vm.CampaignId;
            //    c.CampaignName = vm.CampaignName;
            //    c.TrackingType = vm.TrackingType;
            //    c.PixelCode = vm.PixelCode;
            //    ConversionPixels.Add(c);
            //}
            ViewBag.ConversionPixels = ConversionPixels;
            ViewBag.CampaignId = db.Campaigns.Where(a => a.CustomerId == customerId).Select(a => new SelectListItem { Text = a.CampaignName, Value = SqlFunctions.StringConvert((decimal)a.CampaignId) });
            if (affiliate == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Details(ConversionPixel pixel)
        {
            int customerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
            int affiliateId = int.Parse(Request.Form["AffiliateId"]);

            if (ModelState.IsValid)
            {
                if (pixel == null)
                    pixel = new ConversionPixel();

                using (TransactionScope scope = new TransactionScope())
                {
                    pixel.AffiliateId = affiliateId;
                    //pixel.CustomerId = customerId;
                    //pixel.CampaignId = int.Parse(Request.Form["CampaignId"].Replace(" ", ""));
                    pixel.TrackingType = pixel.TrackingType;
                    pixel.PixelCode = Server.HtmlEncode(Request.Unvalidated.Form["PixelCode"]);
                    db.ConversionPixels.Add(pixel);
                    db.SaveChanges();
                    scope.Complete();
                }
                return RedirectToAction("Details", new { pseudoid = pixel.AffiliateId });

            }
            Affiliate affiliate = db.Affiliates.Single(a => a.CustomerId == customerId && a.AffiliateId == affiliateId);
            ViewBag.AffiliateId = affiliateId;
            ViewBag.Company = affiliate.Company;
            ViewBag.Address1 = affiliate.Address1;
            ViewBag.Address2 = affiliate.Address2;
            ViewBag.Phone = affiliate.Phone;
            ViewBag.Status = affiliate.Status;
            //var cpvm = from cp in db.ConversionPixels
            //           from c in db.Campaigns
            //           where cp.CustomerId == customerId && cp.AffiliateId == affiliateId && c.CustomerId == customerId && c.CampaignId == cp.CampaignId
            //           select new { cp.Id, cp.CustomerId, cp.CampaignId, c.CampaignName, cp.TrackingType, cp.PixelCode };
            List<ConversionPixelViewModel> ConversionPixels = new List<ConversionPixelViewModel>();
            //foreach (var vm in cpvm)
            //{
            //    ConversionPixelViewModel c = new ConversionPixelViewModel();
            //    c.CampaignId = vm.CampaignId;
            //    c.CampaignName = vm.CampaignName;
            //    c.TrackingType = vm.TrackingType;
            //    c.PixelCode = vm.PixelCode;
            //    ConversionPixels.Add(c);
            //}
            ViewBag.ConversionPixels = ConversionPixels;
            ViewBag.CampaignId = db.Campaigns.Where(a => a.CustomerId == customerId).Select(a => new SelectListItem { Text = a.CampaignName, Value = SqlFunctions.StringConvert((decimal)a.CampaignId) });
            return View(pixel);
        }

        //
        // GET: /Affiliate/Create

        public ActionResult Create()
        {
            ViewBag.Countries = repo.GetCountries();
            ViewBag.States = repo.GetCountryStates(228); // usa
            return View();
        }

        //
        // POST: /Affiliate/Create

        [HttpPost]
        public ActionResult Create(Affiliate affiliate)
        {
            if (ModelState.IsValid)
            {
                affiliate.CustomerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
                using (TransactionScope scope = new TransactionScope())
                {
                    int maxAffiliateId = db.Affiliates.Where(a => a.CustomerId == affiliate.CustomerId && a.AffiliateId < 9000)
                        .Max(a => (int?)a.AffiliateId) ?? 1000;
                    affiliate.AffiliateId = maxAffiliateId + 1;
                    db.Affiliates.Add(affiliate);
                    db.SaveChanges();
                    scope.Complete();
                }
                return RedirectToAction("Index");
            }

            return View(affiliate);
        }

        //
        // GET: /Affiliate/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var affiliate = repo.GetAffiliate(id);
            if (affiliate == null)
            {
                return HttpNotFound();
            }
            ModelState.Clear(); // to not bring confilct with the hiddenfor parameter Id
            ViewBag.Countries = repo.GetCountries();
            ViewBag.States = repo.GetCountryStates(affiliate.CountryId.Value);
            return View(affiliate);
        }

        //
        // POST: /Affiliate/Edit/5

        [HttpPost]
        public ActionResult Edit(Affiliate affiliate)
        {
            if (ModelState.IsValid)
            {
                if (repo.SetConversionPixelBlock(affiliate))
                {
                    db.Entry(affiliate).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View(affiliate);
        }

        //
        // GET: /Affiliate/Delete/5

        //public ActionResult Delete(int pseudoid = 0)
        //{
        //    int customerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);

        //    Affiliate affiliate = db.Affiliates.Single(a => a.CustomerId == customerId && a.AffiliateId == pseudoid);
        //    if (affiliate == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(affiliate);
        //}

        public ActionResult AdjustStats(DateTime? date, int type = 1, int campaignid = 0, int urlid = 1, int affiliateid = 0,
            int bannerid = 0, int mode = 0, int qty = 0)
        {
            int customerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
            AdjustModel modelresult = new AdjustModel();

            modelresult.Text = "...";
            DateTime comparedate = (DateTime)date;


            comparedate = comparedate.Date;
            DateTime todate = comparedate.AddDays(1).AddMilliseconds(-1);

            Banner banner = db.Banners.Where(b => b.BannerId == bannerid && b.CustomerId == customerId && b.CampaignId == campaignid).SingleOrDefault();
            Campaign campaign = CPAHelper.GetCampaignById(campaignid, customerId);

            var page = db.PAGEs.FirstOrDefault(p => p.CustomerID == customerId);
            var pageId = 0;

           

            if (page != null)
            {
                pageId = page.Id;
            }

            var device = db.DeviceInfo.FirstOrDefault();
            var userAgentId = 0L;

            if (device != null)
            {
                userAgentId = device.Id;
            }


            // get the url by preview id
            var url = repo.GetCampaignURLs(campaign.Id).Single(u => u.PreviewId == urlid);

            switch (type)
            {
                // click
                // get the defaulturl


                case 1:

                    var clicks = new List<Click>();
                    for (int i = 0; i < qty; i++)
                    {
                        string transactionId = Guid.NewGuid().ToString();
                        // create click and set it
                        Click click = new Click();
                        click.AffiliateId = affiliateid;
                        click.CustomerId = customerId;
                        click.CampaignId = campaignid;
                        click.ClickDate = comparedate.Add(DateTime.Now.TimeOfDay).ToUniversalTime();
                        click.IPAddress = Request.UserHostAddress;
                        click.UserAgent = Request.UserAgent;
                        click.Referrer = (Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString());
                        click.TransactionId = transactionId;
                        click.Status = ClickStatus.Active;
                        click.BannerId = bannerid;

                        click.UserAgentId = userAgentId;
                        click.URLId = pageId;

                        click.Cost = repo.GetCost(url.PayoutType, url.Payout, url.PayoutPercent, ActionType.Click);
                        click.Revenue = repo.GetRevenue(url.RevenueType, url.Revenue, url.RevenuePercent, ActionType.Click);
                        click.URLPreviewId = url.PreviewId;

                        if (mode != 0) // mode == 0 equals to Add, mode == 1 equals to Remove
                        {
                            click.Cost *= -1;
                            click.Revenue *= -1;
                        }
                        clicks.Add(click);
                        //db.Clicks.(click);
                        //repo.AddClick(click);
                    }
                    db.Clicks.AddRange(clicks);
                    db.SaveChanges();
                    modelresult.Text = qty + "  Clicks was Created...";
                    break;
                // conversion
                case 2:
                    //if (mode == 0)
                    //{

                    // this can be placed better is a smell-smell code
                    // get the default conversion which is the first
                    var action = repo.DefaultAction(campaign);

                    int count = 0;
                    var clicklist = db.Clicks.Where(cl => cl.AffiliateId == affiliateid && cl.CustomerId == customerId && cl.CampaignId == campaignid && cl.ClickDate < todate &&  cl.Status == ClickStatus.Active).OrderByDescending(cl => cl.ClickDate).ToList();
                    var conversions = new List<Conversion>();
                    foreach (Click click in clicklist)
                    {
                        if (count < qty)
                        {
                            string transactionId = click.TransactionId;
                            int conversion = db.Conversions.Count(co => co.TransactionId == transactionId);
                            if (conversion == 0)
                            {
                                //no conversion, so let's add one
                                Conversion co = new Conversion();
                                co.CustomerId = customerId;
                                co.ClickId = click.ClickId;
                                co.CampaignId = campaignid;
                                co.AffiliateId = click.AffiliateId;
                                co.ConversionDate = comparedate.Add(DateTime.Now.TimeOfDay).ToUniversalTime();
                                co.IPAddress = Request.UserHostAddress;
                                co.Referrer = (Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString());
                                co.TransactionId = transactionId;
                                co.UserAgent = Request.UserAgent;
                                co.BannerId = click.BannerId;
                                co.Type = ConversionType.Sale;
                                co.UserAgentId = userAgentId;
                                co.Cost = repo.GetCost(action.PayoutType, action.Payout, action.PayoutPercent, ActionType.Conversion);//, Request.QueryString["saleamount"]);
                                co.Revenue = repo.GetRevenue(action.RevenueType, action.Revenue, action.RevenuePercent, ActionType.Conversion);//, Request.QueryString["saleamount"]);


                                if (mode != 0)
                                {
                                    co.Cost *= -1;
                                    co.Revenue *= -1;
                                }

                                co.Status = 1;
                                conversions.Add(co);
                                //db.Conversions.Add(co);

                                count++;

                            }
                        }
                        else
                            break;
                    }
                    db.Conversions.AddRange(conversions);
                    db.SaveChanges();
                    if (mode == 0)
                    {
                        if (count != qty)
                            modelresult.Text = count + "  Conversions was Created. " + count + " Cicks exist";
                        else
                            modelresult.Text = count + "  Conversions was Created. ";
                    }
                    else
                    {
                        if (count != qty)
                            modelresult.Text = count + "  Conversions was Removed. " + count + " Cicks exist";
                        else
                            modelresult.Text = count + "  Conversions was Removed. ";
                    }

                    //}
                    //else 
                    //{
                    //    int count = 0;
                    //    foreach (Conversion con in db.Conversions.Where(co=> co.AffiliateId == affiliateid && co.CustomerId == customerId && co.CampaignId == campaignid && co.ConversionDate >= comparedate && co.ConversionDate <= todate))
                    //    {
                    //        if (count < qty)
                    //        {
                    //            db.Conversions.Remove(con);
                    //            count++;

                    //        }
                    //        else
                    //            break;
                    //    }
                    //    db.SaveChanges();
                    //    modelresult.Text = count + "  Conversions was Deleted";
                    //}
                    break;


                case 3:
                    //if (mode == 0)
                    //{

                    if (banner != null)
                    {
                        var impressions = new List<Impression>();
                        for (int i = 0; i < qty; i++)
                        {

                            // create impression
                            Impression imp = new Impression();
                            imp.AffiliateId = affiliateid;
                            imp.CustomerId = customerId;
                            imp.CampaignId = campaignid;
                            imp.ImpressionDate = comparedate.Add(DateTime.Now.TimeOfDay).ToUniversalTime();
                            imp.IPAddress = Request.UserHostAddress;
                            imp.UserAgent = Request.UserAgent;
                            imp.Referrer = (Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString());
                            imp.BannerId = bannerid;

                            imp.URLPreviewId = url.PreviewId;
                            imp.URLId = pageId;
                            

                            imp.Cost = repo.GetCost(url.PayoutType, url.Payout, url.PayoutPercent, ActionType.Impression);
                            imp.Revenue = repo.GetRevenue(url.RevenueType, url.Revenue, url.RevenuePercent, ActionType.Impression);

                            if (mode != 0)
                            {
                                imp.Cost *= -1;
                                imp.Revenue *= -1;
                            }
                            impressions.Add(imp);
                            //db.Impressions.Add(imp);
                            

                        }
                        db.Impressions.AddRange(impressions);
                        db.SaveChanges();
                        modelresult.Text = qty + "  Impressions was Created...";
                    }

                    else
                        modelresult.Text = "No Impressions was Created... No banner found";





                    break;

            }




            return View(modelresult);


        }

        public ActionResult Adjust()
        {
            //int customerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
            //AdjustStatsModel view = new AdjustStatsModel();
            //Campaign campaign = db.Campaigns.Where(a => a.CustomerId == customerId).First();
            //int campaignid = 0;
            //if (campaign != null)
            //    campaignid = campaign.CampaignId;

            //ViewBag.CampaignId = db.Campaigns.Where(a => a.CustomerId == customerId).Select(a => new SelectListItem { Value = SqlFunctions.StringConvert((decimal)a.CampaignId).TrimStart(), Text = String.Format("{0} - {1}", SqlFunctions.StringConvert((decimal)a.CampaignId).TrimStart(),  a.CampaignName) });
            //ViewBag.AffiliateId = db.Affiliates.Where(a => a.CustomerId == customerId).Select(a => new SelectListItem { Value = SqlFunctions.StringConvert((decimal)a.AffiliateId).TrimStart(), Text = String.Format("{0} - {1}", SqlFunctions.StringConvert((decimal)a.AffiliateId).TrimStart(), a.Company) });
            //ViewBag.BannerId = db.Banners.Where(a => a.CustomerId == customerId && a.CampaignId == campaignid).Select(a => new SelectListItem { Text = SqlFunctions.StringConvert((decimal)a.BannerId).TrimStart(), Value = SqlFunctions.StringConvert((decimal)a.BannerId).TrimStart() });
            ////view.
            //return View(view);

            var user = repo.GetCurrentUser();

            var campaigns = repo.GetUserCampaigns(user).Where(c => c.Status == Status.Active);

            ViewBag.CampaignId = campaigns.AsEnumerable().Select(c => new SelectListItem
            {
                Value = c.CampaignId.ToString(),
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
            });

            ViewBag.AffiliateId = repo.GetCustomerActiveAffiliates(user.CustomerId).AsEnumerable().Select(a => new SelectListItem
            {
                Text = String.Format("{0} - {1}", a.AffiliateId, a.Company),
                Value = a.AffiliateId.ToString()
            }).OrderBy(u => u.Value);

            var firstcampaign = campaigns.First();

            ViewBag.BannerId = repo.GetBannersByCampaign(firstcampaign.CampaignId, user.CustomerId).AsEnumerable().Select(b => new SelectListItem
            {
                Text = b.BannerId.ToString(),
                Value = b.BannerId.ToString()
            });

            ViewBag.URLs = repo.GetCampaignURLs(firstcampaign.Id).AsEnumerable().Select(b => new SelectListItem
            {
                Text = b.PreviewId.ToString(),
                Value = b.PreviewId.ToString()
            });

            return View();

        }

        public ActionResult Users(int id)
        {
            var customerid = repo.GetCurrentCustomerId();
            var affiliate = repo.GetAffiliate(id, customerid);
            if (affiliate == null)
            {
                return HttpNotFound();
            }

            return View(repo.AffiliateUsers(id, customerid));
        }

        public ActionResult AddUser(int id = 0)
        {
            var customerid = repo.GetCurrentCustomerId();
            var affiliate = repo.GetAffiliate(id, customerid);
            if (affiliate == null)
            {
                return HttpNotFound();
            }

            AddUserModel aum = new AddUserModel();
            return View(aum);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddUser(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int customerid = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, propertyValues: new { CustomerId = customerid });
                    bool isaffiliate = model.SelectedAffiliateId > 0;
                    Roles.AddUserToRole(model.UserName, isaffiliate ? "Affiliate" : "Administrator");

                    var userprofile = db.UserProfiles.Where(up => up.UserName == model.UserName).Single();
                    if (isaffiliate)
                    {
                        userprofile.AffiliateId = model.SelectedAffiliateId;
                        userprofile.Permissions = CpaTickerConfiguration.DefaultAffiliateDynamicRestrictions;
                    }
                    else
                    {
                        userprofile.Permissions = 0;
                    }
                    userprofile.Email = model.Email;
                    db.SaveChanges();
                    var Userprofile = repo.UserProfile().SingleOrDefault(u => u.UserName == model.UserName);
                    try
                    {
                        if (isaffiliate)
                        {
                            UserAffiliate objUseraff = new UserAffiliate();
                            objUseraff.AffiliateId = (int)model.SelectedAffiliateId;
                            objUseraff.CustomerId = Userprofile.CustomerId;
                            objUseraff.UserId = Userprofile.UserId;
                            db.UserAffiliate.Add(objUseraff);
                            db.SaveChanges();
                        }
                    }
                    catch
                    {

                    }
                    return RedirectToAction("SetPermissions", "Settings", new { id = Userprofile.UserId });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("Username", CPAHelper.ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }

        //
        // POST: /Affiliate/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    int customerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);

        //    Affiliate affiliate = db.Affiliates.Single(a => a.CustomerId == customerId && a.AffiliateId == id);
        //    db.Affiliates.Remove(affiliate);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            this.repo.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult OverridePayout(int id)
        {
            //var action = repo.FindAction(id);

            //if (action == null || repo.IsHidden(action.Id))
            //{
            //    return HttpNotFound();
            //}
            ViewBag.AffiliateID = id;
            int CustomerId = repo.GetCurrentCustomerId();
            //Affiliate affiliate = db.Affiliates.Single(a => a.CustomerId == customerId && a.AffiliateId == id);
            //var campaign = repo(id);
            //if (campaign == null || repo.IsHidden(campaign))
            //{
            //    return HttpNotFound();
            //}
            //  ViewBag.Campaign = campaign;
            var Affiliateoverride = repo.GetAffiliateOverride(id, CustomerId);
            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);
            return View(Affiliateoverride.AsQueryable());

        }
        public ActionResult CreateOverridePayout(int id)
        {
            //var action = repo.FindAction(id);
            var customerID = repo.GetCurrentCustomer().CustomerId;
            //if (action == null || repo.IsHidden(action.Campaign))
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.Action = action;
            //var campaign = repo.FindCampaign(id);
            //if (campaign == null || repo.IsHidden(campaign))
            //{
            //    return HttpNotFound();
            //}
            ViewBag.AffiliateID = id;
            //ViewBag.AffiliateID = new SelectList(repo.GetAffiliateByCustomer(customerID), "AffiliateID", "Company");
            //ViewBag.ActionID = new SelectList(campaign.Actions, "Id", "Name");

            return View();
        }

        public ActionResult EditOverridePayout(int id)
        {
            var AffiliateOverride = repo.FindAffiliateOverride(id);
            //var action = repo.FindAction(overridepayout.ActionID);
            var customer = repo.GetCurrentCustomer();
            //if (action == null || repo.IsHidden(action.Campaign))
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.Action = action;
            ViewBag.AffiliateId = AffiliateOverride.AffiliateID;
            ViewBag.OverridID = id;
            ViewBag.DDLPayout = AffiliateOverride.PayoutType;
            ViewBag.DDLRevenue = AffiliateOverride.RevenueType;
            return View(AffiliateOverride);
        }

        public ActionResult AddAffiliateOverride(string AffiliateID, string _PayoutType, string Payout, string PayoutPercent, string _RevenueType, string Revenue, string RevenuePercent)
        {
            int CustomerId = repo.GetCurrentCustomerId();
            var affiliateId = repo.AddOverrideAffiliate(Convert.ToInt32(AffiliateID), CustomerId, _PayoutType, Payout, PayoutPercent, _RevenueType, Revenue, RevenuePercent);

            return Json(affiliateId);
            //return RedirectToAction("OverridePayout", new { id = Convert.ToInt32(ActionID) });
        }

        public ActionResult UpdateAffiliateOverride(string OverrideId, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent, string _RevenueType, string Revenue, string RevenuePercent)
        {
            int CustomerId = repo.GetCurrentCustomerId();
            var affiliateId = repo.UpdateOverrideAffiliate(Convert.ToInt32(OverrideId), Convert.ToInt32(AffiliateID), CustomerId, _PayoutType, Payout, PayoutPercent, _RevenueType, Revenue, RevenuePercent);

            return Json(affiliateId);
            //return RedirectToAction("OverridePayout", new { id = Convert.ToInt32(ActionID) });
        }
    }
}