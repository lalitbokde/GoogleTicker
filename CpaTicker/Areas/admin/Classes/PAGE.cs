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
    [Table("PAGE")]
    public class PAGE
    {
        [Key]
        public int Id { get; set; }

        //[Display(Name="Url Name")]
        public string Name { get; set; }

        public int CustomerID { get; set; }


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

        [Display(Name = "Campaign Page")]
        public string OfferUrl { get; set; }

        [Display(Name = "Preview Page")]
        public string PreviewUrl { get; set; }

        [Display(Name = "Parent Page")]
        [ForeignKey("ParentUrl")]
        public int? ParentURLId { get; set; }
        public virtual PAGE ParentUrl { get; set; }

        public int PreviewId { get; set; }

        [Display(Name="Page Category")]
        public int? PageCategoryId { get; set; }

        [Required, DefaultValue(1)]
        public PageStatus Status { get; set; }

        public virtual ICollection<RedirectPAGE> RedirectPages { get; set; }        

        public virtual PAGECategory PageCategory { get; set; }
    }

    public class RedirectPAGE
    {
        public int Id { get; set; }

        public int PAGEId { get; set; }
        public virtual PAGE PAGE { get; set; }

        [DisplayName("Redirect PAGE")]
        public string RedirectPage { get; set; }

        public virtual ICollection<RedirectTargetPage> Targets { get; set; }

    }

    public class RedirectTargetPage
    {
        [Key, Column(Order = 0), ForeignKey("IP2Country")]
        public long Min { get; set; }
        public virtual IP2Country IP2Country { get; set; }

        [Key, Column(Order = 1)]
        public int RedirectPageId { get; set; }
        public virtual RedirectPAGE RedirectPAGE { get; set; }
    }
    public enum PageStatus
    {
        Active = 1,
        InActive = 2,
        Deleted = 3
    }

    public enum PageFilter
    {
        [Display(Name = "Show all Pages")]
        all = 1,
        [Display(Name = "Show only active")]
        active,
        [Display(Name = "Show only Inactive")]
        Inactive,
        [Display(Name = "Show only deleted")]
        deleted
    }
}