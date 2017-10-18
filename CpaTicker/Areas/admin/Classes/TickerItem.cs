using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class TickerItem
    {
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
        /// total campaign cost
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// total campaign revenue
        /// </summary>
        public decimal Revenue { get; set; }
        /// <summary>
        /// total old click
        /// </summary>
        public int OldClicks { get; set; }
        /// <summary>
        /// total old impression
        /// </summary>
        public int OldImpressions { get; set; }
        /// <summary>
        /// total old conversion
        /// </summary>
        public int OldConversions { get; set; }
    }


    public class TickerItemExt : TickerItem
    {
        public int CampaignId { get; set; }
        //public string CampaignName { get; set; }

        public int AffiliateId { get; set; }
        public string Company { get; set; }

        //public int Impressions { get; set; }
        //public int Clicks { get; set; }
        //public int Conversions { get; set; }
        //public decimal Cost { get; set; }
        //public decimal Revenue { get; set; }

        //public int OldClicks { get; set; }
        //public int OldImpressions { get; set; }
        //public int OldConversions { get; set; }

       
    }

    public class TickerElementView 
    {
      
        public string Title { get; set; }

        
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }

        public int OldClicks { get; set; }
        public int OldImpressions { get; set; }
        public int OldConversions { get; set; }
    }
}