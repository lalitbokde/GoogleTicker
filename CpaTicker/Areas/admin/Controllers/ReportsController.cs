using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Models;
using WebMatrix.WebData;
using System.Data.Entity.SqlServer;
using System.Text;
using System.Data.SqlClient;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Reflection;
//using System.Web.Http;

namespace CpaTicker.Areas.admin.Controllers
{
    [Authorize]
    public class ReportsController : BaseController
    {
        private ICpaTickerRepository repo;

        public ReportsController()
        {
            this.repo = new EFCpatickerRepository();
        }

        public ActionResult Affiliate(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, [System.Web.Http.FromUri] int[] af = null, int? ct = null, int state = 0,
            bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);
            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;


            CustomViewModel model = new CustomViewModel();

            try
            {
                model.CustomStatisticsEnum = (CustomStatisticsEnum)dataview;
            }
            catch
            {
                model.CustomStatisticsEnum =

                                CustomStatisticsEnum.CR |
                                CustomStatisticsEnum.CPC |
                                CustomStatisticsEnum.RPC |
                                CustomStatisticsEnum.Impressions |
                                CustomStatisticsEnum.Clicks |
                                CustomStatisticsEnum.Conversions |
                                CustomStatisticsEnum.Cost |
                                CustomStatisticsEnum.Revenue |
                                CustomStatisticsEnum.Profit |
                                CustomStatisticsEnum.Affiliate
                                ;
            }

            var countrycheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Country);



            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            var offset = tzi.GetUtcOffset(DateTime.Now);
            DateTime ufdate = new DateTimeOffset(fdate, offset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(tdate, offset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int Offsethour = tzi.GetUtcOffset(DateTime.Now).Hours;
            if (customoffset != 0)
            {
                Offsethour = customoffset / 60;
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                ViewBag.TimeZone = timezone;
            }



            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            bool campaigncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Campaign); //string.IsNullOrEmpty(offview);
            bool sourcechk = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Source); //!string.IsNullOrEmpty(sourceview);
            //ViewBag.SimpleReport = simplereport;
            //ViewBag.SourceCheck = sourcechk;

