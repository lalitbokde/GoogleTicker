using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    [Table("UserAffiliate")]
    public class UserAffiliate
    {
        [Key]
        public int ID { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public int AffiliateId { get; set; }
    }
}