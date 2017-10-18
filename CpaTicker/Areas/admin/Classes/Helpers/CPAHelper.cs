using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WebMatrix.WebData;
using System.Web.Security;
using CpaTicker.Areas.admin.Models;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;
using CpaTicker.Areas.admin.Classes.SecurityLib;
using CpaTicker.Areas.admin.Classes.LimeLightLib;
using System.Collections.Specialized;
using System.Threading.Tasks;
using log4net.Core;
using System.Diagnostics;
using CpaTicker.Models;
using System.Globalization;
using System.Threading;

namespace CpaTicker.Areas.admin.Classes.Helpers
{
    public static class CPAHelper
    {
        public static string GetLastReport { get; set; }

        public static TypeBuilder CreateTypeBuilder(string assemblyName, string moduleName, string typeName)
        {
            TypeBuilder typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName),
                                       AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        public static void CreateAutoImplementedProperty(TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(
                string.Concat(PrivateFieldPrefix, propertyName),
                              propertyType, FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(
                propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            MethodAttributes propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(GetterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(
                string.Concat(SetterPrefix, propertyName),
                propertyMethodAttributes, null, new Type[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }

        public static string GetGrossCost(int grouptype)
        {
            return "";
        }

        public static List<Campaign> GetCampaigns(int customerid)
        {
            CpaTickerDb db = new CpaTickerDb();
            List<Campaign> campaigns = db.Campaigns.Where(c => c.CustomerId == customerid).ToList();
            return campaigns;
        }

        public static double GetClickRevenue(DateTime FromDate, DateTime ToDate, int customerid, int campaignid)
        {
            CpaTickerDb db = new CpaTickerDb();
            double clicks = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == campaignid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
            return clicks;
        }

        public static double GetConversionRevenue(DateTime FromDate, DateTime ToDate, int customerid, int campaignid)
        {
            CpaTickerDb db = new CpaTickerDb();
            double conversions = db.Conversions.Where(co => co.CustomerId == customerid && co.CampaignId == campaignid && co.ConversionDate >= FromDate && co.ConversionDate < ToDate && co.Status == 1).Sum(co => (double?)co.Revenue) ?? 0.0;
            return conversions;
        }

        public static double GetImpressionRevenue(DateTime FromDate, DateTime ToDate, int customerid, int campaignid)
        {
            CpaTickerDb db = new CpaTickerDb();
            double impressions = db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == campaignid && im.ImpressionDate >= FromDate & im.ImpressionDate < ToDate).Sum(im => (double?)im.Revenue) ?? 0.0;
            return impressions;
        }

        public static string GetRevenueByCampaign(DateTime FromDate, DateTime ToDate, int customerid)
        {
            CpaTickerDb db = new CpaTickerDb();

            return "";
        }

        public static int GetConversions(DateTime FromDate, DateTime ToDate, int customerid, int campaignid = 0)
        {
            CpaTickerDb db = new CpaTickerDb();
            if (campaignid == 0)
            {
                return db.Conversions.Count(co => co.CustomerId == customerid && co.ConversionDate >= FromDate && co.ConversionDate <= ToDate && co.Status == 1);
            }
            else
            {
                return db.Conversions.Count(co => co.CustomerId == customerid && co.ConversionDate >= FromDate && co.ConversionDate <= ToDate && co.CampaignId == campaignid && co.Status == 1);
            }
        }

        public static int GetClicks(DateTime FromDate, DateTime ToDate, int customerid, int campaignid = 0)
        {
            CpaTickerDb db = new CpaTickerDb();

            if (campaignid == 0)
            {
                return db.Clicks.Count(cl => cl.CustomerId == customerid && cl.ClickDate >= FromDate && cl.ClickDate <= ToDate);
            }
            else
            {
                return db.Clicks.Count(cl => cl.CustomerId == customerid && cl.ClickDate >= FromDate && cl.ClickDate <= ToDate && cl.CampaignId == campaignid);
            }
        }

        public static Campaign GetCampaignById(int campaignId, int customerId)
        {
            CpaTickerDb db = new CpaTickerDb();
            Campaign campaign = db.Campaigns.Single(cp => cp.CampaignId == campaignId && cp.CustomerId == customerId);
            return campaign;
        }

        public static int GetCustomerId(int userId)
        {
            CpaTickerDb db = new CpaTickerDb();
            UserProfile userProfile = db.UserProfiles.Single(up => up.UserId == userId);
            int customerId = userProfile.CustomerId;
            db.Dispose();
            return customerId;
        }

        public static int GetCustomerIdFromAPIKey(string apikey)
        {
            Guid APIKey = Guid.Parse(apikey);
            CpaTickerDb db = new CpaTickerDb();
            Customer customer = db.Customers.SingleOrDefault(c => c.APIKey == APIKey);
            int customerId = -1;
            if (customer != null)
            {
                customerId = customer.CustomerId;
            }

            db.Dispose();
            return customerId;
        }

        public static int GetCustomerIdFromSubDomain(string Url)
        {
            CpaTickerDb db = new CpaTickerDb();
            Customer customer = db.Customers.Single(c => c.AccountId == Url);
            int customerid = 0;
            if (customer != null)
            {
                customerid = customer.CustomerId;
            }
            db.Dispose();
            return customerid;
        }

        public static string GetSubDomain(Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                if (host.Split('.').Length > 2)
                {
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);
                    return host.Substring(0, index);
                }
            }
            return null;
        }

        /****************************************************** njhones code **********************************/

        public static void SendAPIKeyMail(UserProfile user)
        {
            //var subject = "A new APIkey has been set for you";
            //var to = user.Email;
            //var from = CpaTickerConfiguration.MailTo;
            //var body = user.APIKey.ToString();

            //SendMail(from, to, subject, body);
            SendAPIKeyMail(user.UserName, user.Email, user.APIKey.ToString());
        }

        public static void SendAPIKeyMail(string username, string email, string apikey)
        {
            var subject = "Your ClickTicker.com API Key";
            var to = email;
            //var from = CpaTickerConfiguration.MailTo;
            var from = "noreply@clickticker.com";
            // var from = "jeff@clickticker.com";

            //var body = "Hi " + username + "," + Environment.NewLine + Environment.NewLine +
            //            "Please find your API key below.Just copy and paste this into your mobile app," + Environment.NewLine +
            //            "and enjoy your real time tracking on your mobile device!" + Environment.NewLine + Environment.NewLine +
            //            "APIKey : " + apikey + Environment.NewLine + Environment.NewLine +
            //            "Best," + Environment.NewLine +
            //            "ClickTicker";
            var body = "Please find your API key below.  Just copy and paste this into your mobile app," + Environment.NewLine +
                        "and enjoy your real time tracking on your mobile device!" + Environment.NewLine + "Get the latest ClickTicker mobile version" + Environment.NewLine + "iOS: https://itunes.apple.com/us/app/clickticker/id913987705" + Environment.NewLine + "Android: https://play.google.com/store/apps/details?id=odesk.johnlife.ticker" + Environment.NewLine + Environment.NewLine +
                        "APIKey : " + apikey + Environment.NewLine + Environment.NewLine +
                        "Best," + Environment.NewLine +
                        "ClickTicker";

            SendMail(from, to, subject, body);
        }

        public static void SendUserOrPassMail(UserProfile user, string password)
        {
            var subject = "Your ClickTicker.com account";
            var to = user.Email;
            var from = "noreply@clickticker.com";

            var body = "Dear " + user.UserName + Environment.NewLine + Environment.NewLine +
                        "Please find your credentials below." + Environment.NewLine + Environment.NewLine +
                        "URL:" + "http://www.clickticker.com/account/login" + Environment.NewLine +
                        "UserName:" + user.UserName + Environment.NewLine +
                        "Password:" + password + Environment.NewLine + Environment.NewLine +
                        "You will be required to change your password after you login." + Environment.NewLine + Environment.NewLine +
                        "Best," + Environment.NewLine + Environment.NewLine +
                        "ClickTicker";



            SendMail(from, to, subject, body);
        }

        public static void SendEmailNewRegisterEmail(string from, RegisterModel Registeruser)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            string frmEmail = from;
            string toemail = Registeruser.Email;
            string Subject = "Welcome to www.ClickTicker.com";
            string Body = "Dear " + textInfo.ToTitleCase(Registeruser.FirstName) + Environment.NewLine + Environment.NewLine +
                          "Thank you, for choosing www.ClickTicker.com." + Environment.NewLine + Environment.NewLine +
                          "You have started your 45 day ClickTicker Free Trial." + Environment.NewLine + Environment.NewLine +
                          "One of our Senior Account Executives will call you by the next business day to help with your campaign set up." + Environment.NewLine +
                          "If you would like to schedule a specific time to speak, simply “REPLY” to this email." + Environment.NewLine + Environment.NewLine +
                          "Happy Tracking and More Power!" + Environment.NewLine + Environment.NewLine +
                          "ClickTicker Support" + Environment.NewLine + Environment.NewLine +
                          "support@clickticker.com" + Environment.NewLine +
                          "1-888-683-4872" + Environment.NewLine;

            SendMail(frmEmail, toemail, Subject, Body);
        }

        public static void SendEmailClickTickerSupport(string from, RegisterModel Registeruser, string ipaddress)
        {

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            string frmEmail = from;
            string toemail = "support@clickticker.com";
            string Subject = "ClickTicker.com New Customer Signup";
            string Body = "A new customer has signed up" + Environment.NewLine + Environment.NewLine +
                          "Name: " + textInfo.ToTitleCase(Registeruser.FirstName) + Environment.NewLine +
                          "Phone: " + Registeruser.Phone + Environment.NewLine +
                          "Email: " + Registeruser.Email + Environment.NewLine +
                          "Address: " + Registeruser.Address + Environment.NewLine +
                          "IP: " + ipaddress + Environment.NewLine;

            SendMail(frmEmail, toemail, Subject, Body);
        }

        public static void SendEmailForDemo(string from, RequestDemo model)
        {

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            string frmEmail = from;
            string toemail = "support@clickticker.com";
            string Subject = "Request a Demo";
            string Body =
                          "FirstName: " + textInfo.ToTitleCase(model.FirstName) + Environment.NewLine +
                         "LastName: " + textInfo.ToTitleCase(model.LastName) + Environment.NewLine +
                         "Email: " + model.Email + Environment.NewLine +
                         "Phone: " + model.Phone + Environment.NewLine +
                         "Message: " + model.Message + Environment.NewLine;

            SendMail(frmEmail, toemail, Subject, Body);
        }
        public static void SendEmailForAffiliateSignup(string from, Affiliate model,string Toemail)
        {
            //CpaTickerDb db = new CpaTickerDb();

            //var AdminList = db.UserProfiles.Where(u => u.CustomerId == model.CustomerId && u.AffiliateId == null && u.Email != null).Select(u => u.Email).ToArray();
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            string frmEmail = from;
            //string toemail = string.Join(",", AdminList);
            string Subject = "Affiliate Sign Up";
            string Body =
                          "Company: " + textInfo.ToTitleCase(model.Company) + Environment.NewLine +
                         "Address1: " + textInfo.ToTitleCase(model.Address1) + Environment.NewLine +
                         "Address2: " + model.Address2 + Environment.NewLine +
                         "Phone: " + model.Phone + Environment.NewLine +
                         "Fax: " + model.Fax + Environment.NewLine +
                         "City: " + model.City + Environment.NewLine +
                         "PostalCode: " + model.PostalCode + Environment.NewLine +
                         "Country: " + model.Country.Name + Environment.NewLine +
                         "State: " + model.State + Environment.NewLine;

            SendMail(frmEmail, Toemail, Subject, Body);
        }

        // Generic method for sending emails
        public static void SendMail(string from, string to, string subject, string body)
        {
            try
            {
                // Configure mail client
                SmtpClient mailClient = new SmtpClient(CpaTickerConfiguration.MailServer, CpaTickerConfiguration.MailPort);
                // Set credentials (for SMTP servers that require authentication)
                mailClient.Credentials = new NetworkCredential(CpaTickerConfiguration.MailUsername, CpaTickerConfiguration.MailPassword);
                // Enable SSL
                mailClient.EnableSsl = CpaTickerConfiguration.MailEnableSsl;
                // Create the mail message
                MailMessage mailMessage = new MailMessage(from, to, subject, body);
                // Send mail
                mailClient.Send(mailMessage);
            }

            catch (Exception ex)
            {
                // mail fail log the email fail problem 

                //log4net.ILog log = log4net.LogManager.GetLogger("SendEmail");
                //log.Error(exception.Message, exception);

                log4net.ILog log = log4net.LogManager.GetLogger("SendEmail");

                Type declaringType = typeof(log4net.LogManager);

                LoggingEvent loggingEvent = new LoggingEvent(declaringType, log.Logger.Repository, log.Logger.Name, log4net.Core.Level.Error, ex.Message, ex); //null = Message, Exception    

                var stacktrace = new StackTrace(ex, true);
                var frame = stacktrace.GetFrame(0);

                //int l = frame.GetFileLineNumber();
                //var f = frame.GetFileName();

                loggingEvent.Properties["line_number"] = frame.GetFileLineNumber();
                loggingEvent.Properties["file_name"] = frame.GetFileName();

                log.Logger.Log(loggingEvent);


            }

        }

        internal static uint IpToUInt32(string ipAddress)
        {
            return BitConverter.ToUInt32(System.Net.IPAddress.Parse(ipAddress).GetAddressBytes().Reverse().ToArray(), 0);
        }

        internal static TickerSetting GetDefaultTickerSettings()
        {
            return new TickerSetting()
            {
                BackgroundColor = CpaTickerConfiguration.BackgroundColor,
                CampaignColor = CpaTickerConfiguration.CampaignColor,
                ImpressionColor = CpaTickerConfiguration.ImpressionColor,
                ClickColor = CpaTickerConfiguration.ClickColor,
                ConversionColor = CpaTickerConfiguration.ConversionColor,
                CostColor = CpaTickerConfiguration.CostColor,
                RevenueColor = CpaTickerConfiguration.RevenueColor,
                UserId = 0
            };
        }

        internal static Ticker GetDefaultTicker()
        {
            return new Ticker()
            {
                BackgroundColor = CpaTickerConfiguration.BackgroundColor,
                CampaignColor = CpaTickerConfiguration.CampaignColor,
                ImpressionColor = CpaTickerConfiguration.ImpressionColor,
                ClickColor = CpaTickerConfiguration.ClickColor,
                ConversionColor = CpaTickerConfiguration.ConversionColor,

                //CostColor = CpaTickerConfiguration.CostColor,
                //RevenueColor = CpaTickerConfiguration.RevenueColor,

                TickerElements = new List<TickerElement>(),
            };
        }

        public static void SetTimeBasedonVD(ref string viewdata, ref DateTime? FromDate, ref DateTime? ToDate, int customerId, string timezone = "")
        {
            EFCpatickerRepository repo = new EFCpatickerRepository();

            // string timezone = "Eastern Standard Time";
            var customer = repo.GetCurrentCustomer(customerId);
            var tzi = repo.FindTimeZoneInfo(timezone, customer.TimeZone);
            var ServerTimeZone = repo.FindTimeZoneInfo(TimeZone.CurrentTimeZone.StandardName, customer.TimeZone);
            DateTime MasterDate = new DateTimeOffset(DateTime.Now.Ticks, ServerTimeZone.GetUtcOffset(DateTime.Now)).UtcDateTime;
            MasterDate = ConvertFromUtc(MasterDate, tzi.Id).Date;

            // if viewdata is null and any of the datetime values are null
            if (string.IsNullOrEmpty(viewdata) && (FromDate.HasValue == false || ToDate == null))
                //viewdata = "Last 7 Days";
                viewdata = "Today";

            // prededined values
            if (!string.IsNullOrEmpty(viewdata))
            {
                ToDate = MasterDate.AddDays(1).AddSeconds(-1);

                switch (viewdata)
                {
                    case "Today":
                        FromDate = MasterDate.Date;
                        break;
                    case "Yesterday":
                        FromDate = MasterDate.Date.AddDays(-1);
                        ToDate = MasterDate.AddSeconds(-1);
                        break;
                    case "Last 7 Days":
                        FromDate = MasterDate.AddDays(-6);
                        break;
                    case "This Month":
                        FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        break;
                    case "Last Month":
                        FromDate = new DateTime(DateTime.Now.Month - 1 <= 0 ? DateTime.Now.Year - 1 : DateTime.Now.Year, DateTime.Now.Month - 1 <= 0 ? 12 : DateTime.Now.Month - 1, 1);
                        ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                        break;
                    case "Last Six Months":
                        FromDate = new DateTime(DateTime.Now.Month - 5 <= 0 ? DateTime.Now.Year - 1 : DateTime.Now.Year, DateTime.Now.Month - 5 <= 0 ? 12 + (DateTime.Now.Month - 5) : DateTime.Now.Month - 5, 1);
                        break;
                    case "All Time on reports":
                        FromDate = repo.GetFirstOperation(customerId); //CPAHelper.GetFirstOperation(customerId);
                        break;
                }
            }

            if (FromDate == null || ToDate == null)
            {
                viewdata = "Today";
                FromDate = MasterDate.AddDays(-1);
                ToDate = MasterDate.AddDays(1).AddSeconds(-1);
            }
            //if (!ToDate.HasValue)
            //    ToDate = DateTime.Today.Date.AddDays(1).AddSeconds(-1);
            //else
            //    ToDate = ToDate.Value;

        }

        public static IEnumerable<ConversionLog> GetConversionLogs()
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.ConversionLogs.OrderByDescending(c => c.ConversionDate).Take(100).ToList();
        }

