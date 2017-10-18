using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class SparkModel
    {
        public decimal[] Revenues { get; set; }

        public decimal TodayRevenue {

            get {

                return this.Revenues.Last();
            }
        }

        public int[] Clicks { get; set; }
        public int[] Conversions { get; set; }

        public int TodayClicks
        {

            get
            {
                //string[] r = this.Clicks.Split(',');
                //return (int.Parse(r[r.Length - 1]) - int.Parse(r[r.Length - 2])).ToString();

                //return Clicks[Clicks.Length - 1] - Clicks[Clicks.Length - 2];

                return Clicks[Clicks.Length - 1];
            }
        }


        public string ClicksToString()
        {
            var sb = new StringBuilder();

            foreach (var item in Clicks)
            {
                sb.AppendFormat("{0},", item);
            }
            sb.Length--;
            return sb.ToString();

        }

        public string ConversionsToString()
        {
            var sb = new StringBuilder();

            foreach (var item in Conversions)
            {
                sb.AppendFormat("{0},", item);
            }
            sb.Length--;
            return sb.ToString();

        }

        public string RevenuesToString()
        {
            var sb = new StringBuilder();

            foreach (var item in Revenues)
            {
                sb.AppendFormat("{0},", item);
            }
            sb.Length--;
            return sb.ToString();

        }

        public int TodayCoversions
        {

            get
            {
                //string[] r = this.Conversions.Split(',');
                //return r[r.Length - 1];

                //return Conversions[Conversions.Length - 1] - Conversions[Conversions.Length - 2];

                return Conversions[Conversions.Length - 1];
            }
        }


    }

    public class Spark 
    {
        public decimal Revenue { get; set; }
        public int Conversions { get; set; }
        public int Clicks { get; set; }
    }
}