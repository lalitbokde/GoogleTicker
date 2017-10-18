using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CpaTicker.Areas.admin.Classes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Classes
{
    public enum TrackingType
    {
        [Description("HTTP iFrame Pixel")]
        HttpiFrame = 1,
        [Description("HTTPS iFrame Pixel")]
        HttpsiFrame = 2,
        [Description("HTTP Image Pixel")]
        HttpImage = 3,
        [Description("HTTPS Image Pixel")]
        HttpsImage = 4,
        [Description("Server Postback")]
        ServerPostback = 5
    }

    public enum Status
    {
        Active = 1,
        Paused = 2,
        Pending = 3,
        Deleted = 4
    }

    public enum RevenueType
    {
        [Display(Name="RPA")]
        [Description("Revenue per Conversion (RPA)")]
        RPA = 1,
        [Display(Name="RPS")]
        [Description("Revenue per Sale (RPS)")]
        RPS = 2,
        [Display(Name="RPA + RPS")]
        [Description("Revenue per Conversion + Revenue per Sale (RPA + RPS)")]
        RPA_RPS = 3,
        [Display(Name="RPC")]
        [Description("Revenue per Click (RPC)")]
        RPC = 4,
        [Display(Name="RPM")]
        [Description("Revenue per Thousand Impressions (RPM)")]
        RPM = 5
    }

    public enum PayoutType
    {
        [Display(Name="CPA")]
        [Description("Cost per Conversion (CPA)")]
        CPA = 1,
        [Display(Name="CPS")]
        [Description("Cost per Sale (CPS)")]
        CPS = 2,
        [Display(Name="CPA + CPS")]
        [Description("Cost per Conversion + Cost per Sale (CPA + CPS)")]
        CPA_CPS = 3,
        [Display(Name="CPC")]
        [Description("Cost per Click (CPC)")]
        CPC = 4,
        [Display(Name="CPM")]
        [Description("Cost per Thousand Impressions (CPM)")]
        CPM = 5
    }

    public class Campaign
    {
        public int Id { get; set; }

        public int CampaignId { get; set; }

        public int CustomerId { get; set; }
        
        [Display(Name="Campaign Name")]
        [Required]
        public string CampaignName { get; set; }

        [Display(Name="Description")]
        public string Description { get; set; }

        //[Display(Name = "Offer URL")]
        //public string OfferUrl { get; set; }

        //[Display(Name = "Preview URL")]
        //public string PreviewUrl { get; set; }

        //[Display(Name = "Tracking Type")]
        //[Required]
        //public TrackingType TrackingType { get; set; }

        [Required]
        public Status Status { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate  { get; set; }

        [Display(Name ="Cookie Expiry (days)")]
        public int? CookieExpirationInDays { get; set; }

        //public RevenueType RevenueType { get; set; }

        //public Decimal Revenue { get; set; }

        //public Decimal RevenuePercent { get; set; }

        //public PayoutType PayoutType { get; set; }

        //public Decimal Payout { get; set; }

        //public Decimal PayoutPercent { get; set; }

        public bool Enforce { get; set; }

        public virtual ICollection<URL> Urls { get; set; }

        public virtual ICollection<Action> Actions { get; set; }

        public virtual ICollection<CustomFieldValue> CustomFieldValues { get; set; }

        public virtual ICollection<CampaignCountry> Countries { get; set; }

    }

    public enum CampaignFilter
    {
        [Display(Name = "Show all Campaigns")]
        all = 1,
        [Display(Name = "Show only active and pending")]
        active_pending,
        [Display(Name = "Show only active")]
        active,
        [Display(Name = "Show only pending")]
        pending,
        [Display(Name = "Show only deleted")]
        deleted,
        [Display(Name = "Show only paused")]
        paused,
    }

    public enum AffiliateFilter
    {
        [Display(Name = "Show all Affiliates")]
        all = 1,
        [Display(Name = "Show only active and pending")]
        active_pending,
        [Display(Name = "Show only active")]
        active,
        [Display(Name = "Show only pending")]
        pending,
        [Display(Name = "Show only rejected")]
        rejected,
        [Display(Name = "Show only blocked")]
        blocked,
        [Display(Name = "Show only deleted")]
        deleted,
        
    }

    public enum PixelFilter
    {
        [Display(Name = "Show all Pixel")]
        all = 1,
        [Display(Name = "Show only active")]
        active,
        [Display(Name = "Show only pending")]
        pending,
        [Display(Name = "Show only paused")]
        paused,
        [Display(Name = "Show only Blocked")]
        Blocked,
        [Display(Name = "Show only Rejected")]
        Rejected,
    }

    public class CampaignCreateVM {

        public Campaign Campaign { get; set; }
        //public URL DefaultUrl { get; set; }
        public PAGE DefaultPage { get; set; }
        public Action DefaultAction { get; set; }
    }
}