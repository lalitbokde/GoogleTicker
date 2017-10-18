using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class Impression
    {
        public int ImpressionId { get; set; }
        public int CustomerId { get; set; }
        public int CampaignId { get; set; }
        public int AffiliateId { get; set; }
        public DateTime ImpressionDate { get; set; }
        
        [MaxLength(2000), Column(TypeName = "nvarchar")]
        public string UserAgent { get; set; }
        
        [MaxLength(39), Column(TypeName = "varchar")]
        public string IPAddress { get; set; }

        //[MaxLength(6000), Column(TypeName = "nvarchar")] // 0,0) : error 0026: MaxLength '6000' is not valid. Length must be between '1' and '4000' for 'nvarchar' type.
        public string Referrer { get; set; }

        [MaxLength(255), Column(TypeName = "nvarchar")]
        public string Source { get; set; }
       
        public decimal Cost { get; set; }
       
        public decimal Revenue { get; set; }
        public int BannerId { get; set; }
                
        public int URLPreviewId { get; set; }
        
        [MaxLength(2), Column(TypeName = "char")]
        public string Country { get; set; }

        public virtual ICollection<ImpressionSubId> SubIds { get; set; }

        public int bot { get; set; }

        public int URLId { get; set; }
    }

    public class ImpressionSubId
    {
        public int Id { get; set; }

        public int ImpressionId { get; set; }
        public virtual Impression Impression { get; set; }

        [MaxLength(1000), Column(TypeName = "nvarchar")]
        public string SubValue { get; set; }
        
        [Index]
        public int SubIndex { get; set; }
    }
}