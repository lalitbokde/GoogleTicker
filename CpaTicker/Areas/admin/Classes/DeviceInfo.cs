using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class DeviceInfo
    {
        public long Id { get; set; }

        public string DeviceId { get; set; }
        public bool IsSmartphone { get; set; }
        public bool IsiOS { get; set; }
        public bool IsAndroid { get; set; }

        public string OS { get; set; }
        public string Browser { get; set; }
        public string Device_os { get; set; }
        public string Pointing_method { get; set; }
        public bool Is_tablet { get; set; }
        public string Model_name { get; set; }
        public string Device_os_version { get; set; }
        public bool Is_wireless_device { get; set; }
        public string Brand_name { get; set; }
        public string Marketing_name { get; set; }
        public bool Is_assign_phone_number { get; set; }
        public string Xhtmlmp_mime_type { get; set; }
        public string Xhtml_support_level { get; set; }
        public string Resolution_height { get; set; }
        public string Resolution_width { get; set; }
        public string Canvas_support { get; set; }
        public string Viewport_width { get; set; }
        public string Html_preferred_dtd { get; set; }
        public bool Isviewport_supported { get; set; }
        public bool Ismobileoptimized { get; set; }

        public bool Isimage_inlining { get; set; }
        public bool Ishandheldfriendly { get; set; }
        public bool Is_smarttv { get; set; }
        public bool Isux_full_desktop { get; set; }      
    }
}