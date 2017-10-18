using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class AdCampaignView
    {
        /// <summary>
        /// ad/banner name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// campaign name
        /// </summary>
        public string CampaignName { get; set; }
        /// <summary>
        /// total impression
        /// </summary>
        public int Impressions { get; set; }
        /// <summary>
        /// total click
        /// </summary>
        public int Clicks { get; set; }
        /// <summary>
        /// total conversion
        /// </summary>
        public int Conversions { get; set; }
        /// <summary>
        /// total cost
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// total revenue
        /// </summary>
        public decimal Revenue { get; set; }
        /// <summary>
        /// ad/banner id
        /// </summary>
        public int BannerId { get; set; }
        /// <summary>
        /// campaign id
        /// </summary>
        public int CampaignId { get; set; }
        /// <summary>
        /// country name
        /// </summary>
        public string Country { get; set; }
    }
    public class AdCampaignViewModel
    {
        //public IEnumerable<AdCampaignView> AdCampaignViewList { get; set; }
        //public Filter Filter { get; set; }
        //public Statistics Statistics { get; set; }
        //public Calculation Calculation { get; set; }


        //public AdCampaignView Totals()
        //{
        //    var result = new AdCampaignView();
        //    foreach (var item in AdCampaignViewList)
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