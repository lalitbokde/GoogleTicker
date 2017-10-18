using CpaTicker.Areas.admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WURFL;


namespace CpaTicker.Areas.admin.Classes.Helpers
{
    public interface ICpaTickerRepository : IDisposable
    {
        // features added to test the click action
        void AddClick(Click ck);
        void AddImpression(Impression imp);
        void AddConversion(Conversion cv);
        decimal GetCost(PayoutType pt, decimal? payout, decimal? payoutpercent, ActionType actiontype, string SaleAmount = "0.0");
        decimal GetRevenue(RevenueType rt, decimal? revenue, decimal? revenuepercent, ActionType actiontype, string SaleAmount = "0.0");
        IQueryable<CampaignCountry> CampaignCountries(int campaignid);
        Block GetBlockFromInt32IP(long intAddress);

        IQueryable<Click> ClickQuery(DateTime ufdate, DateTime utdate, UserProfile up, int[] affs, int? cp, string ct);
        IQueryable<Conversion> ConversionQuery(DateTime ufdate, DateTime utdate, UserProfile up, int[] affs, int? cp, string ct, int conversionstatus = 1);
        IQueryable<Impression> ImpressionQuery(DateTime ufdate, DateTime utdate, UserProfile up, int[] affs, int? cp, string ct);
        IEnumerable<DailyView> DailyReport(DateTime fdate, DateTime tdate, UserProfile up, TimeZoneInfo tzi);
        IEnumerable<HourlyView> HourlyReport(DateTime fdate, UserProfile up, TimeZoneInfo tzi);
        /// <summary>
        /// Throw 404 not found exception if the customer isn’t found.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        int GetCustomerId(Uri url);

        Campaign GetCampaignById(int campaignid, int customerid = 0);

        bool IsHidden(Campaign c);
        bool IsHidden(int campaignid);

        /// <summary>
        /// Get the min date from a customer impression, click or conversion
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        DateTime GetFirstOperation(int customerid);

        Affiliate GetAffiliate(int affiliateid, int customerid = 0);

        IQueryable<ClickSubId> ClickSubIds();
        /// <summary>
        /// Returns the first action for the specified campaign if no action is found it creates
        /// a default action using the campaign data (GetDefaultAction) stores it and returns it
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        Action DefaultAction(Campaign campaign);

        /// <summary>
        /// Returns the default url (previewid = 1) for the specified campaign if no url is found it creates
        /// a default url using the campaign data (GetDefaultUrl) stores it and returns it
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        URL DefaultURL(Campaign campaign);

        IQueryable<Impression> Impressions();

        IQueryable<Affiliate> Affiliates();

        System.Data.Entity.Infrastructure.DbRawSqlQuery ExecuteQuery(Type type, string query, params object[] parameters);

        System.Data.Entity.Infrastructure.DbRawSqlQuery<T> ExecuteQuery<T>(string query);

        List<T> ExecuteQuery<T>(string query, params object[] parameters);

        //Action GetDefaultAction(Campaign campaign);

        int GetClicks(DateTime FromDate, DateTime ToDate, int customerid, int campaignid = 0);

        int GetConversions(DateTime FromDate, DateTime ToDate, int customerid, int campaignid = 0);

        // features added to test some methods in the account controller
        void AddTransaction(Transaction transaction);

        void AddOrderDecline(int orderid);

        IQueryable<Ticker> GetTickers(int userid = 0);

        IQueryable<TickerCampaign> GetTickerCampaigns(int tickerid);

        IQueryable<EditTickerViewModel> GetEditTickerVieModel(int tickerid, bool all);

        void DeleteTickerCampaigns(int tickerid);

        void AddTickerCampaign(int tickerid, int camapaigid);

        void SaveChanges();

        IQueryable<Campaign> Campaigns();

        void DeleteTicker(int tickerid);

        //IQueryable<Campaign> GetCustomerCampaigns(int customerid = 0);
        IQueryable<Campaign> GetUserCampaigns(UserProfile user);

        /// <summary>
        /// Return the all user-customer campaign that are not hidden for the specific user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IEnumerable<Campaign> GetUserCampaigns();

