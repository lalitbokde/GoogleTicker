using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public enum AffiliateStatus
    {
        Active = 1, 
        Pending = 2, 
        Blocked = 3,
        Deleted = 4, 
        Rejected = 5
    }
    public class Affiliate
    {
        public int Id { get; set; }
        public int AffiliateId { get; set; }
        public int CustomerId { get; set; }
        public string Company { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        
        [ForeignKey("Country")]
        [DisplayName("Country")]
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
        

        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public AffiliateStatus Status { get; set; }


    }
}