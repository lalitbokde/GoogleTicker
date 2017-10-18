using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class ConversionLog
    {
        public int ConversionLogId { get; set; }
        public DateTime ConversionDate { get; set; }
        public bool Success { get; set; }
        public string Reason { get; set; }
        public string IPAddress { get; set; }
        public int? CampaignId { get; set; }
        public int? CustomerId { get; set; }
        public int? AffiliateId { get; set; }

        public string TransactionId { get; set; }

        //public DateTime  

        public bool? Postback { get; set; }

        public int PixelsFound { get; set; }

        public string Output { get; set; }
        public long TimeInterval { get; set; }
    }
}