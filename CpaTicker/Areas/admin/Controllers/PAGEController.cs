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
    public class PAGEController : BaseController
    {
        private ICpaTickerRepository repo;
        public PAGEController()
        {
            this.repo = new EFCpatickerRepository();
        }

        public ActionResult Index()
        {
            int CustomerId = repo.GetCurrentCustomer().CustomerId;
            //var campaign = repo.FindCampaign(id);
            //if (campaign == null || repo.IsHidden(campaign))
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.Campaign = campaign;


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
            var filter = PageFilter.active;
            try
            {
                filter = Request.QueryString.GetValue<PageFilter>("filter");
            }
            catch { }

            IQueryable<PAGE> PageResults = null;
            switch (filter)
            {
                case PageFilter.all:
                    PageResults = repo.GetCustomerPage(CustomerId);

                    //campaigns.Where(c => c.Status == Status.Active || c.Status == Status.Pending);
                    break;
                case PageFilter.active:
                    PageResults = repo.GetCustomerPageByStatus(CustomerId, PageStatus.Active);
                    break;

                case PageFilter.Inactive:
                    PageResults = repo.GetCustomerPageByStatus(CustomerId, PageStatus.InActive);
                    break;

                case PageFilter.deleted:
                    PageResults = repo.GetCustomerPageByStatus(CustomerId, PageStatus.Deleted);
                    break;

            }
            ViewBag.filter = filter;

            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);
            return View(PageResults);
        }


        public ActionResult Create()
        {
            int CustomerId = repo.GetCurrentCustomer().CustomerId;
            //var campaign = repo.FindCampaign(id);
            //if (campaign == null || repo.IsHidden(campaign))
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.Campaign = campaign;
            ViewBag.ParentURLId = repo.GetCustomerPage(CustomerId).AsEnumerable().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = string.Format("{0} - {1} - {2}", a.Id, a.Name, a.PreviewUrl)
            });
            ViewBag.CategoryList = repo.GetPageCategories().ToList();

            //ModelState.Clear();
            return View();
        }
        
        [HttpGet]
        public ActionResult GetPageCategories()
        {
            return Json(repo.GetPageCategories().ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SavePageCategory(PAGECategory category)
        {
            var item = repo.SavePageCategory(category);
            return Json(item);
        }

        [HttpPost]
        public ActionResult RemovePageCategory(int id, bool force = false)
        {
            var response = repo.RemovePageCategory(id, force);

            if (response == 0)
                return Json(true);
            else
                return Json(response);
        }

        [HttpPost]
        public ActionResult Create(CpaTicker.Areas.admin.Classes.PAGE page)
        {
            int CustomerId = repo.GetCurrentCustomer().CustomerId;
            if (ModelState.IsValid)
            {
                page.CustomerID = CustomerId;
                repo.AddPAGE(page);
                return RedirectToAction("index");
            }
            // if there is an error fill viewbag
            //  ViewBag.Campaign = repo.FindCampaign(id);
            ViewBag.ParentURLId = repo.GetCustomerPage(CustomerId).AsEnumerable().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = string.Format("{0} - {1} - {2}", a.Id, a.Name, a.PreviewUrl)
            });
            return View(page);
        }

        // gets the action id
        public ActionResult Edit(int id)
        {
            var page = repo.FindPAGE(id);
            if (page == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentURLId = repo.GetCustomerPage(page.CustomerID).Where(a => a.Id != page.Id).
            AsEnumerable().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
              //  Selected = (a.ParentURLId == page.ParentURLId),
                Text = string.Format("{0} - {1} - {2}", a.Id, a.Name, a.PreviewUrl)
            });
            ViewBag.CategoryList = repo.GetPageCategories().ToList();
            return View(page);
        }

        [HttpPost]
        public ActionResult Edit(CpaTicker.Areas.admin.Classes.PAGE page)
        {
            #region revenue-cost
            if (page.PayoutType == PayoutType.CPS)
            {
                page.Payout = null; // just take the payoutpercent part
            }
            else if (page.PayoutType != PayoutType.CPA_CPS)
            {
                page.PayoutPercent = null; // just take the payout part
            }

            if (page.RevenueType == RevenueType.RPS)
            {
                page.Revenue = null; // just take the payoutpercent part
            }
            else if (page.RevenueType != RevenueType.RPA_RPS)
            {
                page.RevenuePercent = null; // just take the payout part
            }
            #endregion

            if (ModelState.IsValid)
            {
                page.CustomerID = repo.GetCurrentCustomer().CustomerId;
                repo.EditPAGE(page);
                return RedirectToAction("index");
            }

            ViewBag.ParentURLId = repo.GetCustomerPage(page.CustomerID).Where(a => a.Id != page.Id).
            AsEnumerable().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = string.Format("{0} - {1} - {2}", a.Id, a.Name, a.PreviewUrl)
            });

            return View(page);
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
