using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CpaTicker.Core;

namespace CpaTicker.Controllers
{
    public class ReportsController : ApiController
    {
        public readonly ICpaTickerRepository repo = null;

        public ReportsController()
        {
            this.repo = new EFCpatickerRepository();
        }

        private IHttpActionResult Daily(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata, long? dataview,
            int? cp, int[] af, int? ct, UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            StatisticsEnum stats;

            try
            {
                stats = (StatisticsEnum) dataview;
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


            if (up.AffiliateId.HasValue) // if is affiliate
            {
                if (af.Length == 0)
                {
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
            }
            if (af != null && af.Length == 0)
                af = null;

            string affs = af == null ? null : string.Join(",", af);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            // convert the time with the customer tzi to UTC 
            var ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                offsetHour = customoffset/60;
            }


            /*============================================================================================================*/

            #region Dynamic Statistics

            bool datecheck = true;
            bool hourcheck = false;
            bool campaigncheck = false; //string.IsNullOrEmpty(offview);
            bool affiliatecheck = false;
            bool urlcheck = false;

            bool countrycheck = stats.HasFlag(StatisticsEnum.Country);
            bool statuscheck = false;
            bool pixelcheck = false;
            bool postbackcheck = false;
            bool ipcheck = false;
            bool transactionidcheck = false;
            bool conversiontypecheck = false;
            bool referrercheck = false;
            bool statusdesccheck = false;
            bool sourcechk = false; //!string.IsNullOrEmpty(sourceview);
            bool actionchk = false;
            bool bannerchk = false;
            bool parenturlchk = false;
            bool ctrchk = false;


            /////////////---------Device Info--------------////////////////
            bool deviceid = false;
            bool issmartphone = false;
            bool isios = false;
            bool isandroid = false;
            bool os = false;
            bool browser = false;
            bool device_os = false;
            bool pointing_method = false;
            bool is_tablet = false;
            bool model_name = false;
            bool device_os_version = false;
            bool is_wireless_device = false;
            bool brand_name = false;
            bool marketing_name = false;
            bool resolution_height = false;
            bool resolution_width = false;
            bool canvas_support = false;
            bool viewport_width = false;
            bool isviewport_supported = false;
            bool ismobileoptimized = false;
            bool ishandheldfriendly = false;
            bool is_smarttv = false;
            bool isux_full_desktop = false;
            /////////////---------Device Info--------------////////////////


            //Filtering Device Infoes//
            string Vdeviceid = null;
            string Vdeviceos = null;
            string Vbrowser = null;
            string Vos = null;
            string Vmodelname = null;
            string Vbrandname = null;
            string Vmarketingname = null;
            string Vresolution = null;
            string UserAgent = null;
            //Filtering Device Infoes//


            // get the subids
            var debug = Request.RequestUri.ParseQueryString();
            var subids =
                Request.RequestUri.ParseQueryString()
                    .AllKeys.Where(s => s.ToLower().StartsWith("subid"))
                    .Select(s => int.Parse(s.Substring(5)))
                    .ToArray();

            string ctfilter = ct.HasValue
                ? repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

            #endregion

            #region Dynamic Type

            var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
            //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
            //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


            if (hourcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof (int));
            }
            if (datecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof (string));
            }

