using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using System.IO;
using System.Transactions;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;

namespace CpaTicker.Controllers
{
    public class CPAController : Controller
    {
        private CpaTickerDb db = new CpaTickerDb();
        //using a repository rather the Entity Framework
        private ICpaTickerRepository repo;

        public CPAController()
            : this(new EFCpatickerRepository())
        {

        }

        public CPAController(ICpaTickerRepository repo)
        {
            this.repo = repo;
        }

        public ActionResult Preview(int id) //int affiliateid, int campaignid, int bannerid
        {
            //int cid = 1; //commeted by njhones
            // get the cid by the domain
            //int cid = CPAHelper.GetCustomerIdFromUrlCopy(Request.Url);

            //Banner banner = db.Banners.Where(b => b.BannerId == bannerid && b.CustomerId == cid && b.CampaignId == campaignid).SingleOrDefault();

            var banner = repo.FindBanner(id);
            if (banner != null)
            {
                string contentType = CPAHelper.BannerContentType(banner.Name);
                return File(banner.Image, contentType);
                //string contentType = "";
                //switch (Path.GetExtension(banner.Name))
                //{
                //    case ".png":
                //        contentType = "png";
                //        break;
                //    case ".gif":
                //        contentType = "gif";
                //        break;
                //    case ".jpeg":
                //    case ".jpg":
                //        contentType = "jpeg";
                //        break;
                //    default:
                //        contentType = "png";
                //        break;
                //}
                //return File(banner.Image, "image/" + contentType);
                
            }
            else
            {

                return File("~/images/clear.png", "image/png");
            }

        }

        // impression 
        public async Task<ActionResult> View(int affiliateid, int campaignid, int bannerid, int urlid = 1, int? random = null
            , string source = "", string ip = null, int pageid = 0
            /*, string subid = "", string subid2 = "", string subid3 = "", string subid4 = "", string subid5 = ""*/)
        {
            //int cid = 14; //commeted by njhones
            // get the cid by the domain
            int cid = CPAHelper.GetCustomerIdFromUrlCopy(Request.Url);

            // esto muy mal crear el banner si existe el banner de ese tipo esto esta muy mal
            Banner banner = db.Banners.Where(b => b.BannerId == bannerid && b.CustomerId == cid && b.CampaignId == campaignid).SingleOrDefault();
            if (banner != null)
            {
                // create impression
                var ipaddress = ip ?? Request.UserHostAddress;

                //var block = await repo.FindBlockAsync(CPAHelper.IpToUInt32(ipaddress));
                //Location location = block == null ? null : block.GetLocation();

                var intAddress = CPAHelper.IpToUInt32(ipaddress);
                var block = repo.RunQuery<Location>("set nocount on set transaction isolation level read uncommitted select top 1 * from blocks blc  left join Locations lc on  blc.locid = lc.locid where blc.startIpNum<'" + intAddress + "' and blc.endIpNum>'" + intAddress + "'").FirstOrDefault();
                Location location = block == null ? null : block;

                Campaign campaign = CPAHelper.GetCampaignById(campaignid, cid);

                Impression imp = new Impression();
                imp.AffiliateId = affiliateid;
                imp.CustomerId = cid;
                imp.CampaignId = campaignid;
                //imp.ImpressionDate = System.DateTime.Now;
                imp.ImpressionDate = DateTime.UtcNow;
                imp.IPAddress = ipaddress;
                imp.UserAgent = Request.UserAgent;
                if (String.IsNullOrWhiteSpace(Request.UserAgent) || Request.UserAgent.Contains("bot"))
                { imp.bot = 1; }
                imp.Referrer = (Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString());
                imp.Source = source;
                imp.BannerId = banner.BannerId;


                //imp.Subid1 = subid;
                //imp.Subid2 = subid2;
                //imp.Subid3 = subid3;
                //imp.Subid4 = subid4;
                //imp.Subid5 = subid5;

                if (location != null)
                {
                    imp.Country = location.country_iso_code;
                }

                // if the random is on the querystring then set the random urlid
                if (random.HasValue)
                {
                    ////// get urlid from the click db and use it to get the round robin url
                    //var lastrandomclick = repo.Clicks(cid, campaignid).AsEnumerable().LastOrDefault(c => c.Random != null);
                    //urlid = lastrandomclick == null ? 1 : lastrandomclick.Random.Value + 1;
                    var _noofPages = repo.GetCustomerPage(cid).ToList();
                    if (_noofPages.Count > 0)
                    {
                        Random rnd = new Random();
                        int randomvalue = rnd.Next(0, _noofPages.Count - 1);
                        pageid = _noofPages[randomvalue].Id;
                    }
                    else
                    {
                        return Content("Invalid PageID.. Please add correct pageId");
                    }

                }

                if (pageid == 0)
                {
                    // get the url id
                    var url = repo.GetCampaignURLs(campaign.Id).SingleOrDefault(u => u.PreviewId == urlid);

                    if (url == null)
                    {
                        urlid = 1;
                        url = repo.GetCampaignURLs(campaign.Id).SingleOrDefault(u => u.PreviewId == urlid);

                    }
                    pageid = url.Id;
                }

                var page = repo.GetCustomerPage(cid).Where(u => u.Id == pageid).FirstOrDefault();
                if (page == null)
                {
                    return Content("Invalid PageID.. Please add correct pageId");

                }
                //if (url == null)
                //{
                //    urlid = 1;
                //    url = repo.GetCampaignURLs(campaign.Id).SingleOrDefault(u => u.PreviewId == urlid);
                //    //////////if (url == null)  // this is a patch the default url must already exits
                //    //////////{
                //    //////////    // this is a patch the default url must already exits
                //    //////////    // cerate the default url

                //    //////////    //URL default_url = new URL
                //    //////////    //{
                //    //////////    //    CampaignId = campaign.Id,
                //    //////////    //    OfferUrl = campaign.OfferUrl,
                //    //////////    //    PreviewUrl = campaign.PreviewUrl,
                //    //////////    //    Cost = campaign.Payout,
                //    //////////    //    Revenue = campaign.Revenue,
                //    //////////    //    PreviewId = 1
                //    //////////    //};
                //    //////////    url = repo.GetDefaultUrl(campaign);
                //    //////////    repo.AddURL(url);
                //    //////////}
                //}

                imp.URLPreviewId = page.PreviewId; // newly added static value it's no longer needed.
                //imp.URLPreviewId = url.PreviewId;
                imp.URLId = page.Id;    // newly added urlid
                // set the cost & revenue based on the url
                if (page.PayoutType == PayoutType.CPM)
                {
                    var result = repo.CheckOverridebyCampaign(campaignid, cid);
                    if (result != null)
                    { imp.Cost = repo.GetCost(result.PayoutType, result.Payout, result.PayoutPercent, ActionType.Impression); }
                    else
                    { imp.Cost = repo.GetCost(page.PayoutType, page.Payout, page.PayoutPercent, ActionType.Impression); }
                }
                else
                {
                    imp.Cost = repo.GetCost(page.PayoutType, page.Payout, page.PayoutPercent, ActionType.Impression);
                }


                imp.Revenue = repo.GetRevenue(page.RevenueType, page.Revenue, page.RevenuePercent, ActionType.Impression);



                ////////////////////////      for affiliate override start  







                //////////////////////       for affiliate override end.

                // get all subid in the querystring
                Dictionary<int, string> dic = new Dictionary<int, string>();
                //var regex = new Regex(@"^subid[1-9](\d*)$");

                foreach (string index in Request.QueryString.AllKeys.Where(s => s != null && s.StartsWith("subid"))) // regex.IsMatch(s)
                {
                    try
                    {
                        dic.Add(int.Parse(index.Substring(5)), Request.QueryString[index].Trim());
                    }
                    catch
                    {
                        // store subid in dic[1] assuming that dic[1] not exits
                        var _sid = index.Substring(5);
                        if (string.IsNullOrEmpty(_sid))
                        {
                            try
                            {
                                dic.Add(1, Request.QueryString["subid"].Trim());
                            }
                            catch { }
                        }
                    }
                }



                if (dic.Count() > 0)
                {
                    imp.SubIds = new List<ImpressionSubId>();
                    foreach (var item in dic)
                    {
                        // create the clicksubid
                        imp.SubIds.Add(new ImpressionSubId()
                        {
                            SubIndex = item.Key,
                            SubValue = item.Value,
                        });
                    }
                }



                db.Impressions.Add(imp);
                db.SaveChanges();
                //string contentType = "";
                //switch (Path.GetExtension(banner.Name))
                //{
                //    case ".png":
                //        contentType = "png";
                //        break;
                //    case ".gif":
                //        contentType = "gif";
                //        break;
                //    case ".jpeg":
                //    case ".jpg":
                //        contentType = "jpeg";
                //        break;
                //    default:
                //        contentType = "png";
                //        break;
                //}
                //return File(banner.Image, "image/" + contentType);
                string contentType = CPAHelper.BannerContentType(banner.Name);
                return File(banner.Image, contentType);
            }
            else
            {
                return File("~/images/clear.png", "image/png");
            }

        }