            // get the subids
            var subids = Request.QueryString.AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();
            ViewBag.MaxSubId = repo.ClickSubIds().Max(c => (int?)c.SubIndex) ?? 0;
            ViewBag.SubIds = subids;

            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up).Where(c => c.Status == Status.Active)
                .AsEnumerable().Select(c => new SelectListItem
                {
                    Value = c.CampaignId.ToString(),
                    Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
                });

            //string affs = null;
            //bool faffiliate = false;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {
                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;


                //af = new int [] { up.AffiliateId.Value }; // set the af to this af
                //affs = up.AffiliateId.ToString();
                //   af = new int[] { up.AffiliateId.Value };
            }

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            /******************************************************************************************************************/

            if (state == 0)
            {
                //foreach (var item in list)
                //{
                //    lavm.AffiliateViewList.Add(item);
                //}

                //ViewBag.list = list;
                //lavm.AffiliateViewList = list;

                return View("CustomReport", model);
            }

            else
            {

                #region Dynamic Statistics

                bool datecheck = false;
                bool hourcheck = false;
                //bool campaigncheck = stats.HasFlag(StatisticsEnum.Campaign);  //string.IsNullOrEmpty(offview);
                bool affiliatecheck = true;
                bool urlcheck = false;

                //var countrycheck = stats.HasFlag(StatisticsEnum.Country);
                bool statuscheck = false;
                bool pixelcheck = false;
                bool postbackcheck = false;
                bool ipcheck = false;
                bool transactionidcheck = false;
                bool conversiontypecheck = false;
                bool referrercheck = false;
                bool statusdesccheck = false;
                //bool sourcechk = stats.HasFlag(StatisticsEnum.Source); //!string.IsNullOrEmpty(sourceview);
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
                string Vdeviceid = null; string Vdeviceos = null; string Vbrowser = null; string Vos = null; string Vmodelname = null; string Vbrandname = null; string Vmarketingname = null; string Vresolution = null; string UserAgent = null;
                //Filtering Device Infoes//


                //// get the subids
                //var debug = Request.RequestUri.ParseQueryString();
                //var subids = Request.RequestUri.ParseQueryString().AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();

                string ctfilter = ct.HasValue ?
                                       repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                                       : null;

                #endregion

                #region Dynamic Type
                var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
                //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


                if (hourcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof(int));
                }
                if (datecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof(string));
                }

                if (campaigncheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof(string));
                }
                if (affiliatecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                }

                if (urlcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof(string));
                }
                if (countrycheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof(string));
                }
                if (statuscheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof(int));
                }
                if (pixelcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof(DateTime?));
                }
                if (postbackcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof(DateTime?));
                }
                if (ipcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof(string));
                }
                if (transactionidcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof(string));
                }
                if (conversiontypecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof(ConversionType));
                }
                if (referrercheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof(string));
                }
                if (statusdesccheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof(string));
                }
                if (sourcechk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof(string));
                }
                if (actionchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof(string));
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof(int));

                }
                if (bannerchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof(string));
                }
                if (parenturlchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                }
                if (ctrchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                }


                ///////////////-----------Device Info-----------/////////////

                if (deviceid) { DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof(string)); }
                if (issmartphone) { DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof(string)); }
                if (isios) { DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof(string)); }
                if (isandroid) { DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof(string)); }
                if (os) { DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof(string)); }
                if (browser) { DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof(string)); }
                if (device_os) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof(string)); }
                if (pointing_method) { DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof(string)); }
                if (is_tablet) { DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof(string)); }
                if (model_name) { DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof(string)); }
                if (device_os_version) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof(string)); }
                if (is_wireless_device) { DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof(string)); }
                if (brand_name) { DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof(string)); }
                if (marketing_name) { DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof(string)); }
                if (resolution_height) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof(string)); }
                if (resolution_width) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof(string)); }
                if (canvas_support) { DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof(string)); }
                if (viewport_width) { DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof(string)); }
                if (isviewport_supported) { DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof(string)); }
                if (ismobileoptimized) { DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof(string)); }
                if (ishandheldfriendly) { DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof(string)); }
                if (is_smarttv) { DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof(string)); }
                if (isux_full_desktop) { DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof(string)); }



                ///////////////-----------Device Info-----------/////////////



                DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof(decimal));
                DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof(decimal));
                foreach (var item in subids)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof(string));
                }
                Type resultType = affview.CreateType();

                #endregion

                var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, utdate, Offsethour, datecheck, hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck, ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk, bannerchk, parenturlchk, ctrchk,
                   deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name, device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width, canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv, isux_full_desktop,
                 subids.Length > 0 ? subids : null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname, Vmarketingname, Vresolution, UserAgent
                 );

                dynamic list = repo.ExecuteQuery(resultType, query);

                //ViewBag._from = ufdate;
                //ViewBag._to = utdate;
                //ViewBag._query = query;

                StringBuilder sb = sb = new StringBuilder();

                var subidcheck = subids.Length > 0 ? true : false;
                var impressioncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Impressions);
                var clickcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Clicks);
                var conversioncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Conversions);
                var costcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Cost);
                var revenuecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Revenue);
                var profitcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Profit);
                var _countrycheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Country);
                var crcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CR);
                var cpccheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CPC);
                var rpccheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.RPC);

                //sb = new StringBuilder("Company,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");

                if (campaigncheck) sb.AppendFormat("Campaign,");
                if (sourcechk) sb.AppendFormat("Source,");

                if (subidcheck)
                {
                    for (int i = 0; i < subids.Length; i++)
                    {
                        sb.AppendFormat("SubId" + subids[i] + ",");
                    }
                }



                sb.AppendFormat("Company,");

                if (impressioncheck) sb.AppendFormat("Impressions,");
                if (clickcheck) sb.AppendFormat("Clicks,");
                if (conversioncheck) sb.AppendFormat("Conversions,");
                if (crcheck) sb.AppendFormat("Conv Rate,");
                if (costcheck) sb.AppendFormat("Cost,");
                if (cpccheck) sb.AppendFormat("CPC,");
                if (revenuecheck) sb.AppendFormat("Revenue,");
                if (rpccheck) sb.AppendFormat("RPC,");
                if (profitcheck) sb.AppendFormat("Profit,");
                if (_countrycheck) sb.AppendFormat("Country,");
                sb.AppendLine();

                foreach (var item in list)
                {
                    if (campaigncheck) sb.AppendFormat("{0},", item.CampaignId);
                    if (sourcechk) sb.AppendFormat("{0},", item.Source);
                    if (subidcheck)
                    {
                        for (int a = 0; a < subids.Length; a++)
                        {
                            string sub = "SubId" + subids[a];
                            sb.AppendFormat("{0},", item.GetType().GetProperty(sub).GetValue(item, null));
                            //sb.AppendFormat("{0},", item.SubId + subids[a]); 
                        }
                    }


                    sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);

                    // impressions
                    if (impressioncheck) sb.AppendFormat("{0},", item.Impressions);
                    //clicks
                    if (clickcheck) sb.AppendFormat("{0},", item.Clicks);
                    //conversions
                    if (conversioncheck) sb.AppendFormat("{0},", item.Conversions);
                    // conv rate
                    if (crcheck) sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));
                    // cost
                    if (costcheck) sb.AppendFormat("${0},", item.Cost.ToString("F2"));
                    // cpc
                    if (cpccheck) sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));
                    // revenue
                    if (revenuecheck) sb.AppendFormat("${0},", item.Revenue.ToString("F2"));
                    // rpc
                    if (rpccheck) sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));
                    // profit
                    if (profitcheck) sb.AppendFormat("${0},", (item.Revenue - item.Cost).ToString("F2"));
                    if (_countrycheck) sb.AppendFormat("{0},", item.Country);
                    sb.AppendLine();
                }
                //}
                //else
                //{
                //    sb = new StringBuilder("Campaign,Company,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                //    sb.AppendLine();

                //    foreach (var item in list)
                //    {
                //        sb.AppendFormat("{0},", item.CampaignId);
                //        sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);
                //        // impressions
                //        sb.AppendFormat("{0},", item.Impressions);
                //        //clicks
                //        sb.AppendFormat("{0},", item.Clicks);
                //        //conversions
                //        sb.AppendFormat("{0},", item.Conversions);
                //        // conv rate
                //        sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));
                //        // cost
                //        sb.AppendFormat("${0},", item.Cost.ToString("F2"));
                //        // cpc
                //        sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));
                //        // revenue
                //        sb.AppendFormat("${0},", item.Revenue.ToString("F2"));
                //        // rpc
                //        sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));
                //        // profit
                //        sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));
                //        sb.AppendLine();
                //    }
                //}

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_affiliates.csv");
            }
        }

        public ActionResult ConversionStatus(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;

            var model = new ConversionStatusViewModel();

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            try
            {
                model.StatisticsEnum = (StatisticsEnum)dataview;

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

            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up).Where(c => c.Status == Status.Active).AsEnumerable().Select(c => new SelectListItem
            {
                Value = c.CampaignId.ToString(),
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
            });

            string affs = null;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {
                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                    // set the parameter for the query
                    var sb = new StringBuilder();
                    foreach (var item in af)
                    {
                        sb.AppendFormat("{0},", item);
                    }
                    affs = sb.ToString().TrimEnd(',');
                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {

                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;

                var sb = new StringBuilder();
                foreach (var item in af)
                {
                    sb.AppendFormat("{0},", item);
                }
                affs = sb.ToString().TrimEnd(',');

                // af = new int[] { up.AffiliateId.Value };
            }

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            var offset = tzi.GetUtcOffset(DateTime.Now);
            DateTime ufdate = new DateTimeOffset(fdate, offset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(tdate, offset).UtcDateTime;


            ViewBag.TimeZone = tzi.Id;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                ViewBag.TimeZone = timezone;
            }


            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            /*==============================================================================================================*/

            if (state == 0)
            {


                return View(model);
            }
            else
            {
                string ctfilter = ct.HasValue ?
                repo.GetCountries()
                .Where(c => c.Id == ct.Value)
                .Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

                var approved = from c in repo.ConversionQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
                               group c by new
                               {
                                   AffiliateId = c.AffiliateId,
                                   CampaignId = c.CampaignId,
                                   Country = countrycheck ? c.Country ?? "" : "",
                               } into dc
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
                               } into dc
                               select new
                               {
                                   CampaignId = dc.Key.CampaignId,
                                   AffiliateId = dc.Key.AffiliateId,
                                   Country = dc.Key.Country,
                                   RejectedConversions = dc.Count(),
                               };

                var union = approved.Select(c => new { AffiliateId = c.AffiliateId, CampaignId = c.CampaignId, Country = c.Country })
                   .Union(rejected.Select(c => new { AffiliateId = c.AffiliateId, CampaignId = c.CampaignId, Country = c.Country }));


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
                                  equals new { AffiliateId = a.AffiliateId, CampaignId = a.CampaignId, Country = a.Country } into da
                             from a in da.DefaultIfEmpty()

                             join r in rejected on c.Union
                                  equals new { AffiliateId = r.AffiliateId, CampaignId = r.CampaignId, Country = r.Country } into dr
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


                var grossconversioncheck = model.StatisticsEnum.HasFlag(StatisticsEnum.GrossConversions);
                var approvedconvcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.ApprovedConversions);
                var percentapprovedcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.ApprovedPercent);
                var rejectedconvcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.RejectedConversions);
                var percentrejectcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.RejectedPercent);
                var netpayoutcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.NetPayout);
                var netrevenuecheck = model.StatisticsEnum.HasFlag(StatisticsEnum.NetRevenue);
                var _countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);



                //var sb = new StringBuilder("Offer,Affiliate,Gross Conversions,Approved Conversions,%Approved,RejectedConversions,%Rejected,Net Payout,Net Revenue");
                var sb = new StringBuilder();

                sb.AppendFormat("Offer,");
                sb.AppendFormat("Affiliate,");

                if (grossconversioncheck) sb.AppendFormat("Gross Conversions,");
                if (approvedconvcheck) sb.AppendFormat("Approved Conversions,");
                if (percentapprovedcheck) sb.AppendFormat("%Approved,");
                if (rejectedconvcheck) sb.AppendFormat("RejectedConversions,");
                if (percentrejectcheck) sb.AppendFormat("%Rejected,");
                if (netpayoutcheck) sb.AppendFormat("Net Payout,");
                if (netrevenuecheck) sb.AppendFormat("Net Revenue,");
                if (_countrycheck) sb.AppendFormat("Country,");

                sb.AppendLine();

                foreach (var item in report)
                {

                    // offer
                    sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
                    // affiliate
                    sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);


                    if (grossconversioncheck) sb.AppendFormat("{0},", item.GrossConversions);
                    if (approvedconvcheck) sb.AppendFormat("{0},", item.ApprovedConversions);
                    if (percentapprovedcheck) sb.AppendFormat("{0}%,", item.GrossConversions == 0 ? 0 : 100 * ((double)item.ApprovedConversions / item.GrossConversions));
                    if (rejectedconvcheck) sb.AppendFormat("${0},", item.RejectedConversions);
                    if (percentrejectcheck) sb.AppendFormat("{0}%,", item.GrossConversions == 0 ? 0 : 100 * ((double)item.RejectedConversions / item.GrossConversions));
                    if (netpayoutcheck) sb.AppendFormat("${0},", item.NetPayout);
                    if (netrevenuecheck) sb.AppendFormat("${0},", item.NetRevenue);
                    if (_countrycheck) sb.AppendFormat("{0},", item.Country);




                    //// offer
                    //sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
                    //// affiliate
                    //sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);
                    //// all conversions
                    //sb.AppendFormat("{0},", item.GrossConversions);
                    //// approved conversions
                    //sb.AppendFormat("{0},", item.ApprovedConversions);
                    //// % approved 
                    //sb.AppendFormat("{0}%,", item.GrossConversions == 0 ? 0 : 100 * ((double)item.ApprovedConversions / item.GrossConversions));
                    //// RejectedConversions
                    //sb.AppendFormat("${0},", item.RejectedConversions);
                    //// % rejected
                    //sb.AppendFormat("{0}%,", item.GrossConversions == 0 ? 0 : 100 * ((double)item.RejectedConversions / item.GrossConversions));
                    //// net payout
                    //sb.AppendFormat("${0},", item.NetPayout);
                    //// net revenue
                    //sb.AppendFormat("${0},", item.NetRevenue);

                    sb.AppendLine();
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_conversion_status.csv");
            }
        }

        public ActionResult Conversion(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;


            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            CustomViewModel model = new CustomViewModel();

            try
            {
                model.CustomStatisticsEnum = (CustomStatisticsEnum)dataview;

            }
            catch
            {

                model.CustomStatisticsEnum =
                                CustomStatisticsEnum.ConversionDate |
                                CustomStatisticsEnum.CR |
                                CustomStatisticsEnum.CPC |
                                CustomStatisticsEnum.RPC |
                                CustomStatisticsEnum.Conversions |
                                CustomStatisticsEnum.Cost |
                                CustomStatisticsEnum.Revenue |
                                CustomStatisticsEnum.Profit |
                                CustomStatisticsEnum.IsSmartphone |
                                CustomStatisticsEnum.Browser
                                ;
            }



            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up)
                .Where(c => c.Status == Status.Active).AsEnumerable()
                .Select(c => new SelectListItem
                {
                    Value = c.CampaignId.ToString(),
                    Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
                });
            var subids = Request.QueryString.AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();
            ViewBag.MaxSubId = repo.ClickSubIds().Max(c => (int?)c.SubIndex) ?? 0;
            ViewBag.SubIds = subids;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {
                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;

                ViewBag.SelectedAffiliateId = af != null;

            }
            else
            {
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;
            }



            if (af != null && af.Length == 0)
                af = null;
            string affs = af == null ? null : string.Join(",", af);

            ViewBag.AffiliateCountries = repo.GetCountries();
            ViewBag.SelectedCountryId = ct;
            //ViewBag.SelectedCountry = ctfilter;

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);
            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //var offset = tzi.GetUtcOffset(DateTime.Now);

            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;



            //DateTime ufdate = TimeZoneInfo.ConvertTimeToUtc(FromDate.Value);
            //DateTime utdate = TimeZoneInfo.ConvertTimeToUtc(ToDate.Value);

            //DateTimeOffset dateAndOffset = new DateTimeOffset(fdate, offset);
            //DateTime utcfrom = dateAndOffset.UtcDateTime;
            //DateTime utcto = new DateTimeOffset(tdate, offset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;
            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                offsetHour = customoffset / 60;
                ViewBag.TimeZone = timezone;
            }
            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;




            if (state == 0)
            {

                //ViewBag.list = list;
                //return View(lcvm);
                return View("CustomReport", model);

            }
            else
            {

                string ctfilter = ct.HasValue ?
               repo.GetCountries()
               .Where(c => c.Id == ct.Value)
               .Select(c => c.CountryAbbreviation).FirstOrDefault()
               : null;
                bool _af = af == null;
                if (_af)
                {
                    af = new int[] { };
                }

                var list = from co in repo.Conversions()
                           join ck in repo.Clicks() on co.ClickId equals ck.ClickId
                           join url in repo.Urls() on new { a = ck.CustomerId, b = ck.CampaignId, c = ck.URLPreviewId } equals new { a = url.Campaign.CustomerId, b = url.Campaign.CampaignId, c = url.PreviewId }
                           join ca in repo.Campaigns() on new { a = co.CustomerId, b = co.CampaignId } equals new { a = ca.CustomerId, b = ca.CampaignId }
                           join aff in repo.Affiliates() on new { a = co.CustomerId, b = co.AffiliateId } equals new { a = aff.CustomerId, b = aff.AffiliateId }
                           join ov in repo.Override().Where(c => c.CustomerID == up.CustomerId) on co.ActionId equals ov.ActionID into overridedata
                           from ovfinal in overridedata.DefaultIfEmpty()
                           where co.CustomerId == up.CustomerId
                           && co.bot == 0
                           && co.ConversionDate >= ufdate && co.ConversionDate <= utdate
                           && (!cp.HasValue || co.CampaignId == cp)
                           && (_af || (from g in af select g).Contains(co.AffiliateId))
                           && (ctfilter == null || co.Country == ctfilter)

                           select new ConversionView
                           {
                               AffiliateId = co.AffiliateId,
                               Company = aff.Company,
                               CampaignId = co.CampaignId,
                               CampaignName = ca.CampaignName,
                               ConversionDate = co.ConversionDate, //TimeZoneInfo.ConvertTimeFromUtc(co.ConversionDate, tzi).ToString("MM/dd/yyyy"),
                               ConversionId = co.ConversionId,
                               URLId = ck.URLPreviewId,

                               PreviewUrl = url.PreviewUrl,

                               UserAgent = co.UserAgent,
                               TransactionId = co.TransactionId,
                               IPAddress = co.IPAddress,
                               Type = co.Type,//.ToString(),
                               Cost = (ovfinal.Payout != null) ? (decimal)ovfinal.Payout : co.Cost,
                               Revenue = co.Revenue,
                               Source = co.Click.Source,
                               //TODO:SUBID
                               //SubIds = co.Click.SubIds,
                               Status = co.Status,
                               StatusDescription = co.StatusDescription,
                               Postback_IPAddress = co.Postback_IPAddress,
                               Pixel = co.Pixel,
                               Postback = co.Postback,
                               ActionName = co.Action.Name,
                               Country = co.Country,
                           };



                var statuscheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Status);
                var datecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Date);
                var statusdesccheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.StatusDescription);
                var useragentcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.UserAgent);
                var ipcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.IP);
                var transactionidcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.TransactionID);
                var typecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.ConversionType);
                var subidscheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.SubId);
                var actioncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.ActionName);
                var pixelcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Pixel);
                var postbackcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Postback);
                var campaigncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Campaign);
                var affiliatecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Affiliate);
                var urlidcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.URLId);
                var urlcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.URL);
                var sourcecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Source);
                var costcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Cost);
                var revenuecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Revenue);
                var profitcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Profit);
                var _countrycheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Country);
                //var sb = new StringBuilder("Date,Status,Campaign,Affiliate,UserAgent,IPAddress,TransactionId,Cost,Revenue,Profit");
                var sb = new StringBuilder();

                if (statuscheck) sb.AppendFormat("Status,");
                if (datecheck) sb.AppendFormat("Date,");
                if (statusdesccheck) sb.AppendFormat("StatusDescription,");
                if (useragentcheck) sb.AppendFormat("UserAgent,");
                if (ipcheck) sb.AppendFormat("IP,");
                if (transactionidcheck) sb.AppendFormat("TransactionID,");
                if (typecheck) sb.AppendFormat("ConversionType,");
                if (subidscheck) sb.AppendFormat("SubId,");
                if (actioncheck) sb.AppendFormat("ActionName,");
                if (pixelcheck) sb.AppendFormat("Pixel,");
                if (postbackcheck) sb.AppendFormat("Postback,");
                if (campaigncheck) sb.AppendFormat("Campaign,");
                if (affiliatecheck) sb.AppendFormat("Affiliate,");
                if (urlidcheck) sb.AppendFormat("URLId,");
                if (urlcheck) sb.AppendFormat("URL,");
                if (sourcecheck) sb.AppendFormat("Source,");
                if (costcheck) sb.AppendFormat("Cost,");
                if (revenuecheck) sb.AppendFormat("Revenue,");
                if (profitcheck) sb.AppendFormat("Profit,");
                if (_countrycheck) sb.AppendFormat("Country,");



                sb.AppendLine();

                foreach (var item in list)
                {


                    if (statuscheck) sb.AppendFormat("{0},", item.Status);
                    if (datecheck) sb.AppendFormat("{0},", item.ConversionDate);
                    if (statusdesccheck) sb.AppendFormat("{0},", item.StatusDescription);
                    if (useragentcheck) sb.AppendFormat("{0},", item.UserAgent);
                    if (ipcheck) sb.AppendFormat("{0},", item.IPAddress);
                    if (transactionidcheck) sb.AppendFormat("{0},", item.TransactionId);
                    if (typecheck) sb.AppendFormat("{0},", item.Type);
                    if (subidscheck) sb.AppendFormat("{0},", item.SubIds);
                    if (actioncheck) sb.AppendFormat("{0},", item.ActionName);
                    if (pixelcheck) sb.AppendFormat("{0},", item.Pixel);
                    if (postbackcheck) sb.AppendFormat("{0},", item.Postback);
                    if (campaigncheck) sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
                    if (affiliatecheck) sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);
                    if (urlidcheck) sb.AppendFormat("{0},", item.URLId);

                    if (urlcheck) sb.AppendFormat("{0},", item.PreviewUrl);

                    if (sourcecheck) sb.AppendFormat("{0},", item.Source);
                    if (costcheck) sb.AppendFormat("${0},", item.Cost.ToString("F2"));
                    if (revenuecheck) sb.AppendFormat("${0},", item.Revenue.ToString("F2"));
                    if (profitcheck) sb.AppendFormat("${0},", (item.Revenue - item.Cost).ToString("F2"));
                    if (_countrycheck) sb.AppendFormat("{0},", item.Country);









                    //// date
                    //sb.AppendFormat("{0},", item.ConversionDate);
                    //// Status
                    //sb.AppendFormat("{0},", item.Status);
                    //// CampaignName
                    //sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
                    //// company
                    //sb.AppendFormat("{0},", item.Company);
                    ////UserAgent
                    //sb.AppendFormat("{0},", item.UserAgent);
                    ////ip
                    //sb.AppendFormat("{0},", item.IPAddress);
                    //// TransactionId
                    //sb.AppendFormat("{0},", item.TransactionId);
                    //// cost
                    //sb.AppendFormat("${0},", item.Cost.ToString("F2"));
                    //// revenue
                    //sb.AppendFormat("${0},", item.Revenue.ToString("F2"));
                    //// profit
                    //sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));

                    sb.AppendLine();
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_conversions.csv");
            }
        }

        public ActionResult Campaign(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
           long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0,
           bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);
            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;


            CustomViewModel model = new CustomViewModel();

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);


            try
            {
                model.CustomStatisticsEnum = (CustomStatisticsEnum)dataview;
            }
            catch
            {
                model.CustomStatisticsEnum =

                                CustomStatisticsEnum.CR |
                                CustomStatisticsEnum.CPC |
                                CustomStatisticsEnum.RPC |
                                CustomStatisticsEnum.Impressions |
                                CustomStatisticsEnum.Clicks |
                                CustomStatisticsEnum.Conversions |
                                CustomStatisticsEnum.Cost |
                                CustomStatisticsEnum.Revenue |
                                CustomStatisticsEnum.Profit |
                                CustomStatisticsEnum.Campaign
                                ;
            }


            //lavm.OfferViewList = new List<OfferView>();
            //lavm.Filter = new Models.Filter();
            //lavm.Stadisctics = Statistics.Create(statistics);
            //lavm.Calculation = Calculation.Create(calculation);

            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;

            bool affiliatecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Affiliate);
            bool urlcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.URLId);
            bool sourcechk = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Source);
            //bool subidcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.SubId);
            bool impressioncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Impressions);

            bool clickcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Clicks);
            bool conversioncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Conversions);

            bool costcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Cost);
            bool revenuecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Revenue);
            bool profitcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Profit);

            bool countrycheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Country);
            bool CRcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CR);

            bool CPCcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CPC);
            bool RPCvcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.RPC);

            //ViewBag.URLCheck = urlcheck;


            //ViewBag.SourceCheck = sourcechk;

            // get the subids
            var subids = Request.QueryString.AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();
            ViewBag.MaxSubId = repo.ClickSubIds().Max(c => (int?)c.SubIndex) ?? 0;
            ViewBag.SubIds = subids;

            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up).Where(c => c.Status == Status.Active).AsEnumerable().Select(c => new SelectListItem
            {
                Value = c.CampaignId.ToString(),
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
            });

            //string affs = null;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {

                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);


                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;

                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;



                //affs = up.AffiliateId.ToString();
                // af = new int[] { up.AffiliateId.Value };
            }

            //var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //var offset = tzi.GetUtcOffset(DateTime.Now);
            //DateTime ufdate = new DateTimeOffset(fdate, offset).UtcDateTime;
            //DateTime utdate = new DateTimeOffset(tdate, offset).UtcDateTime;

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

            ViewBag.TimeZone = tzi.Id;
            int customoffset = repo.Getcustomoffset(up.UserId, timezone);

            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;

            if (customoffset != 0)
            {
                offsetHour = customoffset / 60;
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                ViewBag.TimeZone = timezone;
            }


            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            if (state == 0)
            {
                //lavm.OfferViewList = list;
                //ViewBag.list = list;
                //return View(lavm);

                return View("CustomReport", model);
            }
            else
            {
                #region Dynamic Statistics

                bool datecheck = false;
                bool hourcheck = false;
                bool campaigncheck = true; //string.IsNullOrEmpty(offview);
                //  bool affiliatecheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Affiliate);
                //  bool countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);
                //  bool urlcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.URLId);

                bool statuscheck = false;
                bool pixelcheck = false;
                bool postbackcheck = false;
                bool ipcheck = false;
                bool transactionidcheck = false;
                bool conversiontypecheck = false;
                bool referrercheck = false;
                bool statusdesccheck = false;
                //  bool sourcechk = model.StatisticsEnum.HasFlag(StatisticsEnum.Source); //!string.IsNullOrEmpty(sourceview);
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
                string Vdeviceid = null; string Vdeviceos = null; string Vbrowser = null; string Vos = null; string Vmodelname = null; string Vbrandname = null; string Vmarketingname = null; string Vresolution = null; string UserAgent = null;
                //Filtering Device Infoes//


                // get the subids
                //var debug = Request.RequestUri.ParseQueryString();
                //var subids = Request.RequestUri.ParseQueryString().AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();

                string ctfilter = ct.HasValue ?
                                       repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                                       : null;

                #endregion

                #region Dynamic Type
                var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
                //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


                if (hourcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof(int));
                }
                if (datecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof(string));
                }

                if (campaigncheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof(string));
                }
                if (affiliatecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                }

                if (urlcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof(string));
                }
                if (countrycheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof(string));
                }
                if (statuscheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof(int));
                }
                if (pixelcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof(DateTime?));
                }
                if (postbackcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof(DateTime?));
                }
                if (ipcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof(string));
                }
                if (transactionidcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof(string));
                }
                if (conversiontypecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof(ConversionType));
                }
                if (referrercheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof(string));
                }
                if (statusdesccheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof(string));
                }
                if (sourcechk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof(string));
                }
                if (actionchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof(string));
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof(int));

                }
                if (bannerchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof(string));
                }
                if (parenturlchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                }
                if (ctrchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                }


                ///////////////-----------Device Info-----------/////////////

                if (deviceid) { DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof(string)); }
                if (issmartphone) { DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof(string)); }
                if (isios) { DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof(string)); }
                if (isandroid) { DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof(string)); }
                if (os) { DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof(string)); }
                if (browser) { DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof(string)); }
                if (device_os) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof(string)); }
                if (pointing_method) { DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof(string)); }
                if (is_tablet) { DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof(string)); }
                if (model_name) { DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof(string)); }
                if (device_os_version) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof(string)); }
                if (is_wireless_device) { DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof(string)); }
                if (brand_name) { DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof(string)); }
                if (marketing_name) { DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof(string)); }
                if (resolution_height) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof(string)); }
                if (resolution_width) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof(string)); }
                if (canvas_support) { DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof(string)); }
                if (viewport_width) { DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof(string)); }
                if (isviewport_supported) { DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof(string)); }
                if (ismobileoptimized) { DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof(string)); }
                if (ishandheldfriendly) { DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof(string)); }
                if (is_smarttv) { DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof(string)); }
                if (isux_full_desktop) { DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof(string)); }



                ///////////////-----------Device Info-----------/////////////



                DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof(decimal));
                DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof(decimal));
                foreach (var item in subids)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof(string));
                }
                Type resultType = affview.CreateType();

                #endregion

                var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, utdate, offsetHour, datecheck, hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck, ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk, bannerchk, parenturlchk, ctrchk,
                  deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name, device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width, canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv, isux_full_desktop,
                subids.Length > 0 ? subids : null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname, Vmarketingname, Vresolution, UserAgent
                );

                dynamic list = repo.ExecuteQuery(resultType, query);

                //var query = CKQueryBuilder.CampaignReportQuery(up.CustomerId, up.UserId, Offsethour, affiliatecheck, sourcechk

                //    , subids.Length > 0 ? subids : null
                //    , urlcheck, countrycheck
                //    , cp, af, ctfilter);


                //dynamic list = repo.ExecuteQuery(resultType, query
                //   , new SqlParameter("fromdate", ufdate)
                //   , new SqlParameter("todate", utdate)
                //   );

                var subidcheck = subids.Length > 0 ? true : false;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("CampaignName,");
                if (affiliatecheck) sb.AppendFormat("Affiliate,");
                if (urlcheck) sb.AppendFormat("URLId,");
                if (sourcechk) sb.AppendFormat("Source,");
                if (countrycheck) sb.AppendFormat("Country,");
                if (subidcheck)
                {
                    for (int i = 0; i < subids.Length; i++)
                    {
                        sb.AppendFormat("SubId" + subids[i] + ",");
                    }
                }
                if (impressioncheck) sb.AppendFormat("Impressions,");

                if (clickcheck) sb.AppendFormat("Clicks,");
                if (conversioncheck) sb.AppendFormat("Conversions,");
                if (CRcheck) sb.AppendFormat("Conv,");
                if (costcheck) sb.AppendFormat("Cost,");
                if (CPCcheck) sb.AppendFormat("CPC,");
                if (revenuecheck) sb.AppendFormat("Revenue,");
                if (RPCvcheck) sb.AppendFormat("RPC,");
                if (profitcheck) sb.AppendFormat("Profit,");


                //if (!affiliatecheck)
                //{
                // sb = new StringBuilder("CampaignName,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                sb.AppendLine();

                foreach (var item in list)
                {
                    sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Affiliate))
                        sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.URLId))
                        sb.AppendFormat("{0},", item.URLPreviewId);
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Source))
                        sb.AppendFormat("{0},", item.Source);
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Country))
                        sb.AppendFormat("{0},", item.Country);
                    if (subidcheck)
                    {
                        for (int a = 0; a < subids.Length; a++)
                        {
                            string sub = "SubId" + subids[a];
                            sb.AppendFormat("{0},", item.GetType().GetProperty(sub).GetValue(item, null));
                            //sb.AppendFormat("{0},", item.SubId + subids[a]); 
                        }
                    }
                    // impressions
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Impressions))
                        sb.AppendFormat("{0},", item.Impressions);
                    //clicks
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Clicks))
                        sb.AppendFormat("{0},", item.Clicks);
                    //conversions
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Conversions))
                        sb.AppendFormat("{0},", item.Conversions);
                    // conv rate
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CR))
                        sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));
                    // cost
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Cost))
                        sb.AppendFormat("${0},", item.Cost.ToString("F2"));
                    // cpc
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CPC))
                        sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));
                    // revenue
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Revenue))
                        sb.AppendFormat("${0},", item.Revenue.ToString("F2"));
                    // rpc
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.RPC))
                        sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));
                    // profit
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Profit))
                        sb.AppendFormat("${0},", (item.Revenue - item.Cost).ToString("F2"));
                    sb.AppendLine();
                }


                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_offer.csv");
            }

        }

        public ActionResult Daily(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {
            //DateTime current = DateTime.Now;
            //var indtimezone = repo.FindTimeZoneInfo("India Standard Time", "");
            //var indoffset = indtimezone.GetUtcOffset(DateTime.Now);
            //DateTime ind = new DateTimeOffset(current.Ticks, indtimezone.GetUtcOffset(DateTime.Now)).UtcDateTime;

            //var esttimezone = repo.FindTimeZoneInfo("Eastern Standard Time", "");
            //var estoffset = esttimezone.GetUtcOffset(DateTime.Now);
            //DateTime est = new DateTimeOffset(current.Ticks, esttimezone.GetUtcOffset(DateTime.Now)).UtcDateTime;


            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;


            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            CustomViewModel model = new CustomViewModel();

            try
            {
                model.CustomStatisticsEnum = (CustomStatisticsEnum)dataview;

            }
            catch
            {

                model.CustomStatisticsEnum =
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



            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up)
                .Where(c => c.Status == Status.Active).AsEnumerable()
                .Select(c => new SelectListItem
                {
                    Value = c.CampaignId.ToString(),
                    Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
                });
            var subids = Request.QueryString.AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();
            ViewBag.MaxSubId = repo.ClickSubIds().Max(c => (int?)c.SubIndex) ?? 0;
            ViewBag.SubIds = subids;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {
                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;

                ViewBag.SelectedAffiliateId = af != null;

            }
            else
            {
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;
            }



            if (af != null && af.Length == 0)
                af = null;
            string affs = af == null ? null : string.Join(",", af);

            ViewBag.AffiliateCountries = repo.GetCountries();
            ViewBag.SelectedCountryId = ct;
            //ViewBag.SelectedCountry = ctfilter;

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);
            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //var offset = tzi.GetUtcOffset(DateTime.Now);

            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;



            //DateTime ufdate = TimeZoneInfo.ConvertTimeToUtc(FromDate.Value);
            //DateTime utdate = TimeZoneInfo.ConvertTimeToUtc(ToDate.Value);

            //DateTimeOffset dateAndOffset = new DateTimeOffset(fdate, offset);
            //DateTime utcfrom = dateAndOffset.UtcDateTime;
            //DateTime utcto = new DateTimeOffset(tdate, offset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;
            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                offsetHour = customoffset / 60;
                ViewBag.TimeZone = timezone;
            }
            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            /*============================================================================================================*/



            //var list = dailyreport;
            //var list = repo.DailyReport(fdate, tdate, up.CustomerId, offset.Hours, up.UserId, affs, cp, ct);

            if (state == 0)
            {
                //int i = 0;
                //List<DailyView> l = new List<DailyView>();


                //ldvm.DailyViewList = l;
                //ldvm.DailyViewList = report;

                return View("CustomReport", model);
            }

            else
            {





                var countrycheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Country);
                var datecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Date);
                //var yearcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Year);
                //var monthcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Month);
                var daycheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Day);
                var impressioncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Impressions);
                var clickcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Clicks);
                var conversioncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Conversions);
                var costcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Cost);
                var revenuecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Revenue);
                var profitcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Profit);
                // var _countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);
                var crcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CR);
                var cpccheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CPC);
                var rpccheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.RPC);

                #region Dynamic Statistics

                // bool datecheck = false;
                bool hourcheck = false;
                bool campaigncheck = false; //string.IsNullOrEmpty(offview);
                bool affiliatecheck = false;
                bool urlcheck = false;

                // bool countrycheck = false;
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
                string Vdeviceid = null; string Vdeviceos = null; string Vbrowser = null; string Vos = null; string Vmodelname = null; string Vbrandname = null; string Vmarketingname = null; string Vresolution = null; string UserAgent = null;
                //Filtering Device Infoes//


                // get the subids
                //var debug = Request.RequestUri.ParseQueryString();
                //var subids = Request.RequestUri.ParseQueryString().AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();

                string ctfilter = ct.HasValue ?
                                       repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                                       : null;

                #endregion

                #region Dynamic Type
                var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
                //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


                if (hourcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof(int));
                }
                if (datecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof(string));
                }

                if (campaigncheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof(string));
                }
                if (affiliatecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                }

                if (urlcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof(string));
                }
                if (countrycheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof(string));
                }
                if (statuscheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof(int));
                }
                if (pixelcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof(DateTime?));
                }
                if (postbackcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof(DateTime?));
                }
                if (ipcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof(string));
                }
                if (transactionidcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof(string));
                }
                if (conversiontypecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof(ConversionType));
                }
                if (referrercheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof(string));
                }
                if (statusdesccheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof(string));
                }
                if (sourcechk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof(string));
                }
                if (actionchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof(string));
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof(int));

                }
                if (bannerchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof(string));
                }
                if (parenturlchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                }
                if (ctrchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                }


                ///////////////-----------Device Info-----------/////////////

                if (deviceid) { DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof(string)); }
                if (issmartphone) { DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof(string)); }
                if (isios) { DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof(string)); }
                if (isandroid) { DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof(string)); }
                if (os) { DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof(string)); }
                if (browser) { DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof(string)); }
                if (device_os) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof(string)); }
                if (pointing_method) { DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof(string)); }
                if (is_tablet) { DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof(string)); }
                if (model_name) { DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof(string)); }
                if (device_os_version) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof(string)); }
                if (is_wireless_device) { DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof(string)); }
                if (brand_name) { DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof(string)); }
                if (marketing_name) { DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof(string)); }
                if (resolution_height) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof(string)); }
                if (resolution_width) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof(string)); }
                if (canvas_support) { DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof(string)); }
                if (viewport_width) { DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof(string)); }
                if (isviewport_supported) { DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof(string)); }
                if (ismobileoptimized) { DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof(string)); }
                if (ishandheldfriendly) { DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof(string)); }
                if (is_smarttv) { DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof(string)); }
                if (isux_full_desktop) { DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof(string)); }



                ///////////////-----------Device Info-----------/////////////



                DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof(decimal));
                DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof(decimal));
                //foreach (var item in subids)
                //{
                //    DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof(string));
                //}
                Type resultType = affview.CreateType();

                #endregion





                //var report = repo.RunQuery<DailyView>("EXEC [DailyRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                //                up.CustomerId, ufdate, utdate, tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, affs, cp, ctfilter, countrycheck);

                var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, utdate, offsetHour, datecheck, hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck, ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk, bannerchk, parenturlchk, ctrchk,
                 deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name, device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width, canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv, isux_full_desktop,
                null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname, Vmarketingname, Vresolution, UserAgent
               );

                dynamic report = repo.ExecuteQuery(resultType, query);

                //var sb = new StringBuilder("Date,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                var sb = new StringBuilder();
                if (datecheck) sb.AppendFormat("Date,");
                //if (yearcheck) sb.AppendFormat("Year,");
                //   if (monthcheck) sb.AppendFormat("Month,");
                if (daycheck) sb.AppendFormat("Day,");
                if (impressioncheck) sb.AppendFormat("Impressions,");
                if (clickcheck) sb.AppendFormat("Clicks,");
                if (conversioncheck) sb.AppendFormat("Conversions,");
                if (crcheck) sb.AppendFormat("Conv Rate,");
                if (costcheck) sb.AppendFormat("Cost,");
                if (cpccheck) sb.AppendFormat("CPC,");
                if (revenuecheck) sb.AppendFormat("Revenue,");
                if (rpccheck) sb.AppendFormat("RPC,");
                if (profitcheck) sb.AppendFormat("Profit,");
                if (countrycheck) sb.AppendFormat("Country,");


                sb.AppendLine();
                foreach (var item in report)
                {
                    if (datecheck) sb.AppendFormat("{0},", item.Date);
                    //   if (yearcheck) sb.AppendFormat("{0},", Convert.ToDateTime(item.Date).Year);
                    // if (monthcheck) sb.AppendFormat("{0},", Convert.ToDateTime(item.Date).Month);
                    if (daycheck) sb.AppendFormat("{0},", Convert.ToDateTime(item.Date).Day);
                    if (impressioncheck) sb.AppendFormat("{0},", item.Impressions);
                    if (clickcheck) sb.AppendFormat("{0},", item.Clicks);
                    if (conversioncheck) sb.AppendFormat("{0},", item.Conversions);
                    if (crcheck) sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));
                    if (costcheck) sb.AppendFormat("${0},", item.Cost.ToString("F2"));
                    if (cpccheck) sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));
                    if (revenuecheck) sb.AppendFormat("${0},", item.Revenue.ToString("F2"));
                    if (rpccheck) sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));
                    if (profitcheck) sb.AppendFormat("${0},", (item.Revenue - item.Cost).ToString("F2"));
                    if (countrycheck) sb.AppendFormat("{0},", item.Country);


                    sb.AppendLine();



                    sb.AppendFormat("{0},", item.Date);
                    // impressions
                    sb.AppendFormat("{0},", item.Impressions);
                    //clicks
                    sb.AppendFormat("{0},", item.Clicks);
                    //conversions
                    sb.AppendFormat("{0},", item.Conversions);
                    // conv rate
                    sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));
                    // cost
                    sb.AppendFormat("${0},", item.Cost.ToString("F2"));
                    // cpc
                    sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));
                    // revenue
                    sb.AppendFormat("${0},", item.Revenue.ToString("F2"));
                    // rpc
                    sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));
                    // profit
                    sb.AppendFormat("${0},", (item.Revenue - item.Cost).ToString("F2"));
                    sb.AppendLine();
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_daily.csv");
            }

        }

        public ActionResult AdCampaign(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;

            var model = new AdCampaignViewModel();

            try
            {
                model.StatisticsEnum = (StatisticsEnum)dataview;

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

            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up).Where(c => c.Status == Status.Active).AsEnumerable().Select(c => new SelectListItem
            {
                Value = c.CampaignId.ToString(),
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
            });

            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {

                if (af != null)
                {
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);



                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;



            }

            string affs = af == null || af.Length == 0 ? null : string.Join(",", af);

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            var offset = tzi.GetUtcOffset(DateTime.Now);
            DateTime ufdate = new DateTimeOffset(fdate, offset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(tdate, offset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;
            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                ViewBag.TimeZone = timezone;
            }


            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            /*============================================================================================================*/

            if (state == 0)
            {
                return View(model);
            }
            else
            {

                string ctfilter = ct.HasValue ?
              repo.GetCountries()
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




                var impressioncheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Impressions);
                var clickcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Clicks);
                var conversioncheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Conversions);
                var costcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Cost);
                var revenuecheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Revenue);
                var profitcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Profit);
                var _countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);
                var crcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.CR);
                var cpccheck = model.StatisticsEnum.HasFlag(StatisticsEnum.CPC);
                var rpccheck = model.StatisticsEnum.HasFlag(StatisticsEnum.RPC);


                // var sb = new StringBuilder("Name,Campaign,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");


                var sb = new StringBuilder();

                sb.AppendFormat("Name,");
                sb.AppendFormat("Campaign,");
                if (impressioncheck) sb.AppendFormat("Impressions,");
                if (clickcheck) sb.AppendFormat("Clicks,");
                if (conversioncheck) sb.AppendFormat("Conversions,");
                if (crcheck) sb.AppendFormat("Conv Rate,");
                if (costcheck) sb.AppendFormat("Cost,");
                if (cpccheck) sb.AppendFormat("CPC,");
                if (revenuecheck) sb.AppendFormat("Revenue,");
                if (rpccheck) sb.AppendFormat("RPC,");
                if (profitcheck) sb.AppendFormat("Profit,");
                if (_countrycheck) sb.AppendFormat("Country,");


                sb.AppendLine();

                foreach (var item in report)
                {
                    sb.AppendFormat("{0} - {1},", item.BannerId, item.Name);
                    // CampaignName
                    sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
                    // impressions
                    if (impressioncheck) sb.AppendFormat("{0},", item.Impressions);
                    //clicks
                    if (clickcheck) sb.AppendFormat("{0},", item.Clicks);
                    //conversions
                    if (conversioncheck) sb.AppendFormat("{0},", item.Conversions);
                    // conv rate
                    if (crcheck) sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));
                    // cost
                    if (costcheck) sb.AppendFormat("${0},", item.Cost.ToString("F2"));
                    // cpc
                    if (cpccheck) sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));
                    // revenue
                    if (revenuecheck) sb.AppendFormat("${0},", item.Revenue.ToString("F2"));
                    // rpc
                    if (rpccheck) sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));
                    // profit
                    if (profitcheck) sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));
                    //Country
                    if (_countrycheck) sb.AppendFormat("{0},", item.Country);
                    sb.AppendLine();
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_adcampaign.csv");
            }
        }

        public ActionResult Hourly(DateTime? FromDate, string timezone = "",
            long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;

            // set the fromdate using the passed timezone or the customertimezone or the localtimezone
            DateTime fromdate = FromDate ?? DateTime.Today;

            // find user timezone
            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            fromdate = new DateTime(fromdate.Ticks, DateTimeKind.Unspecified);
            // get the offset
            var offset = tzi.GetUtcOffset(DateTime.Now);
            int offsetHour = (int)tzi.GetUtcOffset(DateTime.Now).TotalMinutes;
            // set the offset to the date and return the utc time
            var ufdate = new DateTimeOffset(fromdate, offset).UtcDateTime;

            ViewBag.fdate = fromdate;
            ViewBag.FromDate = fromdate.ToString("MM/dd/yyyy");
            ViewBag.TimeZone = tzi.Id;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            if (customoffset != 0)
            {
                ufdate = fromdate.AddMinutes(customoffset);
                offsetHour = customoffset;
                // utdate = ToDate.Value.AddMinutes(customoffset);
                ViewBag.TimeZone = timezone;
            }

            CustomViewModel model = new CustomViewModel();

            try
            {
                model.CustomStatisticsEnum = (CustomStatisticsEnum)dataview;
            }
            catch
            {
                model.CustomStatisticsEnum =
                                CustomStatisticsEnum.Hour |
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
            var countrycheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Country);

            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up).Where(c => c.Status == Status.Active).AsEnumerable().Select(c => new SelectListItem
            {
                Value = c.CampaignId.ToString(),
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
            });
            var subids = Request.QueryString.AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();
            ViewBag.MaxSubId = repo.ClickSubIds().Max(c => (int?)c.SubIndex) ?? 0;
            ViewBag.SubIds = subids;
            //string affs = null;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {

                if (af != null)
                {
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                    // set the parameter for the query
                    //var sb = new StringBuilder();
                    //foreach (var item in af)
                    //{
                    //    sb.AppendFormat("{0},", item);
                    //}
                    //affs = sb.ToString().TrimEnd(',');
                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;




                //af = up.AffiliateId; // set the af to this af
                //affs = up.AffiliateId.ToString();

                //af = new int[] { up.AffiliateId.Value };
            }

            string affs = af == null || af.Length == 0 ? null : string.Join(",", af);


            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            /* ====================================================================================================== */




            if (state == 0)
            {
                //foreach (var item in list)
                //{
                //    item.Year = fromdate.Year;
                //    item.Day = fromdate.Day;
                //    item.Month = fromdate.Month;
                //    item.Date = fromdate.ToString("MM/dd/yyyy");
                //    lhvm.HourlyViewList.Add(item);
                //}

                //lhvm.HourlyViewList = list;


                return View("CustomReport", model);
            }
            else
            {

                #region Dynamic Statistics

                bool datecheck = false;
                bool hourcheck = true;
                bool campaigncheck = false; //string.IsNullOrEmpty(offview);
                bool affiliatecheck = false;
                bool urlcheck = false;

                // bool countrycheck = false;
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
                string Vdeviceid = null; string Vdeviceos = null; string Vbrowser = null; string Vos = null; string Vmodelname = null; string Vbrandname = null; string Vmarketingname = null; string Vresolution = null; string UserAgent = null;
                //Filtering Device Infoes//


                // get the subids
                //var debug = Request.RequestUri.ParseQueryString();
                //var subids = Request.RequestUri.ParseQueryString().AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();

                string ctfilter = ct.HasValue ?
                                       repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                                       : null;

                #endregion

                #region Dynamic Type
                var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
                //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


                if (hourcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof(int));
                }
                if (datecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof(string));
                }

                if (campaigncheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof(string));
                }
                if (affiliatecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                }

                if (urlcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof(string));
                }
                if (countrycheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof(string));
                }
                if (statuscheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof(int));
                }
                if (pixelcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof(DateTime?));
                }
                if (postbackcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof(DateTime?));
                }
                if (ipcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof(string));
                }
                if (transactionidcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof(string));
                }
                if (conversiontypecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof(ConversionType));
                }
                if (referrercheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof(string));
                }
                if (statusdesccheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof(string));
                }
                if (sourcechk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof(string));
                }
                if (actionchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof(string));
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof(int));

                }
                if (bannerchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof(string));
                }
                if (parenturlchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                }
                if (ctrchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                }


                ///////////////-----------Device Info-----------/////////////

                if (deviceid) { DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof(string)); }
                if (issmartphone) { DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof(string)); }
                if (isios) { DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof(string)); }
                if (isandroid) { DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof(string)); }
                if (os) { DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof(string)); }
                if (browser) { DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof(string)); }
                if (device_os) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof(string)); }
                if (pointing_method) { DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof(string)); }
                if (is_tablet) { DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof(string)); }
                if (model_name) { DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof(string)); }
                if (device_os_version) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof(string)); }
                if (is_wireless_device) { DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof(string)); }
                if (brand_name) { DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof(string)); }
                if (marketing_name) { DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof(string)); }
                if (resolution_height) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof(string)); }
                if (resolution_width) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof(string)); }
                if (canvas_support) { DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof(string)); }
                if (viewport_width) { DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof(string)); }
                if (isviewport_supported) { DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof(string)); }
                if (ismobileoptimized) { DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof(string)); }
                if (ishandheldfriendly) { DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof(string)); }
                if (is_smarttv) { DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof(string)); }
                if (isux_full_desktop) { DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof(string)); }



                ///////////////-----------Device Info-----------/////////////



                DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof(decimal));
                DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof(decimal));
                //foreach (var item in subids)
                //{
                //    DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof(string));
                //}
                Type resultType = affview.CreateType();

                #endregion



                //var list = repo.RunQuery<HourlyView>("EXEC [HourlyRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                //                up.CustomerId, ufdate, ufdate.AddDays(1).AddMilliseconds(-1), tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, affs, cp, ctfilter, countrycheck);
                var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, ufdate.AddDays(1).AddMilliseconds(-1), offsetHour, datecheck, hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck, ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk, bannerchk, parenturlchk, ctrchk,
                 deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name, device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width, canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv, isux_full_desktop,
                null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname, Vmarketingname, Vresolution, UserAgent
               );

                dynamic list = repo.ExecuteQuery(resultType, query);

                //var sb = new StringBuilder("Date,Hour,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                var sb = new StringBuilder();

                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Date) ? ",Date" : string.Empty);
                //   sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Year) ? ",Year" : string.Empty);
                //sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Month) ? ",Month" : string.Empty);
                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Day) ? ",Day" : string.Empty);
                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Hour) ? ",Hour" : string.Empty);
                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Impressions) ? ",Impressions" : string.Empty);
                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Clicks) ? ",Clicks" : string.Empty);
                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Conversions) ? ",Conversions" : string.Empty);

                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CR) ? ",CR" : string.Empty);
                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Cost) ? ",Cost" : string.Empty);

                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CPC) ? ",CPC" : string.Empty);
                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Revenue) ? ",Revenue" : string.Empty);
                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.RPC) ? ",RPC" : string.Empty);
                sb.Append(model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Profit) ? ",Profit" : string.Empty);

                if (sb.Length > 0)
                    sb.Remove(0, 1);
                sb.AppendLine();

                foreach (var item in list)
                {
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Date))
                        sb.AppendFormat("{0},", fromdate.ToString("MM/dd/yyyy"));

                    //if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Year))
                    //    sb.AppendFormat("{0},", fromdate.Year);

                    //if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Month))
                    //    sb.AppendFormat("{0},", fromdate.Month);

                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Day))
                        sb.AppendFormat("{0},", fromdate.Day);

                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Hour))
                        sb.AppendFormat("{0},", item.Hour);

                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Impressions))
                        sb.AppendFormat("{0},", item.Impressions);

                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Clicks))
                        sb.AppendFormat("{0},", item.Clicks);

                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Conversions))
                        sb.AppendFormat("{0},", item.Conversions);

                    // conv rate
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CR))
                        sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));

                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Cost))
                        sb.AppendFormat("${0},", item.Cost.ToString("F2"));

                    // cpc
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CPC))
                        sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));

                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Revenue))
                        sb.AppendFormat("${0},", item.Revenue.ToString("F2"));

                    // rpc
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.RPC))
                        sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));

                    // profit
                    if (model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Profit))
                        sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));


                    sb.AppendLine();
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_hourly_" + fromdate.ToString("yyyyMMdd") + ".csv");
            }
        }

        public ActionResult ClicksLogs(string id, DateTime? id2)
        {
            DateTime date = id2.HasValue ? id2.Value : DateTime.Today;
            ViewBag.TimeZone = id ?? repo.GetCurrentCustomer().TimeZone;
            return View(date);
        }
        public ActionResult ClicksDetailsLog(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {

            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;


            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            CustomViewModel model = new CustomViewModel();

            try
            {
                model.CustomStatisticsEnum = (CustomStatisticsEnum)dataview;

            }
            catch
            {

                model.CustomStatisticsEnum =
                                CustomStatisticsEnum.ClickDate |
                                CustomStatisticsEnum.CR |
                                CustomStatisticsEnum.CPC |
                                CustomStatisticsEnum.RPC |
                                CustomStatisticsEnum.Clicks |
                                CustomStatisticsEnum.Cost |
                                CustomStatisticsEnum.Revenue |
                                CustomStatisticsEnum.Profit |
                                CustomStatisticsEnum.IsSmartphone |
                                CustomStatisticsEnum.Browser

                                ;
            }



            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up)
                .Where(c => c.Status == Status.Active).AsEnumerable()
                .Select(c => new SelectListItem
                {
                    Value = c.CampaignId.ToString(),
                    Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
                });
            var subids = Request.QueryString.AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();
            ViewBag.MaxSubId = repo.ClickSubIds().Max(c => (int?)c.SubIndex) ?? 0;
            ViewBag.SubIds = subids;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {
                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;

                ViewBag.SelectedAffiliateId = af != null;

            }
            else
            {
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;
            }



            if (af != null && af.Length == 0)
                af = null;
            string affs = af == null ? null : string.Join(",", af);

            ViewBag.AffiliateCountries = repo.GetCountries();
            ViewBag.SelectedCountryId = ct;
            //ViewBag.SelectedCountry = ctfilter;

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);
            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //var offset = tzi.GetUtcOffset(DateTime.Now);

            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;



            //DateTime ufdate = TimeZoneInfo.ConvertTimeToUtc(FromDate.Value);
            //DateTime utdate = TimeZoneInfo.ConvertTimeToUtc(ToDate.Value);

            //DateTimeOffset dateAndOffset = new DateTimeOffset(fdate, offset);
            //DateTime utcfrom = dateAndOffset.UtcDateTime;
            //DateTime utcto = new DateTimeOffset(tdate, offset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;
            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                offsetHour = customoffset / 60;
                ViewBag.TimeZone = timezone;
            }
            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            /*============================================================================================================*/



            //var list = dailyreport;
            //var list = repo.DailyReport(fdate, tdate, up.CustomerId, offset.Hours, up.UserId, affs, cp, ct);


            return View("CustomReport", model);
        }

        public ActionResult Traffic(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);
            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;


            var model = new TrafficViewModel();
            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            try
            {
                model.StatisticsEnum = (StatisticsEnum)dataview;

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



            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up).Where(c => c.Status == Status.Active).AsEnumerable().Select(c => new SelectListItem
            {
                Value = c.CampaignId.ToString(),
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
            });

            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {
                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;



            }

            string affs = af == null || af.Length == 0 ? null : string.Join(",", af);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime ufdate = TimeZoneInfo.ConvertTimeToUtc(fdate, tzi);
            //DateTime utdate = TimeZoneInfo.ConvertTimeToUtc(tdate, tzi);
            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                offsetHour = customoffset / 60;
                ViewBag.TimeZone = timezone;
            }

            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            /*=================================================================================================*/

            if (state == 0)
            {
                return View(model);
            }

            else
            {

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
                string Vdeviceid = null; string Vdeviceos = null; string Vbrowser = null; string Vos = null; string Vmodelname = null; string Vbrandname = null; string Vmarketingname = null; string Vresolution = null; string UserAgent = null;
                //Filtering Device Infoes//


                //// get the subids
                //var debug = Request.RequestUri.ParseQueryString();
                //var subids = Request.RequestUri.ParseQueryString().AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();

                string ctfilter = ct.HasValue ?
                                       repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                                       : null;

                #endregion

                #region Dynamic Type
                var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
                //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


                if (hourcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof(int));
                }
                if (datecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof(string));
                }

                if (campaigncheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof(string));
                }
                if (affiliatecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                }

                if (urlcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof(string));
                }
                if (countrycheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof(string));
                }
                if (statuscheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof(int));
                }
                if (pixelcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof(DateTime?));
                }
                if (postbackcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof(DateTime?));
                }
                if (ipcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof(string));
                }
                if (transactionidcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof(string));
                }
                if (conversiontypecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof(ConversionType));
                }
                if (referrercheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof(string));
                }
                if (statusdesccheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof(string));
                }
                if (sourcechk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof(string));
                }
                if (actionchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof(string));
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof(int));

                }
                if (bannerchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof(string));
                }
                if (parenturlchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                }
                if (ctrchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                }


                ///////////////-----------Device Info-----------/////////////

                if (deviceid) { DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof(string)); }
                if (issmartphone) { DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof(string)); }
                if (isios) { DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof(string)); }
                if (isandroid) { DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof(string)); }
                if (os) { DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof(string)); }
                if (browser) { DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof(string)); }
                if (device_os) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof(string)); }
                if (pointing_method) { DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof(string)); }
                if (is_tablet) { DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof(string)); }
                if (model_name) { DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof(string)); }
                if (device_os_version) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof(string)); }
                if (is_wireless_device) { DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof(string)); }
                if (brand_name) { DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof(string)); }
                if (marketing_name) { DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof(string)); }
                if (resolution_height) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof(string)); }
                if (resolution_width) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof(string)); }
                if (canvas_support) { DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof(string)); }
                if (viewport_width) { DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof(string)); }
                if (isviewport_supported) { DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof(string)); }
                if (ismobileoptimized) { DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof(string)); }
                if (ishandheldfriendly) { DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof(string)); }
                if (is_smarttv) { DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof(string)); }
                if (isux_full_desktop) { DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof(string)); }



                ///////////////-----------Device Info-----------/////////////



                DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof(decimal));
                DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof(decimal));
                //foreach (var item in subids)
                //{
                //    DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof(string));
                //}
                Type resultType = affview.CreateType();

                #endregion





                var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, utdate, offsetHour, datecheck, hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck, ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk, bannerchk, parenturlchk, ctrchk,
                     deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name, device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width, canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv, isux_full_desktop,
                   null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname, Vmarketingname, Vresolution, UserAgent
                   );

                dynamic report = repo.ExecuteQuery(resultType, query);


                var clickscheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Clicks);
                var conversioncheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Conversions);
                var costcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Cost);
                var revenuecheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Revenue);
                var profitcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Profit);
                var _countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);

                //var sb = new StringBuilder("Campaign,Affiliate,OfferUrl,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                var sb = new StringBuilder();


                sb.AppendFormat("Campaign,");
                sb.AppendFormat("Affiliate,");
                sb.AppendFormat("OfferUrl,");

                if (clickscheck) sb.AppendFormat("Clicks,");
                if (conversioncheck) sb.AppendFormat("Conversions,");

                sb.AppendFormat("Conv Rate,");

                if (costcheck) sb.AppendFormat("Cost,");

                sb.AppendFormat("CPC,");

                if (revenuecheck) sb.AppendFormat("Revenue,");

                sb.AppendFormat("RPC,");

                if (profitcheck) sb.AppendFormat("Profit,");
                if (_countrycheck) sb.AppendFormat("Country,");
                sb.AppendLine();

                foreach (var item in report)
                {
                    // offer
                    sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
                    // affiliate
                    sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);
                    // OfferUrl
                    sb.AppendFormat("{0},", string.IsNullOrEmpty(item.OfferUrl) ? string.Empty : item.OfferUrl);
                    if (clickscheck) sb.AppendFormat("{0},", item.Clicks);
                    if (conversioncheck) sb.AppendFormat("{0},", item.Conversions);
                    // conv rate
                    sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));
                    // cost
                    if (costcheck) sb.AppendFormat("${0},", item.Cost);
                    // cpc
                    sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));
                    // revenue
                    if (revenuecheck) sb.AppendFormat("${0},", item.Revenue);
                    // rpc
                    sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));
                    // profit
                    if (profitcheck) sb.AppendFormat("${0},", (item.Revenue - item.Cost).ToString("F2"));
                    if (_countrycheck) sb.AppendFormat("{0},", item.Country);
                    sb.AppendLine();
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_traffic_referrals.csv");

            }
        }

        //public ActionResult Statuschange(int status, int conversionid)
        //{
        //    CpaTickerDb db = new CpaTickerDb();
        //    Conversion conversion = db.Conversions.First(con => con.ConversionId == conversionid);
        //    conversion.Status = (conversion.Status + 1) % 2;
        //    db.SaveChanges();
        //    return Content( conversion.Status + "," + conversionid);
        //}

        public ActionResult CTR(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata, long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            CustomViewModel model = new CustomViewModel();

            try
            {
                model.CustomStatisticsEnum = (CustomStatisticsEnum)dataview;

            }
            catch
            {

                model.CustomStatisticsEnum =
                                CustomStatisticsEnum.Campaign |
                                CustomStatisticsEnum.URL |
                                CustomStatisticsEnum.ParentURL |
                                CustomStatisticsEnum.CTR |
                                CustomStatisticsEnum.Clicks
                    //CustomStatisticsEnum.Impressions |
                    //CustomStatisticsEnum.Clicks |
                    //CustomStatisticsEnum.Conversions |
                    //CustomStatisticsEnum.Cost |
                    //CustomStatisticsEnum.Revenue |
                    //CustomStatisticsEnum.Profit
                                ;
            }

            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime ufdate = TimeZoneInfo.ConvertTimeToUtc(fdate, tzi);
            //DateTime utdate = TimeZoneInfo.ConvertTimeToUtc(tdate, tzi);

            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;


            ViewBag.TimeZone = tzi.Id;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int offsetHour = tzi.GetUtcOffset(DateTime.Now).Hours;
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                offsetHour = customoffset / 60;
                ViewBag.TimeZone = timezone;
            }

            ViewBag.FromDate = FromDate.Value.ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ToDate.Value.ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up).Where(c => c.Status == Status.Active).AsEnumerable().Select(c => new SelectListItem
            {
                Value = c.CampaignId.ToString(),
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
            });
            var subids = Request.QueryString.AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();
            ViewBag.MaxSubId = repo.ClickSubIds().Max(c => (int?)c.SubIndex) ?? 0;
            ViewBag.SubIds = subids;
            //string affs = null;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {
                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;



                //affs = up.AffiliateId.ToString();
                //af = new int[] { up.AffiliateId.Value };
            }

            string affs = af == null || af.Length == 0 ? null : string.Join(",", af);

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            /*=================================================================================================================*/

            //string ctfilter = ct.HasValue ?
            //   repo.GetCountries()
            //   .Where(c => c.Id == ct.Value)
            //   .Select(c => c.CountryAbbreviation).FirstOrDefault()
            //   : null;   

            if (state == 1) // export
            {
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
                string Vdeviceid = null; string Vdeviceos = null; string Vbrowser = null; string Vos = null; string Vmodelname = null; string Vbrandname = null; string Vmarketingname = null; string Vresolution = null; string UserAgent = null;
                //Filtering Device Infoes//


                // get the subids
                //var debug = Request.RequestUri.ParseQueryString();
                //var subids = Request.RequestUri.ParseQueryString().AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();

                string ctfilter = ct.HasValue ?
                                       repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                                       : null;

                #endregion

                #region Dynamic Type
                var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
                //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


                if (hourcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof(int));
                }
                if (datecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof(string));
                }

                if (campaigncheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof(string));
                }
                if (affiliatecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                }

                if (urlcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof(string));
                }
                if (countrycheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof(string));
                }
                if (statuscheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof(int));
                }
                if (pixelcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof(DateTime?));
                }
                if (postbackcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof(DateTime?));
                }
                if (ipcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof(string));
                }
                if (transactionidcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof(string));
                }
                if (conversiontypecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof(ConversionType));
                }
                if (referrercheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof(string));
                }
                if (statusdesccheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof(string));
                }
                if (sourcechk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof(string));
                }
                if (actionchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof(string));
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof(int));

                }
                if (bannerchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof(string));
                }
                if (parenturlchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                }
                if (ctrchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ULID", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                }


                ///////////////-----------Device Info-----------/////////////

                if (deviceid) { DynamicType.CreateAutoImplementedProperty(affview, "DeviceId", typeof(string)); }
                if (issmartphone) { DynamicType.CreateAutoImplementedProperty(affview, "IsSmartphone", typeof(string)); }
                if (isios) { DynamicType.CreateAutoImplementedProperty(affview, "IsiOS", typeof(string)); }
                if (isandroid) { DynamicType.CreateAutoImplementedProperty(affview, "IsAndroid", typeof(string)); }
                if (os) { DynamicType.CreateAutoImplementedProperty(affview, "OS", typeof(string)); }
                if (browser) { DynamicType.CreateAutoImplementedProperty(affview, "Browser", typeof(string)); }
                if (device_os) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os", typeof(string)); }
                if (pointing_method) { DynamicType.CreateAutoImplementedProperty(affview, "Pointing_method", typeof(string)); }
                if (is_tablet) { DynamicType.CreateAutoImplementedProperty(affview, "Is_tablet", typeof(string)); }
                if (model_name) { DynamicType.CreateAutoImplementedProperty(affview, "Model_name", typeof(string)); }
                if (device_os_version) { DynamicType.CreateAutoImplementedProperty(affview, "Device_os_version", typeof(string)); }
                if (is_wireless_device) { DynamicType.CreateAutoImplementedProperty(affview, "Is_wireless_device", typeof(string)); }
                if (brand_name) { DynamicType.CreateAutoImplementedProperty(affview, "Brand_name", typeof(string)); }
                if (marketing_name) { DynamicType.CreateAutoImplementedProperty(affview, "Marketing_name", typeof(string)); }
                if (resolution_height) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_height", typeof(string)); }
                if (resolution_width) { DynamicType.CreateAutoImplementedProperty(affview, "Resolution_width", typeof(string)); }
                if (canvas_support) { DynamicType.CreateAutoImplementedProperty(affview, "Canvas_support", typeof(string)); }
                if (viewport_width) { DynamicType.CreateAutoImplementedProperty(affview, "Viewport_width", typeof(string)); }
                if (isviewport_supported) { DynamicType.CreateAutoImplementedProperty(affview, "Isviewport_supported", typeof(string)); }
                if (ismobileoptimized) { DynamicType.CreateAutoImplementedProperty(affview, "Ismobileoptimized", typeof(string)); }
                if (ishandheldfriendly) { DynamicType.CreateAutoImplementedProperty(affview, "Ishandheldfriendly", typeof(string)); }
                if (is_smarttv) { DynamicType.CreateAutoImplementedProperty(affview, "Is_smarttv", typeof(string)); }
                if (isux_full_desktop) { DynamicType.CreateAutoImplementedProperty(affview, "Isux_full_desktop", typeof(string)); }



                ///////////////-----------Device Info-----------/////////////



                DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof(decimal));
                DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof(decimal));
                //foreach (var item in subids)
                //{
                //    DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof(string));
                //}
                Type resultType = affview.CreateType();

                #endregion





                var query = CKQueryBuilder.CustomReportQuery(up.CustomerId, up.UserId, ufdate, utdate, offsetHour, datecheck, hourcheck, campaigncheck, affiliatecheck, urlcheck, countrycheck, statuscheck, pixelcheck, postbackcheck, ipcheck, transactionidcheck, conversiontypecheck, referrercheck, statusdesccheck, sourcechk, actionchk, bannerchk, parenturlchk, ctrchk,
                     deviceid, issmartphone, isios, isandroid, os, browser, device_os, pointing_method, is_tablet, model_name, device_os_version, is_wireless_device, brand_name, marketing_name, resolution_height, resolution_width, canvas_support, viewport_width, isviewport_supported, ismobileoptimized, ishandheldfriendly, is_smarttv, isux_full_desktop,
                   null, cp, af, ctfilter, Vdeviceid, Vdeviceos, Vbrowser, Vos, Vmodelname, Vbrandname, Vmarketingname, Vresolution, UserAgent
                   );

                var report = repo.ExecuteQuery(resultType, query);

                //var report = repo.RunQuery<CTRView>("EXEC [CTRReport] {0}, {1}, {2}, {3}, {4}, {5}, {6}",
                //               up.CustomerId, ufdate, utdate, up.UserId, affs, cp, null).ToList();

                dynamic Returnlist = report.ToListAsync().Result;

                if (ctrchk)
                {
                    Returnlist.Clear();
                    var dic = new Dictionary<int, Holder>();

                    // fill the dictionary
                    int count = 1;
                    foreach (var item in report)
                    {
                        int UlID = count;
                        //int UlID = Convert.ToInt32(item.GetType().GetProperty("ULID").GetValue(item, null));
                        int clicks = Convert.ToInt32(item.GetType().GetProperty("Clicks").GetValue(item, null));
                        int? ParentURLID = (int?)item.GetType().GetProperty("ParentURL").GetValue(item, null);
                        int CampaignId = Convert.ToInt32(item.GetType().GetProperty("CampaignId").GetValue(item, null));
                        int PreviewId = Convert.ToInt32(item.GetType().GetProperty("URLPreviewId").GetValue(item, null));

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
                        int? ParentURLID = (int?)item.GetType().GetProperty("ParentURL").GetValue(item, null);
                        int clicks = Convert.ToInt32(item.GetType().GetProperty("Clicks").GetValue(item, null));
                        int previous = ctr(ParentURLID);

                        PropertyInfo property = item.GetType().GetProperty("CTR");
                        property.SetValue(item, Convert.ChangeType(previous == 0 ? clicks : clicks / (int)previous, property.PropertyType), null);
                        // item.CTR = previous == 0 ? item.Clicks : item.Clicks / (int)previous;

                        if (ParentURLID.HasValue)
                        {
                            if (dic.Keys.Contains(ParentURLID.Value))
                            {
                                var parent = dic[ParentURLID.Value];

                                //  item.ParentURLIdText = string.Format("{0} - {1}", parent.CampaignId, parent.PreviewId);
                            }
                            //item.ParentURLIdText = ParentURLID.ToString();
                        }
                        Returnlist.Add(item);
                    }
                }
                var sb = new StringBuilder();
                sb.AppendLine("Campaign,URLPreviewId,ParentActionId,OfferUrl,Clicks,CTR");

                foreach (var item in Returnlist)
                {
                    sb.AppendFormat("{0}-{1},", item.CampaignId, item.CampaignName);
                    sb.AppendFormat("{0},", item.URLPreviewId);
                    sb.AppendFormat("{0},", (item.ParentURL != null) ? item.ParentURL : "");
                    sb.AppendFormat("{0},", item.OfferUrl);
                    sb.AppendFormat("{0},", item.Clicks);
                    sb.AppendFormat("{0},", item.CTR);
                    sb.AppendLine();
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_ctr.csv");
            }

            //return View(model);
            return View("CustomReport", model);
        }

        public ActionResult CustomReport(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
             long? dataview = null, [System.Web.Http.FromUri] int[] cp = null, [System.Web.Http.FromUri] int[] af = null, [System.Web.Http.FromUri] int[] ct = null, int state = 0,
             bool showall = true, bool QDate = false, bool QHour = false, bool QCampaign = false, bool QAffiliate = false, bool QURL = false, bool QCountry = false, bool QStatus = false, bool QPixel = false, bool QPostback = false, bool QIP = false, bool QTransactionID = false, bool QConversionType = false, bool QReferrer = false, bool QStatusDescription = false, bool QSource = false, bool QActionName = false, bool QBanner = false, bool QParentURL = false, bool QCTR = false,
             bool QDeviceId = false, bool QIsSmartphone = false, bool QIsiOS = false, bool QIsAndroid = false, bool QOS = false, bool QBrowser = false, bool QDevice_os = false, bool QPointing_method = false, bool QIs_tablet = false, bool QModel_name = false, bool QDevice_os_version = false, bool QIs_wireless_device = false, bool QBrand_name = false,
             bool QMarketing_name = false, bool QResolution_height = false, bool QResolution_width = false, bool QCanvas_support = false, bool QViewport_width = false, bool QIsviewport_supported = false, bool QIsmobileoptimized = false, bool QIshandheldfriendly = false, bool QIs_smarttv = false, bool QIsux_full_desktop = false, bool QClickDate = false, bool QConversionDate = false, bool QImpressionDate = false, bool QPAGE = false, bool QRedirect = false,
             string Vdeviceid = null, string Vdeviceos = null, string Vbrowser = null, string Vos = null, string Vmodelname = null, string Vbrandname = null, string Vmarketingname = null, string Vresolution = null, string UserAgent = null,
            int ReportID = 0, string ReportType = "")
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);
            var TimeZoneList = TimeZoneInfo.GetSystemTimeZones().ToList();
            List<SelectListItem> lsttizezones = new List<SelectListItem>();
            for (int a = 0; a < TimeZoneList.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = TimeZoneList[a].Id, Text = TimeZoneList[a].DisplayName });

            }
            var customtimezonelist = repo.GetCustomTimezoneByUser(up.UserId);
            for (int a = 0; a < customtimezonelist.Count; a++)
            {
                lsttizezones.Add(new SelectListItem { Value = customtimezonelist[a].Value, Text = customtimezonelist[a].Text });

            }
            ViewBag.CustomTimeZone = lsttizezones;
            CustomViewModel model = new CustomViewModel();

            try
            {
                model.CustomStatisticsEnum = (CustomStatisticsEnum)dataview;
            }
            catch
            {

                // here ReportType is predefined reports based on type statistics will be pre selected.
                if (ReportType == "hourly")
                {
                    model.CustomStatisticsEnum =
                                CustomStatisticsEnum.Hour |
                                CustomStatisticsEnum.CR |
                                CustomStatisticsEnum.CPC |
                                CustomStatisticsEnum.RPC |
                        // CustomStatisticsEnum.Impressions |
                                CustomStatisticsEnum.Clicks |
                                CustomStatisticsEnum.Conversions |
                                CustomStatisticsEnum.Cost |
                                CustomStatisticsEnum.Revenue |
                                CustomStatisticsEnum.Profit
                                ;
                }
                else if (ReportType == "daily")
                {
                    model.CustomStatisticsEnum =
                                  CustomStatisticsEnum.Date |
                                  CustomStatisticsEnum.CR |
                                  CustomStatisticsEnum.CPC |
                                  CustomStatisticsEnum.RPC |
                        //  CustomStatisticsEnum.Impressions |
                                  CustomStatisticsEnum.Clicks |
                                  CustomStatisticsEnum.Conversions |
                                  CustomStatisticsEnum.Cost |
                                  CustomStatisticsEnum.Revenue |
                                  CustomStatisticsEnum.Profit
                                  ;
                }
                else if (ReportType == "affiliate")
                {
                    model.CustomStatisticsEnum =
                                  CustomStatisticsEnum.CR |
                                  CustomStatisticsEnum.CPC |
                                  CustomStatisticsEnum.RPC |
                        //  CustomStatisticsEnum.Impressions |
                                  CustomStatisticsEnum.Clicks |
                                  CustomStatisticsEnum.Conversions |
                                  CustomStatisticsEnum.Cost |
                                  CustomStatisticsEnum.Revenue |
                                  CustomStatisticsEnum.Profit |
                                  CustomStatisticsEnum.Affiliate
                                  ;
                }
                else if (ReportType == "conversion")
                {
                    model.CustomStatisticsEnum =
                                  CustomStatisticsEnum.ConversionDate |
                                  CustomStatisticsEnum.CR |
                                  CustomStatisticsEnum.CPC |
                                  CustomStatisticsEnum.RPC |
                                  CustomStatisticsEnum.Conversions |
                                  CustomStatisticsEnum.Cost |
                                  CustomStatisticsEnum.Revenue |
                                  CustomStatisticsEnum.Profit |
                                  CustomStatisticsEnum.Status |
                                  CustomStatisticsEnum.IsSmartphone |
                                  CustomStatisticsEnum.Browser
                                  ;
                }
                else if (ReportType == "campaign")
                {
                    model.CustomStatisticsEnum =
                                 CustomStatisticsEnum.CR |
                                 CustomStatisticsEnum.CPC |
                                 CustomStatisticsEnum.RPC |
                        // CustomStatisticsEnum.Impressions |
                                 CustomStatisticsEnum.Clicks |
                                 CustomStatisticsEnum.Conversions |
                                 CustomStatisticsEnum.Cost |
                                 CustomStatisticsEnum.Revenue |
                                 CustomStatisticsEnum.Profit |
                                 CustomStatisticsEnum.Campaign
                                 ;
                }
                else if (ReportType == "clicksdetailslog")
                {
                    model.CustomStatisticsEnum =
                                  CustomStatisticsEnum.ClickDate |
                                  CustomStatisticsEnum.CR |
                                  CustomStatisticsEnum.CPC |
                                  CustomStatisticsEnum.RPC |
                                  CustomStatisticsEnum.Clicks |
                                  CustomStatisticsEnum.Cost |
                                  CustomStatisticsEnum.Revenue |
                                  CustomStatisticsEnum.Profit |
                                  CustomStatisticsEnum.Affiliate |
                                  CustomStatisticsEnum.Referrer
                                  ;
                }
                else if (ReportType == "ctr")
                {
                    model.CustomStatisticsEnum =
                                   CustomStatisticsEnum.Campaign |
                                   CustomStatisticsEnum.URL |
                                   CustomStatisticsEnum.ParentURL |
                                   CustomStatisticsEnum.CTR |
                                   CustomStatisticsEnum.Clicks
                                   ;
                }
                else
                {
                    model.CustomStatisticsEnum =
                                    CustomStatisticsEnum.CR |
                                    CustomStatisticsEnum.CPC |
                                    CustomStatisticsEnum.RPC |
                        //   CustomStatisticsEnum.Impressions |
                                    CustomStatisticsEnum.Clicks |
                                    CustomStatisticsEnum.Conversions |
                                    CustomStatisticsEnum.Cost |
                                    CustomStatisticsEnum.Revenue |
                                    CustomStatisticsEnum.Profit
                                    ;
                }

            }

            var countrycheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Country);



            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            var offset = tzi.GetUtcOffset(DateTime.Now);
            DateTime ufdate = new DateTimeOffset(fdate, offset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(tdate, offset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;

            int customoffset = repo.Getcustomoffset(up.UserId, timezone);
            int offsetHour = (int)tzi.GetUtcOffset(DateTime.Now).TotalMinutes;
            if (customoffset != 0)
            {
                ufdate = FromDate.Value.AddMinutes(customoffset);
                utdate = ToDate.Value.AddMinutes(customoffset);
                ViewBag.TimeZone = timezone;
                offsetHour = customoffset;
            }

            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;
            ViewBag.ReportData = repo.GetCustomReportByID(up.CustomerId, up.UserId, ReportID);
            ViewBag.UserAgent = repo.GetUserAgent();
            //bool datecheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Date);
            //bool hourcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Hour);
            //bool campaigncheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Campaign); //string.IsNullOrEmpty(offview);
            //bool affiliatecheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Affiliate);
            //bool urlcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.URL);
            //bool sourcecheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Source); //!string.IsNullOrEmpty(sourceview);
            //bool statuscheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Status);
            //bool statusdesccheck = model.StatisticsEnum.HasFlag(StatisticsEnum.StatusDescription);

            ViewBag.FirstOperation = repo.GetFirstOperation(up.CustomerId); //.AddMinutes(customoffset);

            bool datecheck = QDate;
            bool hourcheck = QHour;
            bool campaigncheck = QCampaign; //string.IsNullOrEmpty(offview);
            bool affiliatecheck = QAffiliate;
            bool urlcheck = QURL;

            countrycheck = QCountry;
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
            bool parenturlchk = QParentURL;
            bool ctrchk = QCTR;
            bool pagechk = QPAGE;
            bool redirectchk = QRedirect;
            //ViewBag.SimpleReport = simplereport;
            //ViewBag.SourceCheck = sourcechk;

            /////////////---------Device Info--------------////////////////
            bool deviceid = QDeviceId;
            bool issmartphone = QIsSmartphone;
            bool isios = QIsiOS;
            bool isandroid = QIsAndroid;
            bool os = QOS;
            bool browser = QBrowser;
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
            bool clickdatecheck = QClickDate;
            bool conversiondatecheck = QConversionDate;
            bool impressiondatecheck = QImpressionDate;



            // get the subids
            var subids = Request.QueryString.AllKeys.Where(s => s.ToLower().StartsWith("subid")).Select(s => int.Parse(s.Substring(5))).ToArray();
            ViewBag.MaxSubId = repo.ClickSubIds().Max(c => (int?)c.SubIndex) ?? 0;
            ViewBag.SubIds = subids;
            ViewBag.SelectedUserAgent = UserAgent;

            ViewBag.IsAdmin = !up.AffiliateId.HasValue;


            ViewBag.SelectedCampaignId = false;
            if (cp != null)
            {
                ViewBag.SelectedCampaignId = true;
                ViewBag.CustomerCampaigns = (from c in repo.GetUserCampaigns(up).Where(c => c.Status == Status.Active).AsEnumerable()
                                             join s in cp on c.CampaignId equals s into sa
                                             from s in sa.DefaultIfEmpty()
                                             select new SelectListItem
                                             {
                                                 Value = c.CampaignId.ToString(),
                                                 Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
                                                 Selected = s != 0
                                             }).OrderBy(u => u.Value);
            }
            else
            {
                ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up).Where(c => c.Status == Status.Active)
                    .AsEnumerable().Select(c => new SelectListItem
                    {
                        Value = c.CampaignId.ToString(),
                        Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
                    });
            }


            //string affs = null;
            //bool faffiliate = false;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {
                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                if (af != null)
                {
                    ViewBag.SelectedAffiliateId = true;
                    ViewBag.CustomerAffiliates = (from a in repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable()
                                                  join s in af on a.AffiliateId equals s into sa
                                                  from s in sa.DefaultIfEmpty()
                                                  select new SelectListItem
                                                  {
                                                      Value = a.AffiliateId.ToString(),
                                                      Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                      Selected = s != 0
                                                  }).OrderBy(u => u.Value);

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetUserAffiliates(up.UserId, up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    }).OrderBy(u => u.Value);
                    af = repo.GetUserAffiliates(up.UserId, up.CustomerId).Select(u => u.AffiliateId).ToArray();
                }

                ViewBag.ShowAll = showall;



                //af = new int [] { up.AffiliateId.Value }; // set the af to this af
                //affs = up.AffiliateId.ToString();
                //af = new int[] { up.AffiliateId.Value };
            }

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            /******************************************************************************************************************/

            if (state == 0)
            {
                //foreach (var item in list)
                //{
                //    lavm.AffiliateViewList.Add(item);
                //}

                //ViewBag.list = list;
                //lavm.AffiliateViewList = list;

                return View(model);
            }

            else
            {

                var subidcheck = subids.Length > 0 ? true : false;
                var impressioncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Impressions);
                var clickcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Clicks);
                var conversioncheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Conversions);
                var costcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Cost);
                var revenuecheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Revenue);
                var profitcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.Profit);

                var crcheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CR);
                var cpccheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.CPC);
                var rpccheck = model.CustomStatisticsEnum.HasFlag(CustomStatisticsEnum.RPC);

                #region Dynamic Type
                var affview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");
                //DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                //DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));


                if (hourcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Hour", typeof(int));
                }
                if (datecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Date", typeof(string));
                }

                if (clickdatecheck || conversiondatecheck || impressiondatecheck)
                    DynamicType.CreateAutoImplementedProperty(affview, "CDatetime", typeof(DateTime?));

                if (clickdatecheck)
                    DynamicType.CreateAutoImplementedProperty(affview, "ClickDate", typeof(DateTime?));

                if (impressiondatecheck)
                    DynamicType.CreateAutoImplementedProperty(affview, "ImpressionDate", typeof(DateTime?));

                if (conversiondatecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ConversionDate", typeof(DateTime?));
                    DynamicType.CreateAutoImplementedProperty(affview, "ConversionID", typeof(int));
                }


                if (campaigncheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "CampaignName", typeof(string));
                }
                if (affiliatecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "AffiliateId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "Company", typeof(string));
                }

                if (urlcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "URLPreviewId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "OfferUrl", typeof(string));
                }

                if (pagechk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "PAGEId", typeof(int));
                    DynamicType.CreateAutoImplementedProperty(affview, "PAGEURL", typeof(string));
                }
                if (countrycheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof(string));
                }
                if (statuscheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Status", typeof(int));
                }
                if (pixelcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Pixel", typeof(string));
                }
                if (postbackcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Postback", typeof(string));
                }
                if (ipcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "IP", typeof(string));
                }
                if (transactionidcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "TransactionID", typeof(string));
                }
                if (conversiontypecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ConversionType", typeof(ConversionType));
                }
                if (referrercheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Referrer", typeof(string));
                }
                if (statusdesccheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "StatusDescription", typeof(string));
                }
                if (sourcechk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof(string));
                }
                if (actionchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionName", typeof(string));
                    DynamicType.CreateAutoImplementedProperty(affview, "ActionId", typeof(int));
                }
                if (bannerchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Banner", typeof(string));
                }
                if (parenturlchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "ParentURL", typeof(int?));
                }
                if (ctrchk)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "CTR", typeof(int));
                }

                DynamicType.CreateAutoImplementedProperty(affview, "Impressions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Conversions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Clicks", typeof(int));
                DynamicType.CreateAutoImplementedProperty(affview, "Cost", typeof(decimal));
                DynamicType.CreateAutoImplementedProperty(affview, "Revenue", typeof(decimal));

                foreach (var item in subids)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof(string));
                }
                Type resultType = affview.CreateType();

                #endregion

                //string ctfilter = ct.HasValue ?
                //                   repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                //                   : null;
                string[] cts = (ct != null) ? repo.GetCountries().Where(c => ct.Contains(c.Id)).Select(c => c.CountryAbbreviation).ToArray() : null;

                dynamic list = repo.ExecuteQuery(resultType, "EXEC [MasterCustomReportB] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}, {31}, {32}, {33}, {34}, {35}, {36}, {37}, {38}, {39}, {40}, {41}, {42}, {43}, {44}, {45}, {46}, {47}, {48}, {49}, {50}, {51}, {52}, {53}, {54}, {55}, {56}, {57}, {58}, {59}, {60}, {61}, {62}, {63}, {64}, {65}, {66}, {67}",
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
                         clickcheck,
                         conversioncheck,
                         impressioncheck
                         );


                // dynamic list = repo.ExecuteQuery(resultType, query);


                StringBuilder sb = sb = new StringBuilder();

                
                if (hourcheck)
                {
                    sb.AppendFormat("Hour,");
                }
                if (datecheck)
                {
                    sb.AppendFormat("Date,");
                }
                if (subidcheck)
                {
                    for (int i = 0; i < subids.Length; i++)
                    {
                        sb.AppendFormat("SubId" + subids[i] + ",");
                    }
                }


                if (campaigncheck)
                {
                    sb.AppendFormat("Campaign,");

                }
                if (affiliatecheck)
                {
                    sb.AppendFormat("Affiliate,");

                }

                if (urlcheck)
                {
                    sb.AppendFormat("OfferUrl,");

                }
                if (countrycheck)
                {
                    sb.AppendFormat("Country,");
                }
                if (statuscheck)
                {
                    sb.AppendFormat("Status,");
                }

                if (statusdesccheck)
                {
                    sb.AppendFormat("StatusDescription,");
                }
                if (sourcechk)
                {
                    sb.AppendFormat("Source,");
                }
                if (actionchk)
                {
                    sb.AppendFormat("Action,");
                }
                if (bannerchk)
                {
                    sb.AppendFormat("Banner,");
                }
                if (parenturlchk)
                {
                    sb.AppendFormat("ParentURL,");
                }
                if (ctrchk)
                {
                    sb.AppendFormat("CTR,");
                }



                if (impressioncheck) sb.AppendFormat("Impressions,");
                if (clickcheck) sb.AppendFormat("Clicks,");
                if (conversioncheck) sb.AppendFormat("Conversions,");
                if (crcheck) sb.AppendFormat("Conv Rate,");
                if (costcheck) sb.AppendFormat("Cost,");
                if (cpccheck) sb.AppendFormat("CPC,");
                if (revenuecheck) sb.AppendFormat("Revenue,");
                if (rpccheck) sb.AppendFormat("RPC,");
                if (profitcheck) sb.AppendFormat("Profit,");

                sb.AppendLine();

                foreach (var item in list)
                {
                    if (hourcheck)
                    {
                        sb.AppendFormat("{0},", item.Hour);
                    }
                    if (datecheck)
                    {
                        sb.AppendFormat("{0},", item.Date);
                    }
                    if (subidcheck)
                    {
                        for (int a = 0; a < subids.Length; a++)
                        {
                            string sub = "SubId" + subids[a];
                            sb.AppendFormat("{0},", item.GetType().GetProperty(sub).GetValue(item, null));
                            //sb.AppendFormat("{0},", item.SubId + subids[a]); 
                        }
                    }
                    if (campaigncheck)
                    {
                        sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);

                    }
                    if (affiliatecheck)
                    {
                        sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);

                    }

                    if (urlcheck)
                    {

                        sb.AppendFormat("{0},", item.OfferUrl);

                    }
                    if (countrycheck)
                    {
                        sb.AppendFormat("{0},", item.Country);
                    }
                    if (statuscheck)
                    {
                        sb.AppendFormat("{0},", item.Source);
                    }

                    if (statusdesccheck)
                    {
                        string Desc = ((string)item.StatusDescription);
                        sb.AppendFormat("{0},", Regex.Replace(Desc, @"[^0-9a-zA-Z]+", "").Replace(",", ""));
                    }
                    if (sourcechk)
                    {
                        sb.AppendFormat("{0},", item.Source);
                    }
                    if (actionchk)
                    {
                        sb.AppendFormat("{0} - {1},", item.ActionId, item.ActionName);
                    }
                    if (bannerchk)
                    {
                        sb.AppendFormat("{0},", item.Banner);
                    }
                    if (parenturlchk)
                    {
                        sb.AppendFormat("{0},", item.ParentURL);
                    }
                    if (ctrchk)
                    {
                        sb.AppendFormat("{0},", item.CTR);
                    }

                    // impressions
                    if (impressioncheck) sb.AppendFormat("{0},", item.Impressions);
                    //clicks
                    if (clickcheck) sb.AppendFormat("{0},", item.Clicks);
                    //conversions
                    if (conversioncheck) sb.AppendFormat("{0},", item.Conversions);
                    // conv rate
                    if (crcheck) sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));
                    // cost
                    if (costcheck) sb.AppendFormat("${0},", item.Cost.ToString("F2"));
                    // cpc
                    if (cpccheck) sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));
                    // revenue
                    if (revenuecheck) sb.AppendFormat("${0},", item.Revenue.ToString("F2"));
                    // rpc
                    if (rpccheck) sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));
                    // profit
                    if (profitcheck) sb.AppendFormat("${0},", (item.Revenue - item.Cost).ToString("F2"));
                    sb.AppendLine();
                }


                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_affiliates.csv");
            }
        }


        public ActionResult ManageReport()
        {
            var up = repo.GetCurrentUser();
            var Result = repo.GetCustomReports(up.CustomerId, up.UserId);
            return View(Result);
        }


        public ActionResult DeleteManageReport(int id)
        {
            var up = repo.GetCurrentUser();
            var CR = repo.FindCustomReport(id);
            if (CR == null)
            {
                return HttpNotFound();
            }
            return View(repo.GetCustomReportByID(up.CustomerId, up.UserId, id));
        }


        [HttpPost, ActionName("DeleteManageReport")]
        public ActionResult DeleteConfirmed(int id)
        {
            var up = repo.GetCurrentUser();
            var Result = repo.DeleteCustomReports(up.CustomerId, up.UserId, id);
            return RedirectToAction("ManageReport");
        }

        private delegate int myDelegate(int? id);

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }

        //private void SetFilterTitleViewbag(bool selectedaffiliate, int[] af, int? cp, int? ct)
        //{
        //    var _title = new StringBuilder();

        //    // affiliate
        //    if (ViewBag.SelectedAffiliateId)
        //    {
        //        _title.Append(" - AffiliateId");
        //        foreach (var item in af)
        //        {
        //            _title.AppendFormat(", {0}", item);
        //        }
        //    }

        //    // campaign
        //    if (cp.HasValue)
        //    {
        //        _title.AppendFormat(" - Campaign, {0}", cp);
        //    }

        //    //country
        //    if (ct.HasValue)
        //    {
        //        _title.AppendFormat(" - Country, {0}", 
        //            repo.FindCountry(ct.Value).CountryAbbreviation
        //            );
        //    }

        //    if (_title.Length > 0)
        //    {
        //        _title.Remove(0, 2);
        //        _title.Insert(0, "Active Filters:");
        //    }

        //    ViewBag.tfilters = _title.ToString();

        //}
    }
}
