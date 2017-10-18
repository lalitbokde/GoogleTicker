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
    public class HomeController : Controller
    {
        private CpaTickerDb db = new CpaTickerDb();

        public readonly ICpaTickerRepository repo = null;

        public HomeController()
        {
            this.repo = new EFCpatickerRepository();
        }
        public ActionResult Index()
        {
            ViewBag.Message = "";
            var model = new RequestDemo();

            bool isredirect = repo.GetCustomerByURL(Request.Url);
            if (isredirect)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }



        [HttpPost]
        public ActionResult Index(RequestDemo model)
        {
            if (ModelState.IsValid)
            {

                CPAHelper.SendEmailForDemo("noreply@clickticker.com", model);

                ViewBag.Message = "Thank you for your submission. Someone will contact you shortly.";
            }

            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "";

        //    return View();
        //}

        public ActionResult Features()
        {
            return View();
        }
        [ActionName("The-Necessity-of-Multiple-Technology-Conjunction-Points-for-Transaction-Attribution-in-the-Modern-Internet-Marketing-Industry")]
        public ActionResult Article1()
        {
            return View();
        }

        public ActionResult Contact(string subject = "")
        {
            ViewBag.Message = "";
            var model = new ContactModel { Subject = subject };

            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                var sb = new System.Text.StringBuilder();

                sb.AppendFormat("Name: {0}\r\n", model.Name);
                sb.AppendFormat("Phone: {0}\r\n", model.Phone);
                sb.AppendFormat("Message:\r\n{0}", model.Message);

                CPAHelper.SendMail(model.Email, CpaTickerConfiguration.MailTo,
                    "website inquiry: " + model.Subject, sb.ToString());

                ViewBag.Message = "Thank you for your submission. Someone will contact you shortly.";
            }

            return View();
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

        public ActionResult HttpStatus404()
        {
            //return HttpNotFound();

            return View();
        }

        public ActionResult ios_welcome()
        {
            return View();
        }

        public ActionResult android_welcome()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult pricing()
        {
            return View();
        }

        public ActionResult Index2()
        {
            ViewBag.Message = "";
            var model = new RequestDemo();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index2(RequestDemo model)
        {
            if (ModelState.IsValid)
            {

                CPAHelper.SendEmailForDemo("noreply@clickticker.com", model);

                ViewBag.Message = "Thank you for your submission. Someone will contact you shortly.";
            }

            return View();
        }

        //public ActionResult DashBoardCompains()
        //{

        //    var CustomerObj = repo.GetCurrentCustomer();
        //    int CustomerID = CustomerObj.CustomerId;
        //    // select count (campaignid) from campaigns where customerid=@customerid and status=1
        //    int Campains = db.Campaigns.Where(u => u.CustomerId == CustomerID && u.Status == Status.Active).ToList().Count;
        //   // var NotifyCampaign = repo.GetNotifyCampaignData(CustomerID, "campaign");
        //  //  object obj = 
        //    return Json(Campains);
        //}

        public ActionResult DashBoardSparks()
        {
            //var customerid = repo.GetCurrentCustomerId();
            //var pagename = "DashboardSparks";
            //var report = repo.GetNotifyHourlyRptData(customerid, pagename);
            SparksController obj = new SparksController();
            var Result = obj.Get().ToList();
            var CustomerObj = repo.GetCurrentCustomer();
            int CustomerID = CustomerObj.CustomerId;
            int Campains = db.Campaigns.Where(u => u.CustomerId == CustomerID && u.Status == Status.Active).ToList().Count;
            return Json(new { Campains = Campains, Revenue = Result[Result.Count - 1].Revenue, Click = Result[Result.Count - 1].Clicks, Conversion = Result[Result.Count - 1].Conversions });

        }

        //public ActionResult TickerNotify(string TickerID)
        //{
        //    var ticker = repo.FindTicker(Convert.ToInt32(TickerID));      
        //    var CustomerObj = repo.GetCurrentUser();
        //    int CustomerID = CustomerObj.CustomerId;
        //    int UserId = CustomerObj.UserId;


        //    // select count (campaignid) from campaigns where customerid=@customerid and status=1
        //    //int Campains = db.Campaigns.Where(u => u.CustomerId == CustomerID && u.Status == Status.Active).ToList().Count;
        //    var NotifyTicker = repo.GetNotifyTickerData(CustomerID,Convert.ToInt32(TickerID), "ticker"); ;

        //    return Json(true);
        //}
        //public ActionResult NotifySparks()
        //{

        //    var customerid = repo.GetCurrentCustomerId();
        //    var pagename = "onlySparks";
        //    var report = repo.GetNotifyHourlyRptData(customerid, pagename);
        //    return Json(true);
        //}
    }
}
