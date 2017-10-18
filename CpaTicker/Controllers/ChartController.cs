using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using WebMatrix.WebData;
using Newtonsoft.Json;
using System.Collections.Specialized;
using CpaTicker.Areas.admin.Models;

namespace CpaTicker.Controllers
{
    [Authorize]
    public class ChartController : Controller
    {
        public ChartController()
        {
            this.repo = new EFCpatickerRepository();
        }

        private ICpaTickerRepository repo;
        private CpaTickerDb db = new CpaTickerDb();
        private Customer CurrentCustomer
        {
            get
            {
                int customerid = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
                return db.Customers.Find(customerid);
            }
        }
        private int customerid = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);

        /******************** change to improve *********************/
        internal List<decimal> GrossCost(DateTime fromdate, DateTime todate, TimeZoneInfo info, int mode, int year)
        {
            fromdate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)fromdate, TimeZoneInfo.Local.Id, info.Id);
            todate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)todate, TimeZoneInfo.Local.Id, info.Id);
            List<decimal> result = new List<decimal>();

            var affiliateid = repo.GetCurrentUser().AffiliateId ?? 0;

            switch (mode)
            {
                case 0:

                    for (int i = 0; i < 24; i++)
                    {
                        result.Add(repo.GetGrossCost(fromdate.AddHours(i), fromdate.AddHours(i + 1).AddMilliseconds(-1), customerid, affiliateid));
                    }
                    break;

                case 1:
                    for (int i = 0; i < todate.Subtract(fromdate).Days + 1; i++)
                    {
                        result.Add(repo.GetGrossCost(fromdate.AddDays(i), fromdate.AddDays(i + 1).AddMilliseconds(-1), customerid, affiliateid));
                    }
                    break;
                case 2:
                    for (int i = 0; i < todate.Subtract(fromdate).Days / 7 + 1; i++)
                    {
                        DateTime nextdate = fromdate.AddDays((i + 1) * 6 + i);
                        if (nextdate > todate)
                            nextdate = todate;
                        result.Add(repo.GetGrossCost(fromdate.AddDays(i * 6 + i), nextdate.AddDays(1).AddMilliseconds(-1), customerid, affiliateid));
                    }
                    break;
                case 3:
                    for (int i = 1; i < 12; i++)
                    {
                        result.Add(repo.GetGrossCost(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(year, i, 1), TimeZoneInfo.Local.Id, info.Id), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(year, i + 1, 1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, info.Id), customerid, affiliateid));
                    }
                    break;
                case 4:
                    DateTime firstdate = repo.GetFirstOperation(customerid); //CPAHelper.GetFirstOperation(customerid);

                    for (int i = 0; i < DateTime.Now.Year - firstdate.Year + 1; i++)
                    {
                        result.Add(repo.GetGrossCost(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(firstdate, TimeZoneInfo.Local.Id, info.Id).AddYears(i), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(firstdate, TimeZoneInfo.Local.Id, info.Id).AddYears(i + 1).AddMilliseconds(-1), customerid, affiliateid));
                    }
                    break;
                default:
                    for (int i = 0; i < 24; i++)
                    {
                        result.Add(repo.GetGrossCost(todate.AddHours(i), todate.AddHours(i + 1).AddMilliseconds(-1), customerid, affiliateid));
                    }
                    break;

            }
            return result;
        }

        /******************** change to improve *********************/
        internal List<decimal> GrossRevenue(DateTime fromdate, DateTime todate, TimeZoneInfo info, int mode, int year)
        {
            fromdate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)fromdate, TimeZoneInfo.Local.Id, info.Id);
            todate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)todate, TimeZoneInfo.Local.Id, info.Id);
            //string revenue = "";
            List<decimal> result = new List<decimal>();
            int customerid = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
            int affiliateid = repo.GetCurrentUser().AffiliateId ?? 0;
            switch (mode)
            {
                case 0:
                    for (int i = 0; i < 24; i++)
                    {
                        result.Add(repo.GetGrossRevenue(fromdate.AddHours(i), fromdate.AddHours(i + 1).AddMilliseconds(-1), customerid, affiliateid));
                    }
                    break;
                case 1:
                    for (int i = 0; i < todate.Subtract(fromdate).Days + 1; i++)
                    {
                        result.Add(repo.GetGrossRevenue(fromdate.AddDays(i), fromdate.AddDays(i + 1).AddMilliseconds(-1), customerid, affiliateid));
                    }
                    break;
                case 2:
                    for (int i = 0; i < todate.Subtract(fromdate).Days / 7 + 1; i++)
                    {
                        DateTime nextdate = fromdate.AddDays((i + 1) * 6 + i);
                        if (nextdate > todate)
                            nextdate = todate;
                        result.Add(repo.GetGrossRevenue(fromdate.AddDays(i * 6 + i), nextdate.AddDays(1).AddMilliseconds(-1), customerid, affiliateid));
                    }
                    break;
                case 3:
                    for (int i = 1; i < 12; i++)
                    {
                        result.Add(repo.GetGrossRevenue(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(year, i, 1), TimeZoneInfo.Local.Id, info.Id), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(year, i + 1, 1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, info.Id), customerid, affiliateid));
                    }
                    result.Add(repo.GetGrossRevenue(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(year, 12, 1), TimeZoneInfo.Local.Id, info.Id), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(year + 1, 1, 1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, info.Id), customerid, affiliateid));
                    break;
                case 4:
                    DateTime firstdate = repo.GetFirstOperation(customerid); //CPAHelper.GetFirstOperation(customerid);

                    for (int i = 0; i < DateTime.Now.Year - firstdate.Year + 1; i++)
                    {
                        result.Add(repo.GetGrossRevenue(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(firstdate, TimeZoneInfo.Local.Id, info.Id).AddYears(i), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(firstdate, TimeZoneInfo.Local.Id, info.Id).AddYears(i + 1).AddMilliseconds(-1), customerid, affiliateid));
                    }
                    break;
                default:
                    for (int i = 0; i < 24; i++)
                    {
                        result.Add(repo.GetGrossRevenue(todate.AddHours(i), todate.AddHours(i + 1).AddMilliseconds(-1), customerid, affiliateid));
                    }
                    break;

            }
            return result;
        }

        /********* test to see performance in the int.parse *********/
        internal List<int> GetTrafficSummaryClicks(DateTime fromdate, DateTime todate, TimeZoneInfo info, int mode, int year)
        {
            fromdate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)fromdate, TimeZoneInfo.Local.Id, info.Id);
            todate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)todate, TimeZoneInfo.Local.Id, info.Id);
            int customerid = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);
            List<int> result = new List<int>();

            switch (mode)
            {
                case 0: // hourly
                    for (int i = 0; i < 24; i++)
                    {
                        //result.Add(CPAHelper.GetClicksPerHour(fromdate, i, customerid));
                        result.Add(repo.GetClicks(fromdate.AddHours(i), fromdate.AddHours(i + 1).AddMilliseconds(-1), customerid));
                    }
                    break;
                case 1: // daily

                    for (int i = 0; i < todate.Subtract(fromdate).Days + 1; i++)
                    {
                        result.Add(repo.GetClicks(fromdate.AddDays(i), fromdate.AddDays(i + 1).AddMilliseconds(-1), customerid));
                    }
                    break;

                case 2: // week


                    for (int i = 0; i < todate.Subtract(fromdate).Days / 7 + 1; i++)
                    {
                        DateTime nextdate = fromdate.AddDays((i + 1) * 6 + i);
                        if (nextdate > todate)
                            nextdate = todate;
                        result.Add(repo.GetClicks(fromdate.AddDays(i * 6 + i), nextdate.AddDays(1).AddMilliseconds(-1), customerid));
                    }
                    break;

                case 3: // month

                    for (int i = 1; i < 12; i++)
                    {
                        result.Add(repo.GetClicks(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(year, i, 1), TimeZoneInfo.Local.Id, info.Id), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(year, i + 1, 1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, info.Id), customerid));
                    }
                    result.Add(repo.GetClicks(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(year, 12, 1), TimeZoneInfo.Local.Id, info.Id), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(year + 1, 1, 1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, info.Id), customerid));
                    break;


                case 4: // year

                    DateTime firstdate = repo.GetFirstOperation(customerid);//CPAHelper.GetFirstOperation(customerid);

                    for (int i = 0; i < DateTime.Now.Year - firstdate.Year + 1; i++)
                    {
                        result.Add(repo.GetClicks(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(firstdate, TimeZoneInfo.Local.Id, info.Id).AddYears(i), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(firstdate, TimeZoneInfo.Local.Id, info.Id).AddYears(i + 1).AddMilliseconds(-1), customerid));
                    }
                    break;
                default:

                    for (int i = 0; i < todate.Subtract(fromdate).Days + 1; i++)
                    {
                        result.Add(repo.GetClicks(fromdate.AddDays(i), fromdate.AddDays(i + 1).AddMilliseconds(-1), customerid));
                    }
                    break;
            }
            return result;
        }

        /********* test to see performance in the int.parse *********/
        internal List<string> GetTrafficSummaryX(DateTime fromdate, DateTime todate, int mode, int year)
        {
            string dates = "";
            List<string> result;
            switch (mode)
            {
                case 0:

                    result = new List<string>();
                    for (int i = 0; i < 24; i++)
                    {
                        result.Add(i.ToString());
                    }
                    return result;
                case 1:

                    result = new List<string>();
                    for (int i = 0; i < todate.Subtract(fromdate).Days + 1; i++)
                    {
                        result.Add(fromdate.AddDays(i).ToString("MMM d"));
                    }
                    return result;

                case 2:

                    result = new List<string>();
                    for (int i = 0; i < todate.Subtract(fromdate).Days / 7 + 1; i++)
                    {
                        DateTime nextdate = fromdate.AddDays((i + 1) * 6 + i);
                        if (nextdate > todate)
                            nextdate = todate;

                        //dates += "'" + fromdate.AddDays(i * 6 + i).ToString("MMM d") + "-" + nextdate.Day.ToString() + "',";
                        result.Add(fromdate.AddDays(i * 6 + i).ToString("MMM d") + "-" + nextdate.Day.ToString());
                    }
                    return result;

                case 3:
                    result = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                    return result;

                case 4:
                    DateTimeOffset firstdate = repo.GetFirstOperation(customerid); //CPAHelper.GetFirstOperation(customerid);
                    result = new List<string>();
                    for (int i = 0; i < DateTime.Now.Year - firstdate.Year + 1; i++)
                    {
                        dates += "'" + (firstdate.Year + i).ToString() + "',";
                        result.Add((firstdate.Year + i).ToString());
                    }
                    return result;

                default:
                    result = new List<string>();
                    for (int i = 0; i < todate.Subtract(fromdate).Days + 1; i++)
                    {
                        result.Add(fromdate.AddDays(i).ToString("MMM d"));
                    }
                    return result;
            }
        }

        /********* test to see performance in the int.parse *********/
        internal List<int> GetTrafficSummaryConversions(DateTime fromdate, DateTime todate, TimeZoneInfo info, int mode, int year)
        {
            List<int> result = new List<int>();
            fromdate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)fromdate, TimeZoneInfo.Local.Id, info.Id);
            todate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)todate, TimeZoneInfo.Local.Id, info.Id);
            int customerid = CPAHelper.GetCustomerId(WebSecurity.CurrentUserId);

            switch (mode)
            {
                case 0:

                    for (int i = 0; i < 24; i++)
                    {
                        result.Add(CPAHelper.GetConversionsperHour(fromdate, i, customerid));
                    }
                    break;

                case 1:

                    for (int i = 0; i < todate.Subtract(fromdate).Days + 1; i++)
                    {
                        result.Add(repo.GetConversions(fromdate.AddDays(i), fromdate.AddDays(i + 1).AddMilliseconds(-1), customerid));
                    }
                    break;

                case 2:

                    for (int i = 0; i < todate.Subtract(fromdate).Days / 7 + 1; i++)
                    {
                        DateTime nextdate = fromdate.AddDays((i + 1) * 6 + i);
                        if (nextdate > todate)
                            nextdate = todate;
                        result.Add(repo.GetConversions(fromdate.AddDays(i * 6 + i), nextdate.AddDays(1).AddMilliseconds(-1), customerid));
                    }
                    break;
                case 3:

                    for (int i = 1; i < 12; i++)
                    {
                        result.Add(repo.GetConversions(new DateTime(year, i, 1), new DateTime(year, i + 1, 1).AddMilliseconds(-1), customerid));
                    }
                    break;

                case 4:
                    DateTime firstdate = repo.GetFirstOperation(customerid); // CPAHelper.GetFirstOperation(customerid);

                    for (int i = 0; i < DateTime.Now.Year - firstdate.Year + 1; i++)
                    {
                        result.Add(repo.GetConversions(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(firstdate, TimeZoneInfo.Local.Id, info.Id).AddYears(i), TimeZoneInfo.ConvertTimeBySystemTimeZoneId(firstdate, TimeZoneInfo.Local.Id, info.Id).AddYears(i + 1).AddMilliseconds(-1), customerid));
                    }
                    break;

                default:

                    for (int i = 0; i < todate.Subtract(fromdate).Days + 1; i++)
                    {
                        result.Add(repo.GetConversions(fromdate.AddDays(i), fromdate.AddDays(i + 1).AddMilliseconds(-1), customerid));
                    }
                    break;
            }
            return result;
        }

        //public /*JsonResult*/ ActionResult GetJsonData(DateTime fromdate, DateTime todate, TimeZoneInfo timezone, int mode, int year)
        //{
        //    var tc = new ChartOpts();
        //    tc.Xaxis = GetTrafficSummaryX(fromdate, todate, mode, year);
        //    tc.Serie1 = GetTrafficSummaryClicks(fromdate, todate, timezone, mode, year);
        //    tc.Serie2 = GetTrafficSummaryConversions(fromdate, todate, timezone, mode, year);
        //    //return Json(tc, JsonRequestBehavior.AllowGet);

        //    return Content(JsonConvert.SerializeObject(tc));
        //}

        public ActionResult GetTraffic(DateTime? fromdate, DateTime? todate, string timezone = "", int mode = 1, int year = 0)
        {
            if (fromdate == null)
                fromdate = DateTime.Today.Date.AddDays(-6);
            if (todate == null)
                todate = DateTime.Today.Date.AddDays(1).AddSeconds(-1);
            //else
            //    todate = todate.Value.AddDays(1).AddSeconds(-1); // add the whole current "todate" date
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);
            var tzi = repo.FindTimeZoneInfo(null, customer.TimeZone);


            if (year == 0)
                year = DateTime.Now.Year;

            //return GetJsonData(((DateTime)fromdate).Date, ((DateTime)todate).Date, info, mode, year);

            var tc = new ChartOpts();
            tc.Xaxis = GetTrafficSummaryX(fromdate.Value, todate.Value, mode, year);
            tc.Serie1 = new List<int>();
            tc.Serie2 = new List<int>();

            dynamic list = null;

            DateTime fdate = new DateTime(fromdate.Value.Ticks, DateTimeKind.Unspecified);
            DateTime tdate = new DateTime(todate.Value.Ticks, DateTimeKind.Unspecified);
            TimeSpan offset = tzi.GetUtcOffset(fdate);

            var ufdate = new DateTimeOffset(fdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(tdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            switch (mode)
            {
                case 0: // hourly
                    //DateTimeOffset dateAndOffset = new DateTimeOffset(fdate, offset);
                    //list = repo.HourlyReport(dateAndOffset.UtcDateTime, customer.CustomerId, offset.Hours, repo.GetCurrentUserId());

                    var report = repo.RunQuery<HourlyView>("EXEC [HourlyRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                                up.CustomerId, ufdate, ufdate.AddDays(1).AddMilliseconds(-1), tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, 
                                up.AffiliateId.HasValue ? up.AffiliateId.Value.ToString() : null, null, null, false);

                    list = from h in Enumerable.Range(0, 24)
                           join r in report on h equals r.Hour into dr
                           from r in dr.DefaultIfEmpty()
                           select new HourlyView
                           {
                               Hour = h,
                               Clicks = r == null ? 0 : r.Clicks,
                               Conversions = r == null ? 0 : r.Conversions,
                               Impressions = r == null ? 0 : r.Impressions,
                               Revenue = r == null ? 0 : r.Revenue,
                               Cost = r == null ? 0 : r.Cost,
                           };

                    break;
                case 1: // daily
                    //list = repo.DailyReport(fdate, tdate, customer.CustomerId, offset.Hours, repo.GetCurrentUserId());
                    //list = repo.RunQuery<DailyView>("", up.CustomerId, fdate, tdate, offset.Hours, repo.GetCurrentUserId());
                    list = repo.RunQuery<DailyView>("EXEC [Spark] {0}, {1}, {2}, {3}, {4}, {5}",
                                up.CustomerId, ufdate, utdate, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, up.AffiliateId);
                    break;
                //case 2: // week is more fast the other way
                //    var auxlist = repo.DailyReport(fdate, tdate, customer.CustomerId, offset.Hours);
                //    list = new List<DailyView>();
                //    int clicks = 0, conversions = 0, i = 0;
                //    foreach (var item in auxlist)
                //    {
                //        clicks += item.Clicks;
                //        conversions += item.Conversions;

                //        if ((i + 1) % 7 == 0)
                //        {
                //            list.Add(new DailyView() { Clicks = clicks, Conversions = conversions });
                //            clicks = 0;
                //            conversions = 0;
                //        }
                //        i++;
                //    }
                //    if (i % 7 != 0 || i == 0) // add last week
                //    {
                //        list.Add( new DailyView(){ Clicks = clicks, Conversions = conversions });
                //    }
                //    break;

            }
            if (!(mode > 1))
            {
                foreach (var item in list)
                {
                    tc.Serie1.Add(item.Clicks);
                    tc.Serie2.Add(item.Conversions);
                }
            }

            else
            {
                tc.Serie1 = GetTrafficSummaryClicks(fromdate.Value, todate.Value, tzi, mode, year);
                tc.Serie2 = GetTrafficSummaryConversions(fromdate.Value, todate.Value, tzi, mode, year);
            }
            return Content(JsonConvert.SerializeObject(tc));
        }

        public ActionResult GetRevenue(DateTime? fromdate, DateTime? todate, string timezone = "", int mode = 0, int year = 0)
        {
            if (fromdate == null)
                //fromdate = DateTime.Today.Date.AddDays(-6);
                fromdate = DateTime.Today.Date;
            if (todate == null)
                todate = DateTime.Today.Date.AddDays(1).AddSeconds(-1);
            //else
            //    todate = todate.Value.AddDays(1).AddSeconds(-1); // add the whole current "todate" date

            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);
            var tzi = repo.FindTimeZoneInfo(null, customer.TimeZone);


            if (year == 0)
                year = DateTime.Now.Year;

            //return GetJsonGross(((DateTime)fromdate).Date, ((DateTime)todate).Date, info, mode, year);

            var tc = new ChartGrossData();
            tc.Xaxis = GetTrafficSummaryX(fromdate.Value, todate.Value, mode, year);

            if (mode == 0) // hourly report
            {
                //fromdate = fromdate.HasValue ? fromdate : DateTime.Today;
                //DateTime fdate = new DateTime(fromdate.Value.Ticks, DateTimeKind.Unspecified);
                //var offset = tzi.GetUtcOffset(fdate);
                //DateTimeOffset dateAndOffset = new DateTimeOffset(fdate, offset);
                //var utctime = dateAndOffset.UtcDateTime;

                //var list = repo.HourlyReport(utctime, customer.CustomerId, offset.Hours, repo.GetCurrentUserId());

                DateTime fdate = new DateTime(fromdate.Value.Ticks, DateTimeKind.Unspecified);               
                var ufdate = new DateTimeOffset(fdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

                var report = repo.RunQuery<HourlyView>("EXEC [HourlyRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                                up.CustomerId, ufdate, ufdate.AddDays(1).AddMilliseconds(-1), tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId,
                                up.AffiliateId.HasValue ? up.AffiliateId.Value.ToString() : null, null, null, false);

                var list = from h in Enumerable.Range(0, 24)
                           join r in report on h equals r.Hour into dr
                           from r in dr.DefaultIfEmpty()
                           select new HourlyView
                           {
                               Hour = h,
                               Clicks = r == null ? 0 : r.Clicks,
                               Conversions = r == null ? 0 : r.Conversions,
                               Impressions = r == null ? 0 : r.Impressions,
                               Revenue = r == null ? 0 : r.Revenue,
                               Cost = r == null ? 0 : r.Cost,
                           };



                tc.Serie1 = new List<decimal>();
                tc.Serie2 = new List<decimal>();

                foreach (var item in list)
                {
                    tc.Serie1.Add(item.Cost);
                    tc.Serie2.Add(item.Revenue);
                }
            }

            else if (mode == 1)
            {
                #region DateTime Adjust

                DateTime fdate = new DateTime(fromdate.Value.Ticks, DateTimeKind.Unspecified);
                DateTime tdate = new DateTime(todate.Value.Ticks, DateTimeKind.Unspecified);
                //var offset = tzi.GetUtcOffset(fdate);

                var ufdate = new DateTimeOffset(fdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
                var utdate = new DateTimeOffset(tdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

                #endregion

                //var list = repo.DailyReport(fdate, tdate, customer.CustomerId, offset.Hours, repo.GetCurrentUserId());
                var list = repo.RunQuery<DailyView>("EXEC [Spark] {0}, {1}, {2}, {3}, {4}, {5}",
                                up.CustomerId, ufdate, utdate, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, up.AffiliateId);
                tc.Serie1 = new List<decimal>();
                tc.Serie2 = new List<decimal>();

                foreach (var item in list)
                {
                    tc.Serie1.Add(item.Cost);
                    tc.Serie2.Add(item.Revenue);
                }

            }

            else
            {
                tc.Serie1 = GrossCost(fromdate.Value, todate.Value, tzi, mode, year);
                tc.Serie2 = GrossRevenue(fromdate.Value, todate.Value, tzi, mode, year);
            }

            return Json(tc, JsonRequestBehavior.AllowGet);

        }

        //private JsonResult GetJsonGross(DateTime fromdate, DateTime todate, TimeZoneInfo timezone, int mode, int year)
        //{
        //    var tc = new ChartGrossData();
        //    tc.Xaxis = GetTrafficSummaryX(fromdate, todate, mode, year);

        //    //if (mode == 0) // hourly report
        //    //{
        //    //    var list = repo.HourlyReport(fromdate, repo.GetCurrentUser().CustomerId);
        //    //    var revenue = new List<double>();
        //    //    var cost = new List<double>();

        //    //    foreach (var item in list)
        //    //    {
        //    //        revenue.Add(item.Revenue);
        //    //        cost.Add(item.Cost);
        //    //    }

        //    //    tc.Serie1 = cost;
        //    //    tc.Serie2 = revenue;

        //    //}

        //    //else
        //    {
        //        tc.Serie1 = GrossCost(fromdate, todate, timezone, mode, year);
        //        tc.Serie2 = GrossRevenue(fromdate, todate, timezone, mode, year);
        //    }

        //    return Json(tc, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult CByCampaign(DateTime? fromdate, string timezone = "")
        {
            if (fromdate == null)
                fromdate = DateTime.Now.Date;

            TimeZoneInfo info = null;
            if (timezone != "")
                try
                {
                    info = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                }
                catch { }
            try
            {
                if (info == null)
                    info = TimeZoneInfo.FindSystemTimeZoneById(CurrentCustomer.TimeZone);
            }
            catch
            {
                info = TimeZoneInfo.Local;
            }

            return GetJsonCByCampaign((DateTime)fromdate, info);
        }

        private JsonResult GetJsonCByCampaign(DateTime fromdate, TimeZoneInfo timezone)
        {
            var results = new List<List<object>>();
            fromdate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(fromdate, TimeZoneInfo.Local.Id, timezone.Id);
            //string data = "";
            //List<Campaign> campaigns = CPAHelper.GetCampaigns(customerid);
            var user = repo.GetCurrentUser();
            List<Campaign> campaigns = repo.GetUserCampaigns(user).Where(c => c.Status == Status.Active).ToList();
            double totalcost = 0.0;
            List<double> plot = new List<double>();
            foreach (Campaign campaign in campaigns)
            {
                double clickcost = CPAHelper.GetClickCost(fromdate, fromdate.AddDays(1).AddMilliseconds(-1), user.CustomerId, campaign.CampaignId);
                double conversioncost = CPAHelper.GetConversionCost(fromdate, fromdate.AddDays(1).AddMilliseconds(-1), user.CustomerId, campaign.CampaignId);
                double impressioncost = CPAHelper.GetImpressionCost(fromdate, fromdate.AddDays(1).AddMilliseconds(-1), user.CustomerId, campaign.CampaignId);
                //data += conversionrevenue + ":" + clickrevenue + ":" + impressionrevenue;
                totalcost += clickcost + conversioncost + impressioncost;
                plot.Add(clickcost + conversioncost + impressioncost);
            }
            if (totalcost == 0.0)
                totalcost = 1.0;
            for (int i = 0; i < campaigns.Count; i++)
            { 
                double Revenue = ((plot[i] / totalcost) * 100);
                if (Revenue > 0.1){results.Add(new List<object>(new object[] { campaigns[i].CampaignName, ((plot[i] / totalcost) * 100) }));}
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RByCampaign(DateTime? fromdate, string timezone = "")
        {
            if (fromdate == null)
                fromdate = DateTime.Now.Date;

            TimeZoneInfo info = null;
            if (timezone != "")
                try
                {
                    info = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                }
                catch { }
            try
            {
                if (info == null)
                    info = TimeZoneInfo.FindSystemTimeZoneById(CurrentCustomer.TimeZone);
            }
            catch
            {
                info = TimeZoneInfo.Local;
            }

            return GetJsonRByCampaign((DateTime)fromdate, info);
        }

        private JsonResult GetJsonRByCampaign(DateTime fromdate, TimeZoneInfo timezone)
        {
            var results = new List<List<object>>();
            fromdate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(fromdate, TimeZoneInfo.Local.Id, timezone.Id);
            //string data = "";
            //List<NameValueCollection> result = new List<NameValueCollection>();
            //List<Campaign> campaigns = CPAHelper.GetCampaigns(customerid);
            List<Campaign> campaigns = repo.GetUserCampaigns().Where(c => c.Status == Status.Active).ToList();
            double totalrevenue = 0.0;
            List<double> plot = new List<double>();
            int affiliateid = repo.GetCurrentUser().AffiliateId ?? 0;
            foreach (Campaign campaign in campaigns)
            {
                double clickrevenue = repo.GetClickRevenue(fromdate, fromdate.AddDays(1).AddMilliseconds(-1), customerid, campaign.CampaignId, affiliateid);
                double conversionrevenue = repo.GetConversionRevenue(fromdate, fromdate.AddDays(1).AddMilliseconds(-1), customerid, campaign.CampaignId, affiliateid);
                double impressionrevenue = repo.GetImpressionRevenue(fromdate, fromdate.AddDays(1).AddMilliseconds(-1), customerid, campaign.CampaignId, affiliateid);
                //data += conversionrevenue + ":" + clickrevenue + ":" + impressionrevenue;
                totalrevenue += clickrevenue + conversionrevenue + impressionrevenue;
                plot.Add(clickrevenue + conversionrevenue + impressionrevenue);
            }
            if (totalrevenue == 0.0)
                totalrevenue = 1.0;


            for (int i = 0; i < campaigns.Count; i++)
            {
                //data += "['" + campaigns[i].CampaignName.Replace("'", "\\'") + "'," + ((plot[i] / totalrevenue) * 100).ToString("f2") + "],";
                double Revenue = ((plot[i] / totalrevenue) * 100);
                if (Revenue > 0.1) { results.Add(new List<object>(new object[] { campaigns[i].CampaignName, ((plot[i] / totalrevenue) * 100) })); }
                
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }



        public class ChartOpts
        {
            public List<string> Xaxis { get; set; }
            public List<int> Serie1 { get; set; }
            public List<int> Serie2 { get; set; }
        }

        public class ChartGrossData
        {
            public List<string> Xaxis { get; set; }
            public List<decimal> Serie1 { get; set; }
            public List<decimal> Serie2 { get; set; }
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}