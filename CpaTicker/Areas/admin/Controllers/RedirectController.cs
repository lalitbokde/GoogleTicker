using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Controllers
{
    public class RedirectController : BaseController
    {
        private ICpaTickerRepository repo;

        public RedirectController()
        {
            this.repo = new EFCpatickerRepository();
        }

        public ActionResult Index(int id)
        {
            var page = repo.FindPAGE(id);
            //if (url == null || repo.IsHidden(url.Campaign))
            //{
            //    return HttpNotFound();
            //}
            ViewBag.PAGE = page;
            ViewBag.Countries = repo.RedirectTargetPages().Where(u => u.RedirectPAGE.PAGEId == page.Id);
            int userid = repo.GetCurrentUserId();
            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);

            return View(repo.RedirectPages().Where(u => u.PAGEId == page.Id));
        }

        public ActionResult Create(int id) // urlid
        {
            var page = repo.FindPAGE(id);
            //if (url == null || repo.IsHidden(url.Campaign))
            //{
            //    return HttpNotFound();
            //}

            ViewBag.PAGE = page;

            // select only those countries who are not already redirected for this url
            var targets = repo.RedirectTargetPages().Where(t => t.RedirectPAGE.PAGE.Id == id).Select(t => t.Min);

            ViewBag.Countries = repo.GetIP2Countries().Where(c => !targets.Contains(c.Min));

            return View(new RedirectPAGE
            {
                // RedirectURL = string.Format("http://www.{0}", CpaTickerConfiguration.DefaultDomainName)
                RedirectPage = string.Format("{0}", page.OfferUrl)
            });
        }

        [HttpPost]
        public ActionResult Create(int id, RedirectPAGE ru, int[] countries)
        {
            ru.PAGEId = id;
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    repo.AddRedirectPage(ru);

                    if (countries != null)
                    {
                        foreach (var item in countries)
                        {
                            repo.AddRedirectTargetPages(new RedirectTargetPage
                            {
                                RedirectPageId = ru.Id,
                                Min = item
                            });

                        }
                    }


                    scope.Complete();
                    return RedirectToAction("Index", new { id = id });
                }

            }
            ViewBag.PAGE = repo.FindPAGE(id);
            // select only those countries who are not already banned
            var targets = repo.RedirectTargetPages().Where(t => t.RedirectPAGE.PAGE.Id == id).Select(t => t.Min);
            ViewBag.Countries = repo.GetIP2Countries().Where(c => !targets.Contains(c.Min));
            return View(ru);
        }

        public ActionResult Edit(int id) // redirecturlid
        {
            var ru = repo.FindRedirectPage(id);

            //if (ru == null || repo.IsHidden(ru.URL.Campaign))
            //{
            //    return HttpNotFound();
            //}

            var alredyredirectedcountries = ru.PAGE.RedirectPages
                .Where(r => r.Id != ru.Id)
                .SelectMany(c => c.Targets)
                .Select(t => t.Min);

            ViewBag.Countries = from c in repo.GetIP2Countries().Where(c => !alredyredirectedcountries.Contains(c.Min)).AsEnumerable()
                                join g in repo.RedirectTargetPages().Where(t => t.RedirectPageId == id) on c.Min equals g.Min into gt
                                from g in gt.DefaultIfEmpty()
                                select new SelectListItem
                                {
                                    Value = c.Min.ToString(),
                                    Text = c.Name,
                                    Selected = g != null
                                };
            return View(ru);
        }

        [HttpPost]
        public ActionResult Edit(RedirectPAGE ru, int[] countries) // redirecturlid
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    repo.EditRedirectPage(ru);

                    // remove all countries and then added the new ones
                    repo.DeleteRedirectTargetsPages(ru.Id);
                    if (countries != null)
                    {
                        foreach (var item in countries)
                        {
                            repo.AddRedirectTargetPages(new RedirectTargetPage
                            {
                                RedirectPageId = ru.Id,
                                Min = item
                            });

                        }
                    }

                    scope.Complete();
                    return RedirectToAction("Index", new { id = ru.PAGEId });
                }

            }
            ViewBag.Countries = from c in repo.GetIP2Countries().AsEnumerable()
                                join g in repo.RedirectTargetPages().Where(t => t.RedirectPageId == ru.Id) on c.Min equals g.Min into gt
                                from g in gt.DefaultIfEmpty()
                                select new SelectListItem
                                {
                                    Value = c.Min.ToString(),
                                    Text = c.Name,
                                    Selected = g != null
                                };
            return View(ru);
        }
    }
}
