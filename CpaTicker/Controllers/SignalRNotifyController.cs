using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CpaTicker.Controllers
{
    public class SignalRNotifyController : Controller
    {

        private CpaTickerDb db = new CpaTickerDb();

        public readonly ICpaTickerRepository repo = null;

        public SignalRNotifyController()
        {
            this.repo = new EFCpatickerRepository();
        }
        public ActionResult DashBoardCompains()
        {

            var CustomerObj = repo.GetCurrentCustomer();
            int CustomerID = CustomerObj.CustomerId;
            // select count (campaignid) from campaigns where customerid=@customerid and status=1
            int Campains = db.Campaigns.Where(u => u.CustomerId == CustomerID && u.Status == Status.Active).ToList().Count;
            // var NotifyCampaign = repo.GetNotifyCampaignData(CustomerID, "campaign");
            //  object obj = 
            return Json(Campains);
        }

        //public ActionResult DashBoardSparks()
        //{
        //    //var customerid = repo.GetCurrentCustomerId();
        //    //var pagename = "DashboardSparks";
        //    //var report = repo.GetNotifyHourlyRptData(customerid, pagename);
        //    SparksController obj = new SparksController();
        //    var Result = obj.Get().ToList();
        //    var CustomerObj = repo.GetCurrentCustomer();
        //    int CustomerID = CustomerObj.CustomerId;
        //    int Campains = db.Campaigns.Where(u => u.CustomerId == CustomerID && u.Status == Status.Active).ToList().Count;
        //    return Json(new {Campains=Campains, Revenue = Result[Result.Count - 1].Revenue, Click = Result[Result.Count - 1].Clicks, Conversion = Result[Result.Count - 1].Conversions });

        //}

        public ActionResult TickerNotify(string ConnectionID)
        {

            var CustomerObj = repo.GetCurrentUser();
            int CustomerID = CustomerObj.CustomerId;
            int UserId = CustomerObj.UserId;

            // select count (campaignid) from campaigns where customerid=@customerid and status=1
            //int Campains = db.Campaigns.Where(u => u.CustomerId == CustomerID && u.Status == Status.Active).ToList().Count;
            var NotifyTicker = repo.GetNotifyTickerData(CustomerID, "ticker", ConnectionID, UserId);

            return Json(true);
        }
        //public ActionResult NotifySparks(string ConnectionID)
        //{

        //    var customerid = repo.GetCurrentCustomerId();
        //    var pagename = "onlySparks";
        //    var report = repo.GetNotifySparkHourlyRptData(customerid, pagename, ConnectionID);
        //    return Json(true);
        //}


        //public ActionResult NotificationChangeDatepicker(string timezone, string viewdata, string fromdate, string todate, string dataview)
        //{
        //    var pagename = "Hourly";
        //    var customerid = repo.GetCurrentCustomerId();
        //    var report = repo.ChangeDatepickerNotification(customerid,pagename, timezone, viewdata, fromdate, todate, dataview);
        //    return Json(true);
        //}

        public ActionResult NotifyHourlyReport(string timezone, string viewdata, string fromdate, string todate, string dataview, string ConnectionID)
        {
            var up = repo.GetCurrentUser();

            var customerid = up.CustomerId;
            var userid = up.UserId;

            repo.GetNotifySparkHourlyRptData(customerid, "onlySparks", ConnectionID);
            var NotifyTicker = repo.GetNotifyTickerData(customerid, "ticker", ConnectionID, userid);
            repo.GetNotifyHourlyRptData(customerid, "Hourly", timezone, viewdata, fromdate, todate, dataview, ConnectionID);



            //////var NotifyCampaign = repo.GetNotifyCampaignData(customerid, "campaign", ConnectionID);
            //////var reportConversionStatus = repo.NotifyConversionStatus(customerid, "ConversionStatus", timezone, viewdata, fromdate, todate, dataview, ConnectionID);
            //////var reportCTR = repo.NotifyCTRData(customerid, userid, fromdate, todate, timezone, viewdata, ConnectionID);
            return Json(true);
        }
        //public ActionResult NotifyHourlyReport()
        //{

        //    var customerid = repo.GetCurrentCustomerId();
        //    var pagename = "Hourly";
        //    var report = repo.GetNotifyHourlyRptData(customerid, pagename);
        //    return Json(true);
        //}

        public ActionResult NotifyAffiliateReport(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null, string ConnectionID = "")
        {
            var report = repo.NotifyAffiliateData(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, ConnectionID);
            return Json(true);
        }

        public ActionResult NotifyCTRReport(string timezone, string viewdata, string fromdate, string todate, string dataview, string ConnectionID)
        {
            var customerid = repo.GetCurrentCustomerId();
            var userid = repo.GetCurrentUserId();
            var report = repo.NotifyCTRData(customerid, userid, fromdate, todate, timezone, viewdata, ConnectionID);
            return Json(true);
        }


        public ActionResult NotifyConversionStatus(string timezone, string viewdata, string fromdate, string todate, string dataview, string ConnectionID)
        {
            var customerid = repo.GetCurrentCustomerId();
            var pagename = "ConversionStatus";
            var report = repo.NotifyConversionStatus(customerid, pagename, timezone, viewdata, fromdate, todate, dataview, ConnectionID);
            return Json(true);
        }

        public ActionResult StopDependency()
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlDependency.Stop(connection);
            return Json(true);
        }

        public ActionResult StartDependency()
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlDependency.Start(connection);
            return Json(true);
        }

        public ActionResult ProfilePhotoUpload()
        {
            long UserID = repo.GetCurrentUserId();

            var headers = Request.Headers;
            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                string url = UploadWholeFile(Request, UserID);
                if (url != null)
                {

                    return Json(new { flag = "true", path = url });
                }
                else
                {

                    return Json(new { flag = "false", path = "Opps Something wrong." });
                }
            }


            return null;
        }

        private string UploadWholeFile(HttpRequestBase request, long UserID)
        {
            byte[] Image = null;
            try
            {

                var file = request.Files[0];

                Image = new Byte[file.ContentLength];
                file.InputStream.Read(Image, 0, file.ContentLength);
                repo.SaveProfilePic(Image);
                string imageBase64Data = Convert.ToBase64String(Image);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);


                return imageDataURL;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public ActionResult GetBitsAssemblyJson()
        {
            List<TestAssemblyValues> objAssigned = new List<TestAssemblyValues>();
            List<TestAssemblyValues> objNew = new List<TestAssemblyValues>();
            List<TestAssemblyValues> MasterList = new List<TestAssemblyValues>();

            Type enumtype = CPAHelper.GetEnumType();
            /*
             * put the name of the action fallow by the name of the controller
             * */

            int pow = 59;
            foreach (var controllerType in GetSubClasses<CpaTicker.Areas.admin.Controllers.BaseController>())
            {
                foreach (var action in ActionNames(controllerType))
                {
                    string name = String.Format("{0}{1}", action,
                               controllerType.Name.Substring(0, controllerType.Name.Length - 10));
                    System.Reflection.FieldInfo enumItem = enumtype.GetField(name);
                    if (enumItem != null)
                    {

                        objAssigned.Add(new TestAssemblyValues { name = name, key = (long)enumItem.GetValue(enumtype) });
                    }
                    else
                    {
                        if (name == "indexHome")
                        {
                            //  objNew.Add(new TestAssemblyValues { name = name, key = 0 });
                        }
                        else
                        {
                            if (pow < 63)
                            {
                                objNew.Add(new TestAssemblyValues { name = name, key = Convert.ToInt64(Math.Pow(2, pow)) });
                            }
                            else
                            {
                                objNew.Add(new TestAssemblyValues { name = name, key = 0 });
                            }
                            pow = pow + 1;
                        }

                    }


                }

            }

            object abc = new { objAssigned, objNew };
            MasterList = objAssigned.Union(objNew).ToList();
            return Json(abc, JsonRequestBehavior.AllowGet);
        }

        public List<TestAssemblyValues> GetBitsAssembly()
        {
            List<TestAssemblyValues> objAssigned = new List<TestAssemblyValues>();
            List<TestAssemblyValues> objNew = new List<TestAssemblyValues>();
            List<TestAssemblyValues> MasterList = new List<TestAssemblyValues>();

            Type enumtype = CPAHelper.GetEnumType();
            /*
             * put the name of the action fallow by the name of the controller
             * */

            //int pow = 59;
            foreach (var controllerType in GetSubClasses<CpaTicker.Areas.admin.Controllers.BaseController>())
            {
                foreach (var action in ActionNames(controllerType))
                {
                    string name = String.Format("{0}{1}", action,
                               controllerType.Name.Substring(0, controllerType.Name.Length - 10));
                    System.Reflection.FieldInfo enumItem = enumtype.GetField(name);
                    if (enumItem != null)
                    {

                        objAssigned.Add(new TestAssemblyValues { name = name, key = (long)enumItem.GetValue(enumtype) });
                    }
                    //else
                    //{
                    //    if (name == "indexHome")
                    //    {
                    //        //  objNew.Add(new TestAssemblyValues { name = name, key = 0 });
                    //    }
                    //    else
                    //    {
                    //        if (pow < 63)
                    //        {
                    //            objNew.Add(new TestAssemblyValues { name = name, key = Convert.ToInt64(Math.Pow(2, pow)) });
                    //        }
                    //        else
                    //        {
                    //            objNew.Add(new TestAssemblyValues { name = name, key = 0 });
                    //        }
                    //        pow = pow + 1;
                    //    }

                    //}


                }

            }
            //  MasterList = objAssigned.Union(objNew).ToList();
            return objAssigned;
            //Json(MasterList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBitsAssembly1()
        {
            List<TestAssemblyValues> objAssigned = new List<TestAssemblyValues>();
            List<TestAssemblyValues> objNew = new List<TestAssemblyValues>();
            List<TestAssemblyValues> MasterList = new List<TestAssemblyValues>();

            Type enumtype = CPAHelper.GetEnumType();
            /*
             * put the name of the action fallow by the name of the controller
             * */

            int pow = 59;
            foreach (var controllerType in GetSubClasses<CpaTicker.Areas.admin.Controllers.BaseController1>())
            {
                foreach (var action in ActionNames(controllerType))
                {
                    string name = String.Format("{0}{1}", action,
                               controllerType.Name.Substring(0, controllerType.Name.Length - 10));
                    System.Reflection.FieldInfo enumItem = enumtype.GetField(name);
                    if (enumItem != null)
                    {

                        objAssigned.Add(new TestAssemblyValues { name = name, key = (long)enumItem.GetValue(enumtype) });
                    }
                    else
                    {
                        if (name == "indexHome")
                        {
                            // objNew.Add(new TestAssemblyValues { name = name, key = 0 });
                        }
                        else
                        {
                            if (pow < 63)
                            {
                                objNew.Add(new TestAssemblyValues { name = name, key = Convert.ToInt64(Math.Pow(2, pow)) });
                            }
                            else
                            {
                                objNew.Add(new TestAssemblyValues { name = name, key = 0 });
                            }
                            pow = pow + 1;
                        }

                    }


                }

            }
            object abc = new { objAssigned, objNew };
            MasterList = objAssigned.Union(objNew).ToList();
            return Json(abc, JsonRequestBehavior.AllowGet);
        }

        private static List<Type> GetSubClasses<T>()
        {
            return System.Reflection.Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }
        private static List<string> ActionNames(Type controllerType)
        {
            //return new ReflectedControllerDescriptor(controllerType).GetCanonicalActions().Select(x => x.ActionName).ToList();

            //return new ReflectedControllerDescriptor(controllerType).GetCanonicalActions().
            //    Where(
            //    x => !x.IsDefined(typeof(HttpPostAttribute), false) &&
            //    (((ReflectedActionDescriptor)x).MethodInfo.ReturnType == typeof(ActionResult) || ((ReflectedActionDescriptor)x).MethodInfo.ReturnType.IsSubclassOf(typeof(ActionResult)))
            //    ).Select(x => x.ActionName.ToLower()).ToList();

            ActionDescriptor[] adlist = new ReflectedControllerDescriptor(controllerType).GetCanonicalActions();
            List<string> rlist = new List<string>();
            foreach (var action in adlist)
            {

                // this is for skip actions with the httppostattribute
                if (action.IsDefined(typeof(System.Web.Mvc.HttpPostAttribute), false))
                    continue;
                ReflectedActionDescriptor rad = action as ReflectedActionDescriptor;
                if (rad != null)
                {
                    if (rad.MethodInfo.ReturnType == typeof(ActionResult) || rad.MethodInfo.ReturnType.IsSubclassOf(typeof(ActionResult)))
                        rlist.Add(rad.ActionName.ToLower());
                }

            }

            return rlist;

        }

        public class TestAssemblyValues
        {
            public string name { get; set; }
            public long key { get; set; }
        }

        public ActionResult SaveCustomReport(string ReportName, string ReportData, string ColumOrder, int ReportId = 0)
        {
            ReportName = ReportName.Replace("singleqoute", "'");
            var res = repo.SaveUserCustomReport(ReportName, ReportData, ColumOrder, ReportId);
            return Json(res);
        }
        public ActionResult testbulkinsert(int start, int end)
        {
            var res = repo.bulkinsert(start, end);


            //var GetResult = db.Clicks.Where(u => u.UserAgent != null).Select(u => u.UserAgent).Distinct().ToList();


            //var str = new StringBuilder();
            //for (int a = 0; a < GetResult.Count; a++)
            //{
            //    str.AppendFormat(",\"{0}\"", GetResult[a].ToString());

            //}
            //var reshd = str.ToString();


            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomTimezones()
        {
            int userID = repo.GetCurrentUserId();
            var result = repo.getTimeZoneList(userID);
            return Json(result);
        }
    }
}