        double GetClickRevenue(DateTime FromDate, DateTime ToDate, int customerid, int campaignid, int affiliateid = 0);
        double GetConversionRevenue(DateTime FromDate, DateTime ToDate, int customerid, int campaignid, int affiliateid = 0);
        double GetImpressionRevenue(DateTime FromDate, DateTime ToDate, int customerid, int campaignid, int affiliateid = 0);
        UserProfile GetCurrentUser();

        //IQueryable<Campaign> GetCustomerActiveCampaigns(int customerid = 0);

        IQueryable<EmployeeIP> GetEmployeeIpList(int customerid = 0);

        void AddEmployeeIp(EmployeeIP eip);

        void DeleteEmployeeIp(string ip);

        bool IsEmployeeIp(string ip, int customerid = 0);

        decimal GetGrossCost(DateTime fromdate, DateTime todate, int customerid, int affiliateid);

        decimal GetGrossRevenue(DateTime fromdate, DateTime todate, int customerid, int affiliateid);

        Customer GetCurrentCustomer(int customerid = 0);

        TimeZoneInfo FindTimeZoneInfo(string timezone, string customerTimeZone);

        dynamic HourlyReport(DateTime fromdate, int customerid, int offset, int userid, string affiliateid = null, int? campaignid = null, int? countryid = null);
        dynamic DailyReport(DateTime fromdate, DateTime todate, int customerid, int offset, int userid, string affiliateid = null, int? campaignid = null, int? countryid = null);

        dynamic AdCampaignReport(DateTime fromdate, DateTime todate, int customerid, int userid, string affiliateid = null, int? campaignid = null, int? countryid = null);

        //dynamic ConversionReport(DateTime fromdate, DateTime todate, int customerid, string affiliateid = null, int? campaignid = null, int? countryid = null);
        dynamic CoversionStatusReport(DateTime fromdate, DateTime todate, int customerid, int userid, string affiliateid = null, int? campaignid = null, int? countryid = null);
        dynamic TrafficReport(DateTime fromdate, DateTime todate, int customerid, int userid, string affiliateid = null, int? campaignid = null, int? countryid = null);
        dynamic CTRReport(DateTime ufdate, DateTime utdate, int customerid, int userid, string af, int? cp, int? ct);

        //dynamic OfferReport(DateTime fromdate, DateTime todate, int customerid, string affiliateid = null, int? campaignid = null, int? countryid = null);
        //dynamic OfferSimpleWithURLReport(DateTime fromdate, DateTime todate, int customerid, string af, int? cp, int? ct);
        //dynamic OfferSimpleReport(DateTime fromdate, DateTime todate, int customerid, string af, int? cp, int? ct);
        //dynamic OfferWithURLReport(DateTime fromdate, DateTime todate, int customerid, string affiliateid = null, int? campaignid = null, int? countryid = null);

        //dynamic AffiliatesReport(DateTime fromdate, DateTime todate, int customerid, string affiliateid, int? campaignid = null, int? countryid = null);
        //dynamic AffiliatesSimpleReport(DateTime fromdate, DateTime todate, int customerid, string af, int? cp, int? ct);

        IQueryable<Affiliate> GetCustomerActiveAffiliates(int customerid = 0);

        IQueryable<Country> GetCountries();

        //IQueryable<Click> ClicksLogs(DateTime fromdate, DateTime todate, int customerid, int? affiliateid);

        IEnumerable<CpaTicker.Controllers.ReportsController.InnerLogDisp> ClicksLogs(string Id, DateTime fromdate, DateTime todate, int customerid, int? affiliateid);

        Domain GetDomain(int domainid);

        IQueryable<URL> GetCampaignURLs(int campaignid);

        Campaign FindCampaign(int campaignid);
        URL FindURL(int id);
        void EditURL(URL url);

        void AddURL(URL url);

        IQueryable<PAGE> GetCustomerPage(int CustomerId);
        IQueryable<PAGE> GetCustomerPageByStatus(int CustomerId, PageStatus status);
        void AddPAGE(PAGE page);
        PAGE FindPAGE(int id);
        void EditPAGE(PAGE page);
        IQueryable<PAGECategory> GetPageCategories();
        PAGECategory SavePageCategory(PAGECategory category);
        int RemovePageCategory(int categoryId,bool force=false);    // Soft delete
        IQueryable<Banner> GetBannersByCampaign(int campaignid, int customerid = 0);

