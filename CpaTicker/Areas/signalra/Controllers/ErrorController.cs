using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.signalra.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /admin/Error/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logs()
        {
            return View(/*(new EFCpatickerRepository()).Logs().OrderByDescending(l => l.ID).Take(100)*/);
        }

        public ActionResult Tmp()
        {
            var assembly = System.Reflection.Assembly.Load("EnumAssembly");
            Type type = assembly.GetType("ViewPermissiond");

            //throw new Exception("here12");

            //log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
            //log.Info("Test");

            //var exception = new ArgumentNullException("test__");
            //log.Error(exception.Message, exception);

            List<string> list = new List<string>();

            foreach (object o in Enum.GetValues(type))
            {
                list.Add(String.Format("{0}.{1} = {2}", type, o, ((long)o)));
            }

            ViewBag.values = list;

            /*****************************************************************/


            Type type1 = CPAHelper.GetEnumType1();

            List<string> list1 = new List<string>();
            foreach (object o in Enum.GetValues(type1))
            {
                list1.Add(String.Format("{0}.{1} = {2}", type1, o, ((long)o)));
            }

            ViewBag.values1 = list1;


            return View();
        }


        public ActionResult test(int id = 0)
        {
            //throw new StackOverflowException("jajajaja");

            EFCpatickerRepository repo = new EFCpatickerRepository();

            var user = repo.GetCurrentUser();
            //ViewBag.Campaigns = from c in repo.GetCustomerCampaigns(user.CustomerId).AsEnumerable()
            //                    join x in repo.ConversionPixelCampaigns().Where(x => x.ConversionPixelId == 5) on c equals x.Campaign into cx
            //                    from x in cx.DefaultIfEmpty()
            //                    select new SelectListItem
            //                    {
            //                        Value = c.Id.ToString(),
            //                        Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
            //                        Selected = x != null,

            //                    };

            var up = repo.GetCurrentUser();
            int[] af = { 1001, 1004 };
            ViewBag.Affiliates = from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                 join s in af on a.AffiliateId equals s into sa
                                 from s in sa.DefaultIfEmpty()
                                 select new SelectListItem
                                 {
                                     Value = a.AffiliateId.ToString(),
                                     Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                     Selected = s != 0
                                 };

            //foreach (var item in ViewBag.Campaigns)
            //{
            //    string c = item.Value.ToString();
            //}

            return View();
        }

        [Authorize]
        public ActionResult colog()
        {
            ViewBag.tzid = new EFCpatickerRepository().GetCurrentCustomer().TimeZone;
            return View(CPAHelper.GetConversionLogs());
        }

        [Authorize]
        public ActionResult Report()
        {
            return View();
        }

    }
}
