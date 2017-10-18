using System.Web.Http.Description;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CpaTicker.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SignalRController : ApiController
    {
        public readonly ICpaTickerRepository repo = null;

        public SignalRController()
        {
            this.repo = new EFCpatickerRepository();
        }

        //[HttpGet]
        //public IHttpActionResult SignalRMaster()
        //{
        //    var up = repo.GetCurrentUser();
        //    var notifydata = repo.GetNotifyHourlyRptData(up.CustomerId);
        //    return Ok(notifydata);

        //}
        [HttpGet]
        //GET -api/signalR/hourly
        public IHttpActionResult Hourly(DateTime? FromDate = null, string timezone = "",
           long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null)
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            // set the fromdate using the passed timezone or the customertimezone or the localtimezone
            DateTime fromdate = FromDate ?? DateTime.Today;

            // find user timezone
            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);

            var ufdate = new DateTimeOffset(fromdate.Ticks, tzi.GetUtcOffset(DateTime.Now)).UtcDateTime;


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

            if (up.AffiliateId.HasValue)
                af = new int[] { up.AffiliateId.Value };
            if (af != null && af.Length == 0)
                af = null;

            string affs = af == null ? null : string.Join(",", af);

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

            //var report = from hc in countries

            //             join c in hourlyclicks on hc equals
            //              new { D = c.Hour, C = c.Country } into dc
            //             from c in dc.DefaultIfEmpty()

            //             join co in hourlyconversions on hc equals
            //              new { D = co.Hour, C = co.Country } into dco
            //             from co in dco.DefaultIfEmpty()

            //             join i in hourlyimpressions on hc equals
            //              new { D = i.Hour, C = i.Country } into di
            //             from i in di.DefaultIfEmpty()
            //             orderby hc.D
            //             select new HourlyView
            //             {
            //                 Hour = hc.D,
            //                 Country = hc.C,
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
            //var pagename = "Hourly";
            //var notifydata = repo.GetNotifyHourlyRptData(up.CustomerId, pagename);
            var report = repo.GetHourlyRptData(Convert.ToString(up.CustomerId), ufdate, ufdate.AddDays(1).AddMilliseconds(-1), Convert.ToString(tzi.GetUtcOffset(DateTime.Now).Hours), Convert.ToString(up.UserId), affs, cp, ctfilter, Convert.ToString(countrycheck));
            //                         affs,);
            //up.CustomerId, ufdate, ufdate.AddDays(1).AddMilliseconds(-1), tzi.GetUtcOffset(DateTime.Now).Hours, up.UserId, affs, cp, ctfilter, countrycheck
            // return Ok(report);
            return Ok(report);
        }
    }
}
