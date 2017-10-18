using CpaTicker.Areas.admin.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin
{
    public class DispOverrideAffiliate
    {
        public int OverrideID { get; set; }
        public int CustomerID { get; set; }
        public int? AffiliateID { get; set; }
        public int? CampaignID { get; set; }
        public int? ActionID { get; set; }
        public int? UrlID { get; set; }
        public string affiliate { get; set; }
        public string campaign { get; set; }
        public string action { get; set; }
        public string url { get; set; }
        public Decimal? PayoutPercent { get; set; }
        public PayoutType PayoutType { get; set; }
        public Decimal? Payout { get; set; }
        public Decimal? RevenuePercent { get; set; }
        public RevenueType RevenueType { get; set; }
        public Decimal? Revenue { get; set; }
    }
}