        public static DateTime ConvertFromUtc(DateTime utctime, string tzid)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utctime, TimeZoneInfo.FindSystemTimeZoneById(tzid));
        }

        public static DateTimeOffset ConvertUtcTimeToTimeZone(this DateTime dateTime, string toTimeZoneDesc)
        {
            if (dateTime.Kind != DateTimeKind.Utc) throw new Exception("dateTime needs to have Kind property set to Utc");
            var toUtcOffset = TimeZoneInfo.FindSystemTimeZoneById(toTimeZoneDesc).GetUtcOffset(dateTime);
            var convertedTime = DateTime.SpecifyKind(dateTime.Add(toUtcOffset), DateTimeKind.Unspecified);
            return new DateTimeOffset(convertedTime, toUtcOffset);
        }

        public static IEnumerable<SelectListItem> GetModeList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "hour", Value = "0"},
                new SelectListItem{Text = "day", Value = "1"},
                new SelectListItem{Text = "week", Value = "2"},
                new SelectListItem{Text = "month", Value = "3"},
                new SelectListItem{Text = "year", Value = "4"}
            };
            return items;
        }

        //public static IEnumerable<SelectListItem> GetCustomerCampaigns(int customerid = 0)
        //{
        //    CpaTickerDb db = new CpaTickerDb();
        //    if (customerid == 0)
        //        customerid = db.UserProfiles.Single(u => u.UserId == WebMatrix.WebData.WebSecurity.CurrentUserId).CustomerId;
        //    return db.Campaigns.Where(p => p.CustomerId == customerid).OrderBy(p => p.CampaignName).AsEnumerable().Select(p => new SelectListItem { Value = p.CampaignId.ToString(), Text = p.CampaignName });
        //}

        public static IEnumerable<SelectListItem> GetCountriesDdl()
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.Countries.OrderBy(p => p.Name).AsEnumerable().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name });
        }

        /* chechout the comments inside this feature */
        public static List<Domain> GetCustomerDomains(int customerid)
        {
            CpaTickerDb db = new CpaTickerDb();

            // this works!
            //return (from domains in db.Domains
            //        join customerdomains in db.CustomerDomains on domains.Id equals customerdomains.DomainId
            //        where customerdomains.CustomerId == customerid 
            //        select domains).ToList();

            // this works too!? yes
            //return (from domains in db.Domains
            //        from customerdomains in db.CustomerDomains
            //        where customerdomains.CustomerId == customerid && domains.Id == customerdomains.DomainId
            //        select domains).ToList();

            // this must work as well?
            //var customerdomains = db.CustomerDomains.Where(cd => cd.CustomerId == customerid).Select(cd => cd.DomainId);
            //return db.Domains.Where(d => customerdomains.Contains(d.Id)).ToList();

            // this is where i want to go 
            return db.CustomerDomains.Where(cd => cd.CustomerId == customerid).Join(db.Domains, cd => cd.DomainId, dom => dom.Id, (cd, dom) => dom).ToList();

        }

        public static IEnumerable<EditTickerViewModel> GetEditTickerVieModel(int tickerid)
        {
            CpaTickerDb db = new CpaTickerDb();
            return from c in db.Campaigns
                   join t in db.TickerCampaigns.Where(t => t.TickerId == tickerid) on c equals t.Campaign into r
                   from t in r.DefaultIfEmpty()
                   select new EditTickerViewModel { CamapaignId = c.Id, CamapaignName = c.CampaignName, HasCampaign = t != null };
        }

        public static string GetDomainName(int id)
        {
            CpaTickerDb db = new CpaTickerDb();
            Domain dom = db.Domains.Single(d => d.Id == id);
            db.Dispose();
            return dom.DomainName;
        }

        public static bool IsDomainExists(string name)
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.Domains.Where(d => d.DomainName == name).Count() > 0;
        }

        public static List<int> ToListofInt(string s)
        {
            var l = new List<int>();
            try
            {
                string[] arr = s.Split(',');
                foreach (var item in arr)
                {
                    l.Add(int.Parse(item));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Data: " + s);

            }
            return l;
        }

        //public static bool EmailExists(string email)
        //{
        //    CpaTickerDb db = new CpaTickerDb();
        //    return db.Customers.Count(c => c.Email == email) > 0;
        //}

        //public static bool EmailExistsNotMine(string email)
        //{
        //    CpaTickerDb db = new CpaTickerDb();
        //    int cid = GetCustomerId(WebSecurity.CurrentUserId);
        //    return db.Customers.Where(c  => c.CustomerId != cid).Count(c => c.Email == email) > 0;
        //}

        public static bool AccountIdExists(string accountid)
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.Customers.Count(c => c.AccountId == accountid) > 0;
        }

        public static int GetCustomerIdFromUrl(Uri url)
        {
            int customerid = 0;
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                CpaTickerDb db = new CpaTickerDb();

                if (host.Split('.').Length > 2)
                {
                    // this means that this has a subdomain
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);

                    string domain = host.Substring(index + 1);

                    if (domain.Equals(CpaTickerConfiguration.DefaultDomainName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        string subdomain = host.Substring(0, index);

                        Customer customer = db.Customers.Single(c => c.AccountId == subdomain);
                        if (customer != null)
                        {
                            customerid = customer.CustomerId;
                        }
                    }
                    else
                    {
                        customerid = db.Domains.Where(d => d.DomainName == host).Join(db.CustomerDomains, d => d.Id, cd => cd.DomainId, (d, cd) => cd.CustomerId).Single();
                    }

                }

                else
                {
                    customerid = db.Domains.Where(d => d.DomainName == host).Join(db.CustomerDomains, d => d.Id, cd => cd.DomainId, (d, cd) => cd.CustomerId).Single();
                }
                db.Dispose();
            }

            return customerid;

        }

        public static int GetCustomerIdFromUrlCopy(Uri url)
        {
            int customerid = 0;
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                CpaTickerDb db = new CpaTickerDb();
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
                db.Dispose();
            }

            return customerid;
        }

        public static Domain GetDefaultDomain(int cid)
        {
            Domain result = new Domain();

            CpaTickerDb db = new CpaTickerDb();
            string accountId = db.Customers.Single(c => c.CustomerId == cid).AccountId;
            db.Dispose();
            result.DomainName = string.Format("{1}.{0}", CpaTickerConfiguration.DefaultDomainName, accountId);
            result.Id = CpaTickerConfiguration.DefaultDomainId;
            return result;
        }

        public static List<UserProfile> GetCustomerUsers(int customerId)
        {
            CpaTickerDb db = new CpaTickerDb();
            List<UserProfile> _culist = db.UserProfiles.Where(up => up.CustomerId == customerId).ToList();
            db.Dispose();
            return _culist;
        }

        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        /// <summary>
        /// Get the affiliateid it should exist since i only call this from a user that 
        /// belongs to the Affiliate role thats why there is no validation if the affiliateid hasvalue
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetAffiliateId(int userId)
        {
            CpaTickerDb db = new CpaTickerDb();
            UserProfile userProfile = db.UserProfiles.Single(up => up.UserId == userId);
            int affiliateId = userProfile.AffiliateId.Value;
            db.Dispose();
            return affiliateId;
        }

        public static UserProfile GetCurrentUserProfile(int id)
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.UserProfiles.SingleOrDefault(up => up.UserId == id);
        }

        public static long GetUserPermissions(int userid)
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.UserProfiles.Where(up => up.UserId == userid).Single().Permissions;
        }

        public static long GetUserPermissions1(int userid)
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.UserProfiles.Where(up => up.UserId == userid).Single().Permissions1;
        }

        public static Type GetEnumType()
        {
            var assembly = System.Reflection.Assembly.Load("EnumAssembly");
            return assembly.GetType("ViewPermissiond");
        }

        public static Type GetEnumType1()
        {
            var assembly = System.Reflection.Assembly.Load("EnumAssembly");
            return assembly.GetType("ViewPermission");
        }

        internal static Affiliate GetAffiliate(int customerid, int affiliateid)
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.Affiliates.Where(a => a.CustomerId == customerid && a.AffiliateId == affiliateid).Single();
        }

        public static int GetCustomerIdFromUsername(string username)
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.UserProfiles.Single(up => up.UserName == username).CustomerId;
        }

        public static int GetCustomerDeclines(int customerid)
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.Orders.Single(o => o.CustomerId == customerid).Declines;
        }

        public static int GetCustomerOrder(int customerid)
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.Orders.Single(o => o.CustomerId == customerid).OrderId;
        }

        public static string GetStateName(string statecode)
        {
            CpaTickerDb db = new CpaTickerDb();
            var state = db.States.SingleOrDefault(s => s.StateCode == statecode);
            if (state != null)
            {
                return state.StateName;
            }
            return statecode;
        }

        public static bool CountryHasStates(int contryid)
        {
            CpaTickerDb db = new CpaTickerDb();
            return db.States.Count(s => s.CountryId == contryid) > 0;
        }

        /************************************************** yasel code ********************/

        public static Customer GetCustomer(int customerId)
        {


            CpaTickerDb db = new CpaTickerDb();

            Customer result = db.Customers.Single(cus => cus.CustomerId == customerId);
            db.Dispose();
            return result;


        }

        public static List<ClickViewModel> GetClickLogs(DateTime FromDate)
        {
            List<ClickViewModel> result = new List<ClickViewModel>();
            for (int i = 0; i < 24; i++)
            {
                ClickViewModel click = new ClickViewModel();
                click.showtext = FromDate.ToString("MM/dd/yyyy") + " " + i + ":00";
                click.Date = FromDate.AddHours(i).Ticks.ToString();
                click.hour = i;
                result.Add(click);
            }
            return result;
        }

        //public static Models.HourlyViewModel GetHourlyReport(DateTime FromDate, string timezone, int customerid, Interval interval, Statistics Statistics, Calculation calculation)
        //{

        //    GetLastReport = "Date,Year,Month,Day,Hour,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    List<string> listmonth = new List<string>() { "Jan","Feb","Mar","Apr","May","Jun",
        //    "Jul","Aug","Sep","Oct","Nov","Dec" };
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    HourlyViewModel lhvm = new HourlyViewModel();
        //    lhvm.HourlyViewList = new List<HourlyView>();
        //    int affiliateid = CurrentUser().AffiliateId ?? 0;
        //    lhvm.Interval = interval;
        //    lhvm.Stadisctics = Statistics;
        //    lhvm.Filter = new Models.Filter();
        //    lhvm.Calculation = calculation;
        //    CpaTickerDb db = new CpaTickerDb();
        //    if (IsAdmin())
        //    {
        //        for (int i = 0; i < 24; i++)
        //        {
        //            string aux = "";
        //            DateTime comparefromDate = TimeZoneFromDate.AddHours(i);
        //            DateTime comparetoDate = TimeZoneFromDate.AddHours(i + 1).AddMilliseconds(-1);
        //            int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //            int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //            int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //            double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //            cost += db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //            double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //            revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;

        //            HourlyView hvm = new HourlyView();
        //            hvm.Impressions = impressions;
        //            hvm.Clicks = clicks;
        //            hvm.Conversions = conversions;
        //            hvm.Cost = cost;
        //            hvm.Revenue = revenue;
        //            hvm.Hour = i;
        //            hvm.Year = FromDate.Year;
        //            hvm.Day = FromDate.Day;
        //            hvm.Month = FromDate.Month;
        //            hvm.Date = FromDate.ToString("MM/dd/yyyy");
        //            lhvm.HourlyViewList.Add(hvm);
        //            aux += hvm.Date + ",";
        //            aux += hvm.Year + ",";
        //            aux += hvm.Month + ",";
        //            aux += hvm.Day + ",";
        //            aux += hvm.Hour + ",";
        //            aux += hvm.Impressions + ",";
        //            aux += hvm.Clicks + ",";
        //            aux += hvm.Conversions + ",";
        //            aux += hvm.Impressions + ",";
        //            aux += hvm.Clicks == 0 ? (hvm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)hvm.Conversions / (double)hvm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += hvm.Cost + ",";
        //            aux += hvm.Clicks == 0 ? hvm.Cost.ToString("F2").Replace(',', '.') : (hvm.Cost / hvm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += ",";
        //            aux += hvm.Revenue + ",";
        //            aux += hvm.Clicks == 0 ? hvm.Revenue.ToString("F2").Replace(',', '.') : (hvm.Revenue / hvm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += "," + ((hvm.Revenue - hvm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //        }
        //        return lhvm;

        //    }
        //    else
        //    {
        //        for (int i = 0; i < 24; i++)
        //        {
        //            string aux = "";
        //            DateTime comparefromDate = TimeZoneFromDate.AddHours(i);
        //            DateTime comparetoDate = TimeZoneFromDate.AddHours(i + 1).AddMilliseconds(-1);
        //            int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //            int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.AffiliateId == affiliateid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //            int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //            double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //            cost += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //            double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //            revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;

        //            HourlyView hvm = new HourlyView();
        //            hvm.Impressions = impressions;
        //            hvm.Clicks = clicks;
        //            hvm.Conversions = conversions;
        //            hvm.Cost = cost;
        //            hvm.Revenue = revenue;
        //            hvm.Hour = i;
        //            hvm.Year = FromDate.Year;
        //            hvm.Day = FromDate.Day;
        //            hvm.Date = FromDate.ToString("MM/dd/yyyy");
        //            hvm.Month = FromDate.Month;
        //            lhvm.HourlyViewList.Add(hvm);
        //            aux += hvm.Date + ",";
        //            aux += hvm.Year + ",";
        //            aux += hvm.Month + ",";
        //            aux += hvm.Day + ",";
        //            aux += hvm.Hour + ",";
        //            aux += hvm.Impressions + ",";
        //            aux += hvm.Clicks + ",";
        //            aux += hvm.Conversions + ",";
        //            aux += hvm.Impressions + ",";
        //            aux += hvm.Clicks == 0 ? (hvm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)hvm.Conversions / (double)hvm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += hvm.Cost + ",";
        //            aux += hvm.Clicks == 0 ? hvm.Cost.ToString("F2").Replace(',', '.') : (hvm.Cost / hvm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += ",";
        //            aux += hvm.Revenue + ",";
        //            aux += hvm.Clicks == 0 ? hvm.Revenue.ToString("F2").Replace(',', '.') : (hvm.Revenue / hvm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += "," + ((hvm.Revenue - hvm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //        }

        //        return lhvm;
        //    }
        //}

        //public static Models.HourlyViewModel GetHourlyReport(DateTime FromDate, string timezone, int customerid, Interval interval, Statistics Statistics, Calculation calculation, CpaTicker.Areas.admin.Models.Filter filter, List<object> filtervalues)
        //{

        //    GetLastReport = "Date,Year,Month,Day,Hour,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    List<string> listmonth = new List<string>() { "Jan","Feb","Mar","Apr","May","Jun",
        //    "Jul","Aug","Sep","Oct","Nov","Dec" };
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    HourlyViewModel lhvm = new HourlyViewModel();
        //    lhvm.HourlyViewList = new List<HourlyView>();

        //    lhvm.Interval = interval;
        //    lhvm.Filter = filter;
        //    lhvm.Stadisctics = Statistics;
        //    lhvm.Calculation = calculation;

        //    CpaTickerDb db = new CpaTickerDb();

        //    for (int i = 0; i < 24; i++)
        //    {
        //        string aux = "";
        //        DateTime comparefromDate = TimeZoneFromDate.AddHours(i);
        //        DateTime comparetoDate = TimeZoneFromDate.AddHours(i + 1).AddMilliseconds(-1);
        //        int clicks = 0;
        //        int conversions = 0;
        //        int impressions = 0;
        //        var listclicks = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //        var conversionlist = db.Conversions.Where(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //        var impressionlist = db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //        if (filter.Affiliate)
        //        {
        //            int affiliateid = (int)filtervalues[0];
        //            listclicks = listclicks.Where(cl => cl.AffiliateId == affiliateid);
        //            conversionlist = conversionlist.Where(con => con.AffiliateId == affiliateid);
        //            impressionlist = impressionlist.Where(im => im.AffiliateId == affiliateid);
        //        }
        //        if (filter.Campaign)
        //        {
        //            int campaignid = (int)filtervalues[1];
        //            listclicks = listclicks.Where(cl => cl.CampaignId == campaignid);
        //            conversionlist = conversionlist.Where(cl => cl.CampaignId == campaignid);
        //            impressionlist = impressionlist.Where(cl => cl.CampaignId == campaignid);

        //        }
        //        if (filter.Contries)
        //        {
        //            string contry = filtervalues[2].ToString();
        //            foreach (Affiliate Affiliate in db.Affiliates.Where(af => af.Country == contry))
        //            {
        //                listclicks = listclicks.Where(cl => cl.AffiliateId == Affiliate.AffiliateId);
        //                conversionlist = conversionlist.Where(cl => cl.AffiliateId == Affiliate.AffiliateId);
        //                impressionlist = impressionlist.Where(cl => cl.AffiliateId == Affiliate.AffiliateId);
        //            }
        //        }
        //        clicks = listclicks.Count();
        //        impressions = impressionlist.Count();
        //        conversions = conversionlist.Count();


        //        double cost = listclicks.Sum(cl => (double?)cl.Cost) ?? 0.0;
        //        cost += impressionlist.Sum(im => (double?)im.Cost) ?? 0.0;
        //        cost += conversionlist.Sum(cn => (double?)cn.Cost) ?? 0.0;
        //        double revenue = listclicks.Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //        revenue += impressionlist.Sum(im => (double?)im.Revenue) ?? 0.0;
        //        revenue += conversionlist.Sum(cn => (double?)cn.Revenue) ?? 0.0;

        //        HourlyView hvm = new HourlyView();
        //        hvm.Impressions = impressions;
        //        hvm.Clicks = clicks;
        //        hvm.Conversions = conversions;
        //        hvm.Cost = cost;
        //        hvm.Revenue = revenue;
        //        hvm.Hour = i;
        //        hvm.Year = FromDate.Year;
        //        hvm.Day = FromDate.Day;
        //        hvm.Month = FromDate.Month;
        //        hvm.Date = FromDate.ToString("MM/dd/yyyy");
        //        lhvm.HourlyViewList.Add(hvm);
        //        aux += hvm.Date + ",";
        //        aux += hvm.Year + ",";
        //        aux += hvm.Month + ",";
        //        aux += hvm.Day + ",";
        //        aux += hvm.Hour + ",";
        //        aux += hvm.Impressions + ",";
        //        aux += hvm.Clicks + ",";
        //        aux += hvm.Conversions + ",";
        //        aux += hvm.Impressions + ",";
        //        aux += hvm.Clicks == 0 ? (hvm.Conversions * 100).ToString("F2") + "," : (((double)hvm.Conversions / (double)hvm.Clicks) * 100).ToString("F2") + ",";
        //        aux += hvm.Cost + ",";
        //        aux += hvm.Clicks == 0 ? hvm.Cost.ToString("F2") : (hvm.Cost / hvm.Clicks).ToString("F2");
        //        aux += ",";
        //        aux += hvm.Revenue + ",";
        //        aux += hvm.Clicks == 0 ? hvm.Revenue.ToString("F2") : (hvm.Revenue / hvm.Clicks).ToString("F2");
        //        aux += "," + ((hvm.Revenue - hvm.Cost).ToString("F2"));
        //        GetLastReport += aux + "\n";

        //    }

        //    return lhvm;

        //}

        //public static DailyViewModel GetDailyReport(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Interval interval, Statistics Statistics, Calculation calculation)
        //{
        //    GetLastReport = "Date,Year,Month,Day,Hour,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    DailyViewModel ldvm = new DailyViewModel();
        //    ldvm.DailyViewList = new List<DailyView>();
        //    ldvm.Interval = interval;
        //    ldvm.Filter = new Models.Filter();
        //    ldvm.Stadisctics = Statistics;
        //    ldvm.Calculation = calculation;

        //    int affiliateid = CurrentUser().AffiliateId ?? 0;
        //    if (IsAdmin())
        //    {
        //        for (int i = 0; i < ToDate.Subtract(FromDate).Days + 1; i++)
        //        {
        //            DateTime comparefromDate = TimeZoneFromDate.AddDays(i);
        //            DateTime comparetoDate = TimeZoneFromDate.AddDays(i + 1).AddMilliseconds(-1);
        //            int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //            int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //            int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //            double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //            cost += db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //            double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //            revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;

        //            DailyView dvm = new DailyView();
        //            dvm.Impressions = impressions;
        //            dvm.Clicks = clicks;
        //            dvm.Conversions = conversions;
        //            dvm.Cost = cost;
        //            dvm.Month = comparefromDate.Month;
        //            dvm.Day = comparefromDate.Day;
        //            dvm.Year = comparefromDate.Year;
        //            dvm.Revenue = revenue;
        //            dvm.Date = comparefromDate.ToString("MM/dd/yyyy");
        //            ldvm.DailyViewList.Add(dvm);

        //            string aux = "";
        //            aux += dvm.Date + ",";
        //            aux += dvm.Year + ",";
        //            aux += dvm.Month + ",";
        //            aux += dvm.Day + ",";
        //            aux += dvm.Hour + ",";
        //            aux += dvm.Impressions + ",";
        //            aux += dvm.Clicks + ",";
        //            aux += dvm.Conversions + ",";
        //            aux += dvm.Impressions + ",";
        //            aux += dvm.Clicks == 0 ? (dvm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)dvm.Conversions / (double)dvm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += dvm.Cost + ",";
        //            aux += dvm.Clicks == 0 ? dvm.Cost.ToString("F2") : (dvm.Cost / dvm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += ",";
        //            aux += dvm.Revenue + ",";
        //            aux += dvm.Clicks == 0 ? dvm.Revenue.ToString("F2").Replace(',', '.') : (dvm.Revenue / dvm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += "," + ((dvm.Revenue - dvm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //        }

        //        return ldvm;
        //    }
        //    else
        //    {
        //        for (int i = 0; i < ToDate.Subtract(FromDate).Days + 1; i++)
        //        {
        //            DateTime comparefromDate = TimeZoneFromDate.AddDays(i);
        //            DateTime comparetoDate = TimeZoneFromDate.AddDays(i + 1).AddMilliseconds(-1);
        //            int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //            int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.AffiliateId == affiliateid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //            int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //            double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //            cost += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //            double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //            revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //            DailyView dvm = new DailyView();
        //            dvm.Impressions = impressions;
        //            dvm.Clicks = clicks;
        //            dvm.Month = comparefromDate.Month;
        //            dvm.Day = comparefromDate.Day;
        //            dvm.Year = comparefromDate.Year;
        //            dvm.Conversions = conversions;
        //            dvm.Cost = cost;
        //            dvm.Revenue = revenue;
        //            dvm.Date = comparefromDate.ToString("MM/dd/yyyy");
        //            ldvm.DailyViewList.Add(dvm);
        //            string aux = "";
        //            aux += dvm.Date + ",";
        //            aux += dvm.Year + ",";
        //            aux += dvm.Month + ",";
        //            aux += dvm.Day + ",";
        //            aux += dvm.Hour + ",";
        //            aux += dvm.Impressions + ",";
        //            aux += dvm.Clicks + ",";
        //            aux += dvm.Conversions + ",";
        //            aux += dvm.Impressions + ",";
        //            aux += dvm.Clicks == 0 ? (dvm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)dvm.Conversions / (double)dvm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += dvm.Cost + ",";
        //            aux += dvm.Clicks == 0 ? dvm.Cost.ToString("F2") : (dvm.Cost / dvm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += ",";
        //            aux += dvm.Revenue + ",";
        //            aux += dvm.Clicks == 0 ? dvm.Revenue.ToString("F2").Replace(',', '.') : (dvm.Revenue / dvm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += "," + ((dvm.Revenue - dvm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //        }

        //        return ldvm;
        //    }
        //}

        //public static DailyViewModel GetDailyReport(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Interval interval, Statistics Statistics, Calculation calculation, CpaTicker.Areas.admin.Models.Filter filter, List<object> filtervalues)
        //{
        //    GetLastReport = "Date,Year,Month,Day,Hour,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    DailyViewModel ldvm = new DailyViewModel();
        //    ldvm.DailyViewList = new List<DailyView>();
        //    ldvm.Interval = interval;
        //    ldvm.Filter = filter;
        //    ldvm.Stadisctics = Statistics;
        //    ldvm.Calculation = calculation;


        //    for (int i = 0; i < ToDate.Subtract(FromDate).Days + 1; i++)
        //    {
        //        DateTime comparefromDate = TimeZoneFromDate.AddDays(i);
        //        DateTime comparetoDate = TimeZoneFromDate.AddDays(i + 1).AddMilliseconds(-1);

        //        int clicks = 0;
        //        int conversions = 0;
        //        int impressions = 0;
        //        var listclicks = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //        var conversionlist = db.Conversions.Where(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //        var impressionlist = db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //        if (filter.Affiliate)
        //        {
        //            int affiliateid = (int)filtervalues[0];
        //            listclicks = listclicks.Where(cl => cl.AffiliateId == affiliateid);
        //            conversionlist = conversionlist.Where(con => con.AffiliateId == affiliateid);
        //            impressionlist = impressionlist.Where(im => im.AffiliateId == affiliateid);
        //        }
        //        if (filter.Campaign)
        //        {
        //            int campaignid = (int)filtervalues[1];
        //            listclicks = listclicks.Where(cl => cl.CampaignId == campaignid);
        //            conversionlist = conversionlist.Where(cl => cl.CampaignId == campaignid);
        //            impressionlist = impressionlist.Where(cl => cl.CampaignId == campaignid);

        //        }
        //        if (filter.Contries)
        //        {
        //            string contry = filtervalues[2].ToString();
        //            foreach (Affiliate Affiliate in db.Affiliates.Where(af => af.Country == contry))
        //            {
        //                listclicks = listclicks.Where(cl => cl.AffiliateId == Affiliate.AffiliateId);
        //                conversionlist = conversionlist.Where(cl => cl.AffiliateId == Affiliate.AffiliateId);
        //                impressionlist = impressionlist.Where(cl => cl.AffiliateId == Affiliate.AffiliateId);
        //            }
        //        }
        //        clicks = listclicks.Count();
        //        impressions = impressionlist.Count();
        //        conversions = conversionlist.Count();


        //        double cost = listclicks.Sum(cl => (double?)cl.Cost) ?? 0.0;
        //        cost += impressionlist.Sum(im => (double?)im.Cost) ?? 0.0;
        //        cost += conversionlist.Sum(cn => (double?)cn.Cost) ?? 0.0;
        //        double revenue = listclicks.Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //        revenue += impressionlist.Sum(im => (double?)im.Revenue) ?? 0.0;
        //        revenue += conversionlist.Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //        DailyView dvm = new DailyView();
        //        dvm.Impressions = impressions;
        //        dvm.Clicks = clicks;
        //        dvm.Month = comparefromDate.Month;
        //        dvm.Day = comparefromDate.Day;
        //        dvm.Year = comparefromDate.Year;
        //        dvm.Conversions = conversions;
        //        dvm.Cost = cost;
        //        dvm.Revenue = revenue;
        //        dvm.Date = comparefromDate.ToString("MM/dd/yyyy");
        //        ldvm.DailyViewList.Add(dvm);
        //        string aux = "";
        //        aux += dvm.Date + ",";
        //        aux += dvm.Year + ",";
        //        aux += dvm.Month + ",";
        //        aux += dvm.Day + ",";
        //        aux += dvm.Hour + ",";
        //        aux += dvm.Impressions + ",";
        //        aux += dvm.Clicks + ",";
        //        aux += dvm.Conversions + ",";
        //        aux += dvm.Impressions + ",";
        //        aux += dvm.Clicks == 0 ? (dvm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)dvm.Conversions / (double)dvm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //        aux += dvm.Cost + ",";
        //        aux += dvm.Clicks == 0 ? dvm.Cost.ToString("F2") : (dvm.Cost / dvm.Clicks).ToString("F2").Replace(',', '.');
        //        aux += ",";
        //        aux += dvm.Revenue + ",";
        //        aux += dvm.Clicks == 0 ? dvm.Revenue.ToString("F2").Replace(',', '.') : (dvm.Revenue / dvm.Clicks).ToString("F2").Replace(',', '.');
        //        aux += "," + ((dvm.Revenue - dvm.Cost).ToString("F2")).Replace(',', '.');
        //        GetLastReport += aux + "\n";
        //    }

        //    return ldvm;

        //}

        //public static AdCampaignViewModel GetAdCampaignReport(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics staticstic, Calculation calculation)
        //{
        //    GetLastReport = "Name,Campaign,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    AdCampaignViewModel ldvm = new AdCampaignViewModel();
        //    ldvm.AdCampaignViewList = new List<AdCampaignView>();
        //    ldvm.Statistics = staticstic;
        //    ldvm.Calculation = calculation;
        //    ldvm.Filter = new Models.Filter();

        //    foreach (Banner bam in db.Banners.Where(ba => ba.CustomerId == customerid).ToList())
        //    {
        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);

        //        int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate && cl.BannerId != null && cl.BannerId == bam.BannerId);
        //        int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate && co.BannerId != null && co.BannerId == bam.BannerId);
        //        int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate && im.BannerId != null && im.BannerId == bam.BannerId);
        //        double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate && cl.BannerId != null && cl.BannerId == bam.BannerId).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //        cost += db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate && im.BannerId != null && im.BannerId == bam.BannerId).Sum(im => (double?)im.Cost) ?? 0.0;
        //        cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate && cn.BannerId != null && cn.BannerId == bam.BannerId).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //        double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate && cl.BannerId != null && cl.BannerId == bam.BannerId).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //        revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate && im.BannerId != null && im.BannerId == bam.BannerId).Sum(im => (double?)im.Revenue) ?? 0.0;
        //        revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate && cn.BannerId != null && cn.BannerId == bam.BannerId).Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //        AdCampaignView adv = new AdCampaignView();
        //        adv.CampaignName = db.Campaigns.First(ca => ca.CustomerId == customerid && ca.CampaignId == bam.CampaignId).CampaignName;
        //        adv.Name = bam.Name;
        //        adv.Clicks = clicks;
        //        adv.Impressions = impressions;
        //        adv.Conversions = conversions;
        //        adv.Cost = cost;
        //        adv.Revenue = revenue;
        //        string aux = "";
        //        ldvm.AdCampaignViewList.Add(adv);
        //        aux += adv.Name + ',';
        //        aux += adv.CampaignName + ",";
        //        aux += adv.Impressions + ",";
        //        aux += adv.Clicks + ",";
        //        aux += adv.Conversions + ",";
        //        aux += adv.Impressions + ",";
        //        aux += adv.Clicks == 0 ? (adv.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)adv.Conversions / (double)adv.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //        aux += adv.Cost + ",";
        //        aux += adv.Clicks == 0 ? adv.Cost.ToString("F2") : (adv.Cost / adv.Clicks).ToString("F2").Replace(',', '.');
        //        aux += ",";
        //        aux += adv.Revenue + ",";
        //        aux += adv.Clicks == 0 ? adv.Revenue.ToString("F2").Replace(',', '.') : (adv.Revenue / adv.Clicks).ToString("F2").Replace(',', '.');
        //        aux += "," + ((adv.Revenue - adv.Cost).ToString("F2")).Replace(',', '.');
        //        GetLastReport += aux + "\n";

        //    }
        //    return ldvm;

        //}

        //public static AdCampaignViewModel GetAdCampaignReport(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics statistic, Calculation calculation, CpaTicker.Areas.admin.Models.Filter filter, List<object> filtervalues)
        //{
        //    GetLastReport = "Name,Campaign,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    AdCampaignViewModel ldvm = new AdCampaignViewModel();
        //    ldvm.AdCampaignViewList = new List<AdCampaignView>();
        //    ldvm.Statistics = statistic;
        //    ldvm.Filter = filter;
        //    ldvm.Calculation = calculation;



        //    foreach (Banner bam in db.Banners.Where(ba => ba.CustomerId == customerid))
        //    {
        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
        //        int clicks = 0;
        //        int conversions = 0;
        //        int impressions = 0;
        //        var listclicks = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate && cl.BannerId != null && cl.BannerId == bam.BannerId);
        //        var conversionlist = db.Conversions.Where(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate && co.BannerId != null && co.BannerId == bam.BannerId);
        //        var impressionlist = db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate && im.BannerId != null && im.BannerId == bam.BannerId);

        //        if (filter.Campaign)
        //        {
        //            int campaignid = (int)filtervalues[0];
        //            //int campaignid = int.Parse(filtervalues[1].ToString());
        //            listclicks = listclicks.Where(cl => cl.CampaignId == campaignid);
        //            conversionlist = conversionlist.Where(cl => cl.CampaignId == campaignid);
        //            impressionlist = impressionlist.Where(cl => cl.CampaignId == campaignid);

        //        }

        //        clicks = listclicks.Count();
        //        impressions = impressionlist.Count();
        //        conversions = conversionlist.Count();


        //        double cost = listclicks.Sum(cl => (double?)cl.Cost) ?? 0.0;
        //        cost += impressionlist.Sum(im => (double?)im.Cost) ?? 0.0;
        //        cost += conversionlist.Sum(cn => (double?)cn.Cost) ?? 0.0;
        //        double revenue = listclicks.Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //        revenue += impressionlist.Sum(im => (double?)im.Revenue) ?? 0.0;
        //        revenue += conversionlist.Sum(cn => (double?)cn.Revenue) ?? 0.0;

        //        AdCampaignView adv = new AdCampaignView();
        //        adv.CampaignName = db.Campaigns.First(ca => ca.CustomerId == customerid && ca.CampaignId == bam.CampaignId).CampaignName;
        //        adv.Name = bam.Name;
        //        adv.Clicks = clicks;
        //        adv.Conversions = conversions;
        //        adv.Impressions = impressions;
        //        adv.Cost = cost;
        //        adv.Revenue = revenue;
        //        ldvm.AdCampaignViewList.Add(adv);
        //        string aux = "";
        //        aux += adv.Name + ',';
        //        aux += adv.CampaignName + ",";
        //        aux += adv.Impressions + ",";
        //        aux += adv.Clicks + ",";
        //        aux += adv.Conversions + ",";
        //        aux += adv.Impressions + ",";
        //        aux += adv.Clicks == 0 ? (adv.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)adv.Conversions / (double)adv.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //        aux += adv.Cost + ",";
        //        aux += adv.Clicks == 0 ? adv.Cost.ToString("F2") : (adv.Cost / adv.Clicks).ToString("F2").Replace(',', '.');
        //        aux += ",";
        //        aux += adv.Revenue + ",";
        //        aux += adv.Clicks == 0 ? adv.Revenue.ToString("F2").Replace(',', '.') : (adv.Revenue / adv.Clicks).ToString("F2").Replace(',', '.');
        //        aux += "," + ((adv.Revenue - adv.Cost).ToString("F2")).Replace(',', '.');
        //        GetLastReport += aux + "\n";


        //    }
        //    return ldvm;

        //}

        //public static AffiliateViewModel GetAffiliateReport(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics Statistics, Calculation calculation)
        //{
        //    GetLastReport = "Name,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    AffiliateViewModel lavm = new AffiliateViewModel();
        //    lavm.AffiliateViewList = new List<AffiliateView>();

        //    lavm.Filter = new Models.Filter();
        //    lavm.Stadisctics = Statistics;
        //    lavm.Calculation = calculation;


        //    if (IsAdmin())
        //    {
        //        List<Affiliate> list = db.Affiliates.Where(af => af.CustomerId == customerid).ToList();
        //        foreach (Affiliate aff in list)
        //        {
        //            DateTime comparefromDate = TimeZoneFromDate;
        //            DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
        //            int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.AffiliateId == aff.AffiliateId && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //            int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.AffiliateId == aff.AffiliateId && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //            int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.AffiliateId == aff.AffiliateId && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //            double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == aff.AffiliateId && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //            cost += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == aff.AffiliateId && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == aff.AffiliateId && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //            double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == aff.AffiliateId && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //            revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == aff.AffiliateId && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == aff.AffiliateId && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //            AffiliateView avm = new AffiliateView();
        //            avm.Company = aff.Company;
        //            avm.Impressions = impressions;
        //            avm.Clicks = clicks;
        //            avm.Conversions = conversions;
        //            avm.Cost = cost;
        //            avm.Revenue = revenue;
        //            string aux = "";
        //            aux += avm.Company + ',';
        //            aux += avm.Impressions + ",";
        //            aux += avm.Clicks + ",";
        //            aux += avm.Conversions + ",";
        //            aux += avm.Impressions + ",";
        //            aux += avm.Clicks == 0 ? (avm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)avm.Conversions / (double)avm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += avm.Cost + ",";
        //            aux += avm.Clicks == 0 ? avm.Cost.ToString("F2") : (avm.Cost / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += ",";
        //            aux += avm.Revenue + ",";
        //            aux += avm.Clicks == 0 ? avm.Revenue.ToString("F2").Replace(',', '.') : (avm.Revenue / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += "," + ((avm.Revenue - avm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //            lavm.AffiliateViewList.Add(avm);
        //        }


        //        return lavm;
        //    }
        //    else
        //    {
        //        int affiliateid = CurrentUser().AffiliateId ?? 0;
        //        Affiliate affiliate = db.Affiliates.Single(af => af.AffiliateId == affiliateid && af.CustomerId == customerid);
        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
        //        int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //        int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.AffiliateId == affiliateid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //        int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //        double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //        cost += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //        cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //        double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //        revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //        revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //        AffiliateView avm = new AffiliateView();
        //        if (affiliate != null)
        //            avm.Company = affiliate.Company;
        //        avm.Impressions = impressions;
        //        avm.Clicks = clicks;
        //        avm.Conversions = conversions;
        //        avm.Cost = cost;
        //        avm.Revenue = revenue;
        //        string aux = "";
        //        aux += avm.Company + ',';
        //        aux += avm.Impressions + ",";
        //        aux += avm.Clicks + ",";
        //        aux += avm.Conversions + ",";
        //        aux += avm.Impressions + ",";
        //        aux += avm.Clicks == 0 ? (avm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)avm.Conversions / (double)avm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //        aux += avm.Cost + ",";
        //        aux += avm.Clicks == 0 ? avm.Cost.ToString("F2") : (avm.Cost / avm.Clicks).ToString("F2").Replace(',', '.');
        //        aux += ",";
        //        aux += avm.Revenue + ",";
        //        aux += avm.Clicks == 0 ? avm.Revenue.ToString("F2").Replace(',', '.') : (avm.Revenue / avm.Clicks).ToString("F2").Replace(',', '.');
        //        aux += "," + ((avm.Revenue - avm.Cost).ToString("F2")).Replace(',', '.');
        //        GetLastReport += aux + "\n";

        //        lavm.AffiliateViewList.Add(avm);
        //        return lavm;
        //    }
        //}

        //public static AffiliateViewModel GetAffiliateReport(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics Statistics, Calculation calculation, CpaTicker.Areas.admin.Models.Filter filter, List<object> filtervalues)
        //{
        //    GetLastReport = "Name,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    AffiliateViewModel lavm = new AffiliateViewModel();
        //    lavm.AffiliateViewList = new List<AffiliateView>();
        //    lavm.Filter = filter;
        //    lavm.Stadisctics = Statistics;
        //    lavm.Calculation = calculation;



        //    if (!filter.Affiliate)
        //    {
        //        foreach (Affiliate aff in db.Affiliates.Where(af => af.CustomerId == customerid).ToList())
        //        {
        //            string contry = "";
        //            if (filter.Contries)
        //                contry = filtervalues[2].ToString();
        //            if (!filter.Contries || aff.Country == contry)
        //            {
        //                DateTime comparefromDate = TimeZoneFromDate;
        //                DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
        //                int clicks = 0;
        //                int conversions = 0;
        //                int impressions = 0;
        //                var listclicks = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == aff.AffiliateId && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //                var conversionlist = db.Conversions.Where(co => co.CustomerId == customerid && co.AffiliateId == aff.AffiliateId && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //                var impressionlist = db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == aff.AffiliateId && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);


        //                if (filter.Campaign)
        //                {
        //                    int campaignid = (int)filtervalues[1];
        //                    listclicks = listclicks.Where(cl => cl.CampaignId == campaignid);
        //                    conversionlist = conversionlist.Where(cl => cl.CampaignId == campaignid);
        //                    impressionlist = impressionlist.Where(cl => cl.CampaignId == campaignid);

        //                }

        //                clicks = listclicks.Count();
        //                impressions = impressionlist.Count();
        //                conversions = conversionlist.Count();


        //                double cost = listclicks.Sum(cl => (double?)cl.Cost) ?? 0.0;
        //                cost += impressionlist.Sum(im => (double?)im.Cost) ?? 0.0;
        //                cost += conversionlist.Sum(cn => (double?)cn.Cost) ?? 0.0;
        //                double revenue = listclicks.Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //                revenue += impressionlist.Sum(im => (double?)im.Revenue) ?? 0.0;
        //                revenue += conversionlist.Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //                AffiliateView avm = new AffiliateView();
        //                avm.Company = aff.Company;
        //                avm.Impressions = impressions;
        //                avm.Clicks = clicks;
        //                avm.Conversions = conversions;
        //                avm.Cost = cost;
        //                avm.Revenue = revenue;
        //                string aux = "";
        //                aux += avm.Company + ',';
        //                aux += avm.Impressions + ",";
        //                aux += avm.Clicks + ",";
        //                aux += avm.Conversions + ",";
        //                aux += avm.Impressions + ",";
        //                aux += avm.Clicks == 0 ? (avm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)avm.Conversions / (double)avm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //                aux += avm.Cost + ",";
        //                aux += avm.Clicks == 0 ? avm.Cost.ToString("F2") : (avm.Cost / avm.Clicks).ToString("F2").Replace(',', '.');
        //                aux += ",";
        //                aux += avm.Revenue + ",";
        //                aux += avm.Clicks == 0 ? avm.Revenue.ToString("F2").Replace(',', '.') : (avm.Revenue / avm.Clicks).ToString("F2").Replace(',', '.');
        //                aux += "," + ((avm.Revenue - avm.Cost).ToString("F2")).Replace(',', '.');
        //                GetLastReport += aux + "\n";

        //                lavm.AffiliateViewList.Add(avm);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        int affiliateid = (int)filtervalues[0];
        //        string contry = "";
        //        if (filter.Contries)
        //            contry = filtervalues[2].ToString();
        //        Affiliate affiliate = db.Affiliates.Single(af => af.AffiliateId == affiliateid && af.CustomerId == customerid);


        //        if (affiliate != null && (!filter.Contries || affiliate.Country == contry))
        //        {
        //            DateTime comparefromDate = TimeZoneFromDate;
        //            DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
        //            int clicks = 0;
        //            int conversions = 0;
        //            int impressions = 0;
        //            var listclicks = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //            var conversionlist = db.Conversions.Where(co => co.CustomerId == customerid && co.AffiliateId == affiliateid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //            var impressionlist = db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);


        //            if (filter.Campaign)
        //            {
        //                int campaignid = (int)filtervalues[1];
        //                listclicks = listclicks.Where(cl => cl.CampaignId == campaignid);
        //                conversionlist = conversionlist.Where(cl => cl.CampaignId == campaignid);
        //                impressionlist = impressionlist.Where(cl => cl.CampaignId == campaignid);

        //            }

        //            clicks = listclicks.Count();
        //            impressions = impressionlist.Count();
        //            conversions = conversionlist.Count();


        //            double cost = listclicks.Sum(cl => (double?)cl.Cost) ?? 0.0;
        //            cost += impressionlist.Sum(im => (double?)im.Cost) ?? 0.0;
        //            cost += conversionlist.Sum(cn => (double?)cn.Cost) ?? 0.0;
        //            double revenue = listclicks.Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //            revenue += impressionlist.Sum(im => (double?)im.Revenue) ?? 0.0;
        //            revenue += conversionlist.Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //            AffiliateView avm = new AffiliateView();
        //            if (affiliate != null)
        //                avm.Company = affiliate.Company;
        //            avm.Impressions = impressions;
        //            avm.Clicks = clicks;
        //            avm.Conversions = conversions;
        //            avm.Cost = cost;
        //            avm.Revenue = revenue;
        //            string aux = "";
        //            aux += avm.Company + ',';
        //            aux += avm.Impressions + ",";
        //            aux += avm.Clicks + ",";
        //            aux += avm.Conversions + ",";
        //            aux += avm.Impressions + ",";
        //            aux += avm.Clicks == 0 ? (avm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)avm.Conversions / (double)avm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += avm.Cost + ",";
        //            aux += avm.Clicks == 0 ? avm.Cost.ToString("F2") : (avm.Cost / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += ",";
        //            aux += avm.Revenue + ",";
        //            aux += avm.Clicks == 0 ? avm.Revenue.ToString("F2").Replace(',', '.') : (avm.Revenue / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += "," + ((avm.Revenue - avm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //            lavm.AffiliateViewList.Add(avm);
        //        }
        //    }
        //    return lavm;
        //}

        //public static OfferViewModel GetOfferReport(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics Statistics, Calculation calculation)
        //{
        //    GetLastReport = "Name,Impressions,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    OfferViewModel lavm = new OfferViewModel();
        //    lavm.OfferViewList = new List<OfferView>();
        //    lavm.Filter = new Models.Filter();
        //    lavm.Stadisctics = Statistics;
        //    lavm.Calculation = calculation;


        //    if (IsAdmin())
        //    {

        //        foreach (Campaign cam in db.Campaigns.Where(ca => ca.CustomerId == customerid).ToList())
        //        {

        //            DateTime comparefromDate = TimeZoneFromDate;
        //            DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
        //            int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //            int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.CampaignId == cam.CampaignId && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //            int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //            double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //            cost += db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.CampaignId == cam.CampaignId && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //            double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //            revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.CampaignId == cam.CampaignId && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //            OfferView avm = new OfferView();
        //            avm.CampaignName = cam.CampaignName;
        //            avm.Impressions = impressions;
        //            avm.Clicks = clicks;
        //            avm.Conversions = conversions;
        //            avm.Cost = cost;
        //            avm.Revenue = revenue;
        //            string aux = "";
        //            aux += avm.CampaignName + ',';
        //            aux += avm.Impressions + ",";
        //            aux += avm.Clicks + ",";
        //            aux += avm.Conversions + ",";
        //            aux += avm.Impressions + ",";
        //            aux += avm.Clicks == 0 ? (avm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)avm.Conversions / (double)avm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += avm.Cost + ",";
        //            aux += avm.Clicks == 0 ? avm.Cost.ToString("F2") : (avm.Cost / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += ",";
        //            aux += avm.Revenue + ",";
        //            aux += avm.Clicks == 0 ? avm.Revenue.ToString("F2").Replace(',', '.') : (avm.Revenue / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += "," + ((avm.Revenue - avm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //            lavm.OfferViewList.Add(avm);
        //        }


        //        return lavm;
        //    }
        //    else
        //    {
        //        int affiliateid = CurrentUser().AffiliateId ?? 0;
        //        Affiliate affiliate = db.Affiliates.Single(af => af.AffiliateId == affiliateid && af.CustomerId == customerid);
        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);


        //        foreach (Campaign cam in db.Campaigns.Where(ca => ca.CustomerId == customerid).ToList())
        //        {
        //            int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //            int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.CampaignId == cam.CampaignId && co.AffiliateId == affiliateid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //            int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //            double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //            cost += db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.CampaignId == cam.CampaignId && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //            double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //            revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.CampaignId == cam.CampaignId && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //            OfferView avm = new OfferView();


        //            avm.CampaignName = cam.CampaignName;
        //            avm.Impressions = impressions;
        //            avm.Clicks = clicks;
        //            avm.Conversions = conversions;
        //            avm.Cost = cost;
        //            avm.Revenue = revenue;
        //            string aux = "";
        //            aux += avm.CampaignName + ',';
        //            aux += avm.Impressions + ",";
        //            aux += avm.Clicks + ",";
        //            aux += avm.Conversions + ",";
        //            aux += avm.Impressions + ",";
        //            aux += avm.Clicks == 0 ? (avm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)avm.Conversions / (double)avm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += avm.Cost + ",";
        //            aux += avm.Clicks == 0 ? avm.Cost.ToString("F2") : (avm.Cost / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += ",";
        //            aux += avm.Revenue + ",";
        //            aux += avm.Clicks == 0 ? avm.Revenue.ToString("F2").Replace(',', '.') : (avm.Revenue / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += "," + ((avm.Revenue - avm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //            lavm.OfferViewList.Add(avm);

        //        }

        //        return lavm;
        //    }
        //}

        //public static OfferViewModel GetOfferReport(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics Statistics, Calculation calculation, CpaTicker.Areas.admin.Models.Filter filter, List<object> filtervalues)
        //{
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    OfferViewModel lavm = new OfferViewModel();
        //    lavm.OfferViewList = new List<OfferView>();

        //    lavm.Filter = filter;
        //    lavm.Stadisctics = Statistics;
        //    lavm.Calculation = calculation;

        //    var campaignlist = db.Campaigns.Where(ca => ca.CustomerId == customerid);
        //    if (filter.Campaign)
        //    {
        //        int campaignid = (int)filtervalues[1];
        //        campaignlist = campaignlist.Where(cm => cm.CampaignId == campaignid);

        //    }
        //    foreach (Campaign cam in campaignlist)
        //    {

        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);

        //        int clicks = 0;
        //        int conversions = 0;
        //        int impressions = 0;
        //        var listclicks = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //        var conversionlist = db.Conversions.Where(co => co.CustomerId == customerid && co.CampaignId == cam.CampaignId && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //        var impressionlist = db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //        if (filter.Affiliate)
        //        {
        //            int affiliateid = (int)filtervalues[0];
        //            listclicks = listclicks.Where(cl => cl.AffiliateId == affiliateid);
        //            conversionlist = conversionlist.Where(con => con.AffiliateId == affiliateid);
        //            impressionlist = impressionlist.Where(im => im.AffiliateId == affiliateid);
        //        }

        //        if (filter.Contries)
        //        {
        //            string contry = filtervalues[2].ToString();
        //            foreach (Affiliate Affiliate in db.Affiliates.Where(af => af.Country == contry).ToList())
        //            {
        //                listclicks = listclicks.Where(cl => cl.AffiliateId == Affiliate.AffiliateId);
        //                conversionlist = conversionlist.Where(cl => cl.AffiliateId == Affiliate.AffiliateId);
        //                impressionlist = impressionlist.Where(cl => cl.AffiliateId == Affiliate.AffiliateId);
        //            }
        //        }
        //        clicks = listclicks.Count();
        //        impressions = impressionlist.Count();
        //        conversions = conversionlist.Count();


        //        double cost = listclicks.Sum(cl => (double?)cl.Cost) ?? 0.0;
        //        cost += impressionlist.Sum(im => (double?)im.Cost) ?? 0.0;
        //        cost += conversionlist.Sum(cn => (double?)cn.Cost) ?? 0.0;
        //        double revenue = listclicks.Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //        revenue += impressionlist.Sum(im => (double?)im.Revenue) ?? 0.0;
        //        revenue += conversionlist.Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //        OfferView avm = new OfferView();
        //        avm.CampaignName = cam.CampaignName;
        //        avm.Impressions = impressions;
        //        avm.Clicks = clicks;
        //        avm.Conversions = conversions;
        //        avm.Cost = cost;
        //        avm.Revenue = revenue;
        //        string aux = "";
        //        aux += avm.CampaignName + ',';
        //        aux += avm.Impressions + ",";
        //        aux += avm.Clicks + ",";
        //        aux += avm.Conversions + ",";
        //        aux += avm.Impressions + ",";
        //        aux += avm.Clicks == 0 ? (avm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)avm.Conversions / (double)avm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //        aux += avm.Cost + ",";
        //        aux += avm.Clicks == 0 ? avm.Cost.ToString("F2") : (avm.Cost / avm.Clicks).ToString("F2").Replace(',', '.');
        //        aux += ",";
        //        aux += avm.Revenue + ",";
        //        aux += avm.Clicks == 0 ? avm.Revenue.ToString("F2").Replace(',', '.') : (avm.Revenue / avm.Clicks).ToString("F2").Replace(',', '.');
        //        aux += "," + ((avm.Revenue - avm.Cost).ToString("F2")).Replace(',', '.');
        //        GetLastReport += aux + "\n";

        //        lavm.OfferViewList.Add(avm);

        //    }


        //    return lavm;

        //}

        //public static TrafficViewModel GetTrafficReport(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics Statistics, Calculation calculation)
        //{
        //    GetLastReport = "Campaign,Affiliate,URL,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    TrafficViewModel lavm = new TrafficViewModel();
        //    lavm.TrafficViewList = new List<TrafficView>();
        //    lavm.Filter = new Models.Filter();
        //    lavm.Stadisctics = Statistics;
        //    lavm.Calculation = calculation;


        //    if (IsAdmin())
        //    {
        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);



        //        foreach (Campaign cam in db.Campaigns.Where(ca => ca.CustomerId == customerid))
        //        {
        //            foreach (Affiliate affiliate in db.Affiliates.Where(af => af.CustomerId == customerid).ToList())
        //            {
        //                int affiliateid = affiliate.AffiliateId;
        //                int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //                int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.CampaignId == cam.CampaignId && co.AffiliateId == affiliateid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //                int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);

        //                double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //                cost += db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //                cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.CampaignId == cam.CampaignId && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;

        //                double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //                revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //                revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.CampaignId == cam.CampaignId && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //                TrafficView avm = new TrafficView();

        //                avm.Campaign = cam.CampaignName;
        //                avm.Affiliate = affiliate.Company;
        //                avm.URL = cam.OfferUrl;
        //                avm.Clicks = clicks;
        //                avm.Conversions = conversions;
        //                avm.Cost = cost;
        //                avm.Revenue = revenue;
        //                string aux = "";
        //                aux += avm.Campaign + ',';
        //                aux += avm.Affiliate + ",";
        //                aux += avm.URL + ",";
        //                aux += avm.Clicks + ",";
        //                aux += avm.Conversions + ",";
        //                aux += avm.Clicks == 0 ? (avm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)avm.Conversions / (double)avm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //                aux += avm.Cost + ",";
        //                aux += avm.Clicks == 0 ? avm.Cost.ToString("F2") : (avm.Cost / avm.Clicks).ToString("F2").Replace(',', '.');
        //                aux += ",";
        //                aux += avm.Revenue + ",";
        //                aux += avm.Clicks == 0 ? avm.Revenue.ToString("F2").Replace(',', '.') : (avm.Revenue / avm.Clicks).ToString("F2").Replace(',', '.');
        //                aux += "," + ((avm.Revenue - avm.Cost).ToString("F2")).Replace(',', '.');
        //                GetLastReport += aux + "\n";

        //                lavm.TrafficViewList.Add(avm);
        //            }
        //        }


        //        return lavm;
        //    }
        //    else
        //    {
        //        int affiliateid = CurrentUser().AffiliateId ?? 0;
        //        Affiliate affiliate = db.Affiliates.Single(af => af.AffiliateId == affiliateid && af.CustomerId == customerid);
        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);


        //        foreach (Campaign cam in db.Campaigns.Where(ca => ca.CustomerId == customerid).ToList())
        //        {
        //            int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //            int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.CampaignId == cam.CampaignId && co.AffiliateId == affiliateid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //            int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //            double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //            cost += db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.CampaignId == cam.CampaignId && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //            double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //            revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.CampaignId == cam.CampaignId && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //            TrafficView avm = new TrafficView();


        //            avm.Campaign = cam.CampaignName;
        //            avm.Affiliate = affiliate.Company;
        //            avm.URL = cam.OfferUrl;
        //            avm.Clicks = clicks;
        //            avm.Conversions = conversions;
        //            avm.Cost = cost;
        //            avm.Revenue = revenue;
        //            string aux = "";
        //            aux += avm.Campaign + ',';
        //            aux += avm.Affiliate + ",";
        //            aux += avm.URL + ",";
        //            aux += avm.Clicks + ",";
        //            aux += avm.Conversions + ",";
        //            aux += avm.Clicks == 0 ? (avm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)avm.Conversions / (double)avm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += avm.Cost + ",";
        //            aux += avm.Clicks == 0 ? avm.Cost.ToString("F2") : (avm.Cost / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += ",";
        //            aux += avm.Revenue + ",";
        //            aux += avm.Clicks == 0 ? avm.Revenue.ToString("F2").Replace(',', '.') : (avm.Revenue / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += "," + ((avm.Revenue - avm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //            lavm.TrafficViewList.Add(avm);

        //        }

        //        return lavm;
        //    }
        //}

        //public static TrafficViewModel GetTrafficReport(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics Statistics, Calculation calculation, CpaTicker.Areas.admin.Models.Filter filter, List<object> filtervalues)
        //{
        //    GetLastReport = "Campaign,Affiliate,URL,Clicks,Conversions,Conv Rate,Cost,CPC,Revenue,RPC,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    TrafficViewModel lavm = new TrafficViewModel();
        //    lavm.TrafficViewList = new List<TrafficView>();

        //    lavm.Filter = filter;
        //    lavm.Stadisctics = Statistics;
        //    lavm.Calculation = calculation;

        //    DateTime comparefromDate = TimeZoneFromDate;
        //    DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);

        //    var campaignlist = db.Campaigns.Where(ca => ca.CustomerId == customerid);
        //    if (filter.Campaign)
        //    {
        //        int campaignid = (int)filtervalues[1];
        //        campaignlist = campaignlist.Where(cm => cm.CampaignId == campaignid);

        //    }
        //    var affiliatelist = db.Affiliates.Where(af => af.CustomerId == customerid);
        //    if (filter.Affiliate)
        //    {
        //        int affiliateid = (int)filtervalues[0];
        //        affiliatelist = affiliatelist.Where(aff => aff.AffiliateId == affiliateid);
        //    }

        //    if (filter.Contries)
        //    {
        //        string contry = filtervalues[2].ToString();
        //        foreach (Affiliate Affiliate in db.Affiliates.Where(af => af.Country == contry))
        //        {
        //            affiliatelist = affiliatelist.Where(aff => aff.AffiliateId == Affiliate.AffiliateId);
        //        }
        //    }
        //    foreach (Campaign cam in campaignlist)
        //    {

        //        foreach (Affiliate affiliate in affiliatelist)
        //        {
        //            int affiliateid = affiliate.AffiliateId;
        //            int clicks = db.Clicks.Count(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
        //            int conversions = db.Conversions.Count(co => co.CustomerId == customerid && co.CampaignId == cam.CampaignId && co.AffiliateId == affiliateid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate);
        //            int impressions = db.Impressions.Count(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate & im.ImpressionDate < comparetoDate);
        //            double cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
        //            cost += db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.CampaignId == cam.CampaignId && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
        //            double revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == cam.CampaignId && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate < comparetoDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
        //            revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == cam.CampaignId && im.AffiliateId == affiliateid && im.ImpressionDate >= comparefromDate && im.ImpressionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.CampaignId == cam.CampaignId && cn.AffiliateId == affiliateid && cn.ConversionDate >= comparefromDate && cn.ConversionDate < comparetoDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;
        //            TrafficView avm = new TrafficView();


        //            avm.Campaign = cam.CampaignName;
        //            avm.Affiliate = affiliate.Company;
        //            avm.URL = cam.OfferUrl;
        //            avm.Clicks = clicks;
        //            avm.Conversions = conversions;
        //            avm.Cost = cost;
        //            avm.Revenue = revenue;
        //            string aux = "";
        //            aux += avm.Campaign + ',';
        //            aux += avm.Affiliate + ",";
        //            aux += avm.URL + ",";
        //            aux += avm.Clicks + ",";
        //            aux += avm.Conversions + ",";
        //            aux += avm.Clicks == 0 ? (avm.Conversions * 100).ToString("F2").Replace(',', '.') + "," : (((double)avm.Conversions / (double)avm.Clicks) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += avm.Cost + ",";
        //            aux += avm.Clicks == 0 ? avm.Cost.ToString("F2") : (avm.Cost / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += ",";
        //            aux += avm.Revenue + ",";
        //            aux += avm.Clicks == 0 ? avm.Revenue.ToString("F2").Replace(',', '.') : (avm.Revenue / avm.Clicks).ToString("F2").Replace(',', '.');
        //            aux += "," + ((avm.Revenue - avm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //            lavm.TrafficViewList.Add(avm);
        //        }
        //    }


        //    return lavm;



        //}

        //public static ConversionStatusViewModel GetConversionStatus(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics Statistics)
        //{
        //    GetLastReport = "Offer,Affiliate,Gross Conversions,Approved Conversions,%Approved,RejectedConversions,%Rejected,Net Payout,Net Revenue" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    ConversionStatusViewModel lcvm = new ConversionStatusViewModel();
        //    lcvm.ConversionStatusViewList = new List<ConversionStatusView>();
        //    lcvm.Filter = new Models.Filter();
        //    lcvm.Statistics = Statistics;

        //    if (IsAdmin())
        //    {
        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
        //        foreach (Campaign cam in db.Campaigns.Where(ca => ca.CustomerId == customerid))
        //        {
        //            foreach (Affiliate aff in db.Affiliates.Where(aff => aff.CustomerId == customerid).ToList())
        //            {
        //                int conversionapproved = 0;
        //                int conversionrejected = 0;
        //                int grossconversion = 0;
        //                double payout = 0;
        //                double revenue = 0;

        //                conversionapproved = db.Conversions.Count(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 1 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate);
        //                conversionrejected = db.Conversions.Count(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 0 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate);

        //                grossconversion = conversionrejected + conversionapproved;

        //                payout = db.Conversions.Where(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 1 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //                revenue = db.Conversions.Where(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 1 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;

        //                ConversionStatusView csv = new ConversionStatusView();
        //                csv.CompanyName = cam.CampaignName;
        //                csv.AffiliateName = aff.Company;
        //                csv.ApprovedConversion = conversionapproved;
        //                csv.RejectedConversions = conversionrejected;
        //                csv.NetPayout = payout;
        //                csv.GrossConversion = grossconversion;
        //                csv.NetRevenue = revenue;

        //                lcvm.ConversionStatusViewList.Add(csv);
        //                string aux = "";
        //                aux += csv.CompanyName + ',';
        //                aux += csv.AffiliateName + ",";
        //                aux += csv.GrossConversion + ",";
        //                aux += csv.ApprovedConversion + ",";
        //                aux += csv.GrossConversion == 0 ? (0).ToString("F2").Replace(',', '.') : (((double)csv.ApprovedConversion / (double)csv.GrossConversion) * 100).ToString("F2").Replace(',', '.') + ",";
        //                aux += csv.RejectedConversions + ",";
        //                aux += csv.GrossConversion == 0 ? (0).ToString("F2").Replace(',', '.') : (((double)csv.RejectedConversions / (double)csv.GrossConversion) * 100).ToString("F2").Replace(',', '.') + ",";
        //                aux += csv.NetPayout.ToString("F2").Replace(',', '.');
        //                aux += csv.NetRevenue.ToString("F2").Replace(',', '.');
        //                GetLastReport += aux + "\n";

        //            }
        //        }


        //        return lcvm;
        //    }
        //    else
        //    {
        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
        //        int affiliateid = CurrentUser().AffiliateId ?? 0;
        //        Affiliate aff = db.Affiliates.Single(af => af.AffiliateId == affiliateid && af.CustomerId == customerid);
        //        foreach (Campaign cam in db.Campaigns.Where(ca => ca.CustomerId == customerid).ToList())
        //        {

        //            int conversionapproved = 0;
        //            int conversionrejected = 0;
        //            int grossconversion = 0;
        //            double payout = 0;
        //            double revenue = 0;
        //            conversionapproved = db.Conversions.Count(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 1 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate);
        //            conversionrejected = db.Conversions.Count(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 0 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate);
        //            grossconversion = conversionrejected + conversionapproved;
        //            payout = db.Conversions.Where(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 1 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            revenue = db.Conversions.Where(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 1 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            ConversionStatusView csv = new ConversionStatusView();
        //            csv.CompanyName = cam.CampaignName;
        //            csv.AffiliateName = aff.Company;
        //            csv.ApprovedConversion = conversionapproved;
        //            csv.RejectedConversions = conversionrejected;
        //            csv.NetPayout = payout;
        //            csv.GrossConversion = grossconversion;
        //            csv.NetRevenue = revenue;
        //            lcvm.ConversionStatusViewList.Add(csv);
        //            string aux = "";
        //            aux += csv.CompanyName + ',';
        //            aux += csv.AffiliateName + ",";
        //            aux += csv.GrossConversion + ",";
        //            aux += csv.ApprovedConversion + ",";
        //            aux += csv.GrossConversion == 0 ? (0).ToString("F2").Replace(',', '.') : (((double)csv.ApprovedConversion / (double)csv.GrossConversion) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += csv.RejectedConversions + ",";
        //            aux += csv.GrossConversion == 0 ? (0).ToString("F2").Replace(',', '.') : (((double)csv.RejectedConversions / (double)csv.GrossConversion) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += csv.NetPayout.ToString("F2").Replace(',', '.');
        //            aux += csv.NetRevenue.ToString("F2").Replace(',', '.');
        //            GetLastReport += aux + "\n";


        //        }


        //        return lcvm;
        //    }
        //}

        //public static ConversionStatusViewModel GetConversionStatus(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics Statistics, CpaTicker.Areas.admin.Models.Filter filter, List<object> filtervalues)
        //{
        //    GetLastReport = "Offer,Affiliate,Gross Conversions,Approved Conversions,%Approved,RejectedConversions,%Rejected,Net Payout,Net Revenue" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    ConversionStatusViewModel lcvm = new ConversionStatusViewModel();
        //    lcvm.ConversionStatusViewList = new List<ConversionStatusView>();

        //    lcvm.Filter = filter;
        //    lcvm.Statistics = Statistics;



        //    DateTime comparefromDate = TimeZoneFromDate;
        //    DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);

        //    var listcamp = db.Campaigns.Where(co => co.CustomerId == customerid);
        //    var listaff = db.Affiliates.Where(co => co.CustomerId == customerid);
        //    if (filter.Affiliate)
        //    {
        //        int affiliateid = (int)filtervalues[0];
        //        listaff = listaff.Where(co => co.AffiliateId == affiliateid);

        //    }
        //    if (filter.Campaign)
        //    {
        //        int campaignid = (int)filtervalues[1];
        //        listcamp = listcamp.Where(co => co.CampaignId == campaignid);

        //    }
        //    if (filter.Contries)
        //    {
        //        string contry = filtervalues[2].ToString();
        //        foreach (Affiliate Affiliate in db.Affiliates.Where(af => af.Country == contry).ToList())
        //        {
        //            listaff = listaff.Where(co => co.AffiliateId == Affiliate.AffiliateId);
        //        }
        //    }
        //    foreach (Campaign cam in listcamp)
        //    {
        //        foreach (Affiliate aff in listaff)
        //        {
        //            int conversionapproved = 0;
        //            int conversionrejected = 0;
        //            int grossconversion = 0;
        //            double payout = 0;
        //            double revenue = 0;
        //            conversionapproved = db.Conversions.Count(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 1 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate);
        //            conversionrejected = db.Conversions.Count(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 0 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate);
        //            grossconversion = conversionrejected + conversionapproved;
        //            payout = db.Conversions.Where(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 1 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate).Sum(im => (double?)im.Cost) ?? 0.0;
        //            revenue = db.Conversions.Where(cn => cn.AffiliateId == aff.AffiliateId && cn.CampaignId == cam.CampaignId && cn.Status == 1 && cn.ConversionDate > comparefromDate && cn.ConversionDate < comparetoDate).Sum(im => (double?)im.Revenue) ?? 0.0;
        //            ConversionStatusView csv = new ConversionStatusView();
        //            csv.CompanyName = cam.CampaignName;
        //            csv.AffiliateName = aff.Company;
        //            csv.ApprovedConversion = conversionapproved;
        //            csv.RejectedConversions = conversionrejected;
        //            csv.GrossConversion = grossconversion;
        //            csv.NetPayout = payout;
        //            csv.NetRevenue = revenue;
        //            lcvm.ConversionStatusViewList.Add(csv);
        //            string aux = "";
        //            aux += csv.CompanyName + ',';
        //            aux += csv.AffiliateName + ",";
        //            aux += csv.GrossConversion + ",";
        //            aux += csv.ApprovedConversion + ",";
        //            aux += csv.GrossConversion == 0 ? (0).ToString("F2").Replace(',', '.') : (((double)csv.ApprovedConversion / (double)csv.GrossConversion) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += csv.RejectedConversions + ",";
        //            aux += csv.GrossConversion == 0 ? (0).ToString("F2").Replace(',', '.') : (((double)csv.RejectedConversions / (double)csv.GrossConversion) * 100).ToString("F2").Replace(',', '.') + ",";
        //            aux += csv.NetPayout.ToString("F2").Replace(',', '.');
        //            aux += csv.NetRevenue.ToString("F2").Replace(',', '.');
        //            GetLastReport += aux + "\n";

        //        }
        //    }


        //    return lcvm;

        //}

        //public static ConversionViewModel GetConversion(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics Statistics)
        //{
        //    GetLastReport = "Date,Status,Campaign,Affiliate,UserAgent,ConversionIp,TransactionId,Cost,Revenue,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();
        //    ConversionViewModel lcvm = new ConversionViewModel();
        //    lcvm.ConversionViewList = new List<ConversionView>();
        //    lcvm.Filter = new Models.Filter();
        //    lcvm.Stadisctics = Statistics;

        //    if (IsAdmin())
        //    {
        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
        //        foreach (Conversion con in db.Conversions.Where(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate < comparetoDate).ToList())
        //        {
        //            ConversionView cvm = new ConversionView();
        //            //cvm.CompanyName = db.Campaigns.First(cam => cam.CampaignId == con.CampaignId).CampaignName ?? "";
        //            cvm.CompanyName = db.Campaigns.Single(c => c.CampaignId == con.CampaignId && c.CustomerId == customerid).CampaignName ?? "";

        //            cvm.Cost = con.Cost;
        //            cvm.Date = con.ConversionDate.ToString("MM/dd/yyyy") + " " + con.ConversionDate.ToShortTimeString();
        //            cvm.Revenue = con.Revenue;
        //            cvm.AffiliateName = db.Affiliates.First(aff => aff.AffiliateId == con.AffiliateId && aff.CustomerId == customerid).Company ?? "";
        //            cvm.UserAgent = con.UserAgent;
        //            cvm.Ip = con.IPAddress;
        //            cvm.ConversionId = con.ConversionId;
        //            cvm.Status = con.Status;
        //            cvm.Transaction = con.TransactionId;
        //            lcvm.ConversionViewList.Add(cvm);
        //            string aux = "";
        //            aux += cvm.Date + ',';
        //            aux += cvm.Status + ",";
        //            aux += cvm.CompanyName + ",";
        //            aux += cvm.AffiliateName + ",";
        //            aux += cvm.UserAgent + ",";
        //            aux += cvm.Ip + ",";
        //            aux += cvm.Transaction + ",";
        //            aux += cvm.Cost.ToString("F2").Replace(',', '.');
        //            aux += cvm.Revenue.ToString("F2").Replace(',', '.');
        //            aux += "," + ((cvm.Revenue - cvm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";
        //        }
        //        return lcvm;
        //    }
        //    else
        //    {
        //        DateTime comparefromDate = TimeZoneFromDate;
        //        DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
        //        int affiliateid = CurrentUser().AffiliateId ?? 0;
        //        Affiliate affiliate = db.Affiliates.Single(af => af.AffiliateId == affiliateid && af.CustomerId == customerid);
        //        foreach (Conversion con in db.Conversions.Where(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate < comparetoDate && co.AffiliateId == affiliateid).ToList())
        //        {
        //            ConversionView cvm = new ConversionView();
        //            //cvm.CompanyName = db.Campaigns.First(cam => cam.CampaignId == con.CampaignId).CampaignName ?? "";
        //            cvm.CompanyName = db.Campaigns.Single(c => c.CampaignId == con.CampaignId && c.CustomerId == customerid).CampaignName ?? "";

        //            cvm.Cost = con.Cost;
        //            cvm.Date = con.ConversionDate.ToString("MM/dd/yyyy") + " " + con.ConversionDate.ToShortTimeString();
        //            cvm.Revenue = con.Revenue;
        //            cvm.AffiliateName = db.Affiliates.First(aff => aff.AffiliateId == con.AffiliateId && aff.CustomerId == customerid).Company ?? "";
        //            cvm.Ip = con.IPAddress;
        //            cvm.Status = con.Status;
        //            cvm.ConversionId = con.ConversionId;
        //            cvm.Transaction = con.TransactionId;
        //            cvm.UserAgent = con.UserAgent;
        //            lcvm.ConversionViewList.Add(cvm);
        //            string aux = "";
        //            aux += cvm.Date + ',';
        //            aux += cvm.Status + ",";
        //            aux += cvm.CompanyName + ",";
        //            aux += cvm.AffiliateName + ",";
        //            aux += cvm.UserAgent + ",";
        //            aux += cvm.Ip + ",";
        //            aux += cvm.Transaction + ",";
        //            aux += cvm.Cost.ToString("F2").Replace(',', '.');
        //            aux += cvm.Revenue.ToString("F2").Replace(',', '.');
        //            aux += "," + ((cvm.Revenue - cvm.Cost).ToString("F2")).Replace(',', '.');
        //            GetLastReport += aux + "\n";
        //        }
        //        return lcvm;
        //    }
        //}

        //public static ConversionViewModel GetConversion(DateTime FromDate, DateTime ToDate, int customerid, string timezone, Statistics Statistics, CpaTicker.Areas.admin.Models.Filter filter, List<object> filtervalues)
        //{
        //    GetLastReport = "Date,Status,Campaign,Affiliate,UserAgent,ConversionIp,TransactionId,Cost,Revenue,Profit" + "\n";
        //    DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)FromDate, TimeZoneInfo.Local.Id, timezone);
        //    CpaTickerDb db = new CpaTickerDb();

        //    ConversionViewModel lcvm = new ConversionViewModel();
        //    lcvm.ConversionViewList = new List<ConversionView>();
        //    lcvm.ConversionViewList = new List<ConversionView>();
        //    lcvm.Filter = filter;
        //    lcvm.Stadisctics = Statistics;



        //    DateTime comparefromDate = TimeZoneFromDate;
        //    DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)ToDate.AddDays(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);

        //    var list = db.Conversions.Where(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate < comparetoDate);
        //    if (filter.Affiliate)
        //    {
        //        int affiliateid = (int)filtervalues[0];
        //        list = list.Where(co => co.AffiliateId == affiliateid);

        //    }
        //    if (filter.Campaign)
        //    {
        //        int campaignid = (int)filtervalues[1];
        //        list = list.Where(co => co.CampaignId == campaignid);

        //    }
        //    if (filter.Contries)
        //    {
        //        string contry = filtervalues[2].ToString();
        //        foreach (Affiliate Affiliate in db.Affiliates.Where(af => af.Country == contry).ToList())
        //        {
        //            list = list.Where(co => co.AffiliateId == Affiliate.AffiliateId);
        //        }
        //    }
        //    foreach (Conversion con in list)
        //    {

        //        ConversionView cvm = new ConversionView();
        //        cvm.CompanyName = db.Campaigns.First(cam => cam.CampaignId == con.CampaignId).CampaignName ?? "";
        //        cvm.Cost = con.Cost;
        //        cvm.Date = con.ConversionDate.ToString("MM/dd/yyyy") + " " + con.ConversionDate.ToShortTimeString();
        //        cvm.Revenue = con.Revenue;
        //        cvm.AffiliateName = db.Affiliates.First(aff => aff.AffiliateId == con.AffiliateId && aff.CustomerId == customerid).Company ?? "";
        //        cvm.UserAgent = con.UserAgent;
        //        cvm.Ip = con.IPAddress;
        //        cvm.ConversionId = con.ConversionId;
        //        cvm.Status = con.Status;
        //        cvm.Transaction = con.TransactionId;
        //        lcvm.ConversionViewList.Add(cvm);
        //        string aux = "";
        //        aux += cvm.Date + ',';
        //        aux += cvm.Status + ",";
        //        aux += cvm.CompanyName + ",";
        //        aux += cvm.AffiliateName + ",";
        //        aux += cvm.UserAgent + ",";
        //        aux += cvm.Ip + ",";
        //        aux += cvm.Transaction + ",";
        //        aux += cvm.Cost.ToString("F2").Replace(',', '.');
        //        aux += cvm.Revenue.ToString("F2").Replace(',', '.');
        //        aux += "," + ((cvm.Revenue - cvm.Cost).ToString("F2")).Replace(',', '.');
        //        GetLastReport += aux + "\n";
        //    }


        //    return lcvm;

        //}

        public static string GetGrossCost(DateTime FromDate, DateTime ToDate, int customerid, int grouptype)
        {
            int affiliateid = CurrentUser().AffiliateId ?? 0;
            CpaTickerDb db = new CpaTickerDb();
            double cost = 0;
            if (IsAdmin())
            {
                cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
                cost += db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= FromDate && im.ImpressionDate < ToDate).Sum(im => (double?)im.Cost) ?? 0.0;
                cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.ConversionDate >= FromDate && cn.ConversionDate < ToDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
            }
            else
            {
                cost = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
                cost += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= FromDate && im.ImpressionDate < ToDate).Sum(im => (double?)im.Cost) ?? 0.0;
                cost += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == affiliateid && cn.ConversionDate >= FromDate && cn.ConversionDate < ToDate).Sum(cn => (double?)cn.Cost) ?? 0.0;
            }
            return cost.ToString();
        }

        public static string GetGrossRevenue(DateTime FromDate, DateTime ToDate, int customerid, int grouptype)
        {
            int affiliateid = CurrentUser().AffiliateId ?? 0;
            CpaTickerDb db = new CpaTickerDb();
            double revenue = 0;
            if (IsAdmin())
            {
                revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
                revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.ImpressionDate >= FromDate && im.ImpressionDate < ToDate).Sum(im => (double?)im.Revenue) ?? 0.0;
                revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.ConversionDate >= FromDate && cn.ConversionDate < ToDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;
            }
            else
            {
                revenue = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate).Sum(cl => (double?)cl.Revenue) ?? 0.0;
                revenue += db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.ImpressionDate >= FromDate && im.ImpressionDate < ToDate).Sum(im => (double?)im.Revenue) ?? 0.0;
                revenue += db.Conversions.Where(cn => cn.CustomerId == customerid && cn.AffiliateId == affiliateid && cn.ConversionDate >= FromDate && cn.ConversionDate < ToDate).Sum(cn => (double?)cn.Revenue) ?? 0.0;
            }

            return revenue.ToString();
        }

        /*********************** improve end ********************/

        public static int GetClicksPerHour(DateTime FromDate, int hour, int customerid, int campaignid = 0)
        {
            CpaTickerDb db = new CpaTickerDb();
            int affiliateid = CurrentUser().AffiliateId ?? 0;
            DateTime comparefromDate = FromDate.AddHours(hour);
            DateTime comparetoDate = FromDate.AddHours(hour + 1).AddMilliseconds(-1);
            if (campaignid == 0)
            {
                if (IsAdmin())
                    return db.Clicks.Count(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
                else
                    return db.Clicks.Count(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate);
            }
            else
            {
                if (IsAdmin())
                    return db.Clicks.Count(cl => cl.CustomerId == customerid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate && cl.CampaignId == campaignid);
                else
                    return db.Clicks.Count(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.ClickDate >= comparefromDate && cl.ClickDate <= comparetoDate && cl.CampaignId == campaignid);

            }
        }

        public static int GetConversionsperHour(DateTime FromDate, int hour, int customerid, int campaignid = 0)
        {
            CpaTickerDb db = new CpaTickerDb();
            int affiliateid = CurrentUser().AffiliateId ?? 0;
            DateTime comparefromDate = FromDate.AddHours(hour);
            DateTime comparetoDate = FromDate.AddHours(hour + 1).AddMilliseconds(-1);
            if (campaignid == 0)
            {
                if (IsAdmin())
                    return db.Conversions.Count(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate && co.Status == 1);
                else
                    return db.Conversions.Count(co => co.CustomerId == customerid && co.AffiliateId == affiliateid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate && co.Status == 1);
            }
            else
            {
                if (IsAdmin())
                    return db.Conversions.Count(co => co.CustomerId == customerid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate && co.CampaignId == campaignid && co.Status == 1);
                else
                    return db.Conversions.Count(co => co.CustomerId == customerid && co.AffiliateId == affiliateid && co.ConversionDate >= comparefromDate && co.ConversionDate <= comparetoDate && co.CampaignId == campaignid && co.Status == 1);

            }
        }

        public static double GetClickCost(DateTime FromDate, DateTime ToDate, int customerid, int campaignid)
        {
            int affiliateid = CurrentUser().AffiliateId ?? 0;
            CpaTickerDb db = new CpaTickerDb();
            double clicks = 0;
            if (IsAdmin())
                clicks = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.CampaignId == campaignid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
            else
                clicks = db.Clicks.Where(cl => cl.CustomerId == customerid && cl.AffiliateId == affiliateid && cl.CampaignId == campaignid && cl.ClickDate >= FromDate && cl.ClickDate < ToDate).Sum(cl => (double?)cl.Cost) ?? 0.0;
            return clicks;
        }

        public static double GetConversionCost(DateTime FromDate, DateTime ToDate, int customerid, int campaignid)
        {
            int affiliateid = CurrentUser().AffiliateId ?? 0;
            CpaTickerDb db = new CpaTickerDb();
            double conversions = 0;
            if (IsAdmin())
                conversions = db.Conversions.Where(co => co.CustomerId == customerid && co.CampaignId == campaignid && co.ConversionDate >= FromDate && co.ConversionDate < ToDate && co.Status == 1).Sum(co => (double?)co.Cost) ?? 0.0;
            else
                conversions = db.Conversions.Where(co => co.CustomerId == customerid && co.AffiliateId == affiliateid && co.CampaignId == campaignid && co.ConversionDate >= FromDate && co.ConversionDate < ToDate && co.Status == 1).Sum(co => (double?)co.Cost) ?? 0.0;
            return conversions;
        }

        public static double GetImpressionCost(DateTime FromDate, DateTime ToDate, int customerid, int campaignid)
        {
            int affiliateid = CurrentUser().AffiliateId ?? 0;
            CpaTickerDb db = new CpaTickerDb();
            double impressions = 0;
            if (IsAdmin())
                impressions = db.Impressions.Where(im => im.CustomerId == customerid && im.CampaignId == campaignid && im.ImpressionDate >= FromDate & im.ImpressionDate < ToDate).Sum(im => (double?)im.Cost) ?? 0.0;
            else
                impressions = db.Impressions.Where(im => im.CustomerId == customerid && im.AffiliateId == affiliateid && im.CampaignId == campaignid && im.ImpressionDate >= FromDate & im.ImpressionDate < ToDate).Sum(im => (double?)im.Cost) ?? 0.0;
            return impressions;
        }

        public static string GetGrossRevenueperHour(DateTime FromDate, DateTime ToDate, int customerid, int grouptype)
        {
            string result = "";
            CpaTickerDb db = new CpaTickerDb();
            TypeBuilder builder = CreateTypeBuilder("DynamicTickerObject", "RevenueByHour", "RevenueByHourType");
            CreateAutoImplementedProperty(builder, "Hour", typeof(int));
            CreateAutoImplementedProperty(builder, "Revenue", typeof(double));
            CreateAutoImplementedProperty(builder, "Cost", typeof(double));
            Type resultType = builder.CreateType();
            dynamic revenue = db.Database.SqlQuery(resultType, "EXEC GetGrossCostRevenue {0}, {1}, {2}", customerid, FromDate, ToDate);

            foreach (var revenueItem in revenue)
            {
                result += revenueItem.Revenue + ",";
            }
            result = result.TrimEnd(',');
            return result;
        }

        public static string GetGrossCostperHours(DateTime FromDate, DateTime ToDate, int customerid, int grouptype)
        {
            string result = "";
            CpaTickerDb db = new CpaTickerDb();
            TypeBuilder builder = CreateTypeBuilder("DynamicTickerObject", "CostByHour", "CostByHourType");
            CreateAutoImplementedProperty(builder, "Hour", typeof(int));
            CreateAutoImplementedProperty(builder, "Revenue", typeof(double));
            CreateAutoImplementedProperty(builder, "Cost", typeof(double));
            Type resultType = builder.CreateType();
            dynamic cost = db.Database.SqlQuery(resultType, "EXEC GetGrossCostRevenue @customerid = {0}, @fromdate = {1}, @todate = {2}", customerid, FromDate, ToDate);

            foreach (var costItem in cost)
            {
                result += costItem.Cost + ",";
            }
            result = result.TrimEnd(',');
            return result;
        }

        //public static DateTime GetFirstOperation(int customerid)
        //{
        //    CpaTickerDb db = new CpaTickerDb();
        //    DateTime result = new DateTime(DateTime.Now.Year, 1, 1);
        //    Click firstclick = null;
        //    Impression firsimpresion = null;
        //    Conversion firsconversion = null;
        //    try
        //    {
        //        if (db.Clicks.Count() > 0)
        //            firstclick = db.Clicks.OrderBy(cl => cl.ClickDate).First(cl => cl.CustomerId == customerid);
        //    }
        //    catch { }
        //    try
        //    {
        //        if (db.Impressions.Count() > 0)
        //            firsimpresion = db.Impressions.OrderBy(im => im.ImpressionDate).First(im => im.CustomerId == customerid);
        //    }
        //    catch { }
        //    try
        //    {
        //        if (db.Conversions.Count() > 0)
        //            firsconversion = db.Conversions.OrderBy(cn => cn.ConversionDate).First(cn => cn.CustomerId == customerid);
        //    }
        //    catch { }

        //    if (firstclick != null)
        //        if (result > firstclick.ClickDate)
        //            result = firstclick.ClickDate;

        //    if (firsimpresion != null)
        //        if (result > firsimpresion.ImpressionDate)
        //            result = firsimpresion.ImpressionDate;

        //    if (firsconversion != null)
        //        if (result > firsconversion.ConversionDate)
        //            result = firsconversion.ConversionDate;
        //    return new DateTime(result.Year, 1, 1);

        //}

        public static UserProfile CurrentUser()
        {
            CpaTickerDb db = new CpaTickerDb();
            int userid = WebSecurity.CurrentUserId;
            return db.UserProfiles.Single(ur => ur.UserId == userid);
        }

        public static void CreateClickReportfor(int customerid, DateTime date, string timezone)
        {
            GetLastReport = "CampaignId,AffiliateId,BannerId,ClickDate,UserAgent,IPAddress,Referrer,HitId,TransactionId,Cost,Revenue" + "\n";
            DateTime TimeZoneFromDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)date, TimeZoneInfo.Local.Id, timezone);
            DateTime comparefromDate = TimeZoneFromDate;
            DateTime comparetoDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)date.AddHours(1).AddMilliseconds(-1), TimeZoneInfo.Local.Id, timezone);
            CpaTickerDb db = new CpaTickerDb();
            foreach (Click click in db.Clicks.Where(cl => cl.ClickDate < comparetoDate && cl.ClickDate > comparefromDate && cl.CustomerId == customerid))
            {
                GetLastReport += click.CampaignId + ",";
                GetLastReport += click.AffiliateId + ",";
                GetLastReport += click.BannerId + ",";
                GetLastReport += click.ClickDate.ToString("MM/dd/yyyy") + ",";
                GetLastReport += click.UserAgent + ",";
                GetLastReport += click.IPAddress + ",";
                GetLastReport += click.Referrer + ",";
                GetLastReport += click.Source + ",";
                GetLastReport += click.TransactionId + ",";
                GetLastReport += click.Cost.ToString("F2") + ",";
                GetLastReport += click.Revenue.ToString("F2") + "\n";

            }
        }

        public static bool IsAdmin()
        {
            return Roles.IsUserInRole("Administrator");
        }

        static string[] mediaExtensions = {
            ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", //etc
            //".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", //etc
            //".AVI", ".MP4", ".DIVX", ".WMV", //etc
        };

        public static bool BannerIsImage(string fileName)
        { 
            return -1 != Array.IndexOf(mediaExtensions, Path.GetExtension(fileName).ToUpperInvariant());
        }

        public static string BannerContentType(string name)
        { 
            string contentType = "";
            switch (Path.GetExtension(name).ToLower())
            {
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".jpeg":
                case ".jpg":
                    contentType = "image/jpeg";
                    break;

                    /*============== Videos========*/
                case ".mp4":
                    contentType = "video/mp4";
                    break;
                case ".flv":
                    contentType = "video/x-flv";
                    break;
                case ".ogv":
                    contentType = "video/ogg";
                    break;
                case ".3gp":
                    contentType = "video/3gp"; // 3gpp
                    break;
                case ".webm":
                    contentType = "video/webm";
                    break;
                case ".mov":
                    contentType = "video/quicktime";
                    break;
                case ".avi":
                    contentType = "video/x-msvideo";
                    break;
                case ".wmv":
                    contentType = "video/x-ms-wmv";
                    break;

                    /*========================================*/
                default:
                    contentType = "image/png";
                    break;
            }

            return contentType;
        }




        internal static Type GetEnumType(string enumtypeName)
        {
            var assembly = System.Reflection.Assembly.Load("EnumAssembly");
            return assembly.GetType(enumtypeName);
        }
    }
}