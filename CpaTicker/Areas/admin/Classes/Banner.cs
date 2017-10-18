using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class Banner
    {
        public int Id { get; set; }
        public int BannerId { get; set; }
        public DateTime BannerDate { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public int CampaignId { get; set; }
        public string Name { get; set; }

        //[Required]
        public byte[] Image { get; set; }
    }
}