using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class URL
    {
        public int Id { get; set; }

        //[Display(Name="Url Name")]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        /************ Payout / Cost *************/

        [Display(Name = "Percent Cost")]
        [RequiredIfType(PropertyName = "PayoutType", ExpectedValue = ((int)(PayoutType.CPS | PayoutType.CPA_CPS)), ErrorMessage = "This Percent Cost field is required.")]
        public Decimal? PayoutPercent { get; set; }
        
        [Display(Name = "Payout Type")]
        public PayoutType PayoutType { get; set; }

        [RequiredIfNot(PropertyName = "PayoutType", ExpectedValue = PayoutType.CPS, ErrorMessage = "This Cost field is required.")]
        [Display(Name = "Cost")]
        public Decimal? Payout { get; set; }

        /************ Revenue *************/
        
        [Display(Name = "Percent Revenue")]
        [RequiredIfType(PropertyName = "RevenueType", ExpectedValue = ((int)(RevenueType.RPA_RPS | RevenueType.RPS)), ErrorMessage = "This Percent Revenue field is required.")]
        public Decimal? RevenuePercent { get; set; }
        
        [Display(Name = "Revenue Type")]
        public RevenueType RevenueType { get; set; }

        [Display(Name = "Revenue")]
        [RequiredIfNot(PropertyName = "RevenueType", ExpectedValue = RevenueType.RPS, ErrorMessage = "This Revenue field is required.")]
        public Decimal? Revenue { get; set; }

        /*************************************/
        
        [Display(Name = "Campaign URL")]
        public string OfferUrl { get; set; }

        [Display(Name = "Preview URL")]
        public string PreviewUrl { get; set; }

        [Display(Name = "Parent URL")]
        [ForeignKey("ParentUrl")]
        public int? ParentURLId { get; set; }
        public virtual URL ParentUrl { get; set; }

        public int PreviewId { get; set; }

        public virtual ICollection<RedirectUrl> RedirectUrls { get; set; }
    }

    public class RedirectUrl
    {
        public int Id { get; set; }

        public int URLId { get; set; }
        public virtual URL URL { get; set; }

        [DisplayName("Redirect URL")]
        public string RedirectURL { get; set; }

        public virtual ICollection<RedirectTarget> Targets { get; set; }

    }

    public class RedirectTarget
    {
        [Key, Column(Order = 0), ForeignKey("IP2Country")]
        public long Min { get; set; }
        public virtual IP2Country IP2Country { get; set; }

        [Key, Column(Order = 1)]
        public int RedirectUrlId { get; set; }
        public virtual RedirectUrl RedirectUrl { get; set; }
    }
}