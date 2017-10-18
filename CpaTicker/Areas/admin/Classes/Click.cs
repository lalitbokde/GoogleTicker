using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class Click
    {
        public int ClickId { get; set; }
        public int CustomerId { get; set; }
        public int CampaignId { get; set; }
        public int AffiliateId { get; set; }
        public DateTime ClickDate { get; set; }

        [MaxLength(2500), Column(TypeName = "nvarchar")]
        public string UserAgent { get; set; }

        [MaxLength(39), Column(TypeName = "varchar")]
        public string IPAddress { get; set; }

        //[MaxLength(6000), Column(TypeName = "nvarchar")] // 0,0) : error 0026: MaxLength '6000' is not valid. Length must be between '1' and '4000' for 'nvarchar' type.
        public string Referrer { get; set; }

        [MaxLength(255), Column(TypeName = "nvarchar")]
        public string Source { get; set; }

        [MaxLength(36), Column(TypeName = "char")]
        public string TransactionId { get; set; }
        
        [Index]
        public ClickStatus Status { get; set; }

        public decimal Cost { get; set; }

       
        public decimal Revenue { get; set; }

        public int BannerId { get; set; }

        
        public int URLPreviewId { get; set; }

        public int? Random { get; set; }

        public bool Cookies { get; set; }

        public int? RedirectUrlId { get; set; }
        public virtual RedirectUrl RedirectUrl { get; set; }
        
        [MaxLength(2), Column(TypeName = "char")]
        public string Country { get; set; }

        //public virtual ICollection<ClickSubId> SubIds { get; set; }

        public long UserAgentId { get; set; }

        public long TimeInterval { get; set; }
        public int bot { get; set; }

        public int URLId { get; set; }

        public string SubId1 { get; set; }
        public string SubId2 { get; set; }
        public string SubId3 { get; set; }
        public string SubId4 { get; set; }
        public string SubId5 { get; set; }
        public string SubId6 { get; set; }
        public string SubId7 { get; set; }
        public string SubId8 { get; set; }
        public string SubId9 { get; set; }
        public string SubId10 { get; set; }
    }

    public enum ClickStatus
    { 
        Archive = 0,
        Active,
    }


    public class ClickSubId
    {
        public int Id { get; set; }

        public int ClickId { get; set; }
        public virtual Click Click { get; set; }
        
        [MaxLength(1000), Column(TypeName = "nvarchar")]
        public string SubValue { get; set; }
        
        public int SubIndex { get; set; }
    }

    
}