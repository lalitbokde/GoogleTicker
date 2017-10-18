using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class Customer
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int CustomerId { get; set; }
        public string AccountId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Guid APIKey { get; set; }
        public string TimeZone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CreditCardData { get; set; }
        [Column(TypeName = "Date")]
        public DateTime MemberSince { get; set; }
        //public string Email { get; set; }
        public Level Level { get; set; }

        //[DefaultValue(1)]
        [Display(Name="TransactionId Type")]
        public TransactionIdGenerationType TransactionIdType { get; set; }

        [ForeignKey("Country")]
        [DisplayName("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        //public string IPAddress { get; set; }
    }

    public enum Level
    { 
        Gold = 1,
        Platinum = 2,
        //Diamond = 3
    }

    public class CustomField
    {
        public int CustomFieldId { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [DisplayName("Field Name")]
        [Required]
        [RemoteWithServerSideAttribute("CheckFieldNameExists", "Validation", "admin", ErrorMessage = "This field name is already taken.")]
        public string FieldName { get; set; }
    }

    public class CustomFieldValue
    {
        [Key, Column(Order = 0)]
        public int CustomFieldId { get; set; }
        public virtual CustomField CustomField { get; set; }
        [Key, Column(Order = 1)]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        public string Value { get; set; }
    }

    
    public enum TransactionIdGenerationType : byte
    {
        Random, // = 0;
        [Description("IP Address + Date")]
        IPDate,
        [Description("IP Address + Date + Campaign ID")]
        IPDateCampaign,
    }
}