using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
//using System.Data.Objects.SqlClient;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using WebMatrix.WebData;
using System.Web.Security;
using System.Data.Entity.Infrastructure;

namespace CpaTicker.Areas.signalra.Controllers
{
    [Authorize]
    public class BannerController : BaseController
    {
        private CpaTickerDb db = new CpaTickerDb();
        private ICpaTickerRepository repo;

        public BannerController()
        {
            this.repo = new EFCpatickerRepository();
        }

        //
        // GET: /admin/Banner/

        public ActionResult Index()
        {
            var user = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(user.CustomerId);
            ViewBag.tzi = customer.TimeZone;

            var hidden = user.HiddenCampaigns.Select(h => h.Campaign.CampaignId);
            return View(repo.Banners().Where(b => b.CustomerId == user.CustomerId && !hidden.Contains(b.CampaignId)));
        }


        public ActionResult Details(int id)
        {
            var user = repo.GetCurrentUser();

            var banner = repo.Banners().SingleOrDefault(b => b.CustomerId == user.CustomerId && b.BannerId == id);


            if (banner == null || repo.IsHidden(banner.CampaignId))
            {
                return HttpNotFound();
            }


            Domain defaultdomain = CPAHelper.GetDefaultDomain(user.CustomerId);
            /*adding the default domain to the list of domains*/
            List<Domain> cdlist = CPAHelper.GetCustomerDomains(user.CustomerId);
            cdlist.Add(defaultdomain);

            ViewBag.CustomerDomains = cdlist.Select(d => new SelectListItem { Text = d.DomainName, Value = d.Id.ToString() });

            ViewBag.ImgPreviewUrl = defaultdomain.DomainName;


            if (user.AffiliateId.HasValue)
            {
                ViewBag.Affiliates = new List<Affiliate>() { repo.GetAffiliate(user.AffiliateId.Value, user.CustomerId) }.Select(
                    a => new SelectListItem
                    {
                        Text = String.Format("{0} - {1}", a.AffiliateId, a.Company),
                        Value = a.AffiliateId.ToString()
                    });
            }
            else
            {
                ViewBag.Affiliates = repo.GetCustomerActiveAffiliates(user.CustomerId).AsEnumerable().Select(
                    a => new SelectListItem
                    {
                        Text = String.Format("{0} - {1}", a.AffiliateId, a.Company),
                        Value = a.AffiliateId.ToString()
                    });
            }

            // set a viewbag with the set up custom field - values for this campaign
            var campaignid = repo.GetCampaignById(banner.CampaignId, user.CustomerId).Id;
            ViewBag.CustomFields = repo.GetCampaignCustomFieldValue(campaignid, user.CustomerId);
            ViewBag.CampaignId = campaignid;

            var urls = repo.GetCampaignURLs(campaignid);
            ViewBag.URLs = urls;
            ViewBag.MultipleUrls = urls.Count() > 1;

            //if (Roles.IsUserInRole("Affiliate"))
            //{
            //    // get only the affiliate id to which this user belong from userprofile
            //    int affiliateId = CPAHelper.GetAffiliateId(WebSecurity.CurrentUserId);

            //    // the affiliate is determinated by the affiliateid and the customerid
            //    ViewBag.Affiliates = db.Affiliates.Where(a => a.AffiliateId == affiliateId && a.CustomerId == user.CustomerId).Select(a => new SelectListItem { Text = a.Company, Value = SqlFunctions.StringConvert((decimal)a.AffiliateId) });

            //}
            //else
            //{
            //    //get all the affiliates associated with the customerid
            //    ViewBag.Affiliates = db.Affiliates.Where(a => a.CustomerId == user.CustomerId).Select(a => new SelectListItem { Text = a.Company, Value = SqlFunctions.StringConvert((decimal)a.AffiliateId) });

            //}
            return View(banner);
        }

        //
        // GET: /admin/Banner/Create

        public ActionResult Create()
        {
            //int customerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
            //ViewBag.CampaignId = db.Campaigns.Where(a => a.CustomerId == customerId).Select(a => new SelectListItem { Text = a.CampaignName, Value = SqlFunctions.StringConvert((decimal)a.CampaignId) });

            ViewBag.Campaigns = repo.GetUserCampaigns().AsEnumerable().Select(c => new SelectListItem()
            {
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
                Value = c.CampaignId.ToString()
            });
            return View();
        }

        //
        // POST: /admin/Banner/Create

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase image)
        {
            if (image == null)
            {
                ModelState.AddModelError("Image", "A picture is required");
            }
            int customerId = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
            Banner banner = new Banner();


            if (ModelState.IsValid)
            {

                banner.CustomerId = customerId;
                banner.CampaignId = int.Parse(Request.Form["CampaignId"]);
                banner.Name = image.FileName;
                banner.Image = new BinaryReader(image.InputStream).ReadBytes(image.ContentLength);
                banner.BannerDate = System.DateTime.UtcNow;
                using (TransactionScope scope = new TransactionScope())
                {
                    int maxBannerId = db.Banners.Where(a => a.CustomerId == customerId).Max(a => (int?)a.BannerId) ?? 1000;
                    banner.BannerId = maxBannerId + 1;
                    db.Banners.Add(banner);
                    db.SaveChanges();
                    scope.Complete();
                }
                return RedirectToAction("Index");
            }

            ViewBag.Campaigns = repo.GetUserCampaigns().AsEnumerable().Select(c => new SelectListItem()
            {
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
                Value = c.CampaignId.ToString()
            });
            return View(banner);
        }

        public ActionResult Remove(int id)
        {
            Banner banner = db.Banners.Find(id);
            if (banner == null || repo.IsHidden(banner.CampaignId))
            {
                return HttpNotFound();
            }
            db.Banners.Remove(banner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /admin/Banner/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            ViewBag.Campaigns = repo.GetUserCampaigns().AsEnumerable().Select(c => new SelectListItem()
            {
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
                Value = c.CampaignId.ToString()
            });
            return View(banner);
        }

        //
        // POST: /admin/Banner/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Banner banner)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(banner).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(banner);
        //}

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "image")] Banner banner, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                banner.CampaignId = int.Parse(Request.Form["CampaignId"]);

                if (image != null)
                {
                    banner.Name = image.FileName;
                    banner.Image = new BinaryReader(image.InputStream).ReadBytes(image.ContentLength);
                }

                banner.BannerDate = System.DateTime.UtcNow;

                db.Banners.Attach(banner);
                DbEntityEntry<Banner> entry = db.Entry(banner);
                entry.Property(e => e.BannerDate).IsModified = true;
                entry.Property(e => e.CampaignId).IsModified = true;
                if (image != null)
                {
                    entry.Property(e => e.Image).IsModified = true;
                    entry.Property(e => e.Name).IsModified = true;
                }
                db.SaveChanges();


                return RedirectToAction("Index");

                //db.Entry(banner).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            ViewBag.Campaigns = repo.GetUserCampaigns().AsEnumerable().Select(c => new SelectListItem()
            {
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
                Value = c.CampaignId.ToString()
            });
            return View(banner);
        }

        //
        // GET: /admin/Banner/Delete/5
        //[AuthorizeUser(RequiredPermission = ViewPermission.DeleteBanner)]
        //public ActionResult Delete(int id = 0)
        //{
        //    Banner banner = db.Banners.Find(id);
        //    if (banner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(banner);
        //}

        ////
        //// POST: /admin/Banner/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Banner banner = db.Banners.Find(id);
        //    db.Banners.Remove(banner);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            repo.Dispose();
            base.Dispose(disposing);
        }


    }
}