        Ticker FindTicker(int id);

        void UpdateTicker(Ticker ticker);

        IQueryable<State> GetCountryStates(int countryid);

        State GetStateByCode(string code);

        IQueryable<URL> GetURLs(int? customerid = null);

        IQueryable<Domain> GetCustomerDomains(int customerid);

        Domain GetDefaultDomain(int customerid);

        IQueryable<UserProfile> GetCustomerUsers(int customerid = 0);

        TickerSetting GetTickerSettings(int userid = 0);

        int GetCurrentUserId();

        void AddTickerSetting(TickerSetting ts);

        void UpdateTickerSettings(TickerSetting ts);

        IEnumerable<TickerItem> BuildTicker(DateTime ufdate, DateTime utdate, int customerid, int userid, int? affiliateid, string subid = null);

        IEnumerable<TickerItemExt> BuildTickerExt(DateTime ufdate, DateTime utdate, int customerid, int userid, int? affiliateid, string subid = null);

        void DeleteTickerSettings();

        void AddTicker(Ticker t);

        IQueryable<CustomField> GetCustomFields(int customerid = 0);

        void AddCustomField(CustomField cf);

        int GetCurrentCustomerId();

        bool FieldNameExists(string fname);

        void DeleteCustomField(CustomField cf);

        CustomField FindCustomField(int id);

        void DeleteCustomerUser(int id);

        void DeleteDomain(int id);

        void AddCustomFieldValue(CustomFieldValue customFieldValue);

        void AddCampaign(Campaign campaign);

        IEnumerable<EditCFVModel> GetCampaignCustomField(int campaignid, int customerid = 0);

        IQueryable<CustomFieldValue> GetCampaignCustomFieldValue(int campaignid, int customerid);

        UserProfile GetUserFromAPIKey(string api);

        IQueryable<Affiliate> GetCustomerAffiliates(int customerid);
        IQueryable<UserAffiliate> GetUserAffiliates(int userid);

        IQueryable<IP2Country> GetIP2Countries();

        void AddCampaignCountry(CampaignCountry campaignCountry);

        IEnumerable<EditCampaignCountryViewModel> GetCampaignCountries(int campaignid);

        bool CheckIP(long intAddress, int campaignid);

        void DeleteUserTickers(int userid);

        IQueryable<UserProfile> AffiliateUsers(int affiliateid, int customerid);

        IQueryable<ConversionPixel> ConversionPixels();

        void AddConversionPixel(ConversionPixel cp);

        void AddConversionPixelCampaign(ConversionPixelCampaign cpc);

        ConversionPixel FindConversionPixel(int id);

        ConversionPixel FindCenversionPixelActive(int id, PixelStatus status);

        IQueryable<ConversionPixelCampaign> ConversionPixelCampaigns();

        void RemoveConversionPixel(ConversionPixel cp);

        void EditConversionPixel(ConversionPixel cp);

        ConversionPixelCampaign FindConversionPixelCampaign(int id);

        void RemoveConversionPixelCampaign(ConversionPixelCampaign cpc);

        void EditConversionPixelCampaign(ConversionPixelCampaign cpc);

        IQueryable<Action> Actions();

        void AddAction(Action action);

        string TrackingCode(Action action, int did = 0, int? actionid = null);

        IQueryable<ConversionPixel> GetCustomerConversionPixel(int customerid);
        IQueryable<ConversionPixel> GetCustomerConversionPixelFilter(int customerid, PixelStatus Status);
        Boolean SetConversionPixelBlock(Affiliate affiliate);
        Action FindAction(int id);

        void EditAction(Action act);

        IQueryable<Click> Clicks(int customerid, int campaignid);

        IQueryable<RedirectUrl> RedirectUrls();

        IQueryable<RedirectTarget> RedirectTargets();

        void AddRedirectUrl(RedirectUrl ru);

        void AddRedirectTarget(RedirectTarget redirectTarget);

        RedirectUrl FindRedirectUrl(int id);

        void EditRedirectUrl(RedirectUrl ru);

        void DeleteRedirectTargets(int id);

