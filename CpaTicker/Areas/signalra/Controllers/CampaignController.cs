using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;

namespace CpaTicker.Areas.signalra.Controllers
{
    [Authorize]
    public class CampaignController : BaseController
    {
        private CpaTickerDb db = new CpaTickerDb();
        private ICpaTickerRepository repo;

        public CampaignController()
        {
            this.repo = new EFCpatickerRepository();
        }

        //
        // GET: /Campaign/

        public ActionResult Index()
        {
            //var filter = Request.QueryString.HasKeys() ? 
            //    EnumHelper<CampaignFilter>.Parse(Request.QueryString["filter"]) : 
            //    CampaignFilter.active_pending;

            var filter = CampaignFilter.active_pending;
            try
            {
                filter = Request.QueryString.GetValue<CampaignFilter>("filter");
            }
            catch { }
            //var campaigns = repo.GetCustomerCampaigns();
            var customerid = repo.GetCurrentUser().CustomerId;
            var campaigns = repo.GetUserCampaigns(); // not hidden campaigns from the current customer

            switch (filter)
            {
                case CampaignFilter.active_pending:
                    campaigns = campaigns.Where(c => c.Status == Status.Active || c.Status == Status.Pending);
                    break;
                case CampaignFilter.active:
                    campaigns = campaigns.Where(c => c.Status == Status.Active);
                    break;

                case CampaignFilter.pending:
                    campaigns = campaigns.Where(c => c.Status == Status.Pending);
                    break;

                case CampaignFilter.deleted:
                    campaigns = campaigns.Where(c => c.Status == Status.Deleted);
                    break;
                case CampaignFilter.paused:
                    campaigns = campaigns.Where(c => c.Status == Status.Paused);
                    break;
                case CampaignFilter.all: // show hidden campaigns here marked as inactived
                    ViewBag.UserCampaigns = campaigns; // save user allowed campaigns
                    campaigns = repo.Campaigns().Where(c => c.CustomerId == customerid); // bring all customer campaigns                    
                    break;
            }


            ViewBag.filter = filter;
            return View(campaigns);
        }

        //
        // GET: /Campaign/Details/5

        public ActionResult Details(int id, int? id2) // campaignid, urlid
        {
            var user = repo.GetCurrentUser();
            var campaign = repo.GetCampaignById(id, user.CustomerId);

            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }

            if (!user.AffiliateId.HasValue) // is admin
                //get all the affiliates associated with the customerid
                ViewBag.Affiliates = repo.GetCustomerActiveAffiliates(user.CustomerId).AsEnumerable().Select(
                    a => new SelectListItem
                    {
                        Text = String.Format("{0} - {1}", a.AffiliateId, a.Company),
                        Value = a.AffiliateId.ToString()
                    });
            else
                ViewBag.Affiliates = new List<Affiliate>() { repo.GetAffiliate(user.AffiliateId.Value, user.CustomerId) }.Select(
                    a => new SelectListItem { Text = String.Format("{0} - {1}", a.AffiliateId, a.Company), Value = a.AffiliateId.ToString() });

            ViewBag.Banners = repo.GetBannersByCampaign(campaign.CampaignId, user.CustomerId).AsEnumerable().Select(a => new SelectListItem { Text = a.BannerId.ToString(), Value = a.BannerId.ToString() });

            var urls = repo.GetCampaignURLs(campaign.Id);
            ViewBag.URLs = urls; //.AsEnumerable().Select(a => new SelectListItem { Value = a.ActionId.ToString(), Text = a.ActionId.ToString() });
            ViewBag.SelectedURLId = id2;
            ViewBag.MultipleURLs = urls.Count() > 1;

            var cdlist = repo.GetCustomerDomains(user.CustomerId).ToList();
            cdlist.Add(repo.GetDefaultDomain(user.CustomerId));

            ViewBag.CustomerDomains = cdlist.Select(d => new SelectListItem { Text = d.DomainName, Value = d.Id.ToString() });

