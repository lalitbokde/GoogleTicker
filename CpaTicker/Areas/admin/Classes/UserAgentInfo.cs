using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes
{
    public class UserAgentInfo : UserAgentInfoBase
    {
        public UserAgentInfo()
        {
            Capabilities = new Dictionary<String, String>();
            WorkingMode = String.Empty;
            ApiVersion = String.Empty;
            DataLastUpdated = String.Empty;
            DataVersion = String.Empty;
            DeviceId = String.Empty;
            DeviceRootId = String.Empty;
            StartupTime = String.Empty;
            Matcher = String.Empty;
            MatchingMethod = String.Empty;
            RequestDuration = String.Empty;
            NormalizedUserAgent = String.Empty;
            FiltersForDisplay = String.Empty;
            VirtualIsSmartphone = "N/A";
            VirtualIsApple = "N/A";
            VirtualIsAndroid = "N/A";
            VirtualIsNative = "N/A";
            VirtualBrowser = "N/A";
            VirtualOs = "N/A";
            Cargo = "";
        }

        public String UserAgent { get; set; }
        public String NormalizedUserAgent { get; set; }
        public String WorkingMode { get; set; }

        public String RequestDuration { get; set; }
        public String DeviceId { get; set; }
        public String DeviceRootId { get; set; }
        public String Matcher { get; set; }
        public String MatchingMethod { get; set; }
        public IDictionary<String, String> Capabilities { get; set; }

        public String DataVersion { get; set; }
        public String DataLastUpdated { get; set; }
        public String ApiVersion { get; set; }
        public String StartupTime { get; set; }
        public String FiltersForDisplay { get; set; }
        public Int32 TotalDevices { get; set; }

        public String VirtualIsNative { get; set; }
        public String VirtualIsSmartphone { get; set; }
        public String VirtualBrowser { get; set; }
        public String VirtualOs { get; set; }
        public String VirtualIsApple { get; set; }
        public String VirtualIsAndroid { get; set; }

        public String Cargo { get; set; }

        public String WurflHeaders { get; set; }
    }
}