        void DeleteRedirectUrl(RedirectUrl ru);

        IP2Country GetCountryFromInt32IP(long intAddress);

        //URL GetDefaultUrl(Campaign campaign);

        void AddActionConversionPixel(ActionConversionPixel actionConversionPixel);

        Banner FindBanner(int id);

        void DeleteUrl(URL url);

        IQueryable<ActionConversionPixel> ActionConversionPixels();
        IQueryable<string> ActionConversionPixelsActivePostBack(int actionId, int AffiliateId, int customerid);
        IQueryable<string> ActionConversionPixelsActiveNoPostBack(int actionId, int AffiliateId, int customerid);

        ActionConversionPixel FindActionConversionPixel(int id);

        void DeleteActionConversionPixel(ActionConversionPixel obj);

        /// <summary>
        /// Returns the firts(default) action for the specified campaign. Assumes that the campaign has actions.
        /// For internal use.
        /// </summary>
        /// <param name="campaignid">Id field of the campaign table</param>
        /// <returns></returns>
        Action DefaultAction(int campaignid);



        IQueryable<Conversion> Conversions();



        TickerElement FindTickerElement(int id);

        void DeleteTickerElement(TickerElement e);

        void AddTickerElement(TickerElement tickerElement);

        void EditUserProfile(UserProfile up);

        IQueryable<UserHiddenCampaign> UserHiddenCampaigns();

        void AddUserHiddenCampaign(UserHiddenCampaign hc);

        void RemoveUserHiddenCampaigns(UserProfile user);

        IQueryable<UserProfile> UserProfile();

        IQueryable<Click> Clicks();

        IQueryable<URL> Urls();

        IQueryable<Banner> Banners();

        UserProfile FindUserProfile(int id);

        Order FindOrder(int id);

        void EditOrder(Order order);

        //IQueryable<APIKeyOrder> APIKeyOrders();

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id">UserId</param>
        ///// <returns></returns>
        //bool HasAPIKeyOrder(int userid);

        //void AddAPIKeyOrder(APIKeyOrder aPIKeyOrder);

        void EditCustomer(Customer customer);

        IQueryable<Order> Orders();

        void AddCustomer(Customer customer);

        void AddOrder(Order order);

        Country FindCountry(int id);

        void EditCampaign(Campaign campaign);

        void AddLimeLightLog(LimeLightLib.LimeLightLog limeLightLog);

        Task<Block> FindBlockAsync(uint ip);

        Location FindLocation(int locId);

        DbSet<Log> Logs();

        Click FindClick(int id);

        DbRawSqlQuery<T> RunQuery<T>(string query, params object[] parameters);
        SqlDataReader GetHourlyRptData(string CustomerId, DateTime fromdate, DateTime todate, string offset, string UserId, string AffiliateId, int? CampaignId, string Country, string ct);

        void GetNotifySparkHourlyRptData(int CustomerId, string pagename, string ConnectionID);
        SqlDataReader NotifyConversionStatus(int CustomerId, string pagename, string timezone, string viewdata, string fromdate, string todate, string dataview, string ConnectionID);
        int GetNotifyCampaignData(int CustomerId, string pagename, string ConnectionID);
        int GetNotifyTickerData(int CustomerId, string pagename, string ConnectionID, int userid);

        SqlDataReader NotifyAffiliateData(DateTime? FromDate = null, DateTime? ToDate = null, string timezone = "", string viewdata = "",
            long? dataview = null, int? cp = null, [FromUri] int[] af = null, int? ct = null, string ConnectionID = "");
        SqlDataReader NotifyCTRData(int CustomerID, int UserID, string fromdate, string todate, string timezone, string viewdata, string ConnectionID);

        List<DispOverridePayout> GetOverridePayout(int actionid);
        List<DispOverridePayout> GetOverridePayoutCampaign(int campaignID);

        List<DispAffiliateOverride> GetAffiliateOverride(int AffiliateId, int CustomerId);


        string AddOverridePayout(string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent);
        string AddOverridePayoutCampaign(string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent);
        int AddOverrideAffiliate(int AffiliateID, int CustomerId, string _PayoutType, string Payout, string PayoutPercent, string _RevenueType, string Revenue, string RevenuePercent);

