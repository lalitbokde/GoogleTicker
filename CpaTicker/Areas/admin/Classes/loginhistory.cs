using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    [Table("loginhistory")]
    public class loginhistory
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string UserAgent { get; set; }
        public string IPAddress { get; set; }
    }
}