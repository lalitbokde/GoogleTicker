using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class DailyView
    {
        public string Date { get; set; }

        //public string Year { get { return Date.Split('/').Last(); } }
        //public string Month { get { return Date.Split('/').First(); } }
        //public string Day { get { return Date.Split('/')[1]; } }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
       

        //public bool IsRelevant()
        //{
        //    return Impressions != 0 ||
        //            Clicks != 0 ||
        //            Conversions != 0 
        //            ;
        //}


        public string Country { get; set; }
    }

    
    public class DailyViewModel
    {
        //public IEnumerable<DailyView> DailyViewList { get; set; }
        //public Interval Interval { get; set; }
        //public Filter Filter { get; set; }
        //public Statistics Stadisctics { get; set; }
        //public Calculation Calculation { get; set; }

        //public IntervalEnum IntervalEnum { get; set; }
        public StatisticsEnum StatisticsEnum { get; set; }
        //public DailyView Totals()
        //{
        //    DailyView result = new DailyView();
        //    foreach (var item in DailyViewList)
        //    {
        //        result.Impressions += item.Impressions;
        //        result.Clicks += item.Clicks;
        //        result.Conversions += item.Conversions;
        //        result.Cost += item.Cost;
        //        result.Revenue += item.Revenue;
        //    }

        //    return result;
        //}
    }
}