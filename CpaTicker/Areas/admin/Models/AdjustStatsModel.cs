using CpaTicker.Areas.admin.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Models
{
    public class AdjustStatsModel
    {
        public IQueryable<SelectListItem> AffiliateList { get; set; }
        public IQueryable<SelectListItem> BannerList { get; set; }
        public IQueryable<SelectListItem> CampaignList { get; set; }
    }
}