        string UpdateOverridePayout(int OverrideID, string ActionID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent);
        string UpdateOverridePayoutCampaign(int OverrideID, string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent);
        IQueryable<OverridePayout> Override();
        OverridePayout FindActionByOverride(int id);

        AffiliateOverridePayout FindAffiliateOverride(int id);
        int UpdateOverrideAffiliate(int OverrideId, int AffiliateID, int CustomerId, string _PayoutType, string Payout, string PayoutPercent, string _RevenueType, string Revenue, string RevenuePercent);

        List<AffiliateOverridePayout> CheckAffiliateOverride(int customerid, int affiliateid);
        List<Affiliate> GetAffiliateByCustomer(int CustomerID);

        OverridePayout CheckOverridebyCampaign(int CampaignID, int customerId);

        OverridePayout CheckOverridebyAction(int? ActionID, int? CustomerId);

        void GetNotifyHourlyRptData(int CustomerId, string pagename, string timezone, string viewdata, string fromdate, string todate, string dataview, string ConnectionID);

        Byte[] SaveProfilePic(byte[] Image);

        Byte[] GetProfilePic();

        List<UserCustomReport> SaveUserCustomReport(string ReportName, string ReportData, string ColumOrder, int ReportID);
        List<UserCustomReport> GETUserCustomReport();
        IQueryable<UserCustomReport> GetCustomReports(int customerid, int userID);
        IQueryable<UserCustomReport> GetCustomReportByID(int customerid, int userID, int reportId);
        Boolean DeleteCustomReports(int customerid, int userID, int reportId);

        UserCustomReport FindCustomReport(int ReportId);
        // bool ChangeDatepickerNotification(int customerID, string pagename, string timezone, string viewdata, string fromdate, string todate, string dataview);
        UserAgentInfo GetUserAgentDetails(String userAgent, MatchMode mode);
        DeviceInfo addUserAgentInfos(string UserAgent);

        List<SelectListItem> GetDevice_DeviceID();
        List<SelectListItem> GetDevice_OS();
        List<SelectListItem> GetDevice_Browser();
        List<SelectListItem> GetDevice_DeviceOS();
        List<SelectListItem> GetDevice_ModelName();
        List<SelectListItem> GetDevice_BrandName();
        List<SelectListItem> GetDevice_MarketingName();
        List<SelectListItem> GetDevice_Resolution();

        List<SelectListItem> GetUserAgent();

        IQueryable<DeviceInfo> DeviceInfos();
        string bulkinsert(int start, int end);
        List<DispOverrideAffiliate> GetAffiliatPayoutCampaign(int campaignID);
        int addAffiliateOverrideCampaign(string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent, string _RevenueType, string Revenue, string RevenuePercent);

        int updateAffiliateOverrideCampaign(int OverrideID, string ActionID, string CampaignID, string AffiliateID, string _PayoutType, string Payout, string PayoutPercent, string _RevenueType, string Revenue, string RevenuePercent);
        OverrideAffiliate FindAffiliatePayout(int id);
        IEnumerable<CustomTimeZone> getTimeZoneList(int userID);
        CustomTimeZone AddCustomTimezone(CustomTimeZone model);
        CustomTimeZone GetTimezoneById(int id);
        CustomTimeZone EditCustomTimezone(CustomTimeZone model);
        int Getcustomoffset(int userId, string timezone);
        List<SelectListItem> GetCustomTimezoneByUser(int userId);
        List<SelectListItem> GetSources();
        bool GetCustomerByURL(Uri url);
        IQueryable<Affiliate> GetUserAffiliates(int userid, int CustomerId);


        IQueryable<RedirectTargetPage> RedirectTargetPages();
        IQueryable<RedirectPAGE> RedirectPages();
        void AddRedirectPage(RedirectPAGE ru);
        void AddRedirectTargetPages(RedirectTargetPage redirectTarget);
        RedirectPAGE FindRedirectPage(int id);
        void EditRedirectPage(RedirectPAGE ru);
        void DeleteRedirectTargetsPages(int id);
        void DeleteRedirectPage(RedirectPAGE ru);
        List<permissionlist> GetPermissionNames();
        List<permissionlist> GetPermissionNames1();
    }

}

