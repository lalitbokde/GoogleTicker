using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Classes
{
    public class ConversionPixel
    {
        public int Id { get; set; }
        //public int CustomerId { get; set; } // getting this value from the affiliate

        //[Required]
        [ForeignKey("Affiliate")]
        public int AffiliateId { get; set; }  
        public virtual Affiliate Affiliate { get; set; } // this field is providing the customerid

        //public int CampaignId { get; set; }

        [Display(Name = "Tracking Type")]
        public TrackingType TrackingType { get; set; }
        
        [AllowHtml]
        //[Required(ErrorMessage = "Pixel is required.")]
        [RequireHttpsIf("TrackingType", "Https is required")]
        [Required]
        [Display(Name="Pixel Code")]
        public string PixelCode { get; set; }

        public virtual ICollection<ConversionPixelCampaign> Campaigns { get; set; }

        public virtual ICollection<ActionConversionPixel> Actions { get; set; }
    }

    public enum PixelStatus
    {
        Active = 1,
        Paused = 2,
        Pending = 3,
        Blocked = 4,
        Rejected = 5,
    }

    public class ConversionPixelCampaign
    {
        public int Id { get; set; }

        public int ConversionPixelId { get; set; }
        public virtual ConversionPixel ConversionPixel { get; set; }

        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        [Display(Name = "Status")]
        public PixelStatus PixelStatus { get; set; }
    }

    public class ActionConversionPixel
    {
        public int Id { get; set; }

        public int ActionId { get; set; }
        public virtual Action Action { get; set; }

        public int ConversionPixelId { get; set; }
        public virtual ConversionPixel ConversionPixel { get; set; }
    }
}