using CpaTicker.Areas.admin.Models;
using CpaTicker.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using WURFL;
using WURFL.Aspnet.Extensions.Config;

namespace CpaTicker.Areas.admin.Classes.Helpers
{
    public class EFCpatickerRepository : ICpaTickerRepository
    {
        //private string page_name = "";
        private string GB_timezone = "";
        private string GB_viewdata = "";

        private string GB_fromdate = "";
        private string GB_todate = "";
        private string GB_dataview = "";
        private int GB_customerid = 0;
        private int GB_userid = 0;
        private string GB_connectionid = "";



        /// <summary>
        /// Auxiliar feauture just return an action based on the campaign data
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        //public Action GetDefaultAction(Campaign campaign)
        //{
        //    return new Action
        //    {
        //        Name = "default",
        //        CampaignId = campaign.Id,
        //        //TrackingCode = System.Web.HttpUtility.HtmlEncode(TrackingCode(campaign.CustomerId, campaign)),
        //        Type = ConversionType.Sale,

        //        Revenue = campaign.Revenue,
        //        RevenuePercent = campaign.RevenuePercent,
        //        RevenueType = campaign.RevenueType,

        //        Payout = campaign.Payout,
        //        PayoutPercent = campaign.PayoutPercent,
        //        PayoutType = campaign.PayoutType,

        //        TrackingType = campaign.TrackingType
        //    };
        //}


        public IQueryable<Click> ClickQuery(DateTime ufdate, DateTime utdate, UserProfile up, int[] affs, int? cp, string ct)
        {
            // raw click query

            var _af = (from i in (affs ?? new int[] { }) select i).AsQueryable();
            bool b = _af.Count() == 0;


            var clicksquery = (from c in db.Clicks
                                   .Where(c => c.CustomerId == up.CustomerId
                                       && c.bot == 0
                                       && ufdate <= c.ClickDate && c.ClickDate <= utdate
                                       && (b || _af.Contains(c.AffiliateId)) // affiliate filter
                                       && (ct == null || c.Country == ct))
                                    .GroupBy(c => c.TransactionId)
                                    .Select(g => g.FirstOrDefault()) // counting distinct transactions ids
                               join ca in db.Campaigns
                                    .Where(c => c.CustomerId == up.CustomerId
                                        && c.Status == Status.Active
                                        && !up.HiddenCampaigns.Select(h => h.CampaignId).Contains(c.Id)
                                        && (!cp.HasValue || c.CampaignId == cp.Value)) // campaign filter
                               on c.CampaignId equals ca.CampaignId
                               select c);

            return clicksquery;
        }

        public IQueryable<Conversion> ConversionQuery(DateTime ufdate, DateTime utdate, UserProfile up, int[] affs, int? cp, string ct, int conversionstatus = 1)
        {

            var _af = from i in (affs ?? new int[] { }) select i;
            bool b = _af.Count() == 0;
            // raw conversion query
            return (from c in db.Conversions
                    .Where(c => c.CustomerId == up.CustomerId
                             && c.Status == conversionstatus
                             && ufdate <= c.ConversionDate && c.ConversionDate <= utdate
                             && (b || _af.Contains(c.AffiliateId)) // affiliate filter
                             && (ct == null || c.Country == ct)) // country filter
                    join ca in db.Campaigns
                         .Where(c => c.CustomerId == up.CustomerId
                             && c.Status == Status.Active
                             && !up.HiddenCampaigns.Select(h => h.CampaignId).Contains(c.Id)
                             && (!cp.HasValue || c.CampaignId == cp.Value)) // campaign filter
                    on c.CampaignId equals ca.CampaignId
                    select c);
        }

        public IQueryable<Impression> ImpressionQuery(DateTime ufdate, DateTime utdate, UserProfile up, int[] affs, int? cp, string ct)
        {
            // raw impressions query
            var _af = from i in (affs ?? new int[] { }) select i;
            bool b = _af.Count() == 0;

            return (from c in db.Impressions.Where(c => c.CustomerId == up.CustomerId
                                           && ufdate <= c.ImpressionDate && c.ImpressionDate <= utdate
                                           && (b || _af.Contains(c.AffiliateId)) // affiliate filter
                                           && (ct == null || c.Country == ct)) // country filter
                    join ca in db.Campaigns
                         .Where(c => c.CustomerId == up.CustomerId
                             && c.Status == Status.Active
                             && !up.HiddenCampaigns.Select(h => h.CampaignId).Contains(c.Id)
                             && (!cp.HasValue || c.CampaignId == cp.Value)) // campaign filter
                    on c.CampaignId equals ca.CampaignId
                    select c);
        }


        public IQueryable<Banner> GetBannersByCampaign(int campaignid, int customerid = 0)
        {
            if (customerid == 0)
                customerid = GetCurrentCustomerId();
            return db.Banners.Where(b => b.CampaignId == campaignid && b.CustomerId == customerid);
        }

        public Ticker FindTicker(int id)
        {
            return db.Tickers.Find(id);
        }

