using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Models;
using System.Text;

namespace CpaTicker.Areas.admin.Controllers
{
    [Authorize]
    public class SignalRController : BaseController
    {
        private ICpaTickerRepository repo;
        public SignalRController()
        {
            this.repo = new EFCpatickerRepository();
        }

        public ActionResult Index()
        {
            return View();
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
    }
}
