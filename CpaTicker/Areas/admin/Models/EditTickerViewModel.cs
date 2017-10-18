using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class EditTickerViewModel
    {
        public int CamapaignId { get; set; }
        public bool HasCampaign { get; set; }
        public string CamapaignName { get; set; }
    }

    public class EditCampaignCountryViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Checked { get; set; }
    }
}