using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    [Table("OverridePayout")]
    public class OverridePayout
    {
        [Key]
        public int OverridID { get; set; }
        public int? ActionID { get; set; }
        public int CustomerID { get; set; }
        public int? AffiliateID { get; set; }
        public int? CampaignID { get; set; }
        public Decimal? PayoutPercent { get; set; }
        public PayoutType PayoutType { get; set; }
        public Decimal? Payout { get; set; }

     
    }
}