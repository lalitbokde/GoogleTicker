using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    [Table("NewsLetter")]
    public class NewsLetter
    {
        [Key]
        public int ID { get; set; }
        public string Email { get; set; }
    }
}