using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class DispAffiliateOverride
    {
        public int OverrideID { get; set; }
        public int CustomerID { get; set; }
        public int? AffiliateID { get; set; }
        public Decimal? PayoutPercent { get; set; }
        public PayoutType PayoutType { get; set; }
        public Decimal? Payout { get; set; }
        public Decimal? RevenuePercent { get; set; }
        public RevenueType RevenueType { get; set; }
        public Decimal? Revenue { get; set; }
    }
}