using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Classes.SecurityLib;
using CpaTicker.Areas.admin.Models;
using CpaTicker.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
namespace CpaTicker.Areas.signalra.Controllers
{
    [Authorize]
    public class HelperController : Controller
    {
        ICpaTickerRepository repo;

        public HelperController()
        {
            this.repo = new EFCpatickerRepository();
        }

        public ActionResult DisplaySparks()
        {
            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);
            var tzi = repo.FindTimeZoneInfo(null, customer.TimeZone);

            var count = 12;

            SparkModel sm = new SparkModel();
            sm.Clicks = new int[count];
            sm.Conversions = new int[count];
            sm.Revenues = new decimal[count];

            //var fdate = new DateTime(DateTime.Today.AddDays(1 - count).Ticks, DateTimeKind.Unspecified);
            //var tdate = new DateTime(DateTime.Today.AddDays(1).AddSeconds(-1).Ticks, DateTimeKind.Unspecified);
            //var offset = tzi.BaseUtcOffset;

            /***********************************************************************************************************************/

            var fdate = DateTime.Today.AddDays(1 - count);
            var tdate = DateTime.Today.AddDays(1).AddSeconds(-1);

            var report = repo.DailyReport(fdate, tdate, up, tzi);

            int j = 0;

            foreach (var item in report)
            {
                sm.Clicks[j] = item.Clicks;
                sm.Conversions[j] = item.Conversions;
                sm.Revenues[j] = item.Revenue;
                j++;

            }

            return PartialView("_Sparks", sm);
        }

        public ActionResult DisplayEmployeeIPList()
        {
            return PartialView("_EmployeeIPList", repo.GetEmployeeIpList());
        }

        //public ActionResult ClicksLogs(string id, long id2) // timezone, ticks
        //{
        //    var up = repo.GetCurrentUser();
        //    var customer = repo.GetCurrentCustomer(up.CustomerId);

        //    var tzi = repo.FindTimeZoneInfo(id, customer.TimeZone);
        //    var date = new DateTime(id2, DateTimeKind.Unspecified);
        //    var offset = tzi.GetUtcOffset(date);
        //    var fdate = new DateTimeOffset(date, offset).UtcDateTime;
        //    var tdate = fdate.AddHours(1).AddMilliseconds(-1);

        //    var sb = new StringBuilder("CampaignId,AffiliateId,BannerId,ClickDate,UserAgent,IPAddress,Referrer,Source,TransactionId,Country,Cost,Revenue");
        //    sb.AppendLine();

        //    var clicks = repo.ClicksLogs(id, fdate, tdate, up.CustomerId, up.AffiliateId);

        //    foreach (Click click in clicks)
        //    {
        //        sb.AppendFormat("{0},", click.CampaignId);
        //        sb.AppendFormat("{0},", click.AffiliateId);
        //        sb.AppendFormat("{0},", click.BannerId);
        //        sb.AppendFormat("{0},", click.ClickDate.ToString("MM/dd/yyyy"));
        //        sb.AppendFormat("{0},", click.UserAgent);
        //        sb.AppendFormat("{0},", click.IPAddress);
        //        sb.AppendFormat("{0},", click.Referrer);
        //        sb.AppendFormat("{0},", click.Source);
        //        sb.AppendFormat("{0},", click.TransactionId);
        //        sb.AppendFormat("{0},", click.Country);
        //        sb.AppendFormat("{0},", click.Cost.ToString("F2"));
        //        sb.AppendLine(click.Revenue.ToString("F2"));
        //    }

        //    return File(Encoding.UTF8.GetBytes(sb.ToString()), ".csv", String.Format("report_clicklog_{0}_{1}.csv", date.ToString("yyyyMMdd"), date.Hour));
        //}

        //public ActionResult GenerateLink(int? affiliateid, int campaignid, int cid, int did, int bannerid = 0, string hitid = "", int? actionid = null, string subid1 = "", string subid2 = "", string subid3 = "", string subid4 = "", string subid5 = "")
        //{
        //    if (affiliateid == null)
        //        return Content("");

        //    int customerid = repo.GetCurrentUser().CustomerId;

        //    StringBuilder url = new StringBuilder();

        //    url.AppendFormat("http://{0}/cpa/click", did == CpaTickerConfiguration.DefaultDomainId ?
        //        CPAHelper.GetDefaultDomain(customerid).DomainName : repo.GetDomain(did).DomainName);