            // set a viewbag with the set up custom field - values for this campaign
            ViewBag.CustomFields = repo.GetCampaignCustomFieldValue(campaign.Id, user.CustomerId);

            return View(campaign);
        }

        //
        // GET: /Campaign/Create

        //[AuthorizeUser(RequiredPermission = ViewPermission.AddCampaign)]
        public ActionResult Create()
        {
            ViewBag.Countries = repo.GetIP2Countries();
            ViewBag.ParentURLId = repo.GetURLs().AsEnumerable().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = string.Format("{0} - {1} - {2}", a.Campaign.CampaignId, a.Campaign.CampaignName, a.PreviewId)
            });
            return View();
        }

        //
        // POST: /Campaign/Create

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AuthorizeUser(RequiredPermission = ViewPermission.AddCampaign)]
        public ActionResult Create(CampaignCreateVM model, string[] ct)
        {
            if (ModelState.IsValid)
            {
                model.Campaign.CustomerId = repo.GetCurrentCustomerId();

                model.DefaultPage.PreviewId = 1;
                model.DefaultPage.Name = "default";
                model.DefaultPage.Status = PageStatus.Active;
                model.DefaultPage.CustomerID = model.Campaign.CustomerId;
                //model.Campaign.Urls = new List<URL> { model.DefaultUrl };

                // when you create a campaign, it should just create a default page, not a url.
                repo.AddPAGE(model.DefaultPage);

                model.DefaultAction.Name = "default";

                model.DefaultAction.Revenue = model.DefaultPage.Revenue;
                model.DefaultAction.RevenuePercent = model.DefaultPage.RevenuePercent;
                model.DefaultAction.RevenueType = model.DefaultPage.RevenueType;

                model.DefaultAction.Payout = model.DefaultPage.Payout;
                model.DefaultAction.PayoutPercent = model.DefaultPage.PayoutPercent;
                model.DefaultAction.PayoutType = model.DefaultPage.PayoutType;


                model.Campaign.Actions = new List<CpaTicker.Areas.admin.Classes.Action> { model.DefaultAction };

                int maxCampaignId = repo.Campaigns().Where(c => c.CustomerId == model.Campaign.CustomerId).Max(c => (int?)c.CampaignId) ?? 100;
                model.Campaign.CampaignId = maxCampaignId + 1;

                // add the custom fields values
                var cfields = repo.GetCustomFields(model.Campaign.CustomerId);
                model.Campaign.CustomFieldValues = new List<CustomFieldValue>();
                foreach (var item in cfields)
                {
                    string value = Request["cfield-" + item.CustomFieldId];
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        model.Campaign.CustomFieldValues.Add(new CustomFieldValue { CustomFieldId = item.CustomFieldId, Value = value });
                    }
                }

                // add the geotargetcountries
                // add the countries
                model.Campaign.Enforce = Request["geoenforce"] == "on";
                if (ct != null)
                {
                    model.Campaign.Countries = new List<CampaignCountry>();
                    foreach (var item in ct)
                    {
                        model.Campaign.Countries.Add(new CampaignCountry { Code = item });
                    }
                }

                // add the campaign
                repo.AddCampaign(model.Campaign);
                return RedirectToAction("Index");
            }
            ViewBag.ParentURLId = repo.GetURLs().AsEnumerable().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = string.Format("{0} - {1} - {2}", a.Campaign.CampaignId, a.Campaign.CampaignName, a.PreviewId)
            });
            ViewBag.Countries = repo.GetIP2Countries();
            return View(model);

            //if (ModelState.IsValid)
            //{
            //    campaign.CustomerId = repo.GetCurrentCustomerId();
            //    using (TransactionScope scope = new TransactionScope())
            //    {
            //        //int maxCampaignId = repo.GetCustomerCampaigns(campaign.CustomerId).Max(c => (int?)c.CampaignId) ?? 100;

            //        campaign.CampaignId = maxCampaignId + 1;
            //        campaign.Enforce = Request["geoenforce"] == "on";


            //        repo.AddCampaign(campaign);

            //        // add the custom fields values
            //        var cfields = repo.GetCustomFields(campaign.CustomerId);
            //        foreach (var item in cfields)
            //        {
            //            string value = Request["cfield-" + item.CustomFieldId];
            //            if (!string.IsNullOrWhiteSpace(value))
            //            {
            //                repo.AddCustomFieldValue(new CustomFieldValue 
            //                {
            //                    CustomFieldId = item.CustomFieldId, 
            //                    CampaignId = campaign.Id,
            //                    Value = value
            //                });
            //            }
            //        }

            //        // add the countries
            //        if (ct != null)
            //        {
            //            foreach (var item in ct)
            //            {
            //                repo.AddCampaignCountry(new CampaignCountry
            //                {
            //                    CampaignId = campaign.Id,
            //                    Code = item
            //                });
            //            }
            //        }

            //        // add the default url
            //        repo.AddURL(repo.GetDefaultUrl(campaign));

            //        // add the default action
            //        repo.AddAction(repo.GetDefaultAction(campaign));

            //        repo.SaveChanges();
            //        scope.Complete();
            //    }

            //    return RedirectToAction("Index");
            //}
            //ViewBag.Countries = repo.GetIP2Countries();
            //return View(campaign);
        }

        //
        // GET: /Campaign/Edit/5
        //[AuthorizeUserAttribute(RequiredPermission = ViewPermission.EditCampaign)]
        public ActionResult Edit(int id = 0)
        {
            var campaign = repo.GetCampaignById(id);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }

            ModelState.Clear();
            ViewBag.Countries = repo.GetCampaignCountries(campaign.Id);
            return View(campaign);
        }

        //
        // POST: /Campaign/Edit/5

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AuthorizeUserAttribute(RequiredPermission = ViewPermission.EditCampaign)]
        public ActionResult Edit(Campaign model, string[] ct)
        {
            model.Enforce = Request["geoenforce"] == "on";

            var campaign = repo.FindCampaign(model.Id);

            campaign.Enforce = model.Enforce;
            campaign.Description = model.Description;
            campaign.Status = model.Status;
            campaign.ExpirationDate = model.ExpirationDate;
            campaign.CampaignName = model.CampaignName;

            // campaign countries
            campaign.Countries.Clear();
            if (ct != null)
            {
                foreach (var item in ct)
                {
                    campaign.Countries.Add(new CampaignCountry { Code = item });
                }
            }

            // campaign custom fields
            campaign.CustomFieldValues.Clear();
            var cfields = repo.GetCustomFields(campaign.CustomerId);
            foreach (var item in cfields)
            {
                string value = Request["cfield-" + item.CustomFieldId];

                if (!string.IsNullOrWhiteSpace(value))
                {
                    // add the custom field value
                    campaign.CustomFieldValues.Add(new CustomFieldValue { Value = value, CustomFieldId = item.CustomFieldId });
                }

            }

            if (ModelState.IsValid)
            {
                repo.EditCampaign(campaign);
                return RedirectToAction("Index");
            }


            //campaign.Enforce = Request["geoenforce"] == "on";
            //if (ModelState.IsValid)
            //{
            //    using (TransactionScope scope = new TransactionScope())
            //    {
            //        db.Entry(campaign).State = System.Data.Entity.EntityState.Modified;

            //        // edit the custom fields
            //        var cfields = repo.GetCustomFields(campaign.CustomerId);
            //        foreach (var item in cfields)
            //        {
            //            var cfv = db.CustomFieldValues.Find(item.CustomFieldId, campaign.Id);
            //            string value = Request["cfield-" + item.CustomFieldId];

            //            if (string.IsNullOrWhiteSpace(value))
            //            {
            //                // delete if exists
            //                if (cfv != null)
            //                {
            //                    db.CustomFieldValues.Remove(cfv);
            //                }
            //            }
            //            else
            //            {
            //                // edit if exits or added otherwise
            //                if (cfv != null)
            //                {
            //                    cfv.Value = value;
            //                    db.Entry(cfv).State = System.Data.Entity.EntityState.Modified;
            //                }
            //                else
            //                {
            //                    cfv = new CustomFieldValue
            //                    {
            //                        CampaignId = campaign.Id,
            //                        CustomFieldId = item.CustomFieldId,
            //                        Value = value
            //                    };
            //                    db.CustomFieldValues.Add(cfv);
            //                }

            //            }
            //        }

            //        // edit campaign-countries
            //        db.CampaignCountries.RemoveRange(db.CampaignCountries.Where(c => c.CampaignId == campaign.Id));
            //        if (ct != null)
            //        {
            //            foreach (var item in ct)
            //            {
            //                db.CampaignCountries.Add(new CampaignCountry
            //                {
            //                    CampaignId = campaign.Id,
            //                    Code = item
            //                });
            //            }
            //        }

            //        // record all changes
            //        db.SaveChanges(); 

            //        // after this point all changes must be recorded so edit the default url for this campaign


            //        //var default_url = repo.DefaultURL(campaign);

            //        scope.Complete();
            //    }
            //    return RedirectToAction("Index");
            //}
            ViewBag.Countries = repo.GetCampaignCountries(campaign.Id);
            return View(campaign);
        }

        /* Delete Campaign

        public ActionResult Delete(int id = 0)
        {

            int customerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);

            Campaign campaign = db.Campaigns.Single(c => c.CustomerId == customerId && c.CampaignId == id);

            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int customerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);

            Campaign campaign = db.Campaigns.Single(c => c.CustomerId == customerId && c.CampaignId == id);
            if (campaign == null)
            {
                return HttpNotFound();
            }

            campaign.Status = Status.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
         * 
         * */

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            repo.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult OverridePayout(int id)
        {
            //var action = repo.FindAction(id);

            //if (action == null || repo.IsHidden(action.Id))
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.Action = action;
            var campaign = repo.FindCampaign(id);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Campaign = campaign;           
            var overridepayout = repo.GetOverridePayoutCampaign(id);

            return View(overridepayout.AsQueryable());

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
            var campaign = repo.FindCampaign(id);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Campaign = campaign;
            ViewBag.AffiliateID = new SelectList(repo.GetAffiliateByCustomer(customerID), "AffiliateID", "Company");
            ViewBag.ActionID = new SelectList(campaign.Actions, "Id", "Name");
            
            return View();
        }

        public ActionResult AddOverridePayout(string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent)
        {
            var result = repo.AddOverridePayoutCampaign(ActionID,CampaignID, AffiliateID, _PayoutType, Payout, PayoutPercent);

            return Json(CampaignID);
            //return RedirectToAction("OverridePayout", new { id = Convert.ToInt32(ActionID) });
        }


        public ActionResult EditOverridePayout(int id)
        {
            var overridepayout = repo.FindActionByOverride(id);
            //var action = repo.FindAction(overridepayout.ActionID);
            var customer = repo.GetCurrentCustomer();
            //if (action == null || repo.IsHidden(action.Campaign))
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.Action = action;
            var campaign = repo.FindCampaign((int)overridepayout.CampaignID);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Campaign = campaign;
            ViewBag.OverridID = id;
            ViewBag.DDLPayout = overridepayout.PayoutType;
            ViewBag.AffiliateID = new SelectList(repo.GetAffiliateByCustomer(customer.CustomerId), "AffiliateID", "Company", overridepayout.AffiliateID);
            ViewBag.ActionID = new SelectList(campaign.Actions, "Id", "Name",overridepayout.ActionID);
            return View(overridepayout);
        }

        public ActionResult UpdateOverridePayout(string OverridID,string ActionID, string CampaignID, string AffiliateID, string PayoutType, string Payout, string PayoutPercent)
        {
            var result = repo.UpdateOverridePayoutCampaign(Convert.ToInt32(OverridID),ActionID, CampaignID, AffiliateID, PayoutType, Payout, PayoutPercent);

            return Json(CampaignID);
        }


    }
}