            if (campaigncheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof (string));
            }
            if (affiliatecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof (string));
            }

            if (urlcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof (string));
            }
            if (countrycheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof (string));
            }
            if (statuscheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof (int));
            }
            if (pixelcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof (DateTime?));
            }
            if (postbackcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof (DateTime?));
            }
            if (ipcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof (string));
            }
            if (transactionidcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof (string));
            }
            if (conversiontypecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof (ConversionType));
            }
            if (referrercheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof (string));
            }
            if (statusdesccheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof (string));
            }
            if (sourcechk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof (string));
            }
            if (actionchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof (string));
                DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof (int));
            }
            if (bannerchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof (string));
            }
            if (parenturlchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
            }
            if (ctrchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
            }


            ///////////////-----------Device Info-----------/////////////

            if (deviceid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof (string));
            }
            if (issmartphone)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof (string));
            }
            if (isios)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof (string));
            }
            if (isandroid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof (string));
            }
            if (os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof (string));
            }
            if (browser)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof (string));
            }
            if (device_os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof (string));
            }
            if (pointing_method)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof (string));
            }
            if (is_tablet)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof (string));
            }
            if (model_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof (string));
            }
            if (device_os_version)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof (string));
            }
            if (is_wireless_device)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof (string));
            }
            if (brand_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof (string));
            }
            if (marketing_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof (string));
            }
            if (resolution_height)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof (string));
            }
            if (resolution_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof (string));
            }
            if (canvas_support)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof (string));
            }
            if (viewport_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof (string));
            }
            if (isviewport_supported)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof (string));
            }
            if (ismobileoptimized)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof (string));
            }
            if (ishandheldfriendly)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof (string));
            }
            if (is_smarttv)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof (string));
            }
            if (isux_full_desktop)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof (string));
            }


            ///////////////-----------Device Info-----------/////////////


            DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof (decimal));
            DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof (decimal));
            foreach (var item in subids)
            {
                DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof (string));
            }
            Type resultType = affview.CreateType();

            #endregion

            var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, utdate, offsetHour, datecheck,
                hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck,
                ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk,
                bannerchk, parenturlchk, ctrchk,
                deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name,
                device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width,
                canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv,
                isux_full_desktop,
                subids.Length > 0 ? subids : null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname,
                Vmarketingname, Vresolution, UserAgent
                );

            var report = repo.ExecuteQuery(resultType, query);


            //var report = repo.RunQuery<DailyView>("EXEC [DailyRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
            //                    up.CustomerId, ufdate, utdate, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, affs, cp, ctfilter, countrycheck);

            return Ok(report);
            //var affs = af == null ? null : string.Join(",", af);
            //return repo.DailyReport(ufdate, utdate, up.CustomerId, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, affs, cp, ct); 
        }

        private IHttpActionResult Affiliate(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata,
            long? dataview, int? cp, int[] af, int? ct, UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            StatisticsEnum stats;

            //int statistics = 63;

            try
            {
                //string[] dataviewlist = dataview.Split(',');
                //statistics = int.Parse(dataviewlist[0]);
                stats = (StatisticsEnum) dataview;
            }
            catch
            {
                stats =
                    StatisticsEnum.Date |
                    StatisticsEnum.CR |
                    StatisticsEnum.CPC |
                    StatisticsEnum.RPC |
                    //   StatisticsEnum.Impressions |
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
            {
                if (af.Length == 0)
                {
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
            }
            if (af != null && af.Length == 0)
                af = null;

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            // convert the time with the customer tzi to UTC 
            var ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            int Offsethour = tzi.GetUtcOffset(DateTime.Now).Hours;
            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            if (customoffset != 0)
            {
                Offsethour = customoffset/60;
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
            }

            #region Dynamic Statistics

            bool datecheck = false;
            bool hourcheck = false;
            bool campaigncheck = stats.HasFlag(StatisticsEnum.Campaign); //string.IsNullOrEmpty(offview);
            bool affiliatecheck = true;
            bool urlcheck = false;

            var countrycheck = stats.HasFlag(StatisticsEnum.Country);
            bool statuscheck = false;
            bool pixelcheck = false;
            bool postbackcheck = false;
            bool ipcheck = false;
            bool transactionidcheck = false;
            bool conversiontypecheck = false;
            bool referrercheck = false;
            bool statusdesccheck = false;
            bool sourcechk = stats.HasFlag(StatisticsEnum.Source); //!string.IsNullOrEmpty(sourceview);
            bool actionchk = false;
            bool bannerchk = false;
            bool parenturlchk = false;
            bool ctrchk = false;


            /////////////---------Device Info--------------////////////////
            bool deviceid = false;
            bool issmartphone = false;
            bool isios = false;
            bool isandroid = false;
            bool os = false;
            bool browser = false;
            bool device_os = false;
            bool pointing_method = false;
            bool is_tablet = false;
            bool model_name = false;
            bool device_os_version = false;
            bool is_wireless_device = false;
            bool brand_name = false;
            bool marketing_name = false;
            bool resolution_height = false;
            bool resolution_width = false;
            bool canvas_support = false;
            bool viewport_width = false;
            bool isviewport_supported = false;
            bool ismobileoptimized = false;
            bool ishandheldfriendly = false;
            bool is_smarttv = false;
            bool isux_full_desktop = false;
            /////////////---------Device Info--------------////////////////


            //Filtering Device Infoes//
            string Vdeviceid = null;
            string Vdeviceos = null;
            string Vbrowser = null;
            string Vos = null;
            string Vmodelname = null;
            string Vbrandname = null;
            string Vmarketingname = null;
            string Vresolution = null;
            string UserAgent = null;
            //Filtering Device Infoes//


            // get the subids
            var debug = Request.RequestUri.ParseQueryString();
            var subids =
                Request.RequestUri.ParseQueryString()
                    .AllKeys.Where(s => s.ToLower().StartsWith("subid"))
                    .Select(s => int.Parse(s.Substring(5)))
                    .ToArray();

            string ctfilter = ct.HasValue
                ? repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

            #endregion

            #region Dynamic Type

            var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
            //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
            //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


            if (hourcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof (int));
            }
            if (datecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof (string));
            }

            if (campaigncheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof (string));
            }
            if (affiliatecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof (string));
            }

            if (urlcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof (string));
            }
            if (countrycheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof (string));
            }
            if (statuscheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof (int));
            }
            if (pixelcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof (DateTime?));
            }
            if (postbackcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof (DateTime?));
            }
            if (ipcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof (string));
            }
            if (transactionidcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof (string));
            }
            if (conversiontypecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof (ConversionType));
            }
            if (referrercheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof (string));
            }
            if (statusdesccheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof (string));
            }
            if (sourcechk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof (string));
            }
            if (actionchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof (string));
                DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof (int));
            }
            if (bannerchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof (string));
            }
            if (parenturlchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
            }
            if (ctrchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
            }


            ///////////////-----------Device Info-----------/////////////

            if (deviceid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof (string));
            }
            if (issmartphone)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof (string));
            }
            if (isios)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof (string));
            }
            if (isandroid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof (string));
            }
            if (os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof (string));
            }
            if (browser)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof (string));
            }
            if (device_os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof (string));
            }
            if (pointing_method)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof (string));
            }
            if (is_tablet)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof (string));
            }
            if (model_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof (string));
            }
            if (device_os_version)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof (string));
            }
            if (is_wireless_device)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof (string));
            }
            if (brand_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof (string));
            }
            if (marketing_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof (string));
            }
            if (resolution_height)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof (string));
            }
            if (resolution_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof (string));
            }
            if (canvas_support)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof (string));
            }
            if (viewport_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof (string));
            }
            if (isviewport_supported)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof (string));
            }
            if (ismobileoptimized)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof (string));
            }
            if (ishandheldfriendly)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof (string));
            }
            if (is_smarttv)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof (string));
            }
            if (isux_full_desktop)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof (string));
            }


            ///////////////-----------Device Info-----------/////////////


            DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof (decimal));
            DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof (decimal));
            foreach (var item in subids)
            {
                DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof (string));
            }
            Type resultType = affview.CreateType();

            #endregion

            var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, utdate, Offsethour, datecheck,
                hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck,
                ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk,
                bannerchk, parenturlchk, ctrchk,
                deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name,
                device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width,
                canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv,
                isux_full_desktop,
                subids.Length > 0 ? subids : null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname,
                Vmarketingname, Vresolution, UserAgent
                );

            var list = repo.ExecuteQuery(resultType, query);
            //var query = CKQueryBuilder.AffiliateReportQueryTest(up.CustomerId, up.UserId, Offsethour, campaigncheck, sourcechk
            //  , subids.Length > 0 ? subids : null, countrycheck
            //  , cp, af, ctfilter);

            //var list = repo.ExecuteQuery(resultType, query
            //    , new SqlParameter("fromdate", ufdate)
            //    , new SqlParameter("todate", utdate)
            //    );

            return Ok(list);
            //return Request.CreateResponse(HttpStatusCode.OK, list);
        }

        private async Task<IHttpActionResult> Campaign(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata, long? dataview, int? cp,
            int[] af, int? ct, UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var model = new OfferViewModel();

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            try
            {
                model.StatisticsEnum = (StatisticsEnum) dataview;
            }
            catch
            {
                model.StatisticsEnum =
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


            // get the subids
            // var subids = Request.RequestUri.ParseQueryString().AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();

            if (up.AffiliateId.HasValue) // if is admin
            {
                if (af.Length == 0)
                {
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
            }
            if (af != null && af.Length == 0)
                af = null;

            //var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            //DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            //DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            var fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            var tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            TimeSpan toffset;
            //var tzi = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZone);
            //toffset = tzi.GetUtcOffset(fdate);
            //var ufdate = new DateTimeOffset(fdate, toffset).UtcDateTime;
            //var utdate = new DateTimeOffset(tdate, toffset).UtcDateTime;
            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            var ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);

            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;
            if (customoffset != 0)
            {
                offsetHour = customoffset/60;
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
            }

            #region Dynamic Statistics

            bool datecheck = false;
            bool hourcheck = false;
            bool campaigncheck = true; //string.IsNullOrEmpty(offview);
            bool affiliatecheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Affiliate);
            bool countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);
            bool urlcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.URLId);

            bool statuscheck = false;
            bool pixelcheck = false;
            bool postbackcheck = false;
            bool ipcheck = false;
            bool transactionidcheck = false;
            bool conversiontypecheck = false;
            bool referrercheck = false;
            bool statusdesccheck = false;
            bool sourcechk = model.StatisticsEnum.HasFlag(StatisticsEnum.Source); //!string.IsNullOrEmpty(sourceview);
            bool actionchk = false;
            bool bannerchk = false;
            bool parenturlchk = false;
            bool ctrchk = false;


            /////////////---------Device Info--------------////////////////
            bool deviceid = false;
            bool issmartphone = false;
            bool isios = false;
            bool isandroid = false;
            bool os = false;
            bool browser = false;
            bool device_os = false;
            bool pointing_method = false;
            bool is_tablet = false;
            bool model_name = false;
            bool device_os_version = false;
            bool is_wireless_device = false;
            bool brand_name = false;
            bool marketing_name = false;
            bool resolution_height = false;
            bool resolution_width = false;
            bool canvas_support = false;
            bool viewport_width = false;
            bool isviewport_supported = false;
            bool ismobileoptimized = false;
            bool ishandheldfriendly = false;
            bool is_smarttv = false;
            bool isux_full_desktop = false;
            /////////////---------Device Info--------------////////////////


            //Filtering Device Infoes//
            string Vdeviceid = null;
            string Vdeviceos = null;
            string Vbrowser = null;
            string Vos = null;
            string Vmodelname = null;
            string Vbrandname = null;
            string Vmarketingname = null;
            string Vresolution = null;
            string UserAgent = null;
            //Filtering Device Infoes//


            // get the subids
            var debug = Request.RequestUri.ParseQueryString();
            var subids =
                Request.RequestUri.ParseQueryString()
                    .AllKeys.Where(s => s.ToLower().StartsWith("subid"))
                    .Select(s => int.Parse(s.Substring(5)))
                    .ToArray();

            string ctfilter = ct.HasValue
                ? repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

            #endregion

            #region Dynamic Type

            var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
            //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
            //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


            if (hourcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof (int));
            }
            if (datecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof (string));
            }

            if (campaigncheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof (string));
            }
            if (affiliatecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof (string));
            }

            if (urlcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof (string));
            }
            if (countrycheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof (string));
            }
            if (statuscheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof (int));
            }
            if (pixelcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof (DateTime?));
            }
            if (postbackcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof (DateTime?));
            }
            if (ipcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof (string));
            }
            if (transactionidcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof (string));
            }
            if (conversiontypecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof (ConversionType));
            }
            if (referrercheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof (string));
            }
            if (statusdesccheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof (string));
            }
            if (sourcechk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof (string));
            }
            if (actionchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof (string));
                DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof (int));
            }
            if (bannerchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof (string));
            }
            if (parenturlchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
            }
            if (ctrchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
            }


            ///////////////-----------Device Info-----------/////////////

            if (deviceid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof (string));
            }
            if (issmartphone)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof (string));
            }
            if (isios)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof (string));
            }
            if (isandroid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof (string));
            }
            if (os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof (string));
            }
            if (browser)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof (string));
            }
            if (device_os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof (string));
            }
            if (pointing_method)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof (string));
            }
            if (is_tablet)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof (string));
            }
            if (model_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof (string));
            }
            if (device_os_version)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof (string));
            }
            if (is_wireless_device)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof (string));
            }
            if (brand_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof (string));
            }
            if (marketing_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof (string));
            }
            if (resolution_height)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof (string));
            }
            if (resolution_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof (string));
            }
            if (canvas_support)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof (string));
            }
            if (viewport_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof (string));
            }
            if (isviewport_supported)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof (string));
            }
            if (ismobileoptimized)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof (string));
            }
            if (ishandheldfriendly)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof (string));
            }
            if (is_smarttv)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof (string));
            }
            if (isux_full_desktop)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof (string));
            }


            ///////////////-----------Device Info-----------/////////////


            DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof (decimal));
            DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof (decimal));
            foreach (var item in subids)
            {
                DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof (string));
            }
            Type resultType = affview.CreateType();

            #endregion

            var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, utdate, offsetHour, datecheck,
                hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck,
                ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk,
                bannerchk, parenturlchk, ctrchk,
                deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name,
                device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width,
                canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv,
                isux_full_desktop,
                subids.Length > 0 ? subids : null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname,
                Vmarketingname, Vresolution, UserAgent
                );

            var list = await repo.ExecuteQuery(resultType, query).ToListAsync();
            //var query = CKQueryBuilder.CampaignReportQuery(up.CustomerId, up.UserId, Offsethour, affiliatecheck, sourcechk

            //    , subids.Length > 0 ? subids : null
            //    , urlcheck, countrycheck
            //    , cp, af, ctfilter);


            //var list = await repo.ExecuteQuery(resultType, query
            //   , new SqlParameter("fromdate", ufdate)
            //   , new SqlParameter("todate", utdate)
            //   ).ToListAsync();

            return Ok(list);
        }

        private IHttpActionResult Hourly(DateTime? FromDate, string timezone, long? dataview, int? cp, int[] af, int? ct,
            UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            // set the fromdate using the passed timezone or the customertimezone or the localtimezone
            DateTime fromdate = FromDate ?? DateTime.Today;

            // find user timezone
            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            var ufdate = new DateTimeOffset(fromdate.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;
            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            if (customoffset != 0)
            {
                ufdate = fromdate.AddMinutes(customoffset);
                offsetHour = customoffset/60;
                // utdate = ToDate.Value.AddMinutes(customoffset);
            }
            HourlyViewModel model = new HourlyViewModel();

            try
            {
                model.StatisticsEnum = (StatisticsEnum) dataview;
            }
            catch
            {
                model.StatisticsEnum =
                    StatisticsEnum.Hour |
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

            if (up.AffiliateId.HasValue)
            {
                if (af.Length == 0)
                {
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
            }
            if (af != null && af.Length == 0)
                af = null;

            string affs = af == null ? null : string.Join(",", af);

            #region Dynamic Statistics

            bool datecheck = false;
            bool hourcheck = true;
            bool campaigncheck = false; //string.IsNullOrEmpty(offview);
            bool affiliatecheck = false;
            bool urlcheck = false;

            var countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);
            bool statuscheck = false;
            bool pixelcheck = false;
            bool postbackcheck = false;
            bool ipcheck = false;
            bool transactionidcheck = false;
            bool conversiontypecheck = false;
            bool referrercheck = false;
            bool statusdesccheck = false;
            bool sourcechk = false; //!string.IsNullOrEmpty(sourceview);
            bool actionchk = false;
            bool bannerchk = false;
            bool parenturlchk = false;
            bool ctrchk = false;


            /////////////---------Device Info--------------////////////////
            bool deviceid = false;
            bool issmartphone = false;
            bool isios = false;
            bool isandroid = false;
            bool os = false;
            bool browser = false;
            bool device_os = false;
            bool pointing_method = false;
            bool is_tablet = false;
            bool model_name = false;
            bool device_os_version = false;
            bool is_wireless_device = false;
            bool brand_name = false;
            bool marketing_name = false;
            bool resolution_height = false;
            bool resolution_width = false;
            bool canvas_support = false;
            bool viewport_width = false;
            bool isviewport_supported = false;
            bool ismobileoptimized = false;
            bool ishandheldfriendly = false;
            bool is_smarttv = false;
            bool isux_full_desktop = false;
            /////////////---------Device Info--------------////////////////


            //Filtering Device Infoes//
            string Vdeviceid = null;
            string Vdeviceos = null;
            string Vbrowser = null;
            string Vos = null;
            string Vmodelname = null;
            string Vbrandname = null;
            string Vmarketingname = null;
            string Vresolution = null;
            string UserAgent = null;
            //Filtering Device Infoes//


            // get the subids
            var debug = Request.RequestUri.ParseQueryString();
            var subids =
                Request.RequestUri.ParseQueryString()
                    .AllKeys.Where(s => s.ToLower().StartsWith("subid"))
                    .Select(s => int.Parse(s.Substring(5)))
                    .ToArray();

            string ctfilter = ct.HasValue
                ? repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

            #endregion

            #region Dynamic Type

            var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
            //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
            //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


            if (hourcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof (int));
            }
            if (datecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof (string));
            }

            if (campaigncheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof (string));
            }
            if (affiliatecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof (string));
            }

            if (urlcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof (string));
            }
            if (countrycheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof (string));
            }
            if (statuscheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof (int));
            }
            if (pixelcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof (DateTime?));
            }
            if (postbackcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof (DateTime?));
            }
            if (ipcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof (string));
            }
            if (transactionidcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof (string));
            }
            if (conversiontypecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof (ConversionType));
            }
            if (referrercheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof (string));
            }
            if (statusdesccheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof (string));
            }
            if (sourcechk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof (string));
            }
            if (actionchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof (string));
                DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof (int));
            }
            if (bannerchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof (string));
            }
            if (parenturlchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
            }
            if (ctrchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
            }


            ///////////////-----------Device Info-----------/////////////

            if (deviceid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof (string));
            }
            if (issmartphone)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof (string));
            }
            if (isios)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof (string));
            }
            if (isandroid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof (string));
            }
            if (os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof (string));
            }
            if (browser)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof (string));
            }
            if (device_os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof (string));
            }
            if (pointing_method)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof (string));
            }
            if (is_tablet)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof (string));
            }
            if (model_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof (string));
            }
            if (device_os_version)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof (string));
            }
            if (is_wireless_device)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof (string));
            }
            if (brand_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof (string));
            }
            if (marketing_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof (string));
            }
            if (resolution_height)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof (string));
            }
            if (resolution_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof (string));
            }
            if (canvas_support)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof (string));
            }
            if (viewport_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof (string));
            }
            if (isviewport_supported)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof (string));
            }
            if (ismobileoptimized)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof (string));
            }
            if (ishandheldfriendly)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof (string));
            }
            if (is_smarttv)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof (string));
            }
            if (isux_full_desktop)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof (string));
            }


            ///////////////-----------Device Info-----------/////////////


            DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof (decimal));
            DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof (decimal));
            foreach (var item in subids)
            {
                DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof (string));
            }
            Type resultType = affview.CreateType();

            #endregion

            var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, ufdate.AddDays(1).AddMilliseconds(-1),
                offsetHour, datecheck, hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck,
                postbackcheck, ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk,
                actionchk, bannerchk, parenturlchk, ctrchk,
                deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name,
                device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width,
                canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv,
                isux_full_desktop,
                subids.Length > 0 ? subids : null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname,
                Vmarketingname, Vresolution, UserAgent
                );

            var report = repo.ExecuteQuery(resultType, query);

            //var report = repo.RunQuery<HourlyView>("EXEC [HourlyRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
            //                    up.CustomerId, ufdate, ufdate.AddDays(1).AddMilliseconds(-1), offsetHour, up.UserId, affs, cp, ctfilter, countrycheck);

            return Ok(report);
        }

        private IEnumerable<CustomConversion> Conversion(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata,
            int? cp, int[] af, int? ct, UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);


            if (up.AffiliateId.HasValue)
            {
                if (af.Length == 0)
                {
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
                // af = new int[] { up.AffiliateId.Value };
            }

            if (af != null && af.Length == 0)
            {
                af = null;
            }


            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
            }

            //bool _af = af == null;
            //if (_af)
            //{
            //    af = new int[] { };
            //}
            // var affiliates = repo.Affiliates();


            // string ctfilter = ct.HasValue ?
            //repo.GetCountries()
            //.Where(c => c.Id == ct.Value)
            //.Select(c => c.CountryAbbreviation).FirstOrDefault()
            //: null;


            // var list = from co in repo.Conversions().Where(c => c.CustomerId == up.CustomerId)
            //            join ck in repo.Clicks()/*.Where(c => c.CustomerId == up.CustomerId)*/ on co.ClickId equals ck.ClickId
            //            join url in repo.Urls().Where(c => c.Campaign.CustomerId == up.CustomerId) on new { /*a = ck.CustomerId,*/ b = ck.CampaignId, c = ck.URLPreviewId } equals new { /*a = url.Campaign.CustomerId,*/ b = url.Campaign.CampaignId, c = url.PreviewId }
            //            join ca in repo.Campaigns().Where(c => c.CustomerId == up.CustomerId) on new { /*a = co.CustomerId,*/ b = co.CampaignId } equals new { /*a = ca.CustomerId,*/ b = ca.CampaignId }
            //            join aff in repo.Affiliates().Where(c => c.CustomerId == up.CustomerId) on new { /*a = co.CustomerId,*/ b = co.AffiliateId } equals new { /*a = aff.CustomerId,*/ b = aff.AffiliateId }
            //            join ov in repo.Override().Where(c => c.CustomerID == up.CustomerId) on co.ActionId equals ov.ActionID into overridedata
            //            from ovfinal in overridedata.DefaultIfEmpty()
            //            where /*co.CustomerId == up.CustomerId
            //                &&*/ co.bot == 0
            //            && ufdate <= co.ConversionDate && co.ConversionDate <= utdate
            //            && (!cp.HasValue || co.CampaignId == cp)
            //            && (_af || (from g in af select g).Contains(co.AffiliateId))
            //            && (ctfilter == null || co.Country == ctfilter)

            //            select new ConversionView
            //            {
            //                AffiliateId = co.AffiliateId,
            //                Company = aff.Company,
            //                CampaignId = co.CampaignId,
            //                CampaignName = ca.CampaignName,
            //                ConversionDate = co.ConversionDate,//TimeZoneInfo.ConvertTimeFromUtc(co.ConversionDate, tzi).ToString("MM/dd/yyyy"),
            //                ConversionId = co.ConversionId,
            //                URLId = ck.URLPreviewId,

            //                PreviewUrl = url.PreviewUrl,

            //                UserAgent = co.UserAgent,
            //                TransactionId = co.TransactionId,
            //                IPAddress = co.IPAddress,
            //                Type = co.Type, // co.Type.ToString(),
            //                Cost = (ovfinal.Payout != null) ? (decimal)ovfinal.Payout : co.Cost,
            //                Revenue = co.Revenue,
            //                Source = co.Click.Source,
            //                SubIds = co.Click.SubIds,
            //                Status = co.Status,
            //                StatusDescription = co.StatusDescription,
            //                Postback_IPAddress = co.Postback_IPAddress,
            //                Pixel = co.Pixel,
            //                Postback = co.Postback,
            //                ActionName = co.Action.Name,
            //                Country = co.Country,
            //                UserAgentId = co.UserAgentId
            //            };

            // var list1 = from c in list.AsEnumerable()
            //             join d in repo.DeviceInfos() on c.UserAgentId equals d.Id into devieinfo
            //             from di in devieinfo.DefaultIfEmpty()
            //             select new
            //             {
            //                 AffiliateId = c.AffiliateId,
            //                 Company = c.Company,
            //                 CampaignId = c.CampaignId,
            //                 CampaignName = c.CampaignName,
            //                 Date = TimeZoneInfo.ConvertTimeFromUtc(c.ConversionDate, tzi).ToString("G"),
            //                 ConversionId = c.ConversionId,
            //                 URLId = c.URLId,

            //                 URL = c.PreviewUrl,

            //                 UserAgent = c.UserAgent,
            //                 TransactionId = c.TransactionId,

            //                 IPAddress = c.IPAddress,


            //                 Type = c.Type.DisplayValue(),
            //                 Cost = c.Cost,
            //                 Revenue = c.Revenue,
            //                 Source = c.Source,
            //                 SubIds = from s in c.SubIds select new CpaTicker.Areas.admin.Classes.SubId { SubIndex = s.SubIndex, SubValue = s.SubValue },  //c.SubIds,
            //                 Status = c.Status,
            //                 StatusDescription = c.StatusDescription,
            //                 Postback_IPAddress = c.Postback_IPAddress,
            //                 Pixel = c.Pixel.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(c.Pixel.Value, tzi).ToString("g") : string.Empty,
            //                 Postback = c.Postback.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(c.Postback.Value, tzi).ToString("g") : string.Empty,
            //                 Action = c.ActionName,
            //                 Country = c.Country,

            //                 DeviceId = (di != null) ? di.DeviceId : "",
            //                 IsSmartphone = (di != null) ? di.IsSmartphone : false,
            //                 IsiOS = (di != null) ? di.IsiOS : false,
            //                 IsAndroid = (di != null) ? di.IsAndroid : false,
            //                 OS = (di != null) ? di.OS : "",
            //                 Browser = (di != null) ? di.Browser : "",
            //                 Device_os = (di != null) ? di.Device_os : "",
            //                 Pointing_method = (di != null) ? di.Pointing_method : "",
            //                 Is_tablet = (di != null) ? di.Is_tablet : false,
            //                 Model_name = (di != null) ? di.Model_name : "",
            //                 Device_os_version = (di != null) ? di.Device_os_version : "",
            //                 Is_wireless_device = (di != null) ? di.Is_wireless_device : false,
            //                 Brand_name = (di != null) ? di.Brand_name : "",
            //                 Marketing_name = (di != null) ? di.Marketing_name : "",
            //                 Is_assign_phone_number = (di != null) ? di.Is_assign_phone_number : false,
            //                 Xhtmlmp_mime_type = (di != null) ? di.Xhtmlmp_mime_type : "",
            //                 Xhtml_support_level = (di != null) ? di.Xhtml_support_level : "",
            //                 Resolution_height = (di != null) ? di.Resolution_height : "",
            //                 Resolution_width = (di != null) ? di.Resolution_width : "",
            //                 Canvas_support = (di != null) ? di.Canvas_support : "",
            //                 Viewport_width = (di != null) ? di.Viewport_width : "",
            //                 Html_preferred_dtd = (di != null) ? di.Html_preferred_dtd : "",
            //                 Isviewport_supported = (di != null) ? di.Isviewport_supported : false,
            //                 Ismobileoptimized = (di != null) ? di.Ismobileoptimized : false,
            //                 Isimage_inlining = (di != null) ? di.Isimage_inlining : false,
            //                 Ishandheldfriendly = (di != null) ? di.Ishandheldfriendly : false,
            //                 Is_smarttv = (di != null) ? di.Is_smarttv : false,
            //                 Isux_full_desktop = (di != null) ? di.Isux_full_desktop : false

            //             };
            string affs = af == null || af.Length == 0 ? null : string.Join(",", af);
            var report = repo.RunQuery<CustomConversion>("EXEC [Conversionrpt] {0}, {1}, {2}, {3}, {4}, {5},{6}",
                up.CustomerId, ufdate, utdate, tzi.GetUtcOffset(DateTime.Now).Hours, affs, cp, ct).ToList();

            return report;
        }

        private IEnumerable<ConversionStatusView> ConversionStatus(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata,
            long? dataview, int? cp, int[] af, int? ct, UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var model = new ConversionStatusViewModel();

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            try
            {
                model.StatisticsEnum = (StatisticsEnum) dataview;
            }
            catch
            {
                model.StatisticsEnum =
                    StatisticsEnum.GrossConversions |
                    StatisticsEnum.RejectedConversions |
                    StatisticsEnum.ApprovedConversions |
                    StatisticsEnum.NetPayout |
                    StatisticsEnum.NetRevenue |
                    StatisticsEnum.ApprovedPercent |
                    StatisticsEnum.RejectedPercent
                    ;
            }

            var countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);


            if (up.AffiliateId.HasValue) // if is affiliate
            {
                if (af.Length == 0)
                {
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
            }
            if (af != null && af.Length == 0)
                af = null;

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;


            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
            }


            string ctfilter = ct.HasValue
                ? repo.GetCountries()
                    .Where(c => c.Id == ct.Value)
                    .Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

            var approved = from c in repo.ConversionQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
                group c by new
                {
                    AffiliateId = c.AffiliateId,
                    CampaignId = c.CampaignId,
                    Country = countrycheck ? c.Country ?? "" : "",
                }
                into dc
                select new
                {
                    CampaignId = dc.Key.CampaignId,
                    AffiliateId = dc.Key.AffiliateId,
                    Country = dc.Key.Country,
                    ApprovedConversion = dc.Count(),
                    Cost = dc.Sum(c => c.Cost),
                    Revenue = dc.Sum(c => c.Revenue),
                };

            var rejected = from c in repo.ConversionQuery(ufdate, utdate, up, af, cp, ctfilter, 0).AsEnumerable()
                group c by new
                {
                    AffiliateId = c.AffiliateId,
                    CampaignId = c.CampaignId,
                    Country = countrycheck ? c.Country ?? "" : "",
                }
                into dc
                select new
                {
                    CampaignId = dc.Key.CampaignId,
                    AffiliateId = dc.Key.AffiliateId,
                    Country = dc.Key.Country,
                    RejectedConversions = dc.Count(),
                };

            var union = approved.Select(c => new {AffiliateId = c.AffiliateId, CampaignId = c.CampaignId, Country = c.Country})
                .Union(rejected.Select(c => new {AffiliateId = c.AffiliateId, CampaignId = c.CampaignId, Country = c.Country}));


            var su = from u in union
                join ca in repo.Campaigns().Where(c => c.CustomerId == up.CustomerId) on
                    u.CampaignId equals ca.CampaignId
                join _af in repo.Affiliates().Where(c => c.CustomerId == up.CustomerId) on
                    u.AffiliateId equals _af.AffiliateId
                select new
                {
                    Union = u,
                    CampaignName = ca.CampaignName,
                    Company = _af.Company,
                };


            var report = from c in su
                join a in approved on c.Union
                    equals new {AffiliateId = a.AffiliateId, CampaignId = a.CampaignId, Country = a.Country} into da
                from a in da.DefaultIfEmpty()
                join r in rejected on c.Union
                    equals new {AffiliateId = r.AffiliateId, CampaignId = r.CampaignId, Country = r.Country} into dr
                from r in dr.DefaultIfEmpty()
                orderby c.Union.CampaignId, c.Union.AffiliateId
                select new ConversionStatusView
                {
                    AffiliateId = c.Union.AffiliateId,
                    CampaignId = c.Union.CampaignId,
                    CampaignName = c.CampaignName,
                    Company = c.Company,
                    Country = c.Union.Country,
                    ApprovedConversions = a == null ? 0 : a.ApprovedConversion,
                    RejectedConversions = r == null ? 0 : r.RejectedConversions,
                    NetPayout = a == null ? 0 : a.Cost,
                    NetRevenue = a == null ? 0 : a.Revenue,
                };

            return report.ToList();
        }

        private IEnumerable<AdCampaignView> AdCampaign(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata, long? dataview,
            int? cp, int[] af, int? ct, UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var model = new AdCampaignViewModel();

            try
            {
                model.StatisticsEnum = (StatisticsEnum) dataview;
            }
            catch
            {
                model.StatisticsEnum =
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

            var countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);

            if (up.AffiliateId.HasValue) // if is affiliate
            {
                if (af.Length == 0)
                {
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
            }
            if (af != null && af.Length == 0)
                af = null;

            string affs = af == null ? null : string.Join(",", af);

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);
            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //var offset = tzi.GetUtcOffset(DateTime.Now);
            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
            }


            string ctfilter = ct.HasValue
                ? repo.GetCountries()
                    .Where(c => c.Id == ct.Value)
                    .Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

            //var bclicks = from c in repo.ClickQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
            //              group c by new
            //              {
            //                  BannerId = c.BannerId,
            //                  Country = countrycheck ? c.Country ?? "" : "",
            //              } into dc
            //              select new
            //              {
            //                  BannerId = dc.Key.BannerId,
            //                  Country = dc.Key.Country,
            //                  Clicks = dc.Count(),
            //                  Cost = dc.Sum(c => c.Cost),
            //                  Revenue = dc.Sum(c => c.Revenue),
            //              };

            //var bconversions = from c in repo.ConversionQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
            //                   group c by new
            //                   {
            //                       BannerId = c.BannerId,
            //                       Country = countrycheck ? c.Country ?? "" : "",
            //                   } into dc
            //                   select new
            //                   {
            //                       BannerId = dc.Key.BannerId,
            //                       Country = dc.Key.Country,
            //                       Conversions = dc.Count(),
            //                       Cost = dc.Sum(c => c.Cost),
            //                       Revenue = dc.Sum(c => c.Revenue),
            //                   };

            //var bimpressions = from c in repo.ImpressionQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
            //                   group c by new
            //                   {
            //                       BannerId = c.BannerId,
            //                       Country = countrycheck ? c.Country ?? "" : "",
            //                   } into dc
            //                   select new
            //                   {
            //                       BannerId = dc.Key.BannerId,
            //                       Country = dc.Key.Country,
            //                       Impressions = dc.Count(),
            //                       Cost = dc.Sum(c => c.Cost),
            //                       Revenue = dc.Sum(c => c.Revenue),
            //                   };

            //var union = bclicks.Select(c => new { D = c.BannerId, C = c.Country })
            //   .Union(bconversions.Select(c => new { D = c.BannerId, C = c.Country }))
            //   .Union(bimpressions.Select(c => new { D = c.BannerId, C = c.Country }));

            //var su = from u in union
            //         join ba in repo.Banners().Where(c => c.CustomerId == up.CustomerId) on u.D equals ba.BannerId
            //         join ca in repo.Campaigns().Where(c => c.CustomerId == up.CustomerId) on ba.CampaignId equals ca.CampaignId
            //         select new
            //         {
            //             Union = u,
            //             Name = ba.Name,
            //             CampaignId = ca.CampaignId,
            //             CampaignName = ca.CampaignName,
            //         };

            //var report = from u in su

            //             join c in bclicks on u.Union equals new { D = c.BannerId, C = c.Country } into dc
            //             from c in dc.DefaultIfEmpty()

            //             join co in bconversions on u.Union equals new { D = co.BannerId, C = co.Country } into dco
            //             from co in dco.DefaultIfEmpty()

            //             join i in bimpressions on u.Union equals new { D = i.BannerId, C = i.Country } into di
            //             from i in di.DefaultIfEmpty()

            //             select new AdCampaignView
            //             {
            //                 BannerId = u.Union.D,
            //                 Country = u.Union.C,
            //                 CampaignId = u.CampaignId,
            //                 CampaignName = u.CampaignName,
            //                 Name = u.Name,
            //                 Clicks = c == null ? 0 : c.Clicks,
            //                 Conversions = co == null ? 0 : co.Conversions,
            //                 Impressions = i == null ? 0 : i.Impressions,
            //                 Cost = (c == null ? 0 : c.Cost)
            //                       + (co == null ? 0 : co.Cost)
            //                       + (i == null ? 0 : i.Cost),
            //                 Revenue = (c == null ? 0 : c.Revenue)
            //                       + (co == null ? 0 : co.Revenue)
            //                       + (i == null ? 0 : i.Revenue),
            //             };

            var report = repo.RunQuery<AdCampaignView>("EXEC [AdCampaignRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                up.CustomerId, ufdate, utdate, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, affs, cp, ctfilter, countrycheck);

            return report.ToList();
        }

        private IHttpActionResult Traffic(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata, long? dataview,
            int? cp, int[] af, int? ct, UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var model = new TrafficViewModel();
            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            try
            {
                model.StatisticsEnum = (StatisticsEnum) dataview;
            }
            catch
            {
                model.StatisticsEnum =
                    StatisticsEnum.Clicks |
                    StatisticsEnum.Conversions |
                    StatisticsEnum.Cost |
                    StatisticsEnum.Revenue |
                    StatisticsEnum.Profit
                    ;
            }


            if (up.AffiliateId.HasValue) // if is affiliate
            {
                if (af.Length == 0)
                {
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
            }
            if (af != null && af.Length == 0)
                af = null;

            string affs = af == null ? null : string.Join(",", af);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            var ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                offsetHour = customoffset/60;
            }

            #region Dynamic Statistics

            bool datecheck = false;
            bool hourcheck = false;
            bool campaigncheck = true; //string.IsNullOrEmpty(offview);
            bool affiliatecheck = true;
            bool urlcheck = true;

            bool countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);
            bool statuscheck = false;
            bool pixelcheck = false;
            bool postbackcheck = false;
            bool ipcheck = false;
            bool transactionidcheck = false;
            bool conversiontypecheck = false;
            bool referrercheck = false;
            bool statusdesccheck = false;
            bool sourcechk = false; //!string.IsNullOrEmpty(sourceview);
            bool actionchk = false;
            bool bannerchk = false;
            bool parenturlchk = false;
            bool ctrchk = false;


            /////////////---------Device Info--------------////////////////
            bool deviceid = false;
            bool issmartphone = false;
            bool isios = false;
            bool isandroid = false;
            bool os = false;
            bool browser = false;
            bool device_os = false;
            bool pointing_method = false;
            bool is_tablet = false;
            bool model_name = false;
            bool device_os_version = false;
            bool is_wireless_device = false;
            bool brand_name = false;
            bool marketing_name = false;
            bool resolution_height = false;
            bool resolution_width = false;
            bool canvas_support = false;
            bool viewport_width = false;
            bool isviewport_supported = false;
            bool ismobileoptimized = false;
            bool ishandheldfriendly = false;
            bool is_smarttv = false;
            bool isux_full_desktop = false;
            /////////////---------Device Info--------------////////////////


            //Filtering Device Infoes//
            string Vdeviceid = null;
            string Vdeviceos = null;
            string Vbrowser = null;
            string Vos = null;
            string Vmodelname = null;
            string Vbrandname = null;
            string Vmarketingname = null;
            string Vresolution = null;
            string UserAgent = null;
            //Filtering Device Infoes//


            // get the subids
            var debug = Request.RequestUri.ParseQueryString();
            var subids =
                Request.RequestUri.ParseQueryString()
                    .AllKeys.Where(s => s.ToLower().StartsWith("subid"))
                    .Select(s => int.Parse(s.Substring(5)))
                    .ToArray();

            string ctfilter = ct.HasValue
                ? repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

            #endregion

            #region Dynamic Type

            var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
            //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
            //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


            if (hourcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof (int));
            }
            if (datecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof (string));
            }

            if (campaigncheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof (string));
            }
            if (affiliatecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof (string));
            }

            if (urlcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof (string));
            }
            if (countrycheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof (string));
            }
            if (statuscheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof (int));
            }
            if (pixelcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof (DateTime?));
            }
            if (postbackcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof (DateTime?));
            }
            if (ipcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof (string));
            }
            if (transactionidcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof (string));
            }
            if (conversiontypecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof (ConversionType));
            }
            if (referrercheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof (string));
            }
            if (statusdesccheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof (string));
            }
            if (sourcechk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof (string));
            }
            if (actionchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof (string));
                DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof (int));
            }
            if (bannerchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof (string));
            }
            if (parenturlchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
            }
            if (ctrchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
            }


            ///////////////-----------Device Info-----------/////////////

            if (deviceid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof (string));
            }
            if (issmartphone)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof (string));
            }
            if (isios)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof (string));
            }
            if (isandroid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof (string));
            }
            if (os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof (string));
            }
            if (browser)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof (string));
            }
            if (device_os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof (string));
            }
            if (pointing_method)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof (string));
            }
            if (is_tablet)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof (string));
            }
            if (model_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof (string));
            }
            if (device_os_version)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof (string));
            }
            if (is_wireless_device)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof (string));
            }
            if (brand_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof (string));
            }
            if (marketing_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof (string));
            }
            if (resolution_height)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof (string));
            }
            if (resolution_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof (string));
            }
            if (canvas_support)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof (string));
            }
            if (viewport_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof (string));
            }
            if (isviewport_supported)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof (string));
            }
            if (ismobileoptimized)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof (string));
            }
            if (ishandheldfriendly)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof (string));
            }
            if (is_smarttv)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof (string));
            }
            if (isux_full_desktop)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof (string));
            }


            ///////////////-----------Device Info-----------/////////////


            DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof (decimal));
            DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof (decimal));
            foreach (var item in subids)
            {
                DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof (string));
            }
            Type resultType = affview.CreateType();

            #endregion

            var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, utdate, offsetHour, datecheck,
                hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck,
                ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk,
                bannerchk, parenturlchk, ctrchk,
                deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name,
                device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width,
                canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv,
                isux_full_desktop,
                subids.Length > 0 ? subids : null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname,
                Vmarketingname, Vresolution, UserAgent
                );

            var report = repo.ExecuteQuery(resultType, query);


            //var report = repo.RunQuery<TrafficView>("EXEC [TrafficRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
            //                    up.CustomerId, ufdate, utdate, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, affs, cp, ctfilter, countrycheck);

            return Ok(report);
        }

        private IHttpActionResult Ctr(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata, int? cp, int[] af,
            int? ct, UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                offsetHour = customoffset/60;
            }


            if (up.AffiliateId.HasValue) // if is affiliate
            {
                if (af.Length == 0)
                {
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
            }
            if (af != null && af.Length == 0)
                af = null;

            string affs = af == null ? null : string.Join(",", af);

            #region Dynamic Statistics

            bool datecheck = false;
            bool hourcheck = false;
            bool campaigncheck = true; //string.IsNullOrEmpty(offview);
            bool affiliatecheck = false;
            bool urlcheck = true;

            var countrycheck = false;
            bool statuscheck = false;
            bool pixelcheck = false;
            bool postbackcheck = false;
            bool ipcheck = false;
            bool transactionidcheck = false;
            bool conversiontypecheck = false;
            bool referrercheck = false;
            bool statusdesccheck = false;
            bool sourcechk = false; //!string.IsNullOrEmpty(sourceview);
            bool actionchk = false;
            bool bannerchk = false;
            bool parenturlchk = true;
            bool ctrchk = true;


            /////////////---------Device Info--------------////////////////
            bool deviceid = false;
            bool issmartphone = false;
            bool isios = false;
            bool isandroid = false;
            bool os = false;
            bool browser = false;
            bool device_os = false;
            bool pointing_method = false;
            bool is_tablet = false;
            bool model_name = false;
            bool device_os_version = false;
            bool is_wireless_device = false;
            bool brand_name = false;
            bool marketing_name = false;
            bool resolution_height = false;
            bool resolution_width = false;
            bool canvas_support = false;
            bool viewport_width = false;
            bool isviewport_supported = false;
            bool ismobileoptimized = false;
            bool ishandheldfriendly = false;
            bool is_smarttv = false;
            bool isux_full_desktop = false;
            /////////////---------Device Info--------------////////////////


            //Filtering Device Infoes//
            string Vdeviceid = null;
            string Vdeviceos = null;
            string Vbrowser = null;
            string Vos = null;
            string Vmodelname = null;
            string Vbrandname = null;
            string Vmarketingname = null;
            string Vresolution = null;
            string UserAgent = null;
            //Filtering Device Infoes//


            // get the subids
            var debug = Request.RequestUri.ParseQueryString();
            var subids =
                Request.RequestUri.ParseQueryString()
                    .AllKeys.Where(s => s.ToLower().StartsWith("subid"))
                    .Select(s => int.Parse(s.Substring(5)))
                    .ToArray();

            string ctfilter = ct.HasValue
                ? repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

            #endregion

            #region Dynamic Type

            var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
            //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
            //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


            if (hourcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof (int));
            }
            if (datecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof (string));
            }

            if (campaigncheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof (string));
            }
            if (affiliatecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof (string));
            }

            if (urlcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof (string));
            }
            if (countrycheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof (string));
            }
            if (statuscheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof (int));
            }
            if (pixelcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof (DateTime?));
            }
            if (postbackcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof (DateTime?));
            }
            if (ipcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof (string));
            }
            if (transactionidcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof (string));
            }
            if (conversiontypecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof (ConversionType));
            }
            if (referrercheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof (string));
            }
            if (statusdesccheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof (string));
            }
            if (sourcechk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof (string));
            }
            if (actionchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof (string));
                DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof (int));
            }
            if (bannerchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof (string));
            }
            if (parenturlchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
            }
            if (ctrchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
                DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
            }


            ///////////////-----------Device Info-----------/////////////

            if (deviceid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof (string));
            }
            if (issmartphone)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof (string));
            }
            if (isios)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof (string));
            }
            if (isandroid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof (string));
            }
            if (os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof (string));
            }
            if (browser)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof (string));
            }
            if (device_os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof (string));
            }
            if (pointing_method)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof (string));
            }
            if (is_tablet)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof (string));
            }
            if (model_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof (string));
            }
            if (device_os_version)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof (string));
            }
            if (is_wireless_device)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof (string));
            }
            if (brand_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof (string));
            }
            if (marketing_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof (string));
            }
            if (resolution_height)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof (string));
            }
            if (resolution_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof (string));
            }
            if (canvas_support)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof (string));
            }
            if (viewport_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof (string));
            }
            if (isviewport_supported)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof (string));
            }
            if (ismobileoptimized)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof (string));
            }
            if (ishandheldfriendly)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof (string));
            }
            if (is_smarttv)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof (string));
            }
            if (isux_full_desktop)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof (string));
            }


            ///////////////-----------Device Info-----------/////////////


            DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof (decimal));
            DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof (decimal));
            foreach (var item in subids)
            {
                DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof (string));
            }
            Type resultType = affview.CreateType();

            #endregion

            //var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, utdate, offsetHour, datecheck, hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck, ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk, bannerchk, parenturlchk, ctrchk,
            //     deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name, device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width, canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv, isux_full_desktop,
            //   subids.Length > 0 ? subids : null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname, Vmarketingname, Vresolution, UserAgent
            //   );

            //var report = repo.ExecuteQuery(resultType, query);

            var report = repo.RunQuery<CTRView>("EXEC [CTRReport] {0}, {1}, {2}, {3}, {4}, {5}, {6}",
                up.CustomerId, ufdate, utdate, up.UserId, affs, cp, null).ToList();

            //var Returnlist = report.ToListAsync().Result;

            //if (ctrchk)
            //{
            //    Returnlist.Clear();
            //    var dic = new Dictionary<int, Holder>();

            //    // fill the dictionary
            //    int count = 1;
            //    foreach (var item in report)
            //    {
            //        int UlID = count;
            //        //int UlID = Convert.ToInt32(item.GetType().GetProperty("ULID").GetValue(item, null));
            //        int clicks = Convert.ToInt32(item.GetType().GetProperty("Clicks").GetValue(item, null));
            //        int? ParentURLID = (int?)item.GetType().GetProperty("ParentURL").GetValue(item, null);
            //        int CampaignId = Convert.ToInt32(item.GetType().GetProperty("CampaignId").GetValue(item, null));
            //        int PreviewId = Convert.ToInt32(item.GetType().GetProperty("URLPreviewId").GetValue(item, null));

            //        dic.Add(UlID, new Holder
            //        {
            //            Clicks = clicks,
            //            ParentURLId = ParentURLID,
            //            CampaignId = CampaignId,
            //            PreviewId = PreviewId
            //        });
            //        count = count + 1;
            //    }

            //    myDelegate ctr = null;

            //    ctr = delegate(int? actionid)
            //    {
            //        if (actionid.HasValue && dic.Keys.Contains(actionid.Value))
            //            return dic[actionid.Value].Clicks + ctr(dic[actionid.Value].ParentURLId);
            //        return 0;
            //    };

            //    foreach (var item in report)
            //    {
            //        int? ParentURLID = (int?)item.GetType().GetProperty("ParentURL").GetValue(item, null);
            //        int clicks = Convert.ToInt32(item.GetType().GetProperty("Clicks").GetValue(item, null));
            //        int previous = ctr(ParentURLID);

            //        PropertyInfo property = item.GetType().GetProperty("CTR");
            //        property.SetValue(item, Convert.ChangeType(previous == 0 ? clicks : clicks / (int)previous, property.PropertyType), null);
            //        // item.CTR = previous == 0 ? item.Clicks : item.Clicks / (int)previous;

            //        if (ParentURLID.HasValue)
            //        {
            //            if (dic.Keys.Contains(ParentURLID.Value))
            //            {
            //                var parent = dic[ParentURLID.Value];

            //                //  item.ParentURLIdText = string.Format("{0} - {1}", parent.CampaignId, parent.PreviewId);
            //            }
            //            //item.ParentURLIdText = ParentURLID.ToString();
            //        }
            //        Returnlist.Add(item);
            //    }
            //}


            return Ok(report);
        }

        private IHttpActionResult CustomReport(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata,
            long? dataview, int[] cp, int[] af, int[] ct, bool QDate, bool QHour, bool QCampaign, bool QAffiliate, bool QURL,
            bool QCountry, bool QStatus, bool QPixel, bool QPostback, bool QIP, UserProfile up, bool QTransactionID,
            bool QConversionType, bool QReferrer, bool QStatusDescription, bool QSource, bool QActionName, bool QBanner,
            bool QParentURL, bool QCTR, bool QDeviceId, bool QIsSmartphone, bool QIsiOS, bool QIsAndroid, bool QOS,
            bool QBrowser, bool QDevice_os, bool QPointing_method, bool QIs_tablet, bool QModel_name, bool QDevice_os_version,
            bool QIs_wireless_device, bool QBrand_name, bool QMarketing_name, bool QResolution_height, bool QResolution_width,
            bool QCanvas_support, bool QViewport_width, bool QIsviewport_supported, bool QIsmobileoptimized,
            bool QIshandheldfriendly, bool QIs_smarttv, bool QIsux_full_desktop, bool QClickDate, bool QConversionDate,
            bool QImpressionDate, bool QPAGE, bool QRedirect, string Vdeviceid, string Vdeviceos, string Vbrowser, string Vos,
            string Vmodelname, string Vbrandname, string Vmarketingname, string Vresolution, string UserAgent)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            CustomStatisticsEnum stats;

            //int statistics = 63;

            try
            {
                //string[] dataviewlist = dataview.Split(',');
                //statistics = int.Parse(dataviewlist[0]);
                stats = (CustomStatisticsEnum) dataview;
            }
            catch
            {
                stats =
                    CustomStatisticsEnum.Date |
                    CustomStatisticsEnum.CR |
                    CustomStatisticsEnum.CPC |
                    CustomStatisticsEnum.RPC |
                    CustomStatisticsEnum.Impressions |
                    CustomStatisticsEnum.Clicks |
                    CustomStatisticsEnum.Conversions |
                    CustomStatisticsEnum.Cost |
                    CustomStatisticsEnum.Revenue |
                    CustomStatisticsEnum.Profit
                    ;
            }

            //var stats = Statistics.Create(statistics);

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            if (up.AffiliateId.HasValue) // if is affiliate
            {
                if (af.Length == 0)
                {
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
            }
            if (af != null && af.Length == 0)
                af = null;

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            // convert the time with the customer tzi to UTC 
            var ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;


            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int offsetHour = (int) tzi.GetUtcOffset(DateTime.Now).TotalMinutes;
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                offsetHour = customoffset;
            }

            //bool datecheck = stats.HasFlag(StatisticsEnum.Date);
            //bool hourcheck = stats.HasFlag(StatisticsEnum.Hour);
            //bool campaigncheck = stats.HasFlag(StatisticsEnum.Campaign); //string.IsNullOrEmpty(offview);
            //bool affiliatecheck = stats.HasFlag(StatisticsEnum.Affiliate);
            //bool urlcheck = stats.HasFlag(StatisticsEnum.URL);

            //bool countrycheck = stats.HasFlag(StatisticsEnum.Country);
            //bool statuscheck = stats.HasFlag(StatisticsEnum.Status);
            //bool statusdesccheck = stats.HasFlag(StatisticsEnum.StatusDescription);
            //bool sourcechk = stats.HasFlag(StatisticsEnum.Source); //!string.IsNullOrEmpty(sourceview);
            //bool bannerchk = stats.HasFlag(StatisticsEnum.Banner);

            // njhones
            var clicks_check = stats.HasFlag(CustomStatisticsEnum.Clicks);
            var conversions_check = stats.HasFlag(CustomStatisticsEnum.Conversions);
            var impressions_check = stats.HasFlag(CustomStatisticsEnum.Impressions);

            bool datecheck = QDate;
            bool hourcheck = QHour;
            bool campaigncheck = (!QCampaign) ? stats.HasFlag(CustomStatisticsEnum.Campaign) : true;
                //string.IsNullOrEmpty(offview);
            bool affiliatecheck = (!QAffiliate) ? stats.HasFlag(CustomStatisticsEnum.Affiliate) : true;
            bool urlcheck = (!QURL) ? stats.HasFlag(CustomStatisticsEnum.URL) : true;

            bool countrycheck = QCountry;
            bool statuscheck = QStatus;
            bool pixelcheck = QPixel;
            bool postbackcheck = QPostback;
            bool ipcheck = QIP;
            bool transactionidcheck = QTransactionID;
            bool conversiontypecheck = QConversionType;
            bool referrercheck = QReferrer;
            bool statusdesccheck = QStatusDescription;
            bool sourcechk = QSource; //!string.IsNullOrEmpty(sourceview);
            bool actionchk = QActionName;
            bool bannerchk = QBanner;
            bool parenturlchk = (!QParentURL) ? stats.HasFlag(CustomStatisticsEnum.ParentURL) : true;
            bool ctrchk = (!QCTR) ? stats.HasFlag(CustomStatisticsEnum.CTR) : true;
            bool pagechk = (!QPAGE) ? stats.HasFlag(CustomStatisticsEnum.PAGE) : true;
            bool redirectchk = (!QRedirect) ? stats.HasFlag(CustomStatisticsEnum.Redirect) : true;

            /////////////---------Device Info--------------////////////////
            bool deviceid = QDeviceId;
            bool issmartphone = (!QIsSmartphone) ? stats.HasFlag(CustomStatisticsEnum.IsSmartphone) : true;
            bool isios = QIsiOS;
            bool isandroid = QIsAndroid;
            bool os = QOS;
            bool browser = (!QBrowser) ? stats.HasFlag(CustomStatisticsEnum.Browser) : true;
            bool device_os = QDevice_os;
            bool pointing_method = QPointing_method;
            bool is_tablet = QIs_tablet;
            bool model_name = QModel_name;
            bool device_os_version = QDevice_os_version;
            bool is_wireless_device = QIs_wireless_device;
            bool brand_name = QBrand_name;
            bool marketing_name = QMarketing_name;
            bool resolution_height = QResolution_height;
            bool resolution_width = QResolution_width;
            bool canvas_support = QCanvas_support;
            bool viewport_width = QViewport_width;
            bool isviewport_supported = QIsviewport_supported;
            bool ismobileoptimized = QIsmobileoptimized;
            bool ishandheldfriendly = QIshandheldfriendly;
            bool is_smarttv = QIs_smarttv;
            bool isux_full_desktop = QIsux_full_desktop;
            /////////////---------Device Info--------------////////////////

            bool clickdatecheck = (!QClickDate) ? stats.HasFlag(CustomStatisticsEnum.ClickDate) : true;
            bool conversiondatecheck = (!QConversionDate) ? stats.HasFlag(CustomStatisticsEnum.ConversionDate) : true;
            bool impressiondatecheck = (!QImpressionDate) ? stats.HasFlag(CustomStatisticsEnum.ImpressionDate) : true;


            // get the subids
            var debug = Request.RequestUri.ParseQueryString();
            var subids =
                Request.RequestUri.ParseQueryString()
                    .AllKeys.Where(s => s.ToLower().StartsWith("subid"))
                    .Select(s => int.Parse(s.Substring(5)))
                    .ToArray();

            //string ctfilter = ct.HasValue ?
            //                       repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
            //                       : null;
            string[] cts = (ct != null)
                ? repo.GetCountries().Where(c => ct.Contains(c.Id)).Select(c => c.CountryAbbreviation).ToArray()
                : null;

            #region Dynamic Type

            var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
            //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
            //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


            if (hourcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof (int));
            }
            if (datecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof (string));
            }

            if (clickdatecheck || conversiondatecheck || impressiondatecheck)
                DynamicType.CreateAutoImplementedProperty(affview, "CDatetime", typeof (DateTime?));

            if (clickdatecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ClickDate", typeof (DateTime?));
                clicks_check = true;
            }
            if (impressiondatecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ImpressionDate", typeof (DateTime?));
                impressions_check = true; // if selected then better show the impression to include and left join the table
            }
            if (conversiondatecheck)
            {
                conversions_check = true;
                DynamicType.CreateAutoImplementedProperty(affview, "ConversionDate", typeof (DateTime?));
                DynamicType.CreateAutoImplementedProperty(affview, "ConversionID", typeof (int));
            }


            if (campaigncheck || ctrchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof (string));
            }
            if (affiliatecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof (string));
            }

            //if (urlcheck) // obsoleted
            //{
            //    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
            //    DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof(string));
            //}
            if (pagechk || ctrchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "PAGEId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "PAGEURL", typeof (string));
            }
            if (redirectchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "RedirectId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "RedirectURL", typeof (string));
            }
            if (countrycheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof (string));
            }
            if (statuscheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof (int));
            }
            if (pixelcheck)
            {
                //DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof(DateTime?));
                DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof (string));
            }
            if (postbackcheck)
            {
                //DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof(DateTime?));
                DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof (string));
            }
            if (ipcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof (string));
            }
            if (transactionidcheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof (string));
            }
            if (conversiontypecheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof (ConversionType));
            }
            if (referrercheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof (string));
            }
            if (statusdesccheck)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof (string));
            }
            if (sourcechk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof (string));
            }
            if (actionchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof (string));
                DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof (int));
                DynamicType.CreateAutoImplementedProperty(affview, "ActionType", typeof (ConversionType));
            }
            if (bannerchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof (string));
            }
            if (parenturlchk || ctrchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof (int?));
            }
            if (ctrchk)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof (int));
                //DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof(int));
                //DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                //DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                //DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
            }


            ///////////////-----------Device Info-----------/////////////

            if (deviceid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof (string));
            }
            if (issmartphone)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof (bool));
            }
            if (isios)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof (bool));
            }
            if (isandroid)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof (bool));
            }
            if (os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof (string));
            }
            if (browser)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof (string));
            }
            if (device_os)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof (string));
            }
            if (pointing_method)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof (string));
            }
            if (is_tablet)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof (bool));
            }
            if (model_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof (string));
            }
            if (device_os_version)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof (string));
            }
            if (is_wireless_device)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof (bool));
            }
            if (brand_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof (string));
            }
            if (marketing_name)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof (string));
            }
            if (resolution_height)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof (string));
            }
            if (resolution_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof (string));
            }
            if (canvas_support)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof (string));
            }
            if (viewport_width)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof (string));
            }
            if (isviewport_supported)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof (int));
            } // int?
            if (ismobileoptimized)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof (bool));
            }
            if (ishandheldfriendly)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof (bool));
            }
            if (is_smarttv)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof (int));
            } // int ?
            if (isux_full_desktop)
            {
                DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof (int));
            } // int?


            ///////////////-----------Device Info-----------/////////////
            //if (impressions_check)
            //{
            DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof (int));
            //}
            //if (clicks_check)
            //{
            DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof (int));
            //}
            //if (conversiondatecheck)
            //{
            DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof (int));
            //}

            DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof (decimal));
            DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof (decimal));

            foreach (var item in subids)
            {
                DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof (string));
            }
            Type resultType = affview.CreateType();

            #endregion

            var report = repo.ExecuteQuery(resultType,
                "EXEC [MasterCustomReportB] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, {31}, {32}, {33}, {34}, {35}, {36}, {37}, {38}, {39}, {40}, {41}, {42}, {43}, {44}, {45}, {46}, {47}, {48}, {49}, {50}, {51}, {52}, {53}, {54}, {55}, {56}, {57}, {58}, {59}, {60}, {61}, {62}, {63}, {64}, {65}, {66}, {67}",
                up.CustomerId,
                up.UserId,
                ufdate,
                utdate,
                offsetHour,
                datecheck,
                hourcheck,
                campaigncheck,
                affiliatecheck,
                urlcheck,
                countrycheck,
                statuscheck,
                pixelcheck,
                postbackcheck,
                ipcheck,
                transactionidcheck,
                conversiontypecheck,
                referrercheck,
                statusdesccheck,
                sourcechk,
                actionchk,
                bannerchk,
                parenturlchk,
                ctrchk,
                deviceid,
                issmartphone,
                isios,
                isandroid,
                os,
                browser,
                device_os,
                pointing_method,
                is_tablet,
                model_name,
                device_os_version,
                is_wireless_device,
                brand_name,
                marketing_name,
                resolution_height,
                resolution_width,
                canvas_support,
                viewport_width,
                isviewport_supported,
                ismobileoptimized,
                ishandheldfriendly,
                is_smarttv,
                isux_full_desktop,
                clickdatecheck,
                conversiondatecheck,
                impressiondatecheck,
                pagechk,
                redirectchk,
                subids.Length > 0 ? string.Join(",", subids) : null,
                (cp != null && cp.Length > 0) ? string.Join(",", cp) : null,
                (af != null && af.Length > 0) ? string.Join(",", af) : null,
                (cts != null && cts.Length > 0) ? string.Join(",", cts) : null,
                Vdeviceid,
                Vdeviceos,
                Vbrowser,
                Vos,
                Vmodelname,
                Vbrandname,
                Vmarketingname,
                Vresolution,
                UserAgent,
                clicks_check,
                conversions_check,
                impressions_check);


            // debug
            //foreach (var item in report)
            //{
            //    foreach (var pi in item.GetType().GetProperties())
            //    {
            //        object o = item.GetType().GetProperty(pi.Name).GetValue(item, null);
            //    }
            //}


            if (ctrchk)
            {
                var Returnlist = report.ToListAsync().Result;
                Returnlist.Clear();
                var dic = new Dictionary<int, Holder>();

                // fill the dictionary
                int count = 1;
                foreach (var item in report)
                {
                    int UlID = count;
                    //int UlID = Convert.ToInt32(item.GetType().GetProperty("ULID").GetValue(item, null));
                    int clicks = Convert.ToInt32(item.GetType().GetProperty("Clicks").GetValue(item, null));
                    int ParentURLID_t = (int) item.GetType().GetProperty("ParentURL").GetValue(item, null);
                    int? ParentURLID = ParentURLID_t == 0 ? (int?) null : ParentURLID_t;
                    int CampaignId = Convert.ToInt32(item.GetType().GetProperty("CampaignId").GetValue(item, null));
                    //int PreviewId = Convert.ToInt32(item.GetType().GetProperty("URLPreviewId").GetValue(item, null));
                    int PreviewId = Convert.ToInt32(item.GetType().GetProperty("PAGEId").GetValue(item, null));

                    dic.Add(UlID, new Holder
                    {
                        Clicks = clicks,
                        ParentURLId = ParentURLID,
                        CampaignId = CampaignId,
                        PreviewId = PreviewId
                    });
                    count = count + 1;
                }

                myDelegate ctr = null;

                ctr = delegate(int? actionid)
                {
                    if (actionid.HasValue && dic.Keys.Contains(actionid.Value))
                        return dic[actionid.Value].Clicks + ctr(dic[actionid.Value].ParentURLId);
                    return 0;
                };

                foreach (var item in report)
                {
                    int? ParentURLID = (int?) item.GetType().GetProperty("ParentURL").GetValue(item, null);
                    int clicks = Convert.ToInt32(item.GetType().GetProperty("Clicks").GetValue(item, null));
                    int previous = ctr(ParentURLID);

                    PropertyInfo property = item.GetType().GetProperty("CTR");
                    property.SetValue(item,
                        Convert.ChangeType(previous == 0 ? clicks : clicks/(int) previous, property.PropertyType), null);
                    // item.CTR = previous == 0 ? item.Clicks : item.Clicks / (int)previous;

                    //if (ParentURLID.HasValue) // commented by njhones because it wasn't used
                    //{
                    //    if (dic.Keys.Contains(ParentURLID.Value))
                    //    {
                    //        var parent = dic[ParentURLID.Value];

                    //        //  item.ParentURLIdText = string.Format("{0} - {1}", parent.CampaignId, parent.PreviewId);
                    //    }
                    //    //item.ParentURLIdText = ParentURLID.ToString();
                    //}
                    Returnlist.Add(item);
                }
                return Ok(Returnlist);
            }


            return Ok(report);
            //return Request.CreateResponse(HttpStatusCode.OK, list);
        }

