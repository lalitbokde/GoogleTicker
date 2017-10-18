using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class Ticker
    {
        public int TickerId { get; set; }
        [ForeignKey("UserProfile")]
        public int UserId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        [DefaultValue(1)]
        public double Speed { get; set; }
        public TickerView View { get; set; }
        public bool All { get; set; }
        public bool Direction { get; set; }

        public virtual ICollection<TickerElement> TickerElements { get; set; }

        [DisplayName("Background Color")]
        public string BackgroundColor { get; set; }
        [DisplayName("Campaign Color")]
        public string CampaignColor { get; set; }
        [DisplayName("Impression Color")]
        public string ImpressionColor { get; set; }
        [DisplayName("Click Color")]
        public string ClickColor { get; set; }
        [DisplayName("Conversion Color")]
        public string ConversionColor { get; set; }
        //[DisplayName("Cost Color")]
        //public string CostColor { get; set; }
        //[DisplayName("Revenue Color")]
        //public string RevenueColor { get; set; }
                
    }

    public class TickerElement
    {
        public int Id { get; set; }
        
        public int TickerId { get; set; }
        public virtual Ticker Ticker { get; set; }

        public int? CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        public int? AffiliateId { get; set; }
        public virtual Affiliate Affiliate { get; set; }
    }

    public class TickerCampaign
    {
        [Key, Column(Order = 0)]
        public int TickerId { get; set; }

        [Key, Column(Order = 1)]
        public int CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }
    }

    public class TickerSetting
    {
        [Key]
        [ForeignKey("UserProfile")]
        public int UserId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        [DisplayName("Background Color")]
        public string BackgroundColor { get; set; }
        [DisplayName("Campaign Color")]
        public string CampaignColor { get; set; }
        [DisplayName("Impression Color")]
        public string ImpressionColor { get; set; }
        [DisplayName("Click Color")]
        public string ClickColor { get; set; }
        [DisplayName("Conversion Color")]
        public string ConversionColor { get; set; }
        [DisplayName("Cost Color")]
        public string CostColor { get; set; }
        [DisplayName("Revenue Color")]
        public string RevenueColor { get; set; }
    }

    [Flags]
    public enum TickerView
    {
        [Display(Name = "Impressions")]
        Impressions = 0x1,

        [Display(Name = "Clicks")]
        Clicks = 0x2,

        [Display(Name = "Conversions")]
        Conversions = 0x4,

        [Display(Name = "Cost")]
        Cost = 0x8,

        [Display(Name = "Revenue")]
        Revenue = 0x10,

        //[Display(Name = "Cost per Click")]
        CPC = 0x20,

        //[Display(Name = "Cost per Sale (CPS)")]
        CPS = 0x40,

        //[Display(Name = "Cost per Thousand Impressions (CPM)")]
        CPM = 0x80,

        //[Display(Name = "Revenue per Click (RPC)")]
        RPC = 0x100,

        //[Display(Name = "Revenue per Sale (RPS)")]
        RPS = 0x200,

        //[Display(Name = "Revenue per Thousand Impressions (RPM)")]
        RPM = 0x400,
    }
}