        //    url.AppendFormat("/?affiliateid={0}&campaignid={1}&actionid={2}", affiliateid, campaignid, actionid);
        //    //url.AppendFormat("/?affiliateid={0}&actionid={1}&", affiliateid, actionid);

        //    if (bannerid != 0)
        //        url.AppendFormat("&bannerid={0}", bannerid);

        //    if (hitid != "")
        //        url.AppendFormat("&hitid={0}", HttpUtility.UrlEncode(hitid));
        //    if (subid1 != "")
        //        url.AppendFormat("&subid={0}", HttpUtility.UrlEncode(subid1));
        //    if (subid2 != "")
        //        url.AppendFormat("&subid2={0}", HttpUtility.UrlEncode(subid2));
        //    if (subid3 != "")
        //        url.AppendFormat("&subid3={0}", HttpUtility.UrlEncode(subid3));
        //    if (subid4 != "")
        //        url.AppendFormat("&subid4={0}", HttpUtility.UrlEncode(subid4));
        //    if (subid5 != "")
        //        url.AppendFormat("&subid5={0}", HttpUtility.UrlEncode(subid5));

        //    return Content(url.ToString());
        //}

        //public ActionResult GenerateConversionPixel(int campaignid, int did/*, int urlid = 0*/)
        //{
        //int customerid = repo.GetCurrentUser().CustomerId;

        //var domain = did == CpaTickerConfiguration.DefaultDomainId ?
        //    CPAHelper.GetDefaultDomain(customerid) : repo.GetDomain(did);

        //var urlparameter = urlid == 0 ? "" : "&urlid=" + urlid;

        //string url = String.Format("//{0}/cpa/conversion/?cpid={1}{2}", domain.DomainName, campaignid, urlparameter);

        //Campaign c = repo.GetCampaignById(campaignid, customerid);



        //string tracking = "";
        //switch (c.TrackingType)
        //{
        //    case TrackingType.HttpiFrame:
        //        tracking = "<iframe scrolling=\"no\" frameborder=\"0\" width=\"1\" height=\"1\" src=\"http:" + url + "\"></iframe>";
        //        break;
        //    case TrackingType.HttpsiFrame:
        //        tracking = "<iframe scrolling=\"no\" frameborder=\"0\" width=\"1\" height=\"1\" src=\"https:" + url + "\"></iframe>";
        //        break;
        //    case TrackingType.HttpImage:
        //        tracking = "<img border=\"0\" width=\"1\" height=\"1\" src=\"http:" + url + "\">";
        //        break;
        //    case TrackingType.HttpsImage:
        //        tracking = "<img border=\"0\" width=\"1\" height=\"1\" src=\"https:" + url + "\">";
        //        break;
        //    case TrackingType.ServerPostback:
        //        tracking = "https:" + url;
        //        break;
        //    default:
        //        tracking = url;
        //        break;
        //}

        //return Content(tracking);

        //    var campaign = repo.FindCampaign(campaignid);
        //    return Content(repo.TrackingCode(campaign.Actions.First(), did));
        //}

        // this is user in details view of the bannercontroller
        //public ActionResult GenerateBannerLink(int affiliateid, int campaignid, int cid, int bannerid, int did, string subid1 = "", string subid2 = "", string subid3 = "", string subid4 = "", string subid5 = "")
        //{
        //    int customerid = repo.GetCurrentUser().CustomerId;

        //    StringBuilder url = new StringBuilder();

        //    url.AppendFormat("http://{0}/cpa/view/?affiliateid={1}&campaignid={2}&bannerid={3}", 
        //        did == CpaTickerConfiguration.DefaultDomainId ? CPAHelper.GetDefaultDomain(customerid).DomainName : repo.GetDomain(did).DomainName,
        //        affiliateid, campaignid, bannerid);

        //    if (cid == customerid)
        //    {
        //        if (subid1 != "")
        //            url.AppendFormat("&subid1={0}", subid1);
        //        if (subid2 != "")
        //            url.AppendFormat("&subid2={0}", subid2);
        //        if (subid3 != "")
        //            url.AppendFormat("&subid3={0}", subid3);
        //        if (subid4 != "")
        //            url.AppendFormat("&subid4={0}", subid4);
        //        if (subid5 != "")
        //            url.AppendFormat("&subid5={0}", subid5);
        //    }
        //    return Content(url.ToString());
        //}

