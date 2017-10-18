using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Controllers
{
    [Authorize]
    public class URLController : BaseController
    {
        private ICpaTickerRepository repo;

        public URLController()
        {
            this.repo = new EFCpatickerRepository();
        }

        // gets the campaign real id
        public ActionResult Index(int id)
        {
            var campaign = repo.FindCampaign(id);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Campaign = campaign;


            // this is a patch
            //int count = repo.GetCampaignURLs(id).Count();
            //if (count == 0)
            //{
            //    // add the default url
            //    //URL default_url = new URL
            //    //{
            //    //    CampaignId = campaign.Id,
            //    //    OfferUrl = campaign.OfferUrl,
            //    //    PreviewUrl = campaign.PreviewUrl,
            //    //    Cost = campaign.Payout,
            //    //    Revenue = campaign.Revenue,
            //    //    PreviewId = 1
            //    //};
            //    repo.AddURL(repo.GetDefaultUrl(campaign));
            //}
            // end of the patch

            return View(repo.GetCampaignURLs(id));
        }


        public ActionResult Create(int id)
        {
            var campaign = repo.FindCampaign(id);
            if (campaign == null || repo.IsHidden(campaign))
            {
                return HttpNotFound();
            }
            ViewBag.Campaign = campaign;
            ViewBag.ParentURLId = repo.GetURLs().AsEnumerable().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = string.Format("{0} - {1} - {2}", a.Campaign.CampaignId, a.Campaign.CampaignName, a.PreviewId)
            });

            //ModelState.Clear();
            return View();
        }

        [HttpPost]
        public ActionResult Create(int id, CpaTicker.Areas.admin.Classes.URL url)
        {
            url.CampaignId = id;
            // set the previewid
            int maxid = repo.GetCampaignURLs(id).Max(u => (int?)u.PreviewId) ?? 0;
            url.PreviewId = maxid + 1;

            if (ModelState.IsValid)
            {
                repo.AddURL(url);
                return RedirectToAction("index", new { id = url.CampaignId });
            }
            // if there is an error fill viewbag
            ViewBag.Campaign = repo.FindCampaign(id);
            ViewBag.ParentURLId = repo.GetURLs().AsEnumerable().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = string.Format("{0} - {1} - {2}", a.Campaign.CampaignId, a.Campaign.CampaignName, a.PreviewId)
            });
            return View(url);
        }

        // gets the action id
        public ActionResult Edit(int id)
        {
            var url = repo.FindURL(id);
            if (url == null || repo.IsHidden(url.Campaign))
            {
                return HttpNotFound();
            }

            ViewBag.ParentURLId = repo.GetURLs().Where(a => a.Id != url.Id).
            AsEnumerable().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = string.Format("{0} - {1} - {2}", a.Campaign.CampaignId, a.Campaign.CampaignName, a.PreviewId)
            });
            return View(url);
        }

        [HttpPost]
        public ActionResult Edit(CpaTicker.Areas.admin.Classes.URL url)
        {
            #region revenue-cost
            if (url.PayoutType == PayoutType.CPS)
            {
                url.Payout = null; // just take the payoutpercent part
            }
            else if (url.PayoutType != PayoutType.CPA_CPS)
            {
                url.PayoutPercent = null; // just take the payout part
            }

            if (url.RevenueType == RevenueType.RPS)
            {
                url.Revenue = null; // just take the payoutpercent part
            }
            else if (url.RevenueType != RevenueType.RPA_RPS)
            {
                url.RevenuePercent = null; // just take the payout part
            }
            #endregion

            if (ModelState.IsValid)
            {
                repo.EditURL(url);
                return RedirectToAction("index", new { id = url.CampaignId });
            }

            ViewBag.ParentURLId = repo.GetURLs().Where(a => a.Id != url.Id).
            AsEnumerable().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = string.Format("{0} - {1} - {2}", a.Campaign.CampaignId, a.Campaign.CampaignName, a.PreviewId)
            });

            return View(url);
        }

        //public ActionResult GeoTargets(int id) // urlid
        //{
        //    var url = repo.FindURL(id);
        //    if (url == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.URL = url;
        //    ViewBag.Countries = repo.RedirectTargets().Where(u => u.RedirectUrl.URLId == url.Id).ToList();
        //    return View(repo.RedirectUrls().Where(u => u.URLId == url.Id));

        //}

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

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}
