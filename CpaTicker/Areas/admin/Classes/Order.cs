using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int OrderId { get; set; }
        
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        public int Declines { get; set; }
    }

    public class Transaction
    {
        // this is the new orderid
        [Key, Column(Order = 0)]
        public int TransactionId { get; set; }

        [Key, Column(Order = 1)]
        public int OrderId { get; set; }
        
        public bool Status { get; set; }

        public virtual Order Order { get; set; }
    }

    //public class APIKeyOrder
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int OrderId { get; set; }

    //    [ForeignKey("UserProfile")]
    //    public int UserId { get; set; }
    //    public virtual UserProfile UserProfile { get; set; }

    //    public int Declines { get; set; }
    //}

}