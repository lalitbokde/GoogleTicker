using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Models;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Classes;

namespace CpaTicker.Controllers
{
    public class meet_clicktickerController : Controller
    {
        private CpaTickerDb db = new CpaTickerDb();

        public readonly ICpaTickerRepository repo = null;

        public meet_clicktickerController()
        {
            this.repo = new EFCpatickerRepository();
        }

        public ActionResult RequestDemo(string subject = "")
        {
            ViewBag.Message = "";
            var model = new RequestDemo { Subject = subject };

            return View(model);
        }

        [HttpPost]
        public ActionResult RequestDemo(RequestDemo model)
        {
            if (ModelState.IsValid)
            {

                CPAHelper.SendEmailForDemo("noreply@clickticker.com", model);

                ViewBag.Message = "Thank you for your submission. Someone will contact you shortly.";
            }

            return View();
        }
    }
}
