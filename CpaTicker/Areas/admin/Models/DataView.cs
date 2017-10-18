using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Models
{

    public class Interval
    {

        public bool Date { get; set; }
        public bool Year { get; set; }
        public bool Month { get; set; }
        public bool Day { get; set; }
        public bool Hour { get; set; }


        public static Interval Create(int interval)
        {
            Interval result = new Interval();
            result.Date = interval % 2 == 1;
            interval = interval / 2;
            result.Year = interval % 2 == 1;
            interval = interval / 2;
            result.Month = interval % 2 == 1;
            interval = interval / 2;
            result.Day = interval % 2 == 1;
            interval = interval / 2;
            result.Hour = interval % 2 == 1;
            return result;
        }
    }

    [Flags]
    public enum IntervalEnum
    {
        Date = 0x1,
        Year = 0x2,
        Month = 0x4,
        Day = 0x8,
        Hour = 0x10,
        CR = 0x20,
        CPC = 0x40,
        RPC = 0x80,
    }

    [Flags]
    public enum StatisticsEnum : long
    {
        Impressions = 0x1,
        Clicks = 0x2,
        Conversions = 0x4,
        Cost = 0x8,
        Revenue = 0x10,
        Profit = 0x20,

        CR = 0x40,
        CPC = 0x80,
        RPC = 0x100,
        Country = 0x200,

        NetPayout = 0x400,
        ApprovedPercent = 0x800,
        NetRevenue = 0x1000,


        // for the ConversionReport
        TransactionID = 0x2000,
        IP = 0x4000,
        UserAgent = 0x8000,
        SubId = 0x10000,
        Status = 0x20000,
        Date = 0x40000,
        Campaign = 0x80000,
        Affiliate = 0x100000,
        ConversionType = 0x200000,
        URL = 0x400000,
        URLId = 0x800000,
        Source = 0x1000000,
        StatusDescription = 0x2000000,

        ActionName = 0x4000000,
        Pixel = 0x8000000,
        Postback = 0x10000000,

        GrossConversions = 0x20000000,


        Year = 0x40000000,
        Month = 0x80000000,
        Day = 0x1000000000,
        Hour = 0x200000000,



        RejectedConversions = 0x400000000,
        ApprovedConversions = 0x800000000,
        RejectedPercent = 0x1000000000,
        Banner = 0x2000000000,
        ParentURL = 0x4000000000,
        CTR = 0x8000000000,


        //  for Device Info details
        DeviceId = 0x10000000000,
        IsSmartphone = 0x20000000000,
        IsiOS = 0x40000000000,
        IsAndroid = 0x80000000000,

        OS = 0x100000000000,
        Browser = 0x200000000000,
        Device_os = 0x400000000000,
        Pointing_method = 0x800000000000,

        Is_tablet = 0x1000000000000,
        Model_name = 0x2000000000000,
        Device_os_version = 0x4000000000000,
        Is_wireless_device = 0x8000000000000,

        Brand_name = 0x10000000000000,
        Marketing_name = 0x20000000000000,

        // 
        //Is_assign_phone_number
        // Xhtmlmp_mime_type
        // Xhtml_support_level
        //

        Resolution_height = 0x40000000000000,
        Resolution_width = 0x80000000000000,

        Canvas_support = 0x100000000000000,
        Viewport_width = 0x200000000000000,

        //
        //Html_preferred_dtd
        //
        Isviewport_supported = 0x400000000000000,
        Ismobileoptimized = 0x800000000000000,

        //Isimage_inlining

        Ishandheldfriendly = 0x1000000000000000,
        Is_smarttv = 0x2000000000000000,
        Isux_full_desktop = 0x4000000000000000,


    }

    [Flags]
    public enum CustomStatisticsEnum : long
    {
        Impressions = 0x1,
        Clicks = 0x2,
        Conversions = 0x4,
        Cost = 0x8,
        Revenue = 0x10,
        Profit = 0x20,

        CR = 0x40,
        CPC = 0x80,
        RPC = 0x100,
        Country = 0x200,

        ClickDate = 0x400,
        ConversionDate = 0x800,
        ImpressionDate = 0x1000,


        // for the ConversionReport
        TransactionID = 0x2000,
        IP = 0x4000,
        UserAgent = 0x8000,
        SubId = 0x10000,
        Status = 0x20000,
        Date = 0x40000,
        Campaign = 0x80000,
        Affiliate = 0x100000,
        ConversionType = 0x200000,
        URL = 0x400000,
        URLId = 0x800000,
        Source = 0x1000000,
        StatusDescription = 0x2000000,

        ActionName = 0x4000000,
        Pixel = 0x8000000,
        Postback = 0x10000000,

        Referrer = 0x20000000,


        PAGE = 0x40000000,
        Redirect = 0x80000000,
        Day = 0x1000000000,
        Hour = 0x200000000,



        RejectedConversions = 0x400000000,
        ApprovedConversions = 0x800000000,
        RejectedPercent = 0x1000000000,
        Banner = 0x2000000000,
        ParentURL = 0x4000000000,
        CTR = 0x8000000000,


        //  for Device Info details
        DeviceId = 0x10000000000,
        IsSmartphone = 0x20000000000,
        IsiOS = 0x40000000000,
        IsAndroid = 0x80000000000,

        OS = 0x100000000000,
        Browser = 0x200000000000,
        Device_os = 0x400000000000,
        Pointing_method = 0x800000000000,

        Is_tablet = 0x1000000000000,
        Model_name = 0x2000000000000,
        Device_os_version = 0x4000000000000,
        Is_wireless_device = 0x8000000000000,

        Brand_name = 0x10000000000000,
        Marketing_name = 0x20000000000000,

        // 
        //Is_assign_phone_number
        // Xhtmlmp_mime_type
        // Xhtml_support_level
        //

        Resolution_height = 0x40000000000000,
        Resolution_width = 0x80000000000000,

        Canvas_support = 0x100000000000000,
        Viewport_width = 0x200000000000000,

        //
        //Html_preferred_dtd
        //
        Isviewport_supported = 0x400000000000000,
        Ismobileoptimized = 0x800000000000000,

        //Isimage_inlining

        Ishandheldfriendly = 0x1000000000000000,
        Is_smarttv = 0x2000000000000000,
        Isux_full_desktop = 0x4000000000000000,




    }
    public class Statistics
    {
        public bool Impressions { get; set; } // 1
        public bool Clicks { get; set; } //2
        public bool Conversions { get; set; } // 4
        public bool Cost { get; set; } // 8
        public bool Revenue { get; set; } // 16
        public bool Proffit { get; set; } // 32
        public bool GrossConversions { get; set; } // 64
        public bool RejectedConversions { get; set; } // 128
        public bool ApprovedConversions { get; set; } // 256
        public bool RejectedPercent { get; set; } // 512
        public bool NetPayout { get; set; } // 1024
        public bool ApprovedPercent { get; set; } // 2048 
        public bool NetRevenue { get; set; } // 4096  2^12

        // for the ConversionReport
        public bool TransactionID { get; set; } //2^13 0x2000
        public bool IP { get; set; } //2^14 0x4000
        public bool UserAgent { get; set; } //2^15 0x8000
        public bool SubId { get; set; }//2^16 0x10000
        public bool Status { get; set; }//2^17 0x20000
        public bool Date { get; set; }//2^18 0x40000
        public bool Campaign { get; set; }//2^19 0x80000
        public bool Affiliate { get; set; }//2^20 0x100000
        public bool ConversionType { get; set; }//2^21 0x200000
        public bool URL { get; set; }//2^22 0x400000
        public bool URLId { get; set; }//2^23 0x800000
        public bool Source { get; set; } // 2^24 0x1000000
        public bool StatusDescription { get; set; } // 2^25 0x2000000

        public bool ActionName { get; set; } // 2^26 0x4000000
        public bool Pixel { get; set; } // 2^27 0x8000000
        public bool Postback { get; set; } // 2^28 0x10000000

        public bool Country { get; set; } // 2^29 0x20000000

        public static Statistics Create(int Statistics)
        {
            Statistics result = new Statistics();
            result.Impressions = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.Clicks = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.Conversions = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.Cost = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.Revenue = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.Proffit = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.GrossConversions = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.RejectedConversions = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.ApprovedConversions = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.RejectedPercent = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.NetPayout = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.ApprovedPercent = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.NetRevenue = Statistics % 2 == 1;


            Statistics = Statistics / 2;
            result.TransactionID = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.IP = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.UserAgent = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.SubId = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.Status = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.Date = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.Campaign = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.Affiliate = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.ConversionType = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.URL = Statistics % 2 == 1;
            Statistics = Statistics / 2;
            result.URLId = Statistics % 2 == 1;

            Statistics = Statistics / 2;
            result.Source = Statistics % 2 == 1;

            Statistics = Statistics / 2;
            result.StatusDescription = Statistics % 2 == 1;

            Statistics = Statistics / 2;
            result.ActionName = Statistics % 2 == 1;

            Statistics = Statistics / 2;
            result.Pixel = Statistics % 2 == 1;

            Statistics = Statistics / 2;
            result.Postback = Statistics % 2 == 1;

            Statistics = Statistics / 2;
            result.Country = Statistics % 2 == 1;

            return result;
        }


    }

    public class Filter
    {

        public bool Affiliate { get; set; }
        public bool Campaign { get; set; }
        public bool Contries { get; set; }


        public static Filter Create(int interval)
        {
            Filter result = new Filter();
            result.Affiliate = interval % 2 == 1;
            interval = interval / 2;
            result.Campaign = interval % 2 == 1;
            interval = interval / 2;
            result.Contries = interval % 2 == 1;

            return result;
        }
    }


    public class Calculation
    {

        public bool CR { get; set; }
        public bool CPC { get; set; }
        public bool RPC { get; set; }


        public static Calculation Create(int calc)
        {
            Calculation result = new Calculation();
            result.CR = calc % 2 == 1;
            calc = calc / 2;
            result.CPC = calc % 2 == 1;
            calc = calc / 2;
            result.RPC = calc % 2 == 1;

            return result;
        }
    }
}