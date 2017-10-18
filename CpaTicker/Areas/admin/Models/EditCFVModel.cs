using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class EditCFVModel
    {
        public CpaTicker.Areas.admin.Classes.CustomField CustomField { get; set; }
        public string Value { get; set; }
    }
}