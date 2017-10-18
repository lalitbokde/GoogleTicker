using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes;

namespace CpaTicker.ViewModels.Admin
{
    public class IndexViewModel
    {
        public int NumCampaigns { get; set; }
        public int NumClicks { get; set; }
        public int NumConversions { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Cost { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Revenue { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Profit { get; set; }
        public List<Areas.admin.Classes.Affiliate> Affiliates { get; set; }
        public List<Areas.admin.Classes.Campaign> Campaigns { get; set; }
    }
}