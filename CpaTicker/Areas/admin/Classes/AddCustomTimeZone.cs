using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class AddCustomTimeZone
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public string DisplayID { get; set; }
        public int offsetHour { get; set; }
        public int offsetMinute { get; set; }
        public int dstoffsetHour { get; set; }
        public int dstoffsetMinute { get; set; }
        public bool IsdstSupport { get; set; }
        public DateTime? dstStart { get; set; }
        public DateTime? dstEnd { get; set; }
    }
}