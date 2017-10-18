using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class Action
    {
        public int Id { get; set; }

        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        //[System.Web.Mvc.AllowHtml]
        //[Required]
        //[Display(Name="Tracking Code")]
        //public string TrackingCode { get; set; }

        [Display(Name = "Conversion Type")]
        public ConversionType Type { get; set; }

        // the validation would be done in the view models 
        //[Required] 
        //[RemoteWithServerSideAttribute("CheckActionName", "Validation", "admin", AdditionalFields = "CampaignId", ErrorMessage = "There is already an action with this name.")]
        public string Name { get; set; }

        //[Display(Name="Id")]
        //public int PreviewActionId { get; set; }

        /************ Payout *************/

        [Display(Name = "Percent Cost")]
        public Decimal? PayoutPercent { get; set; }

        [Display(Name = "Payout Type")]
        public PayoutType PayoutType { get; set; }

        [Display(Name = "Cost")]
        public Decimal? Payout { get; set; }

        /************ Revenue *************/

        [Display(Name = "Percent Revenue")]
        public Decimal? RevenuePercent { get; set; }

        [Display(Name = "Revenue Type")]
        public RevenueType RevenueType { get; set; }

        [Display(Name = "Revenue")]
        public Decimal? Revenue { get; set; }

        /*************************************/

        //[Display(Name = "Tracking Type")]
        //public TrackingType TrackingType { get; set; }
    }
}