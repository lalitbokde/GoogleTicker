using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class DispOverridePayout
    {
        public int OverrideID { get; set; }
        public string affiliate { get; set; }
        public string campaign { get; set; }
        public string action { get; set; }
        public Decimal? Payout { get; set; }
        public Decimal? PayoutPercent { get; set; }
    }
}