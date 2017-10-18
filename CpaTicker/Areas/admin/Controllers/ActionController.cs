using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Models;


namespace CpaTicker.Areas.admin.Controllers
{
    [Authorize]
    public class ActionController : BaseController1
    {
        private ICpaTickerRepository repo;


        public ActionController()
        {
            this.repo = new EFCpatickerRepository();
        }

        public ActionResult Index(int id) // campaignid
        {
            var campaign = repo.FindCampaign(id);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }

            var user = repo.GetCurrentUser();
            var model = repo.Actions().Where(a => a.CampaignId == id);

            //there allways be one default action This a patch OJO
            //if (model.Count() == 0)
            //{
            //    // create a new action for this user and this campaign  with the default tracking code for this campaign
            //    //var default_cp = repo.ConversionPixels().First();
            //    var default_action = repo.GetDefaultAction(campaign);
            //    repo.AddAction(default_action);
            //    model.Concat(new Classes.Action[] { default_action });
            //}
            ViewBag.Campaign = campaign;
            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);
            return View(model);
        }

        //
        // GET: /admin/Action/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        ////
        //// GET: /admin/Action/Create

        public ActionResult Create(int id = 0)
        {
            var campaign = repo.FindCampaign(id);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Campaign = campaign;
            return View();
        }

        ////
        //// POST: /admin/Action/Create

        [HttpPost]
        public ActionResult Create(int id, ActionCreateVM act)
        {
            //act.CampaignId = id;
            //act.UserId = repo.GetCurrentUserId();
            //act.TrackingCode = Server.HtmlEncode(act.TrackingCode);
            if (ModelState.IsValid)
            {
                repo.AddAction(new CpaTicker.Areas.admin.Classes.Action
                {
                    //TrackingCode = Server.HtmlEncode(act.TrackingCode),
                    CampaignId = act.CampaignId,
                    Type = act.Type,
                    //TrackingType = act.TrackingType,
                    Name = act.Name,

                    Payout = act.Payout,
                    PayoutType = act.PayoutType,
                    PayoutPercent = act.PayoutPercent,

                    Revenue = act.Revenue,
                    RevenueType = act.RevenueType,
                    RevenuePercent = act.RevenuePercent
                });
                return RedirectToAction("Index", new { id = act.CampaignId });
            }
            // if there is an error fill viewbag
            ViewBag.Campaign = repo.FindCampaign(id);
            return View(act);
        }

        ////
        //// GET: /admin/Action/Edit/5

        public ActionResult Edit(int id)
        {
            var action = repo.FindAction(id);
            if (action == null || repo.IsHidden(action.Campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Campaign = action.Campaign;
            return View(new ActionEditVM
            {
                Name = action.Name,
                Id = action.Id,
                CampaignId = action.CampaignId,

                Type = action.Type,
                //TrackingType = action.TrackingType,

                Payout = action.Payout,
                PayoutType = action.PayoutType,
                PayoutPercent = action.PayoutPercent,

                Revenue = action.Revenue,
                RevenueType = action.RevenueType,
                RevenuePercent = action.RevenuePercent
            });
        }

        ////
        //// POST: /admin/Action/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ActionEditVM act)
        {
            var action = repo.FindAction(act.Id);

            #region revenue-cost
            if (act.PayoutType == PayoutType.CPS)
            {
                act.Payout = null; // just take the payoutpercent part
            }
            else if (act.PayoutType != PayoutType.CPA_CPS)
            {
                act.PayoutPercent = null; // just take the payout part
            }

            if (act.RevenueType == RevenueType.RPS)
            {
                act.Revenue = null; // just take the payoutpercent part
            }
            else if (act.RevenueType != RevenueType.RPA_RPS)
            {
                act.RevenuePercent = null; // just take the payout part
            }
            #endregion

            if (ModelState.IsValid)
            {
                //action.TrackingCode = Server.HtmlEncode(act.TrackingCode);
                action.Name = act.Name;
                action.Type = act.Type;
                //action.TrackingType = act.TrackingType;

                action.Payout = act.Payout;
                action.PayoutType = act.PayoutType;
                action.PayoutPercent = act.PayoutPercent;

                action.Revenue = act.Revenue;
                action.RevenueType = act.RevenueType;
                action.RevenuePercent = act.RevenuePercent;

                repo.EditAction(action);
                return RedirectToAction("index", new { id = action.CampaignId });
            }

            ViewBag.CampaignId = action.CampaignId;
            return View(act);
        }

        public ActionResult Tracking(int id)
        {
            var action = repo.FindAction(id);
            if (action == null || repo.IsHidden(action.Campaign))
            {
                return HttpNotFound();
            }
            //ViewBag.TrackingCode = repo.TrackingCode(action, 0, action.Id);

            //var domain = repo.GetDefaultDomain(action.Campaign.CustomerId);
            ViewBag.Domain = repo.GetDefaultDomain(action.Campaign.CustomerId).DomainName;

            return View(action);
        }

        public ActionResult Pixels(int id)
        {
            var action = repo.FindAction(id);
            if (action == null || repo.IsHidden(action.Campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Action = action;
            return View(repo.ActionConversionPixels().Where(a => a.ActionId == id));
        }


        public ActionResult OverridePayout(int id)
        {
            var action = repo.FindAction(id);

            if (action == null || repo.IsHidden(action.Id))
            {
                return HttpNotFound();
            }
            ViewBag.Action = action;
            var campaign = repo.FindCampaign(action.CampaignId);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Campaign = campaign;

            var overridepayout = repo.GetOverridePayout(id);

            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);

            return View(overridepayout.AsQueryable());

        }



        public ActionResult CreateOverridePayout(int id)
        {
            var action = repo.FindAction(id);
            var customerID = repo.GetCurrentCustomer().CustomerId;
            if (action == null || repo.IsHidden(action.Campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Action = action;
            var campaign = repo.FindCampaign(action.CampaignId);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Campaign = campaign;
            ViewBag.AffiliateID = new SelectList(repo.GetAffiliateByCustomer(customerID), "AffiliateID", "Company");
            return View();
        }

        public ActionResult AddOverridePayout(string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent)
        {
            var result = repo.AddOverridePayout(ActionID, CampaignID, AffiliateID, _PayoutType, Payout, PayoutPercent);

            return Json(ActionID);
            //return RedirectToAction("OverridePayout", new { id = Convert.ToInt32(ActionID) });
        }


        public ActionResult EditOverridePayout(int id)
        {
            var overridepayout = repo.FindActionByOverride(id);
            var action = repo.FindAction((int)overridepayout.ActionID);
            var customer = repo.GetCurrentCustomer();
            if (action == null || repo.IsHidden(action.Campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Action = action;
            var campaign = repo.FindCampaign(action.CampaignId);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Campaign = campaign;
            ViewBag.OverridID = id;
            ViewBag.DDLPayout = overridepayout.PayoutType;
            ViewBag.AffiliateID = new SelectList(repo.GetAffiliateByCustomer(customer.CustomerId), "AffiliateID", "Company", overridepayout.AffiliateID);
            return View(overridepayout);
        }

        public ActionResult UpdateOverridePayout(string OverridID, string ActionID, string AffiliateID, string PayoutType, string Payout, string PayoutPercent)
        {
            var result = repo.UpdateOverridePayout(Convert.ToInt32(OverridID), ActionID, AffiliateID, PayoutType, Payout, PayoutPercent);

            return Json(ActionID);
        }


        ////
        //// GET: /admin/Action/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /admin/Action/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
