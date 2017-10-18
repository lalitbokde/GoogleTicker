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
//using System.Web.Http;

namespace CpaTicker.Areas.signalra.Controllers
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

            AffiliateViewModel model = new AffiliateViewModel();

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



            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            var offset = tzi.BaseUtcOffset;
            DateTime ufdate = new DateTimeOffset(fdate, offset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(tdate, offset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;
            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            bool campaigncheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Campaign); //string.IsNullOrEmpty(offview);
            bool sourcecheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Source); //!string.IsNullOrEmpty(sourceview);
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
                    ViewBag.CustomerAffiliates = from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                 join s in af on a.AffiliateId equals s into sa
                                                 from s in sa.DefaultIfEmpty()
                                                 select new SelectListItem
                                                 {
                                                     Value = a.AffiliateId.ToString(),
                                                     Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                     Selected = s != 0
                                                 };

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    });

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                //af = new int [] { up.AffiliateId.Value }; // set the af to this af
                //affs = up.AffiliateId.ToString();
                af = new int[] { up.AffiliateId.Value };
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
                if (sourcecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Source", typeof(string));
                }
                if (countrycheck)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, "Country", typeof(string));
                }


                foreach (var item in subids)
                {
                    DynamicType.CreateAutoImplementedProperty(affview, string.Format("SubId{0}", item), typeof(string));
                }
                Type resultType = affview.CreateType();

                #endregion

                string ctfilter = ct.HasValue ?
                                   repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                                   : null;

                var query = CKQueryBuilder.AffiliateReportQuery(up.CustomerId, up.UserId, campaigncheck, sourcecheck
                   , subids.Length > 0 ? subids : null, countrycheck
                   , cp, af, ctfilter);

                dynamic list = repo.ExecuteQuery(resultType, query
                    , new SqlParameter("fromdate", ufdate)
                    , new SqlParameter("todate", utdate)
                    );

                //ViewBag._from = ufdate;
                //ViewBag._to = utdate;
                //ViewBag._query = query;

                StringBuilder sb = null;
                if (!campaigncheck)
                {
                    sb = new StringBuilder("Company,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                    sb.AppendLine();

                    foreach (var item in list)
                    {
                        //sb.AppendFormat("{0},", item.CampaignId);
                        sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);
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
                        sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb = new StringBuilder("Campaign,Company,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                    sb.AppendLine();

                    foreach (var item in list)
                    {
                        sb.AppendFormat("{0},", item.CampaignId);
                        sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);
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
                        sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));
                        sb.AppendLine();
                    }
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_affiliates.csv");
            }
        }

        public ActionResult ConversionStatus(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

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
                    ViewBag.CustomerAffiliates = from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                 join s in af on a.AffiliateId equals s into sa
                                                 from s in sa.DefaultIfEmpty()
                                                 select new SelectListItem
                                                 {
                                                     Value = a.AffiliateId.ToString(),
                                                     Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                     Selected = s != 0
                                                 };

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
                    });

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                affs = up.AffiliateId.ToString();

                af = new int[] { up.AffiliateId.Value };
            }

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            var offset = tzi.BaseUtcOffset;
            DateTime ufdate = new DateTimeOffset(fdate, offset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(tdate, offset).UtcDateTime;


            ViewBag.TimeZone = tzi.Id;
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

                var sb = new StringBuilder("Offer,Affiliate,Gross Conversions,Approved Conversions,%Approved,RejectedConversions,%Rejected,Net Payout,Net Revenue");
                sb.AppendLine();

                foreach (var item in report)
                {
                    // offer
                    sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
                    // affiliate
                    sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);
                    // all conversions
                    sb.AppendFormat("{0},", item.GrossConversions);
                    // approved conversions
                    sb.AppendFormat("{0},", item.ApprovedConversions);
                    // % approved 
                    sb.AppendFormat("{0}%,", item.GrossConversions == 0 ? 0 : 100 * ((double)item.ApprovedConversions / item.GrossConversions));
                    // RejectedConversions
                    sb.AppendFormat("${0},", item.RejectedConversions);
                    // % rejected
                    sb.AppendFormat("{0}%,", item.GrossConversions == 0 ? 0 : 100 * ((double)item.RejectedConversions / item.GrossConversions));
                    // net payout
                    sb.AppendFormat("${0},", item.NetPayout);
                    // net revenue
                    sb.AppendFormat("${0},", item.NetRevenue);
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

            var model = new ConversionViewModel();

            try
            {
                model.StatisticsEnum = (StatisticsEnum)dataview;
            }
            catch
            {

                model.StatisticsEnum =
                                StatisticsEnum.Status |
                                StatisticsEnum.Campaign |
                                StatisticsEnum.Affiliate |
                                StatisticsEnum.Cost |
                                StatisticsEnum.Revenue |
                                StatisticsEnum.Profit
                                ;
            }

            //int statistics = 3364376;


            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
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
                    ViewBag.CustomerAffiliates = from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                 join s in af on a.AffiliateId equals s into sa
                                                 from s in sa.DefaultIfEmpty()
                                                 select new SelectListItem
                                                 {
                                                     Value = a.AffiliateId.ToString(),
                                                     Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                     Selected = s != 0
                                                 };


                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    });

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                //af = up.AffiliateId; // set the af to this af
                //affs = up.AffiliateId.ToString();
                af = new int[] { up.AffiliateId.Value };
            }

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            var offset = tzi.BaseUtcOffset;
            DateTime ufdate = new DateTimeOffset(fdate, offset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(tdate, offset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;
            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            bool _af = af == null;
            if (_af)
            {
                af = new int[] { };
            }
            var affiliates = repo.Affiliates();



            if (state == 0)
            {

                //ViewBag.list = list;
                //return View(lcvm);
                return View(model);

            }
            else
            {

                string ctfilter = ct.HasValue ?
               repo.GetCountries()
               .Where(c => c.Id == ct.Value)
               .Select(c => c.CountryAbbreviation).FirstOrDefault()
               : null;


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
                               SubIds = co.Click.SubIds,
                               Status = co.Status,
                               StatusDescription = co.StatusDescription,
                               Postback_IPAddress = co.Postback_IPAddress,
                               Pixel = co.Pixel,
                               Postback = co.Postback,
                               ActionName = co.Action.Name,
                               Country = co.Country,
                           };

                var sb = new StringBuilder("Date,Status,Campaign,Affiliate,UserAgent,IPAddress,TransactionId,Cost,Revenue,Profit");
                sb.AppendLine();

                foreach (var item in list)
                {
                    // date
                    sb.AppendFormat("{0},", item.ConversionDate);
                    // Status
                    sb.AppendFormat("{0},", item.Status);
                    // CampaignName
                    sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
                    // company
                    sb.AppendFormat("{0},", item.Company);
                    //UserAgent
                    sb.AppendFormat("{0},", item.UserAgent);
                    //ip
                    sb.AppendFormat("{0},", item.IPAddress);
                    // TransactionId
                    sb.AppendFormat("{0},", item.TransactionId);
                    // cost
                    sb.AppendFormat("${0},", item.Cost.ToString("F2"));
                    // revenue
                    sb.AppendFormat("${0},", item.Revenue.ToString("F2"));
                    // profit
                    sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));
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

            var model = new OfferViewModel();

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);


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


            //lavm.OfferViewList = new List<OfferView>();
            //lavm.Filter = new Models.Filter();
            //lavm.Stadisctics = Statistics.Create(statistics);
            //lavm.Calculation = Calculation.Create(calculation);

            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;

            bool affiliatecheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Affiliate);
            bool countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);
            bool urlcheck = model.StatisticsEnum.HasFlag(StatisticsEnum.URLId);
            //ViewBag.URLCheck = urlcheck;
            bool sourcechk = model.StatisticsEnum.HasFlag(StatisticsEnum.Source);
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
                    ViewBag.CustomerAffiliates = from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                 join s in af on a.AffiliateId equals s into sa
                                                 from s in sa.DefaultIfEmpty()
                                                 select new SelectListItem
                                                 {
                                                     Value = a.AffiliateId.ToString(),
                                                     Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                     Selected = s != 0
                                                 };


                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    });

                }

                ViewBag.ShowAll = showall;

                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                //affs = up.AffiliateId.ToString();
                af = new int[] { up.AffiliateId.Value };
            }

            //var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //var offset = tzi.BaseUtcOffset;
            //DateTime ufdate = new DateTimeOffset(fdate, offset).UtcDateTime;
            //DateTime utdate = new DateTimeOffset(tdate, offset).UtcDateTime;

            var fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            var tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            TimeSpan toffset;
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZone);
            toffset = tzi.GetUtcOffset(fdate);
            int Offsethour = tzi.GetUtcOffset(DateTime.Now).Hours;
            var ufdate = new DateTimeOffset(fdate, toffset).UtcDateTime;
            var utdate = new DateTimeOffset(tdate, toffset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;
            ViewBag.FromDate = ((DateTime)FromDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ToDate = ((DateTime)ToDate).ToString("MM/dd/yyyy HH:mm");
            ViewBag.ViewData = viewdata;

            //SetFilterTitleViewbag(ViewBag.SelectedAffiliateId, af, cp, ct);

            if (state == 0)
            {
                //lavm.OfferViewList = list;
                //ViewBag.list = list;
                //return View(lavm);

                return View(model);
            }
            else
            {
                string ctfilter = ct.HasValue ?
                       repo.GetCountries().Where(c => c.Id == ct.Value).Select(c => c.CountryAbbreviation).FirstOrDefault()
                       : null;

                #region Dynamic Type
                var offview = DynamicType.CreateTypeBuilder("MyDynamicAssembly", "MyModule", "MyType");

                DynamicType.CreateAutoImplementedProperty(offview, "CampaignName", typeof(string));
                DynamicType.CreateAutoImplementedProperty(offview, "CampaignId", typeof(int));
                DynamicType.CreateAutoImplementedProperty(offview, "Impressions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(offview, "Conversions", typeof(int));
                DynamicType.CreateAutoImplementedProperty(offview, "Clicks", typeof(int));
                DynamicType.CreateAutoImplementedProperty(offview, "Cost", typeof(decimal));
                DynamicType.CreateAutoImplementedProperty(offview, "Revenue", typeof(decimal));

                if (affiliatecheck)
                {
                    DynamicType.CreateAutoImplementedProperty(offview, "Company", typeof(string));
                    DynamicType.CreateAutoImplementedProperty(offview, "AffiliateId", typeof(int));
                }
                if (sourcechk)
                {
                    DynamicType.CreateAutoImplementedProperty(offview, "Source", typeof(string));
                }
                if (countrycheck)
                {
                    DynamicType.CreateAutoImplementedProperty(offview, "Country", typeof(string));
                }
                if (urlcheck)
                {
                    DynamicType.CreateAutoImplementedProperty(offview, "URLPreviewId", typeof(int));
                }
                foreach (var item in subids)
                {
                    DynamicType.CreateAutoImplementedProperty(offview, string.Format("SubId{0}", item), typeof(string));
                }
                Type resultType = offview.CreateType();
                #endregion

                var query = CKQueryBuilder.CampaignReportQuery(up.CustomerId, up.UserId,Offsethour, affiliatecheck, sourcechk

                    , subids.Length > 0 ? subids : null
                    , urlcheck, countrycheck
                    , cp, af, ctfilter);


                dynamic list = repo.ExecuteQuery(resultType, query
                   , new SqlParameter("fromdate", ufdate)
                   , new SqlParameter("todate", utdate)
                   );

                StringBuilder sb = null;
                if (!affiliatecheck)
                {
                    sb = new StringBuilder("CampaignName,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                    sb.AppendLine();

                    foreach (var item in list)
                    {
                        //sb.AppendFormat("{0},", item.AffiliateId);
                        sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
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
                        sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb = new StringBuilder("Affiliate,CampaignName,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                    sb.AppendLine();

                    foreach (var item in list)
                    {
                        sb.AppendFormat("{0},", item.AffiliateId);
                        sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
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
                        sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));
                        sb.AppendLine();
                    }
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_offer.csv");
            }

        }

        public ActionResult Daily(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var model = new DailyViewModel();

            try
            {
                model.StatisticsEnum = (StatisticsEnum)dataview;

            }
            catch
            {

                model.StatisticsEnum =
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

            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {
                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.CustomerAffiliates = from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                 join s in af on a.AffiliateId equals s into sa
                                                 from s in sa.DefaultIfEmpty()
                                                 select new SelectListItem
                                                 {
                                                     Value = a.AffiliateId.ToString(),
                                                     Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                     Selected = s != 0
                                                 };

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    });

                }

                ViewBag.ShowAll = showall;

                ViewBag.SelectedAffiliateId = af != null;

            }
            else
            {
                af = new int[] { up.AffiliateId.Value };
            }



            if (af != null && af.Length == 0)
                af = null;
            string affs = af == null ? null : string.Join(",", af);

            ViewBag.AffiliateCountries = repo.GetCountries();
            ViewBag.SelectedCountryId = ct;
            //ViewBag.SelectedCountry = ctfilter;

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //var offset = tzi.BaseUtcOffset;

            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.BaseUtcOffset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.BaseUtcOffset).UtcDateTime;

            //DateTime ufdate = TimeZoneInfo.ConvertTimeToUtc(FromDate.Value);
            //DateTime utdate = TimeZoneInfo.ConvertTimeToUtc(ToDate.Value);

            //DateTimeOffset dateAndOffset = new DateTimeOffset(fdate, offset);
            //DateTime utcfrom = dateAndOffset.UtcDateTime;
            //DateTime utcto = new DateTimeOffset(tdate, offset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;
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

                return View(model);
            }

            else
            {

                string ctfilter = ct.HasValue ?
                repo.GetCountries()
                .Where(c => c.Id == ct.Value)
                .Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

                var countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);

                //// daily clicks
                //var dailyclicks = from c in repo.ClickQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
                //                  group c by new
                //                  {
                //                      Date = TimeZoneInfo.ConvertTimeFromUtc(c.ClickDate, tzi).ToString("MM/dd/yyyy"),
                //                      Country = countrycheck ? c.Country ?? "" : "",
                //                  } into dc
                //                  select new
                //                  {
                //                      Date = dc.Key.Date,
                //                      Country = dc.Key.Country,
                //                      Clicks = dc.Count(),
                //                      Cost = dc.Sum(c => c.Cost),
                //                      Revenue = dc.Sum(c => c.Revenue),
                //                  };

                //// daily conversions
                //var dailyconversions = from c in repo.ConversionQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
                //                       group c by new
                //                       {
                //                           Date = TimeZoneInfo.ConvertTimeFromUtc(c.ConversionDate, tzi).ToString("MM/dd/yyyy"),
                //                           Country = countrycheck ? c.Country ?? "" : "",
                //                       } into dc
                //                       select new
                //                       {
                //                           Date = dc.Key.Date,
                //                           Country = dc.Key.Country,
                //                           Conversions = dc.Count(),
                //                           Cost = dc.Sum(c => c.Cost),
                //                           Revenue = dc.Sum(c => c.Revenue),
                //                       };

                //// daily impressions
                //var dailyimpressions = from c in repo.ImpressionQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
                //                       group c by new
                //                       {
                //                           Date = TimeZoneInfo.ConvertTimeFromUtc(c.ImpressionDate, tzi).ToString("MM/dd/yyyy"),
                //                           Country = countrycheck ? c.Country ?? "" : "",
                //                       } into dc
                //                       select new
                //                       {
                //                           Date = dc.Key.Date,
                //                           Country = dc.Key.Country,
                //                           Impressions = dc.Count(),
                //                           Cost = dc.Sum(c => c.Cost),
                //                           Revenue = dc.Sum(c => c.Revenue),
                //                       };

                ///*================================================================================================================*/

                //var days1 = dailyclicks.Select(c => new { D = c.Date, C = c.Country })
                //   .Union(dailyconversions.Select(c => new { D = c.Date, C = c.Country }))
                //   .Union(dailyimpressions.Select(c => new { D = c.Date, C = c.Country }));


                //var report = from dct in days1

                //             join c in dailyclicks on dct equals
                //               new { D = c.Date, C = c.Country } into dc
                //             from c in dc.DefaultIfEmpty()

                //             join co in dailyconversions on dct equals
                //               new { D = co.Date, C = co.Country } into dco
                //             from co in dco.DefaultIfEmpty()

                //             join i in dailyimpressions on dct equals
                //               new { D = i.Date, C = i.Country } into di
                //             from i in di.DefaultIfEmpty()

                //             select new DailyView
                //             {
                //                 Date = dct.D,
                //                 Country = dct.C,
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

                var report = repo.RunQuery<DailyView>("EXEC [DailyRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                                up.CustomerId, ufdate, utdate, tzi.BaseUtcOffset.Hours, up.UserId, affs, cp, ctfilter, countrycheck);

                var sb = new StringBuilder("Date,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                sb.AppendLine();
                foreach (var item in report)
                {
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
                    sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));
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
                    ViewBag.CustomerAffiliates = from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                 join s in af on a.AffiliateId equals s into sa
                                                 from s in sa.DefaultIfEmpty()
                                                 select new SelectListItem
                                                 {
                                                     Value = a.AffiliateId.ToString(),
                                                     Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                     Selected = s != 0
                                                 };



                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    });

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                af = new int[] { up.AffiliateId.Value };
            }

            string affs = af == null || af.Length == 0 ? null : string.Join(",", af);

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            var offset = tzi.BaseUtcOffset;
            DateTime ufdate = new DateTimeOffset(fdate, offset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(tdate, offset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;
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
                                up.CustomerId, ufdate, utdate, tzi.BaseUtcOffset.Hours, up.UserId, affs, cp, ctfilter, countrycheck);

                var sb = new StringBuilder("Name,Campaign,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                sb.AppendLine();

                foreach (var item in report)
                {
                    sb.AppendFormat("{0} - {1},", item.BannerId, item.Name);
                    // CampaignName
                    sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
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
                    sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));
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

            // set the fromdate using the passed timezone or the customertimezone or the localtimezone
            DateTime fromdate = FromDate ?? DateTime.Today;

            // find user timezone
            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            fromdate = new DateTime(fromdate.Ticks, DateTimeKind.Unspecified);
            // get the offset
            var offset = tzi.BaseUtcOffset;
            // set the offset to the date and return the utc time
            var ufdate = new DateTimeOffset(fromdate, offset).UtcDateTime;

            ViewBag.fdate = fromdate;
            ViewBag.FromDate = fromdate.ToString("MM/dd/yyyy");
            ViewBag.TimeZone = tzi.Id;

            HourlyViewModel model = new HourlyViewModel();

            try
            {
                model.StatisticsEnum = (StatisticsEnum)dataview;
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
            var countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);

            // setting some viewbag variables            
            ViewBag.SelectedCampaignId = cp;
            ViewBag.IsAdmin = !up.AffiliateId.HasValue;
            ViewBag.CustomerCampaigns = repo.GetUserCampaigns(up).Where(c => c.Status == Status.Active).AsEnumerable().Select(c => new SelectListItem
            {
                Value = c.CampaignId.ToString(),
                Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName)
            });

            //string affs = null;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {

                if (af != null)
                {
                    ViewBag.CustomerAffiliates = from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                 join s in af on a.AffiliateId equals s into sa
                                                 from s in sa.DefaultIfEmpty()
                                                 select new SelectListItem
                                                 {
                                                     Value = a.AffiliateId.ToString(),
                                                     Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                     Selected = s != 0
                                                 };

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
                    });

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                //af = up.AffiliateId; // set the af to this af
                //affs = up.AffiliateId.ToString();

                af = new int[] { up.AffiliateId.Value };
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


                return View(model);
            }
            else
            {

                string ctfilter = ct.HasValue ?
                repo.GetCountries()
                .Where(c => c.Id == ct.Value)
                .Select(c => c.CountryAbbreviation).FirstOrDefault()
                : null;

                //var hourlyclicks = from c in repo.ClickQuery(utctime, utctime.AddDays(1), up, af, cp, ctfilter).AsEnumerable()
                //                   group c by new
                //                   {
                //                       Hour = TimeZoneInfo.ConvertTimeFromUtc(c.ClickDate, tzi).Hour,
                //                       Country = countrycheck ? c.Country ?? "" : "",
                //                   } into dc

                //                   select new
                //                   {
                //                       Hour = dc.Key.Hour,
                //                       Country = dc.Key.Country,
                //                       Clicks = dc.Count(),
                //                       Cost = dc.Sum(c => c.Cost),
                //                       Revenue = dc.Sum(c => c.Revenue),
                //                   };

                //var hourlyconversions = from c in repo.ConversionQuery(utctime, utctime.AddDays(1), up, af, cp, ctfilter).AsEnumerable()
                //                        group c by new
                //                        {
                //                            Hour = TimeZoneInfo.ConvertTimeFromUtc(c.ConversionDate, tzi).Hour,
                //                            Country = countrycheck ? c.Country ?? "" : "",
                //                        } into dc

                //                        select new
                //                        {
                //                            Hour = dc.Key.Hour,
                //                            Country = dc.Key.Country,
                //                            Conversions = dc.Count(),
                //                            Cost = dc.Sum(c => c.Cost),
                //                            Revenue = dc.Sum(c => c.Revenue),
                //                        };

                //var hourlyimpressions = from c in repo.ImpressionQuery(utctime, utctime.AddDays(1), up, af, cp, ctfilter).AsEnumerable()
                //                        group c by new
                //                        {
                //                            Hour = TimeZoneInfo.ConvertTimeFromUtc(c.ImpressionDate, tzi).Hour,
                //                            Country = countrycheck ? c.Country ?? "" : "",
                //                        } into dc

                //                        select new
                //                        {
                //                            Hour = dc.Key.Hour,
                //                            Country = dc.Key.Country,
                //                            Impressions = dc.Count(),
                //                            Cost = dc.Sum(c => c.Cost),
                //                            Revenue = dc.Sum(c => c.Revenue),
                //                        };



                //var countries = hourlyclicks.Select(c => new { D = c.Hour, C = c.Country })
                //    .Union(hourlyconversions.Select(c => new { D = c.Hour, C = c.Country }))
                //    .Union(hourlyimpressions.Select(c => new { D = c.Hour, C = c.Country }));

                //var hourlyreport =
                //                    from hc in countries

                //                    join c in hourlyclicks on hc equals
                //                     new { D = c.Hour, C = c.Country } into dc
                //                    from c in dc.DefaultIfEmpty()

                //                    join co in hourlyconversions on hc equals
                //                     new { D = co.Hour, C = co.Country } into dco
                //                    from co in dco.DefaultIfEmpty()

                //                    join i in hourlyimpressions on hc equals
                //                     new { D = i.Hour, C = i.Country } into di
                //                    from i in di.DefaultIfEmpty()
                //                    orderby hc.D
                //                    select new HourlyView
                //                    {
                //                        Hour = hc.D,
                //                        Country = hc.C,
                //                        Clicks = c == null ? 0 : c.Clicks,
                //                        Conversions = co == null ? 0 : co.Conversions,
                //                        Impressions = i == null ? 0 : i.Impressions,
                //                        Cost = (c == null ? 0 : c.Cost)
                //                              + (co == null ? 0 : co.Cost)
                //                              + (i == null ? 0 : i.Cost),
                //                        Revenue = (c == null ? 0 : c.Revenue)
                //                              + (co == null ? 0 : co.Revenue)
                //                              + (i == null ? 0 : i.Revenue),

                //                    };

                var list = repo.RunQuery<HourlyView>("EXEC [HourlyRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                                up.CustomerId, ufdate, ufdate.AddDays(1).AddMilliseconds(-1), tzi.BaseUtcOffset.Hours, up.UserId, affs, cp, ctfilter, countrycheck);


                //var sb = new StringBuilder("Date,Hour,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                var sb = new StringBuilder();

                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Date) ? ",Date" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Year) ? ",Year" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Month) ? ",Month" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Day) ? ",Day" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Hour) ? ",Hour" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Impressions) ? ",Impressions" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Clicks) ? ",Clicks" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Conversions) ? ",Conversions" : string.Empty);

                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.CR) ? ",CR" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Cost) ? ",Cost" : string.Empty);

                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.CPC) ? ",CPC" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Revenue) ? ",Revenue" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.RPC) ? ",RPC" : string.Empty);
                sb.Append(model.StatisticsEnum.HasFlag(StatisticsEnum.Profit) ? ",Profit" : string.Empty);

                if (sb.Length > 0)
                    sb.Remove(0, 1);
                sb.AppendLine();

                foreach (var item in list)
                {
                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Date))
                        sb.AppendFormat("{0},", fromdate.ToString("MM/dd/yyyy"));

                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Year))
                        sb.AppendFormat("{0},", fromdate.Year);

                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Month))
                        sb.AppendFormat("{0},", fromdate.Month);

                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Day))
                        sb.AppendFormat("{0},", fromdate.Day);

                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Hour))
                        sb.AppendFormat("{0},", item.Hour);

                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Impressions))
                        sb.AppendFormat("{0},", item.Impressions);

                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Clicks))
                        sb.AppendFormat("{0},", item.Clicks);

                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Conversions))
                        sb.AppendFormat("{0},", item.Conversions);

                    // conv rate
                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.CR))
                        sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));

                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Cost))
                        sb.AppendFormat("${0},", item.Cost.ToString("F2"));

                    // cpc
                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.CPC))
                        sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));

                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Revenue))
                        sb.AppendFormat("${0},", item.Revenue.ToString("F2"));

                    // rpc
                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.RPC))
                        sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));

                    // profit
                    if (model.StatisticsEnum.HasFlag(StatisticsEnum.Profit))
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

        public ActionResult Traffic(DateTime? FromDate, DateTime? ToDate, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, int[] af = null, int? ct = null, int state = 0, bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

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
                    ViewBag.CustomerAffiliates = from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                 join s in af on a.AffiliateId equals s into sa
                                                 from s in sa.DefaultIfEmpty()
                                                 select new SelectListItem
                                                 {
                                                     Value = a.AffiliateId.ToString(),
                                                     Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                     Selected = s != 0
                                                 };

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    });

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                af = new int[] { up.AffiliateId.Value };
            }

            string affs = af == null || af.Length == 0 ? null : string.Join(",", af);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime ufdate = TimeZoneInfo.ConvertTimeToUtc(fdate, tzi);
            //DateTime utdate = TimeZoneInfo.ConvertTimeToUtc(tdate, tzi);
            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.BaseUtcOffset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.BaseUtcOffset).UtcDateTime;

            ViewBag.TimeZone = tzi.Id;
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
                string ctfilter = ct.HasValue ?
              repo.GetCountries()
              .Where(c => c.Id == ct.Value)
              .Select(c => c.CountryAbbreviation).FirstOrDefault()
              : null;

                var countrycheck = model.StatisticsEnum.HasFlag(StatisticsEnum.Country);

                //var tclicks = from c in repo.ClickQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
                //              group c by new
                //              {
                //                  URLPreviewId = c.URLPreviewId,
                //                  AffiliateId = c.AffiliateId,
                //                  CampaignId = c.CampaignId,
                //                  Country = countrycheck ? c.Country ?? "" : "",
                //              } into dc
                //              select new
                //              {
                //                  URLPreviewId = dc.Key.URLPreviewId,
                //                  AffiliateId = dc.Key.AffiliateId,
                //                  CampaignId = dc.Key.CampaignId,
                //                  Country = dc.Key.Country,

                //                  Clicks = dc.Count(),
                //                  Cost = dc.Sum(c => c.Cost),
                //                  Revenue = dc.Sum(c => c.Revenue),
                //              };

                //var tconversions = from c in repo.ConversionQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
                //                   group c by new
                //                   {
                //                       URLPreviewId = repo.Clicks().Single(k => k.ClickId == c.ClickId).URLPreviewId,
                //                       AffiliateId = c.AffiliateId,
                //                       CampaignId = c.CampaignId,
                //                       Country = countrycheck ? c.Country ?? "" : "",
                //                   } into dc
                //                   select new
                //                   {
                //                       URLPreviewId = dc.Key.URLPreviewId,
                //                       AffiliateId = dc.Key.AffiliateId,
                //                       CampaignId = dc.Key.CampaignId,
                //                       Country = dc.Key.Country,

                //                       Conversions = dc.Count(),
                //                       Cost = dc.Sum(c => c.Cost),
                //                       Revenue = dc.Sum(c => c.Revenue),
                //                   };

                //var timpressions = from c in repo.ImpressionQuery(ufdate, utdate, up, af, cp, ctfilter).AsEnumerable()
                //                   group c by new
                //                   {
                //                       URLPreviewId = c.URLPreviewId,
                //                       AffiliateId = c.AffiliateId,
                //                       CampaignId = c.CampaignId,
                //                       Country = countrycheck ? c.Country ?? "" : "",
                //                   } into dc
                //                   select new
                //                   {
                //                       URLPreviewId = dc.Key.URLPreviewId,
                //                       AffiliateId = dc.Key.AffiliateId,
                //                       CampaignId = dc.Key.CampaignId,
                //                       Country = dc.Key.Country,

                //                       Impressions = dc.Count(),
                //                       Cost = dc.Sum(c => c.Cost),
                //                       Revenue = dc.Sum(c => c.Revenue),
                //                   };

                //var union = tclicks.Select(c => new { AffiliateId = c.AffiliateId, CampaignId = c.CampaignId, Country = c.Country, URLPreviewId = c.URLPreviewId })
                //  .Union(tconversions.Select(c => new { AffiliateId = c.AffiliateId, CampaignId = c.CampaignId, Country = c.Country, URLPreviewId = c.URLPreviewId }))
                //  .Union(timpressions.Select(c => new { AffiliateId = c.AffiliateId, CampaignId = c.CampaignId, Country = c.Country, URLPreviewId = c.URLPreviewId }));

                //var su = from u in union
                //         join ca in repo.Campaigns().Where(c => c.CustomerId == up.CustomerId) on u.CampaignId equals ca.CampaignId
                //         join _af in repo.Affiliates().Where(c => c.CustomerId == up.CustomerId) on u.AffiliateId equals _af.AffiliateId

                //         select new
                //         {
                //             Union = u,
                //             CampaignName = ca.CampaignName,
                //             Company = _af.Company,
                //             Url = repo.Urls().Single(r => r.CampaignId == ca.Id && r.PreviewId == u.URLPreviewId).PreviewUrl,
                //         };

                //var report = from u in su

                //             join c in tclicks on u.Union equals
                //             new { AffiliateId = c.AffiliateId, CampaignId = c.CampaignId, Country = c.Country, URLPreviewId = c.URLPreviewId } into dc
                //             from c in dc.DefaultIfEmpty()

                //             join co in tconversions on u.Union equals
                //             new { AffiliateId = co.AffiliateId, CampaignId = co.CampaignId, Country = co.Country, URLPreviewId = co.URLPreviewId } into dco
                //             from co in dco.DefaultIfEmpty()

                //             join i in timpressions on u.Union equals
                //             new { AffiliateId = i.AffiliateId, CampaignId = i.CampaignId, Country = i.Country, URLPreviewId = i.URLPreviewId } into di
                //             from i in di.DefaultIfEmpty()

                //             select new TrafficView
                //             {
                //                 AffiliateId = u.Union.AffiliateId,
                //                 UrlPreviewId = u.Union.URLPreviewId,
                //                 Country = u.Union.Country,
                //                 CampaignId = u.Union.CampaignId,

                //                 Company = u.Company,
                //                 CampaignName = u.CampaignName,
                //                 OfferUrl = u.Url,

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

                var report = repo.RunQuery<TrafficView>("EXEC [TrafficRpt] {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                                    up.CustomerId, ufdate, utdate, tzi.BaseUtcOffset.Hours, up.UserId, affs, cp, ctfilter, countrycheck);

                var sb = new StringBuilder("Campaign,Affiliate,OfferUrl,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit");
                sb.AppendLine();

                foreach (var item in report)
                {
                    // offer
                    sb.AppendFormat("{0} - {1},", item.CampaignId, item.CampaignName);
                    // affiliate
                    sb.AppendFormat("{0} - {1},", item.AffiliateId, item.Company);
                    // OfferUrl
                    sb.AppendFormat("{0},", string.IsNullOrEmpty(item.OfferUrl) ? string.Empty : item.OfferUrl);
                    sb.AppendFormat("{0},", item.Clicks);
                    sb.AppendFormat("{0},", item.Conversions);
                    // conv rate
                    sb.AppendFormat("{0}%,", item.Clicks == 0 ? (item.Conversions * 100) : (((double)item.Conversions / (double)item.Clicks) * 100));
                    // cost
                    sb.AppendFormat("${0},", item.Cost);
                    // cpc
                    sb.AppendFormat("${0},", item.Clicks == 0 ? item.Cost.ToString("F2") : (item.Cost / item.Clicks).ToString("F2"));
                    // revenue
                    sb.AppendFormat("${0},", item.Revenue);
                    // rpc
                    sb.AppendFormat("${0},", item.Clicks == 0 ? item.Revenue.ToString("F2") : (item.Revenue / item.Clicks).ToString("F2"));
                    // profit
                    sb.AppendFormat("${0}", (item.Revenue - item.Cost).ToString("F2"));
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

        public ActionResult CTR(DateTime? FromDate, DateTime? ToDate, string timezone, string viewdata, int? cp, int[] af, int? ct, int state = 0, bool showall = true)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            CPAHelper.SetTimeBasedonVD(ref viewdata, ref FromDate, ref ToDate, up.CustomerId, timezone);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            //DateTime fdate = new DateTime(FromDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime tdate = new DateTime(ToDate.Value.Ticks, DateTimeKind.Unspecified);
            //DateTime ufdate = TimeZoneInfo.ConvertTimeToUtc(fdate, tzi);
            //DateTime utdate = TimeZoneInfo.ConvertTimeToUtc(tdate, tzi);

            DateTime ufdate = new DateTimeOffset(FromDate.Value.Ticks, tzi.BaseUtcOffset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(ToDate.Value.Ticks, tzi.BaseUtcOffset).UtcDateTime;


            ViewBag.TimeZone = tzi.Id;
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

            //string affs = null;
            ViewBag.SelectedAffiliateId = false;
            if (!up.AffiliateId.HasValue) // if is admin
            {
                // do not filter affiliates by active
                if (af != null)
                {
                    ViewBag.CustomerAffiliates = from a in repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable()
                                                 join s in af on a.AffiliateId equals s into sa
                                                 from s in sa.DefaultIfEmpty()
                                                 select new SelectListItem
                                                 {
                                                     Value = a.AffiliateId.ToString(),
                                                     Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company),
                                                     Selected = s != 0
                                                 };

                }
                else
                {
                    ViewBag.CustomerAffiliates = repo.GetCustomerAffiliates(up.CustomerId).AsEnumerable().Select(a => new SelectListItem
                    {
                        Value = a.AffiliateId.ToString(),
                        Text = string.Format("{1} (Affiliate ID: {0})", a.AffiliateId, a.Company)
                    });

                }

                ViewBag.ShowAll = showall;
                ViewBag.AffiliateCountries = repo.GetCountries();
                ViewBag.SelectedAffiliateId = af != null;
                ViewBag.SelectedCountryId = ct;
            }
            else
            {
                //affs = up.AffiliateId.ToString();
                af = new int[] { up.AffiliateId.Value };
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
                //var clicks = from c in repo.ClickQuery(ufdate, utdate, up, af, cp, null).AsEnumerable()
                //             group c by new
                //             {
                //                 URLPreviewId = c.URLPreviewId,
                //                 CampaignId = c.CampaignId,
                //             } into dc
                //             select new
                //             {
                //                 URLPreviewId = dc.Key.URLPreviewId,
                //                 CampaignId = dc.Key.CampaignId,

                //                 Clicks = dc.Count(),
                //                 Cost = dc.Sum(c => c.Cost),
                //                 Revenue = dc.Sum(c => c.Revenue),
                //             };

                //var report = (from u in repo.Urls().Where(u => u.Campaign.CustomerId == up.CustomerId).AsEnumerable()
                //             join c in clicks on new { a = u.Campaign.CampaignId, b = u.PreviewId } equals new { a = c.CampaignId, b = c.URLPreviewId } //into dc
                //             //from c in dc.DefaultIfEmpty()
                //             select new CTRView
                //             {
                //                 CampaignId = u.Campaign.CampaignId,
                //                 CampaignName = u.Campaign.CampaignName,
                //                 //Clicks = c == null ? 0 : c.Clicks,
                //                 Clicks = c.Clicks,
                //                 OfferUrl = u.OfferUrl,
                //                 PreviewId = u.PreviewId,
                //                 ParentURLId = u.ParentURLId,
                //                 Id = u.Id,
                //             }).ToList();

                var report = repo.RunQuery<CTRView>("EXEC [CTRReport] {0}, {1}, {2}, {3}, {4}, {5}, {6}",
                               up.CustomerId, ufdate, utdate, up.UserId, affs, cp, null).ToList();

                var dic = new Dictionary<int, Holder>();

                // fill the dictionary
                foreach (var item in report)
                {
                    dic.Add(item.Id, new Holder
                    {
                        Clicks = item.Clicks,
                        ParentURLId = item.ParentURLId,
                        CampaignId = item.CampaignId,
                        PreviewId = item.PreviewId
                    });
                }

                myDelegate ctr = null;

                ctr = delegate(int? actionid)
                {
                    if (actionid.HasValue && dic.Keys.Contains(actionid.Value))
                        return dic[actionid.Value].Clicks + ctr(dic[actionid.Value].ParentURLId);
                    return 0;
                };

                // calculate ctr
                foreach (var item in report)
                {
                    int previous = ctr(item.ParentURLId);
                    item.CTR = previous == 0 ? item.Clicks : item.Clicks / (double)previous;
                }

                var sb = new StringBuilder();
                sb.AppendLine("Campaign,ActionId,ParentActionId,OfferUrl,Clicks,CTR");

                foreach (var item in report)
                {
                    sb.AppendFormat("{0},", item.CampaignId);
                    sb.AppendFormat("{0},", item.Id);
                    sb.AppendFormat("{0},", item.ParentURLId);
                    sb.AppendFormat("{0},", item.OfferUrl);
                    sb.AppendFormat("{0},", item.Clicks);
                    sb.AppendFormat("{0},", item.CTR);
                    sb.AppendLine();
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report_ctr.csv");
            }

            //return View(model);
            return View();
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
