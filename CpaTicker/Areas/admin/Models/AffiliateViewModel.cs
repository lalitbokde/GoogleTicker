using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class AffiliateView
    {
        public int AffiliateId { get; set; }
        public string Company { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public int Impressions { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }

        public string Source { get; set; }

        public bool IsRelevant()
        {
            return Impressions != 0 ||
                    Clicks != 0 ||
                    Conversions != 0 ||
                    Cost != 0 ||
                    Revenue != 0;
        }

    }
    public class AffiliateViewModel
    {
        public List<AffiliateView> AffiliateViewList { get; set; }
        //public System.Data.Entity.Infrastructure.DbRawSqlQuery<AffiliateView> AffiliateViewList { get; set; }

        //public Filter Filter { get; set; }
        //public Statistics Stadisctics { get; set; }
        //public Calculation Calculation { get; set; }

        public StatisticsEnum StatisticsEnum { get; set; }

        //public AffiliateView Totals()
        //{
        //    var result = new AffiliateView();
        //    if (AffiliateViewList != null)
        //    {
        //        foreach (var item in AffiliateViewList)
        //        {
        //            result.Impressions += item.Impressions;
        //            result.Clicks += item.Clicks;
        //            result.Conversions += item.Conversions;
        //            result.Cost += item.Cost;
        //            result.Revenue += item.Revenue;
        //        }
        //    }
        //    return result;
        //}
    }

    public class CustomViewModel
    {
        public List<AffiliateView> AffiliateViewList { get; set; }
        //public System.Data.Entity.Infrastructure.DbRawSqlQuery<AffiliateView> AffiliateViewList { get; set; }

        //public Filter Filter { get; set; }
        //public Statistics Stadisctics { get; set; }
        //public Calculation Calculation { get; set; }

        public CustomStatisticsEnum CustomStatisticsEnum { get; set; }

        //public AffiliateView Totals()
        //{
        //    var result = new AffiliateView();
        //    if (AffiliateViewList != null)
        //    {
        //        foreach (var item in AffiliateViewList)
        //        {
        //            result.Impressions += item.Impressions;
        //            result.Clicks += item.Clicks;
        //            result.Conversions += item.Conversions;
        //            result.Cost += item.Cost;
        //            result.Revenue += item.Revenue;
        //        }
        //    }
        //    return result;
        //}
    }
}