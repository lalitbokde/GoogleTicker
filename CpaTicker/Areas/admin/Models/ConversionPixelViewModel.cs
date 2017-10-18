using System;
using System.Collections.Generic;
using CpaTicker.Areas.admin.Classes;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CpaTicker.Areas.admin.Classes.Helpers;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Models
{
    public class ConversionPixelViewModel
    {
        [RequiredArray]
        public int[] Campaigns { get; set; }
        [Required]
        public int AffiliateId { get; set; }

        [Required]
        [Display(Name = "Tracking Type")]
        public TrackingType TrackingType { get; set; }

        [AllowHtml]
        [Required]
        [RequireHttpsIf("TrackingType", "Https is required")]
        [Display(Name = "Pixel Code")]
        public string PixelCode { get; set; }
        
        [Required]
        [Display(Name = "Pixel Status")]
        public PixelStatus PixelStatus { get; set; }
    }

    public class ConversionPixelCampaignsViewModel
    {
        public int[] Campaigns { get; set; }

        [Required]
        [Display(Name = "Pixel Status")]
        public PixelStatus PixelStatus { get; set; }
    }
}