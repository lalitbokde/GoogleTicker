using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.signalra.Controllers
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
            var url = repo.FindURL(id);
            if (url == null || repo.IsHidden(url.Campaign))
            {
                return HttpNotFound();
            }
            ViewBag.URL = url;
            ViewBag.Countries = repo.RedirectTargets().Where(u => u.RedirectUrl.URLId == url.Id);
            return View(repo.RedirectUrls().Where(u => u.URLId == url.Id));
        }

        public ActionResult Create(int id) // urlid
        {
            var url = repo.FindURL(id);
            if (url == null || repo.IsHidden(url.Campaign))
            {
                return HttpNotFound();
            }

            ViewBag.URL = url;

            // select only those countries who are not already redirected for this url
            var targets = repo.RedirectTargets().Where(t => t.RedirectUrl.URL.Id == id).Select(t => t.Min);

            ViewBag.Countries = repo.GetIP2Countries().Where(c => !targets.Contains(c.Min));

            return View(new RedirectUrl
            {
                RedirectURL = string.Format("http://www.{0}", CpaTickerConfiguration.DefaultDomainName)
            });
        }

        [HttpPost]
        public ActionResult Create(int id, RedirectUrl ru, int[] countries)
        {
            ru.URLId = id;
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    repo.AddRedirectUrl(ru);

                    if (countries != null)
                    {
                        foreach (var item in countries)
                        {
                            repo.AddRedirectTarget(new RedirectTarget
                            {
                                RedirectUrlId = ru.Id,
                                Min = item
                            });

                        }
                    }


                    scope.Complete();
                    return RedirectToAction("Index", new { id = id });
                }

            }
            ViewBag.URL = repo.FindURL(id);
            // select only those countries who are not already banned
            var targets = repo.RedirectTargets().Where(t => t.RedirectUrl.URL.Id == id).Select(t => t.Min);
            ViewBag.Countries = repo.GetIP2Countries().Where(c => !targets.Contains(c.Min));
            return View(ru);
        }

        public ActionResult Edit(int id) // redirecturlid
        {
            var ru = repo.FindRedirectUrl(id);

            if (ru == null || repo.IsHidden(ru.URL.Campaign))
            {
                return HttpNotFound();
            }

            var alredyredirectedcountries = ru.URL.RedirectUrls
                .Where(r => r.Id != ru.Id)
                .SelectMany(c => c.Targets)
                .Select(t => t.Min);

            ViewBag.Countries = from c in repo.GetIP2Countries().Where(c => !alredyredirectedcountries.Contains(c.Min)).AsEnumerable()
                                join g in repo.RedirectTargets().Where(t => t.RedirectUrlId == id) on c.Min equals g.Min into gt
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
        public ActionResult Edit(RedirectUrl ru, int[] countries) // redirecturlid
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    repo.EditRedirectUrl(ru);

                    // remove all countries and then added the new ones
                    repo.DeleteRedirectTargets(ru.Id);
                    if (countries != null)
                    {
                        foreach (var item in countries)
                        {
                            repo.AddRedirectTarget(new RedirectTarget
                            {
                                RedirectUrlId = ru.Id,
                                Min = item
                            });

                        }
                    }

                    scope.Complete();
                    return RedirectToAction("Index", new { id = ru.URLId });
                }

            }
            ViewBag.Countries = from c in repo.GetIP2Countries().AsEnumerable()
                                join g in repo.RedirectTargets().Where(t => t.RedirectUrlId == ru.Id) on c.Min equals g.Min into gt
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