/*        private IEnumerable<Spark> Test(UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);
            var tzi = repo.FindTimeZoneInfo(null, customer.TimeZone);

            var count = 12;

            var fdate = new DateTime(DateTime.Today.AddDays(1 - count).Ticks, DateTimeKind.Unspecified);
            var tdate = new DateTime(DateTime.Today.AddDays(1).AddSeconds(-1).Ticks, DateTimeKind.Unspecified);

            var ufdate = new DateTimeOffset(fdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            var utdate = new DateTimeOffset(tdate, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            //var report = repo.DailyReport(fdate, tdate, up, tzi);

            //var report = repo.DailyReport(fdate, tdate, up.CustomerId, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId);

            //var report = repo.RunQuery<DailyView>("EXEC [Spark] {0}, {1}, {2}, {3}, {4}, {5}",
            //    up.CustomerId, ufdate, utdate, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, up.AffiliateId);

            var report = repo.RunQuery<DailyView>("EXEC [DailyReport] {0}, {1}, {2}, {3}, {4}", //, {6}, {7}, {8}",
                up.CustomerId, ufdate, utdate, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId);
                //, null, cp, ctfilter, countrycheck);

            var result = new List<Spark>();
            foreach (var item in report)
            {
                result.Add(new Spark
                {
                    Clicks = item.Clicks,
                    Conversions = item.Conversions,
                    Revenue = item.Revenue,
                });
            }

            return result;
        }*/

