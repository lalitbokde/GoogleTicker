using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
     [Table("UserCustomReport")]
    public class UserCustomReport
    {
        [Key]
        public int ID { get; set; }
        public int UserId { get; set; }
        public int CustomerID { get; set; }
        public string ReportName { get; set; }
        public string ReportData { get; set; }
        public string ColumnOrder { get; set; }
    }
}