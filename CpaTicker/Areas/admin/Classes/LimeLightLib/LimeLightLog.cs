using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes.LimeLightLib
{
    public class LimeLightLog
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Response { get; set; }
        public string Request { get; set; }
    }
}