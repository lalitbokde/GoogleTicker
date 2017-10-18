using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class TrafficView
    {
        public string Company { get; set; }
        public string CampaignName { get; set; }
        public string OfferUrl { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public int Impressions { get; set; }
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
        public int AffiliateId { get; set; }
        public int CampaignId { get; set; }

        //public bool IsRelevant()
        //{
        //    return Impressions != 0 ||
        //            Clicks != 0 ||
        //            Conversions != 0 ||
        //            Cost != 0 ||
        //            Revenue != 0;
        //}

        public string Country { get; set; }

        public int UrlPreviewId { get; set; }
    }
    public class TrafficViewModel
    {
        //public IEnumerable<TrafficView> TrafficViewList { get; set; } 
        //public Filter Filter { get; set; }
        //public Statistics Stadisctics { get; set; }
        //public Calculation Calculation { get; set; }

        //public TrafficView Totals()
        //{
        //    var result = new TrafficView();
        //    foreach (var item in TrafficViewList)
        //    {
        //        result.Impressions += item.Impressions;
        //        result.Clicks += item.Clicks;
        //        result.Conversions += item.Conversions;
        //        result.Cost += item.Cost;
        //        result.Revenue += item.Revenue;
        //    }

        //    return result;
        //}

        public StatisticsEnum StatisticsEnum { get; set; }
    }
}