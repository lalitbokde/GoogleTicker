using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    [Table("TimeClicks")]
    public class TimeClicks
    {
        [Key]
        public int Id { get; set; }
        public int ClickId { get; set; }
        public int Campaign { get; set; }
        public int URL { get; set; }
        public int SubID { get; set; }
        public int Agent { get; set; }
        public int Click { get; set; }
        public int Cookie { get; set; }
        public int total { get; set; }
    }
}