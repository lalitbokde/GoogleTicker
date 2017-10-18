using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class ActionEditVM
    {
        public int Id { get; set; }
        
        public int CampaignId { get; set; }

        //[System.Web.Mvc.AllowHtml]
        //[Required]
        //[Display(Name = "Tracking Code")]
        //public string TrackingCode { get; set; }

        [Display(Name = "Action Type")]
        public ConversionType Type { get; set; }

        [Required]
        [RemoteWithServerSideAttribute("CheckActionNameForEdit", "Validation", "admin", AdditionalFields = "Id", ErrorMessage = "There is already an action with this name.")]
        public string Name { get; set; }

        /************ Payout *************/

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

        //[Display(Name = "Tracking Type")]
        //public TrackingType TrackingType { get; set; }
    }

    public class ActionCreateVM
    {
        //public int Id { get; set; }

        public int CampaignId { get; set; }

        //[Required]
        //[System.Web.Mvc.AllowHtml]
        //[Display(Name = "Tracking Code")]
        //public string TrackingCode { get; set; }

        [Display(Name="Action Type")]
        public ConversionType Type { get; set; }

        [Required]
        [RemoteWithServerSideAttribute("CheckActionName", "Validation", "admin", AdditionalFields = "CampaignId", ErrorMessage = "There is already an action with this name.")]
        public string Name { get; set; }

        /************ Payout *************/

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

        //[Display(Name = "Tracking Type")]
        //public TrackingType TrackingType { get; set; }
    }
}