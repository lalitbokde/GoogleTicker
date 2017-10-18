using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{
    public class CTRModel
    {
        public List<CTRView> List { get; set; }

        public Dictionary<int, Holder> Dic { get; set; }

        public int CTR(int? actionid)
        {
            if (actionid.HasValue && Dic.Keys.Contains(actionid.Value))
            {
                return Dic[actionid.Value].Clicks + CTR(Dic[actionid.Value].ParentURLId);
            }
            return 0;
        }
    }

    public class CTRView
    {
        public int CampaignId { get; set; }
        public int Id { get; set; }
        public string OfferUrl { get; set; }
        public int Clicks { get; set; }
        public int? ParentURLId { get; set; }
        public int PreviewId { get; set; }
        public string CampaignName { get; set; }

        //public bool IsRelevant()
        //{
        //    return Clicks != 0;
        //}

        public double CTR { get; set; }

        public string ParentURLIdText { get; set; }
    }

    public class Holder
    {
        public int? ParentURLId { get; set; }
        public int Clicks { get; set; }
        public int CampaignId { get; set; }
        public int PreviewId { get; set; }
    }
}