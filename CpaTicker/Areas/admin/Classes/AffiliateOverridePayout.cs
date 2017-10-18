using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    [Table("AffiliateOverridePayout")]
    public class AffiliateOverridePayout
    {
        [Key]
        public int OverridID { get; set; }
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