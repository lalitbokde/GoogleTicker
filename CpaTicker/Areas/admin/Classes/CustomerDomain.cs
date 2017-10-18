using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class CustomerDomain
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("Domain")]
        public int DomainId { get; set; }
        public virtual Domain Domain { get; set; }
    }
}