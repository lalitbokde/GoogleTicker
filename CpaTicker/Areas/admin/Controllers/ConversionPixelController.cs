using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Models;
using System.Transactions;
using System.Web.Security;

namespace CpaTicker.Areas.admin.Controllers
{
    public class ConversionPixelController : BaseController
    {
        ICpaTickerRepository repo;

        public ConversionPixelController()
        {
            repo = new EFCpatickerRepository();
        }

        public ActionResult Index()
        {

            var user = repo.GetCurrentUser();
            ViewBag.isaffiliateuser = user.AffiliateId.HasValue;
            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);
            if (user.AffiliateId.HasValue)
            {
                // at this point the user is an affiliate so display the affiliate cps
                var affiliateId = repo.GetAffiliate(user.AffiliateId.Value, user.CustomerId).Id;
                return View(repo.ConversionPixels().Where(cp => cp.AffiliateId == affiliateId));
            }

            var result = repo.GetCustomerConversionPixel(user.CustomerId);

            var filter = PixelFilter.active;
            try
            {
                filter = Request.QueryString.GetValue<PixelFilter>("filter");
            }
            catch { }

            switch (filter)
            {

                case PixelFilter.active:
                    result = repo.GetCustomerConversionPixelFilter(user.CustomerId, PixelStatus.Active);
                    break;

                case PixelFilter.pending:
                    result = repo.GetCustomerConversionPixelFilter(user.CustomerId, PixelStatus.Pending);
                    break;

                case PixelFilter.paused:
                    result = repo.GetCustomerConversionPixelFilter(user.CustomerId, PixelStatus.Paused);
                    break;
                case PixelFilter.Blocked:
                    result = repo.GetCustomerConversionPixelFilter(user.CustomerId, PixelStatus.Blocked);
                    break;
                case PixelFilter.Rejected:
                    result = repo.GetCustomerConversionPixelFilter(user.CustomerId, PixelStatus.Rejected);
                    break;

            }


            ViewBag.filter = filter;



           

            // user is a customer so return all conversions of all affiliates of that customer
            return View(result);
        }

