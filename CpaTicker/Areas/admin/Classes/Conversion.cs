using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CpaTicker.Areas.admin.Classes
{
    public class Conversion
    {
        public int ConversionId { get; set; }
        public int CustomerId { get; set; }
        public int CampaignId { get; set; }

        public int ClickId { get; set; }
        public virtual Click Click { get; set; }

        public DateTime ConversionDate { get; set; }
        
        public int AffiliateId { get; set; }
        
        [MaxLength(2500), Column(TypeName = "nvarchar")]
        public string UserAgent { get; set; }
        
        [MaxLength(39), Column(TypeName = "varchar")]
        public string IPAddress { get; set; }

        //[MaxLength(6000), Column(TypeName = "nvarchar")] // 0,0) : error 0026: MaxLength '6000' is not valid. Length must be between '1' and '4000' for 'nvarchar' type.
        public string Referrer { get; set; }
        
        [MaxLength(36), Column(TypeName = "char")]
        public string TransactionId { get; set; }
       
        public decimal Cost { get; set; }
                
        public decimal Revenue { get; set; }
        [DefaultValue(1)]
        public int Status { get; set; }
        public int BannerId { get; set; }

        public ConversionType Type { get; set; }

        public DateTime? Pixel { get; set; }

        public DateTime? Postback { get; set; }

        [MaxLength(2500), Column(TypeName = "nvarchar")]
        [Display(Name="Status Description")]
        public string StatusDescription { get; set; }
        
        [MaxLength(39), Column(TypeName = "varchar")]
        public string Postback_IPAddress { get; set; }

        public int? ActionId { get; set; }
        public virtual Action Action { get; set; }
        
        [MaxLength(2), Column(TypeName = "char")]
        public string Country { get; set; }

        public long UserAgentId { get; set; }

        public long TimeInterval { get; set; }
        public int bot { get; set; }

        public string act_data1 { get; set; }
        public string act_data2 { get; set; }
        public string act_data3 { get; set; }
        public string act_data4 { get; set; }
        public string act_data5 { get; set; }

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

    public enum ConversionType
    { 
        Sale = 1,
        Lead,
        Other,
        Clickthrough,
    }

    public enum PixelType : byte
    {
        [Display(Name="Postback")]
        Postback = 1,
        [Display(Name="Pixel")]
        User,
    }
}
