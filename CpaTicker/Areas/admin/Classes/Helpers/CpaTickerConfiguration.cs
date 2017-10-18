using System;
using System.Collections.Generic;
using System.Configuration;

namespace CpaTicker.Areas.admin.Classes.Helpers
{
    public static class CpaTickerConfiguration
    {
        // Returns the default domain
        public static string DefaultDomainName
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultDomainName"];
            }
        }

        public static string BackgroundColor
        {
            get
            {
                return ConfigurationManager.AppSettings["Background"];
            }
        }
        public static string CampaignColor
        {
            get
            {
                return ConfigurationManager.AppSettings["Campaign"];
            }
        }
        public static string ClickColor
        {
            get
            {
                return ConfigurationManager.AppSettings["Click"];
            }
        }
        public static string ImpressionColor
        {
            get
            {
                return ConfigurationManager.AppSettings["Impression"];
            }
        }
        public static string ConversionColor
        {
            get
            {
                return ConfigurationManager.AppSettings["Conversion"];
            }
        }
        public static string CostColor
        {
            get
            {
                return ConfigurationManager.AppSettings["Cost"];
            }
        }
        public static string RevenueColor
        {
            get
            {
                return ConfigurationManager.AppSettings["Revenue"];
            }
        }

        public static int DefaultDomainId
        {
            get
            {
                return 0;
            }
        }

        //public static ViewPermission DefaultAffiliateRestrictions
        //{
        //    get
        //    {
        //        return ViewPermission.editCampaign |
        //               ViewPermission.addcustomerdomainSettings |
        //               ViewPermission.detailsBanner |
        //               ViewPermission.deleteBanner |
        //               ViewPermission.editSettings |
        //               ViewPermission.addcustomeruserSettings |
        //               ViewPermission.resetpwdSettings |
        //               ViewPermission.removecustomerdomainSettings |
        //               ViewPermission.removecustomeruserSettings |
        //               ViewPermission.setpermissionsSettings;
        //    }
        //}

        public static long DefaultAffiliateDynamicRestrictions
        {
            get
            {
                List<long> list = new List<long>();
                Type enumtype = CPAHelper.GetEnumType();
                /*
                 * put the name of the action fallow by the name of the controller
                 * */
                System.Reflection.FieldInfo enumItem = enumtype.GetField("editCampaign");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("addcustomerdomainSettings");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("createBanner");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("removeBanner");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("editBanner");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("editSettings");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("adduserSettings");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("resetpwdSettings");
                list.Add((long)enumItem.GetValue(enumtype));

                //if this method no longer belongs to the Settings Controller better remove it
                //enumItem = enumtype.GetField("removecustomerdomainSettings");
                //list.Add((long)enumItem.GetValue(enumtype));

                //enumItem = enumtype.GetField("removecustomeruserSettings");
                //list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("setpermissionsSettings");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("usersAffiliate");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("adduserAffiliate");
                list.Add((long)enumItem.GetValue(enumtype));

                // added by jignesh

                enumItem = enumtype.GetField("createRedirect");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("editRedirect");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("createURL");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("editURL");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("editstatusConversionPixel");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("createAffiliate");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("editAffiliate");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("adjuststatsAffiliate");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("adjustAffiliate");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("createCampaign");
                list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("createAction");
                if (enumItem != null)
                    list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("editAction");
                if (enumItem != null)
                    list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("usersAffiliate");
                if (enumItem != null)
                    list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("adduserAffiliate");
                if (enumItem != null)
                    list.Add((long)enumItem.GetValue(enumtype));

                enumItem = enumtype.GetField("editCampaign");
                if (enumItem != null)
                    list.Add((long)enumItem.GetValue(enumtype));

                long result = 0;
                foreach (var item in list)
                    result = result | item;

                return result;

            }
        }


        public static long DefaultAffiliateDynamicRestrictionsCopy(string enumtypeName)
        {

            List<long> list = new List<long>();
            Type enumtype = CPAHelper.GetEnumType(enumtypeName);
            /*
             * put the name of the action fallow by the name of the controller
             * */
            System.Reflection.FieldInfo enumItem = enumtype.GetField("editCampaign");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("addcustomerdomainSettings");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("createBanner");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("removeBanner");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editBanner");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editSettings");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("adduserSettings");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("resetpwdSettings");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            //if this method no longer belongs to the Settings Controller better remove it
            //enumItem = enumtype.GetField("removecustomerdomainSettings");
            //list.Add((long)enumItem.GetValue(enumtype));

            //enumItem = enumtype.GetField("removecustomeruserSettings");
            //list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("setpermissionsSettings");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("usersAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("adduserAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            // added by jignesh

            enumItem = enumtype.GetField("createRedirect");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editRedirect");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("createURL");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editURL");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editstatusConversionPixel");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("createAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("adjuststatsAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("adjustAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("createCampaign");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("createAction");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editAction");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("usersAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("adduserAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editCampaign");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            //  newly added after  29th oct. 2015

            enumItem = enumtype.GetField("createoverridepayoutCampaign");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("addoverridepayoutCampaign");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editoverridepayoutCampaign");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("createoverridepayoutAction");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("addoverridepayoutAction");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editoverridepayoutAction");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("updateoverridepayoutAction");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            //enumItem = enumtype.GetField("createaffiliatepayoutCampaign");
            //if (enumItem != null)
            //    list.Add((long)enumItem.GetValue(enumtype));

            //enumItem = enumtype.GetField("editaffiliatepayoutCampaign");
            //if (enumItem != null)
            //    list.Add((long)enumItem.GetValue(enumtype));

            //enumItem = enumtype.GetField("addaffiliateoverrideCampaign");
            //if (enumItem != null)
            //    list.Add((long)enumItem.GetValue(enumtype));

            //enumItem = enumtype.GetField("updateaffiliateoverrideCampaign");
            //if (enumItem != null)
            //    list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("admin_master_loginSettings");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("createPAGE");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editPAGE");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("createoverridepayoutAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("editoverridepayoutAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("addaffiliateoverrideAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));

            enumItem = enumtype.GetField("updateaffiliateoverrideAffiliate");
            if (enumItem != null)
                list.Add((long)enumItem.GetValue(enumtype));























            long result = 0;
            foreach (var item in list)
                result = result | item;

            return result;


        }
        public static bool CreateDynamicAssembly
        {
            get
            {
                if (ConfigurationManager.AppSettings["CreateDynamicAssembly"] != null)
                    return bool.Parse(ConfigurationManager.AppSettings["CreateDynamicAssembly"]);
                return false;
            }
        }

        /*================================LimeLight=================================================*/

        public static int LimeLightProductId(Level customerlevel)
        {
            if (customerlevel == Level.Gold)
            {
                return 1591;
            }
            if (customerlevel == Level.Platinum)
            {
                return 1593;
            }
            return 1595;
        }

        public static int LimeLightCampaignId
        {
            get
            {
                return 327;
            }
        }

        public static int LimeLightShippingId
        {
            get
            {
                return 4;
            }
        }

        public static string LimeLightUsername
        {
            get
            {
                return "cpaticker";
            }
        }

        public static string LimeLightPassword
        {
            get
            {
                return "zWga8NXfhAbrux";
            }
        }

        public static bool LimeLightTesting
        {
            get
            {
                if (ConfigurationManager.AppSettings["LimeLightTesting"] != null)
                    return bool.Parse(ConfigurationManager.AppSettings["LimeLightTesting"]);
                return false;
            }
        }

        public static int LimeLightAPIKeyProductId
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["APIKeyProductId"]);
            }
        }

        /*================================LimeLight======================================================*/

        public static bool URLDefaultRedirect
        {
            get
            {
                if (ConfigurationManager.AppSettings["URLDefaultRedirect"] != null)
                    return bool.Parse(ConfigurationManager.AppSettings["URLDefaultRedirect"]);
                return true;
            }
        }

        public static bool DefaultRedirect
        {
            get
            {
                if (ConfigurationManager.AppSettings["DefaultRedirect"] != null)
                    return bool.Parse(ConfigurationManager.AppSettings["DefaultRedirect"]);
                return true;
            }
        }

        public static string MailServer
        {
            get
            {
                return ConfigurationManager.AppSettings["MailServer"];
            }
        }

        public static int MailPort
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MailPort"]);
            }
        }

        public static string MailUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["MailUsername"];
            }
        }

        public static string MailPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["MailPassword"];
            }
        }

        /// <summary>
        /// support@clickticker.com
        /// </summary>
        public static string MailTo
        {
            get
            {
                return ConfigurationManager.AppSettings["MailTo"];
            }
        }

        public static bool MailEnableSsl
        {
            get
            {
                return ConfigurationManager.AppSettings["MailEnableSsl"].ToUpper() == "YES";
            }
        }



    }
}