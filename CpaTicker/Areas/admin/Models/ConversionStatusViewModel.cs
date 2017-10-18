using System;
using System.Collections.Generic;
using CpaTicker.Areas.admin.Classes;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    /// <summary>
    /// conversion status details
    /// </summary>
    public class ConversionStatusView
    {
        /// <summary>
        ///     company name
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        ///     campaign name
        /// </summary>
        public string CampaignName { get; set; }

        /// <summary>
        ///     total conversion
        /// </summary>
        public int GrossConversions
        {
            get { return ApprovedConversions + RejectedConversions; }
        }

        /// <summary>
        ///     total approved conversion
        /// </summary>
        public int ApprovedConversions { get; set; }

        /// <summary>
        ///     total rejected conversion
        /// </summary>
        public int RejectedConversions { get; set; }

        /// <summary>
        ///     total cost
        /// </summary>
        public decimal NetPayout { get; set; }

        /// <summary>
        ///     total revenue
        /// </summary>
        public decimal NetRevenue { get; set; }

        /// <summary>
        ///     affiliate id
        /// </summary>
        public int AffiliateId { get; set; }

        /// <summary>
        ///     campaign id
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        ///     country name
        /// </summary>
        public string Country { get; set; }
    }
    public class ConversionStatusViewModel
    {
        //public Statistics Statistics { get; set; }
        //public IEnumerable<ConversionStatusView> ConversionStatusViewList { get; set; }
        //public Filter Filter { get; set; }


        //public ConversionStatusView Totals()
        //{
        //    var result = new ConversionStatusView();
        //    foreach (var item in ConversionStatusViewList)
        //    {
        //        //result.GrossConversion += item.GrossConversion;
        //        result.ApprovedConversion += item.ApprovedConversion;
        //        result.RejectedConversions += item.RejectedConversions;
        //        result.NetPayout += item.NetPayout;
        //        result.NetRevenue += item.NetRevenue;
        //    }

        //    return result;
        //}

        public StatisticsEnum StatisticsEnum { get; set; }
    }
}