using System;

namespace CpaTicker.Areas.admin.Classes
{
    /// <summary>
    ///     Conversion detail
    /// </summary>
    public class CustomConversion
    {
        /// <summary>
        ///     affiliate id
        /// </summary>
        public int AffiliateId { get; set; }

        /// <summary>
        ///     company name
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        ///     campaign id
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        ///     campaign name
        /// </summary>
        public string CampaignName { get; set; }

        /// <summary>
        ///     conversion date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     conversion id
        /// </summary>
        public int ConversionId { get; set; }

        /// <summary>
        ///     click's referrer
        /// </summary>
        public string Referrer { get; set; }

        /// <summary>
        ///     clicked url's preview id
        /// </summary>
        public int URLId { get; set; }

        /// <summary>
        ///     clicked url's preview url
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        ///     user agent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        ///     transaction id
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        ///     Ip address
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        ///     type
        /// </summary>
        public ConversionType Type { get; set; }

        /// <summary>
        ///     click's sub ids
        /// </summary>
        public string SubIds { get; set; }

        /// <summary>
        ///     cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        ///     revenue
        /// </summary>
        public decimal Revenue { get; set; }

        /// <summary>
        ///     click source
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        ///     status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///     status description
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        ///     post back ip address
        /// </summary>
        public string Postback_IPAddress { get; set; }

        /// <summary>
        ///     conversion's pixel
        /// </summary>
        public DateTime? Pixel { get; set; }

        /// <summary>
        ///     conversion's postback date time
        /// </summary>
        public DateTime? Postback { get; set; }

        /// <summary>
        ///     action name
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        ///     conversion's country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        ///     conversion's user agent id
        /// </summary>
        public long UserAgentId { get; set; }

        /// <summary>
        ///     device id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        ///     is device a smartphone
        /// </summary>
        public bool IsSmartphone { get; set; }

        /// <summary>
        ///     is device IOS
        /// </summary>
        public bool IsiOS { get; set; }

        /// <summary>
        ///     is device Android
        /// </summary>
        public bool IsAndroid { get; set; }

        /// <summary>
        ///     device's installed os detail, like Android 5.0
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        ///     used browser
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        ///     device's os provider, like Android
        /// </summary>
        public string Device_os { get; set; }

        /// <summary>
        ///     device's pointin method, like touchscreen/mouse
        /// </summary>
        public string Pointing_method { get; set; }

        /// <summary>
        ///     id device tablet
        /// </summary>
        public bool Is_tablet { get; set; }

        /// <summary>
        ///     device build model name
        /// </summary>
        public string Model_name { get; set; }

        /// <summary>
        ///     device's installed os detail, like Android 5.0 where 5.0 is the version
        /// </summary>
        public string Device_os_version { get; set; }

        /// <summary>
        ///     is device wireless
        /// </summary>
        public bool Is_wireless_device { get; set; }

        /// <summary>
        ///     device brand/manufacturer name
        /// </summary>
        public string Brand_name { get; set; }

        /// <summary>
        ///     device marketing name
        /// </summary>
        public string Marketing_name { get; set; }

        /// <summary>
        ///     is device with a phone number
        /// </summary>
        public bool Is_assign_phone_number { get; set; }

        /// <summary>
        ///     device's xhtmlmp mime type
        /// </summary>
        public string Xhtmlmp_mime_type { get; set; }

        /// <summary>
        ///     device's xhtml support level
        /// </summary>
        public string Xhtml_support_level { get; set; }

        /// <summary>
        ///     device display resolution height
        /// </summary>
        public string Resolution_height { get; set; }

        /// <summary>
        ///     device display resolution width
        /// </summary>
        public string Resolution_width { get; set; }

        /// <summary>
        ///     device's canvas support
        /// </summary>
        public string Canvas_support { get; set; }

        /// <summary>
        ///     device's viewport width
        /// </summary>
        public string Viewport_width { get; set; }

        /// <summary>
        ///     device's html preferred dtd
        /// </summary>
        public string Html_preferred_dtd { get; set; }

        /// <summary>
        ///     does device support view port
        /// </summary>
        public bool Isviewport_supported { get; set; }

        /// <summary>
        ///     is device mobile optimized
        /// </summary>
        public bool Ismobileoptimized { get; set; }

        /// <summary>
        ///     Is image inlining
        /// </summary>
        public bool Isimage_inlining { get; set; }

        /// <summary>
        ///     Is handheld friendly
        /// </summary>
        public bool Ishandheldfriendly { get; set; }

        /// <summary>
        ///     Is device smart tv
        /// </summary>
        public bool Is_smarttv { get; set; }

        /// <summary>
        ///     Is device ux full desktop
        /// </summary>
        public bool Isux_full_desktop { get; set; }
    }
}