        public ActionResult GetCampaignBanners(int id)
        {
            var obj = repo.GetBannersByCampaign(id).Select(b => b.BannerId); // use the current customerid
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCampaignURLs(int id) // campaignid
        {
            var campaign = repo.GetCampaignById(id); // use the current customerid
            var obj = repo.GetCampaignURLs(campaign.Id).Select(b => b.PreviewId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayUserDetails()
        {
            var user = repo.GetCurrentUser();
            //ViewBag.HasAPIKey = user.OrderId.HasValue;
            //ViewBag.APIKey = user.APIKey;
            //ViewBag.Email = user.Email;
            ViewBag.User = user;

            if (user.AffiliateId.HasValue)
            {
                var affiliate = repo.GetAffiliate(user.AffiliateId.Value, user.CustomerId);
                return View("_AffiliateDetailsPartial", affiliate);
            }

            var customer = repo.GetCurrentCustomer();
            SecureCard sc = new SecureCard(customer.CreditCardData);
            ViewBag.CardNumberX = sc.CardNumberX;
            ViewBag.ExpDate = String.Format("{0}/{1}", sc.ExpiryMonth, sc.ExpiryYear);
            ViewBag.CVV = sc.CVV;
            ViewBag.CardType = sc.GetCardType;
            ViewBag.State = repo.GetStateByCode(customer.State).StateName; //ViewBag.State = customer. // CPAHelper.GetStateName(customer.State); //LimeLightCodes.GetStateCode()["US"][current.State];
            ViewBag.Country = customer.Country.Name; //db.Countries.Single(c => c.Id == current.CountryId).Name;

            return View("_CustomerDetailsPartial", customer);
        }

        public ActionResult DisplayDomainList()
        {
            var up = repo.GetCurrentUser();
            var model = repo.GetCustomerDomains(up.CustomerId);

            ViewBag.DefaultDomain = repo.GetDefaultDomain(up.CustomerId).DomainName;

            return PartialView("_DomainListPartial", model);
        }

        // settings page customer custom fields
        public ActionResult DisplayCustomFieldList()
        {
            return PartialView("_CustomFieldList", repo.GetCustomFields());
        }

        public ActionResult DisplayTopActions()
        {
            var customer = repo.GetCurrentCustomer();
            ViewBag.UserCurrentTimeZone = customer.TimeZone;
            return View("_TopActions");
        }

        // create campaign display customer custom fields
        public ActionResult DisplayCustomField()
        {
            return PartialView("_CustomField", repo.GetCustomFields());
        }

        // edit campaign displays all customer custom fields and their values
        public ActionResult DisplayCustomFieldValues(int campaignid, int customerid) // campaignid, customerid
        {
            var model = repo.GetCampaignCustomField(campaignid, customerid).ToList();
            foreach (var item in model)
            {
                var a = item.Value;
            }
            return PartialView("_EditCustomFieldValues", repo.GetCampaignCustomField(campaignid, customerid));
        }

        public ActionResult RemoveCustomField(int id)
        {
            var cf = repo.FindCustomField(id);
            if (cf == null)
            {
                return HttpNotFound();
            }
            repo.DeleteCustomField(cf);
            return RedirectToAction("Index", "settings");
        }

        public ActionResult RemoveEmployeeIp(string ip)
        {
            repo.DeleteEmployeeIp(ip);
            return RedirectToAction("Index", "settings");
        }

        public ActionResult RemoveTickerElement(int id)
        {
            var e = repo.FindTickerElement(id);
            if (e == null)
            {
                return HttpNotFound();
            }
            repo.DeleteTickerElement(e);
            return RedirectToAction("elements", "ticker", new { id = e.TickerId });
        }

        //public ActionResult DeleteBanner(int id)
        //{
        //    var banner = repo.FindBanner(id);
        //    if (banner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    repo.DeleteBanner(banner);
        //    return RedirectToAction("index", "banner", new { id = banner.BannerId });
        //}

        [Authorize(Roles = "Administrator")] // this funtions are not allowed to affiliates
        public ActionResult RemoveCustomerUser(int id)
        {
            repo.DeleteCustomerUser(id);
            repo.DeleteUserTickers(id);
            return RedirectToAction("Index", "settings");
        }

        [Authorize(Roles = "Administrator")] // this funtions are not allowed to affiliates
        public ActionResult RemoveUser(int id, int id2) // id is the userid and id2 is the affiliateid
        {
            repo.DeleteCustomerUser(id);
            repo.DeleteUserTickers(id);
            return RedirectToAction("users", "affiliate", new { id = id2 });
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult RemoveCustomerDomain(int id)
        {
            repo.DeleteDomain(id);
            return RedirectToAction("Index", "settings");
        }

        public ActionResult DisplayUserList()
        {
            return PartialView("_UserListPartial", repo.GetCustomerUsers());
        }


        /// <summary>
        /// This is the method that creates the default ticker for a user
        /// </summary>
        /// <returns></returns>
        public ActionResult DisplayTicker()
        {

            var user = repo.GetCurrentUser();
            IEnumerable<Ticker> model = repo.GetTickers(user.UserId);

            if (model.Count() == 0)
            {
                // adds a default ticker
                var t = CPAHelper.GetDefaultTicker();
                t.Speed = 100;
                t.UserId = user.UserId;
                t.View = TickerView.Clicks | TickerView.Conversions | TickerView.Impressions;
                t.All = true;
                t.Direction = true;

                //Ticker t = new Ticker
                //{
                //    Speed = 100,
                //    UserId = userid,
                //    View = (TickerView)63, //TickerView.Campaign | TickerView.Clicks | TickerView.Conversions | TickerView.Cost | TickerView.Impressions | TickerView.Revenue,
                //    All = true,
                //    Direction = true
                //};

                //add default elements to the ticker 
                foreach (var item in repo.GetUserCampaigns(user).Where(c => c.Status == Status.Active))
                {
                    t.TickerElements.Add(new TickerElement()
                    {
                        CampaignId = item.Id
                    });

                }

                repo.AddTicker(t);


                model = new List<Ticker>() { t };
            }

            return PartialView("_Ticker", model);
        }

        public ActionResult DefaultColors()
        {
            repo.DeleteTickerSettings();
            return RedirectToAction("settings", "ticker");
        }

        public ActionResult BuildTicker(DateTime? fromdate, DateTime? todate, int tickerid, string timezone)
        {
            fromdate = fromdate ?? DateTime.Today;
            todate = todate ?? DateTime.Today.AddDays(1);

            var up = repo.GetCurrentUser();
            var customer = repo.GetCurrentCustomer(up.CustomerId);

            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            var fdate = new DateTime(fromdate.Value.Ticks, DateTimeKind.Unspecified);
            var tdate = new DateTime(todate.Value.Ticks, DateTimeKind.Unspecified);
            //this does the same that the above commented
            DateTime ufdate = TimeZoneInfo.ConvertTimeToUtc(fdate, tzi);
            DateTime utdate = TimeZoneInfo.ConvertTimeToUtc(tdate, tzi);

            object[] obj = new object[2];

            var ticker = repo.FindTicker(tickerid);
            obj[0] = (int)ticker.View;
            int? tkid = null;
            if (!ticker.All)
            {
                tkid = tickerid;
            }

            //obj[1] = repo.BuildTicker(ufdate, utdate, up.CustomerId, tkid, up.AffiliateId);

            var ddata = repo.BuildTickerExt(ufdate, utdate, up.CustomerId, up.UserId, up.AffiliateId);
            //var data = (IEnumerable<TickerItem>)repo.BuildTicker(ufdate, utdate, up.CustomerId, null, up.AffiliateId);

            var data = new List<TickerItemExt>();
            foreach (var item in ddata)
            {
                data.Add(item);
            }


            var tickerelementview = new List<TickerElementView>();
            var datafilter = new List<TickerElementView>();
            TickerElementView ev = null;

            foreach (var item in ticker.TickerElements)
            {

                if (item.AffiliateId.HasValue && item.CampaignId.HasValue)
                {
                    ev = data.Where(c => c.CampaignId == item.Campaign.CampaignId && c.AffiliateId == item.Affiliate.AffiliateId)
                        .Select(c => new TickerElementView()
                        {

                            Title = string.Format("{0} / {1}", item.Campaign.CampaignName, item.Affiliate.Company),

                            Clicks = c.Clicks,
                            Conversions = c.Conversions,
                            Impressions = c.Impressions,

                            OldClicks = c.OldClicks,
                            OldConversions = c.OldConversions,
                            OldImpressions = c.OldImpressions,

                            Cost = c.Cost,
                            Revenue = c.Revenue,

                        }).SingleOrDefault();


                }
                else if (item.CampaignId.HasValue)
                {
                    ev = (//from e in ((IEnumerable<TickerItem>)ddata).Where(c => c.CampaignId == item.Campaign.CampaignId)
                          from e in data.Where(c => c.CampaignId == item.Campaign.CampaignId)
                          group e by e.CampaignId into g
                          select new TickerElementView
                          {
                              Title = item.Campaign.CampaignName,

                              Clicks = g.Sum(e => e.Clicks),
                              Conversions = g.Sum(e => e.Conversions),
                              Impressions = g.Sum(e => e.Impressions),

                              OldClicks = g.Sum(e => e.OldClicks),
                              OldConversions = g.Sum(e => e.OldConversions),
                              OldImpressions = g.Sum(e => e.OldImpressions),

                              Cost = g.Sum(e => e.Cost),
                              Revenue = g.Sum(e => e.Revenue),
                          }).SingleOrDefault();
                }
                else
                {
                    ev = (from e in data.Where(c => c.AffiliateId == item.Affiliate.AffiliateId)
                          group e by e.AffiliateId into g
                          select new TickerElementView
                          {
                              Title = item.Affiliate.Company,

                              Clicks = g.Sum(e => e.Clicks),
                              Conversions = g.Sum(e => e.Conversions),
                              Impressions = g.Sum(e => e.Impressions),

                              OldClicks = g.Sum(e => e.OldClicks),
                              OldConversions = g.Sum(e => e.OldConversions),
                              OldImpressions = g.Sum(e => e.OldImpressions),

                              Cost = g.Sum(e => e.Cost),
                              Revenue = g.Sum(e => e.Revenue),
                          }).SingleOrDefault();
                }

                if (ev != null)
                {
                    datafilter.Add(ev);
                }


            }

            //var l = new List<TickerElementView>();
            //foreach (var item in list)
            //{
            //    l.Add(item);
            //}
            //int a = l.Count();

            obj[1] = datafilter;
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }

        public ActionResult DeleteCampaign(int id)
        {
            var cpc = repo.FindConversionPixelCampaign(id);
            repo.RemoveConversionPixelCampaign(cpc);
            return RedirectToAction("campaigns", "conversionpixel", new { id = cpc.ConversionPixelId });
        }

        public ActionResult AllAffiliates(bool id) // showall
        {
            object o;
            var customerid = repo.GetCurrentCustomerId();
            if (id)
            {
                o = repo.GetCustomerAffiliates(customerid).Select(a => new { AffiliateId = a.AffiliateId, Company = a.Company });
            }
            else
            {
                o = repo.GetCustomerActiveAffiliates(customerid).Select(a => new { AffiliateId = a.AffiliateId, Company = a.Company }); ;
            }

            return Json(o, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTicker(int id)
        {
            var ticker = repo.FindTicker(id);
            if (ticker == null)
            {
                return HttpNotFound();
            }
            repo.DeleteTicker(id);
            return RedirectToAction("Index", "ticker");
        }

        public ActionResult DeleteUrl(int id)
        {
            var url = repo.FindURL(id);
            if (url == null)
            {
                return HttpNotFound();
            }
            repo.DeleteUrl(url);
            return RedirectToAction("index", "url", new { id = url.CampaignId });
        }

        public ActionResult DeleteRedirectUrl(int id)
        {
            var ru = repo.FindRedirectUrl(id);
            if (ru == null)
            {
                return HttpNotFound();
            }
            repo.DeleteRedirectUrl(ru);
            return RedirectToAction("index", "redirect", new { id = ru.URLId });
        }

        public ActionResult DeleteActionConversionPixel(int id)
        {
            var obj = repo.FindActionConversionPixel(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            repo.DeleteActionConversionPixel(obj);
            return RedirectToAction("actions", "conversionpixel", new { id = obj.ConversionPixelId });
        }

        /// <summary>
        /// Get campaigns who default action is match tracking type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetCampaignsByTrackingType(TrackingType id)
        {
            //var user = repo.GetCurrentUser();
            var obj = repo.GetUserCampaigns()
                .AsEnumerable()
                /*.Where(c => c.Actions.First().TrackingType == id)*/
                .Select(c => new
                {
                    Value = c.Id,
                    Text = string.Format("{0} - {1}", c.CampaignId, c.CampaignName),
                });

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Statuschange(int status, int conversionid)
        {
            //CpaTickerDb db = new CpaTickerDb();
            //Conversion conversion = db.Conversions.First(con => con.ConversionId == conversionid);

            var conversion = repo.Conversions().Single(c => c.ConversionId == conversionid);
            conversion.Status = (conversion.Status + 1) % 2;
            repo.SaveChanges();
            return Content(conversion.Status + "," + conversionid);
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }

    }
}