        // click 
        public async Task<ActionResult> Click(int affiliateid, int campaignid, int urlid = 1, int? random = null, int bannerid = 0
            , string source = null
            , string ip = null, int pageid = 0
            )
        {
            // Handle Click Logic here
            // Find Affiliate Id / Campaign Id / Sub Id / Increment Clicks
            // Drop cookie




            CpaTicker.Areas.admin.Classes.PAGE page = null;
            //  CpaTicker.Areas.admin.Classes.URL url = null;
            CpaTicker.Areas.admin.Classes.Campaign campaign = null;

            DateTime CampaignStart = DateTime.Now;
            string ipaddress = ip ?? Request.UserHostAddress;
            DateTime CampaignEnd = DateTime.Now;

            DateTime URLStart = DateTime.Now;
            // get the client ipaddress as an Int32
            var intAddress = CPAHelper.IpToUInt32(ipaddress);
            DateTime URLEnd = DateTime.Now;
          
            //intAddress = CPAHelper.IpToUInt32("74.208.194.190"); // for debugging

            // grab the originating country
            //var country = repo.GetCountryFromInt32IP(intAddress);
            //var block = repo.GetBlockFromInt32IP(intAddress);
            DateTime SUBIDStart = DateTime.Now;
            // var block = await repo.FindBlockAsync(intAddress);

            var block = repo.RunQuery<Location>("set nocount on set transaction isolation level read uncommitted select top 1 * from blocks blc  left join Locations lc on  blc.locid = lc.locid where blc.startIpNum<'" + intAddress + "' and blc.endIpNum>'" + intAddress + "'").FirstOrDefault();


            DateTime SUBIDEnd = DateTime.Now;
            DateTime CookieStart = DateTime.Now;
            // Location location = block == null ? null : block.GetLocation();
            Location location = block == null ? null : block;
            //if (block == null && CpaTickerConfiguration.DefaultRedirect)
            //{
            //    return Redirect(string.Format("http://www.{0}", CpaTickerConfiguration.DefaultDomainName)); // this works !!
            //}

            DateTime CookieEnd = DateTime.Now;
            int cid = 0;
            Customer customer;

            try
            {
                //cid = 11; // for debug
                cid = repo.GetCustomerId(Request.Url);
                customer = repo.GetCurrentCustomer(cid);
                if (customer == null)
                {
                    throw new Exception();
                }
            }
            catch
            {
                return Content("Invalid customer");
            }





            Affiliate aff = repo.GetAffiliate(affiliateid, cid);
            if (aff == null)
            {
                return Content("Invalid affililateid");
            }

            if (aff.Status != AffiliateStatus.Active)
            {
                return Content("Inactive affiliateid");
            }




            var datetime = DateTime.UtcNow;
            campaign = repo.GetCampaignById(campaignid, cid);

            if (campaign == null)
            {
                return Content("Invalid campaignid");
            }

            // if the originating country was not found so redirects if default redirect is on
            //if (block == null)
            //{
            //    block = new Block { Location = new Location { country = "", region = "" } }; // unknown country
            //}

            #region Campaign Geo Targets
            DateTime AgentStart = DateTime.Now;
            // if enforce is on and the click came from a selected country then redirect to home
            if (campaign.Enforce && location != null)//& repo.CheckIP(intAddress, campaign.Id)
            {
                //return RedirectToAction("Index", "Home", new { area = ""});
                //return RedirectToRoute("default", new {  }); always redirects using the name in the url 

                // the country was found so if it is in the blacklist redirect to homepage
                if (repo.CampaignCountries(campaign.Id).Count(c => c.Code == location.country_iso_code) > 0)
                {
                    return Redirect(string.Format("http://www.{0}", CpaTickerConfiguration.DefaultDomainName)); // this works !!
                }
            }
            DateTime AgentEnd = DateTime.Now;
            #endregion

            DateTime ClickStart = DateTime.Now;


            // if the random is on the querystring then set the random urlid
            if (random.HasValue)
            {
                ////// get urlid from the click db and use it to get the round robin url
                //var lastrandomclick = repo.Clicks(cid, campaignid).AsEnumerable().LastOrDefault(c => c.Random != null);
                //urlid = lastrandomclick == null ? 1 : lastrandomclick.Random.Value + 1; // OJO if the value is gt must be 1 again is handler down

                var _noofPages = repo.GetCustomerPage(cid).ToList();
                if (_noofPages.Count > 0)
                {
                    Random rnd = new Random();
                    int randomvalue = rnd.Next(0, _noofPages.Count - 1);
                    pageid = _noofPages[randomvalue].Id;
                }
                else
                {
                    return Content("Invalid PageID.. Please add correct pageId");
                }
            }
            // get the url by the campaign and the urlid(previewurlid)
            //  url = repo.GetCampaignURLs(campaign.Id).SingleOrDefault(u => u.PreviewId == urlid);

            if (pageid == 0)
            {
                // get the url id
                var url = repo.GetCampaignURLs(campaign.Id).SingleOrDefault(u => u.PreviewId == urlid);

                if (url == null)
                {
                    urlid = 1;
                    url = repo.GetCampaignURLs(campaign.Id).SingleOrDefault(u => u.PreviewId == urlid);

                }
                pageid = url.Id;
            }

            page = repo.GetCustomerPage(cid).Where(u => u.Id == pageid).FirstOrDefault();
            if (page == null)
            {
                return Content("Invalid PageID.. Please add correct pageId");

            }

            //if (url == null)
            //{
            //    urlid = 1; // setting url = 1 if random is too way grater or if urlid was passed in the querystring
            //    url = repo.GetCampaignURLs(campaign.Id).SingleOrDefault(u => u.PreviewId == urlid);
            //    //////if (url == null)  // this is a patch the default url must already exits
            //    //////{
            //    //////    // this is a patch the default url must already exits
            //    //////    repo.AddURL(repo.GetDefaultUrl(campaign));
            //    //////    url = repo.GetCampaignURLs(campaign.Id).SingleOrDefault(u => u.PreviewId == urlid);
            //    //////}
            //}



            // generate the transactionid
            //string transactionId = Guid.NewGuid().ToString();

            string transactionId = null; // Guid.NewGuid().ToString();
            switch (customer.TransactionIdType)
            {
                case TransactionIdGenerationType.Random:
                    transactionId = Guid.NewGuid().ToString();
                    break;
                case TransactionIdGenerationType.IPDate:
                    transactionId = GuidUtility.Create(GuidUtility.DnsNamespace,
                        string.Format("{0}_{1}", ipaddress, datetime.ToString("yyyyMMdd"))).ToString();
                    break;
                default:
                    transactionId = GuidUtility.Create(GuidUtility.DnsNamespace,
                          string.Format("{0}_{1}_{2}_{3}", ipaddress, datetime.ToString("yyyyMMdd"), campaign.CampaignId, pageid)).ToString();
                    break;
            }


            // get all subid in the querystring
            Dictionary<int, string> dic = new Dictionary<int, string>();
            //var regex = new Regex(@"^subid[1-9](\d*)$");
            foreach (string index in Request.QueryString.AllKeys.Where(s => s != null && s.StartsWith("subid"))) // regex.IsMatch(s)
            {
                try
                {
                    dic.Add(int.Parse(index.Substring(5)), Request.QueryString[index].Trim());
                }
                catch
                {
                    // store subid in dic[1] assuming that dic[1] not exits
                    var _sid = index.Substring(5);
                    if (string.IsNullOrEmpty(_sid))
                    {
                        try
                        {
                            dic.Add(1, Request.QueryString["subid"].Trim());
                        }
                        catch { }
                    }
                }
            }

            // create click and set it
            Click click = new Click();
            click.AffiliateId = affiliateid;
            click.CustomerId = cid;
            click.CampaignId = campaign.CampaignId;
            click.ClickDate = datetime;
            click.IPAddress = ipaddress;
            click.UserAgent = Request.UserAgent;
            click.Status = ClickStatus.Active;

            #region find the active click with the same transaction and turn it off in case there is not an existing conversion
            // if there is a conversion turn the status to archive

            //var activeClick = repo.Clicks().FirstOrDefault(c => c.TransactionId == transactionId && c.Status == ClickStatus.Active); // it must be single or default
            //if (activeClick != null)
            //{
            //    if (repo.Conversions().Any(c => c.ClickId == activeClick.ClickId))
            //    {
            //        click.Status = ClickStatus.Archive;
            //    }
            //    else
            //    {
            //        activeClick.Status = ClickStatus.Archive;
            //        db.Entry(activeClick).State = System.Data.Entity.EntityState.Modified;
            //    }
            //}
            #endregion


            if (String.IsNullOrWhiteSpace(Request.UserAgent) || Request.UserAgent.Contains("bot"))
            { click.bot = 1; }

            var UserAgentDetails = repo.addUserAgentInfos(Request.UserAgent);

            if (UserAgentDetails != null)
            { click.UserAgentId = UserAgentDetails.Id; }


            click.Referrer = (Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString());
            click.TransactionId = transactionId; // store the transactionid for the click
            click.Source = source;
            click.BannerId = bannerid;



            if (page.PayoutType == PayoutType.CPC)
            {
                var result = repo.CheckOverridebyCampaign(campaignid, customer.CustomerId);
                if (result != null)
                { click.Cost = repo.GetCost(result.PayoutType, result.Payout, result.PayoutPercent, ActionType.Click); }
                else
                { click.Cost = repo.GetCost(page.PayoutType, page.Payout, page.PayoutPercent, ActionType.Click); }
            }
            else
            {
                click.Cost = repo.GetCost(page.PayoutType, page.Payout, page.PayoutPercent, ActionType.Click);
            }

            click.Revenue = repo.GetRevenue(page.RevenueType, page.Revenue, page.RevenuePercent, ActionType.Click);



            ////////////////////////      for affiliate override start  






            //////////////////////       for affiliate override end.




            click.URLPreviewId = page.PreviewId;  /// newaly added static value no longer needed.
            // click.URLPreviewId = url.PreviewId;
            click.URLId = page.Id;    // newly added urlid
            click.Country = location == null ? "" : location.country_iso_code;

            if (random.HasValue) // set if the click is a random click
            {
                click.Random = urlid;
            }
            if (Request.Browser.Cookies) // set if browser suppot cookies
            {
                click.Cookies = true;
            }

            if (dic.Count() > 0)
            {
                var clickType = click.GetType();
                foreach (var item in dic)
                {
                    clickType.GetProperty("SubId" + item.Key.ToString())
                            .SetValue(click, item.Value);
                }

            }

            #region Redirect URL Geotarget
            StringBuilder sb = null;
            /*
             * redirect by urlid - redirects
             * get the associated country to that IPaddress 
             * and check if that country is in the blacklist(redirectstarget) 
             * if so get the redirect url and redirect to it
             * 
             * if no country is found redirect to de home site url
             * */
            // get all redirect targets for all rederecturls or this url
            RedirectTargetPage redirecttarget = location == null ? null :
                repo.RedirectTargetPages().SingleOrDefault(t => t.IP2Country.Code == location.country_iso_code && t.RedirectPAGE.PAGEId == page.Id);

            if (redirecttarget != null)
            {
                //return Redirect(redirecttarget.RedirectUrl.RedirectURL);
                sb = new StringBuilder(redirecttarget.RedirectPAGE.RedirectPage);
                click.RedirectUrlId = redirecttarget.RedirectPageId;
            }
            else
            {
                sb = new StringBuilder(page.OfferUrl);
            }



            #endregion
            try
            {
                repo.AddClick(click);
            }
            catch (Exception ex)
            {

                return Content("Error:" + ex.Message);
            }


            // set cookies 
            Response.Cookies["cpaticker"][campaign.CampaignId.ToString()] = transactionId;
            // set domain
            Response.Cookies["cpaticker"].Domain = Request.Url.Host;
            Response.Cookies["cpaticker"].Expires = campaign.CookieExpirationInDays.HasValue ?
                DateTime.Now.AddDays(campaign.CookieExpirationInDays.Value)
                : DateTime.Now.AddYears(1);


            sb.Replace("{affiliate_id}", affiliateid.ToString());
            sb.Replace("{affiliate_name}", HttpUtility.UrlEncode(repo.GetAffiliate(affiliateid, cid).Company));
            sb.Replace("{source}", HttpUtility.UrlEncode(source));
            sb.Replace("{campaign_id}", campaign.CampaignId.ToString());
            sb.Replace("{campaign_name}", HttpUtility.UrlEncode(campaign.CampaignName));
            sb.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));
            sb.Replace("{time}", HttpUtility.UrlEncode(DateTime.Now.ToString("hh:mm:ss")));
            sb.Replace("{datetime}", HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
            sb.Replace("{ip}", Request.UserHostAddress);
            sb.Replace("{transaction_id}", transactionId);
            sb.Replace("{country}", location == null ? "" : location.country_iso_code);
            sb.Replace("{state}", location == null ? "" : location.subdivision_1_iso_code);

            sb.Replace("{request_id}", GuidEncoder.Encode(transactionId));


            try
            {
                foreach (var item in dic.Keys)
                {

                    sb.Replace("{aff_sub" + item + "}",
                        HttpUtility.UrlEncode(dic[item]));
                }

                //path to replace 
                //sb.Replace("{aff_sub}", HttpUtility.UrlEncode(dic[1]));              
            }
            catch
            {
                // if the item isn't present in the dic
            }


            // find all occurries left of {aff_subn} placeholder and replaced for ""
            var urlstring = System.Text.RegularExpressions.Regex.Replace(sb.ToString(), @"{aff_sub\d*}", "");

            // adding the custom fields to the offerurl

            var ub = new UriBuilder(urlstring);
            var httpValueCollection = HttpUtility.ParseQueryString(ub.Query);

            // add passthrought variables 
            // get all querystring variables in the request that are not identified for the system
            // int affiliateid, int campaignid, int urlid = 1, int? random = null, int bannerid = 0, string hitid
            foreach (var variable in Request.QueryString.AllKeys.Where(s => !s.StartsWith("subid")
                && s != "affiliateid"
                && s != "campaignid"
                && s != "bannerid"
                && s != "urlid"
                && s != "source"
                && s != "random"
                && s != "ip"
                && s != "pageid"
                ))
            {
                httpValueCollection.Add(variable, Request.QueryString[variable]);
            }
            // add the custom fields to the redirect url this field will be gotten
            // in the above for because the custom fields are in the querystring with his respected values

            //var cfields = repo.GetCampaignCustomFieldValue(campaign.Id, cid);
            //foreach (var item in cfields)
            //{
            //    httpValueCollection.Add(item.CustomField.FieldName, item.Value);
            //}

            ub.Query = httpValueCollection.ToString();
            DateTime ClickEnd = DateTime.Now;
            try
            {

                TimeSpan Campaignspan = CampaignEnd - CampaignStart;
                int Campaignms = (int)Campaignspan.TotalMilliseconds;

                TimeSpan URLspan = URLEnd - URLStart;
                int URLms = (int)URLspan.TotalMilliseconds;

                TimeSpan SubIDspan = SUBIDEnd - SUBIDStart;
                int SubIDms = (int)SubIDspan.TotalMilliseconds;

                TimeSpan Agentspan = AgentEnd - AgentStart;
                int Agentms = (int)Agentspan.TotalMilliseconds;

                TimeSpan Clickspan = ClickEnd - ClickStart;
                int Clickms = (int)Clickspan.TotalMilliseconds;

                TimeSpan Cookiespan = CookieEnd - CookieStart;
                int Cookiems = (int)Cookiespan.TotalMilliseconds;

                int total = Campaignms + URLms + SubIDms + Agentms + Clickms + Cookiems;
                var GetClick = db.Clicks.Where(u => u.ClickId == click.ClickId).FirstOrDefault();
                GetClick.TimeInterval = total;
                db.SaveChanges();
                TimeClicks tbl = new TimeClicks();
                tbl.ClickId = click.ClickId;
                tbl.Campaign = Campaignms;
                tbl.URL = URLms;
                tbl.SubID = SubIDms;
                tbl.Agent = Agentms;
                tbl.Click = Clickms;
                tbl.Cookie = Cookiems;
                tbl.total = total;
                db.TimeClicks.Add(tbl);

                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectPermanent(ub.ToString());
        }

        // conversion
        public async Task<ActionResult> Conversion(int cpid = 0, int? actionid = null, string ip = null,
            string act_data1 = null,
            string act_data2 = null,
            string act_data3 = null,
            string act_data4 = null,
            string act_data5 = null)
        {
            DateTime Intstart = DateTime.Now;
            DateTime postbackstart = DateTime.Now;
            DateTime postbackend = DateTime.Now;
            ConversionLog clog = new ConversionLog();
            DateTime utcnow = DateTime.UtcNow;
            clog.Postback = null;
            clog.Success = false;
            clog.ConversionDate = utcnow;

            var ipaddress = ip ?? Request.UserHostAddress;
            clog.IPAddress = ipaddress;
            var intAddress = CPAHelper.IpToUInt32(ipaddress);
            //var block = await repo.FindBlockAsync(intAddress);
            //Location location = block == null ? null : block.GetLocation();


            var block = repo.RunQuery<Location>("set nocount on set transaction isolation level read uncommitted select top 1 * from blocks blc  left join Locations lc on  blc.locid = lc.locid where blc.startIpNum<'" + intAddress + "' and blc.endIpNum>'" + intAddress + "'").FirstOrDefault();
            Location location = block == null ? null : block;
            //int customerid = 11;  // debugging
            int customerid;
            #region check customerid
            try
            {
                customerid = repo.GetCustomerId(Request.Url);
            }
            catch
            {
                clog.Reason = "CustomerId not found";
                db.ConversionLogs.Add(clog);
                db.SaveChanges();
                return File("~/images/clear.png", "image/png");
            }
            clog.CustomerId = customerid;
            #endregion

            var campaign = repo.GetCampaignById(cpid, customerid);
            #region check campaign
            if (campaign == null)
            {
                clog.Reason = "Campaign not found for customer" + customerid;
                db.ConversionLogs.Add(clog);
                db.SaveChanges();
                return File("~/images/clear.png", "image/png");
            }
            clog.CampaignId = campaign.CampaignId;
            #endregion


            // get the action from the querystring or the first campaign action
            var action = actionid.HasValue ? repo.FindAction(actionid.Value) : repo.DefaultAction(campaign);
            bool postback = false;

            #region Retrieve the tracsactionid either from the cookie or from the querystring

            string transactionId = "";
            /*
                * if the action is serverpostback then the transaction id must be in the url
                * else the transaction_id is on the cookie
                * */

            //if (action.TrackingType == TrackingType.ServerPostback)
            if (Request.QueryString.AllKeys.Contains("transaction_id"))
            {
                // get the transactionid from the querystring
                transactionId = Request.QueryString["transaction_id"];
                clog.Postback = false;
            }
            else if (Request.QueryString.AllKeys.Contains("request_id"))
            {
                try
                { transactionId = GuidEncoder.Decode(Request.QueryString["request_id"]).ToString(); }
                catch (Exception ex)
                { return Content("Request Id has wrong formet."); }
                postback = true;
                clog.Postback = true;
            }
            else
            {
                clog.Postback = false;
                #region check cookies

                if (Request.Cookies["cpaticker"] == null)
                {
                    clog.Reason = "Error: no click ([cpaticker] cookie not fount)";
                    db.ConversionLogs.Add(clog);
                    db.SaveChanges();

                    return Content("Error: no click");
                }

                // error no campaign info in the cookie
                if (Request.Cookies["cpaticker"][cpid.ToString()] == null)
                {
                    clog.Reason = string.Format("Error: no campaign id click : {0} \r\n [cpaticker][{0}] cookie not fount though [cpaticker] coookie exits", cpid);
                    db.ConversionLogs.Add(clog);
                    db.SaveChanges();

                    return Content("Error: no campaign id click : " + cpid.ToString());
                }
                #endregion

                // get the transactionid from the cookies
                transactionId = Request.Cookies["cpaticker"][cpid.ToString()];
            }
            clog.TransactionId = transactionId;
            #endregion

            #region click
            // grab originating click to see if it exists

            //Click click = db.Clicks.FirstOrDefault(cl => cl.TransactionId == transactionId);
            var click = db.Clicks.Where(cl => cl.TransactionId == transactionId).AsEnumerable().LastOrDefault(); // get the lastone instead of the first


            if (click == null)
            {
                clog.Reason = "The originating click was not found for the transactionid: " + transactionId;
                db.ConversionLogs.Add(clog);
                db.SaveChanges();
                // if we've arrived here, we didn't find the originating click - generate image
                return File("~/images/clear.png", "image/png");
            }
            clog.AffiliateId = click.AffiliateId;
            #endregion


            // see if conversion has already been generated
            // if the conversion is rejected count as no conversion at all so procced like there is no one
            var conversion = db.Conversions.FirstOrDefault(co => co.TransactionId == transactionId
                    && co.ActionId == action.Id
                    && co.Status == 1);



            bool isnew = false;

            if (
                /* if the conversion exit with the same transaction_id but for a different campaign*/
                (conversion != null && (conversion.CampaignId != cpid))
                ||
                /* if the conversion exist for the same campaign and has already post / pixel */
                (conversion != null && ((conversion.Pixel.HasValue && !postback) || (postback && conversion.Postback.HasValue)))
            )
            {

                clog.Reason = "duplicate conversion";
                db.ConversionLogs.Add(clog);
                db.SaveChanges();
                // conversion already exists - display message about duplicate
                return Content("duplicate conversion - transaction id: " + transactionId);
            }

            if (conversion != null && postback)
            {
                // add the conversion postback
                conversion.Postback = utcnow;
                //conversion.ConversionDate = utcnow;  // set to the must recent date
                conversion.Postback_IPAddress = ipaddress;
            }

            else if (conversion != null)
            {
                // add the conversion pixel
                conversion.Pixel = utcnow;
                //conversion.ConversionDate = utcnow;
                conversion.IPAddress = ipaddress;
            }

            else
            {

                // add the conversion everything is OK
                //no conversion, so let's add one
                conversion = new Conversion();
                conversion.CustomerId = customerid;
                conversion.ClickId = click.ClickId;
                conversion.CampaignId = cpid;
                conversion.AffiliateId = click.AffiliateId;
                conversion.ConversionDate = utcnow; //co.ConversionDate = DateTime.UtcNow;

                conversion.Referrer = (Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString());
                conversion.TransactionId = transactionId;
                conversion.UserAgent = Request.UserAgent;

                conversion.act_data1 = act_data1;
                conversion.act_data2 = act_data2;
                conversion.act_data3 = act_data3;
                conversion.act_data4 = act_data4;
                conversion.act_data5 = act_data5;

                if (String.IsNullOrWhiteSpace(Request.UserAgent) || Request.UserAgent.Contains("bot"))
                { conversion.bot = 1; }

                string Conuseragent = Request.UserAgent;

                if (String.IsNullOrWhiteSpace(Conuseragent) || Conuseragent.Contains("Landfall"))
                {
                    conversion.bot = 0;
                    Conuseragent = click.UserAgent;
                    conversion.UserAgent = click.UserAgent;
                }
                var UserAgentDetails = repo.addUserAgentInfos(Conuseragent);
                if (UserAgentDetails != null)
                { conversion.UserAgentId = UserAgentDetails.Id; }

                conversion.BannerId = click.BannerId;

                if (actionid != null)
                {
                    var result = repo.CheckOverridebyAction(actionid, customerid);
                    if (result != null)
                    {
                        conversion.Cost = repo.GetCost(result.PayoutType, result.Payout, result.PayoutPercent, ActionType.Conversion, Request.QueryString["saleamount"]);
                    }
                    else
                    {
                        conversion.Cost = repo.GetCost(action.PayoutType, action.Payout, action.PayoutPercent, ActionType.Conversion, Request.QueryString["saleamount"]);
                    }

                }
                else
                {
                    conversion.Cost = repo.GetCost(action.PayoutType, action.Payout, action.PayoutPercent, ActionType.Conversion, Request.QueryString["saleamount"]);
                }

                conversion.Revenue = repo.GetRevenue(action.RevenueType, action.Revenue, action.RevenuePercent, ActionType.Conversion, Request.QueryString["saleamount"]);


                ////////////////////////      for affiliate override start  






                //////////////////////       for affiliate override end.
                conversion.Status = repo.IsEmployeeIp(ipaddress, customerid) ? 0 : 1;
                conversion.Type = action.Type;
                conversion.ActionId = action.Id;

                if (postback)
                {
                    conversion.Postback = utcnow;
                    conversion.Postback_IPAddress = ipaddress;
                }
                else
                {
                    conversion.Pixel = utcnow;
                    conversion.IPAddress = ipaddress;
                    //if (block != null)
                    //{
                    //    conversion.Country = block.Location.country;
                    //}
                    //conversion.Country = block == null ? click.Country : block.Location.country;

                }
                conversion.Country = click.Country;
                isnew = true;
                //db.Conversions.Add(conversion);
                //db.SaveChanges();

            }

            /*click subid's reference*/
            conversion.SubId1 = click.SubId1;
            conversion.SubId2 = click.SubId2;
            conversion.SubId3 = click.SubId3;
            conversion.SubId4 = click.SubId4;
            conversion.SubId5 = click.SubId5;
            conversion.SubId6 = click.SubId6;
            conversion.SubId7 = click.SubId7;
            conversion.SubId8 = click.SubId8;
            conversion.SubId9 = click.SubId9;
            conversion.SubId10 = click.SubId10;

            // switched cpid to campaign - campaign.Id is the primary key of the campaign, cpid is not
            //var pixels = repo.ConversionPixelCampaigns().Where(x => x.CampaignId == campaign.Id
            //    && x.ConversionPixel.Affiliate.AffiliateId == click.AffiliateId
            //    && x.ConversionPixel.Affiliate.CustomerId == customerid
            //    ).Select(x => x.ConversionPixel);

            // get the cp associated with this action rather than this campaign



            //var pixels = postback ?
            //    repo.ActionConversionPixelsActivePostBack(action.Id,click.AffiliateId,customerid)
            //    :
            //    repo.ActionConversionPixelsActiveNoPostBack(action.Id, click.AffiliateId, customerid)
            //    ;


            var pixels = postback ?
              (from a in db.ActionConversionPixels
               .Where(a => a.ActionId == action.Id
               && a.ConversionPixel.Affiliate.AffiliateId == click.AffiliateId
               && a.ConversionPixel.Affiliate.CustomerId == customerid
               && a.ConversionPixel.TrackingType == TrackingType.ServerPostback)

               join cpc in db.ConversionPixelCampaigns.Where(u => u.PixelStatus == PixelStatus.Active) on a.ConversionPixelId equals cpc.ConversionPixelId
               select new
               {
                   pixelcode = a.ConversionPixel.PixelCode
               }
               )


               :

              (from a in db.ActionConversionPixels
               .Where(a => a.ActionId == action.Id
               && a.ConversionPixel.Affiliate.AffiliateId == click.AffiliateId
               && a.ConversionPixel.Affiliate.CustomerId == customerid
               && a.ConversionPixel.TrackingType != TrackingType.ServerPostback)
               join cpc in db.ConversionPixelCampaigns.Where(u => u.PixelStatus == PixelStatus.Active) on a.ConversionPixelId equals cpc.ConversionPixelId
               select new
               {
                   pixelcode = a.ConversionPixel.PixelCode
               }
               )


               ;



            #region placeholders
            var affiliate_id = click.AffiliateId.ToString();
            var affiliate_name = HttpUtility.UrlEncode(repo.GetAffiliate(click.AffiliateId, customerid).Company);
            var source = HttpUtility.UrlEncode(click.Source);
            var campaign_id = campaign.CampaignId.ToString();
            var campaign_name = HttpUtility.UrlEncode(campaign.CampaignName);
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var time = HttpUtility.UrlEncode(DateTime.Now.ToString("hh:mm:ss"));
            var datetime = HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            var payout = conversion.Revenue.ToString();
            var request_id = GuidEncoder.Encode(transactionId);
            #endregion

            var sb = new StringBuilder();
            var postbackcontent = new StringBuilder();
            var pixelcontent = new StringBuilder();
            var output = new StringBuilder();
            int count = 0;
            foreach (var code in pixels)
            {
                //return Content("Conversion: true\n" + cp.PixelCode);
                // this will never work for multiple pixels.
                // saving pixels to string for output later.

                //sb.Append(HttpUtility.HtmlDecode(code));

                sb.Append(HttpUtility.HtmlDecode(code.pixelcode));
                count++;

                sb.Replace("{affiliate_id}", affiliate_id);
                sb.Replace("{affiliate_name}", affiliate_name);
                sb.Replace("{source}", source);
                sb.Replace("{campaign_id}", campaign_id);
                sb.Replace("{campaign_name}", campaign_name);
                sb.Replace("{date}", date);
                sb.Replace("{time}", time);
                sb.Replace("{datetime}", datetime);
                sb.Replace("{ip}", ipaddress);
                sb.Replace("{transaction_id}", transactionId);
                sb.Replace("{payout}", payout);
                sb.Replace("{request_id}", request_id);
                sb.Replace("{country}", conversion.Country);
                sb.Replace("{state}", location == null ? "" : location.city_name);

                // i need to iterate throw each placeholder found in the sb
                var result = Regex.Replace(sb.ToString(), @"{aff_sub([1-9]\d*)}", (match) =>
                {
                    //TODO:SUBID
                    var regEx = new Regex("/d+^", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    var clickType = click.GetType();
                    var subIds = clickType.GetProperties()
                                            .Where(p => p.Name.StartsWith("SubId"))
                                            .Select(p =>
                                                new KeyValuePair<int, string>(
                                                    int.Parse(regEx.Match(p.Name).Value),
                                                    Convert.ToString(p.GetValue(click)))
                                                    );

                    var subid = subIds.SingleOrDefault(s => s.Key == int.Parse(match.Groups[1].Value));
                    return subid.Value == null ? "" : subid.Value;
                });



                if (postback)
                {
                    // make the web request log the content into colog output
                    postbackstart = DateTime.Now;
                    try
                    {

                        var request = WebRequest.Create(new Uri(result)) as HttpWebRequest;

                        /* This is wrong.  this needs to be a GET request.
                         * Commenting this out 
                         * Jeff Cheung
                         * 
                        // Set the Method property of the request to POST.
                        request.Method = "POST";
                        // Create POST data and convert it to a byte array.
                        //string postData = GetPostData();
                        //byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                        // Set the ContentType property of the WebRequest.
                        request.ContentType = "application/x-www-form-urlencoded";

                        // Set the ContentLength property of the WebRequest.
                        //request.ContentLength = byteArray.Length;

                        // Get the request stream.
                        Stream dataStream = request.GetRequestStream();


                        // Write the data to the request stream.
                        //dataStream.Write(byteArray, 0, byteArray.Length);
                        // Close the Stream object.
                        dataStream.Close();
                        */
                        request.Method = "GET";
                        // Get the response.
                        WebResponse response = request.GetResponse();

                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();

                        reader.Close();
                        dataStream.Close();
                        response.Close();

                        output.AppendFormat("URL: {0}\n", result);
                        output.AppendFormat("Result: {0}\n", responseFromServer);
                        output.AppendLine();
                        // this is wrong - this is hardcoding OK
                        //postbackcontent.AppendFormat("URL: {0} OK", result);

                        postbackcontent.AppendFormat("URL: {0}\r\n Result: {1}", result, responseFromServer);
                        postbackcontent.AppendLine();




                    }
                    catch (Exception e)
                    {
                        output.AppendFormat("URL: {0}\n", result);
                        output.AppendFormat("Result: {0}\n", e.Message);

                        postbackcontent.AppendFormat("URL: {0} ERROR: {1}", result, e.Message);
                        postbackcontent.AppendLine();

                    }
                    postbackend = DateTime.Now;
                }
                // this should be result, not sb
                //pixelcontent.Append(sb);
                pixelcontent.Append(result);
                sb.Clear();
            }

            var content = pixelcontent.ToString();
            if (postback)
            {
                if (count == 0)
                {
                    conversion.StatusDescription = "no output";
                }
                else
                {
                    conversion.StatusDescription = postbackcontent.ToString();
                }

                content = conversion.StatusDescription;
            }

            string subReason = "";
            var GetSubValue = repo.ClickSubIds().Where(u => u.ClickId == click.ClickId).Select(u => u.SubValue).FirstOrDefault();
            if (GetSubValue != null)
            {
                if (GetSubValue == "-1")
                {
                    conversion.Status = 5;
                    subReason = "test sale";
                }


            }
            if (isnew)
            {
                try
                {
                    DateTime Intend = DateTime.Now;
                    TimeSpan span = Intend - Intstart;
                    int ms = (int)span.TotalMilliseconds;
                    conversion.TimeInterval = ms;
                }
                catch { }


                db.Conversions.Add(conversion);
                db.SaveChanges();
            }
            else
            {
                try
                {
                    DateTime Intend = DateTime.Now;
                    TimeSpan span = Intend - Intstart;
                    int ms = (int)span.TotalMilliseconds;
                    conversion.TimeInterval = ms;
                }
                catch { }
                db.Entry(conversion).State = System.Data.Entity.EntityState.Modified;
            }

            clog.Success = true;
            clog.Reason = String.Format("CoversionId: {0}", conversion.ConversionId);
            if (subReason != "")
            { clog.Reason = subReason; }
            clog.PixelsFound = count;
            clog.Output = output.ToString();
            try
            {

                TimeSpan span = postbackend - postbackstart;
                int ms = (int)span.TotalMilliseconds;
                clog.TimeInterval = ms;
            }
            catch
            {

            }


            db.ConversionLogs.Add(clog);


            db.SaveChanges();

            // output pixel code (accounts for multiple pixels)
            return Content("Conversion: true\n" + content, "text/html");


            // this is the original code
            //return File("~/images/clear.png", "image/png");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }
            //db.Dispose();
            base.Dispose(disposing);
        }
    }
}