        private void CreateViewBag(TrackingType type = TrackingType.HttpiFrame)
        {
            var user = repo.GetCurrentUser();
            //ViewBag.Campaigns = repo.GetCustomerCampaigns(user.CustomerId).Where(c => c.TrackingType == type).AsEnumerable().Select(c => new SelectListItem

            // return all campaigns who default action has the corresponding tracking type
            ViewBag.Campaigns = repo.GetUserCampaigns(user)
                .AsEnumerable()
                //.Where(c => c.Actions.First().TrackingType == type)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
                });

            ViewBag.IsAdmin = !user.AffiliateId.HasValue;
            if (ViewBag.IsAdmin)
            {

                // get the list of affiliates
                ViewBag.Affiliates = repo.GetCustomerAffiliates(user.CustomerId).AsEnumerable().Select(c => new SelectListItem
                {
                    // changed to ID
                    Value = c.Id.ToString(),
                    Text = string.Format("{0} - {1}", c.AffiliateId, c.Company),
                }).OrderBy(x=>x.Text);
            }
            else
            {
                ViewBag.AffiliateId = repo.GetAffiliate(user.AffiliateId.Value, user.CustomerId).Id;
            }
        }

        private void EditViewBag(ConversionPixel cp)
        {
            var user = repo.GetCurrentUser();
            //ViewBag.Campaigns = from c in repo.GetCustomerCampaigns(user.CustomerId).AsEnumerable()
            //                    join x in repo.ConversionPixelCampaigns().Where(x => x.ConversionPixelId == cp.Id) on c equals x.Campaign into cx
            //                    from x in cx.DefaultIfEmpty()
            //                    select new SelectListItem
            //                    {
            //                        Value = c.Id.ToString(),
            //                        //Value = System.Data.Entity.SqlServer.SqlFunctions.StringConvert((decimal)c.Id),
            //                        Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
            //                        Selected = x != null

            //                    };
            ViewBag.IsAdmin = !user.AffiliateId.HasValue;
            if (ViewBag.IsAdmin)
            {
                // get the list of affiliates
                ViewBag.Affiliates = repo.GetCustomerAffiliates(user.CustomerId).AsEnumerable().Select(c => new SelectListItem
                {
                    //Value = c.AffiliateId.ToString(),
                    // This relationship is wrong.  needs to be id.
                    Value = c.Id.ToString(),
                    Text = string.Format("{0} - {1}", c.AffiliateId, c.Company)
                }).OrderBy(x=>x.Text);
            }
            else
            {
                // get the real affiliate id
                ViewBag.AffiliateId = repo.GetAffiliate(user.AffiliateId.Value, user.CustomerId).Id;
            }

            // how many campaigns this pixel has? Disabled trackingtype then
            if (repo.ConversionPixelCampaigns().Count(x => x.ConversionPixelId == cp.Id) > 0)
            {
                ViewBag.DisabledTrackingType = new { @disabled = "disabled" };
            }
            else
            {
                ViewBag.DisabledTrackingType = null;
            }
        }

        public ActionResult Create()
        {
            CreateViewBag();
            string[] AssignedRoles = Roles.GetRolesForUser(User.Identity.Name);
            if (AssignedRoles.Contains("Administrator"))
            { return View(new ConversionPixelViewModel { PixelStatus = PixelStatus.Active }); }
            return View(new ConversionPixelViewModel { PixelStatus = PixelStatus.Pending });
        }

        [HttpPost]
        public ActionResult Create(ConversionPixelViewModel cpvm)
        {
            //if (campaigns == null)
            //{
            //    ModelState.AddModelError("campaings", "At least one campaign is required");
            //    ViewBag.CampaignsErrorMessage = "The Campaigns field is required";
            //}

            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var cp = new ConversionPixel
                    {
                        AffiliateId = cpvm.AffiliateId,
                        //PixelCode = cpvm.PixelCode,

                        PixelCode = Server.HtmlEncode(cpvm.PixelCode),
                        //Server.HtmlEncode(Request.Unvalidated.Form["PixelCode"]
                        TrackingType = cpvm.TrackingType
                    };
                    repo.AddConversionPixel(cp);

                    foreach (var campaignid in cpvm.Campaigns)
                    {
                        repo.AddConversionPixelCampaign(new ConversionPixelCampaign { CampaignId = campaignid, ConversionPixelId = cp.Id, PixelStatus = cpvm.PixelStatus });
                    }
                    repo.SaveChanges();

                    // after this point is all recorded so add the action-cp relation

                    // adding this cp to the default action of each of the campaigns selected
                    foreach (var campaignid in cpvm.Campaigns)
                    {
                        var defaultaction = repo.DefaultAction(repo.FindCampaign(campaignid));
                        repo.AddActionConversionPixel(new ActionConversionPixel { ActionId = defaultaction.Id, ConversionPixelId = cp.Id });
                    }

                    scope.Complete();
                    return RedirectToAction("index");
                }
            }
            CreateViewBag(cpvm.TrackingType);
            return View();

        }

        public ActionResult Edit(int id)
        {
            var cp = repo.FindConversionPixel(id);
            if (cp == null)
            {
                return HttpNotFound();
            }
            EditViewBag(cp);
            cp.PixelCode = Server.HtmlDecode(cp.PixelCode);
            return View(cp);
        }

        [HttpPost]
        public ActionResult Edit(ConversionPixel cp)
        {
            if (ModelState.IsValid)
            {
                cp.PixelCode = Server.HtmlEncode(cp.PixelCode);
                repo.EditConversionPixel(cp);
                return RedirectToAction("Index");
            }
            EditViewBag(cp);
            return View(cp);
        }

        public ActionResult Delete(int id)
        {
            var cp = repo.FindConversionPixel(id);
            if (cp == null)
            {
                return HttpNotFound();
            }
            return View(repo.ConversionPixelCampaigns().Where(x => x.ConversionPixelId == id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var cp = repo.FindConversionPixel(id);
            repo.RemoveConversionPixel(cp);
            return RedirectToAction("Index");
        }

        public ActionResult Campaigns(int id)
        {
            var cp = repo.FindConversionPixel(id);
            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);
            var filter = PixelFilter.all;
            try
            {
                filter = Request.QueryString.GetValue<PixelFilter>("filter");
            }
            catch { }

            switch (filter)
            {

                case PixelFilter.active:
                    cp = repo.FindCenversionPixelActive(id, PixelStatus.Active);
                    break;

                case PixelFilter.pending:
                    cp = repo.FindCenversionPixelActive(id, PixelStatus.Pending);
                    break;

                case PixelFilter.paused:
                    cp = repo.FindCenversionPixelActive(id, PixelStatus.Paused);
                    break;

            }


            ViewBag.filter = filter;


            //if (cp == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(repo.ConversionPixelCampaigns().Where(x => x.ConversionPixelId == cp.Id));
            ViewBag.HiddenCampaigns = repo.GetCurrentUser().HiddenCampaigns.Select(h => h.CampaignId);
            return View(cp);
        }

        private void AddCampaignViewBag(ConversionPixel cp)
        {
            //var user = repo.GetCurrentUser();
            var alreadyincampaigns = repo.ConversionPixelCampaigns().Where(p => p.ConversionPixelId == cp.Id).Select(p => p.CampaignId);
            // all campaigns who default action match the tracking type
            ViewBag.Campaigns = repo.GetUserCampaigns()
                .AsEnumerable()
                .Where(c => !alreadyincampaigns.Contains(c.Id) /*&& c.Actions.First().TrackingType == cp.TrackingType*/)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
                });
        }

        public ActionResult AddCampaign(int id)
        {
            var cp = repo.FindConversionPixel(id);
            if (cp == null)
            {
                return HttpNotFound();
            }

            AddCampaignViewBag(cp);
            return View(new ConversionPixelCampaignsViewModel { PixelStatus = PixelStatus.Pending });
        }

        [HttpPost]
        public ActionResult AddCampaign(int id, ConversionPixelCampaignsViewModel cpcvm)
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    foreach (var campaignid in cpcvm.Campaigns)
                    {
                        repo.AddConversionPixelCampaign(new ConversionPixelCampaign { CampaignId = campaignid, ConversionPixelId = id, PixelStatus = cpcvm.PixelStatus });

                        // tied this cp the the campaign default action ?
                        try
                        {

                            //the try - catch if for is the action already belogns to this cp 

                            // get the default action for the campaign and added
                            var defaultact = repo.DefaultAction(campaignid);
                            repo.AddActionConversionPixel(new ActionConversionPixel
                            {
                                ActionId = defaultact.Id,
                                ConversionPixelId = id,
                            });
                        }
                        catch { }

                    }
                    repo.SaveChanges();
                    scope.Complete();
                    return RedirectToAction("campaigns", new { id = id });
                }

            }
            //var user = repo.GetCurrentUser();
            var cp = repo.FindConversionPixel(id);
            AddCampaignViewBag(cp);
            return View(cpcvm);
        }

        public ActionResult EditStatus(int id)
        {
            var cpc = repo.FindConversionPixelCampaign(id);
            if (cpc == null)
            {
                return HttpNotFound();
            }
            return View(cpc);
        }

        [HttpPost]
        public ActionResult EditStatus(ConversionPixelCampaign cpc)
        {
            //var cpc = repo.FindConversionPixelCampaign(id);
            if (ModelState.IsValid)
            {
                repo.EditConversionPixelCampaign(cpc);
                return RedirectToAction("campaigns", new { id = cpc.ConversionPixelId });
            }
            return View(cpc);
        }

        public ActionResult Actions(int id)
        {
            var cp = repo.FindConversionPixel(id);
            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);
            if (cp == null)
            {
                return HttpNotFound();
            }
            //return View(repo.ActionConversionPixels().Where(x => x.ConversionPixelId == cp.Id));
            ViewBag.HiddenCampaigns = repo.GetCurrentUser().HiddenCampaigns.Select(h => h.CampaignId);
            return View(cp);
        }

        public ActionResult AddAction(int id)
        {
            var cp = repo.FindConversionPixel(id);
            if (cp == null)
            {
                return HttpNotFound();
            }
            var customerid = repo.GetCurrentCustomerId();
            var inactions = repo.ActionConversionPixels().Where(a => a.ConversionPixelId == cp.Id).Select(a => a.ActionId);

            // gets all the actions with equal tracking type that are not already in associated with this cp
            // actions that belongs to the present customer' campaigns
            var hidden = repo.GetCurrentUser().HiddenCampaigns.Select(h => h.CampaignId);
            ViewBag.Actions = repo.Actions().Where(a => /*a.TrackingType == cp.TrackingType 
                &&*/ a.Campaign.CustomerId == customerid && !hidden.Contains(a.CampaignId)
                && !inactions.Contains(a.Id)
                )

                .AsEnumerable().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = String.Format("{0} - {1}", a.Campaign.CampaignId, a.Name),
                });
            ViewBag.cp = cp;

            return View();
        }

        [HttpPost]
        public ActionResult AddAction(int id, int[] actions)
        {
            // add the selected actions the this cp
            using (var scope = new TransactionScope())
            {
                foreach (var item in actions)
                {
                    repo.AddActionConversionPixel(new ActionConversionPixel { ActionId = item, ConversionPixelId = id });
                }

                scope.Complete();
            }

            return RedirectToAction("actions", new { id = id });
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}