        public void AddURL(URL url)
        {
            db.URLs.Add(url);
            db.SaveChanges();
        }
        public void AddPAGE(PAGE page)
        {
            db.PAGEs.Add(page);
            db.SaveChanges();
        }
        public void EditURL(URL url)
        {
            db.Entry(url).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void EditPAGE(PAGE page)
        {
            db.Entry(page).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }


        public Campaign FindCampaign(int campaignid)
        {
            return db.Campaigns.Find(campaignid);
        }

        public URL FindURL(int urlid)
        {
            return db.URLs.Find(urlid);
        }

        public PAGE FindPAGE(int id)
        {
            return db.PAGEs.Find(id);
        }

        public IQueryable<URL> GetCampaignURLs(int campaignid)
        {
            return db.URLs.Where(a => a.CampaignId == campaignid);
        }


        public IQueryable<PAGE> GetCustomerPage(int CustomerId)
        {
            return db.PAGEs.Where(a => a.CustomerID == CustomerId);
        }

        public IQueryable<PAGE> GetCustomerPageByStatus(int CustomerId, PageStatus status)
        {
            return db.PAGEs.Where(a => a.CustomerID == CustomerId && a.Status == status);
        }

        public Domain GetDomain(int domainid)
        {
            //return db.Domains.Single(d => d.Id == domainid);
            return db.Domains.Find(domainid);
        }

        public IQueryable<Country> GetCountries()
        {
            return db.Countries.OrderBy(c => c.Name);
        }

        public IQueryable<Affiliate> GetCustomerActiveAffiliates(int customerid = 0)
        {
            if (customerid == 0)
                customerid = this.GetCurrentUser().CustomerId;
            return db.Affiliates.Where(a => a.CustomerId == customerid && a.Status == AffiliateStatus.Active);
        }

        /*====================================== Reports Start =================================================*/

        public TimeZoneInfo FindTimeZoneInfo(string timezone, string customerTimeZone)
        {
            TimeZoneInfo result;
            if (!String.IsNullOrEmpty(timezone))
            {
                try
                {
                    result = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                    return result;
                }
                catch { }
            }
            try
            {
                result = TimeZoneInfo.FindSystemTimeZoneById(customerTimeZone);
            }
            catch
            {
                result = TimeZoneInfo.Local;
            }

            return result;
        }

        public DateTime GetFirstOperation(int customerid)
        {
            //DateTime result = new DateTime(DateTime.Now.Year, 1, 1);

            int year = DateTime.Today.Year;

            //var firstclick = db.Clicks.Where(c => c.CustomerId == customerid).OrderBy(c => c.ClickDate).FirstOrDefault();
            //var firstimpression = db.Impressions.Where(i => i.CustomerId == customerid).OrderBy(i => i.ImpressionDate).FirstOrDefault();
            //var firstconversion = db.Conversions.Where(c => c.CustomerId == customerid).OrderBy(c => c.ConversionDate).FirstOrDefault();

            //if (firstclick != null)
            //    if (result > firstclick.ClickDate)
            //        result = firstclick.ClickDate;

            //if (firstimpression != null)
            //    if (result > firstimpression.ImpressionDate)
            //        result = firstimpression.ImpressionDate;

            //if (firstconversion != null)
            //    if (result > firstconversion.ConversionDate)
            //        result = firstconversion.ConversionDate;

            var firstclick = db.Clicks.Where(c => c.CustomerId == customerid).FirstOrDefault();
            var firstimpression = db.Impressions.Where(i => i.CustomerId == customerid).FirstOrDefault();
            var firstconversion = db.Conversions.Where(c => c.CustomerId == customerid).FirstOrDefault();


            if (firstclick != null)
            {
                if (firstclick.ClickDate.Year < year)
                {
                    year = firstclick.ClickDate.Year;
                }
            }
            if (firstimpression != null)
            {
                if (firstimpression.ImpressionDate.Year < year)
                {
                    year = firstimpression.ImpressionDate.Year;
                }
            }
            if (firstconversion != null)
            {
                if (firstconversion.ConversionDate.Year < year)
                {
                    year = firstconversion.ConversionDate.Year;
                }
            }

            return new DateTime(year, 1, 1);
        }

        public List<T> ExecuteQuery<T>(string query, params object[] parameters)
        {
            // call to list to avoid deferred execution causing parameters problems
            return db.Database.SqlQuery<T>(query, parameters).ToList<T>();
        }

        public DbRawSqlQuery<T> ExecuteQuery<T>(string query)
        {
            return db.Database.SqlQuery<T>(query);
        }

        public IQueryable<Impression> Impressions()
        {
            return db.Impressions;
        }

        public IQueryable<Affiliate> Affiliates()
        {
            return db.Affiliates;
        }

        public DbRawSqlQuery ExecuteQuery(Type type, string query, params object[] parameters)
        {
            return db.Database.SqlQuery(type, query, parameters);
        }

        public dynamic CTRReport(DateTime fromdate, DateTime todate, int customerid, int userid, string af, int? cp, int? ct)
        {
            return db.Database.SqlQuery(typeof(CTRView), "EXEC [CTRReport] {0}, {1}, {2}, {3}, {4}, {5}, {6}", customerid, fromdate, todate, userid, af, cp, ct);
        }

        public dynamic CoversionStatusReport(DateTime fromdate, DateTime todate, int customerid, int userid, string affiliateid = null, int? campaignid = null, int? countryid = null)
        {
            return db.Database.SqlQuery(typeof(ConversionStatusView), "EXEC [ConversionStatusReport] {0}, {1}, {2}, {3}, {4}, {5}, {6}", customerid, fromdate, todate, userid, affiliateid, campaignid, countryid);
        }

        public dynamic TrafficReport(DateTime fromdate, DateTime todate, int customerid, int userid, string affiliateid = null, int? campaignid = null, int? countryid = null)
        {
            return db.Database.SqlQuery(typeof(TrafficView), "EXEC [TrafficReport] {0}, {1}, {2}, {3}, {4}, {5}, {6}", customerid, fromdate, todate, userid, affiliateid, campaignid, countryid);
        }

        //public dynamic ConversionReport(DateTime fromdate, DateTime todate, int customerid, string affiliateid = null, int? campaignid = null, int? countryid = null)
        //{
        //    return db.Database.SqlQuery(typeof(ConversionView), "EXEC [ConversionReport] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, affiliateid, campaignid, countryid);
        //}

        public dynamic DailyReport(DateTime fromdate, DateTime todate, int customerid, int offset, int userid, string affiliateid = null, int? campaignid = null, int? countryid = null)
        {
            return db.Database.SqlQuery(typeof(DailyView), "EXEC [DailyReport] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", customerid, fromdate, todate, offset, userid, affiliateid, campaignid, countryid);
        }

        //public dynamic AffiliatesReport(DateTime fromdate, DateTime todate, int customerid, string affiliateid = null, int? campaignid = null, int? countryid = null)
        //{
        //    return db.Database.SqlQuery(typeof(AffiliateView), "EXEC [AffiliatesReport] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, affiliateid, campaignid, countryid);
        //}

        //public dynamic AffiliatesSimpleReport(DateTime fromdate, DateTime todate, int customerid, string af, int? cp, int? ct)
        //{
        //    return db.Database.SqlQuery(typeof(AffiliateView), "EXEC [AffiliatesSimpleReport] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, af, cp, ct);
        //}

        //public dynamic OfferReport(DateTime fromdate, DateTime todate, int customerid, string affiliateid = null, int? campaignid = null, int? countryid = null)
        //{
        //    return db.Database.SqlQuery(typeof(OfferView), "EXEC [OfferReport] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, affiliateid, campaignid, countryid);
        //}

        //public dynamic OfferSimpleReport(DateTime fromdate, DateTime todate, int customerid, string af, int? cp, int? ct)
        //{
        //    return db.Database.SqlQuery(typeof(OfferView), "EXEC [OfferSimpleReport] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, af, cp, ct);
        //}

        //public dynamic OfferWithURLReport(DateTime fromdate, DateTime todate, int customerid, string affiliateid = null, int? campaignid = null, int? countryid = null)
        //{
        //    return db.Database.SqlQuery(typeof(OfferView), "EXEC [OfferWithURLReport] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, affiliateid, campaignid, countryid);
        //}

        //public dynamic OfferSimpleWithURLReport(DateTime fromdate, DateTime todate, int customerid, string af, int? cp, int? ct)
        //{
        //    return db.Database.SqlQuery(typeof(OfferView), "EXEC [OfferSimpleWithURLReport] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, af, cp, ct);
        //}

        public dynamic AdCampaignReport(DateTime fromdate, DateTime todate, int customerid, int userid, string affiliateid = null, int? campaignid = null, int? countryid = null)
        {
            return db.Database.SqlQuery(typeof(AdCampaignView), "EXEC [AdCampaignReport] {0}, {1}, {2}, {3}, {4}, {5}, {6}", customerid, fromdate, todate, userid, affiliateid, campaignid, countryid);
        }

        //public IQueryable<Click> ClicksLogs(DateTime fromdate, DateTime todate, int customerid, int? affiliateid)
        //{
        //    var hidden = this.GetCurrentUser().HiddenCampaigns.Select(c => c.Campaign.CampaignId);
        //    return db.Clicks.Where(cl => cl.ClickDate < todate && cl.ClickDate > fromdate
        //        && cl.CustomerId == customerid
        //        && (!affiliateid.HasValue || cl.AffiliateId == affiliateid.Value)
        //        && !cl.UserAgent.Contains("bot")
        //        && !hidden.Contains(cl.CampaignId)
        //        );
        //}


        public IEnumerable<CpaTicker.Controllers.ReportsController.InnerLogDisp> ClicksLogs(string Id, DateTime fromdate, DateTime todate, int customerid, int? affiliateid)
        {
            var up = GetCurrentUser();
            var customer = GetCurrentCustomer(up.CustomerId);

            var tzi = FindTimeZoneInfo(Id, customer.TimeZone);
            var hidden = this.GetCurrentUser().HiddenCampaigns.Select(c => c.Campaign.CampaignId);
            //var clicks = db.Clicks.Where(cl => cl.ClickDate < todate && cl.ClickDate > fromdate
            //    && cl.CustomerId == customerid
            //    && (!affiliateid.HasValue || cl.AffiliateId == affiliateid.Value)
            //    && !cl.UserAgent.Contains("bot")
            //    && !hidden.Contains(cl.CampaignId)
            //    ).ToList();
            var result = (from i in db.Clicks.Where(cl => cl.ClickDate < todate && cl.ClickDate > fromdate && cl.CustomerId == customerid
                                                    && (!affiliateid.HasValue || cl.AffiliateId == affiliateid.Value) && cl.bot == 0 && !hidden.Contains(cl.CampaignId)).AsEnumerable()
                          join d in db.DeviceInfo on i.UserAgentId equals d.Id into devieinfo
                          from di in devieinfo.DefaultIfEmpty()
                          select new CpaTicker.Controllers.ReportsController.InnerLogDisp
                          {
                              CampaignId = i.CampaignId,
                              AffiliateId = i.AffiliateId,
                              BannerId = i.BannerId,
                              ClickDate = TimeZoneInfo.ConvertTimeFromUtc(i.ClickDate, tzi).ToString("G"),
                              //TimeZoneInfo.ConvertTimeFromUtc(i.ClickDate, tzi).ToString("G"),
                              // ClickDate = new DateTimeOffset(i.ClickDate.Ticks, -tzi.GetUtcOffset(DateTime.Now)).UtcDateTime,
                              //new DateTimeOffset(i.ClickDate.Ticks, -tzi.GetUtcOffset(DateTime.Now)).UtcDateTime,
                              UserAgent = i.UserAgent,
                              IPAddress = i.IPAddress,
                              Referrer = i.Referrer,
                              Source = i.Source,
                              TransactionId = i.TransactionId,
                              Country = i.Country,
                              Cost = i.Cost,
                              Revenue = i.Revenue,
                              DeviceId = (di != null) ? di.DeviceId : "",
                              IsSmartphone = (di != null) ? di.IsSmartphone : false,
                              IsiOS = (di != null) ? di.IsiOS : false,
                              IsAndroid = (di != null) ? di.IsAndroid : false,
                              OS = (di != null) ? di.OS : "",
                              Browser = (di != null) ? di.Browser : "",
                              Device_os = (di != null) ? di.Device_os : "",
                              Pointing_method = (di != null) ? di.Pointing_method : "",
                              Is_tablet = (di != null) ? di.Is_tablet : false,
                              Model_name = (di != null) ? di.Model_name : "",
                              Device_os_version = (di != null) ? di.Device_os_version : "",
                              Is_wireless_device = (di != null) ? di.Is_wireless_device : false,
                              Brand_name = (di != null) ? di.Brand_name : "",
                              Marketing_name = (di != null) ? di.Marketing_name : "",
                              Is_assign_phone_number = (di != null) ? di.Is_assign_phone_number : false,
                              Xhtmlmp_mime_type = (di != null) ? di.Xhtmlmp_mime_type : "",
                              Xhtml_support_level = (di != null) ? di.Xhtml_support_level : "",
                              Resolution_height = (di != null) ? di.Resolution_height : "",
                              Resolution_width = (di != null) ? di.Resolution_width : "",
                              Canvas_support = (di != null) ? di.Canvas_support : "",
                              Viewport_width = (di != null) ? di.Viewport_width : "",
                              Html_preferred_dtd = (di != null) ? di.Html_preferred_dtd : "",
                              Isviewport_supported = (di != null) ? di.Isviewport_supported : false,
                              Ismobileoptimized = (di != null) ? di.Ismobileoptimized : false,
                              Isimage_inlining = (di != null) ? di.Isimage_inlining : false,
                              Ishandheldfriendly = (di != null) ? di.Ishandheldfriendly : false,
                              Is_smarttv = (di != null) ? di.Is_smarttv : false,
                              Isux_full_desktop = (di != null) ? di.Isux_full_desktop : false

                          }).OrderBy(u => u.ClickDate);
            return result;
        }
        public dynamic HourlyReport(DateTime fromdate, int customerid, int offset, int userid, string affiliateid = null, int? campaignid = null, int? countryid = null)
        {
            return db.Database.SqlQuery(typeof(HourlyView), "EXEC [HourlyReport] {0}, {1}, {2}, {3}, {4}, {5}, {6}", customerid, fromdate, offset, userid, affiliateid, campaignid, countryid);
        }

        /// <summary>
        /// Daily report shows all days
        /// </summary>
        /// <param name="fdate"></param>
        /// <param name="tdate"></param>
        /// <param name="up"></param>
        /// <param name="tzi"></param>
        /// <returns></returns>
        public IEnumerable<DailyView> DailyReport(DateTime fdate, DateTime tdate, UserProfile up, TimeZoneInfo tzi)
        {
            var ufdate = new DateTimeOffset(fdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(tdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            int[] af = null;
            if (up.AffiliateId.HasValue)
            {
                af = GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                //af = new int[] { up.AffiliateId.Value };
            }

            // daily clicks
            var dailyclicks = (from c in ClickQuery(ufdate, utdate, up, af, null, null)//.AsEnumerable()
                               group c by DbFunctions.TruncateTime(c.ClickDate) /*new
                              {
                                  Date = TimeZoneInfo.ConvertTimeFromUtc(c.ClickDate, tzi).ToString("MM/dd/yyyy"),
                              }*/
                                   into dc
                               select new
                               {
                                   Date = dc.Key,//.Date,
                                   Clicks = dc.Count(),
                                   Cost = dc.Sum(c => c.Cost),
                                   Revenue = dc.Sum(c => c.Revenue),
                               }).ToList();

            // daily conversions
            var dailyconversions = (from c in ConversionQuery(ufdate, utdate, up, af, null, null).AsEnumerable()
                                    group c by new
                                    {
                                        Date = TimeZoneInfo.ConvertTimeFromUtc(c.ConversionDate, tzi).ToString("MM/dd/yyyy"),
                                    } into dc
                                    select new
                                    {
                                        Date = dc.Key.Date,
                                        Conversions = dc.Count(),
                                        Cost = dc.Sum(c => c.Cost),
                                        Revenue = dc.Sum(c => c.Revenue),
                                    }).ToList();

            // daily impressions
            var dailyimpressions = (from c in ImpressionQuery(ufdate, utdate, up, af, null, null).AsEnumerable()
                                    group c by new
                                    {
                                        Date = TimeZoneInfo.ConvertTimeFromUtc(c.ImpressionDate, tzi).ToString("MM/dd/yyyy"),
                                    } into dc
                                    select new
                                    {
                                        Date = dc.Key.Date,
                                        Impressions = dc.Count(),
                                        Cost = dc.Sum(c => c.Cost),
                                        Revenue = dc.Sum(c => c.Revenue),
                                    }).ToList();

            var count = (tdate - fdate).Days + 1;

            var days = new List<string>();
            for (int i = 0; i < count; i++)
            {
                days.Add(fdate.AddDays(i).ToString("MM/dd/yyyy"));
            }

            //var report = from d in days
            //             join c in dailyclicks on d equals c.Date into dc
            //             from c in dc.DefaultIfEmpty()
            //             join co in dailyconversions on d equals co.Date into dco
            //             from co in dco.DefaultIfEmpty()
            //             join i in dailyimpressions on d equals i.Date into di
            //             from i in di.DefaultIfEmpty()
            //             select new DailyView
            //             {
            //                 Clicks = c == null ? 0 : c.Clicks,
            //                 Conversions = co == null ? 0 : co.Conversions,
            //                 Revenue = (c == null ? 0 : c.Revenue)
            //                            + (co == null ? 0 : co.Revenue)
            //                            + (i == null ? 0 : i.Revenue),

            //             };

            //return report;

            return null;
        }

        /// <summary>
        /// Hourly report shows all 24 hours
        /// </summary>
        /// <param name="fdate"></param>
        /// <param name="up"></param>
        /// <param name="tzi"></param>
        /// <returns></returns>
        public IEnumerable<HourlyView> HourlyReport(DateTime fdate, UserProfile up, TimeZoneInfo tzi)
        {
            //var utctime = TimeZoneInfo.ConvertTimeToUtc(fdate);
            var utctime = new DateTimeOffset(fdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            int[] af = null;
            if (up.AffiliateId.HasValue)
            {
                af = new int[] { up.AffiliateId.Value };
            }

            var hourlyclicks = from c in ClickQuery(utctime, utctime.AddDays(1), up, af, null, null).AsEnumerable()
                               group c by new
                               {
                                   Hour = TimeZoneInfo.ConvertTimeFromUtc(c.ClickDate, tzi).Hour,
                               } into dc

                               select new
                               {
                                   Hour = dc.Key.Hour,
                                   Clicks = dc.Count(),
                                   Cost = dc.Sum(c => c.Cost),
                                   Revenue = dc.Sum(c => c.Revenue),
                               };

            var hourlyconversions = from c in ConversionQuery(utctime, utctime.AddDays(1), up, af, null, null).AsEnumerable()
                                    group c by new
                                    {
                                        Hour = TimeZoneInfo.ConvertTimeFromUtc(c.ConversionDate, tzi).Hour,
                                    } into dc

                                    select new
                                    {
                                        Hour = dc.Key.Hour,
                                        Conversions = dc.Count(),
                                        Cost = dc.Sum(c => c.Cost),
                                        Revenue = dc.Sum(c => c.Revenue),
                                    };

            var hourlyimpressions = from c in ImpressionQuery(utctime, utctime.AddDays(1), up, af, null, null).AsEnumerable()
                                    group c by new
                                    {
                                        Hour = TimeZoneInfo.ConvertTimeFromUtc(c.ImpressionDate, tzi).Hour,
                                    } into dc

                                    select new
                                    {
                                        Hour = dc.Key.Hour,
                                        Impressions = dc.Count(),
                                        Cost = dc.Sum(c => c.Cost),
                                        Revenue = dc.Sum(c => c.Revenue),
                                    };

            var report = from h in Enumerable.Range(0, 24)

                         join c in hourlyclicks on h equals c.Hour into dc
                         from c in dc.DefaultIfEmpty()

                         join co in hourlyconversions on h equals co.Hour into dco
                         from co in dco.DefaultIfEmpty()

                         join i in hourlyimpressions on h equals i.Hour into di
                         from i in di.DefaultIfEmpty()
                         orderby h
                         select new HourlyView
                         {
                             Hour = h,

                             Clicks = c == null ? 0 : c.Clicks,
                             Conversions = co == null ? 0 : co.Conversions,
                             Impressions = i == null ? 0 : i.Impressions,
                             Cost = (c == null ? 0 : c.Cost)
                                     + (co == null ? 0 : co.Cost)
                                     + (i == null ? 0 : i.Cost),
                             Revenue = (c == null ? 0 : c.Revenue)
                                     + (co == null ? 0 : co.Revenue)
                                     + (i == null ? 0 : i.Revenue),

                         };

            return report;

        }
        /*====================================== Reports End =================================================*/

        /*====================================== EmployeeIPs =================================================*/

        public void DeleteEmployeeIp(string ip)
        {
            db.EmployeeIPs.Remove(db.EmployeeIPs.Find(GetCurrentUser().CustomerId, ip));
            db.SaveChanges();
        }

        public void AddEmployeeIp(EmployeeIP eip)
        {
            db.EmployeeIPs.Add(eip);
            db.SaveChanges();
        }

        public IQueryable<EmployeeIP> GetEmployeeIpList(int customerid = 0)
        {
            if (customerid == 0)
                customerid = GetCurrentUser().CustomerId;
            return db.EmployeeIPs.Where(e => e.CustomerId == customerid);

        }

        public bool IsEmployeeIp(string ip, int customerid = 0)
        {
            if (customerid == 0)
                customerid = GetCurrentUser().CustomerId;
            return db.EmployeeIPs.Find(customerid, ip) != null;

        }

        /*====================================== EmployeeIPs =================================================*/

        /*========================================== this is obsolete the queries are done by SP Chart ===================================================*/

        public double GetClickRevenue(DateTime FromDate, DateTime ToDate, int customerid, int campaignid, int affiliateid = 0)
        {
            return db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == campaignid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate && (affiliateid == 0 || cl.AffiliateId == affiliateid)).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        }

        public double GetConversionRevenue(DateTime FromDate, DateTime ToDate, int customerid, int campaignid, int affiliateid = 0)
        {
            return db.Conversions.Where(co => co.CustomerId == customerid && co.CampaignId == campaignid && co.ConversionDate >= FromDate && co.ConversionDate < ToDate && co.Status == 1 && (affiliateid == 0 || co.AffiliateId == affiliateid)).Sum(co => (double?)co.Revenue) ?? 0.0;
        }

        public double GetImpressionRevenue(DateTime FromDate, DateTime ToDate, int customerid, int campaignid, int affiliateid = 0)
        {
            return db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == campaignid && im.ImpressionDate >= FromDate & im.ImpressionDate < ToDate && (affiliateid == 0 || im.AffiliateId == affiliateid)).Sum(im => (double?)im.Revenue) ?? 0.0;
        }

        public int GetConversions(DateTime FromDate, DateTime ToDate, int customerid, int campaignid = 0)
        {
            int? affiliateid = GetCurrentUser().AffiliateId;
            var hidden = GetCurrentUser().HiddenCampaigns.Select(c => c.Campaign.CampaignId);
            if (campaignid == 0)
            {
                return db.Conversions.Count(co => co.CustomerId == customerid
                    && !hidden.Contains(co.CampaignId)
                    && co.ConversionDate >= FromDate
                    && co.ConversionDate <= ToDate &&
                    //filter by conversion status and if the affiliate in case is not a customer
                    co.Status == 1 && (affiliateid == null || co.AffiliateId == affiliateid) &&
                     // show only active campaigns
                     db.Campaigns.Where(c => c.Status == Status.Active && c.CustomerId == customerid).Select(c => c.CampaignId).Contains(co.CampaignId));
            }
            else
            {
                return db.Conversions.Count(co => co.CustomerId == customerid && co.ConversionDate >= FromDate
                    && co.ConversionDate <= ToDate && co.CampaignId == campaignid && co.Status == 1
                    && (affiliateid == null || co.AffiliateId == affiliateid)
                    && !hidden.Contains(co.CampaignId)
                    );
            }
        }

        public int GetClicks(DateTime FromDate, DateTime ToDate, int customerid, int campaignid = 0)
        {

            int? affiliateid = GetCurrentUser().AffiliateId;
            var hidden = GetCurrentUser().HiddenCampaigns.Select(c => c.Campaign.CampaignId);

            if (campaignid == 0)
            {
                return db.Clicks.Count(cl => cl.CustomerId == customerid
                    && cl.ClickDate >= FromDate && cl.ClickDate <= ToDate &&
                    // if the user is and affiliate then filter by affiliate
                    (affiliateid == null || cl.AffiliateId == affiliateid) &&
                    // filter by actives campaigns
                    db.Campaigns.Where(c => c.Status == Status.Active && c.CustomerId == customerid).Select(c => c.CampaignId).Contains(cl.CampaignId)
                    && !hidden.Contains(cl.CampaignId)
                );
            }
            else
            {
                return db.Clicks.Count(cl => cl.CustomerId == customerid
                    && cl.ClickDate >= FromDate
                    && cl.ClickDate <= ToDate
                    && cl.CampaignId == campaignid && (affiliateid == null || cl.AffiliateId == affiliateid)
                    && !hidden.Contains(cl.CampaignId)
                    );
            }
        }

        public decimal GetGrossRevenue(DateTime FromDate, DateTime ToDate, int customerid, int affiliateid)
        {
            decimal revenue = 0;
            var activecamapaigns = GetUserCampaigns().Where(c => c.Status == Status.Active).Select(c => c.CampaignId);
            if (affiliateid == 0)
            {
                revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate && activecamapaigns.Contains(cl.CampaignId)).Sum(cl => (decimal?)cl.Revenue) ?? 0;
                revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= FromDate && im.ImpressionDate < ToDate && activecamapaigns.Contains(im.CampaignId)).Sum(im => (decimal?)im.Revenue) ?? 0;
                revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.ConversionDate >= FromDate && cn.ConversionDate < ToDate && cn.Status == 1 && activecamapaigns.Contains(cn.CampaignId)).Sum(cn => (decimal?)cn.Revenue) ?? 0;
            }
            else
            {
                revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate && activecamapaigns.Contains(cl.CampaignId)).Sum(cl => (decimal?)cl.Revenue) ?? 0;
                revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= FromDate && im.ImpressionDate < ToDate && activecamapaigns.Contains(im.CampaignId)).Sum(im => (decimal?)im.Revenue) ?? 0;
                revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == affiliateid && cn.ConversionDate >= FromDate && cn.ConversionDate < ToDate && cn.Status == 1 && activecamapaigns.Contains(cn.CampaignId)).Sum(cn => (decimal?)cn.Revenue) ?? 0;
            }

            return revenue;
        }

        public decimal GetGrossCost(DateTime FromDate, DateTime ToDate, int customerid, int affiliateid)
        {
            var activecamapaigns = GetUserCampaigns().Where(c => c.Status == Status.Active).Select(c => c.CampaignId);
            decimal cost = 0;
            if (affiliateid == 0)
            {
                cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate && activecamapaigns.Contains(cl.CampaignId)).Sum(cl => (decimal?)cl.Cost) ?? 0;
                cost += db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= FromDate && im.ImpressionDate < ToDate && activecamapaigns.Contains(im.CampaignId)).Sum(im => (decimal?)im.Cost) ?? 0;
                cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.ConversionDate >= FromDate && cn.ConversionDate < ToDate && cn.Status == 1 && activecamapaigns.Contains(cn.CampaignId)).Sum(cn => (decimal?)cn.Cost) ?? 0;
            }
            else
            {
                cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate && activecamapaigns.Contains(cl.CampaignId)).Sum(cl => (decimal?)cl.Cost) ?? 0;
                cost += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= FromDate && im.ImpressionDate < ToDate && activecamapaigns.Contains(im.CampaignId)).Sum(im => (decimal?)im.Cost) ?? 0;
                cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == affiliateid && cn.ConversionDate >= FromDate && cn.ConversionDate < ToDate && cn.Status == 1 && activecamapaigns.Contains(cn.CampaignId)).Sum(cn => (decimal?)cn.Cost) ?? 0;
            }
            return cost;
        }

        /*========================================== Chart ===================================================*/

        /*====================================== Tickers =================================================*/

        public void AddTicker(Ticker t)
        {
            db.Tickers.Add(t);
            db.SaveChanges();
        }

        public void AddTickerCampaign(int tickerid, int camapaigid)
        {
            db.TickerCampaigns.Add(new TickerCampaign() { TickerId = tickerid, CampaignId = camapaigid });
        }

        public IQueryable<Ticker> GetTickers(int userid = 0)
        {
            if (userid == 0)
                userid = GetCurrentUserId();
            return db.Tickers.Where(t => t.UserId == userid);
        }

        public void DeleteTickerCampaigns(int tickerid)
        {
            db.TickerCampaigns.RemoveRange(db.TickerCampaigns.Where(t => t.TickerId == tickerid));
        }

        public void DeleteTicker(int tickerid)
        {
            DeleteTickerCampaigns(tickerid);
            db.Tickers.Remove(db.Tickers.Find(tickerid));
            db.SaveChanges();
        }

        public IQueryable<EditTickerViewModel> GetEditTickerVieModel(int tickerid, bool all)
        {
            int customerid = GetCurrentCustomerId();
            if (all)
            {
                return db.Campaigns.Where(c => c.CustomerId == customerid && c.Status == Status.Active).Select(c =>
                    new EditTickerViewModel { CamapaignId = c.Id, CamapaignName = c.CampaignName, HasCampaign = true });
            }

            return from c in db.Campaigns.Where(c => c.CustomerId == customerid && c.Status == Status.Active)
                   join t in db.TickerCampaigns.Where(t => t.TickerId == tickerid) on c equals t.Campaign into r
                   from t in r.DefaultIfEmpty()
                   select new EditTickerViewModel { CamapaignId = c.Id, CamapaignName = c.CampaignName, HasCampaign = t != null };
        }

        public TickerSetting GetTickerSettings(int userid = 0)
        {
            if (userid == 0)
            {
                userid = this.GetCurrentUserId();
            }
            return db.TickerSettings.Find(userid);
        }

        public void AddTickerSetting(TickerSetting ts)
        {
            ts.UserId = GetCurrentUserId();
            db.TickerSettings.Add(ts);
            db.SaveChanges();
        }

        public void UpdateTickerSettings(TickerSetting ts)
        {
            db.Entry(ts).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateTicker(Ticker ticker)
        {
            db.Tickers.Attach(ticker);
            DbEntityEntry<Ticker> entry = db.Entry(ticker);
            entry.Property(e => e.Speed).IsModified = true;
            entry.Property(e => e.View).IsModified = true;
            entry.Property(e => e.All).IsModified = true;
            entry.Property(e => e.Direction).IsModified = true;
            entry.Property(e => e.BackgroundColor).IsModified = true;
            entry.Property(e => e.CampaignColor).IsModified = true;
            entry.Property(e => e.ImpressionColor).IsModified = true;
            entry.Property(e => e.ClickColor).IsModified = true;
            entry.Property(e => e.ConversionColor).IsModified = true;
            db.SaveChanges();
        }

        public IEnumerable<TickerItem> BuildTicker(DateTime fromdate, DateTime todate, int customerid, int userid, int? affiliateid, string subid = null)
        {
            //return db.Database.SqlQuery(typeof(TickerItem), "EXEC [BuildTicker] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, userid, affiliateid, subid);
            return db.Database.SqlQuery<TickerItem>("EXEC [NewBuildTicker] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, userid, affiliateid, subid);
        }

        public IEnumerable<TickerItemExt> BuildTickerExt(DateTime fromdate, DateTime todate, int customerid, int userid, int? affiliateid, string subid = null)
        {
            //return db.Database.SqlQuery(typeof(TickerItemExt), "EXEC [BuildTickerExt] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, userid, affiliateid, subid);
            return db.Database.SqlQuery<TickerItemExt>("EXEC [NewBuildTickerExt] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, userid, affiliateid, subid);
        }

        public void DeleteUserTickers(int userid)
        {
            db.Tickers.RemoveRange(db.Tickers.Where(t => t.UserId == userid));
            db.SaveChanges();
        }

        public void DeleteTickerSettings()
        {
            var ts = db.TickerSettings.Find(GetCurrentUserId());
            if (ts != null)
            {
                db.TickerSettings.Remove(ts);
            }
            db.SaveChanges();
        }

        /*====================================== Tickers =================================================*/

        public Customer GetCurrentCustomer(int customerid = 0)
        {
            if (customerid == 0)
                customerid = GetCurrentUser().CustomerId;
            return db.Customers.Single(c => c.CustomerId == customerid);
        }

        private CpaTickerDb db;

        //private Uri request;

        //public Uri Request
        //{
        //    get { return request; }
        //    set { request = value; }
        //}

        public IQueryable<Campaign> Campaigns()
        {
            return db.Campaigns;
        }

        //public IQueryable<Campaign> GetCustomerCampaigns(int customerid = 0)
        //{
        //    if (customerid == 0)
        //    {
        //        customerid = GetCurrentCustomerId();
        //    }
        //    return db.Campaigns.Where(c => c.CustomerId == customerid);
        //}

        //public IQueryable<Campaign> GetCustomerActiveCampaigns(int customerid = 0)
        //{
        //    if (customerid == 0)
        //    {
        //        customerid = GetCurrentCustomerId();
        //    }
        //    return db.Campaigns.Where(c => c.CustomerId == customerid && c.Status == Status.Active);
        //}

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public EFCpatickerRepository()
        {
            this.db = new CpaTickerDb();
        }

        public int GetCurrentCustomerId()
        {

            return GetCurrentUser().CustomerId;
        }

        public UserProfile GetCurrentUser()
        {
            return db.UserProfiles.Single(u => u.UserId == WebMatrix.WebData.WebSecurity.CurrentUserId);
        }

        public int GetCurrentUserId()
        {
            return WebMatrix.WebData.WebSecurity.CurrentUserId;
        }

        public IQueryable<TickerCampaign> GetTickerCampaigns(int tickerid)
        {
            return db.TickerCampaigns.Where(t => t.TickerId == tickerid);
        }

        public Affiliate GetAffiliate(int affiliateid, int customerid = 0)
        {
            if (customerid == 0)
            {
                customerid = GetCurrentUser().CustomerId;
            }
            return db.Affiliates.SingleOrDefault(a => a.CustomerId == customerid && a.AffiliateId == affiliateid);
        }

        public decimal GetRevenue(RevenueType rt, decimal? revenue, decimal? revenuepercent, ActionType actiontype, string SaleAmount = "0.0")
        {

            switch (rt)
            {
                case RevenueType.RPC:
                    if (actiontype == ActionType.Click)
                        return revenue.Value;
                    return 0;
                case RevenueType.RPM:
                    if (actiontype == ActionType.Impression)
                        return revenue.Value / 1000;
                    return 0;
                case RevenueType.RPA:
                    if (actiontype == ActionType.Conversion)
                        return revenue.Value;
                    return 0;
                case RevenueType.RPS:
                    if (actiontype == ActionType.Conversion)
                        return revenuepercent.Value * decimal.Parse(SaleAmount);
                    return 0;
                case RevenueType.RPA_RPS:
                    if (actiontype == ActionType.Conversion)
                        return revenue.Value + revenuepercent.Value * decimal.Parse(SaleAmount);
                    return 0;
                default:
                    return 0;
            }

        }

        public decimal GetCost(PayoutType pt, decimal? payout, decimal? payoutpercent, ActionType actiontype, string SaleAmount = "0.0")
        {
            switch (pt)
            {
                case PayoutType.CPC:
                    if (actiontype == ActionType.Click)
                        return payout.Value;
                    return 0;
                case PayoutType.CPM:
                    if (actiontype == ActionType.Impression)
                        return payout.Value / 1000;
                    return 0;
                case PayoutType.CPA:
                    if (actiontype == ActionType.Conversion)
                        return payout.Value;
                    return 0;
                case PayoutType.CPS:
                    if (actiontype == ActionType.Conversion)
                        return payoutpercent.Value * decimal.Parse(SaleAmount);
                    return 0;
                case PayoutType.CPA_CPS:
                    if (actiontype == ActionType.Conversion)
                        return payout.Value + (payoutpercent.Value * decimal.Parse(SaleAmount));
                    return 0;
                default:
                    return 0;
            }
        }

        public Campaign GetCampaignById(int campaignid, int customerid = 0)
        {
            if (customerid == 0)
            {
                customerid = GetCurrentUser().CustomerId;
            }

            // return db.Campaigns.SingleOrDefault(c => c.CampaignId == campaignid && c.CustomerId == customerid && c.Status == Status.Active);
            return db.Campaigns.SingleOrDefault(c => c.CampaignId == campaignid && c.CustomerId == customerid);
        }

        public void AddClick(Click ck)
        {
            db.Clicks.Add(ck);
            db.SaveChanges();
        }

        public void AddImpression(Impression imp)
        {
            throw new NotImplementedException();
        }

        public void AddConversion(Conversion cv)
        {
            throw new NotImplementedException();
        }

        public int GetCustomerId(Uri url)
        {
            int customerid = 0;
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;

                try
                {
                    customerid = db.Domains.Where(d => d.DomainName == host).Join(db.CustomerDomains, d => d.Id, cd => cd.DomainId, (d, cd) => cd.CustomerId).Single();
                }
                catch
                {
                    // if i'm here is because that domain wasn't found so ...
                    if (host.Split('.').Length > 2)
                    {
                        // this means that this has a subdomain
                        int lastIndex = host.LastIndexOf(".");
                        int index = host.LastIndexOf(".", lastIndex - 1);
                        string subdomain = host.Substring(0, index);

                        try
                        {
                            customerid = db.Customers.Single(c => c.AccountId == subdomain).CustomerId;
                        }
                        catch
                        {
                            throw new HttpException(404, "NotFound");
                        }
                    }
                    else
                        throw new HttpException(404, "NotFound");

                }

            }

            return customerid;

        }

        public void AddTransaction(Transaction transaction)
        {
            db.Transactions.Add(transaction);
            db.SaveChanges();
        }

        public void AddOrderDecline(int orderid)
        {
            Order order = db.Orders.Find(orderid);
            order.Declines += 1;

            System.Data.Entity.Infrastructure.DbEntityEntry<Order> entry = db.Entry(order);
            entry.Property("Declines").IsModified = true;

            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }

        public IQueryable<State> GetCountryStates(int countryid)
        {
            return db.States.Where(s => s.CountryId == countryid).OrderBy(s => s.StateName);
        }

        public State GetStateByCode(string code)
        {
            return db.States.SingleOrDefault(s => s.StateCode == code);
        }

        public IQueryable<URL> GetURLs(int? customerid)
        {
            if (!customerid.HasValue)
                customerid = this.GetCurrentCustomerId();
            return db.URLs.Where(a => a.Campaign.CustomerId == customerid).OrderBy(a => a.Campaign.CampaignId);
        }

        public IQueryable<Domain> GetCustomerDomains(int customerid)
        {
            return db.CustomerDomains.Where(cd => cd.CustomerId == customerid).Select(cd => cd.Domain);
        }

        public Domain GetDefaultDomain(int customerid)
        {
            return new Domain
            {
                Id = CpaTickerConfiguration.DefaultDomainId,
                DomainName = string.Format("{1}.{0}", CpaTickerConfiguration.DefaultDomainName, GetCurrentCustomer(customerid).AccountId)
            };
        }

        public IQueryable<UserProfile> GetCustomerUsers(int customerid = 0)
        {
            if (customerid == 0)
            {
                customerid = GetCurrentCustomerId();
            }
            return db.UserProfiles.Where(up => up.CustomerId == customerid && up.UserId != WebMatrix.WebData.WebSecurity.CurrentUserId);
        }

        /*====================================== CustomFields =================================================*/

        public IQueryable<CustomField> GetCustomFields(int customerid = 0)
        {
            if (customerid == 0)
            {
                customerid = GetCurrentCustomerId();
            }
            return db.CustomFields.Where(f => f.CustomerId == customerid);
        }

        public void AddCustomField(CustomField cf)
        {
            db.CustomFields.Add(cf);
            db.SaveChanges();
        }

        public bool FieldNameExists(string fname)
        {
            int customerid = GetCurrentCustomerId();
            return db.CustomFields.Where(f => f.CustomerId == customerid && f.FieldName == fname).Count() > 0;
        }

        public void DeleteCustomField(CustomField cf)
        {
            db.CustomFields.Remove(cf);
            db.SaveChanges();
        }

        public CustomField FindCustomField(int id)
        {
            return db.CustomFields.Find(id);
        }

        /*====================================== CustomFields =================================================*/

        public void DeleteCustomerUser(int id)
        {
            string username = db.UserProfiles.Find(id).UserName;

            foreach (var role in Roles.GetRolesForUser(username))
                Roles.RemoveUserFromRole(username, role);

            var membership = (WebMatrix.WebData.SimpleMembershipProvider)Membership.Provider;
            membership.DeleteAccount(username);
            membership.DeleteUser(username, true);
        }

        public void DeleteDomain(int id)
        {
            CustomerDomain custdom = db.CustomerDomains.Single(cd => cd.Id == id);
            db.CustomerDomains.Remove(custdom);
            // and remove the domain from the domains table
            Domain dom = db.Domains.Single(d => d.Id == custdom.DomainId);
            db.Domains.Remove(dom);
            db.SaveChanges();
        }

        public void AddCustomFieldValue(CustomFieldValue customFieldValue)
        {
            db.CustomFieldValues.Add(customFieldValue);
        }

        public void AddCampaign(Campaign campaign)
        {
            db.Campaigns.Add(campaign);
            db.SaveChanges();
        }

        public IEnumerable<EditCFVModel> GetCampaignCustomField(int campaignid, int customerid = 0)
        {
            if (customerid == 0)
            {
                customerid = GetCurrentCustomerId();
            }
            return from f in db.CustomFields.Where(f => f.CustomerId == customerid)
                   join v in db.CustomFieldValues.Where(f => f.CampaignId == campaignid) on f equals v.CustomField into r
                   from t in r.DefaultIfEmpty()
                   select new EditCFVModel { CustomField = f, Value = t.Value };
        }


        public IQueryable<CustomFieldValue> GetCampaignCustomFieldValue(int campaignid, int customerid)
        {
            return db.CustomFieldValues.Where(f => f.CampaignId == campaignid && f.CustomField.CustomerId == customerid);
        }

        public UserProfile GetUserFromAPIKey(string api)
        {
            Guid APIKey = Guid.Parse(api);
            return db.UserProfiles.SingleOrDefault(c => c.APIKey == APIKey);
        }

        public IQueryable<Affiliate> GetCustomerAffiliates(int customerid)
        {
            return db.Affiliates.Where(a => a.CustomerId == customerid);
        }
        public IQueryable<UserAffiliate> GetUserAffiliates(int userid)
        {
            return db.UserAffiliate.Where(a => a.UserId == userid);
        }
        public IQueryable<IP2Country> GetIP2Countries()
        {
            return db.IP2Countries.GroupBy(c => c.Code).Select(g => g.FirstOrDefault()).OrderBy(c => c.Name);
        }

        public void AddCampaignCountry(CampaignCountry campaignCountry)
        {
            db.CampaignCountries.Add(campaignCountry);
        }

        public IEnumerable<EditCampaignCountryViewModel> GetCampaignCountries(int campaignid)
        {
            return from c in db.IP2Countries.GroupBy(c => c.Code).Select(g => g.FirstOrDefault())
                   join t in db.CampaignCountries.Where(t => t.CampaignId == campaignid) on c.Code equals t.Code into r
                   from t in r.DefaultIfEmpty()
                   orderby c.Name
                   select new EditCampaignCountryViewModel { Name = c.Name, Code = c.Code, Checked = t != null };
        }

        public bool CheckIP(long intAddress, int campaignid)
        {
            var ct = db.IP2Countries.SingleOrDefault(i => i.Min <= intAddress && intAddress <= i.Max);
            if (ct != null)
            {
                return db.CampaignCountries.Count(c => c.CampaignId == campaignid && c.Code == ct.Code) > 0;
            }
            return true; // if null means that the ipaddress wasn't found so there is no way to tell so redirect to default so true
        }

        /// <summary>
        /// Return the userprofile according the parameters
        /// </summary>
        /// <param name="affiliateid"></param>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public IQueryable<UserProfile> AffiliateUsers(int affiliateid, int customerid)
        {
            return db.UserProfiles.Where(up => up.AffiliateId == affiliateid && up.CustomerId == customerid);
        }

        public IQueryable<ConversionPixel> ConversionPixels()
        {
            return db.ConversionPixels;
        }


        public void AddConversionPixel(ConversionPixel cp)
        {
            db.ConversionPixels.Add(cp);
            db.SaveChanges();
        }

        public void AddConversionPixelCampaign(ConversionPixelCampaign cpc)
        {
            db.ConversionPixelCampaigns.Add(cpc);
            //db.SaveChanges();
        }


        public ConversionPixel FindConversionPixel(int id)
        {
            // var abd = db.ConversionPixels.Find(id).Campaigns.Where(u => u.PixelStatus == PixelStatus.Active);
            // abd= abd.Campaigns.Where(u => u.PixelStatus == PixelStatus.Active);
            return db.ConversionPixels.Find(id);
        }
        public ConversionPixel FindCenversionPixelActive(int id, PixelStatus status)
        {
            int CustomerID = GetCurrentCustomerId();
            var result = (from au in db.ConversionPixels
                          join cpm in db.ConversionPixelCampaigns on au.Id equals cpm.ConversionPixelId
                          where au.Id == id && au.Affiliate.CustomerId == CustomerID && cpm.PixelStatus == status && cpm.Campaign.CustomerId == CustomerID
                          select au).FirstOrDefault();
            return result;
        }

        public IQueryable<ConversionPixelCampaign> ConversionPixelCampaigns()
        {
            return db.ConversionPixelCampaigns;
        }

        public void RemoveConversionPixel(ConversionPixel cp)
        {
            db.ConversionPixels.Remove(cp);
            db.SaveChanges();
        }

        public void EditConversionPixel(ConversionPixel cp)
        {
            db.Entry(cp).State = EntityState.Modified;
            db.Entry(cp).Property("AffiliateId").IsModified = false;
            db.SaveChanges();
        }

        public ConversionPixelCampaign FindConversionPixelCampaign(int id)
        {
            return db.ConversionPixelCampaigns.Find(id);
        }

        public void RemoveConversionPixelCampaign(ConversionPixelCampaign cpc)
        {
            db.ConversionPixelCampaigns.Remove(cpc);
            db.SaveChanges();
        }

        public void EditConversionPixelCampaign(ConversionPixelCampaign cpc)
        {
            db.Entry(cpc).State = EntityState.Modified;
            db.SaveChanges();
        }


        public IQueryable<Action> Actions()
        {
            return db.Actions;
        }


        public void AddAction(Action action)
        {
            db.Actions.Add(action);
            db.SaveChanges();
        }


        public string TrackingCode(Action action, int did = 0, int? actionid = null)
        {

            var domain = did == 0 ? this.GetDefaultDomain(action.Campaign.CustomerId) : this.GetDomain(did);

            //var actionparameter = actionid == 0 ? "&random=1" : "&urlid=" + urlid;
            //var urlparameter = urlid == 0 ? "&random=1" : "&urlid=" + urlid;

            var sb = new StringBuilder();


            sb.AppendFormat("//{0}/cpa/conversion/?cpid={1}", domain.DomainName, action.Campaign.CampaignId);
            if (actionid.HasValue)
            {
                sb.AppendFormat("&actionid={0}", actionid);
            }

            //Campaign c = GetCampaignById(campaignid, customerid);

            string tracking = "";
            //switch (action.TrackingType)
            //{
            //    case TrackingType.HttpiFrame:
            //        tracking = "<iframe scrolling=\"no\" frameborder=\"0\" width=\"1\" height=\"1\" src=\"http:" + sb.ToString() + "\"></iframe>";
            //        break;
            //    case TrackingType.HttpsiFrame:
            //        tracking = "<iframe scrolling=\"no\" frameborder=\"0\" width=\"1\" height=\"1\" src=\"https:" + sb.ToString() + "\"></iframe>";
            //        break;
            //    case TrackingType.HttpImage:
            //        tracking = "<img border=\"0\" width=\"1\" height=\"1\" src=\"http:" + sb.ToString() + "\">";
            //        break;
            //    case TrackingType.HttpsImage:
            //        tracking = "<img border=\"0\" width=\"1\" height=\"1\" src=\"https:" + sb.ToString() + "\">";
            //        break;
            //    case TrackingType.ServerPostback:
            //        tracking = "https:" + sb.ToString();
            //        break;
            //    default:
            //        tracking = sb.ToString();
            //        break;
            //}

            return tracking;
        }


        public IQueryable<ConversionPixel> GetCustomerConversionPixel(int customerid)
        {
            return db.ConversionPixels.Where(cp => cp.Affiliate.CustomerId == customerid);
        }

        public IQueryable<ConversionPixel> GetCustomerConversionPixelFilter(int customerid, PixelStatus Status)
        {
            var result = (from au in db.ConversionPixels
                          join cpm in db.ConversionPixelCampaigns on au.Id equals cpm.ConversionPixelId
                          where au.Affiliate.CustomerId == customerid && cpm.PixelStatus == Status && cpm.Campaign.CustomerId == customerid
                          select au).Distinct();
            return result;
            //db.ConversionPixels.Where(cp => cp.Affiliate.CustomerId == customerid);
        }

        public Boolean SetConversionPixelBlock(Affiliate affiliate)
        {
            try
            {
                var result = (from au in db.ConversionPixels
                              join cpm in db.ConversionPixelCampaigns on au.Id equals cpm.ConversionPixelId
                              where au.AffiliateId == affiliate.AffiliateId && au.Affiliate.CustomerId == affiliate.CustomerId && cpm.Campaign.CustomerId == affiliate.CustomerId
                              select cpm.Id).Distinct().ToList();
                var filter = affiliate.Status;
                foreach (var Id in result)
                {
                    var CPC = db.ConversionPixelCampaigns.Where(u => u.Id == Id).FirstOrDefault();
                    if (CPC != null)
                    {
                        switch (filter)
                        {
                            case AffiliateStatus.Active:
                                CPC.PixelStatus = PixelStatus.Active;
                                db.SaveChanges();
                                break;
                            case AffiliateStatus.Pending:
                                CPC.PixelStatus = PixelStatus.Pending;
                                db.SaveChanges();
                                break;
                            case AffiliateStatus.Blocked:
                                CPC.PixelStatus = PixelStatus.Blocked;
                                db.SaveChanges();
                                break;
                            case AffiliateStatus.Rejected:
                                CPC.PixelStatus = PixelStatus.Rejected;
                                db.SaveChanges();
                                break;
                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        public Action FindAction(int id)
        {
            return db.Actions.Find(id);
        }


        public void EditAction(Action act)
        {
            try
            {
                db.Entry(act).State = System.Data.Entity.EntityState.Modified;
                //db.Configuration.ValidateOnSaveEnabled = false; // to avoid exception due to the remote attribute on the action entity
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    string s, t;
                    s = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        t = string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }


        public IQueryable<Click> Clicks(int customerid, int campaignid)
        {
            return db.Clicks.Where(c => c.CampaignId == campaignid && c.CustomerId == customerid);
        }


        public IQueryable<RedirectUrl> RedirectUrls()
        {
            return db.RedirectUrls;
        }

        public IQueryable<RedirectTarget> RedirectTargets()
        {
            return db.RedirectTargets;
        }


        public void AddRedirectUrl(RedirectUrl ru)
        {
            db.RedirectUrls.Add(ru);
            db.SaveChanges();
        }


        public void AddRedirectTarget(RedirectTarget redirectTarget)
        {
            db.RedirectTargets.Add(redirectTarget);
            db.SaveChanges();
        }


        public RedirectUrl FindRedirectUrl(int id)
        {
            return db.RedirectUrls.Find(id);
        }


        public void EditRedirectUrl(RedirectUrl ru)
        {
            db.Entry(ru).State = EntityState.Modified;
            db.SaveChanges();
        }

        // remove all target of a specific redirecturl
        public void DeleteRedirectTargets(int id)
        {
            db.RedirectTargets.RemoveRange(db.RedirectTargets.Where(t => t.RedirectUrlId == id));
            db.SaveChanges();
        }


        public void DeleteRedirectUrl(RedirectUrl ru)
        {
            db.RedirectUrls.Remove(ru);
            db.SaveChanges();
        }


        public IP2Country GetCountryFromInt32IP(long intAddress)
        {
            return db.IP2Countries.SingleOrDefault(i => i.Min <= intAddress && intAddress <= i.Max);
        }

        public Block GetBlockFromInt32IP(long intAddress)
        {
            return db.Blocks.SingleOrDefault(b => b.startIpNum <= intAddress && intAddress <= b.endIpNum);
        }


        //public URL GetDefaultUrl(Campaign campaign)
        //{
        //    return new URL 
        //    {
        //        CampaignId = campaign.Id,
        //        OfferUrl = campaign.OfferUrl,
        //        PreviewUrl = campaign.PreviewUrl,
        //        Payout = campaign.Payout,
        //        Revenue = campaign.Revenue,
        //        PreviewId = 1,
        //        Name = "default",
        //    };
        //}


        public Action DefaultAction(Campaign campaign)
        {
            var action = db.Actions.FirstOrDefault(a => a.CampaignId == campaign.Id);
            //if (action == null)
            //{
            //    action = GetDefaultAction(campaign);
            //    AddAction(action);
            //}
            return action;
        }


        public URL DefaultURL(Campaign campaign)
        {
            var url = db.URLs.SingleOrDefault(u => u.CampaignId == campaign.Id && u.PreviewId == 1);
            //if (url == null)
            //{
            //    url = GetDefaultUrl(campaign);
            //    AddURL(url);
            //}
            return url;
        }


        public IQueryable<CampaignCountry> CampaignCountries(int campaignid)
        {
            return db.CampaignCountries.Where(c => c.CampaignId == campaignid);
        }


        public void AddActionConversionPixel(ActionConversionPixel actionConversionPixel)
        {
            db.ActionConversionPixels.Add(actionConversionPixel);
            db.SaveChanges();
        }


        public Banner FindBanner(int id)
        {
            return db.Banners.Find(id);
        }


        public void DeleteUrl(URL url)
        {
            db.URLs.Remove(url);
            db.SaveChanges();
        }


        public IQueryable<ActionConversionPixel> ActionConversionPixels()
        {
            return db.ActionConversionPixels;
        }

        public IQueryable<string> ActionConversionPixelsActivePostBack(int actionId, int AffiliateId, int customerid)
        {
            List<string> obj = new List<string>();
            var Result = (from a in db.ActionConversionPixels
                          join cpc in db.ConversionPixelCampaigns on a.ConversionPixelId equals cpc.ConversionPixelId
                          where a.ActionId == actionId && a.ConversionPixel.Affiliate.AffiliateId == AffiliateId
                        && a.ConversionPixel.Affiliate.CustomerId == customerid
                        && a.ConversionPixel.TrackingType == TrackingType.ServerPostback
                        && cpc.PixelStatus == PixelStatus.Active
                          select new
                          {
                              pixelCode = a.ConversionPixel.PixelCode
                          }).ToList();
            for (int i = 0; i < Result.Count; i++)
            { obj.Add(Result[i].pixelCode); }

            return obj.AsQueryable();
        }
        public IQueryable<string> ActionConversionPixelsActiveNoPostBack(int actionId, int AffiliateId, int customerid)
        {
            List<string> obj = new List<string>();
            var Result = (from a in db.ActionConversionPixels
                          join cpc in db.ConversionPixelCampaigns on a.ConversionPixelId equals cpc.ConversionPixelId
                          where a.ActionId == actionId && a.ConversionPixel.Affiliate.AffiliateId == AffiliateId
                        && a.ConversionPixel.Affiliate.CustomerId == customerid
                        && a.ConversionPixel.TrackingType != TrackingType.ServerPostback
                        && cpc.PixelStatus == PixelStatus.Active
                          select new
                          {
                              pixelCode = a.ConversionPixel.PixelCode
                          }).ToList();
            for (int i = 0; i < Result.Count; i++)
            { obj.Add(Result[i].pixelCode); }

            return obj.AsQueryable();
        }


        public ActionConversionPixel FindActionConversionPixel(int id)
        {
            return db.ActionConversionPixels.Find(id);
        }

        public void DeleteActionConversionPixel(ActionConversionPixel obj)
        {
            db.ActionConversionPixels.Remove(obj);
            db.SaveChanges();
        }


        public Action DefaultAction(int campaignid)
        {
            return db.Actions.Find(db.Campaigns.Find(campaignid).Id);
        }


        public IQueryable<Conversion> Conversions()
        {
            return db.Conversions;
        }


        public void AddTickerElement(TickerElement tickerElement)
        {
            db.TickerElements.Add(tickerElement);
            db.SaveChanges();
        }

        public TickerElement FindTickerElement(int id)
        {
            return db.TickerElements.Find(id);
        }

        public void DeleteTickerElement(TickerElement e)
        {
            db.TickerElements.Remove(e);
            db.SaveChanges();
        }

        public IQueryable<ClickSubId> ClickSubIds()
        {
            return db.ClickSubIds;
        }

        public void EditUserProfile(UserProfile up)
        {
            db.Entry(up).State = EntityState.Modified;
            db.SaveChanges();
        }


        public IQueryable<UserHiddenCampaign> UserHiddenCampaigns()
        {
            return db.UserHiddenCampaigns;
        }

        public void AddUserHiddenCampaign(UserHiddenCampaign hc)
        {
            db.UserHiddenCampaigns.Add(hc);
            db.SaveChanges();
        }


        public void RemoveUserHiddenCampaigns(UserProfile user)
        {
            db.UserHiddenCampaigns.RemoveRange(user.HiddenCampaigns);
            db.SaveChanges();
        }


        public IQueryable<UserProfile> UserProfile()
        {
            return db.UserProfiles;
        }


        public IQueryable<Click> Clicks()
        {
            return db.Clicks;
        }

        public IQueryable<URL> Urls()
        {
            return db.URLs;
        }

        /// <summary>
        /// Return the all user-customer campaign that are not hidden for the specific user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IQueryable<Campaign> GetUserCampaigns(UserProfile user)
        {
            var hidden = user.HiddenCampaigns.Select(h => h.CampaignId);
            return db.Campaigns.Where(c => c.CustomerId == user.CustomerId
                && !hidden.Contains(c.Id) && c.Status == Status.Active && (c.ExpirationDate > DateTime.Today || c.ExpirationDate == null));
            //!user.HiddenCampaigns.Select(h => h.CampaignId).Contains(c.Id));
        }

        public IEnumerable<Campaign> GetUserCampaigns()
        {
            // Only primitive types or enumeration types are supported in this context.
            var user = GetCurrentUser();
            var hidden = user.HiddenCampaigns.Select(h => h.CampaignId);
            return db.Campaigns.Where(c => c.CustomerId == user.CustomerId
                && !hidden.Contains(c.Id));
            //&& !user.HiddenCampaigns.Select(h => h.CampaignId).ToList().Contains(c.Id));
        }


        public bool IsHidden(Campaign c)
        {
            return GetCurrentUser().HiddenCampaigns.Select(h => h.CampaignId).Contains(c.Id);
        }

        /// <summary>
        /// The Campaign.CampaignId not the Campaign.Id field
        /// </summary>
        /// <param name="campaignid"> The Campaign.CampaignId not the Campaign.Id field</param>
        /// <returns></returns>
        public bool IsHidden(int campaignid)
        {
            return GetCurrentUser().HiddenCampaigns.Select(h => h.Campaign.CampaignId).Contains(campaignid);
        }


        public IQueryable<Banner> Banners()
        {
            return db.Banners;
        }


        public UserProfile FindUserProfile(int id)
        {
            return db.UserProfiles.Find(id);
        }


        public Order FindOrder(int id)
        {
            return db.Orders.Find(id);
        }


        public void EditOrder(Order order)
        {
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
        }


        //public IQueryable<APIKeyOrder> APIKeyOrders()
        //{
        //    return db.APIKeyOrders;
        //}


        //public bool HasAPIKeyOrder(int userid)
        //{
        //    return db.APIKeyOrders.FirstOrDefault(o => o.UserId == userid) != null;
        //}


        //public void AddAPIKeyOrder(APIKeyOrder aPIKeyOrder)
        //{
        //    db.APIKeyOrders.Add(aPIKeyOrder);
        //    db.SaveChanges();
        //}


        public void EditCustomer(Customer customer)
        {
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
        }


        public IQueryable<Order> Orders()
        {
            return db.Orders;
        }


        public void AddCustomer(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public void AddOrder(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
        }


        public Country FindCountry(int id)
        {
            return db.Countries.Find(id);
        }


        public void EditCampaign(Campaign campaign)
        {
            db.Entry(campaign).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void AddLimeLightLog(LimeLightLib.LimeLightLog limeLightLog)
        {
            db.LimeLightLogs.Add(limeLightLog);
            db.SaveChanges();
        }


        public Task<Block> FindBlockAsync(uint ip)
        {
            //return this.GetBlockFromInt32IP(ip);

            return db.Blocks.SingleOrDefaultAsync(b => b.startIpNum <= ip && ip <= b.endIpNum);
        }


        public Location FindLocation(int locId)
        {
            return db.Locations.Find(locId);
        }


        public DbSet<Log> Logs()
        {
            return db.Logs;
        }


        public Click FindClick(int id)
        {
            return db.Clicks.Find(id);
        }

        public dynamic Spark(DateTime fromdate, DateTime todate, int customerid, int offset, int userid, int? affiliateid = null)
        {
            return db.Database.SqlQuery(typeof(DailyView), "EXEC [Spark] {0}, {1}, {2}, {3}, {4}, {5}", customerid, fromdate, todate, offset, userid, affiliateid);
        }



        public DbRawSqlQuery<T> RunQuery<T>(string query, params object[] parameters)
        {
            return db.Database.SqlQuery<T>(query, parameters);
        }

        public SqlDataReader GetHourlyRptData(string CustomerId, DateTime fromdate, DateTime todate, string offset, string UserId, string AffiliateId, int? CampaignId, string Country, string ct)
        {
            //var test = db.Clicks.Where(x => x.CustomerId == 9).ToList();
            //DataTable dt = new DataTable();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand("HourlyRpt", connection);
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.AddWithValue("@CustomerId", CustomerId);
                command.Parameters.AddWithValue("@fromdate", fromdate);
                command.Parameters.AddWithValue("@todate", todate);
                command.Parameters.AddWithValue("@offset", offset);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@AffiliateId", DBNull.Value);
                command.Parameters.AddWithValue("@CampaignId", DBNull.Value);
                command.Parameters.AddWithValue("@Country", DBNull.Value);
                command.Parameters.AddWithValue("@ct", ct);
                command.Notification = null;

                //SqlDependency dependency = new SqlDependency(command);
                //dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                return command.ExecuteReader(CommandBehavior.CloseConnection);

                //   dt.Load(command.ExecuteReader(
                //CommandBehavior.CloseConnection));
                //   return dt;

            }
        }


        public void GetNotifyHourlyRptData(int CustomerId, string pagename, string timezone, string viewdata,
            string fromdate, string todate, string dataview, string ConnectionID) // and the fromdate & todate are for what
        {
            //DateTime? FromDate = Convert.ToDateTime(fromdate);
            //DateTime? ToDate = Convert.ToDateTime(todate);
            DateTime? FromDate = DateTime.Today;
            DateTime? ToDate = DateTime.Today.AddDays(1).AddSeconds(-1);

            GB_timezone = timezone;
            GB_viewdata = viewdata;
            GB_fromdate = FromDate.ToString();
            GB_todate = ToDate.ToString();
            GB_dataview = dataview;
            GB_customerid = CustomerId;
            GB_connectionid = ConnectionID;
            var customer = GetCurrentCustomer(CustomerId);
            // CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, CustomerId);
            var tzi = FindTimeZoneInfo(timezone, customer.TimeZone);

            // convert the time with the customer tzi to UTC 
            var ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT  [ID],[Date],[TableName],[CustomerID] FROM [dbo].[HourlyRptNotification] where CustomerID=@CustomerId and ([Date] between @FDate and @TDate) ", connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", CustomerId);
                    command.Parameters.AddWithValue("@FDate", ufdate);
                    command.Parameters.AddWithValue("@TDate", utdate);
                    // command.CommandType = CommandType.StoredProcedure;

                    command.Notification = null;

                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(Hourlydependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
        }


        public void GetNotifySparkHourlyRptData(int CustomerId, string pagename, string ConnectionID)
        {
            GB_customerid = CustomerId;
            GB_connectionid = ConnectionID;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT  [ID],[Date],[TableName],[CustomerID] FROM [dbo].[HourlyRptNotification] where CustomerID=@CustomerId AND (TableName='Clicks' OR TableName = 'Conversions') ", connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", CustomerId);
                    // command.Parameters.AddWithValue("@CustomerId", CustomerId);
                    // command.CommandType = CommandType.StoredProcedure;

                    command.Notification = null;

                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(Sparkdependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    command.ExecuteReader(CommandBehavior.CloseConnection);
                }

            }
        }


        public SqlDataReader NotifyConversionStatus(int CustomerId, string pagename, string timezone, string viewdata, string fromdate, string todate, string dataview, string ConnectionID)
        {

            DateTime? FromDate = DateTime.Today;
            DateTime? ToDate = DateTime.Today.AddDays(1).AddSeconds(-1);

            GB_timezone = timezone;
            GB_viewdata = viewdata;
            GB_fromdate = FromDate.ToString();
            GB_todate = ToDate.ToString();
            GB_dataview = dataview;
            GB_customerid = CustomerId;
            GB_connectionid = ConnectionID;

            var customer = GetCurrentCustomer(CustomerId);
            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, CustomerId, timezone);
            var tzi = FindTimeZoneInfo(timezone, customer.TimeZone);

            // convert the time with the customer tzi to UTC 
            var ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            string commandtext = null;
            //DataTable dt = new DataTable();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                connection.Open();
                commandtext = "SELECT  [ID],[Date],[TableName],[CustomerID]FROM [dbo].[HourlyRptNotification] where CustomerID=" + CustomerId + " and TableName='Conversions' and [Date] between @FDATE and  @TDATE";
                SqlCommand command = new SqlCommand(commandtext, connection);
                command.Parameters.AddWithValue("@FDATE", ufdate);
                command.Parameters.AddWithValue("@TDATE", utdate);
                // command.CommandType = CommandType.StoredProcedure;

                command.Notification = null;

                SqlDependency dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(Conversiondependency_OnChange);

                return command.ExecuteReader();
                //using (SqlDataReader reader = command.ExecuteReader())
                //{

                //    dt.Load(reader);
                //}


                //return dt;

            }
        }

        public int GetNotifyCampaignData(int CustomerId, string pagename, string ConnectionID)
        {
            GB_customerid = CustomerId;
            GB_connectionid = ConnectionID;
            string commandText = null;
            //DataTable dt = new DataTable();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                connection.Open();


                commandText = "SELECT [Id] ,[CampaignId] ,[CustomerId] ,[CampaignName] ,[Description]  ,[Status] ,[ExpirationDate] ,[Enforce]  FROM [dbo].[Campaigns] where CustomerId=" + CustomerId + " and Status='1'";

                SqlCommand command = new SqlCommand(commandText, connection);


                command.Notification = null;

                SqlDependency dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(Campaigndependency_OnChange);

                commandText = "SELECT COUNT(*)  FROM [dbo].[Campaigns] where CustomerId=" + CustomerId + " and Status='1'";

                SqlCommand command1 = new SqlCommand(commandText, connection);
                return (int)command1.ExecuteScalar();
                //using (SqlDataReader reader = command.ExecuteReader())
                //{

                //    dt.Load(reader);
                //}


                //return dt.Rows.Count;

            }
        }



        //public int ChangeGetNotifyTickerData(int CustomerId, int userid, string pagename, string ConnectionID)
        ///*
        // * This method is the same that GetNotifyTickerData except that it access a userid what is the difference?
        // */
        //{

        //    //ICpaTickerRepository repo = new EFCpatickerRepository();
        //    GB_customerid = CustomerId;
        //    GB_userid = userid;
        //    GB_connectionid = ConnectionID;
        //    //string commandText = null;
        //    //DataTable dt = new DataTable();

        //    //object[] obj = new object[2];
        //    //var ticker = repo.FindTicker(tickerid);
        //    //obj[0] = (int)ticker.View;
        //    //int? tkid = null;
        //    //if (!ticker.All)
        //    //{
        //    //    tkid = tickerid;
        //    //}


        //    //var GetAffiliate = (from te in db.TickerElements
        //    //                    join ta in db.Tickers on te.TickerId equals ta.TickerId
        //    //                    where ta.UserId == userid && te.AffiliateId.HasValue
        //    //                    select (te.AffiliateId)).Distinct().ToArray();
        //    //ticker.TickerElements.Where(u => u.AffiliateId.HasValue).Select(u => u.Affiliate.AffiliateId).ToArray();
        //    //var GetCampaigns = ticker.TickerElements.Where(u => u.CampaignId.HasValue).Select(u => u.Campaign.CampaignId).ToArray();
        //    //var GetCampaigns = (from te in db.TickerElements
        //    //                   join ca in db.Campaigns on te.CampaignId equals ca.Id
        //    //                    where te.TickerId == tickerid && ca.CustomerId == CustomerId
        //    //                   select (ca.CampaignId)).ToArray();
        //    //var GetCampaigns = (from te in db.TickerElements
        //    //                    join ta in db.Tickers on te.TickerId equals ta.TickerId
        //    //                    join ca in db.Campaigns on te.CampaignId equals ca.Id
        //    //                    where ta.UserId == userid && ca.CustomerId == CustomerId
        //    //                    select (ca.CampaignId)).Distinct().ToArray();

        //    var up = db.UserProfiles.Find(userid);

        //    var GetAffiliate = up.Tickers.SelectMany(t => t.TickerElements.Where(e => e.AffiliateId.HasValue).Select(e => e.Affiliate.AffiliateId)).Distinct().ToList();
        //    var GetCampaigns = up.Tickers.SelectMany(t => t.TickerElements.Where(e => e.CampaignId.HasValue).Select(e => e.Campaign.CampaignId)).Distinct().ToList();

        //    string Campaigns = string.Join(",", GetCampaigns);
        //    string Affiliate = string.Join(",", GetAffiliate);

        //    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
        //    {
        //        string concatCampaign = "";
        //        string concatAffiliate = "";

        //        connection.Open();
        //        if (Campaigns != "")
        //        {

        //            concatCampaign = " and CampaignId in (" + Campaigns + ")";
        //            if (Affiliate != "")
        //            {
        //                concatAffiliate = "or AffiliateId in (" + Affiliate + ")";
        //            }
        //        }
        //        else if (Affiliate != "")
        //        {
        //            concatAffiliate = "and AffiliateId in (" + Affiliate + ")";
        //        }



        //        var commandText = "SELECT [ID] ,[Date] ,[TableName] ,[CustomerID] ,[CampaignId],[AffiliateId]  FROM [dbo].[HourlyRptNotification] where CustomerID = " + CustomerId + concatCampaign + concatAffiliate;

        //        //commandText = "SELECT dbo.HourlyRptNotification.CampaignId as rptCampID FROM  dbo.TickerElements  INNER JOIN dbo.Campaigns ON dbo.TickerElements.CampaignId= dbo.Campaigns.Id  inner join  dbo.HourlyRptNotification on dbo.Campaigns.CampaignId =dbo.HourlyRptNotification.CampaignId  where dbo.TickerElements.TickerId=" + tickerid + "and ;
        //        connection.Open();
        //        using (var command = new SqlCommand(commandText, connection))
        //        {
        //            command.Notification = null;

        //            SqlDependency dependency = new SqlDependency(command);
        //            dependency.OnChange += new OnChangeEventHandler(Tickerdependency_OnChange);

        //            if (connection.State == ConnectionState.Closed)
        //                connection.Open();

        //            command.ExecuteReader(CommandBehavior.CloseConnection);

        //            //commandText = "SELECT COUNT(*)  FROM [dbo].[HourlyRptNotification] where CustomerID = " + CustomerId + concatCampaign + concatAffiliate;

        //            //SqlCommand command1 = new SqlCommand(commandText, connection);
        //            //return (int)command1.ExecuteScalar();

        //            return 1;

        //        }

        //        //using (SqlDataReader reader = command.ExecuteReader())
        //        //{

        //        //    dt.Load(reader);
        //        //}

        //        //return dt.Rows.Count;
        //    }
        //}



        public int GetNotifyTickerData(int CustomerId, string pagename, string ConnectionID, int userid)
        {
            //ICpaTickerRepository repo = new EFCpatickerRepository();
            //var CustomerObj = repo.GetCurrentUser();


            var up = /*userid == 0 ? GetCurrentUser() :*/ db.UserProfiles.Find(userid);
            var CustomerObj = up;
            GB_customerid = CustomerId;
            //int UserId = CustomerObj.UserId;
            GB_connectionid = ConnectionID;
            //string commandText = null;
            //DataTable dt = new DataTable();
            GB_userid = up.UserId;
            //object[] obj = new object[2];
            //var ticker = repo.FindTicker(tickerid);
            //obj[0] = (int)ticker.View;
            //int? tkid = null;
            //if (!ticker.All)
            //{
            //    tkid = tickerid;
            //}


            //var GetAffiliate = (from te in db.TickerElements
            //                    join ta in db.Tickers on te.TickerId equals ta.TickerId
            //                    where ta.UserId == UserId && te.AffiliateId.HasValue
            //                    select (te.AffiliateId)).Distinct().ToArray();
            //ticker.TickerElements.Where(u => u.AffiliateId.HasValue).Select(u => u.Affiliate.AffiliateId).ToArray();
            //var GetCampaigns = ticker.TickerElements.Where(u => u.CampaignId.HasValue).Select(u => u.Campaign.CampaignId).ToArray();
            //var GetCampaigns = (from te in db.TickerElements
            //                   join ca in db.Campaigns on te.CampaignId equals ca.Id
            //                    where te.TickerId == tickerid && ca.CustomerId == CustomerId
            //                   select (ca.CampaignId)).ToArray();
            //var GetCampaigns = (from te in db.TickerElements
            //                    join ta in db.Tickers on te.TickerId equals ta.TickerId
            //                    join ca in db.Campaigns on te.CampaignId equals ca.Id
            //                    where ta.UserId == UserId && ca.CustomerId == CustomerId
            //                    select (ca.CampaignId)).Distinct().ToArray();

            var GetAffiliate = up.Tickers.SelectMany(t => t.TickerElements.Where(e => e.AffiliateId.HasValue).Select(e => e.Affiliate.AffiliateId)).Distinct().ToList();
            var GetCampaigns = up.Tickers.SelectMany(t => t.TickerElements.Where(e => e.CampaignId.HasValue).Select(e => e.Campaign.CampaignId)).Distinct().ToList();

            string Campaigns = string.Join(",", GetCampaigns);
            string Affiliate = string.Join(",", GetAffiliate);


            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string concatCampaign = "";
                string concatAffiliate = "";

                connection.Open();
                if (Campaigns != "")
                {
                    concatCampaign = " and CampaignId in (" + Campaigns + ")";
                    if (Affiliate != "")
                    {
                        concatAffiliate = "or AffiliateId in (" + Affiliate + ")";
                    }
                }
                else if (Affiliate != "")
                {
                    concatAffiliate = "and AffiliateId in (" + Affiliate + ")";
                }

                var commandText = "SELECT [ID] ,[Date] ,[TableName] ,[CustomerID] ,[CampaignId],[AffiliateId]  FROM [dbo].[HourlyRptNotification] where CustomerID = " + CustomerId + concatCampaign + concatAffiliate;

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange -= Tickerdependency_OnChange;
                    dependency.OnChange += new OnChangeEventHandler(Tickerdependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    command.ExecuteReader(CommandBehavior.CloseConnection);

                    return 1; // I don't know why this method has to return a value at all ? Is it necessary ?

                    //using (var reader = command.ExecuteReader()) { } // we need to execute the reader
                }

                //commandText = "SELECT COUNT(*)  FROM [dbo].[HourlyRptNotification] where CustomerID = " + CustomerId + concatCampaign + concatAffiliate;

                //SqlCommand command1 = new SqlCommand(commandText, connection);


                //var countn  = (int)command1.ExecuteScalar();

                //connection.Close();
                //return countn;
                //using (SqlDataReader reader = command.ExecuteReader())
                //{

                //    dt.Load(reader);
                //}


                //return dt.Rows.Count;

            }
        }

        public SqlDataReader NotifyAffiliateData(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null, string ConnectionID = "")
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();
            //DataTable dt = new DataTable();
            var up = GetCurrentUser();
            var customer = GetCurrentCustomer(up.CustomerId);
            string commandText = null;
            StatisticsEnum stats;
            // page_name = "Affiliate";
            GB_connectionid = ConnectionID;
            //int statistics = 63;

            try
            {
                //string[] dataviewlist = dataview.Split(',');
                //statistics = int.Parse(dataviewlist[0]);
                stats = (StatisticsEnum)dataview;

            }
            catch
            {
                stats =
                        StatisticsEnum.Date |
                        StatisticsEnum.CR |
                        StatisticsEnum.CPC |
                        StatisticsEnum.RPC |
                        StatisticsEnum.Impressions |
                        StatisticsEnum.Clicks |
                        StatisticsEnum.Conversions |
                        StatisticsEnum.Cost |
                        StatisticsEnum.Revenue |
                        StatisticsEnum.Profit
                        ;
            }

            //var stats = Statistics.Create(statistics);

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            if (up.AffiliateId.HasValue) // if is affiliate
                af = new int[] { up.AffiliateId.Value };
            if (af != null && af.Length == 0)
                af = null;

            var tzi = FindTimeZoneInfo(timezone, customer.TimeZone);

            // convert the time with the customer tzi to UTC 
            var ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            bool campaigncheck = stats.HasFlag(StatisticsEnum.Campaign); //string.IsNullOrEmpty(offview);
            bool sourcechk = stats.HasFlag(StatisticsEnum.Source); //!string.IsNullOrEmpty(sourceview);
            bool countrycheck = stats.HasFlag(StatisticsEnum.Country);

            // get the subids


            string ctfilter = ct.HasValue ?
                                   GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                                   : null;

            #region Dynamic Type
            var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
            DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
            DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));
            DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof(int));
            DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof(int));
            DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof(int));
            DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof(decimal));
            DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof(decimal));

            if (campaigncheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof(string));
            }
            if (sourcechk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof(string));
            }
            if (countrycheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof(string));
            }


            Type resultType = affview.CreateType();

            #endregion


            var query = CKQueryBuilder.AffiliateReportQuery(up.CustomerId, up.UserId, campaigncheck, sourcechk
              , null, countrycheck
              , cp, af, ctfilter);

            var list = ExecuteQuery(resultType, query
                , new SqlParameter("fromdate", ufdate)
                , new SqlParameter("todate", utdate)
                ).ToString();


            SqlCommand command = new SqlCommand(list, connection);
            command.Parameters.AddWithValue("@fromdate", ufdate);
            command.Parameters.AddWithValue("@todate", utdate);

            List<string> obj = new List<string>();
            using (SqlDataReader reader = command.ExecuteReader())
            {

                //dt.Load(reader);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obj.Add(reader.GetString(0));
                    }
                }
            }


            //foreach (DataRow row in dt.Rows)
            //{
            //    obj.Add(row[0].ToString());
            //}

            string Affiliate = string.Join(",", obj);

            if (Affiliate == "")
            { commandText = "SELECT [ID] ,[Date] ,[TableName] ,[CustomerID] ,[CampaignId],[AffiliateId]  FROM [dbo].[HourlyRptNotification] where CustomerID = " + up.CustomerId + " and AffiliateId in('') and [Date] between @FDATE and @TDATE"; }
            else
            { commandText = "SELECT [ID] ,[Date] ,[TableName] ,[CustomerID] ,[CampaignId],[AffiliateId]  FROM [dbo].[HourlyRptNotification] where CustomerID = " + up.CustomerId + " and AffiliateId in(" + Affiliate + ") and [Date] between @FDATE and @TDATE"; }


            //commandText = "SELECT dbo.HourlyRptNotification.CampaignId as rptCampID FROM  dbo.TickerElements  INNER JOIN dbo.Campaigns ON dbo.TickerElements.CampaignId= dbo.Campaigns.Id  inner join  dbo.HourlyRptNotification on dbo.Campaigns.CampaignId =dbo.HourlyRptNotification.CampaignId  where dbo.TickerElements.TickerId=" + tickerid + "and ;

            SqlCommand command1 = new SqlCommand(commandText, connection);
            command1.Parameters.AddWithValue("@FDATE", ufdate);
            command1.Parameters.AddWithValue("@TDATE", utdate);

            command.Notification = null;

            SqlDependency dependency = new SqlDependency(command1);
            dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

            return command1.ExecuteReader();

            //using (SqlDataReader reader = command1.ExecuteReader())
            //{


            //}



            //return dt;

        }


        public SqlDataReader NotifyCTRData(int CustomerID, int UserID, string fromdate, string todate, string timezone, string viewdata, string ConnectionID)
        {

            // var up = GetCurrentUser();
            // int[] af = null;
            int? cp = null;
            var customer = GetCurrentCustomer(CustomerID);
            DateTime? FromDate = DateTime.Today;
            DateTime? ToDate = DateTime.Today.AddDays(1).AddSeconds(-1);
            GB_userid = UserID;
            GB_fromdate = FromDate.ToString();
            GB_todate = ToDate.ToString();
            GB_customerid = CustomerID;
            GB_timezone = timezone;
            GB_viewdata = viewdata;
            //page_name = "CTRReport";
            GB_connectionid = ConnectionID;
            string commandText = null;
            //DataTable dt = new DataTable();
            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, CustomerID, timezone);

            var tzi = FindTimeZoneInfo(timezone, customer.TimeZone);

            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            // if (db.UserProfiles.Where(u=>u.UserId==UserID &&u.AffiliateId.HasValue) // if is affiliate
            var af = db.UserProfiles.Where(u => u.UserId == UserID && u.AffiliateId.HasValue).Select(u => u.AffiliateId).ToArray();
            //if (af.Length == 0)
            //    af = null;

            string affs = af == null ? null : string.Join(",", af);



            var report = RunQuery<CTRView>("EXEC [CTRReport] {0}, {1}, {2}, {3}, {4}, {5}, {6}",
                                CustomerID, ufdate, utdate, UserID, affs, cp, null).ToList();

            //var report = repo.CTRReport(ufdate, utdate, up.CustomerId, up.UserId, affs, cp, null);

            List<int> obj = new List<int>();

            // fill the dictionary
            foreach (var item in report)
            {
                obj.Add(item.CampaignId);
            }
            obj = obj.Distinct().ToList();

            string Campaigns = string.Join(",", obj);
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string concatCampaign = "";

                connection.Open();
                if (Campaigns != "")
                {

                    concatCampaign = " and CampaignId in (" + Campaigns + ")";

                }
                else
                { concatCampaign = " and CampaignId in ('')"; }



                commandText = "SELECT [ID] ,[Date] ,[TableName] ,[CustomerID] ,[CampaignId],[AffiliateId]  FROM [dbo].[HourlyRptNotification] where TableName='Clicks' and  ([Date] between @FDATE and @TDATE) and CustomerID = " + CustomerID + concatCampaign;

                //commandText = "SELECT dbo.HourlyRptNotification.CampaignId as rptCampID FROM  dbo.TickerElements  INNER JOIN dbo.Campaigns ON dbo.TickerElements.CampaignId= dbo.Campaigns.Id  inner join  dbo.HourlyRptNotification on dbo.Campaigns.CampaignId =dbo.HourlyRptNotification.CampaignId  where dbo.TickerElements.TickerId=" + tickerid + "and ;

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@FDATE", ufdate);
                command.Parameters.AddWithValue("@TDATE", utdate);

                command.Notification = null;

                SqlDependency dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(CTRdependency_OnChange);


                return command.ExecuteReader();
                //using (SqlDataReader reader = command.ExecuteReader())
                //{

                //    dt.Load(reader);
                //}




            }
            //return dt;
        }


        private void Hourlydependency_OnChange(object sender, SqlNotificationEventArgs e)
        {

            //var changtype = e.Type;
            SqlDependency dep = (SqlDependency)sender;
            dep.OnChange -= Hourlydependency_OnChange;
            //SqlDependency depdate = (SqlDependency)sender;
            //depdate.OnChange -= DatepickerHourlydependency_OnChange;
            //Register for the new notification

            TrickerHub.Show("Hourly", GB_connectionid);

            GetNotifyHourlyRptData(GB_customerid, "Hourly", GB_timezone, GB_viewdata, GB_fromdate, GB_todate, GB_dataview, GB_connectionid);
        }


        private void Tickerdependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //var changtype = e.Type;
            //SqlDependency dep = (SqlDependency)sender;
            //dep.OnChange -= Tickerdependency_OnChange;
            //ChangeGetNotifyTickerData(GB_customerid, GB_userid, "ticker", GB_connectionid);


            TrickerHub.Show("ticker", GB_connectionid);
            GetNotifyTickerData(GB_customerid, "ticker", GB_connectionid, GB_userid); // register the dependy all over again? is this necessary?

        }
        private void Sparkdependency_OnChange(object sender, SqlNotificationEventArgs e)
        {

            //var changtype = e.Type;
            SqlDependency dep = (SqlDependency)sender;
            dep.OnChange -= Sparkdependency_OnChange;

            TrickerHub.Show("onlySparks", GB_connectionid);
            GetNotifySparkHourlyRptData(GB_customerid, "onlySparks", GB_connectionid);
        }

        private void Conversiondependency_OnChange(object sender, SqlNotificationEventArgs e)
        {

            var changtype = e.Type;
            NotifyConversionStatus(GB_customerid, "ConversionStatus", GB_timezone, GB_viewdata, GB_fromdate, GB_todate, GB_dataview, GB_connectionid);
            TrickerHub.Show("ConversionStatus", GB_connectionid);
        }


        private void Campaigndependency_OnChange(object sender, SqlNotificationEventArgs e)
        {

            var changtype = e.Type;
            GetNotifyCampaignData(GB_customerid, "campaign", GB_connectionid);
            TrickerHub.Show("campaign", GB_connectionid);
        }
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {

            var changtype = e.Type;
            TrickerHub.Show("", GB_connectionid);
        }

        private void CTRdependency_OnChange(object sender, SqlNotificationEventArgs e)
        {

            var changtype = e.Type;
            NotifyCTRData(GB_customerid, GB_userid, GB_fromdate, GB_todate, GB_timezone, GB_viewdata, GB_connectionid);
            TrickerHub.Show("CTRReport", GB_connectionid);
        }

        public IQueryable<OverridePayout> Override()
        {
            return db.OverridePayout;
        }
        public List<DispOverridePayout> GetOverridePayout(int actionid)
        {
            var customerid = GetCurrentCustomer().CustomerId;
            var Result = (from op in db.OverridePayout
                          join aff in db.Affiliates on op.AffiliateID equals aff.AffiliateId
                          join cam in db.Campaigns on op.CampaignID equals cam.Id
                          where op.ActionID == actionid && op.CustomerID == customerid && aff.CustomerId == customerid
                          select new DispOverridePayout
                          {
                              campaign = cam.CampaignId + " - " + cam.CampaignName,
                              OverrideID = op.OverridID,
                              affiliate = aff.AffiliateId + " - " + aff.Company,
                              Payout = op.Payout,
                              PayoutPercent = op.PayoutPercent
                          }).Distinct().ToList();
            return Result;
        }

        public List<DispOverridePayout> GetOverridePayoutCampaign(int campaignID)
        {
            var customerid = GetCurrentCustomer().CustomerId;
            var Result = (from op in db.OverridePayout
                          join aff in db.Affiliates on op.AffiliateID equals aff.AffiliateId
                          join cam in db.Campaigns on op.CampaignID equals cam.Id
                          join act in db.Actions on op.ActionID equals act.Id into actions
                          from acts in actions.DefaultIfEmpty()
                          where op.CampaignID == campaignID && op.CustomerID == customerid && aff.CustomerId == customerid
                          select new DispOverridePayout
                          {
                              campaign = cam.CampaignId + " - " + cam.CampaignName,
                              action = acts.Name,
                              OverrideID = op.OverridID,
                              affiliate = aff.AffiliateId + " - " + aff.Company,
                              Payout = op.Payout,
                              PayoutPercent = op.PayoutPercent
                          }).Distinct().ToList();
            return Result;
        }

        public string AddOverridePayout(string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent)
        {
            var customerid = GetCurrentCustomer().CustomerId;
            Decimal? _PayoutPercent = null;
            Decimal? _Payout = null;
            if (PayoutPercent != "")
            {
                _PayoutPercent = Convert.ToDecimal(PayoutPercent);
            }

            if (Payout != "")
            {
                _Payout = Convert.ToDecimal(Payout);
            }

            int paytype = 0;
            if (AffiliateID != "")
            {
                if (_PayoutType == "CPS")
                {
                    paytype = 2;
                    _Payout = null;
                }
                else if (_PayoutType == "CPA")
                {
                    paytype = 1;
                    _PayoutPercent = null;

                }
                else if (_PayoutType == "CPA_CPS")
                {
                    paytype = 3;

                }
                else if (_PayoutType == "CPC")
                {
                    paytype = 4;
                    _PayoutPercent = null;
                }
                else if (_PayoutType == "CPM")
                {
                    paytype = 5;
                    _PayoutPercent = null;
                }


                string[] AffiliateIDS = AffiliateID.Split(',');
                for (int i = 0; i < AffiliateIDS.Length; i++)
                {

                    OverridePayout obj = new OverridePayout();
                    obj.ActionID = Convert.ToInt32(ActionID);
                    obj.AffiliateID = Convert.ToInt32(AffiliateIDS[i]);
                    obj.CampaignID = Convert.ToInt32(CampaignID);
                    obj.CustomerID = customerid;
                    if (paytype == 1)
                    {
                        obj.PayoutType = PayoutType.CPA;
                    }
                    else if (paytype == 2)
                    {
                        obj.PayoutType = PayoutType.CPS;
                    }
                    else if (paytype == 3)
                    {
                        obj.PayoutType = PayoutType.CPA_CPS;
                    }
                    else if (paytype == 4)
                    {
                        obj.PayoutType = PayoutType.CPC;
                    }
                    else if (paytype == 5)
                    {
                        obj.PayoutType = PayoutType.CPM;
                    }
                    if (_PayoutPercent != null)
                    { obj.PayoutPercent = _PayoutPercent; }

                    if (_Payout != null)
                    { obj.Payout = _Payout; }

                    db.OverridePayout.Add(obj);
                    db.SaveChanges();

                }
            }


            return null;
        }

        public string AddOverridePayoutCampaign(string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent)
        {
            var customerid = GetCurrentCustomer().CustomerId;
            Decimal? _PayoutPercent = null;
            Decimal? _Payout = null;
            if (PayoutPercent != "")
            {
                _PayoutPercent = Convert.ToDecimal(PayoutPercent);
            }

            if (Payout != "")
            {
                _Payout = Convert.ToDecimal(Payout);
            }

            int paytype = 0;
            if (AffiliateID != "")
            {
                if (_PayoutType == "CPS")
                {
                    paytype = 2;
                    _Payout = null;
                }
                else if (_PayoutType == "CPA")
                {
                    paytype = 1;
                    _PayoutPercent = null;

                }
                else if (_PayoutType == "CPA_CPS")
                {
                    paytype = 3;

                }
                else if (_PayoutType == "CPC")
                {
                    paytype = 4;
                    _PayoutPercent = null;
                }
                else if (_PayoutType == "CPM")
                {
                    paytype = 5;
                    _PayoutPercent = null;
                }


                string[] AffiliateIDS = AffiliateID.Split(',');
                for (int i = 0; i < AffiliateIDS.Length; i++)
                {

                    OverridePayout obj = new OverridePayout();
                    // obj.ActionID = Convert.ToInt32(ActionID);
                    obj.AffiliateID = Convert.ToInt32(AffiliateIDS[i]);
                    obj.CampaignID = Convert.ToInt32(CampaignID);
                    obj.CustomerID = customerid;
                    if (paytype == 1)
                    {
                        obj.PayoutType = PayoutType.CPA;
                        obj.ActionID = Convert.ToInt32(ActionID);
                    }
                    else if (paytype == 2)
                    {
                        obj.PayoutType = PayoutType.CPS;
                        obj.ActionID = Convert.ToInt32(ActionID);
                    }
                    else if (paytype == 3)
                    {
                        obj.PayoutType = PayoutType.CPA_CPS;
                        obj.ActionID = Convert.ToInt32(ActionID);
                    }
                    else if (paytype == 4)
                    {
                        obj.PayoutType = PayoutType.CPC;
                        obj.ActionID = null;
                    }
                    else if (paytype == 5)
                    {
                        obj.PayoutType = PayoutType.CPM;
                        obj.ActionID = null;
                    }
                    if (_PayoutPercent != null)
                    { obj.PayoutPercent = _PayoutPercent; }

                    if (_Payout != null)
                    { obj.Payout = _Payout; }

                    db.OverridePayout.Add(obj);
                    db.SaveChanges();

                }
            }


            return null;
        }

        public int AddOverrideAffiliate(int AffiliateID, int CustomerId, string _PayoutType, string Payout, string PayoutPercent, string _RevenueType, string Revenue, string RevenuePercent)
        {
            Decimal? _PayoutPercent = null;
            Decimal? _Payout = null;
            if (PayoutPercent != "")
            {
                _PayoutPercent = Convert.ToDecimal(PayoutPercent);
            }

            if (Payout != "")
            {
                _Payout = Convert.ToDecimal(Payout);
            }

            int paytype = 0;

            if (_PayoutType == "CPS")
            {
                paytype = 2;
                _Payout = null;
            }
            else if (_PayoutType == "CPA")
            {
                paytype = 1;
                _PayoutPercent = null;

            }
            else if (_PayoutType == "CPA_CPS")
            {
                paytype = 3;

            }
            else if (_PayoutType == "CPC")
            {
                paytype = 4;
                _PayoutPercent = null;
            }
            else if (_PayoutType == "CPM")
            {
                paytype = 5;
                _PayoutPercent = null;
            }

            Decimal? _RevenuePercent = null;
            Decimal? _Revenue = null;
            if (RevenuePercent != "")
            {
                _RevenuePercent = Convert.ToDecimal(RevenuePercent);
            }

            if (Revenue != "")
            {
                _Revenue = Convert.ToDecimal(Revenue);
            }

            int revenuetype = 0;

            if (_RevenueType == "RPS")
            {
                revenuetype = 2;
                _Revenue = null;
            }
            else if (_RevenueType == "RPA")
            {
                revenuetype = 1;
                _RevenuePercent = null;

            }
            else if (_RevenueType == "RPA_RPS")
            {
                revenuetype = 3;

            }
            else if (_RevenueType == "RPC")
            {
                revenuetype = 4;
                _RevenuePercent = null;
            }
            else if (_RevenueType == "RPM")
            {
                revenuetype = 5;
                _RevenuePercent = null;
            }

            AffiliateOverridePayout obj = new AffiliateOverridePayout();
            // obj.ActionID = Convert.ToInt32(ActionID);
            obj.AffiliateID = AffiliateID;
            obj.CustomerID = CustomerId;
            if (paytype == 1)
            {
                obj.PayoutType = PayoutType.CPA;
            }
            else if (paytype == 2)
            {
                obj.PayoutType = PayoutType.CPS;
            }
            else if (paytype == 3)
            {
                obj.PayoutType = PayoutType.CPA_CPS;
            }
            else if (paytype == 4)
            {
                obj.PayoutType = PayoutType.CPC;
            }
            else if (paytype == 5)
            {
                obj.PayoutType = PayoutType.CPM;
            }
            if (_PayoutPercent != null)
            { obj.PayoutPercent = _PayoutPercent; }

            if (_Payout != null)
            { obj.Payout = _Payout; }



            if (revenuetype == 1)
            {
                obj.RevenueType = RevenueType.RPA;
            }
            else if (revenuetype == 2)
            {
                obj.RevenueType = RevenueType.RPS;
            }
            else if (revenuetype == 3)
            {
                obj.RevenueType = RevenueType.RPA_RPS;
            }
            else if (revenuetype == 4)
            {
                obj.RevenueType = RevenueType.RPC;
            }
            else if (revenuetype == 5)
            {
                obj.RevenueType = RevenueType.RPM;
            }
            if (_RevenuePercent != null)
            { obj.RevenuePercent = _RevenuePercent; }

            if (_Revenue != null)
            { obj.Revenue = _Revenue; }

            db.AffiliateOverridePayout.Add(obj);
            db.SaveChanges();

            return AffiliateID;
        }

        public int UpdateOverrideAffiliate(int OverrideId, int AffiliateID, int CustomerId, string _PayoutType, string Payout, string PayoutPercent, string _RevenueType, string Revenue, string RevenuePercent)
        {
            Decimal? _PayoutPercent = null;
            Decimal? _Payout = null;
            if (PayoutPercent != "")
            {
                _PayoutPercent = Convert.ToDecimal(PayoutPercent);
            }

            if (Payout != "")
            {
                _Payout = Convert.ToDecimal(Payout);
            }

            int paytype = 0;

            if (_PayoutType == "CPS")
            {
                paytype = 2;
                _Payout = null;
            }
            else if (_PayoutType == "CPA")
            {
                paytype = 1;
                _PayoutPercent = null;

            }
            else if (_PayoutType == "CPA_CPS")
            {
                paytype = 3;

            }
            else if (_PayoutType == "CPC")
            {
                paytype = 4;
                _PayoutPercent = null;
            }
            else if (_PayoutType == "CPM")
            {
                paytype = 5;
                _PayoutPercent = null;
            }

            Decimal? _RevenuePercent = null;
            Decimal? _Revenue = null;
            if (RevenuePercent != "")
            {
                _RevenuePercent = Convert.ToDecimal(RevenuePercent);
            }

            if (Revenue != "")
            {
                _Revenue = Convert.ToDecimal(Revenue);
            }

            int revenuetype = 0;

            if (_RevenueType == "RPS")
            {
                revenuetype = 2;
                _Revenue = null;
            }
            else if (_RevenueType == "RPA")
            {
                revenuetype = 1;
                _RevenuePercent = null;

            }
            else if (_RevenueType == "RPA_RPS")
            {
                revenuetype = 3;

            }
            else if (_RevenueType == "RPC")
            {
                revenuetype = 4;
                _RevenuePercent = null;
            }
            else if (_RevenueType == "RPM")
            {
                revenuetype = 5;
                _RevenuePercent = null;
            }

            AffiliateOverridePayout obj = db.AffiliateOverridePayout.Where(u => u.OverridID == OverrideId).FirstOrDefault();
            // obj.ActionID = Convert.ToInt32(ActionID);
            obj.AffiliateID = AffiliateID;
            obj.CustomerID = CustomerId;
            if (paytype == 1)
            {
                obj.PayoutType = PayoutType.CPA;
            }
            else if (paytype == 2)
            {
                obj.PayoutType = PayoutType.CPS;
            }
            else if (paytype == 3)
            {
                obj.PayoutType = PayoutType.CPA_CPS;
            }
            else if (paytype == 4)
            {
                obj.PayoutType = PayoutType.CPC;
            }
            else if (paytype == 5)
            {
                obj.PayoutType = PayoutType.CPM;
            }

            obj.PayoutPercent = _PayoutPercent;

            obj.Payout = _Payout;



            if (revenuetype == 1)
            {
                obj.RevenueType = RevenueType.RPA;
            }
            else if (revenuetype == 2)
            {
                obj.RevenueType = RevenueType.RPS;
            }
            else if (revenuetype == 3)
            {
                obj.RevenueType = RevenueType.RPA_RPS;
            }
            else if (revenuetype == 4)
            {
                obj.RevenueType = RevenueType.RPC;
            }
            else if (revenuetype == 5)
            {
                obj.RevenueType = RevenueType.RPM;
            }
            obj.RevenuePercent = _RevenuePercent;

            obj.Revenue = _Revenue;

            db.SaveChanges();

            return AffiliateID;
        }
        public string UpdateOverridePayout(int OverrideID, string ActionID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent)
        {


            Decimal? _PayoutPercent = null;
            Decimal? _Payout = null;
            if (PayoutPercent != "")
            {
                _PayoutPercent = Convert.ToDecimal(PayoutPercent);
            }

            if (Payout != "")
            {
                _Payout = Convert.ToDecimal(Payout);
            }

            int paytype = 0;
            if (AffiliateID != "")
            {
                if (_PayoutType == "CPS")
                {
                    paytype = 2;
                    _Payout = null;
                }
                else if (_PayoutType == "CPA")
                {
                    paytype = 1;
                    _PayoutPercent = null;

                }
                else if (_PayoutType == "CPA_CPS")
                {
                    paytype = 3;

                }
                else if (_PayoutType == "CPC")
                {
                    paytype = 4;
                    _PayoutPercent = null;
                }
                else if (_PayoutType == "CPM")
                {
                    paytype = 5;
                    _PayoutPercent = null;
                }




                OverridePayout obj = db.OverridePayout.Where(u => u.OverridID == OverrideID).FirstOrDefault();
                if (obj != null)
                {

                    obj.ActionID = Convert.ToInt32(ActionID);
                    obj.AffiliateID = Convert.ToInt32(AffiliateID);
                    if (paytype == 1)
                    {
                        obj.PayoutType = PayoutType.CPA;
                    }
                    else if (paytype == 2)
                    {
                        obj.PayoutType = PayoutType.CPS;
                    }
                    else if (paytype == 3)
                    {
                        obj.PayoutType = PayoutType.CPA_CPS;
                    }
                    else if (paytype == 4)
                    {
                        obj.PayoutType = PayoutType.CPC;
                    }
                    else if (paytype == 5)
                    {
                        obj.PayoutType = PayoutType.CPM;
                    }

                    obj.PayoutPercent = _PayoutPercent;
                    obj.Payout = _Payout;
                    db.SaveChanges();
                }
                else
                {

                }



            }


            return null;
        }

        public string UpdateOverridePayoutCampaign(int OverrideID, string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent)
        {


            Decimal? _PayoutPercent = null;
            Decimal? _Payout = null;
            if (PayoutPercent != "")
            {
                _PayoutPercent = Convert.ToDecimal(PayoutPercent);
            }

            if (Payout != "")
            {
                _Payout = Convert.ToDecimal(Payout);
            }

            int paytype = 0;
            if (AffiliateID != "")
            {
                if (_PayoutType == "CPS")
                {
                    paytype = 2;
                    _Payout = null;
                }
                else if (_PayoutType == "CPA")
                {
                    paytype = 1;
                    _PayoutPercent = null;

                }
                else if (_PayoutType == "CPA_CPS")
                {
                    paytype = 3;

                }
                else if (_PayoutType == "CPC")
                {
                    paytype = 4;
                    _PayoutPercent = null;
                }
                else if (_PayoutType == "CPM")
                {
                    paytype = 5;
                    _PayoutPercent = null;
                }




                OverridePayout obj = db.OverridePayout.Where(u => u.OverridID == OverrideID).FirstOrDefault();
                if (obj != null)
                {

                    obj.ActionID = Convert.ToInt32(CampaignID);
                    obj.AffiliateID = Convert.ToInt32(AffiliateID);
                    if (paytype == 1)
                    {
                        obj.PayoutType = PayoutType.CPA;
                        obj.ActionID = Convert.ToInt32(ActionID);
                    }
                    else if (paytype == 2)
                    {
                        obj.PayoutType = PayoutType.CPS;
                        obj.ActionID = Convert.ToInt32(ActionID);
                    }
                    else if (paytype == 3)
                    {
                        obj.PayoutType = PayoutType.CPA_CPS;
                        obj.ActionID = Convert.ToInt32(ActionID);
                    }
                    else if (paytype == 4)
                    {
                        obj.PayoutType = PayoutType.CPC;
                        obj.ActionID = null;
                    }
                    else if (paytype == 5)
                    {
                        obj.PayoutType = PayoutType.CPM;
                        obj.ActionID = null;
                    }

                    obj.PayoutPercent = _PayoutPercent;
                    obj.Payout = _Payout;
                    db.SaveChanges();
                }
                else
                {

                }



            }


            return null;
        }

        public OverridePayout FindActionByOverride(int id)
        {
            OverridePayout obj = db.OverridePayout.Where(u => u.OverridID == id).FirstOrDefault();
            return obj;
        }
        public AffiliateOverridePayout FindAffiliateOverride(int id)
        {
            AffiliateOverridePayout obj = db.AffiliateOverridePayout.Where(u => u.OverridID == id).FirstOrDefault();
            return obj;
        }

        public OverrideAffiliate FindAffiliatePayout(int id)
        {
            OverrideAffiliate obj = db.OverrideAffiliate.Where(u => u.OverridID == id).FirstOrDefault();
            return obj;
        }

        public List<Affiliate> GetAffiliateByCustomer(int CustomerID)
        {
            var GetAffiliate = db.Affiliates.Where(u => u.CustomerId == CustomerID && u.Status == AffiliateStatus.Active).OrderBy(u => u.AffiliateId).ToList();
            List<Affiliate> obj = new List<Affiliate>();
            for (int a = 0; a < GetAffiliate.Count; a++)
            {
                obj.Add(new Affiliate { AffiliateId = GetAffiliate[a].AffiliateId, Company = GetAffiliate[a].AffiliateId + "-" + GetAffiliate[a].Company });
            }
            return obj;

        }

        public OverridePayout CheckOverridebyCampaign(int CampaignID, int customerId)
        {
            //var customerId = GetCurrentCustomer().CustomerId;
            var GetResult = db.OverridePayout.Where(u => u.CampaignID == CampaignID && u.CustomerID == customerId).FirstOrDefault();
            return GetResult;
        }

        public OverridePayout CheckOverridebyAction(int? ActionID, int? CustomerId)
        {
            var customerId = 0;
            if (CustomerId.HasValue)
                customerId = CustomerId.Value;
            else
                customerId = GetCurrentCustomer().CustomerId;
            var GetResult = db.OverridePayout.Where(u => u.ActionID == ActionID && u.CustomerID == customerId).FirstOrDefault();
            return GetResult;
        }

        public List<DispAffiliateOverride> GetAffiliateOverride(int AffiliateId, int CustomerId)
        {
            var Result = (from op in db.AffiliateOverridePayout
                          where op.CustomerID == CustomerId && op.AffiliateID == AffiliateId
                          select new DispAffiliateOverride
                          {
                              OverrideID = op.OverridID,
                              PayoutType = op.PayoutType,
                              Payout = op.Payout,
                              PayoutPercent = op.PayoutPercent,
                              RevenueType = op.RevenueType,
                              Revenue = op.Revenue,
                              RevenuePercent = op.RevenuePercent
                          }).ToList();
            return Result;
        }
        public List<AffiliateOverridePayout> CheckAffiliateOverride(int customerid, int affiliateid)
        {
            var GetOverrideaffiliate = db.AffiliateOverridePayout.Where(u => u.CustomerID == customerid && u.AffiliateID == affiliateid).ToList();
            return GetOverrideaffiliate;
        }

        public Byte[] SaveProfilePic(byte[] Image)
        {
            int userid = GetCurrentUserId();
            UserProfilePic tbl = db.UserProfilePics.Where(u => u.UserId == userid).FirstOrDefault();
            if (tbl == null)
            {
                UserProfilePic tbl1 = new UserProfilePic();
                tbl1.UserId = userid;
                tbl1.ProfilePic = Image;
                db.UserProfilePics.Add(tbl1);

            }
            else
            {
                tbl.ProfilePic = Image;
            }
            db.SaveChanges();
            return Image;
        }

        public Byte[] GetProfilePic()
        {
            int userid = GetCurrentUserId();
            UserProfilePic tbl = db.UserProfilePics.Where(u => u.UserId == userid).FirstOrDefault();
            if (tbl != null)
            { return tbl.ProfilePic; }
            else
            { return null; }
        }

        public List<UserCustomReport> SaveUserCustomReport(string ReportName, string ReportData, string ColumOrder, int ReportID)
        {
            try
            {
                List<UserCustomReport> lst = new List<UserCustomReport>();
                var up = GetCurrentUser();
                if (ReportID == 0)
                {
                    UserCustomReport tbl = new UserCustomReport();
                    tbl.UserId = up.UserId;
                    tbl.CustomerID = up.CustomerId;
                    tbl.ReportName = ReportName;
                    tbl.ReportData = ReportData;
                    tbl.ColumnOrder = ColumOrder;
                    db.UserCustomReports.Add(tbl);
                    db.SaveChanges();
                }
                else
                {
                    UserCustomReport tbl = db.UserCustomReports.Where(u => u.ID == ReportID && u.CustomerID == up.CustomerId && u.UserId == up.UserId).FirstOrDefault();
                    if (tbl != null)
                    {
                        tbl.UserId = up.UserId;
                        tbl.CustomerID = up.CustomerId;
                        tbl.ReportName = ReportName;
                        tbl.ReportData = ReportData;
                        tbl.ColumnOrder = ColumOrder;
                        db.SaveChanges();
                    }
                }
                lst = db.UserCustomReports.Where(u => u.CustomerID == up.CustomerId && u.UserId == up.UserId).ToList();
                return lst;
            }
            catch (Exception ex)
            { return null; }

        }

        public List<UserCustomReport> GETUserCustomReport()
        {
            try
            {
                var up = GetCurrentUser();
                var GetList = db.UserCustomReports.Where(u => u.UserId == up.UserId && u.CustomerID == up.CustomerId).ToList();
                return GetList;
            }
            catch (Exception ex)
            { return null; }

        }
        public IQueryable<UserCustomReport> GetCustomReports(int customerid, int userID)
        {

            var Result = db.UserCustomReports.Where(u => u.UserId == userID && u.CustomerID == customerid);
            return Result;
        }
        public IQueryable<UserCustomReport> GetCustomReportByID(int customerid, int userID, int reportId)
        {
            var Result = db.UserCustomReports.Where(u => u.ID == reportId && u.UserId == userID && u.CustomerID == customerid);
            if (Result.Count() == 0) { return null; }
            return Result;
        }
        public UserCustomReport FindCustomReport(int ReportId)
        {
            return db.UserCustomReports.Find(ReportId);
        }
        public Boolean DeleteCustomReports(int customerid, int userID, int reportId)
        {
            var GetReport = db.UserCustomReports.Where(u => u.ID == reportId && u.UserId == userID && u.CustomerID == customerid).FirstOrDefault();
            db.UserCustomReports.Remove(GetReport);
            db.SaveChanges();
            return true;
        }

        public UserAgentInfo GetUserAgentDetails(String userAgent, MatchMode mode)
        {
            // Call WURFL
            var startTime = DateTime.Now;

            //var mgr = WURFLManagerBuilder.Build(new ApplicationConfigurer());
            var deviceInfo = WURFLManagerBuilder.Instance.GetDeviceForRequest(userAgent, mode);    // or if you like it more ...
            var endTime = DateTime.Now;

            // Prepare response 
            var model = new UserAgentInfo
            {
                Title = "WURFL Explorer",
                RequestDuration = String.Format("{0} ms", (endTime - startTime).TotalMilliseconds),
                UserAgent = userAgent,
                NormalizedUserAgent = deviceInfo.NormalizedUserAgent,
                DataVersion = WURFLManagerBuilder.Instance.GetWurflInfo().WurflVersion,
                DataLastUpdated = WURFLManagerBuilder.Instance.GetWurflInfo().WurflVersion,
                ApiVersion = WURFLManagerBuilder.Instance.GetWurflInfo().ApiVersion,
                FiltersForDisplay = String.Format("<b>Groups: </b>{0}, <b>Caps: </b> {1}",
                                                  String.Join(",", WURFLManagerBuilder.Instance.GetWurflInfo().FilterGroups),
                                                  String.Join(",", WURFLManagerBuilder.Instance.GetWurflInfo().FilterCapabilities)),
                DeviceId = deviceInfo.Id,
                DeviceRootId = deviceInfo.FallbackId,
                WorkingMode = WURFLManagerBuilder.Instance.GetMatchMode().ToString(),
                Matcher = deviceInfo.GetMatcher(),
                MatchingMethod = deviceInfo.GetMethod(),
                Capabilities = deviceInfo.GetCapabilities()
            };
            model.TotalDevices = WURFLManagerBuilder.Instance.GetAllDevices().Count;

            // Virtual
            model.VirtualIsSmartphone = deviceInfo.GetVirtualCapability("is_smartphone");
            model.VirtualOs = String.Format("{0} {1}", deviceInfo.GetVirtualCapability("advertised_device_os"), deviceInfo.GetVirtualCapability("advertised_device_os_version"));
            model.VirtualBrowser = String.Format("{0} {1}", deviceInfo.GetVirtualCapability("advertised_browser"), deviceInfo.GetVirtualCapability("advertised_browser_version"));
            model.VirtualIsAndroid = deviceInfo.GetVirtualCapability("is_android");
            model.VirtualIsApple = deviceInfo.GetVirtualCapability("is_ios");
            model.VirtualIsNative = deviceInfo.GetVirtualCapability("is_app");

            return model;
        }

        public DeviceInfo addUserAgentInfos(string UserAgent)
        {
            DeviceInfo tbl = new DeviceInfo();
            try
            {
                var AgentObj = GetUserAgentDetails(UserAgent, MatchMode.Accuracy);

                tbl.DeviceId = AgentObj.DeviceId;
                tbl.IsSmartphone = Convert.ToBoolean(AgentObj.VirtualIsSmartphone);
                tbl.IsiOS = Convert.ToBoolean(AgentObj.VirtualIsApple);
                tbl.IsAndroid = Convert.ToBoolean(AgentObj.VirtualIsAndroid);
                tbl.OS = AgentObj.VirtualOs;
                tbl.Browser = AgentObj.VirtualBrowser;
                if (AgentObj.Capabilities.ContainsKey("device_os"))
                { tbl.Device_os = AgentObj.Capabilities["device_os"]; }
                if (AgentObj.Capabilities.ContainsKey("pointing_method"))
                { tbl.Pointing_method = AgentObj.Capabilities["pointing_method"]; }

                if (AgentObj.Capabilities.ContainsKey("is_tablet"))
                { tbl.Is_tablet = Convert.ToBoolean(AgentObj.Capabilities["is_tablet"]); }

                if (AgentObj.Capabilities.ContainsKey("model_name"))
                { tbl.Model_name = AgentObj.Capabilities["model_name"]; }
                if (AgentObj.Capabilities.ContainsKey("device_os_version"))
                { tbl.Device_os_version = AgentObj.Capabilities["device_os_version"]; }

                tbl.Is_wireless_device = Convert.ToBoolean(AgentObj.VirtualIsSmartphone);

                if (AgentObj.Capabilities.ContainsKey("brand_name"))
                { tbl.Brand_name = AgentObj.Capabilities["brand_name"]; }
                if (AgentObj.Capabilities.ContainsKey("marketing_name"))
                { tbl.Marketing_name = AgentObj.Capabilities["marketing_name"]; }
                if (AgentObj.Capabilities.ContainsKey("can_assign_phone_number"))
                { tbl.Is_assign_phone_number = Convert.ToBoolean(AgentObj.Capabilities["can_assign_phone_number"]); }
                if (AgentObj.Capabilities.ContainsKey("xhtmlmp_preferred_mime_type"))
                { tbl.Xhtmlmp_mime_type = AgentObj.Capabilities["xhtmlmp_preferred_mime_type"]; }
                if (AgentObj.Capabilities.ContainsKey("xhtml_support_level"))
                { tbl.Xhtml_support_level = AgentObj.Capabilities["xhtml_support_level"]; }
                if (AgentObj.Capabilities.ContainsKey("resolution_height"))
                { tbl.Resolution_height = AgentObj.Capabilities["resolution_height"]; }
                if (AgentObj.Capabilities.ContainsKey("resolution_width"))
                { tbl.Resolution_width = AgentObj.Capabilities["resolution_width"]; }
                if (AgentObj.Capabilities.ContainsKey("canvas_support"))
                { tbl.Canvas_support = AgentObj.Capabilities["canvas_support"]; }
                if (AgentObj.Capabilities.ContainsKey("viewport_width"))
                { tbl.Viewport_width = AgentObj.Capabilities["viewport_width"]; }
                if (AgentObj.Capabilities.ContainsKey("html_preferred_dtd"))
                { tbl.Html_preferred_dtd = AgentObj.Capabilities["html_preferred_dtd"]; }
                if (AgentObj.Capabilities.ContainsKey("viewport_supported"))
                { tbl.Isviewport_supported = Convert.ToBoolean(AgentObj.Capabilities["viewport_supported"]); }
                if (AgentObj.Capabilities.ContainsKey("mobileoptimized"))
                { tbl.Ismobileoptimized = Convert.ToBoolean(AgentObj.Capabilities["mobileoptimized"]); }
                if (AgentObj.Capabilities.ContainsKey("image_inlining"))
                { tbl.Isimage_inlining = Convert.ToBoolean(AgentObj.Capabilities["image_inlining"]); }
                if (AgentObj.Capabilities.ContainsKey("handheldfriendly"))
                { tbl.Ishandheldfriendly = Convert.ToBoolean(AgentObj.Capabilities["handheldfriendly"]); }
                if (AgentObj.Capabilities.ContainsKey("is_smarttv"))
                { tbl.Is_smarttv = Convert.ToBoolean(AgentObj.Capabilities["is_smarttv"]); }
                if (AgentObj.Capabilities.ContainsKey("ux_full_desktop"))
                { tbl.Isux_full_desktop = Convert.ToBoolean(AgentObj.Capabilities["ux_full_desktop"]); }

                db.DeviceInfo.Add(tbl);
                db.SaveChanges();
            }
            catch (Exception ex)
            { return null; }

            return tbl;
        }

        public List<SelectListItem> GetDevice_DeviceID()
        {
            var Result = db.DeviceInfo.Select(u => new SelectListItem { Text = u.DeviceId, Value = u.DeviceId }).Distinct().ToList();
            return Result;
        }
        public List<SelectListItem> GetDevice_OS()
        {
            var Result = db.DeviceInfo.Select(u => new SelectListItem { Text = u.OS, Value = u.OS }).Distinct().ToList();
            return Result;
        }
        public List<SelectListItem> GetDevice_Browser()
        {
            var Result = db.DeviceInfo.Select(u => new SelectListItem { Text = u.Browser, Value = u.Browser }).Distinct().ToList();
            return Result;
        }
        public List<SelectListItem> GetDevice_DeviceOS()
        {
            var Result = db.DeviceInfo.Select(u => new SelectListItem { Text = u.Device_os, Value = u.Device_os }).Distinct().ToList();
            return Result;
        }
        public List<SelectListItem> GetDevice_ModelName()
        {
            var Result = db.DeviceInfo.Select(u => new SelectListItem { Text = u.Model_name, Value = u.Model_name }).Distinct().ToList();
            return Result;
        }
        public List<SelectListItem> GetDevice_BrandName()
        {
            var Result = db.DeviceInfo.Select(u => new SelectListItem { Text = u.Brand_name, Value = u.Brand_name }).Distinct().ToList();
            return Result;
        }
        public List<SelectListItem> GetDevice_MarketingName()
        {
            var Result = db.DeviceInfo.Select(u => new SelectListItem { Text = u.Marketing_name, Value = u.Marketing_name }).Distinct().ToList();
            return Result;
        }
        public List<SelectListItem> GetDevice_Resolution()
        {
            var Result = db.DeviceInfo.Select(u => new SelectListItem { Text = u.Resolution_height, Value = u.Resolution_height }).Distinct().ToList();
            return Result;
        }

        public List<SelectListItem> GetUserAgent()
        {
            var Result = db.Clicks.Where(u => u.ClickDate == DateTime.Today).Select(u => new SelectListItem { Text = u.UserAgent, Value = u.UserAgent }).Distinct().ToList();
            return Result;
        }
        public IQueryable<DeviceInfo> DeviceInfos()
        {
            return db.DeviceInfo;
        }

        public string bulkinsert(int start, int end)
        {
            var str = new StringBuilder();
            try
            {
                var getClicks = db.Clicks.Where(u => u.UserAgent != null && u.ClickId >= start && u.ClickId <= end).Select(s => new { ClickId = s.ClickId, UA = s.UserAgent }).ToList();

                for (int a = 0; a < getClicks.Count; a++)
                {
                    var clickId = getClicks[a].ClickId;
                    var AgentObj = GetUserAgentDetails(getClicks[a].UA, MatchMode.Accuracy);
                    DeviceInfo tbl = new DeviceInfo();

                    tbl.DeviceId = AgentObj.DeviceId;
                    tbl.IsSmartphone = Convert.ToBoolean(AgentObj.VirtualIsSmartphone);
                    tbl.IsiOS = Convert.ToBoolean(AgentObj.VirtualIsApple);
                    tbl.IsAndroid = Convert.ToBoolean(AgentObj.VirtualIsAndroid);
                    tbl.OS = AgentObj.VirtualOs;
                    tbl.Browser = AgentObj.VirtualBrowser;
                    if (AgentObj.Capabilities.ContainsKey("device_os"))
                    { tbl.Device_os = AgentObj.Capabilities["device_os"]; }
                    if (AgentObj.Capabilities.ContainsKey("pointing_method"))
                    { tbl.Pointing_method = AgentObj.Capabilities["pointing_method"]; }

                    tbl.Is_tablet = Convert.ToBoolean(AgentObj.VirtualIsSmartphone);

                    if (AgentObj.Capabilities.ContainsKey("model_name"))
                    { tbl.Model_name = AgentObj.Capabilities["model_name"]; }
                    if (AgentObj.Capabilities.ContainsKey("device_os_version"))
                    { tbl.Device_os_version = AgentObj.Capabilities["device_os_version"]; }

                    tbl.Is_wireless_device = Convert.ToBoolean(AgentObj.VirtualIsSmartphone);

                    if (AgentObj.Capabilities.ContainsKey("brand_name"))
                    { tbl.Brand_name = AgentObj.Capabilities["brand_name"]; }
                    if (AgentObj.Capabilities.ContainsKey("marketing_name"))
                    { tbl.Marketing_name = AgentObj.Capabilities["marketing_name"]; }
                    if (AgentObj.Capabilities.ContainsKey("can_assign_phone_number"))
                    { tbl.Is_assign_phone_number = Convert.ToBoolean(AgentObj.Capabilities["can_assign_phone_number"]); }
                    if (AgentObj.Capabilities.ContainsKey("xhtmlmp_preferred_mime_type"))
                    { tbl.Xhtmlmp_mime_type = AgentObj.Capabilities["xhtmlmp_preferred_mime_type"]; }
                    if (AgentObj.Capabilities.ContainsKey("xhtml_support_level"))
                    { tbl.Xhtml_support_level = AgentObj.Capabilities["xhtml_support_level"]; }
                    if (AgentObj.Capabilities.ContainsKey("resolution_height"))
                    { tbl.Resolution_height = AgentObj.Capabilities["resolution_height"]; }
                    if (AgentObj.Capabilities.ContainsKey("resolution_width"))
                    { tbl.Resolution_width = AgentObj.Capabilities["resolution_width"]; }
                    if (AgentObj.Capabilities.ContainsKey("canvas_support"))
                    { tbl.Canvas_support = AgentObj.Capabilities["canvas_support"]; }
                    if (AgentObj.Capabilities.ContainsKey("viewport_width"))
                    { tbl.Viewport_width = AgentObj.Capabilities["viewport_width"]; }
                    if (AgentObj.Capabilities.ContainsKey("html_preferred_dtd"))
                    { tbl.Html_preferred_dtd = AgentObj.Capabilities["html_preferred_dtd"]; }
                    if (AgentObj.Capabilities.ContainsKey("viewport_supported"))
                    { tbl.Isviewport_supported = Convert.ToBoolean(AgentObj.Capabilities["viewport_supported"]); }
                    if (AgentObj.Capabilities.ContainsKey("mobileoptimized"))
                    { tbl.Ismobileoptimized = Convert.ToBoolean(AgentObj.Capabilities["mobileoptimized"]); }
                    if (AgentObj.Capabilities.ContainsKey("image_inlining"))
                    { tbl.Isimage_inlining = Convert.ToBoolean(AgentObj.Capabilities["image_inlining"]); }
                    if (AgentObj.Capabilities.ContainsKey("handheldfriendly"))
                    { tbl.Ishandheldfriendly = Convert.ToBoolean(AgentObj.Capabilities["handheldfriendly"]); }
                    if (AgentObj.Capabilities.ContainsKey("is_smarttv"))
                    { tbl.Is_smarttv = Convert.ToBoolean(AgentObj.Capabilities["is_smarttv"]); }
                    if (AgentObj.Capabilities.ContainsKey("ux_full_desktop"))
                    { tbl.Isux_full_desktop = Convert.ToBoolean(AgentObj.Capabilities["ux_full_desktop"]); }

                    str.AppendFormat("INSERT INTO [cpaticker].[dbo].[DeviceInfoes] VALUES ('{0}',{1},{2},{3},'{4}','{5}','{6}','{7}',{8},'{9}','{10}',{11},'{12}','{13}',{14},'{15}','{16}','{17}','{18}','{19}','{20}','{21}',{22},{23},{24},{25},{26},{27})  UPDATE [cpaticker].[dbo].[Clicks] SET [UserAgentId] = SCOPE_IDENTITY() WHERE [ClickId]={28}"
                          , tbl.DeviceId
                          , tbl.IsSmartphone ? 1 : 0
                          , tbl.IsiOS ? 1 : 0
                          , tbl.IsAndroid ? 1 : 0
                          , tbl.OS
                          , tbl.Browser
                          , tbl.Device_os
                          , tbl.Pointing_method
                          , tbl.Is_tablet ? 1 : 0
                          , tbl.Model_name
                          , tbl.Device_os_version
                          , tbl.Is_wireless_device ? 1 : 0
                          , tbl.Brand_name
                          , tbl.Marketing_name
                          , tbl.Is_assign_phone_number ? 1 : 0
                          , tbl.Xhtmlmp_mime_type
                          , tbl.Xhtml_support_level
                          , tbl.Resolution_height
                          , tbl.Resolution_width
                          , tbl.Canvas_support
                          , tbl.Viewport_width
                          , tbl.Html_preferred_dtd
                          , tbl.Isviewport_supported ? 1 : 0
                          , tbl.Ismobileoptimized ? 1 : 0
                          , tbl.Isimage_inlining ? 1 : 0
                          , tbl.Ishandheldfriendly ? 1 : 0
                          , tbl.Is_smarttv ? 1 : 0
                          , tbl.Isux_full_desktop ? 1 : 0
                          , clickId
                          );
                    // db.DeviceInfo.Add(tbl);
                    // Click obj = db.Clicks.Where(u => u.ClickId == clickId).FirstOrDefault();
                    // obj.UserAgentId = tbl.Id;
                    // db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

            }

            return str.ToString();
        }

        public List<DispOverrideAffiliate> GetAffiliatPayoutCampaign(int campaignID)
        {
            var customerid = GetCurrentCustomer().CustomerId;
            var Result = (from op in db.OverrideAffiliate
                          join aff in db.Affiliates on op.AffiliateID equals aff.AffiliateId
                          join cam in db.Campaigns on op.CampaignID equals cam.Id
                          join act in db.Actions on op.ActionID equals act.Id into actions
                          from acts in actions.DefaultIfEmpty()
                          where op.CampaignID == campaignID && op.CustomerID == customerid && aff.CustomerId == customerid
                          select new DispOverrideAffiliate
                          {
                              campaign = cam.CampaignId + " - " + cam.CampaignName,
                              action = acts.Name,
                              OverrideID = op.OverridID,
                              affiliate = aff.AffiliateId + " - " + aff.Company,
                              Payout = op.Payout,
                              PayoutPercent = op.PayoutPercent,
                              PayoutType = op.PayoutType,
                              Revenue = op.Revenue,
                              RevenuePercent = op.RevenuePercent,
                              RevenueType = op.RevenueType
                          }).Distinct().ToList();
            return Result;
        }
        public int addAffiliateOverrideCampaign(string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent, string _RevenueType, string Revenue, string RevenuePercent)
        {
            Decimal? _PayoutPercent = null;
            Decimal? _Payout = null;
            if (PayoutPercent != "")
            {
                _PayoutPercent = Convert.ToDecimal(PayoutPercent);
            }

            if (Payout != "")
            {
                _Payout = Convert.ToDecimal(Payout);
            }

            int paytype = 0;

            if (_PayoutType == "CPS")
            {
                paytype = 2;
                _Payout = null;
            }
            else if (_PayoutType == "CPA")
            {
                paytype = 1;
                _PayoutPercent = null;

            }
            else if (_PayoutType == "CPA_CPS")
            {
                paytype = 3;

            }
            else if (_PayoutType == "CPC")
            {
                paytype = 4;
                _PayoutPercent = null;
            }
            else if (_PayoutType == "CPM")
            {
                paytype = 5;
                _PayoutPercent = null;
            }

            Decimal? _RevenuePercent = null;
            Decimal? _Revenue = null;
            if (RevenuePercent != "")
            {
                _RevenuePercent = Convert.ToDecimal(RevenuePercent);
            }

            if (Revenue != "")
            {
                _Revenue = Convert.ToDecimal(Revenue);
            }

            int revenuetype = 0;

            if (_RevenueType == "RPS")
            {
                revenuetype = 2;
                _Revenue = null;
            }
            else if (_RevenueType == "RPA")
            {
                revenuetype = 1;
                _RevenuePercent = null;

            }
            else if (_RevenueType == "RPA_RPS")
            {
                revenuetype = 3;

            }
            else if (_RevenueType == "RPC")
            {
                revenuetype = 4;
                _RevenuePercent = null;
            }
            else if (_RevenueType == "RPM")
            {
                revenuetype = 5;
                _RevenuePercent = null;
            }

            OverrideAffiliate obj = new OverrideAffiliate();
            // obj.ActionID = Convert.ToInt32(ActionID);
            if (!string.IsNullOrWhiteSpace(AffiliateID))
                obj.AffiliateID = Convert.ToInt32(AffiliateID);

            obj.CustomerID = GetCurrentCustomerId();

            if (!string.IsNullOrWhiteSpace(ActionID))
                obj.ActionID = Convert.ToInt32(ActionID);

            if (!string.IsNullOrWhiteSpace(CampaignID))
                obj.CampaignID = Convert.ToInt32(CampaignID);
            if (paytype == 1)
            {
                obj.PayoutType = PayoutType.CPA;
            }
            else if (paytype == 2)
            {
                obj.PayoutType = PayoutType.CPS;
            }
            else if (paytype == 3)
            {
                obj.PayoutType = PayoutType.CPA_CPS;
            }
            else if (paytype == 4)
            {
                obj.PayoutType = PayoutType.CPC;
            }
            else if (paytype == 5)
            {
                obj.PayoutType = PayoutType.CPM;
            }
            if (_PayoutPercent != null)
            { obj.PayoutPercent = _PayoutPercent; }

            if (_Payout != null)
            { obj.Payout = _Payout; }



            if (revenuetype == 1)
            {
                obj.RevenueType = RevenueType.RPA;
            }
            else if (revenuetype == 2)
            {
                obj.RevenueType = RevenueType.RPS;
            }
            else if (revenuetype == 3)
            {
                obj.RevenueType = RevenueType.RPA_RPS;
            }
            else if (revenuetype == 4)
            {
                obj.RevenueType = RevenueType.RPC;
            }
            else if (revenuetype == 5)
            {
                obj.RevenueType = RevenueType.RPM;
            }
            if (_RevenuePercent != null)
            { obj.RevenuePercent = _RevenuePercent; }

            if (_Revenue != null)
            { obj.Revenue = _Revenue; }

            db.OverrideAffiliate.Add(obj);
            db.SaveChanges();

            return obj.CampaignID.Value;
        }

        public int updateAffiliateOverrideCampaign(int OverrideId, string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent, string _RevenueType, string Revenue, string RevenuePercent)
        {
            Decimal? _PayoutPercent = null;
            Decimal? _Payout = null;
            if (PayoutPercent != "")
            {
                _PayoutPercent = Convert.ToDecimal(PayoutPercent);
            }

            if (Payout != "")
            {
                _Payout = Convert.ToDecimal(Payout);
            }

            int paytype = 0;

            if (_PayoutType == "CPS")
            {
                paytype = 2;
                _Payout = null;
            }
            else if (_PayoutType == "CPA")
            {
                paytype = 1;
                _PayoutPercent = null;

            }
            else if (_PayoutType == "CPA_CPS")
            {
                paytype = 3;

            }
            else if (_PayoutType == "CPC")
            {
                paytype = 4;
                _PayoutPercent = null;
            }
            else if (_PayoutType == "CPM")
            {
                paytype = 5;
                _PayoutPercent = null;
            }

            Decimal? _RevenuePercent = null;
            Decimal? _Revenue = null;
            if (RevenuePercent != "")
            {
                _RevenuePercent = Convert.ToDecimal(RevenuePercent);
            }

            if (Revenue != "")
            {
                _Revenue = Convert.ToDecimal(Revenue);
            }

            int revenuetype = 0;

            if (_RevenueType == "RPS")
            {
                revenuetype = 2;
                _Revenue = null;
            }
            else if (_RevenueType == "RPA")
            {
                revenuetype = 1;
                _RevenuePercent = null;

            }
            else if (_RevenueType == "RPA_RPS")
            {
                revenuetype = 3;

            }
            else if (_RevenueType == "RPC")
            {
                revenuetype = 4;
                _RevenuePercent = null;
            }
            else if (_RevenueType == "RPM")
            {
                revenuetype = 5;
                _RevenuePercent = null;
            }

            OverrideAffiliate obj = db.OverrideAffiliate.Where(u => u.OverridID == OverrideId).FirstOrDefault();
            // obj.ActionID = Convert.ToInt32(ActionID);
            if (!string.IsNullOrWhiteSpace(AffiliateID))
                obj.AffiliateID = Convert.ToInt32(AffiliateID);

            obj.CustomerID = GetCurrentCustomerId();

            if (!string.IsNullOrWhiteSpace(ActionID))
                obj.ActionID = Convert.ToInt32(ActionID);

            if (!string.IsNullOrWhiteSpace(CampaignID))
                obj.CampaignID = Convert.ToInt32(CampaignID);
            if (paytype == 1)
            {
                obj.PayoutType = PayoutType.CPA;
            }
            else if (paytype == 2)
            {
                obj.PayoutType = PayoutType.CPS;
            }
            else if (paytype == 3)
            {
                obj.PayoutType = PayoutType.CPA_CPS;
            }
            else if (paytype == 4)
            {
                obj.PayoutType = PayoutType.CPC;
            }
            else if (paytype == 5)
            {
                obj.PayoutType = PayoutType.CPM;
            }

            obj.PayoutPercent = _PayoutPercent;

            obj.Payout = _Payout;



            if (revenuetype == 1)
            {
                obj.RevenueType = RevenueType.RPA;
            }
            else if (revenuetype == 2)
            {
                obj.RevenueType = RevenueType.RPS;
            }
            else if (revenuetype == 3)
            {
                obj.RevenueType = RevenueType.RPA_RPS;
            }
            else if (revenuetype == 4)
            {
                obj.RevenueType = RevenueType.RPC;
            }
            else if (revenuetype == 5)
            {
                obj.RevenueType = RevenueType.RPM;
            }
            obj.RevenuePercent = _RevenuePercent;

            obj.Revenue = _Revenue;

            db.SaveChanges();

            return obj.CampaignID.Value;
        }

        public IEnumerable<CustomTimeZone> getTimeZoneList(int userID)
        {
            var GetResult = db.CustomTimeZone.Where(u => u.UserID == userID);
            return GetResult;
        }
        public CustomTimeZone AddCustomTimezone(CustomTimeZone model)
        {
            try
            {
                db.CustomTimeZone.Add(model);
                db.SaveChanges();
                return model;
            }
            catch (Exception ex)
            { return null; }
        }

        public CustomTimeZone GetTimezoneById(int id)
        {
            return db.CustomTimeZone.Where(u => u.ID == id).FirstOrDefault();
        }

        public CustomTimeZone EditCustomTimezone(CustomTimeZone model)
        {
            try
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return model;
            }
            catch (Exception ex)
            { return null; }
        }
        public int Getcustomoffset(int userId, string timezone)
        {
            int Cusoffset = 0;
            var GetCustomtimezone = db.CustomTimeZone.Where(u => u.UserID == userId && u.DisplayName == timezone).ToList();
            if (GetCustomtimezone.Count > 0)
            {
                Cusoffset = GetCustomtimezone[0].offset;

            }
            return Cusoffset;
        }

        public List<SelectListItem> GetCustomTimezoneByUser(int userId)
        {
            var GetData = (from ou in db.CustomTimeZone
                           where ou.UserID == userId
                           select new SelectListItem
                           {
                               Text = ou.DisplayName,
                               Value = ou.DisplayName
                           }).ToList();
            return GetData;
        }
        public List<SelectListItem> GetSources()
        {
            var Allsource = (from ou in db.Clicks
                             where ou.Source != null && ou.Source != "" && ou.Source != "{lpurl}"
                             select new SelectListItem
                             {
                                 Text = ou.Source,
                                 Value = ou.Source
                             }).Distinct().ToList();
            return Allsource;
        }

        public bool GetCustomerByURL(Uri url)
        {
            int customerid = 0;
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                // if i'm here is because that domain wasn't found so ...
                if (host.Split('.').Length > 2)
                {
                    // this means that this has a subdomain
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);
                    string subdomain = host.Substring(0, index);

                    try
                    {
                        customerid = db.Customers.Single(c => c.AccountId == subdomain).CustomerId;
                        return true;

                    }
                    catch
                    {
                        return false;
                        //throw new HttpException(404, "NotFound");
                    }
                }
            }
            return false;
        }

        public IQueryable<Affiliate> GetUserAffiliates(int userid, int CustomerId)
        {
            var GetAffiliate = from au in db.UserAffiliate.Where(u => u.UserId == userid)
                               join affil in db.Affiliates.Where(u => u.CustomerId == CustomerId) on au.AffiliateId equals affil.AffiliateId
                               select affil;

            return GetAffiliate;
            //db.Affiliates.Where(a => a.CustomerId == customerid);
        }

        public IQueryable<RedirectTargetPage> RedirectTargetPages()
        {
            return db.RedirectTargetsPage;
        }
        public IQueryable<RedirectPAGE> RedirectPages()
        {
            return db.RedirectPages;
        }
        public void AddRedirectPage(RedirectPAGE ru)
        {
            db.RedirectPages.Add(ru);
            db.SaveChanges();
        }

        public void AddRedirectTargetPages(RedirectTargetPage redirectTarget)
        {
            db.RedirectTargetsPage.Add(redirectTarget);
            db.SaveChanges();
        }
        public RedirectPAGE FindRedirectPage(int id)
        {
            return db.RedirectPages.Find(id);
        }
        public void EditRedirectPage(RedirectPAGE ru)
        {
            db.Entry(ru).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void DeleteRedirectTargetsPages(int id)
        {
            db.RedirectTargetsPage.RemoveRange(db.RedirectTargetsPage.Where(t => t.RedirectPageId == id));
            db.SaveChanges();
        }
        public void DeleteRedirectPage(RedirectPAGE ru)
        {
            db.RedirectPages.Remove(ru);
            db.SaveChanges();
        }

        public List<permissionlist> GetPermissionNames()
        {
            List<permissionlist> obj = new List<permissionlist>();

            obj.Add(new permissionlist { name = "UpDate Override Payout Campaign", value = "updateoverridepayoutCampaign", permission = 0 });
            obj.Add(new permissionlist { name = "List Redirect", value = "indexRedirect", permission = 1 });
            obj.Add(new permissionlist { name = "Create Redirect", value = "createRedirect", permission = 2 });
            obj.Add(new permissionlist { name = "Edit Redirect", value = "editRedirect", permission = 4 });
            obj.Add(new permissionlist { name = "List URL", value = "indexURL", permission = 8 });
            obj.Add(new permissionlist { name = "Create URL", value = "createURL", permission = 16 });
            obj.Add(new permissionlist { name = "Edit URL", value = "editURL", permission = 32 });
            obj.Add(new permissionlist { name = "List Banner", value = "indexBanner", permission = 64 });
            obj.Add(new permissionlist { name = "Details Banner", value = "detailsBanner", permission = 128 });
            obj.Add(new permissionlist { name = "Create Banner", value = "createBanner", permission = 256 });
            obj.Add(new permissionlist { name = "Remove Banner", value = "removeBanner", permission = 512 });
            obj.Add(new permissionlist { name = "Edit Banner", value = "editBanner", permission = 1024 });
            obj.Add(new permissionlist { name = "List ConversionPixel", value = "indexConversionPixel", permission = 2048 });
            obj.Add(new permissionlist { name = "Create ConversionPixel", value = "createConversionPixel", permission = 4096 });
            obj.Add(new permissionlist { name = "Edit ConversionPixel", value = "editConversionPixel", permission = 8192 });
            obj.Add(new permissionlist { name = "Delete ConversionPixel", value = "deleteConversionPixel", permission = 16384 });
            obj.Add(new permissionlist { name = "CampaignsConversionPixel", value = "campaignsConversionPixel", permission = 32768 });
            obj.Add(new permissionlist { name = "Add CampaignConversionPixel", value = "addcampaignConversionPixel", permission = 65536 });
            obj.Add(new permissionlist { name = "Edit StatusConversionPixel", value = "editstatusConversionPixel", permission = 131072 });
            obj.Add(new permissionlist { name = "ActionsConversionPixel", value = "actionsConversionPixel", permission = 262144 });
            obj.Add(new permissionlist { name = "Add ActionConversionPixel", value = "addactionConversionPixel", permission = 524288 });
            obj.Add(new permissionlist { name = "Dashboard", value = "dashboardHome", permission = 1048576 });
            obj.Add(new permissionlist { name = "List Ticker", value = "indexTicker", permission = 2097152 });
            obj.Add(new permissionlist { name = "Create Ticker", value = "createTicker", permission = 4194304 });
            obj.Add(new permissionlist { name = "Edit Ticker", value = "editTicker", permission = 8388608 });
            obj.Add(new permissionlist { name = "Elements Ticker", value = "elementsTicker", permission = 16777216 });
            obj.Add(new permissionlist { name = "List Affiliate", value = "indexAffiliate", permission = 33554432 });
            obj.Add(new permissionlist { name = "Details Affiliate", value = "detailsAffiliate", permission = 67108864 });
            obj.Add(new permissionlist { name = "Create Affiliate", value = "createAffiliate", permission = 134217728 });
            obj.Add(new permissionlist { name = "Edit Affiliate", value = "editAffiliate", permission = 268435456 });
            obj.Add(new permissionlist { name = "Adjust StatsAffiliate", value = "adjuststatsAffiliate", permission = 536870912 });
            obj.Add(new permissionlist { name = "Adjust Affiliate", value = "adjustAffiliate", permission = 1073741824 });
            obj.Add(new permissionlist { name = "UsersAffiliate", value = "usersAffiliate", permission = 2147483648 });
            obj.Add(new permissionlist { name = "Add UserAffiliate", value = "adduserAffiliate", permission = 4294967296 });
            obj.Add(new permissionlist { name = "List Campaign", value = "indexCampaign", permission = 8589934592 });
            obj.Add(new permissionlist { name = "Details Campaign", value = "detailsCampaign", permission = 17179869184 });
            obj.Add(new permissionlist { name = "Create Campaign", value = "createCampaign", permission = 34359738368 });
            obj.Add(new permissionlist { name = "Edit Campaign", value = "editCampaign", permission = 68719476736 });
            obj.Add(new permissionlist { name = "Affiliate Reports", value = "affiliateReports", permission = 137438953472 });
            obj.Add(new permissionlist { name = "ConversionStatus Reports", value = "conversionstatusReports", permission = 274877906944 });
            obj.Add(new permissionlist { name = "Conversion Reports", value = "conversionReports", permission = 549755813888 });
            obj.Add(new permissionlist { name = "Campaign Reports", value = "campaignReports", permission = 1099511627776 });
            obj.Add(new permissionlist { name = "Daily Reports", value = "dailyReports", permission = 2199023255552 });
            obj.Add(new permissionlist { name = "Adcampaign Reports", value = "adcampaignReports", permission = 4398046511104 });
            obj.Add(new permissionlist { name = "Hourly Reports", value = "hourlyReports", permission = 8796093022208 });
            obj.Add(new permissionlist { name = "Clickslogs Reports", value = "clickslogsReports", permission = 17592186044416 });
            obj.Add(new permissionlist { name = "Traffic Reports", value = "trafficReports", permission = 35184372088832 });
            obj.Add(new permissionlist { name = "Ctr Reports", value = "ctrReports", permission = 70368744177664 });
            obj.Add(new permissionlist { name = "Settings", value = "indexSettings", permission = 140737488355328 });
            obj.Add(new permissionlist { name = "Edit UserSettings", value = "edituserSettings", permission = 281474976710656 });
            obj.Add(new permissionlist { name = "Email Settings", value = "emailSettings", permission = 562949953421312 });
            obj.Add(new permissionlist { name = "Edit Settings", value = "editSettings", permission = 1125899906842624 });
            obj.Add(new permissionlist { name = "Add UserSettings", value = "adduserSettings", permission = 2251799813685248 });
            obj.Add(new permissionlist { name = "Setpermissions Settings", value = "setpermissionsSettings", permission = 4503599627370496 });
            obj.Add(new permissionlist { name = "Addcustomerdomain Settings", value = "addcustomerdomainSettings", permission = 9007199254740992 });
            obj.Add(new permissionlist { name = "Addemployeeip Settings", value = "addemployeeipSettings", permission = 18014398509481984 });
            obj.Add(new permissionlist { name = "Changepwd Settings", value = "changepwdSettings", permission = 36028797018963968 });
            obj.Add(new permissionlist { name = "Resetpwd Settings", value = "resetpwdSettings", permission = 72057594037927936 });
            obj.Add(new permissionlist { name = "Addcustomfield Settings", value = "addcustomfieldSettings", permission = 144115188075855872 });
            obj.Add(new permissionlist { name = "Editcustomfield Settings", value = "editcustomfieldSettings", permission = 288230376151711744 });
            obj.Add(new permissionlist { name = "Override payout Campaign", value = "overridepayoutCampaign", permission = 576460752303423488 });
            obj.Add(new permissionlist { name = "Create override payout Campaign", value = "createoverridepayoutCampaign", permission = 1152921504606846976 });
            obj.Add(new permissionlist { name = "Add overridepayout Campaign", value = "addoverridepayoutCampaign", permission = 2305843009213693952 });
            obj.Add(new permissionlist { name = "Edit overridepayout Campaign", value = "editoverridepayoutCampaign", permission = 4611686018427387904 });











            return obj;
        }
        public List<permissionlist> GetPermissionNames1()
        {
            List<permissionlist> obj1 = new List<permissionlist>();


            obj1.Add(new permissionlist { name = "List Action", value = "indexAction", permission = 1 });
            obj1.Add(new permissionlist { name = "Create Action", value = "createAction", permission = 2 });
            obj1.Add(new permissionlist { name = "Edit Action", value = "editAction", permission = 4 });
            obj1.Add(new permissionlist { name = "Tracking Action", value = "trackingAction", permission = 8 });
            obj1.Add(new permissionlist { name = "Pixels Action", value = "pixelsAction", permission = 16 });
            obj1.Add(new permissionlist { name = "Override Payout Action", value = "overridepayoutAction", permission = 32 });
            obj1.Add(new permissionlist { name = "Create Override Payout Action", value = "createoverridepayoutAction", permission = 64 });
            obj1.Add(new permissionlist { name = "Add Override Payout Action", value = "addoverridepayoutAction", permission = 128 });
            obj1.Add(new permissionlist { name = "Edit Override Payout Action", value = "editoverridepayoutAction", permission = 256 });
            obj1.Add(new permissionlist { name = "Update Override Payout Action", value = "updateoverridepayoutAction", permission = 512 });
            obj1.Add(new permissionlist { name = "Custom Reports", value = "customreportReports", permission = 1024 });
            obj1.Add(new permissionlist { name = "Manage Reports", value = "managereportReports", permission = 2048 });
            obj1.Add(new permissionlist { name = "Delete Manage Reports", value = "deletemanagereportReports", permission = 4096 });
            obj1.Add(new permissionlist { name = "Clicks Detailslog Reports", value = "clicksdetailslogReports", permission = 8192 });
            obj1.Add(new permissionlist { name = "Affiliate Payout Campaign", value = "affiliatepayoutCampaign", permission = 16384 });
            obj1.Add(new permissionlist { name = "Create Affiliate Payout Campaign", value = "createaffiliatepayoutCampaign", permission = 32768 });
            obj1.Add(new permissionlist { name = "Edit Affiliate Payout Campaign", value = "editaffiliatepayoutCampaign", permission = 65536 });
            obj1.Add(new permissionlist { name = "Add Affiliate Override Campaign", value = "addaffiliateoverrideCampaign", permission = 131072 });
            obj1.Add(new permissionlist { name = "Update Affiliate Override Campaign", value = "updateaffiliateoverrideCampaign", permission = 262144 });
            obj1.Add(new permissionlist { name = "Master Login", value = "admin_master_loginSettings", permission = 524288 });
            obj1.Add(new permissionlist { name = "Add Customtimezone Settings", value = "addcustomtimezoneSettings", permission = 1048576 });
            obj1.Add(new permissionlist { name = "Edit Customtimezone Settings", value = "editcustomtimezoneSettings", permission = 2097152 });
            obj1.Add(new permissionlist { name = "List PAGE", value = "indexPAGE", permission = 4194304 });
            obj1.Add(new permissionlist { name = "Create PAGE", value = "createPAGE", permission = 8388608 });
            obj1.Add(new permissionlist { name = "Edit PAGE", value = "editPAGE", permission = 16777216 });
            obj1.Add(new permissionlist { name = "Override Payout Affiliate", value = "overridepayoutAffiliate", permission = 33554432 });
            obj1.Add(new permissionlist { name = "Create Override Payout Affiliate", value = "createoverridepayoutAffiliate", permission = 67108864 });
            obj1.Add(new permissionlist { name = "Edit Override Payout Affiliate", value = "editoverridepayoutAffiliate", permission = 134217728 });
            obj1.Add(new permissionlist { name = "Add Affiliate Override Affiliate", value = "addaffiliateoverrideAffiliate", permission = 268435456 });
            obj1.Add(new permissionlist { name = "Update Affiliate Override Affiliate", value = "updateaffiliateoverrideAffiliate", permission = 536870912 });
            return obj1;

        }

        public IQueryable<PAGECategory> GetPageCategories()
        {
            return db.PageCategories.Where(t=>t.IsActive);
        }

        public PAGECategory SavePageCategory(PAGECategory category)
        {
            if (category.Id == 0)
            {
                db.PageCategories.Add(category);
                category.IsActive = true;
            }
            else
            {
                var item = db.PageCategories.Find(category.Id);
                item.Name = category.Name;
                item.IsActive = true;
            }
            
            db.SaveChanges();
            return category;
        }

        public int RemovePageCategory(int categoryId,bool force=false)// Soft delete
        {
            var item = db.PageCategories.Find(categoryId);

            if (item == null)
                return 0;

            if (force||!db.PAGEs.Any(p=>p.PageCategoryId==categoryId))
            {
                item.IsActive = false;
                db.SaveChanges();
            }
            else
            {
                return db.PAGEs.Count(p => p.PageCategoryId == categoryId);
            }

            return 0;
        }
    }

    public enum ActionType
    {
        Impression = 0,
        Click = 1,
        Conversion = 2
    }
    public class devicefilterddl
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
    public class permissionlist
    {
        public string name { get; set; }
        public string value { get; set; }
        public long permission { get; set; }
    }
}
