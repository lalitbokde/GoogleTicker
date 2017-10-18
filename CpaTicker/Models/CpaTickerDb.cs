using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CpaTicker.Areas.admin.Classes;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;

namespace CpaTicker.Areas.admin.Classes
{
    public class CpaTickerDb : DbContext
    {
        public CpaTickerDb()
            : base("name=DefaultConnection")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180;
            //Database.Log = sql => Debug.Write(sql);
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserProfilePic> UserProfilePics { get; set; }

        public DbSet<UserCustomReport> UserCustomReports { get; set; }

        public DbSet<Impression> Impressions { get; set; }

        public DbSet<Banner> Banners { get; set; }

        public DbSet<Click> Clicks { get; set; }

        public DbSet<Conversion> Conversions { get; set; }

        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<Affiliate> Affiliates { get; set; }

        public DbSet<ConversionPixel> ConversionPixels { get; set; }

        public DbSet<Domain> Domains { get; set; }

        public DbSet<CustomerDomain> CustomerDomains { get; set; }

        public DbSet<Ticker> Tickers { get; set; }

        public DbSet<TickerCampaign> TickerCampaigns { get; set; }

        public DbSet<TickerSetting> TickerSettings { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<EmployeeIP> EmployeeIPs { get; set; }

        public DbSet<Action> Actions { get; set; }

        public DbSet<URL> URLs { get; set; }

        public DbSet<ConversionLog> ConversionLogs { get; set; }

        public DbSet<CustomField> CustomFields { get; set; }

        public DbSet<IP2Country> IP2Countries { get; set; }

        public DbSet<CustomFieldValue> CustomFieldValues { get; set; }

        public DbSet<CampaignCountry> CampaignCountries { get; set; }

        public DbSet<ConversionPixelCampaign> ConversionPixelCampaigns { get; set; }

        public DbSet<RedirectUrl> RedirectUrls { get; set; }

        public DbSet<RedirectTarget> RedirectTargets { get; set; }

        public DbSet<ActionConversionPixel> ActionConversionPixels { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Block> Blocks { get; set; }

        public DbSet<TickerElement> TickerElements { get; set; }

        public DbSet<ClickSubId> ClickSubIds { get; set; }

        public DbSet<UserHiddenCampaign> UserHiddenCampaigns { get; set; }

        public DbSet<LimeLightLib.LimeLightLog> LimeLightLogs { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<OverridePayout> OverridePayout { get; set; }
        public DbSet<AffiliateOverridePayout> AffiliateOverridePayout { get; set; }
        public DbSet<OverrideAffiliate> OverrideAffiliate { get; set; }

        public DbSet<DeviceInfo> DeviceInfo { get; set; }

        public DbSet<TimeClicks> TimeClicks { get; set; }
        public DbSet<CustomTimeZone> CustomTimeZone { get; set; }
        public DbSet<UserAffiliate> UserAffiliate { get; set; }
        public DbSet<NewsLetter> NewsLetter { get; set; }
        public DbSet<PAGE> PAGEs { get; set; }
        public DbSet<PAGECategory> PageCategories { get; set; }

        public DbSet<RedirectPAGE> RedirectPages { get; set; }

        public DbSet<RedirectTargetPage> RedirectTargetsPage { get; set; }

        public DbSet<loginhistory> loginhistory { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Action>().HasRequired(a => a.CampaignId);

            //base.OnModelCreating(modelBuilder);


            //EntityTypeConfiguration<UserProfile> etc = modelBuilder.Entity<UserProfile>().;

            //modelBuilder.
            //modelBuilder.Entity<UserProfile>().Property(t => t.AffiliateId).IsRequired();
            //modelBuilder.Entity<UserProfile>().Property(t => t.

            //var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            //foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    var entityTypes = assembly
            //      .GetTypes()
            //      .Where(t =>
            //        t.GetCustomAttributes(typeof(PersistentAttribute), inherit: true)
            //        .Any());

            //    foreach (var type in entityTypes)
            //    {
            //        entityMethod.MakeGenericMethod(type)
            //          .Invoke(modelBuilder, new object[] { });
            //    }
            //}
        }
    }
}