using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class HourlyViewModel 
    {
        //public  IEnumerable<HourlyView> HourlyViewList { get; set; }
        //public Interval Interval { get; set; }
        //public Filter Filter { get; set; }
        //public Statistics Stadisctics { get; set; }
        //public Calculation Calculation { get; set; }

        //public HourlyView Totals()
        //{
        //    HourlyView result = new HourlyView();
        //    foreach (var item in HourlyViewList)
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
    public class HourlyView
    {
        //public string Date { get; set; }
        public int Hour { get; set; }
        //public int Year { get; set; }
        //public int Month { get; set; }
        //public int Day { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
        //public decimal Profit { get; set; }

        //public bool IsRelevant()
        //{
        //    return Impressions != 0 ||
        //            Clicks != 0 ||
        //            Conversions != 0
        //            ;
        //}


        public string Country { get; set; }
    }
}