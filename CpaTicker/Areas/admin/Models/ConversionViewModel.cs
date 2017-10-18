using System;
using System.Collections.Generic;
using CpaTicker.Areas.admin.Classes;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class ConversionView
    {
        public DateTime ConversionDate { get; set; }
        public int Status { get; set; }
        public string CampaignName { get; set; }
        public string Company { get; set; }
        public string UserAgent { get; set;  }
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
        public string IPAddress { get; set; }
        public string TransactionId { get; set; }
        public int ConversionId { get; set; }
        public int AffiliateId { get; set; }
        public int CampaignId { get; set; }
        //public string Subid1 { get; set; }
        //public string Subid2 { get; set; }
        //public string Subid3 { get; set; }
        //public string Subid4 { get; set; }
        //public string Subid5 { get; set; }
        public ConversionType Type { get; set; }
        public int URLId { get; set; }
        public string PreviewUrl { get; set; }
        //public int PreviewId { get; set; }
        public string Source { get; set; }

        //public PixelType FiredPixels { get; set; }

        public DateTime? Pixel { get; set; }

        public DateTime? Postback { get; set; }

        //[Display(Name = "Status Description")]
        public string StatusDescription { get; set; }

        public ICollection<ClickSubId> SubIds { get; set; }

        public string Postback_IPAddress { get; set; }

        public string ActionName { get; set; }

        public string Country { get; set; }
        public long UserAgentId { get; set; }
    }
    public class ConversionViewModel
    { 
        //public List<ConversionView> ConversionViewList { get; set; }
        //public Filter Filter { get; set; }
        //public Statistics Stadisctics { get; set; }

        //public ConversionView Totals()
        //{
        //    var result = new ConversionView();
        //    foreach (var item in ConversionViewList)
        //    {
        //        result.Cost += item.Cost;
        //        result.Revenue += item.Revenue;
        //    }

        //    return result;
        //}

        public StatisticsEnum StatisticsEnum { get; set; }
    }
}