/*        private IHttpActionResult ClicksLogs(string id, DateTime? id2, UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            List<ClicksLogDisp> datelist = new List<ClicksLogDisp>();
            DateTime date = id2.HasValue ? id2.Value : DateTime.Today;
            var TimeZone = id ?? customer.TimeZone;
            for (int i = 0; i < 24; i++)
            {
                string hour = "";
                if (i < 10)
                {
                    hour = "0" + i.ToString();
                }
                else
                {
                    hour = i.ToString();
                }
                // datelist.Add(new ClicksLogDisp { datelist = date.AddHours(i).ToString() });
                datelist.Add(new ClicksLogDisp {datelist = string.Format("{0} {1}:00", date.ToString("MM/dd/yyyy"), hour)});
            }
            return Ok(datelist);
        }*/

/*        private IHttpActionResult ClicksDetailsLog(string timezone, DateTime? fromdate, DateTime? todate, int? cp, int[] aff,
            string source, string deviceid, string deviceos, string browser, string os, string modelname, string brandname,
            string marketingname, string resolution, UserProfile up)
        {
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //  var date = new DateTime(id2, DateTimeKind.Unspecified);
            //var offset = tzi.GetUtcOffset(date);
            var fdate = new DateTimeOffset(fromdate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            //var tdate = fdate.AddHours(1).AddMilliseconds(-1);
            var tdate = new DateTimeOffset(todate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            if (customoffset != 0)
            {
                fdate = fromdate.Value.AddMinutes(customoffset);
                tdate = todate.Value.AddMinutes(customoffset);
            }

            #region Dynamic Type

            var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
            //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
            //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));

            DynamicType.CreateAutoImplementedProperty(affview, "ClickId", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "CustomerId", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "ClickDate", typeof (DateTime));
            DynamicType.CreateAutoImplementedProperty(affview, "UserAgent", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "IPAddress", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "TransactionId", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof (decimal));
            DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof (decimal));
            DynamicType.CreateAutoImplementedProperty(affview, "BannerId", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof (int));
            DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof (string));


            ///////////////-----------Device Info-----------/////////////

            DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Isimage_inlining", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof (string));
            DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof (string));


            ///////////////-----------Device Info-----------/////////////


            var subids =
                Request.RequestUri.ParseQueryString()
                    .AllKeys.Where(s => s.ToLower().StartsWith("subid"))
                    .Select(s => int.Parse(s.Substring(5)))
                    .ToArray();
            foreach (var item in subids)
            {
                DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof (string));
            }
            Type resultType = affview.CreateType();

            #endregion

            if (up.AffiliateId.HasValue) // if is affiliate
            {
                if (aff.Length == 0)
                {
                    aff = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }
            }
            //if (aff != null && aff.Length == 0)
            //    aff = null;

            var query = CKQueryBuilder.ClicksDetailsQuery(up.CustomerId, up.UserId, fdate, tdate,
                tzi.GetUtcOffset(DateTime.Now).Hours
                , cp, aff, source, subids, deviceid, deviceos, browser, os, modelname, brandname, marketingname, resolution);
            var report = repo.ExecuteQuery(resultType, query);

            //var report = repo.RunQuery<resultType>();

            return Ok(report);
            // return Ok(detail);
        }*/

        /// <summary>
        /// Campaign details by date (user should be authorized)
        /// </summary>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any among options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, options: 512-Country)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>
        /// detail list
        /// </returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<DailyReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public IHttpActionResult Daily(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = repo.GetCurrentUser();
            return Daily(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign details by affiliate (user should be authorized)
        /// </summary>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any among options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional,any among options: 512-Country, 524288-Campaign, 16777216-Source)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>
        /// detail list
        /// </returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<AffiliateReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public IHttpActionResult Affiliate(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = repo.GetCurrentUser();
            return Affiliate(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign details by campaign (user should be authorized)
        /// </summary>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any among options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, any among options: 1048576-Affiliate, 512-Country, 8388608-URLId, 16777216-Source)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>
        /// detail list
        /// </returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CampaignReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public async Task<IHttpActionResult> Campaign(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
             long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = repo.GetCurrentUser();
            return await Campaign(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign details by hour (user should be authorized)
        /// </summary>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="dataview">data for statistics(Optional, options: 512-Country)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>
        /// detail list
        /// </returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<HourlyReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public IHttpActionResult Hourly(DateTime? FromDate = null, string timezone = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = repo.GetCurrentUser();
            return Hourly(FromDate, timezone, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign's conversion details (user should be authorized)
        /// </summary>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any among options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>Details list</returns>
        [HttpGet]
        public IEnumerable<CustomConversion> Conversion(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = repo.GetCurrentUser();
            return Conversion(FromDate, ToDate, timezone, viewdata, cp, af, ct, up);
        }

        /// <summary>
        /// Conversion status details (user should be authorized)
        /// </summary>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any among options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, no use)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>Detail list</returns>
        [HttpGet]
        public IEnumerable<ConversionStatusView> ConversionStatus(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
           long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = repo.GetCurrentUser();
            return ConversionStatus(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign details by ad/banner (user should be authorized)
        /// </summary>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any among options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, no use)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>Detail list</returns>
        [HttpGet]
        public IEnumerable<AdCampaignView> AdCampaign(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
           long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = repo.GetCurrentUser();
            return AdCampaign(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign's traffic details (user should be authorized)
        /// </summary>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any among options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, options: 512-Country)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>
        /// detail list
        /// </returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<TrafficReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public IHttpActionResult Traffic(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = repo.GetCurrentUser();
            return Traffic(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        [HttpGet]
        public IHttpActionResult CTR(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = repo.GetCurrentUser();
            return Ctr(FromDate, ToDate, timezone, viewdata, cp, af, ct, up);
        }

        /// <summary>
        /// Custom report (user should be authorized)
        /// </summary>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any among options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics in number(Optional, any among options: 2-Clicks, 4-Conversions, 1-Impressions, 524288-Campaign, 1048576-Affiliate, 4194304-URL, 274877906944-ParentURL, 549755813888-CTR, 1073741824-PAGE, 2147483648-Redirect, 2199023255552-IsSmartphone, 35184372088832-Browser, 1024-ClickDate, 2048-ConversionDate, 4096-ImpressionDate; default: considers Clicks, Conversions, Impressions)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <param name="QDate">include date(Optional, default: false)</param>
        /// <param name="QHour">include hour(Optional, default: false)</param>
        /// <param name="QCampaign">include campaign(Optional, default: false)</param>
        /// <param name="QAffiliate">include affiliate(Optional, default: false)</param>
        /// <param name="QURL">include url(Optional, default: false)</param>
        /// <param name="QCountry">include country(Optional, default: false)</param>
        /// <param name="QStatus">include status(Optional, default: false)</param>
        /// <param name="QPixel">include pixel(Optional, default: false)</param>
        /// <param name="QPostback">include postback(Optional, default: false)</param>
        /// <param name="QIP">include ip(Optional, default: false)</param>
        /// <param name="QTransactionID">include transaction id(Optional, default: false)</param>
        /// <param name="QConversionType">include conversion's type(Optional, default: false)</param>
        /// <param name="QReferrer">include referrer(Optional, default: false)</param>
        /// <param name="QStatusDescription">include status description(Optional, default: false)</param>
        /// <param name="QSource">include source(Optional, default: false)</param>
        /// <param name="QActionName">include action name(Optional, default: false)</param>
        /// <param name="QBanner">include banner(Optional, default: false)</param>
        /// <param name="QParentURL">include parrent url(Optional, default: false)</param>
        /// <param name="QCTR">include ctr(Optional, default: false)</param>
        /// <param name="QDeviceId">include device id(Optional, default: false)</param>
        /// <param name="QIsSmartphone">include is smartphone(Optional, default: false)</param>
        /// <param name="QIsiOS">include is device IOS(Optional, default: false)</param>
        /// <param name="QIsAndroid">include is device IOS(Optional, default: false)</param>
        /// <param name="QOS">include device's os name(Optional, default: false)</param>
        /// <param name="QBrowser">include device's browser(Optional, default: false)</param>
        /// <param name="QDevice_os">include device's os provider(Optional, default: false)</param>
        /// <param name="QPointing_method">include device'r pointing method</param>
        /// <param name="QIs_tablet">include is device tablet(Optional, default: false)</param>
        /// <param name="QModel_name">include device's model name(Optional, default: false)</param>
        /// <param name="QDevice_os_version">include device's os version(Optional, default: false)</param>
        /// <param name="QIs_wireless_device">include is device wireless(Optional, default: false)</param>
        /// <param name="QBrand_name">include device's brand name(Optional, default: false)</param>
        /// <param name="QMarketing_name">include device's marketing name(Optional, default: false)</param>
        /// <param name="QResolution_height">include device's display resolution height(Optional, default: false)</param>
        /// <param name="QResolution_width">include device's display resolution width(Optional, default: false)</param>
        /// <param name="QCanvas_support">include device's canvas support(Optional, default: false)</param>
        /// <param name="QViewport_width">include device's viewport width(Optional, default: false)</param>
        /// <param name="QIsviewport_supported">include does device support viewport (Optional, default: false)</param>
        /// <param name="QIsmobileoptimized">include is device mobile optimized(Optional, default: false)</param>
        /// <param name="QIshandheldfriendly">include is device handheld friendly(Optional, default: false)</param>
        /// <param name="QIs_smarttv">include is device smart tv(Optional, default: false)</param>
        /// <param name="QIsux_full_desktop">include is device ux full desktop(Optional, default: false)</param>
        /// <param name="QClickDate">include click date(Optional, default: false)</param>
        /// <param name="QConversionDate">include conversion date(Optional, default: false)</param>
        /// <param name="QImpressionDate">include impression date(Optional, default: false)</param>
        /// <param name="QPAGE">include page detail(Optional, default: false)</param>
        /// <param name="QRedirect">include redirect detail(Optional, default: false)</param>
        /// <param name="Vdeviceid">no use</param>
        /// <param name="Vdeviceos">no use</param>
        /// <param name="Vbrowser">no use</param>
        /// <param name="Vos">no use</param>
        /// <param name="Vmodelname">no use</param>
        /// <param name="Vbrandname">no use</param>
        /// <param name="Vmarketingname">no use</param>
        /// <param name="Vresolution">no use</param>
        /// <param name="UserAgent">no use</param>
        /// <param name="ReportID">no use</param>
        /// <returns>
        /// detail list
        /// </returns> 
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CustomReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public IHttpActionResult CustomReport(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
             long? dataview = null, [FromUri] int[] cp = null, [FromUri] int[] af = null, [FromUri] int[] ct = null,
              bool QDate = false, bool QHour = false, bool QCampaign = false, bool QAffiliate = false, bool QURL = false, bool QCountry = false, bool QStatus = false, bool QPixel = false, bool QPostback = false, bool QIP = false, bool QTransactionID = false, bool QConversionType = false, bool QReferrer = false, bool QStatusDescription = false, bool QSource = false, bool QActionName = false, bool QBanner = false, bool QParentURL = false, bool QCTR = false,
             bool QDeviceId = false, bool QIsSmartphone = false, bool QIsiOS = false, bool QIsAndroid = false, bool QOS = false, bool QBrowser = false, bool QDevice_os = false, bool QPointing_method = false, bool QIs_tablet = false, bool QModel_name = false, bool QDevice_os_version = false, bool QIs_wireless_device = false, bool QBrand_name = false,
              bool QMarketing_name = false, bool QResolution_height = false, bool QResolution_width = false, bool QCanvas_support = false, bool QViewport_width = false, bool QIsviewport_supported = false, bool QIsmobileoptimized = false, bool QIshandheldfriendly = false, bool QIs_smarttv = false, bool QIsux_full_desktop = false, bool QClickDate = false, bool QConversionDate = false, bool QImpressionDate = false, bool QPAGE = false, bool QRedirect = false,
              string Vdeviceid = null, string Vdeviceos = null, string Vbrowser = null, string Vos = null, string Vmodelname = null, string Vbrandname = null, string Vmarketingname = null, string Vresolution = null, string UserAgent = null,
             int ReportID = 0)
        {
            var up = repo.GetCurrentUser();
            return CustomReport(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, QDate, QHour, QCampaign, QAffiliate, QURL, QCountry, QStatus, QPixel, QPostback, QIP, up, QTransactionID, QConversionType, QReferrer, QStatusDescription, QSource, QActionName, QBanner, QParentURL, QCTR, QDeviceId, QIsSmartphone, QIsiOS, QIsAndroid, QOS, QBrowser, QDevice_os, QPointing_method, QIs_tablet, QModel_name, QDevice_os_version, QIs_wireless_device, QBrand_name, QMarketing_name, QResolution_height, QResolution_width, QCanvas_support, QViewport_width, QIsviewport_supported, QIsmobileoptimized, QIshandheldfriendly, QIs_smarttv, QIsux_full_desktop, QClickDate, QConversionDate, QImpressionDate, QPAGE, QRedirect, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname, Vmarketingname, Vresolution, UserAgent);
        }

/*        [HttpGet]
        public IEnumerable<Spark> Test()
        {
            var up = repo.GetCurrentUser();
            return Test(up);
        }

        [HttpGet]
        public IHttpActionResult ClicksLogs(string id = null, DateTime? id2 = null)
        {
            var up = repo.GetCurrentUser();
            return ClicksLogs(id, id2, up);
        }
        
        [HttpGet]
        public IHttpActionResult ClicksDetailsLog(string timezone = "", DateTime? fromdate = null, DateTime? todate = null, int? cp = null, [FromUri] int[] aff = null, string source = null, string deviceid = null, string deviceos = null, string browser = null, string os = null, string modelname = null, string brandname = null, string marketingname = null, string resolution = null)
        {
            var up = repo.GetCurrentUser();
            return ClicksDetailsLog(timezone, fromdate, todate, cp, aff, source, deviceid, deviceos, browser, os, modelname, brandname, marketingname, resolution, up);
        }*/
        
        #region AccessWithApiKey
        /// <summary>
        /// Campaign details by date (accessed via API key)
        /// </summary>
        /// <param name="apiKey">user's api key(required)</param>     
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any among options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, options: 512)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>
        /// detail list
        /// </returns>
        [HttpGet]
        [ActionName("daily_v2")]
        [ResponseType(typeof(IEnumerable<DailyReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public IHttpActionResult Daily(string apiKey = "", DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = GetUser(apiKey);
            return Daily(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign details by affiliate (accessed via API Key)
        /// </summary>
        /// <param name="apiKey">user's api key(required)</param>     
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any among options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, options: 512, 524288, 16777216)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>
        /// detail list
        /// </returns>        
        [HttpGet]
        [ActionName("affiliate_v2")]
        [ResponseType(typeof(IEnumerable<AffiliateReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public IHttpActionResult Affiliate(string apiKey = "", DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {

            var up = GetUser(apiKey);
            return Affiliate(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign details by campaign (accessed via API Key)
        /// </summary>
        /// <param name="apiKey">user's api key(required)</param>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any where options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, options: 1048576, 512, 8388608, 16777216)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>
        /// detail list
        /// </returns>
        [HttpGet]
        [ActionName("campaign_v2")]
        [ResponseType(typeof(IEnumerable<CampaignReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public async Task<IHttpActionResult> Campaign(string apiKey = "", DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
             long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = GetUser(apiKey);
            return await Campaign(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }
        
        /// <summary>
        /// Campaign details by hour (accessed via API key)
        /// </summary>
        /// <param name="apiKey">user's api key(required)</param>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="dataview">data for statistics(Optional, options: 512)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>
        /// detail list
        /// </returns>
        [HttpGet]
        [ActionName("hourly_v2")]
        [ResponseType(typeof(IEnumerable<HourlyReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public IHttpActionResult Hourly(string apiKey = "", DateTime? FromDate = null, string timezone = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = GetUser(apiKey);
            return Hourly(FromDate, timezone, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign's conversion details (accessed via API Key)
        /// </summary>
        /// <param name="apiKey">user's api key(required)</param>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any where options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>Details list</returns>
        [HttpGet]
        [ActionName("conversion_v2")]
        public IEnumerable<CustomConversion> Conversion(string apiKey = "", DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = GetUser(apiKey);
            return Conversion(FromDate, ToDate, timezone, viewdata, cp, af, ct, up);
        }

        /// <summary>
        /// Conversion status details (accessed via API Key)
        /// </summary>
        /// <param name="apiKey">user's api key(required)</param>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any where options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, no use)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>Detail list</returns>
        [HttpGet]
        [ActionName("conversionStatus_v2")]
        public IEnumerable<ConversionStatusView> ConversionStatus(string apiKey = "", DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
           long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = GetUser(apiKey);
            return ConversionStatus(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign details by ad/banner (accessed via API Key)
        /// </summary>
        /// <param name="apiKey">user's api key(required)</param>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any where options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, no use)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>Detail list</returns>
        [HttpGet]
        [ActionName("adCampaign_v2")]
        public IEnumerable<AdCampaignView> AdCampaign(string apiKey = "", DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
           long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = GetUser(apiKey);
            return AdCampaign(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        /// <summary>
        /// Campaign's traffic details (user should be authorized)
        /// </summary>
        /// <param name="apiKey">user's api key(required)</param>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any where options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics(Optional, options: 512)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <returns>
        /// detail list
        /// </returns>
        [HttpGet]
        [ActionName("traffic_v2")]
        [ResponseType(typeof(IEnumerable<TrafficReportVm>))]
        [RequestInfo("can include sub id's as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public IHttpActionResult Traffic(string apiKey = "", DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = GetUser(apiKey);
            return Traffic(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, up);
        }

        [HttpGet]
        [ActionName("ctr_v2")]
        public IHttpActionResult CTR(string apiKey = "", DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = GetUser(apiKey);
            return Ctr(FromDate, ToDate, timezone, viewdata, cp, af, ct, up);
        }

        /// <summary>
        /// Custom report (accessed via API Key)) 
        /// </summary>
        /// <param name="apiKey">user's api key(required)</param>
        /// <param name="FromDate">details from date (Optional, by default Today)</param>
        /// <param name="ToDate">details until date (Optional, by default Today)</param>
        /// <param name="timezone">user timezone name (Optional)</param>
        /// <param name="viewdata">data for duration(Optional, any where options: Today, Yesterday, Last 7 Days, This Month, Last Month, Last Six Months, All Time on reports. By default today.)</param>
        /// <param name="dataview">data for statistics in number(Optional, options: 2-Clicks, 4-Conversions, 1-Impressions, 524288-Campaign, 1048576-Affiliate, 4194304-URL, 274877906944-ParentURL, 549755813888-CTR, 1073741824-PAGE, 2147483648-Redirect, 2199023255552-IsSmartphone, 35184372088832-Browser, 1024-ClickDate, 2048-ConversionDate, 4096-ImpressionDate; default: considers Clicks, Conversions, Impressions)</param>
        /// <param name="cp">campaign id (Optional, by default considering all campaign)</param>
        /// <param name="af">affiliate ids (Optional, by default considering all affiliates</param>
        /// <param name="ct">country id (Optional, by default considering all countries</param>
        /// <param name="QDate">include date(Optional, default: false)</param>
        /// <param name="QHour">include hour(Optional, default: false)</param>
        /// <param name="QCampaign">include campaign(Optional, default: false)</param>
        /// <param name="QAffiliate">include affiliate(Optional, default: false)</param>
        /// <param name="QURL">include url(Optional, default: false)</param>
        /// <param name="QCountry">include country(Optional, default: false)</param>
        /// <param name="QStatus">include status(Optional, default: false)</param>
        /// <param name="QPixel">include pixel(Optional, default: false)</param>
        /// <param name="QPostback">include postback(Optional, default: false)</param>
        /// <param name="QIP">include ip(Optional, default: false)</param>
        /// <param name="QTransactionID">include transaction id(Optional, default: false)</param>
        /// <param name="QConversionType">include conversion's type(Optional, default: false)</param>
        /// <param name="QReferrer">include referrer(Optional, default: false)</param>
        /// <param name="QStatusDescription">include status description(Optional, default: false)</param>
        /// <param name="QSource">include source(Optional, default: false)</param>
        /// <param name="QActionName">include action name(Optional, default: false)</param>
        /// <param name="QBanner">include banner(Optional, default: false)</param>
        /// <param name="QParentURL">include parrent url(Optional, default: false)</param>
        /// <param name="QCTR">include ctr(Optional, default: false)</param>
        /// <param name="QDeviceId">include device id(Optional, default: false)</param>
        /// <param name="QIsSmartphone">include is smartphone(Optional, default: false)</param>
        /// <param name="QIsiOS">include is device IOS(Optional, default: false)</param>
        /// <param name="QIsAndroid">include is device IOS(Optional, default: false)</param>
        /// <param name="QOS">include device's os name(Optional, default: false)</param>
        /// <param name="QBrowser">include device's browser(Optional, default: false)</param>
        /// <param name="QDevice_os">include device's os provider(Optional, default: false)</param>
        /// <param name="QPointing_method">include device'r pointing method</param>
        /// <param name="QIs_tablet">include is device tablet(Optional, default: false)</param>
        /// <param name="QModel_name">include device's model name(Optional, default: false)</param>
        /// <param name="QDevice_os_version">include device's os version(Optional, default: false)</param>
        /// <param name="QIs_wireless_device">include is device wireless(Optional, default: false)</param>
        /// <param name="QBrand_name">include device's brand name(Optional, default: false)</param>
        /// <param name="QMarketing_name">include device's marketing name(Optional, default: false)</param>
        /// <param name="QResolution_height">include device's display resolution height(Optional, default: false)</param>
        /// <param name="QResolution_width">include device's display resolution width(Optional, default: false)</param>
        /// <param name="QCanvas_support">include device's canvas support(Optional, default: false)</param>
        /// <param name="QViewport_width">include device's viewport width(Optional, default: false)</param>
        /// <param name="QIsviewport_supported">include does device support viewport (Optional, default: false)</param>
        /// <param name="QIsmobileoptimized">include is device mobile optimized(Optional, default: false)</param>
        /// <param name="QIshandheldfriendly">include is device handheld friendly(Optional, default: false)</param>
        /// <param name="QIs_smarttv">include is device smart tv(Optional, default: false)</param>
        /// <param name="QIsux_full_desktop">include is device ux full desktop(Optional, default: false)</param>
        /// <param name="QClickDate">include click date(Optional, default: false)</param>
        /// <param name="QConversionDate">include conversion date(Optional, default: false)</param>
        /// <param name="QImpressionDate">include impression date(Optional, default: false)</param>
        /// <param name="QPAGE">include page detail(Optional, default: false)</param>
        /// <param name="QRedirect">include redirect detail(Optional, default: false)</param>
        /// <param name="Vdeviceid">no use</param>
        /// <param name="Vdeviceos">no use</param>
        /// <param name="Vbrowser">no use</param>
        /// <param name="Vos">no use</param>
        /// <param name="Vmodelname">no use</param>
        /// <param name="Vbrandname">no use</param>
        /// <param name="Vmarketingname">no use</param>
        /// <param name="Vresolution">no use</param>
        /// <param name="UserAgent">no use</param>
        /// <param name="ReportID">no use</param>
        /// <returns>
        /// detail list
        /// </returns>
        [HttpGet]
        [ActionName("customReport_v2")]
        [ResponseType(typeof(IEnumerable<CustomReportVm>))]
        [RequestInfo("can include sub ids as query key like Subid1&Subid2...SubId{0}, where {0} is the click's sub id")]
        public IHttpActionResult CustomReport(string apiKey = "", DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
             long? dataview = null, [FromUri] int[] cp = null, [FromUri] int[] af = null, [FromUri] int[] ct = null,
              bool QDate = false, bool QHour = false, bool QCampaign = false, bool QAffiliate = false, bool QURL = false, bool QCountry = false, bool QStatus = false, bool QPixel = false, bool QPostback = false, bool QIP = false, bool QTransactionID = false, bool QConversionType = false, bool QReferrer = false, bool QStatusDescription = false, bool QSource = false, bool QActionName = false, bool QBanner = false, bool QParentURL = false, bool QCTR = false,
             bool QDeviceId = false, bool QIsSmartphone = false, bool QIsiOS = false, bool QIsAndroid = false, bool QOS = false, bool QBrowser = false, bool QDevice_os = false, bool QPointing_method = false, bool QIs_tablet = false, bool QModel_name = false, bool QDevice_os_version = false, bool QIs_wireless_device = false, bool QBrand_name = false,
              bool QMarketing_name = false, bool QResolution_height = false, bool QResolution_width = false, bool QCanvas_support = false, bool QViewport_width = false, bool QIsviewport_supported = false, bool QIsmobileoptimized = false, bool QIshandheldfriendly = false, bool QIs_smarttv = false, bool QIsux_full_desktop = false, bool QClickDate = false, bool QConversionDate = false, bool QImpressionDate = false, bool QPAGE = false, bool QRedirect = false,
              string Vdeviceid = null, string Vdeviceos = null, string Vbrowser = null, string Vos = null, string Vmodelname = null, string Vbrandname = null, string Vmarketingname = null, string Vresolution = null, string UserAgent = null,
             int ReportID = 0)
        {

            var up = GetUser(apiKey);
            return CustomReport(FromDate, ToDate, timezone, viewdata, dataview, cp, af, ct, QDate, QHour, QCampaign, QAffiliate, QURL, QCountry, QStatus, QPixel, QPostback, QIP, up, QTransactionID, QConversionType, QReferrer, QStatusDescription, QSource, QActionName, QBanner, QParentURL, QCTR, QDeviceId, QIsSmartphone, QIsiOS, QIsAndroid, QOS, QBrowser, QDevice_os, QPointing_method, QIs_tablet, QModel_name, QDevice_os_version, QIs_wireless_device, QBrand_name, QMarketing_name, QResolution_height, QResolution_width, QCanvas_support, QViewport_width, QIsviewport_supported, QIsmobileoptimized, QIshandheldfriendly, QIs_smarttv, QIsux_full_desktop, QClickDate, QConversionDate, QImpressionDate, QPAGE, QRedirect, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname, Vmarketingname, Vresolution, UserAgent);
        }

/*        [HttpGet]
        [ActionName("test_v2")]
        public IEnumerable<Spark> Test(string apiKey = "")
        {
            var up = GetUser(apiKey);
            return Test(up);
        }

        [HttpGet]
        [ActionName("clicksLogs_v2")]
        public IHttpActionResult ClicksLogs(string apiKey = "", string id = null, DateTime? id2 = null)
        {
            var up = GetUser(apiKey);
            return ClicksLogs(id, id2, up);
        }

        [HttpGet]
        [ActionName("clicksDetailsLog_v2")]
        public IHttpActionResult ClicksDetailsLog(string apiKey = "", string timezone = "", DateTime? fromdate = null, DateTime? todate = null, int? cp = null, [FromUri] int[] aff = null, string source = null, string deviceid = null, string deviceos = null, string browser = null, string os = null, string modelname = null, string brandname = null, string marketingname = null, string resolution = null)
        {
            var up = GetUser(apiKey);
            return ClicksDetailsLog(timezone, fromdate, todate, cp, aff, source, deviceid, deviceos, browser, os, modelname, brandname, marketingname, resolution, up);
        }*/

        private UserProfile GetUser(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Api key cann't be null or empty."));
            }

            Guid apiKeyAsGuid;
            if (!Guid.TryParse(apiKey, out apiKeyAsGuid))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Invalid api key is not a Guid."));
            }

            var up = repo.GetUserFromAPIKey(apiKey);
            if (string.IsNullOrEmpty(apiKey) || up == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Api key null or empty, or user not found using it."));
            }
            return up;
        }

        #endregion
        public class ClicksLogDisp
        {
            public string datelist { get; set; }
        }

        public class InnerLogDisp
        {
            public int ClickId { get; set; }
            public int CustomerId { get; set; }
            public int CampaignId { get; set; }
            public int AffiliateId { get; set; }
            public string ClickDate { get; set; }
            public string UserAgent { get; set; }
            public string IPAddress { get; set; }
            public string Referrer { get; set; }
            public string Source { get; set; }
            public string TransactionId { get; set; }

            public decimal Cost { get; set; }

            public decimal Revenue { get; set; }

            public int BannerId { get; set; }

            public int URLPreviewId { get; set; }

            public string Country { get; set; }

            public string DeviceId { get; set; }
            public bool IsSmartphone { get; set; }
            public bool IsiOS { get; set; }
            public bool IsAndroid { get; set; }

            public string OS { get; set; }
            public string Browser { get; set; }
            public string Device_os { get; set; }
            public string Pointing_method { get; set; }
            public bool Is_tablet { get; set; }
            public string Model_name { get; set; }
            public string Device_os_version { get; set; }
            public bool Is_wireless_device { get; set; }
            public string Brand_name { get; set; }
            public string Marketing_name { get; set; }
            public bool Is_assign_phone_number { get; set; }
            public string Xhtmlmp_mime_type { get; set; }
            public string Xhtml_support_level { get; set; }
            public string Resolution_height { get; set; }
            public string Resolution_width { get; set; }
            public string Canvas_support { get; set; }
            public string Viewport_width { get; set; }
            public string Html_preferred_dtd { get; set; }
            public bool Isviewport_supported { get; set; }
            public bool Ismobileoptimized { get; set; }

            public bool Isimage_inlining { get; set; }
            public bool Ishandheldfriendly { get; set; }
            public bool Is_smarttv { get; set; }
            public bool Isux_full_desktop { get; set; }
            public StatisticsEnum StatisticsEnum { get; set; }
        }

        public class InnerQueryLogDisp
        {
            public int ClickId { get; set; }
            public int CustomerId { get; set; }
            public int CampaignId { get; set; }
            public int AffiliateId { get; set; }
            public DateTime ClickDate { get; set; }
            public string UserAgent { get; set; }
            public string IPAddress { get; set; }
            public string Referrer { get; set; }
            public string Source { get; set; }
            public string TransactionId { get; set; }

            public decimal Cost { get; set; }

            public decimal Revenue { get; set; }

            public int BannerId { get; set; }

            public int URLPreviewId { get; set; }

            public string Country { get; set; }

            public string DeviceId { get; set; }
            public string IsSmartphone { get; set; }
            public string IsiOS { get; set; }
            public string IsAndroid { get; set; }

            public string OS { get; set; }
            public string Browser { get; set; }
            public string Device_os { get; set; }
            public string Pointing_method { get; set; }
            public string Is_tablet { get; set; }
            public string Model_name { get; set; }
            public string Device_os_version { get; set; }
            public string Is_wireless_device { get; set; }
            public string Brand_name { get; set; }
            public string Marketing_name { get; set; }
            public string Is_assign_phone_number { get; set; }
            public string Xhtmlmp_mime_type { get; set; }
            public string Xhtml_support_level { get; set; }
            public string Resolution_height { get; set; }
            public string Resolution_width { get; set; }
            public string Canvas_support { get; set; }
            public string Viewport_width { get; set; }
            public string Html_preferred_dtd { get; set; }
            public string Isviewport_supported { get; set; }
            public string Ismobileoptimized { get; set; }

            public string Isimage_inlining { get; set; }
            public string Ishandheldfriendly { get; set; }
            public string Is_smarttv { get; set; }
            public string Isux_full_desktop { get; set; }
            public StatisticsEnum StatisticsEnum { get; set; }
        }

        private delegate int myDelegate(int? id);

        /// <summary>
        /// Daily report detail
        /// </summary>
        public class DailyReportVm
        {
            /// <summary>
            /// click's date mm/dd/yyyy
            /// </summary>
            public string Date { get; set; }
            /// <summary>
            /// total impression
            /// </summary>
            public int Impressions { get; set; }
            /// <summary>
            /// total conversion
            /// </summary>
            public int Conversions { get; set; }
            /// <summary>
            /// total click
            /// </summary>
            public int Clicks { get; set; }
            /// <summary>
            /// total cost
            /// </summary>
            public decimal Cost { get; set; }
            /// <summary>
            /// total revenue
            /// </summary>
            public decimal Revenue { get; set; }
            /// <summary>
            /// country name
            /// </summary>
            [AdditionalInfo("Included, when data statistics option 512-Country")]
            public string Country { get; set; }

            /// <summary>
            /// click's sub id. Included when query string with key SubId1, where 1 is the click's sub id
            /// </summary>
            public string SubId1 { get; set; }
            /// <summary>
            /// click's sub id. Included when query string with key SubId2, where 2 is the click's sub id
            /// </summary>
            public string SubId2 { get; set; }
        }

        /// <summary>
        /// Affiliate report detail
        /// </summary>
        public class AffiliateReportVm
        {
            /// <summary>
            /// affiliate id
            /// </summary>
            public int AffiliateId { get; set; }
            /// <summary>
            /// company name
            /// </summary>
            public string Company { get; set; }
            /// <summary>
            /// total impression
            /// </summary>
            public int Impressions { get; set; }
            /// <summary>
            /// total conversion
            /// </summary>
            public int Conversions { get; set; }
            /// <summary>
            /// total click
            /// </summary>
            public int Clicks { get; set; }
            /// <summary>
            /// total cost
            /// </summary>
            public decimal Cost { get; set; }
            /// <summary>
            /// total revenue
            /// </summary>
            public decimal Revenue { get; set; }
            /// <summary>
            /// country name
            /// </summary>
            [AdditionalInfo("Included, when data statistics option 512-Country")]
            public string Country { get; set; }
            /// <summary>
            /// campaign id
            /// </summary>
            [AdditionalInfo("Included, when data statistics with option, 524288-Campaign")]
            public int CampaignId { get; set; }
            /// <summary>
            /// campaign name
            /// </summary>
            [AdditionalInfo("Included, when data statistics with option, 524288-Campaign")]
            public string CampaignName { get; set; }
            /// <summary>
            /// click's source name 
            /// </summary>
            [AdditionalInfo("Included, when data statistics with option, 16777216-Source")]
            public string Source { get; set; }
            /// <summary>
            /// click's sub id. Included, when query string with key SubId1, where 1 is the click's sub id
            /// </summary>
            public string SubId1 { get; set; }
            /// <summary>
            /// click's sub id. Included, when query string with key SubId2, where 2 is the click's sub id
            /// </summary>
            public string SubId2 { get; set; }
        }

        /// <summary>
        /// Campaign report detail
        /// </summary>
        public class CampaignReportVm
        {
            /// <summary>
            /// affiliate id
            /// </summary>
            public int AffiliateId { get; set; }
            /// <summary>
            /// company name
            /// </summary>
            public string Company { get; set; }
            /// <summary>
            /// total impression
            /// </summary>
            public int Impressions { get; set; }
            /// <summary>
            /// total conversion
            /// </summary>
            public int Conversions { get; set; }
            /// <summary>
            /// total click
            /// </summary>
            public int Clicks { get; set; }
            /// <summary>
            /// total cost
            /// </summary>
            public decimal Cost { get; set; }
            /// <summary>
            /// total revenue
            /// </summary>
            public decimal Revenue { get; set; }
            /// <summary>
            /// country name
            /// </summary>
            [AdditionalInfo("Included, when data statistics option 512-Country")]
            public string Country { get; set; }
            /// <summary>
            /// url's PreviewId id
            /// </summary>
            [AdditionalInfo("Included, when data statistics with option 8388608-URLId")]
            public int URLPreviewId { get; set; }
            /// <summary>
            /// url's offer url
            /// </summary>
            [AdditionalInfo("Included, when data statistics with option 8388608-URLId")]
            public string OfferUrl { get; set; }
            /// <summary>
            /// campaign id
            /// </summary>
            [AdditionalInfo("Included, when data statistics with option, 524288-Campaign")]
            public int CampaignId { get; set; }
            /// <summary>
            /// campaign name
            /// </summary>
            [AdditionalInfo("Included, when data statistics with option, 524288-Campaign")]
            public string CampaignName { get; set; }
            /// <summary>
            /// click's source name 
            /// </summary>
            [AdditionalInfo("Included, when data statistics with option, 16777216-Source")]
            public string Source { get; set; }
            /// <summary>
            /// click's sub id. Included, when query string with key SubId1, where 1 is the click's sub id
            /// </summary>
            public string SubId1 { get; set; }
            /// <summary>
            /// click's sub id. Included, when query string with key SubId2, where 2 is the click's sub id
            /// </summary>
            public string SubId2 { get; set; }
        }
        
        /// <summary>
        /// Daily report detail
        /// </summary>
        public class HourlyReportVm
        {
            /// <summary>
            /// click's hour hh
            /// </summary>
            public string Hour { get; set; }
            /// <summary>
            /// total impression
            /// </summary>
            public int Impressions { get; set; }
            /// <summary>
            /// total conversion
            /// </summary>
            public int Conversions { get; set; }
            /// <summary>
            /// total click
            /// </summary>
            public int Clicks { get; set; }
            /// <summary>
            /// total cost
            /// </summary>
            public decimal Cost { get; set; }
            /// <summary>
            /// total revenue
            /// </summary>
            public decimal Revenue { get; set; }
            /// <summary>
            /// country name
            /// </summary>
            [AdditionalInfo("Included, when data statistics option 512-Country")]
            public string Country { get; set; }

            /// <summary>
            /// click's sub id. Included when query string with key SubId1, where 1 is the click's sub id
            /// </summary>
            public string SubId1 { get; set; }
            /// <summary>
            /// click's sub id. Included when query string with key SubId2, where 2 is the click's sub id
            /// </summary>
            public string SubId2 { get; set; }
        }

        /// <summary>
        /// Traffic report detail
        /// </summary>
        public class TrafficReportVm
        {
            /// <summary>
            /// affiliate id
            /// </summary>
            public int AffiliateId { get; set; }
            /// <summary>
            /// company name
            /// </summary>
            public string Company { get; set; }
            /// <summary>
            /// total impression
            /// </summary>
            public int Impressions { get; set; }
            /// <summary>
            /// total conversion
            /// </summary>
            public int Conversions { get; set; }
            /// <summary>
            /// total click
            /// </summary>
            public int Clicks { get; set; }
            /// <summary>
            /// total cost
            /// </summary>
            public decimal Cost { get; set; }
            /// <summary>
            /// total revenue
            /// </summary>
            public decimal Revenue { get; set; }
            /// <summary>
            /// country name
            /// </summary>
            [AdditionalInfo("Included, when data statistics option 512-Country")]
            public string Country { get; set; }
            /// <summary>
            /// url's PreviewId id
            /// </summary>
            public int URLPreviewId { get; set; }
            /// <summary>
            /// url's offer url
            /// </summary>
            public string OfferUrl { get; set; }
            /// <summary>
            /// campaign id
            /// </summary>
            public int CampaignId { get; set; }
            /// <summary>
            /// campaign name
            /// </summary>
            public string CampaignName { get; set; }
            /// <summary>
            /// click's sub id. Included, when query string with key SubId1, where 1 is the click's sub id
            /// </summary>
            public string SubId1 { get; set; }
            /// <summary>
            /// click's sub id. Included, when query string with key SubId2, where 2 is the click's sub id
            /// </summary>
            public string SubId2 { get; set; }
        }

        /// <summary>
        ///     Custom report detail
        /// </summary>
        public class CustomReportVm
        {
            /// <summary>
            /// total impression
            /// </summary>
            public int Impressions { get; set; }
            /// <summary>
            /// total conversion
            /// </summary>
            public int Conversions { get; set; }
            /// <summary>
            /// total click
            /// </summary>
            public int Clicks { get; set; }
            /// <summary>
            ///     cost
            /// </summary>
            public decimal Cost { get; set; }
            /// <summary>
            ///     revenue
            /// </summary>
            public decimal Revenue { get; set; }
            /// <summary>
            /// click/conversion/impression hour:hh
            /// </summary>
            [AdditionalInfo("when include hour is true, and any among include click date, include conversion date, include impression date is true")]
            public string Hour { get; set; }
            /// <summary>
            /// click/conversion/impression date:mm/dd/yyyy 
            /// </summary>
            [AdditionalInfo("when include hour is true, and any among include click date, include conversion date, include impression date is true")]
            public string Date { get; set; }
            /// <summary>
            /// click/conversion/impression datetime
            /// </summary>
            [AdditionalInfo("when include hour is true, and any among include click date, include conversion date, include impression date is true")]
            public DateTime? CDatetime { get; set; }
            /// <summary>
            /// click datetime
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include click date is true
                or
                    include click date false
                    and
                    data statistics option, 1024(ClickDate
            ")]
            public DateTime? ClickDate { get; set; }
            /// <summary>
            /// impression datetime
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include impression date is true
                or
                    include impression date false
                    and
                    data statistics option, 4096(ImpressionDate)
            ")]
            public DateTime? ImpressionDate { get; set; }
            /// <summary>
            /// conversion datetime
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include conversion date is true
                or
                    include conversion date false
                    and
                    data statistics option, 2048(ConversionDate)
            ")]
            public DateTime? ConversionDate { get; set; }
            /// <summary>
            ///     conversion id
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include conversion date is true
                or
                    include conversion date false
                    and
                    data statistics option, 2048(ConversionDate)
            ")]            
            public int ConversionID { get; set; }
            /// <summary>
            ///     campaign id
            /// </summary>
            [AdditionalInfo(@"
                when 
                        include campaingn is true
                    or
                        include campaingn false
                        and
                        data statistics option, 524288(Campaign)
                OR 
                        include ctr is true
                    or
                        include ctr false
                        and
                        data statistics option, 549755813888(CTR)
            ")]
            public int CampaignId { get; set; }
            /// <summary>
            ///     campaign name
            /// </summary>
            [AdditionalInfo(@"
                when 
                        include campaingn is true
                    or
                        include campaingn false
                        and
                        data statistics option, 524288(Campaign)
                OR 
                        include ctr is true
                    or
                        include ctr false
                        and
                        data statistics option, 549755813888(CTR)
            ")]
            public string CampaignName { get; set; }
            /// <summary>
            ///     affiliate id
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include affiliate is true
                or
                    include affiliate false
                    and
                    data statistics option, 1048576(Affiliate)
            ")]            
            public int AffiliateId { get; set; }
            /// <summary>
            ///     company name
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include affiliate is true
                or
                    include affiliate false
                    and
                    data statistics option, 1048576(Affiliate)
            ")]             
            public string Company { get; set; }
            /// <summary>
            ///     clicked url's preview id
            /// </summary>
            [AdditionalInfo(@"
                when 
                        include page is true
                    or
                        include page false
                        and
                        data statistics option, 1073741824(PAGE)
                OR 
                        include ctr is true
                    or
                        include ctr false
                        and
                        data statistics option, 549755813888(CTR)
            ")]
            public int PAGEId { get; set; }
            /// <summary>
            ///     clicked url's preview url
            /// </summary>
            [AdditionalInfo(@"
                when 
                        include page is true
                    or
                        include page false
                        and
                        data statistics option, 1073741824(PAGE)
                OR 
                        include ctr is true
                    or
                        include ctr false
                        and
                        data statistics option, 549755813888(CTR)
            ")]
            public string PAGEURL { get; set; }
            /// <summary>
            ///     clicked url's preview id
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include redirect is true
                or
                    include redirect false
                    and
                    data statistics option, 2147483648(Redirect)
            ")]
            public int RedirectId { get; set; }
            /// <summary>
            ///     clicked url's preview url
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include redirect is true
                or
                    include redirect false
                    and
                    data statistics option, 2147483648(Redirect)
            ")]
            public string RedirectURL { get; set; }
            /// <summary>
            ///     conversion's country
            /// </summary>
            [AdditionalInfo("when include country is true")]
            public string Country { get; set; }
            /// <summary>
            ///     status
            /// </summary>
            [AdditionalInfo("when include status is true")]
            public int Status { get; set; }
            /// <summary>
            ///     conversion's pixel
            /// </summary>
            [AdditionalInfo("when include pixel is true")]
            public string Pixel { get; set; }
            /// <summary>
            ///     conversion's postback date time
            /// </summary>
            [AdditionalInfo("when include postback is true")]
            public string Postback { get; set; }
            /// <summary>
            ///     Ip address
            /// </summary>
            [AdditionalInfo("when include ip is true")]
            public string IP { get; set; }
            /// <summary>
            ///     transaction id
            /// </summary>
            [AdditionalInfo("when include transaction id is true")]
            public string TransactionID { get; set; }
            /// <summary>
            ///     conversion type
            /// </summary>
            [AdditionalInfo("when include conversion type is true")]
            public ConversionType ConversionType { get; set; }
            /// <summary>
            ///     click's referrer
            /// </summary>
            [AdditionalInfo("when include referrer is true")]
            public string Referrer { get; set; }
            /// <summary>
            ///     status description
            /// </summary>
            [AdditionalInfo("when include status description is true")]
            public string StatusDescription { get; set; }
            /// <summary>
            ///     click source
            /// </summary>
            [AdditionalInfo("when include source is true")]
            public string Source { get; set; }
            /// <summary>
            ///     action name
            /// </summary>
            [AdditionalInfo("when include action is true")]
            public string ActionName { get; set; }
            /// <summary>
            ///     action id
            /// </summary>
            [AdditionalInfo("when include action is true")]
            public int ActionId { get; set; }
            /// <summary>
            ///     action type
            /// </summary>
            [AdditionalInfo("when include action is true")]
            public ConversionType ActionType { get; set; }
            /// <summary>
            /// banner
            /// </summary>
            [AdditionalInfo("when include banner is true")]
            public string Banner { get; set; }
            /// <summary>
            /// parent url id
            /// </summary>
            [AdditionalInfo(@"
                when 
                        include parrent url is true
                    or
                        include parrent url false
                        and
                        data statistics option, 274877906944(ParentURL)
                OR 
                        include ctr is true
                    or
                        include ctr false
                        and
                        data statistics option, 549755813888(CTR)
            ")]
            public int? ParentURL { get; set; }
            /// <summary>
            /// ctr
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include ctr is true
                or
                    include ctr false
                    and
                    data statistics option, 549755813888(CTR)
            ")]
            public int CTR { get; set; }
            /// <summary>
            ///     device id
            /// </summary>
            [AdditionalInfo("when include device id is true")]
            public string DeviceId { get; set; }
            /// <summary>
            ///     is device a smartphone
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include is smartphone is true
                or
                    include is smartphone false
                    and
                    data statistics option, 2199023255552(IsSmartphone)
            ")]
            public bool IsSmartphone { get; set; }
            /// <summary>
            ///     is device IOS
            /// </summary>
            [AdditionalInfo("when include is device ios, is true")]
            public bool IsiOS { get; set; }
            /// <summary>
            ///     is device Android
            /// </summary>
            [AdditionalInfo("when include is device ardroid, is true")]
            public bool IsAndroid { get; set; }
            /// <summary>
            ///     device's installed os detail, like Android 5.0
            /// </summary>
            [AdditionalInfo("when include device os, is true")]
            public string OS { get; set; }
            /// <summary>
            ///     used browser
            /// </summary>
            [AdditionalInfo(@"
                when 
                    include browser is true
                or
                    include ctr false
                    and
                    data statistics option, 35184372088832(Browser)
            ")]
            public string Browser { get; set; }
            /// <summary>
            ///     device's os provider, like Android
            /// </summary>
            [AdditionalInfo("when include device's os provider is true")]
            public string Device_os { get; set; }
            /// <summary>
            ///     device's pointin method, like touchscreen/mouse
            /// </summary>
            [AdditionalInfo("when include device's pointing method is true")]
            public string Pointing_method { get; set; }
            /// <summary>
            ///     id device tablet
            /// </summary>
            [AdditionalInfo("when include is device tablet, is true")]
            public bool Is_tablet { get; set; }
            /// <summary>
            ///     device build model name
            /// </summary>
            [AdditionalInfo("when include device model name, is true")]
            public string Model_name { get; set; }
            /// <summary>
            ///     device's installed os detail, like Android 5.0 where 5.0 is the version
            /// </summary>
            [AdditionalInfo("when include device's os version, is true")]
            public string Device_os_version { get; set; }
            /// <summary>
            ///     is device wireless
            /// </summary>
            [AdditionalInfo("when include is device wireless, is true")]
            public bool Is_wireless_device { get; set; }
            /// <summary>
            ///     device brand/manufacturer name
            /// </summary>
            [AdditionalInfo("when include device brand name, is true")]
            public string Brand_name { get; set; }
            /// <summary>
            ///     device marketing name
            /// </summary>
            [AdditionalInfo("when include device marketing name, is true")]
            public string Marketing_name { get; set; }
            /// <summary>
            ///     device display resolution height
            /// </summary>
            [AdditionalInfo("when include device display resolution height, is true")]
            public string Resolution_height { get; set; }
            /// <summary>
            ///     device display resolution width
            /// </summary>
            [AdditionalInfo("when include device display resolution width, is true")]
            public string Resolution_width { get; set; }
            /// <summary>
            ///     device's canvas support
            /// </summary>
            [AdditionalInfo("when include device's canvas support, is true")]
            public string Canvas_support { get; set; }
            /// <summary>
            ///     device's viewport width
            /// </summary>
            [AdditionalInfo("when include device's viewport width, is true")]
            public string Viewport_width { get; set; }
            /// <summary>
            ///     does device support view port
            /// </summary>
            [AdditionalInfo("when include does device support viewport, is true")]
            public int Isviewport_supported { get; set; }
            /// <summary>
            ///     is device mobile optimized
            /// </summary>
            [AdditionalInfo("when include is device mobile optimized, is true")]
            public bool Ismobileoptimized { get; set; }
            /// <summary>
            ///     Is handheld friendly
            /// </summary>
            [AdditionalInfo("when include is device handheld friendly, is true")]
            public bool Ishandheldfriendly { get; set; }
            /// <summary>
            ///     Is device smart tv
            /// </summary>
            [AdditionalInfo("when include is device smart tv, is true")]
            public int Is_smarttv { get; set; }
            /// <summary>
            ///     Is device ux full desktop
            /// </summary>
            [AdditionalInfo("when include is device ux full desktop, is true")]
            public int Isux_full_desktop { get; set; }
            /// <summary>
            /// click's sub id. Included, when query string with key SubId1, where 1 is the click's sub id
            /// </summary>
            public string SubId1 { get; set; }
            /// <summary>
            /// click's sub id. Included, when query string with key SubId2, where 2 is the click's sub id
            /// </summary>
            public string SubId2 { get; set; }
        }
    }
}
