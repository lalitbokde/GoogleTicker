using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    [Table("CustomTimeZone")]
    public class CustomTimeZone
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public string DisplayID { get; set; }
        public int offset { get; set; }
        public int dstoffset { get; set; }
        public bool IsdstSupport { get; set; }
        public DateTime? dstStart { get; set; }
        public DateTime? dstEnd { get; set; }

    }
}