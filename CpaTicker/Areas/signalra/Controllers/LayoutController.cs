using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.signalra.Controllers
{
    [Authorize]
    public class LayoutController : Controller
    {
        private ICpaTickerRepository repo;

        public LayoutController()
        {
            this.repo = new EFCpatickerRepository();
        }

        //
        // GET: /admin/Layout/

        public ActionResult Index()
        {
            ViewBag.TickerSetting = repo.GetTickerSettings() ?? CPAHelper.GetDefaultTickerSettings();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }


    }
}
