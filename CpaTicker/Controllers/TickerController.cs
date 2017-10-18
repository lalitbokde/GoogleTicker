using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Helper;
using WebMatrix.WebData;
using Newtonsoft.Json;

namespace CpaTicker.Controllers
{
    [Authorize]
    public class TickerController : Controller
    {
        private CpaTickerDb db = new CpaTickerDb();

        private ICpaTickerRepository repo;

        public TickerController()
        {
            this.repo = new EFCpatickerRepository();
        }
        //
        // GET: /Admin/Ticker/

        private string BuildTicker(bool isAPI, int customerid, DateTime? dteFromDate, DateTime? dteToDate, int offset, int tickerid, int affiliateid)
        {
            StringBuilder sb = new StringBuilder();
            List<TickerItem> Ticker = new List<TickerItem>();

            dynamic campaigns = db.Database.SqlQuery(typeof(TickerItem), "EXEC [GetTicker] {0}, {1}, {2}, {3}, {4}, {5}", customerid, dteFromDate, dteToDate, offset, tickerid, affiliateid);

            //var campaigns = from c in db.Campaigns
            //                from cl in db.Clicks.DefaultIfEmpty()
            //                from co in db.Conversions.DefaultIfEmpty()
            //                from im in db.Impressions.DefaultIfEmpty()
            //                where c.CustomerId == customerid && c.Status == Status.Active
            //                select new
            //                {
            //                    CampaignId = c.CampaignName,
            //                    Clicks = db.Clicks.Where(dcl => dcl.CampaignId == c.CampaignId && dcl.CustomerId == c.CustomerId && dcl.ClickDate >= dteFromDate).Count(),
            //                    Conversions = db.Conversions.Where(dco => dco.CampaignId == c.CampaignId && dco.CustomerId == c.CustomerId && dco.ConversionDate >= dteFromDate).Count(),
            //                    Impressions = db.Impressions.Where(dim => dim.CampaignId == c.CampaignId && dim.CustomerId == c.CustomerId && dim.ImpressionDate >= dteFromDate).Count(),
            //                    Cost =
            //                        db.Clicks.Where(dcl => dcl.CampaignId == c.CampaignId && dcl.CustomerId == c.CustomerId && dcl.ClickDate >= dteFromDate && dcl.ClickDate < dteToDate).Sum(dcl => (double?)dcl.Cost) ?? 0.0 +
            //                        db.Conversions.Where(dco => dco.CampaignId == c.CampaignId && dco.CustomerId == c.CustomerId && dco.ConversionDate >= dteFromDate && dco.ConversionDate < dteToDate).Sum(dco => (double?)dco.Cost) ?? 0.0 +
            //                        db.Impressions.Where(dim => dim.CampaignId == c.CampaignId && dim.CustomerId == c.CustomerId && dim.ImpressionDate >= dteFromDate && dim.ImpressionDate < dteToDate).Sum(dim => (double?)dim.Cost) ?? 0.0,
            //                    Revenue =
            //                        db.Clicks.Where(dcl => dcl.CampaignId == c.CampaignId && dcl.CustomerId == c.CustomerId && dcl.ClickDate >= dteFromDate && dcl.ClickDate < dteToDate).Sum(dcl => (double?)dcl.Revenue) ?? 0.0 +
            //                        db.Conversions.Where(dco => dco.CampaignId == c.CampaignId && dco.CustomerId == c.CustomerId && dco.ConversionDate >= dteFromDate && dco.ConversionDate < dteToDate).Sum(dco => (double?)dco.Revenue) ?? 0.0 +
            //                        db.Impressions.Where(dim => dim.CampaignId == c.CampaignId && dim.CustomerId == c.CustomerId && dim.ImpressionDate >= dteFromDate && dim.ImpressionDate < dteToDate).Sum(dim => (double?)dim.Revenue) ?? 0.0

            //                };



            foreach (var c in campaigns)
            {
                TickerItem tickerItem = new TickerItem();
                tickerItem.Impressions = c.Impressions;
                tickerItem.CampaignName = c.CampaignName;
                tickerItem.Clicks = c.Clicks;
                tickerItem.Conversions = c.Conversions;
                tickerItem.Cost = c.Cost;
                tickerItem.Revenue = c.Revenue;
                Ticker.Add(tickerItem);
                /*
                sb.Append("<span class=\"c\">Campaign " + c.CampaignId + ":</span>\n");
                sb.Append("<span class=\"cl\">" + c.Clicks.ToString() + " <span class=\"l\">Clicks</span></span>\n");
                sb.Append("<span class=\"co\">" + c.Conversions.ToString() + " <span class=\"l\">Conversions</span></span>\n");
                sb.Append("<span class=\"d\">|</span>\n");
                 */

            }


            //string ticker = sb.ToString();
            // clean up html span tags
            /*            if (isAPI)
                        {
                            ticker = Regex.Replace(ticker, "<span[^>]+>", "");
                            ticker = ticker.Replace("</span>", "");
                        }*/
            return JsonConvert.SerializeObject(Ticker);
        }

