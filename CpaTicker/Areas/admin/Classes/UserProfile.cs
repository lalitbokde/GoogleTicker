using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CustomerId { get; set; }
        public int? AffiliateId { get; set; }
        //public ViewPermission Permissions { get; set; }
        public long Permissions { get; set; }
        public long Permissions1 { get; set; }
        public Guid APIKey { get; set; }
        public int? OrderId { get; set; }
        public string Email { get; set; }
        public virtual ICollection<UserHiddenCampaign> HiddenCampaigns { get; set; }
        public virtual ICollection<Ticker> Tickers { get; set; }
    }

    public class UserHiddenCampaign
    {
        //public int Id { get; set; }

        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        [Key, Column(Order = 1)]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

    }

    //[Flags]
    //public enum ViewPermission : long
    //{
    //    /**
    //     * REALLY IMPORTANT !!!!
    //     * In order to be applied dynamically to every action in each controller i'm going to assume a name 
    //     * convension : 
    //     * it must be defined in lower case (action) in first letter capitalize the controller in the format:
    //     * actionController ej createCampaign         
    //     */

    //    //All = 0x0,
    //    [Display(Name = "Campaign")]
    //    indexCampaign = 0x1,

    //    [Display(Name = "Banner")]
    //    indexBanner = 0x2,

    //    [Display(Name = "Affiliate")]
    //    indexAffiliate = 0x4,

    //    [Display(Name = "Reports")]
    //    indexReports = 0x8,

    //    [Display(Name = "Settings")]
    //    indexSettings = 0x10,

    //    [Display(Name = "Create Campaign")]
    //    createCampaign = 0x20,

    //    [Display(Name = "Create Banner")]
    //    createBanner = 0x40,

    //    [Display(Name = "Edit Campaign")]
    //    editCampaign = 0x80,

    //    [Display(Name = "GenerateLinks")]
    //    detailsCampaign = 0x100,

    //    [Display(Name = "Edit Banner")]
    //    editBanner = 0x200,

    //    [Display(Name = "Banner Details")]
    //    detailsBanner = 0x400,

    //    [Display(Name = "Create Affiliate")]
    //    createAffiliate = 0x800,

    //    [Display(Name = "Edit Affiliate")]
    //    editAffiliate = 0x1000,

    //    [Display(Name = "ConversionPixels")]
    //    detailsAffiliate = 0x2000,

    //    [Display(Name = "Hourly Report")]
    //    hourlyReports = 0x4000,

    //    [Display(Name = "Daily Report")]
    //    dailyReports = 0x8000,

    //    [Display(Name = "Edit Customer Details")]
    //    editSettings = 0x10000,

    //    [Display(Name = "Create Customer Domain")]
    //    addcustomerdomainSettings = 0x20000,

    //    [Display(Name = "Create User")]
    //    addcustomeruserSettings = 0x40000,

    //    [Display(Name = "Set Permissions")]
    //    setpermissionsSettings = 0x80000,

    //    [Display(Name = "Delete Banner")]
    //    deleteBanner = 0x100000,

    //    [Display(Name = "Affiliate Report")]
    //    affiliateReports = 0x200000,

    //    [Display(Name = "Conversion Report")]
    //    conversionReports = 0x400000,

    //    [Display(Name = "AdCampaign Report")]
    //    adcampaignReports = 0x800000,

    //    [Display(Name = "Reset Password")]
    //    resetpwdSettings = 0x1000000,

    //    [Display(Name = "Delete User")]
    //    removecustomeruserSettings = 0x2000000,

    //    [Display(Name = "Remove Domain")]
    //    removecustomerdomainSettings = 0x4000000,

    //    [Display(Name = "Change Password")]
    //    changepwdSettings = 0x8000000,

    //    [Display(Name = "Offer Report")]
    //    offerReports = 0x10000000,

    //    [Display(Name = "Traffic Report")]
    //    trafficReports = 0x20000000
    //}
}