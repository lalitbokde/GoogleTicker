using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CpaTicker.Areas.signalra.Controllers
{
     [Authorize]
    public class HomeController : BaseController
    {        
        public ActionResult Index()
        {
            return View();
        }
        private ICpaTickerRepository repo;

        //public ActionResult Index()
        //{
        //    ViewBag.TickerSetting = repo.GetTickerSettings() ?? CPAHelper.GetDefaultTickerSettings();
        //    return View();
        //}

        public HomeController()
        {
            this.repo = new EFCpatickerRepository();
        }

        public ActionResult Dashboard(/*string fromdate = ""*/)
        {
            //int customerid = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
            //Customer cus = CPAHelper.GetCustomer(customerid);
            //ViewModels.Admin.IndexViewModel model = new ViewModels.Admin.IndexViewModel();
            //ViewBag.UserCurrentTimeZone = cus.TimeZone;

            //model.Cost = 0.00M;
            //model.Revenue = 0.00M;
            //model.Profit = 0.00M;
            //model.NumCampaigns = db.Campaigns.Count(c=>c.CustomerId == customerid);
            //model.NumClicks = db.Clicks.Count(cl=>cl.CustomerId == customerid);
            //model.NumConversions= db.Conversions.Count(co=>co.CustomerId == customerid);

            //var customer = repo.GetCurrentCustomer();
            //ViewBag.UserCurrentTimeZone = customer.TimeZone;

            return View(repo.GetTickers());
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }

    }
}