        public ActionResult Update(DateTime? fromdate = null, DateTime? todate = null, int offset = -5, int tickerid = 0)
        {
            if (fromdate == null)
                fromdate = DateTime.Today;
            if (todate == null)
                todate = DateTime.Today.AddDays(1);

            var up = repo.GetCurrentUser();

            return Content(BuildTicker(false, up.CustomerId, fromdate, todate, offset, tickerid, up.AffiliateId ?? 0));
        }

        /// <summary>
        /// GET: campaign details
        /// </summary>
        /// <param name="api">user’s api key (required)</param>
        /// <param name="fromdate">campaign details from datetime(not required)</param>
        /// <param name="todate">campaign details until datetime(not required)</param>
        /// <param name="offset">user timzone offset from UTC(not required)</param>
        /// <param name="affiliateid">user's affiliate id(not required)</param>
        /// <param name="subid"> click's sub value(not required)</param>
        /// <returns>
        /// list of campaign detail, 
        /// where
        ///     CampaignName:   campaign name
        ///     Clicks:         total click
        ///     Impressions:    total impression
        ///     Conversions:    total conversion
        ///     OldClicks:      total old click
        ///     OldImpressions: total old impression
        ///     OldConversions: total old conversion 
        ///     Cost:           total campaign cost
        ///     Revenue:        total campaign revenue
        /// </returns>
        [AllowAnonymous]
        public ActionResult API(string api = "", DateTime? fromdate = null, DateTime? todate = null, int? offset = null, int? affiliateid = null, string subid = null)
        {

            if (string.IsNullOrEmpty(api))
            {
                return Content("");
            }
            var user = repo.GetUserFromAPIKey(api);

            if (user == null || !user.OrderId.HasValue)
            {
                return Content("");
            }

            var list = new TickerHelper(repo).TickerItems(fromdate, todate, offset, affiliateid, subid, user);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET: campaign details
        /// </summary>
        /// <param name="api">user’s api key (required)</param>
        /// <param name="fromdate">campaign details from datetime(not required)</param>
        /// <param name="todate">campaign details until datetime(not required)</param>
        /// <param name="offset">user timzone offset from UTC(not required)</param>
        /// <param name="affiliateid">user's affiliate id(not required)</param>
        /// <param name="subid"> click's sub value(not required)</param>
        /// <returns>
        /// list of campaign detail, 
        /// where
        ///     CampaignName:   campaign name
        ///     Company:        company name
        ///     AffiliateId:    affiliate id
        ///     Clicks:         total click
        ///     Impressions:    total impression
        ///     Conversions:    total conversion
        ///     OldClicks:      total old click
        ///     OldImpressions: total old impression
        ///     OldConversions: total conversion 
        ///     Cost:           total campaign cost
        ///     Revenue:        total campaign revenue
        /// </returns>
        [AllowAnonymous]
        public ActionResult API_v2(string api = "", DateTime? fromdate = null, DateTime? todate = null, int? offset = null, int? tickerid = null, int? affiliateid = null, string subid = null)
        {
            if (string.IsNullOrEmpty(api))
            {
                return Content("");
            }
            var user = repo.GetUserFromAPIKey(api);

            if (user == null || !user.OrderId.HasValue)
            {
                return Content("");
            }

            if (user.AffiliateId.HasValue)
            {
                affiliateid = user.AffiliateId;
            }

            fromdate = fromdate ?? DateTime.Today;
            todate = todate ?? DateTime.Today.AddDays(1);

            var fdate = new DateTime(fromdate.Value.Ticks, DateTimeKind.Unspecified);
            var tdate = new DateTime(todate.Value.Ticks, DateTimeKind.Unspecified);
            TimeSpan toffset;
            if (offset.HasValue)
            {
                toffset = new TimeSpan(offset.Value, 0, 0);
            }
            else
            {
                var customer = repo.GetCurrentCustomer(user.CustomerId);
                var tzi = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZone);
                //toffset = tzi.GetUtcOffset(fdate);
                toffset = tzi.GetUtcOffset(DateTime.Now);
            }

            DateTime ufdate = new DateTimeOffset(fromdate.Value.Ticks, toffset).UtcDateTime;
            DateTime utdate = new DateTimeOffset(todate.Value.Ticks, toffset).UtcDateTime;

            var list = repo.BuildTickerExt(ufdate, utdate, user.CustomerId, user.UserId, affiliateid, subid).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [AllowAnonymous]
        public ActionResult API_v21(string api = "", int backdays = 0, Format format = Format.json, int? affid = null, int offset = -5, int payout = 0)
        {
            if (string.IsNullOrEmpty(api))
            {
                return Content("");
            }
            var user = repo.GetUserFromAPIKey(api);

            if (user == null || !user.OrderId.HasValue)
            {
                return Content("");
            }

            // calculating the datetime in utc
            var fromdate = DateTime.Today.AddDays(0 - backdays);
            DateTime fdate = new DateTime(fromdate.Ticks, DateTimeKind.Unspecified);
            var customer = repo.GetCurrentCustomer(user.CustomerId);
            //var tzi = TimeZoneInfo.FindSystemTimeZoneById(customer.TimeZone);
            TimeSpan tzoffset = new TimeSpan(offset, 0, 0);
            DateTime ufdate = new DateTimeOffset(fdate, tzoffset).UtcDateTime;
            DateTime utdate = ufdate.AddDays(1);

            var cosubid = from co in repo.Conversions()
                          //join ck in repo.Clicks() on co.ClickId equals ck.ClickId
                          select new
                          {
                              co.AffiliateId,
                              co.CampaignId,
                              co.Status,
                              co.CustomerId,
                              co.ConversionDate,
                              //TODO:SUBID
                              SubId1 =co.Click.SubId1, //co.Click.SubIds.Where(s => s.SubIndex == 1).Select(s => s.SubValue).FirstOrDefault(),
                              co.Revenue,
                              co.Cost
                          };
            //var GetAffiliateOverride = db.OverrideAffiliate.Where(u => u.AffiliateID == affid && u.AffiliateID != null && u.CustomerID == customer.CustomerId).ToList();
            //decimal? Cost = 0;
            //decimal? Revenue = 0;

            //if (GetAffiliateOverride.Count > 0)
            //{
            //    int index = GetAffiliateOverride.Count - 1;
            //    if (payout == 1)
            //    {
            //        GetAffiliateOverride[index].PayoutType = PayoutType.CPA;
            //        GetAffiliateOverride[index].RevenueType = RevenueType.RPA;
            //    }
            //    else if (payout == 2)
            //    {
            //        GetAffiliateOverride[index].PayoutType = PayoutType.CPS;
            //        GetAffiliateOverride[index].RevenueType = RevenueType.RPS;
            //    }
            //    else if (payout == 3)
            //    {
            //        GetAffiliateOverride[index].PayoutType = PayoutType.CPA_CPS;
            //        GetAffiliateOverride[index].RevenueType = RevenueType.RPA_RPS;
            //    }
            //    else if (payout == 4)
            //    {
            //        GetAffiliateOverride[index].PayoutType = PayoutType.CPC;
            //        GetAffiliateOverride[index].RevenueType = RevenueType.RPC;
            //    }
            //    else if (payout == 5)
            //    {
            //        GetAffiliateOverride[index].PayoutType = PayoutType.CPM;
            //        GetAffiliateOverride[index].RevenueType = RevenueType.RPM;
            //    }
            //    Cost = repo.GetCost(GetAffiliateOverride[index].PayoutType, GetAffiliateOverride[index].Payout, GetAffiliateOverride[index].PayoutPercent, ActionType.Conversion);
            //    Revenue = repo.GetRevenue(GetAffiliateOverride[index].RevenueType, GetAffiliateOverride[index].Revenue, GetAffiliateOverride[index].RevenuePercent, ActionType.Conversion);
            //}
            var coquery = cosubid
                        .Where(c => ufdate <= c.ConversionDate && c.ConversionDate < utdate
                            && c.Status == 1
                            && c.CustomerId == user.CustomerId
                            && (!affid.HasValue || c.AffiliateId == affid.Value))
                        //TODO:SUBID
                        .GroupBy(c => new { c.SubId1, c.CampaignId })
                        .Select(c => new
                        {
                            //AffiliateId = c.Key.AffiliateId,
                            //TODO:SUBID
                            SubId1 = c.Key.SubId1,
                            CampaignId = c.Key.CampaignId,
                            Coversions = c.Count(),
                            Revenue = c.Sum(u => u.Revenue),
                            Cost = c.Sum(u => u.Cost),
                            //Revenue = (GetAffiliateOverride.Count > 0) ? c.Count() * Revenue : c.Sum(u => u.Revenue),
                            //Cost = (GetAffiliateOverride.Count > 0) ? c.Count() * Cost : c.Sum(u => u.Cost),
                        });


            var hidden = user.HiddenCampaigns.Select(h => h.CampaignId);
            //var usercampaings = repo.Campaigns().Where(c => c.CustomerId == user.CustomerId && !hidden.Contains(c.Id) && c.Status == Status.Active);

            var campaignswithclicks = from ca in repo.Campaigns().Where(c => c.CustomerId == user.CustomerId && !hidden.Contains(c.Id) && c.Status == Status.Active)
                                      //from ca in usercampaings.Where(c => c.Status == Status.Active) // .AsEnumerable()
                                      join ck in repo.Clicks().Where(c => ufdate <= c.ClickDate
                                          && c.ClickDate < utdate
                                          && (!affid.HasValue || c.AffiliateId == affid.Value)
                                          && c.CustomerId == user.CustomerId)

                                           on ca.CampaignId equals ck.CampaignId
                                      //where 
                                      group ca by new { ca.CampaignId, ca.CampaignName } into gcamp
                                      select new
                                      {
                                          CampaignId = gcamp.Key.CampaignId,
                                          CampaignName = gcamp.Key.CampaignName,
                                      };

            string dateformat = fromdate.ToString("yyyy-MM-dd");
            var list = from ca in campaignswithclicks.AsEnumerable()
                       //from ca in repo.GetUserCampaigns(user).Where(c => c.Status == Status.Active).AsEnumerable()
                       //from af in repo.GetCustomerActiveAffiliates(user.CustomerId) //on ca.CustomerId equals af.CustomerId
                       //join co in coquery on new { a = ca.CampaignId, b = af.AffiliateId } equals new { a = co.CampaignId, b = co.AffiliateId } into ac
                       join co in coquery on ca.CampaignId equals co.CampaignId into ac
                       from co in ac.DefaultIfEmpty()
                       select new API21Model
                       {
                           Date = dateformat,
                           Campaign = ca.CampaignName,
                           //Affiliate = af.Company,
                           //TODO:SUBID
                           SubId1 = co == null || co.SubId1 == null ? "" : co.SubId1,
                           Conversions = co == null ? 0 : co.Coversions,
                           Revenue = co == null ? 0 : co.Revenue,
                           Cost = co == null ? 0 : co.Cost,
                       };

            switch (format)
            {
                case Format.json:
                    if(payout==0)
                    {
                        var result = list.Select(u => new { Date = u.Date, Campaign = u.Campaign, SubId1 = u.SubId1, Conversions = u.Conversions });
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if(payout==1)
                    {
                        var result = list.Select(u => new { Date = u.Date, Campaign = u.Campaign, SubId1 = u.SubId1, Cost = u.Cost });
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    return Json(list, JsonRequestBehavior.AllowGet);
                case Format.xml:
                     if(payout==0)
                    {
                        var result = list.Select(u => new { Date = u.Date, Campaign = u.Campaign, SubId1 = u.SubId1, Conversions = u.Conversions });
                        return new XmlResult(result.ToList());
                    }
                    if(payout==1)
                    {
                        var result = list.Select(u => new { Date = u.Date, Campaign = u.Campaign, SubId1 = u.SubId1, Cost = u.Cost });
                        return new XmlResult(result.ToList());
                    }
                    return new XmlResult(list.ToList());
                case Format.csv:
                    var sb = new StringBuilder("Date,Campaign,SubId1");
                    if (payout == 0)
                    { sb.Append(",Conversions"); }
                    if (payout == 1)
                    { sb.Append(",Payout"); }
                    sb.AppendLine();

                    foreach (var item in list)
                    {
                        if (item.Conversions != 0)
                        {
                            //sb.AppendFormat("{0},{1},{2},", item.Date, item.Campaign, item.SubId1);
                            sb.AppendFormat("{0},\"{1}\",{2}", item.Date, item.Campaign, item.SubId1);

                            if (payout == 0)
                            { sb.AppendFormat(",{0}", item.Conversions); }
                            if (payout == 1)
                            { sb.AppendFormat(",{0}", item.Cost); }
                            //sb.AppendFormat("{0},{1},{2}", item.Conversions, item.Cost, item.Revenue);
                            sb.AppendLine();
                        }
                    }

                    //return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "api21model.csv");
                    return Content("<pre>" + sb.ToString() + "</pre>");

            }

            return Content("Incorrect format");

        }

        public class API21Model
        {
            public string Date { get; set; }
            public string Campaign { get; set; }
            //public string Affiliate { get; set; }
            public int Conversions { get; set; }
            public string SubId1 { get; set; }
            public decimal? Revenue { get; set; }
            public decimal? Cost { get; set; }
        }

        public enum Format
        {
            json = 1,
            xml,
            csv,
        }

        [AllowAnonymous]
        public ActionResult Affiliates(string api)
        {
            var user = repo.GetUserFromAPIKey(api);
            if (user == null)
            {
                return Content("");
            }
            var affiliates = repo.GetCustomerAffiliates(user.CustomerId).Select(a => new { a.AffiliateId, /*Name = */a.Company });
            return Json(affiliates, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            repo.Dispose();
            base.Dispose(disposing);
        }

    }
}
