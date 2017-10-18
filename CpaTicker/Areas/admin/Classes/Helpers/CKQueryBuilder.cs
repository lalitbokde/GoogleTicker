using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CpaTicker.Areas.admin.Classes.Helpers
{
    public static class CKQueryBuilder
    {
        public static string ClickQuery(int customerid, int? userid = null, bool activecampaigns = false,
            bool gby_campaign = false, bool gby_affiliate = false, bool gby_source = false, int[] gby_subid = null,
            bool gby_url = false, bool gby_country = false,
            int? fcampaign = null, int[] faffiliateids = null, string fcountrycode = null)
        {
            var sb = new StringBuilder();

            var select = new StringBuilder();
            var groupby = new StringBuilder();

            if (gby_campaign)
            {
                select.Append(",Clicks.CampaignId");
                groupby.Append(",Clicks.CampaignId");
            }

            if (gby_source)
            {
                select.Append(",ISNULL(Clicks.Source, '') as Source");
                groupby.Append(",ISNULL(Clicks.Source, '')");
            }

            if (gby_affiliate)
            {
                select.Append(",Clicks.AffiliateId");
                groupby.Append(",Clicks.AffiliateId");
            }

            if (gby_url)
            {
                select.Append(",URLPreviewId");
                groupby.Append(",URLPreviewId");
            }

            if (gby_country)
            {
                select.Append(",ISNULL(Country, '') as Country");
                groupby.Append(",ISNULL(Country, '')");
            }

            var subidquery = new StringBuilder();
            if (gby_subid != null)
            {
                subidquery.Append("select distinct cs.ClickId ");
                foreach (var item in gby_subid)
                {
                    subidquery.AppendFormat(",(select SubValue from ClickSubIds where SubIndex = {0} and ClickSubIds.ClickId = cs.ClickId) as 'SubId{0}' ", item);

                    select.AppendFormat(", ISNULL(csu.SubId{0}, '') as SubId{0} ", item);
                    groupby.AppendFormat(", ISNULL(csu.SubId{0}, '') ", item);
                }
                subidquery.Append("from ClickSubIds as cs ");
            }

            // removing the first comma and insert the group by statement
            groupby.Remove(0, 1);
            groupby.Insert(0, "group by ");

            sb.AppendFormat("select count(Clicks.TransactionId) as Clicks, sum(Cost) as Cost, sum(Clicks.Revenue) as Revenue {0} ", select);
            sb.Append("from Clicks ");
            sb.Append(gby_subid == null ? string.Empty : string.Format("left join ({0}) as csu on Clicks.ClickId = csu.ClickId ", subidquery));
            sb.AppendFormat("where CustomerId = {0} ", customerid);
            sb.Append("and ClickDate between @fromdate and @todate and Clicks.bot = 0 ");
            sb.Append(fcampaign == null ? string.Empty : string.Format("and CampaignId={0} ", fcampaign));
            sb.Append(activecampaigns ? string.Format("and CampaignId in (select CampaignId from Campaigns where CustomerId = {0} and Status = 1) ", customerid) : string.Empty);
            sb.Append(userid.HasValue ? string.Format("and CampaignId not in (select ca.CampaignId from UserHiddenCampaigns h inner join Campaigns ca on ca.Id = h.CampaignId where h.UserId = {0}) ", userid) : string.Empty);
            sb.Append(fcountrycode == null ? string.Empty : string.Format("and Clicks.Country = '{0}' ", fcountrycode));

            if (faffiliateids != null)
            {
                var sbaux = new StringBuilder();
                foreach (var item in faffiliateids)
                {
                    sbaux.AppendFormat("OR AffiliateId = {0} ", item);
                }

                sbaux.Remove(0, 2);
                sb.AppendFormat("and ({0}) ", sbaux);
            }

            sb.Append(groupby.ToString());

            return sb.ToString();
        }


        public static string ClickCampaignQuery(int customerid, int? userid = null, bool activecampaigns = false,
            bool gby_campaign = false, bool gby_affiliate = false, bool gby_source = false, int[] gby_subid = null,
            bool gby_url = false, bool gby_country = false,
            int? fcampaign = null, int[] faffiliateids = null, string fcountrycode = null)
        {
            var sb = new StringBuilder();

            var select = new StringBuilder();
            var clickinnerselect = new StringBuilder();
            var groupby = new StringBuilder();

            clickinnerselect.Append(" from ( select Clicks.transactionid as transactionid ,sum(Clicks.Cost) AS Cost  ,sum(Clicks.Revenue) AS Revenue");
            if (gby_campaign)
            {
                select.Append(",Clicks.CampaignId");
                groupby.Append(",Clicks.CampaignId");
                clickinnerselect.Append(",MIN(Clicks.CampaignId) as CampaignId");
            }

            if (gby_source)
            {
                select.Append(",ISNULL(Clicks.Source, '') as Source");
                groupby.Append(",ISNULL(Clicks.Source, '')");
                clickinnerselect.Append(",MIN(Clicks.Source) as Source");
            }

            if (gby_affiliate)
            {
                select.Append(",Clicks.AffiliateId");
                groupby.Append(",Clicks.AffiliateId");
                clickinnerselect.Append(",MIN(Clicks.AffiliateId) as AffiliateId");
            }

            if (gby_url)
            {
                select.Append(",URLPreviewId");
                groupby.Append(",URLPreviewId");
                clickinnerselect.Append(",MIN(Clicks.URLPreviewId) as URLPreviewId");
            }

            if (gby_country)
            {
                select.Append(",ISNULL(Country, '') as Country");
                groupby.Append(",ISNULL(Country, '')");
                clickinnerselect.Append(",MIN(Clicks.Country) as Country");
            }

            var subidquery = new StringBuilder();
            if (gby_subid != null)
            {
                subidquery.Append("select distinct cs.ClickId ");
                foreach (var item in gby_subid)
                {
                    subidquery.AppendFormat(",(select SubValue from ClickSubIds where SubIndex = {0} and ClickSubIds.ClickId = cs.ClickId) as 'SubId{0}' ", item);

                    select.AppendFormat(", ISNULL(clicks.SubId{0}, '') as SubId{0} ", item);
                    clickinnerselect.AppendFormat(",MIN(csu.SubId{0}) as SubId{0}", item);
                    groupby.AppendFormat(", Clicks.SubId{0} ", item);
                }
                subidquery.Append("from ClickSubIds as cs ");
            }

            // removing the first comma and insert the group by statement
            groupby.Remove(0, 1);
            groupby.Insert(0, "group by ");

            sb.AppendFormat("select count(Clicks.TransactionId) as Clicks, sum(Clicks.Cost) as Cost, sum(Clicks.Revenue) as Revenue {0} {1} ", select, clickinnerselect);
            sb.Append("from Clicks ");
            sb.Append(gby_subid == null ? string.Empty : string.Format("left join ({0}) as csu on Clicks.ClickId = csu.ClickId ", subidquery));
            sb.AppendFormat("where CustomerId = {0} ", customerid);
            sb.Append("and ClickDate between @fromdate and @todate and Clicks.bot = 0 ");
            sb.Append(fcampaign == null ? string.Empty : string.Format("and CampaignId={0} ", fcampaign));
            sb.Append(activecampaigns ? string.Format("and CampaignId in (select CampaignId from Campaigns where CustomerId = {0} and Status = 1) ", customerid) : string.Empty);
            sb.Append(userid.HasValue ? string.Format("and CampaignId not in (select ca.CampaignId from UserHiddenCampaigns h inner join Campaigns ca on ca.Id = h.CampaignId where h.UserId = {0}) ", userid) : string.Empty);
            sb.Append(fcountrycode == null ? string.Empty : string.Format("and Clicks.Country = '{0}' ", fcountrycode));

            if (faffiliateids != null)
            {
                var sbaux = new StringBuilder();
                foreach (var item in faffiliateids)
                {
                    sbaux.AppendFormat("OR AffiliateId = {0} ", item);
                }

                sbaux.Remove(0, 2);
                sb.AppendFormat("and ({0}) ", sbaux);
            }
            sb.Append("GROUP BY convert(DATE, DATEADD(hh, -4, Clicks.ClickDate)) ,clicks.transactionid ) as clicks  ");
            sb.Append(groupby.ToString());

            return sb.ToString();
        }

        public static string ClickQueryTest(int customerid, int? userid = null, int offsethour = 0, bool activecampaigns = false,
            bool gby_campaign = false, bool gby_affiliate = false, bool gby_source = false, int[] gby_subid = null,
            bool gby_url = false, bool gby_country = false,
            int? fcampaign = null, int[] faffiliateids = null, string fcountrycode = null)
        {
            var sb = new StringBuilder();

            var select = new StringBuilder();
            var groupby = new StringBuilder();

            //Date based groupby
            groupby.Append(",convert(DATE, DATEADD(hh, " + offsethour + ", Clicks.ClickDate))");

            if (gby_campaign)
            {
                select.Append(",Clicks.CampaignId");
                groupby.Append(",Clicks.CampaignId");
            }

            if (gby_source)
            {
                select.Append(",ISNULL(Clicks.Source, '') as Source");
                groupby.Append(",ISNULL(Clicks.Source, '')");
            }

            if (gby_affiliate)
            {
                select.Append(", Min( clicks.affiliateid ) as AffiliateId");
                //select.Append(",Clicks.AffiliateId");
                groupby.Append(",clicks.transactionid");
            }

            if (gby_url)
            {
                select.Append(",URLPreviewId");
                groupby.Append(",URLPreviewId");
            }

            if (gby_country)
            {
                select.Append(",ISNULL(Country, '') as Country");
                groupby.Append(",ISNULL(Country, '')");
            }
            if (gby_affiliate)
            {
                groupby.Append(") as Test GROUP By  Test.affiliateid");
            }
            if (gby_campaign)
            {
                groupby.Append(",Test.CampaignId");
            }
            if (gby_source)
            {
                groupby.Append(",Test.Source");
            }
            if (gby_country)
            {
                groupby.Append(",Test.Country");
            }
            var subidquery = new StringBuilder();
            if (gby_subid != null)
            {
                subidquery.Append("select distinct cs.ClickId ");
                foreach (var item in gby_subid)
                {
                    subidquery.AppendFormat(",(select SubValue from ClickSubIds where SubIndex = {0} and ClickSubIds.ClickId = cs.ClickId) as 'SubId{0}' ", item);

                    select.AppendFormat(", Min( csu.subid{0} ) as SubId{0}", item);
                    groupby.AppendFormat(", Test.subid{0}", item);

                    //select.AppendFormat(", ISNULL(csu.SubId{0}, '') as SubId{0} ", item);
                    //groupby.AppendFormat(", ISNULL(csu.SubId{0}, '') ", item);
                }
                subidquery.Append("from ClickSubIds as cs ");
            }

            // removing the first comma and insert the group by statement
            groupby.Remove(0, 1);
            groupby.Insert(0, "group by ");


            sb.AppendFormat("SELECT Count(Test.Clicks) AS Clicks , Sum(Test.Cost) AS Cost , Sum(Test.Revenue) AS Revenue, Test.affiliateid");
            if (gby_campaign) { sb.Append(",Test.CampaignId"); }
            if (gby_source) { sb.Append(",Test.Source"); }
            if (gby_country) { sb.Append(",Test.Country"); }
            if (gby_subid != null)
            {
                foreach (var item in gby_subid)
                {
                    sb.AppendFormat(",Test.SubId{0}", item);
                }

            }
            sb.Append(" FROM (");

            sb.AppendFormat("select  Clicks.TransactionId as Clicks, sum(Cost) as Cost, sum(Clicks.Revenue) as Revenue {0} ", select);
            sb.Append("from Clicks ");
            sb.Append(gby_subid == null ? string.Empty : string.Format("left join ({0}) as csu on Clicks.ClickId = csu.ClickId ", subidquery));
            sb.AppendFormat("where CustomerId = {0} ", customerid);
            sb.Append("and ClickDate between @fromdate and @todate and Clicks.bot = 0 ");
            sb.Append(fcampaign == null ? string.Empty : string.Format("and CampaignId={0} ", fcampaign));
            sb.Append(activecampaigns ? string.Format("and CampaignId in (select CampaignId from Campaigns where CustomerId = {0} and Status = 1) ", customerid) : string.Empty);
            sb.Append(userid.HasValue ? string.Format("and CampaignId not in (select ca.CampaignId from UserHiddenCampaigns h inner join Campaigns ca on ca.Id = h.CampaignId where h.UserId = {0}) ", userid) : string.Empty);
            sb.Append(fcountrycode == null ? string.Empty : string.Format("and Clicks.Country = '{0}' ", fcountrycode));

            if (faffiliateids != null)
            {
                var sbaux = new StringBuilder();
                foreach (var item in faffiliateids)
                {
                    sbaux.AppendFormat("OR AffiliateId = {0} ", item);
                }

                sbaux.Remove(0, 2);
                sb.AppendFormat("and ({0}) ", sbaux);
            }

            sb.Append(groupby.ToString());

            return sb.ToString();
        }

        public static string AffiliateReportQuery(int customerid, int userid,
            bool by_campaign = false, bool by_source = false, int[] by_subid = null, bool by_country = false,
            int? fcampaignid = null, int[] faffiliateids = null, string fcountrycode = null)
        {

            var selectfunction = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                //var _name = string.IsNullOrEmpty(tname) ? string.Empty : tname + ".";

                aux.Append(by_campaign ? string.Format(", {0}CampaignId ", tname) : string.Empty);
                aux.Append(by_source ? string.Format(", {0}Source ", tname) : string.Empty);
                aux.Append(by_country ? string.Format(", {0}Country ", tname) : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat(", {1}SubId{0} ", item, tname);

                return aux.ToString();
            });

            var function = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                aux.Append(by_campaign ? string.Format("and t.CampaignId = {0}.CampaignId ", tname) : string.Empty);
                aux.Append(by_source ? string.Format("and t.Source = {0}.Source ", tname) : string.Empty);
                aux.Append(by_country ? string.Format("and t.Country = {0}.Country ", tname) : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat("and t.SubId{0} = {1}.SubId{0} ", item, tname);

                return aux.ToString();
            });

            var sb = new StringBuilder();

            sb.AppendFormat(
                "SET NOCOUNT ON; " +
                "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " +
                "WITH tunion (AffiliateId {0}) AS ( " +
                "select AffiliateId {0} from ({1}) as c " +
                "union " +
                "select AffiliateId {0} from ({2}) as co " +
                "union " +
                "select AffiliateId {0} from ({3}) as i ) " +

                "select t.AffiliateId, af.Company,  " +
                "ISNULL(c.Clicks, 0) as Clicks,  " +
                "ISNULL(co.Conversions, 0) as Conversions, " +
                "ISNULL(i.Impressions, 0) as Impressions, " +
                "ISNULL(c.Revenue, 0) + ISNULL(co.Revenue, 0) + ISNULL(i.Revenue, 0) as Revenue, " +
                "ISNULL(c.Cost, 0) + ISNULL(co.Cost, 0) + ISNULL(i.Cost, 0) as Cost {7} {6}" +

                "from tunion as t {5} " +
                "inner join (select AffiliateId, Company from Affiliates where CustomerId = {4}) as af on t.AffiliateId = af.AffiliateId " +

                "left join ({1}) as c on t.AffiliateId = c.AffiliateId " + function("c") +
                "left join ({2}) as co on t.AffiliateId = co.AffiliateId " + function("co") +
                "left join ({3}) as i on t.AffiliateId = i.AffiliateId " + function("i") +

                "order by t.AffiliateId {6} " +
                "OPTION (MAXRECURSION 0)"

                    , selectfunction(null)
                    , ClickQuery(customerid, userid, true,
                        by_campaign, true, by_source, by_subid, false, by_country, fcampaignid, faffiliateids, fcountrycode)

                    , ConversionQuery(customerid, userid, true,
                        by_campaign, true, by_source, by_subid, false, by_country, fcampaignid, faffiliateids, fcountrycode)

                    , ImpressionQuery(customerid, userid, true,
                        by_campaign, true, by_source, by_subid, false, by_country, fcampaignid, faffiliateids, fcountrycode)

                    , customerid
                    , by_campaign ? string.Format("inner join (select CampaignName, CampaignId from Campaigns where CustomerId = {0}) as ca on t.CampaignId = ca.CampaignId ", customerid) : string.Empty
                    , selectfunction("t.")
                    , by_campaign ? ", ca.CampaignName " : string.Empty
                    );

            return sb.ToString();
        }





        /* Edited by dharmesh to make execution faster*/

        public static string ClickQueryAffiliate(int customerid, int? userid = null, bool activecampaigns = false,
           bool gby_campaign = false, bool gby_affiliate = false, bool gby_source = false, int[] gby_subid = null,
           bool gby_url = false, bool gby_country = false,
           int? fcampaign = null, int[] faffiliateids = null, string fcountrycode = null)
        {
            var sb = new StringBuilder();

            var select = new StringBuilder();
            var groupby = new StringBuilder();

            if (gby_campaign)
            {
                select.Append(",Clicks.CampaignId");
                groupby.Append(",Clicks.CampaignId");
            }

            if (gby_source)
            {
                select.Append(",ISNULL(Clicks.Source, '') as Source");
                groupby.Append(",ISNULL(Clicks.Source, '')");
            }

            if (gby_affiliate)
            {
                select.Append(",Clicks.AffiliateId");
                groupby.Append(",Clicks.AffiliateId");
            }

            if (gby_url)
            {
                select.Append(",URLPreviewId");
                groupby.Append(",URLPreviewId");
            }

            if (gby_country)
            {
                select.Append(",ISNULL(Country, '') as Country");
                groupby.Append(",ISNULL(Country, '')");
            }

            var subidquery = new StringBuilder();
            if (gby_subid != null)
            {
                subidquery.Append("select distinct cs.ClickId ");
                foreach (var item in gby_subid)
                {
                    subidquery.AppendFormat(",(select SubValue from ClickSubIds where SubIndex = {0} and ClickSubIds.ClickId = cs.ClickId) as 'SubId{0}' ", item);

                    select.AppendFormat(", ISNULL(csu.SubId{0}, '') as SubId{0} ", item);
                    groupby.AppendFormat(", ISNULL(csu.SubId{0}, '') ", item);
                }
                subidquery.Append("from ClickSubIds as cs ");
            }

            // removing the first comma and insert the group by statement
            groupby.Remove(0, 1);
            groupby.Insert(0, "group by ");

            sb.AppendFormat("select count(DISTINCT Clicks.TransactionId) as Clicks, sum(Cost) as Cost, sum(Clicks.Revenue) as Revenue {0} ", select);
            sb.Append("from Clicks ");
            sb.Append(gby_subid == null ? string.Empty : string.Format("left join ({0}) as csu on Clicks.ClickId = csu.ClickId ", subidquery));
            sb.AppendFormat("where CustomerId = {0} ", customerid);
            sb.Append("and ClickDate between @fromdate and @todate and Clicks.bot = 0 ");
            sb.Append(fcampaign == null ? string.Empty : string.Format("and CampaignId={0} ", fcampaign));
            sb.Append(activecampaigns ? string.Format("and CampaignId in (select CampaignId from Campaigns where CustomerId = {0} and Status = 1) ", customerid) : string.Empty);
            sb.Append(userid.HasValue ? string.Format("and CampaignId not in (select ca.CampaignId from UserHiddenCampaigns h inner join Campaigns ca on ca.Id = h.CampaignId where h.UserId = {0}) ", userid) : string.Empty);
            sb.Append(fcountrycode == null ? string.Empty : string.Format("and Clicks.Country = '{0}' ", fcountrycode));

            if (faffiliateids != null)
            {
                var sbaux = new StringBuilder();
                foreach (var item in faffiliateids)
                {
                    sbaux.AppendFormat("OR AffiliateId = {0} ", item);
                }

                sbaux.Remove(0, 2);
                sb.AppendFormat("and ({0}) ", sbaux);
            }

            sb.Append(groupby.ToString());

            return sb.ToString();
        }


        public static string AffiliateReportQueryTest(int customerid, int userid, int offsethour = 0,
           bool by_campaign = false, bool by_source = false, int[] by_subid = null, bool by_country = false,
           int? fcampaignid = null, int[] faffiliateids = null, string fcountrycode = null)
        {

            var Delatretablefunction = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                //var _name = string.IsNullOrEmpty(tname) ? string.Empty : tname + ".";

                aux.Append(by_campaign ? string.Format(", CampaignId int") : string.Empty);
                aux.Append(by_source ? string.Format(", Source varchar(max)") : string.Empty);
                aux.Append(by_country ? string.Format(", Country varchar(max)") : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat(", SubId{0} varchar(max)", item, tname);
                return aux.ToString();
            });

            var selectfunctionDelareTable = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                //var _name = string.IsNullOrEmpty(tname) ? string.Empty : tname + ".";

                aux.Append(by_campaign ? string.Format(", {0}CampaignId ", tname) : string.Empty);
                aux.Append(by_campaign ? string.Format(", {0}campaignname ", tname) : string.Empty);
                aux.Append(by_source ? string.Format(", {0}Source ", tname) : string.Empty);
                aux.Append(by_country ? string.Format(", {0}Country ", tname) : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat(", {1}SubId{0} ", item, tname);

                return aux.ToString();
            });
            var Groupbycampaignfunction = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                //var _name = string.IsNullOrEmpty(tname) ? string.Empty : tname + ".";

                aux.Append(by_campaign ? string.Format(" INNER JOIN (SELECT campaignname, campaignid  as cmpid FROM   campaigns WHERE  customerid = {0}) as cmp on Temp.campaignid =cmp.cmpid", customerid) : string.Empty);

                return aux.ToString();
            });

            var selectfunction = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                //var _name = string.IsNullOrEmpty(tname) ? string.Empty : tname + ".";

                aux.Append(by_campaign ? string.Format(", {0}CampaignId ", tname) : string.Empty);
                aux.Append(by_campaign ? string.Format(", campaignname varchar(max)") : string.Empty);
                aux.Append(by_source ? string.Format(", {0}Source ", tname) : string.Empty);
                aux.Append(by_country ? string.Format(", {0}Country ", tname) : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat(", {1}SubId{0} ", item, tname);
                aux.Append(",Clicks as Clicks,0 as Conversion,0 as Impression, Cost,Revenue ");
                return aux.ToString();
            });
            var selectfunctionClick = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                //var _name = string.IsNullOrEmpty(tname) ? string.Empty : tname + ".";

                aux.Append(by_campaign ? string.Format(", {0}CampaignId ", tname) : string.Empty);
                aux.Append(by_source ? string.Format(", {0}Source ", tname) : string.Empty);
                aux.Append(by_country ? string.Format(", {0}Country ", tname) : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat(", {1}SubId{0} ", item, tname);
                aux.Append(",Clicks as Clicks,0 as Conversion,0 as Impression, Cost,Revenue ");
                return aux.ToString();
            });
            var selectfunctionConversion = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                //var _name = string.IsNullOrEmpty(tname) ? string.Empty : tname + ".";

                aux.Append(by_campaign ? string.Format(", {0}CampaignId ", tname) : string.Empty);
                aux.Append(by_source ? string.Format(", {0}Source ", tname) : string.Empty);
                aux.Append(by_country ? string.Format(", {0}Country ", tname) : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat(", {1}SubId{0} ", item, tname);
                aux.Append(",0 as Clicks,conversions as Conversion,0 as Impression, Cost,Revenue ");
                return aux.ToString();
            });
            var selectfunctionImpression = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                //var _name = string.IsNullOrEmpty(tname) ? string.Empty : tname + ".";

                aux.Append(by_campaign ? string.Format(", {0}CampaignId ", tname) : string.Empty);
                aux.Append(by_source ? string.Format(", {0}Source ", tname) : string.Empty);
                aux.Append(by_country ? string.Format(", {0}Country ", tname) : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat(", {1}SubId{0} ", item, tname);
                aux.Append(",0 as Clicks,0 as Conversion,impressions as Impression, Cost,Revenue ");
                return aux.ToString();
            });

            var function = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                aux.Append(by_campaign ? string.Format("and t.CampaignId = {0}.CampaignId ", tname) : string.Empty);
                aux.Append(by_source ? string.Format("and t.Source = {0}.Source ", tname) : string.Empty);
                aux.Append(by_country ? string.Format("and t.Country = {0}.Country ", tname) : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat("and t.SubId{0} = {1}.SubId{0} ", item, tname);

                return aux.ToString();
            });

            var sb = new StringBuilder();

            sb.AppendFormat(
                "SET NOCOUNT ON; " +
                "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " +
                "declare  @temptable table(affiliateid int" +
                "{8}" +
                ",Clicks int,Conversions int,Impressions int,Cost decimal(18,2),Revenue decimal(18,2)) " +
                " INSERT INTO @temptable" +
                "  select AffiliateId {0} from ({1}) as c " +
                "union " +
                "select AffiliateId {10} from ({2}) as co " +
                "union " +
                "select AffiliateId {11} from ({3}) as i  " +

                "select affiliateid{9},Company,sum(Clicks) as Clicks,sum(Conversions) as Conversions,sum(Impressions) as Impressions,sum(Cost) as Cost,sum(Revenue) as Revenue from @temptable as Temp" +

                "  left join ( select Company,affiliateid as affID from affiliates where Customerid={4}) as af on Temp.affiliateid=af.affID " +

                " {12} " +

                " group by affiliateid{9},Company order by affiliateid{9}"

                //"select t.AffiliateId, af.Company,  " +
                //"ISNULL(c.Clicks, 0) as Clicks,  " +
                //"ISNULL(co.Conversions, 0) as Conversions, " +
                //"ISNULL(i.Impressions, 0) as Impressions, " +
                //"ISNULL(c.Revenue, 0) + ISNULL(co.Revenue, 0) + ISNULL(i.Revenue, 0) as Revenue, " +
                //"ISNULL(c.Cost, 0) + ISNULL(co.Cost, 0) + ISNULL(i.Cost, 0) as Cost {7} {6}" +

                //"from tunion as t {5} " +
                //"inner join (select AffiliateId, Company from Affiliates where CustomerId = {4}) as af on t.AffiliateId = af.AffiliateId " +

                //"left join ({1}) as c on t.AffiliateId = c.AffiliateId " + function("c") +
                //"left join ({2}) as co on t.AffiliateId = co.AffiliateId " + function("co") +
                //"left join ({3}) as i on t.AffiliateId = i.AffiliateId " + function("i") +

                //"order by t.AffiliateId {6} " +
                //"OPTION (MAXRECURSION 0)"

                    , selectfunctionClick(null)
                    , ClickQueryTest(customerid, userid, offsethour, true,
                        by_campaign, true, by_source, by_subid, false, by_country, fcampaignid, faffiliateids, fcountrycode)

                    , ConversionQuery(customerid, userid, true,
                        by_campaign, true, by_source, by_subid, false, by_country, fcampaignid, faffiliateids, fcountrycode)

                    , ImpressionQuery(customerid, userid, true,
                        by_campaign, true, by_source, by_subid, false, by_country, fcampaignid, faffiliateids, fcountrycode)

                    , customerid
                    , by_campaign ? string.Format("inner join (select CampaignName, CampaignId from Campaigns where CustomerId = {0}) as ca on t.CampaignId = ca.CampaignId ", customerid) : string.Empty
                    , selectfunction("t.")
                    , by_campaign ? ", ca.CampaignName " : string.Empty
                    , Delatretablefunction(null)
                    , selectfunctionDelareTable(null)
                    , selectfunctionConversion(null)
                    , selectfunctionImpression(null)
                    , Groupbycampaignfunction(null)
                    );

            return sb.ToString();
        }



        public static string CustomReportQuery(int customerid, int userid, DateTime ufdate, DateTime utdate, int offset = 0, bool gby_date = false,
             bool gby_hour = false, bool gby_campaign = false, bool gby_affiliate = false, bool gby_url = false, bool gby_country = false, bool gby_status = false, bool gby_pixel = false, bool gby_postback = false, bool gby_ip = false, bool gby_transactionid = false, bool gby_conversiontype = false, bool gby_referrer = false, bool gby_statusdesc = false, bool gby_source = false, bool gby_action = false, bool gby_banner = false, bool gby_parenturl = false, bool gby_ctr = false,
              bool gby_DeviceId = false, bool gby_IsSmartphone = false, bool gby_IsiOS = false, bool gby_IsAndroid = false, bool gby_OS = false, bool gby_Browser = false, bool gby_Device_os = false, bool gby_Pointing_method = false, bool gby_Is_tablet = false, bool gby_Model_name = false, bool gby_Device_os_version = false, bool gby_Is_wireless_device = false, bool gby_Brand_name = false,
              bool gby_Marketing_name = false, bool gby_Resolution_height = false, bool gby_Resolution_width = false, bool gby_Canvas_support = false, bool gby_Viewport_width = false, bool gby_Isviewport_supported = false, bool gby_Ismobileoptimized = false, bool gby_Ishandheldfriendly = false, bool gby_Is_smarttv = false, bool gby_Isux_full_desktop = false,
             int[] by_subid = null,
            int? fcampaignid = null, int[] faffiliateids = null, string fcountrycode = null, string deviceid = null, string deviceos = null, string browser = null, string os = null, string modelname = null, string brandname = null, string marketingname = null, string resolution = null, string UserAgent = null)
        {



            var sb = new StringBuilder();
            var subIdTdeclaretext = new StringBuilder();
            var ClickselectSubIds = new StringBuilder();
            var ClickconditionrepeatSubIds = new StringBuilder();

            var ClickSubIds = new StringBuilder();


            var ConverstionselectSubIds = new StringBuilder();

            var ImpressionSubIds = new StringBuilder();
            var ImpressionconditionrepeatSubIds = new StringBuilder();

            var InnerSelectMinSubIds = new StringBuilder();

            var UnoionSelectSubIds = new StringBuilder();
            var HourlySelectSubIds = new StringBuilder();
            var TempTableSelectSubIds = new StringBuilder();

            var Converstionselectstatus = new StringBuilder();
            var Impressionselectstatus = new StringBuilder();

            var TempTableSelect = new StringBuilder();
            var actiionjoined = new StringBuilder();
            var insertparam = new StringBuilder();
            var affiliateArray = new StringBuilder();
            if (by_subid != null)
            {
                ClickSubIds.Append("LEFT JOIN (SELECT DISTINCT cs.ClickId , ");
                ImpressionSubIds.Append("LEFT JOIN (SELECT DISTINCT cs.ImpressionId , ");
                foreach (var item in by_subid)
                {
                    subIdTdeclaretext.AppendFormat(", SubId{0} varchar(max)", item);
                    ClickselectSubIds.AppendFormat(", Min(SubId{0}) ", item);
                    ClickconditionrepeatSubIds.AppendFormat(", (SELECT SubValue FROM ClickSubIds  WHERE SubIndex = {0}  AND ClickSubIds.ClickId = cs.ClickId) AS 'SubId{0}' ", item);

                    ConverstionselectSubIds.AppendFormat(", csu.SubId{0} ", item);

                    ImpressionconditionrepeatSubIds.AppendFormat(", (SELECT SubValue  FROM ImpressionSubIds  WHERE SubIndex = {0} AND ImpressionSubIds.ImpressionId = cs.ImpressionId) AS 'SubId{0}' ", item);

                    InnerSelectMinSubIds.AppendFormat(", Min(c.SubId{0}) ", item);

                    UnoionSelectSubIds.AppendFormat(", SubId{0}", item);
                    HourlySelectSubIds.AppendFormat(", Min(SubId{0})", item);
                    TempTableSelectSubIds.AppendFormat(", d.SubId{0}", item);
                    insertparam.AppendFormat(",SubId{0} ", item);
                }
                ClickconditionrepeatSubIds.Remove(0, 1);
                ImpressionconditionrepeatSubIds.Remove(0, 1);
                ClickSubIds.Append(ClickconditionrepeatSubIds);
                ClickSubIds.Append(" FROM ClickSubIds AS cs) AS csu ON c.ClickId = csu.ClickId ");
                ImpressionSubIds.Append(ImpressionconditionrepeatSubIds);
                ImpressionSubIds.Append(" FROM ImpressionSubIds AS cs) AS csu ON c.ImpressionId = csu.ImpressionId ");

            }


            if (faffiliateids != null)
            {
                var sbaux = new StringBuilder();
                foreach (var item in faffiliateids)
                {
                    sbaux.AppendFormat("OR c.AffiliateId = {0} ", item);
                }

                sbaux.Remove(0, 2);
                affiliateArray.AppendFormat("and ({0}) ", sbaux);
            }

            if (gby_status)
            {
                subIdTdeclaretext.Append(" ,Cstatus int");
                ClickselectSubIds.Append(" ,Min(con.Status) as Cstatus");
                Converstionselectstatus.Append(", c.status as Cstatus");
                Impressionselectstatus.Append(", null as Cstatus");
                InnerSelectMinSubIds.Append(", Min(c.Cstatus)");
                UnoionSelectSubIds.Append(", Cstatus");
                HourlySelectSubIds.Append(", Min(Cstatus)");
                TempTableSelectSubIds.Append(", d.Cstatus");
                insertparam.Append(" ,Cstatus");
            }
            if (gby_pixel)
            {
                subIdTdeclaretext.Append(" ,Pixel datetime");
                ClickselectSubIds.Append(" ,Min(con.Pixel) as Pixel");
                Converstionselectstatus.Append(", c.Pixel as Pixel");
                Impressionselectstatus.Append(", null as Pixel");
                InnerSelectMinSubIds.Append(", Min(c.Pixel)");
                UnoionSelectSubIds.Append(", Pixel");
                HourlySelectSubIds.Append(", Min(Pixel)");
                TempTableSelectSubIds.Append(", d.Pixel");
                insertparam.Append(" ,Pixel");
            }
            if (gby_postback)
            {
                subIdTdeclaretext.Append(" ,Postback datetime");
                ClickselectSubIds.Append(" ,Min(con.Postback) as Postback");
                Converstionselectstatus.Append(", c.Postback as Postback");
                Impressionselectstatus.Append(", null as Postback");
                InnerSelectMinSubIds.Append(", Min(c.Postback)");
                UnoionSelectSubIds.Append(", Postback");
                HourlySelectSubIds.Append(", Min(Postback)");
                TempTableSelectSubIds.Append(", d.Postback");
                insertparam.Append(" ,Postback");
            }
            if (gby_ip)
            {
                subIdTdeclaretext.Append(" ,IPAddress varchar(max)");
                ClickselectSubIds.Append(" ,Min(con.IPAddress) as IPAddress");
                Converstionselectstatus.Append(", c.IPAddress as IPAddress");
                Impressionselectstatus.Append(", null as IPAddress");
                InnerSelectMinSubIds.Append(", Min(c.IPAddress)");
                UnoionSelectSubIds.Append(", IPAddress");
                HourlySelectSubIds.Append(", Min(IPAddress)");
                TempTableSelectSubIds.Append(", d.IPAddress");
                insertparam.Append(" ,IPAddress");
            }
            if (gby_transactionid)
            {
                //subIdTdeclaretext.Append(" ,TransactionId varchar(max)");
                //ClickselectSubIds.Append(" ,Min(con.TransactionId) as TransactionId");
                //Converstionselectstatus.Append(", c.TransactionId as TransactionId");
                //Impressionselectstatus.Append(", null as TransactionId");
                //InnerSelectMinSubIds.Append(", Min(c.TransactionId)");
                //UnoionSelectSubIds.Append(", TransactionId");
                //HourlySelectSubIds.Append(", Min(TransactionId)");
                //TempTableSelectSubIds.Append(", d.TransactionId");
                //insertparam.Append(" ,TransactionId");

                subIdTdeclaretext.Append(" ,CTransactionId varchar(max)");
                ClickselectSubIds.Append(" ,Min(con.TransactionId) as CTransactionId");
                Converstionselectstatus.Append(", c.TransactionId as CTransactionId");
                Impressionselectstatus.Append(", null as CTransactionId");
                InnerSelectMinSubIds.Append(", Min(c.CTransactionId)");
                UnoionSelectSubIds.Append(", CTransactionId");
                HourlySelectSubIds.Append(", Min(CTransactionId)");
                TempTableSelectSubIds.Append(", d.CTransactionId");
                insertparam.Append(" ,CTransactionId");

            }
            if (gby_conversiontype)
            {
                subIdTdeclaretext.Append(" ,Type int");
                ClickselectSubIds.Append(" ,Min(con.Type) as Type");
                Converstionselectstatus.Append(", c.Type as Type");
                Impressionselectstatus.Append(", null as Type");
                InnerSelectMinSubIds.Append(", Min(c.Type)");
                UnoionSelectSubIds.Append(", Type");
                HourlySelectSubIds.Append(", Min(Type)");
                TempTableSelectSubIds.Append(", d.Type");
                insertparam.Append(" ,Type");
            }

            if (gby_statusdesc)
            {
                subIdTdeclaretext.Append(" ,CStatusDesc varchar(max)");
                ClickselectSubIds.Append(" ,Min(con.StatusDescription) as CStatusDesc");
                Converstionselectstatus.Append(", c.StatusDescription as CStatusDesc");
                Impressionselectstatus.Append(", null as CStatusDesc");
                InnerSelectMinSubIds.Append(", Min(c.CStatusDesc)");
                UnoionSelectSubIds.Append(", CStatusDesc");
                HourlySelectSubIds.Append(", Min(CStatusDesc)");
                TempTableSelectSubIds.Append(", d.CStatusDesc");
                insertparam.Append(" ,CStatusDesc");
            }
            if (gby_source)
            {
                subIdTdeclaretext.Append(" ,Csource varchar(max)");
                ClickselectSubIds.Append(" ,Min(c.Source) as Csource");
                Converstionselectstatus.Append(", clc.Source as Csource");
                Impressionselectstatus.Append(", null as Csource");
                InnerSelectMinSubIds.Append(", Min(c.Csource)");
                UnoionSelectSubIds.Append(", Csource");
                HourlySelectSubIds.Append(", Min(Csource)");
                TempTableSelectSubIds.Append(", d.Csource");
                insertparam.Append(" ,Csource");
            }
            if (gby_action)
            {
                subIdTdeclaretext.Append(" ,ActionId int ");
                TempTableSelect.Append(", ActionName varchar(max)");
                ClickselectSubIds.Append(" ,Min(con.ActionId)");
                Converstionselectstatus.Append(", c.ActionId");
                Impressionselectstatus.Append(", null as ActionId");
                InnerSelectMinSubIds.Append(", Min(c.ActionId)");
                UnoionSelectSubIds.Append(", ActionId");
                HourlySelectSubIds.Append(", Min(ActionId)");
                TempTableSelectSubIds.Append(", d.ActionId ,ac.Name");
                insertparam.Append(" ,ActionId");
                insertparam.Append(" ,ActionName");
                actiionjoined.Append("left join actions ac on d.ActionId = ac.Id");
            }

            ///////----------device Info-------------///////////////
            if (gby_DeviceId)
            {
                subIdTdeclaretext.Append(" ,DeviceId varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.DeviceId AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.DeviceId ");
                Impressionselectstatus.Append(" , null as DeviceId");
                InnerSelectMinSubIds.Append(" , Min(c.DeviceId)");
                UnoionSelectSubIds.Append(" , DeviceId");
                HourlySelectSubIds.Append(" , Min(DeviceId)");
                TempTableSelectSubIds.Append(" , d.DeviceId");
                insertparam.Append(" ,DeviceId");
            }
            if (gby_IsSmartphone)
            {
                subIdTdeclaretext.Append(" ,IsSmartphone varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.IsSmartphone AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.IsSmartphone ");
                Impressionselectstatus.Append(" , null as IsSmartphone");
                InnerSelectMinSubIds.Append(" , Min(c.IsSmartphone)");
                UnoionSelectSubIds.Append(" , IsSmartphone");
                HourlySelectSubIds.Append(" , Min(IsSmartphone)");
                TempTableSelectSubIds.Append(" , d.IsSmartphone");
                insertparam.Append(" ,IsSmartphone");
            }
            if (gby_IsiOS)
            {
                subIdTdeclaretext.Append(" ,IsiOS varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.IsiOS AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.IsiOS ");
                Impressionselectstatus.Append(" , null as IsiOS");
                InnerSelectMinSubIds.Append(" , Min(c.IsiOS)");
                UnoionSelectSubIds.Append(" , IsiOS");
                HourlySelectSubIds.Append(" , Min(IsiOS)");
                TempTableSelectSubIds.Append(" , d.IsiOS");
                insertparam.Append(" ,IsiOS");
            }
            if (gby_IsAndroid)
            {
                subIdTdeclaretext.Append(" ,IsAndroid varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.IsAndroid AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.IsAndroid ");
                Impressionselectstatus.Append(" , null as IsAndroid");
                InnerSelectMinSubIds.Append(" , Min(c.IsAndroid)");
                UnoionSelectSubIds.Append(" , IsAndroid");
                HourlySelectSubIds.Append(" , Min(IsAndroid)");
                TempTableSelectSubIds.Append(" , d.IsAndroid");
                insertparam.Append(" ,IsAndroid");
            }
            if (gby_OS)
            {
                subIdTdeclaretext.Append(" ,OS varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.OS AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.OS ");
                Impressionselectstatus.Append(" , null as OS");
                InnerSelectMinSubIds.Append(" , Min(c.OS)");
                UnoionSelectSubIds.Append(" , OS");
                HourlySelectSubIds.Append(" , Min(OS)");
                TempTableSelectSubIds.Append(" , d.OS");
                insertparam.Append(" ,OS");
            }
            if (gby_Browser)
            {
                subIdTdeclaretext.Append(" ,Browser varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Browser AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Browser ");
                Impressionselectstatus.Append(" , null as Browser");
                InnerSelectMinSubIds.Append(" , Min(c.Browser)");
                UnoionSelectSubIds.Append(" , Browser");
                HourlySelectSubIds.Append(" , Min(Browser)");
                TempTableSelectSubIds.Append(" , d.Browser");
                insertparam.Append(" ,Browser");
            }
            if (gby_Device_os)
            {
                subIdTdeclaretext.Append(" ,Device_os varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Device_os AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Device_os ");
                Impressionselectstatus.Append(" , null as Device_os");
                InnerSelectMinSubIds.Append(" , Min(c.Device_os)");
                UnoionSelectSubIds.Append(" , Device_os");
                HourlySelectSubIds.Append(" , Min(Device_os)");
                TempTableSelectSubIds.Append(" , d.Device_os");
                insertparam.Append(" ,Device_os");
            }
            if (gby_Pointing_method)
            {
                subIdTdeclaretext.Append(" ,Pointing_method varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Pointing_method AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Pointing_method ");
                Impressionselectstatus.Append(" , null as Pointing_method");
                InnerSelectMinSubIds.Append(" , Min(c.Pointing_method)");
                UnoionSelectSubIds.Append(" , Pointing_method");
                HourlySelectSubIds.Append(" , Min(Pointing_method)");
                TempTableSelectSubIds.Append(" , d.Pointing_method");
                insertparam.Append(" ,Pointing_method");
            }
            if (gby_Is_tablet)
            {
                subIdTdeclaretext.Append(" ,Is_tablet varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Is_tablet AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Is_tablet ");
                Impressionselectstatus.Append(" , null as Is_tablet");
                InnerSelectMinSubIds.Append(" , Min(c.Is_tablet)");
                UnoionSelectSubIds.Append(" , Is_tablet");
                HourlySelectSubIds.Append(" , Min(Is_tablet)");
                TempTableSelectSubIds.Append(" , d.Is_tablet");
                insertparam.Append(" ,Is_tablet");
            }
            if (gby_Model_name)
            {
                subIdTdeclaretext.Append(" ,Model_name varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Model_name AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Model_name ");
                Impressionselectstatus.Append(" , null as Model_name");
                InnerSelectMinSubIds.Append(" , Min(c.Model_name)");
                UnoionSelectSubIds.Append(" , Model_name");
                HourlySelectSubIds.Append(" , Min(Model_name)");
                TempTableSelectSubIds.Append(" , d.Model_name");
                insertparam.Append(" ,Model_name");
            }
            if (gby_Device_os_version)
            {
                subIdTdeclaretext.Append(" ,Device_os_version varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Device_os_version AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Device_os_version ");
                Impressionselectstatus.Append(" , null as Device_os_version");
                InnerSelectMinSubIds.Append(" , Min(c.Device_os_version)");
                UnoionSelectSubIds.Append(" , Device_os_version");
                HourlySelectSubIds.Append(" , Min(Device_os_version)");
                TempTableSelectSubIds.Append(" , d.Device_os_version");
                insertparam.Append(" ,Device_os_version");
            }
            if (gby_Is_wireless_device)
            {
                subIdTdeclaretext.Append(" ,Is_wireless_device varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Is_wireless_device AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Is_wireless_device ");
                Impressionselectstatus.Append(" , null as Is_wireless_device");
                InnerSelectMinSubIds.Append(" , Min(c.Is_wireless_device)");
                UnoionSelectSubIds.Append(" , Is_wireless_device");
                HourlySelectSubIds.Append(" , Min(Is_wireless_device)");
                TempTableSelectSubIds.Append(" , d.Is_wireless_device");
                insertparam.Append(" ,Is_wireless_device");
            }
            if (gby_Brand_name)
            {
                subIdTdeclaretext.Append(" ,Brand_name varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Brand_name AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Brand_name ");
                Impressionselectstatus.Append(" , null as Brand_name");
                InnerSelectMinSubIds.Append(" , Min(c.Brand_name)");
                UnoionSelectSubIds.Append(" , Brand_name");
                HourlySelectSubIds.Append(" , Min(Brand_name)");
                TempTableSelectSubIds.Append(" , d.Brand_name");
                insertparam.Append(" ,Brand_name");
            }
            if (gby_Marketing_name)
            {
                subIdTdeclaretext.Append(" ,Marketing_name varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Marketing_name AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Marketing_name ");
                Impressionselectstatus.Append(" , null as Marketing_name");
                InnerSelectMinSubIds.Append(" , Min(c.Marketing_name)");
                UnoionSelectSubIds.Append(" , Marketing_name");
                HourlySelectSubIds.Append(" , Min(Marketing_name)");
                TempTableSelectSubIds.Append(" , d.Marketing_name");
                insertparam.Append(" ,Marketing_name");
            }
            if (gby_Resolution_height)
            {
                subIdTdeclaretext.Append(" ,Resolution_height varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Resolution_height AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Resolution_height ");
                Impressionselectstatus.Append(" , null as Resolution_height");
                InnerSelectMinSubIds.Append(" , Min(c.Resolution_height)");
                UnoionSelectSubIds.Append(" , Resolution_height");
                HourlySelectSubIds.Append(" , Min(Resolution_height)");
                TempTableSelectSubIds.Append(" , d.Resolution_height");
                insertparam.Append(" ,Resolution_height");
            }
            if (gby_Resolution_width)
            {
                subIdTdeclaretext.Append(" ,Resolution_width varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Resolution_width AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Resolution_width ");
                Impressionselectstatus.Append(" , null as Resolution_width");
                InnerSelectMinSubIds.Append(" , Min(c.Resolution_width)");
                UnoionSelectSubIds.Append(" , Resolution_width");
                HourlySelectSubIds.Append(" , Min(Resolution_width)");
                TempTableSelectSubIds.Append(" , d.Resolution_width");
                insertparam.Append(" ,Resolution_width");
            }
            if (gby_Canvas_support)
            {
                subIdTdeclaretext.Append(" ,Canvas_support varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Canvas_support AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Canvas_support ");
                Impressionselectstatus.Append(" , null as Canvas_support");
                InnerSelectMinSubIds.Append(" , Min(c.Canvas_support)");
                UnoionSelectSubIds.Append(" , Canvas_support");
                HourlySelectSubIds.Append(" , Min(Canvas_support)");
                TempTableSelectSubIds.Append(" , d.Canvas_support");
                insertparam.Append(" ,Canvas_support");
            }
            if (gby_Viewport_width)
            {
                subIdTdeclaretext.Append(" ,Viewport_width varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Viewport_width AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Viewport_width ");
                Impressionselectstatus.Append(" , null as Viewport_width");
                InnerSelectMinSubIds.Append(" , Min(c.Viewport_width)");
                UnoionSelectSubIds.Append(" , Viewport_width");
                HourlySelectSubIds.Append(" , Min(Viewport_width)");
                TempTableSelectSubIds.Append(" , d.Viewport_width");
                insertparam.Append(" ,Viewport_width");
            }
            if (gby_Isviewport_supported)
            {
                subIdTdeclaretext.Append(" ,Isviewport_supported varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Isviewport_supported AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Isviewport_supported ");
                Impressionselectstatus.Append(" , null as Isviewport_supported");
                InnerSelectMinSubIds.Append(" , Min(c.Isviewport_supported)");
                UnoionSelectSubIds.Append(" , Isviewport_supported");
                HourlySelectSubIds.Append(" , Min(Isviewport_supported)");
                TempTableSelectSubIds.Append(" , d.Isviewport_supported");
                insertparam.Append(" ,Isviewport_supported");
            }
            if (gby_Ismobileoptimized)
            {
                subIdTdeclaretext.Append(" ,Ismobileoptimized varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Ismobileoptimized AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Ismobileoptimized ");
                Impressionselectstatus.Append(" , null as Ismobileoptimized");
                InnerSelectMinSubIds.Append(" , Min(c.Ismobileoptimized)");
                UnoionSelectSubIds.Append(" , Ismobileoptimized");
                HourlySelectSubIds.Append(" , Min(Ismobileoptimized)");
                TempTableSelectSubIds.Append(" , d.Ismobileoptimized");
                insertparam.Append(" ,Ismobileoptimized");
            }
            if (gby_Ishandheldfriendly)
            {
                subIdTdeclaretext.Append(" ,Ishandheldfriendly varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Ishandheldfriendly AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Ishandheldfriendly ");
                Impressionselectstatus.Append(" , null as Ishandheldfriendly");
                InnerSelectMinSubIds.Append(" , Min(c.Ishandheldfriendly)");
                UnoionSelectSubIds.Append(" , Ishandheldfriendly");
                HourlySelectSubIds.Append(" , Min(Ishandheldfriendly)");
                TempTableSelectSubIds.Append(" , d.Ishandheldfriendly");
                insertparam.Append(" ,Ishandheldfriendly");
            }
            if (gby_Is_smarttv)
            {
                subIdTdeclaretext.Append(" ,Is_smarttv varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Is_smarttv AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Is_smarttv ");
                Impressionselectstatus.Append(" , null as Is_smarttv");
                InnerSelectMinSubIds.Append(" , Min(c.Is_smarttv)");
                UnoionSelectSubIds.Append(" , Is_smarttv");
                HourlySelectSubIds.Append(" , Min(Is_smarttv)");
                TempTableSelectSubIds.Append(" , d.Is_smarttv");
                insertparam.Append(" ,Is_smarttv");
            }
            if (gby_Isux_full_desktop)
            {
                subIdTdeclaretext.Append(" ,Isux_full_desktop varchar(max) ");
                ClickselectSubIds.Append(" ,Min(CAST(dev.Isux_full_desktop AS varchar(max)))");
                Converstionselectstatus.Append(" , dev.Isux_full_desktop ");
                Impressionselectstatus.Append(" , null as Isux_full_desktop");
                InnerSelectMinSubIds.Append(" , Min(c.Isux_full_desktop)");
                UnoionSelectSubIds.Append(" , Isux_full_desktop");
                HourlySelectSubIds.Append(" , Min(Isux_full_desktop)");
                TempTableSelectSubIds.Append(" , d.Isux_full_desktop");
                insertparam.Append(" ,Isux_full_desktop");
            }

            ///////----------device Info-------------///////////////
            if (gby_parenturl)
            {
                subIdTdeclaretext.Append(" ,camid int ");
                ClickselectSubIds.Append(" ,Min(ca.Id)");
                Converstionselectstatus.Append(", ca.Id ");
                Impressionselectstatus.Append(", ca.Id ");
                InnerSelectMinSubIds.Append(", Min(c.camid)");
                TempTableSelect.Append(", ParentURLId int");
                UnoionSelectSubIds.Append(", camid");
                HourlySelectSubIds.Append(", Min(camid)");
                if (gby_action)
                {
                    TempTableSelectSubIds.Replace("ac.Name", "d.camid"); TempTableSelectSubIds.Append(", ac.Name ,ul.ParentURLId");
                    insertparam.Replace("ActionName", "camid"); insertparam.Append(" ,ActionName ,ParentURLId");
                }
                else { TempTableSelectSubIds.Append(", d.camid ,ul.ParentURLId"); insertparam.Append(" ,camid ,ParentURLId"); }

                actiionjoined.Append(" LEFT JOIN URLs ul ON d.urlpreviewid = ul.previewid AND d.camid = ul.campaignid ");
            }
            if (gby_ctr)
            {

                if (!gby_parenturl)
                {
                    subIdTdeclaretext.Append(" ,camid int ");
                    ClickselectSubIds.Append(" ,Min(ca.Id)");
                    Converstionselectstatus.Append(", ca.Id ");
                    Impressionselectstatus.Append(", ca.Id ");
                    InnerSelectMinSubIds.Append(", Min(c.camid)");
                    TempTableSelect.Append(", ParentURLId int");
                    UnoionSelectSubIds.Append(", camid");
                    HourlySelectSubIds.Append(", Min(camid)");
                    if (gby_action)
                    {
                        TempTableSelectSubIds.Replace("ac.Name", "d.camid"); TempTableSelectSubIds.Append(", ac.Name ,ul.ParentURLId");
                        insertparam.Replace("ActionName", "camid"); insertparam.Append(" ,ActionName ,ParentURLId");
                    }
                    else { TempTableSelectSubIds.Append(", d.camid ,ul.ParentURLId"); insertparam.Append(" ,camid ,ParentURLId"); }

                    actiionjoined.Append(" LEFT JOIN URLs ul ON d.urlpreviewid = ul.previewid AND d.camid = ul.campaignid ");
                }
                TempTableSelect.Append(", URLId int");
                TempTableSelectSubIds.Append(", ul.Id");
                insertparam.Append(" ,URLId ");
            }
            StringBuilder sbofferurl = new StringBuilder();
            if (gby_ctr)
            {
                sbofferurl.Append(" ,ul.OfferUrl");
            }
            else
            {
                sbofferurl.Append(" ,(select top 1 PreviewUrl from URLs u where u.CampaignId = ca.Id and u.PreviewId = d.URLPreviewId) as OfferUrl");
            }

            sb.AppendFormat
                (

               " SET NOCOUNT ON;" + Environment.NewLine +
"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;" + Environment.NewLine +

//"IF not @AffiliateId is null BEGIN DECLARE @afftable TABLE(id int NOT NULL) INSERT INTO @afftable select item from dbo.SplitInts(@AffiliateId, ',') END" + Environment.NewLine +


"DECLARE @Clicks TABLE(Date datetime, TransactionId varchar(50),  Cost decimal(18,2), Revenue decimal(18,2), Country varchar(2),CampaignId int,AffiliateId int,URLPreviewId int,BannerId int,Referrer VARCHAR(MAX) " + subIdTdeclaretext + ") insert into @Clicks " + Environment.NewLine +
"select  Min(c.ClickDate) as Date, c.TransactionId," + Environment.NewLine +
"Min(c.Cost), Min(c.Revenue), Min(c.Country), Min(ca.CampaignId), Min(c.AffiliateId), Min(c.URLPreviewId), Min(c.BannerId), Min(c.Referrer)  " + ClickselectSubIds + " " + Environment.NewLine +
"from Clicks c join Campaigns ca on c.CampaignId = ca.CampaignId Left Join Conversions con on c.ClickId= con.ClickId  LEFT JOIN DeviceInfoes dev ON c.UserAgentId=dev.Id " + ClickSubIds + " where " + Environment.NewLine +
"c.CustomerId = @CustomerId and c.bot = 0 and c.ClickDate between @fromdate and @todate and ca.Status = 1" + Environment.NewLine +
"and ca.CustomerId = @CustomerId and ca.Id not in (SELECT ca.campaignid FROM   userhiddencampaigns h  INNER JOIN campaigns ca ON ca.id = h.campaignid WHERE  h.userid = @UserId)" + Environment.NewLine +
"" + affiliateArray + " AND (@CampaignId is null or c.CampaignId IN ( @CampaignId ) )" + Environment.NewLine +
"and (@Country is null OR c.Country = @Country) group by c.TransactionId , convert(date,DATEADD(hh, @offset, c.ClickDate))" + Environment.NewLine +


"DECLARE @Conversions TABLE(Date datetime, ConversionId int,  Cost decimal(18,2), Revenue decimal(18,2), Country varchar(2),CampaignId int,AffiliateId int,URLPreviewId int,BannerId int,Referrer varchar(max) " + subIdTdeclaretext + ") insert into @Conversions " + Environment.NewLine +
"select c.ConversionDate as Date, c.ConversionId, c.Cost, c.Revenue, c.Country, ca.CampaignId, c.AffiliateId, " + Environment.NewLine +
"clc.URLPreviewId,c.BannerId,clc.Referrer " + ConverstionselectSubIds + Converstionselectstatus + " " + Environment.NewLine +
"from Conversions c join Campaigns ca on c.CampaignId = ca.CampaignId  Left Join Clicks clc ON c.ClickId =clc.ClickId  LEFT JOIN DeviceInfoes dev ON clc.UserAgentId =dev.Id " + ClickSubIds + "  where " + Environment.NewLine +
"c.CustomerId = @CustomerId  and c.bot = 0 and c.ConversionDate between @fromdate and @todate and c.Status = 1 and ca.Status = 1 and ca.CustomerId = @CustomerId and ca.Id not in (SELECT ca.campaignid FROM   userhiddencampaigns h " + Environment.NewLine +
"INNER JOIN campaigns ca ON ca.id = h.campaignid WHERE  h.userid = @UserId) " + affiliateArray + " AND (@CampaignId is null or c.CampaignId IN (" + Environment.NewLine +
" @CampaignId ) ) and (@Country is null OR c.Country = @Country)" + Environment.NewLine +


"DECLARE @Impressions TABLE(Date datetime, ImpressionId int,  Cost decimal(18,2), Revenue decimal(18,2), Country varchar(2),CampaignId int,AffiliateId int,URLPreviewId int,BannerId int,Referrer varchar(max) " + subIdTdeclaretext + ") insert into @Impressions " + Environment.NewLine +
"select c.ImpressionDate as Date, c.ImpressionId, c.Cost, c.Revenue, c.Country, ca.CampaignId, c.AffiliateId, c.URLPreviewId, c.BannerId, c.Referrer  " + ConverstionselectSubIds + Impressionselectstatus + " " + Environment.NewLine +
"from Impressions c join Campaigns ca on c.CampaignId = ca.CampaignId " + ImpressionSubIds + "  where " + Environment.NewLine +
"c.CustomerId = @CustomerId  and c.bot = 0 and c.ImpressionDate between @fromdate and @todate and ca.Status = 1 and ca.CustomerId = @CustomerId and ca.Id not in (SELECT ca.campaignid FROM   userhiddencampaigns h " + Environment.NewLine +
"INNER JOIN campaigns ca ON ca.id = h.campaignid WHERE  h.userid = @UserId)" + Environment.NewLine +
"" + affiliateArray + " AND (@CampaignId is null or c.CampaignId IN ( " + Environment.NewLine +
" @CampaignId ) ) and (@Country is null OR c.Country = @Country)" + Environment.NewLine +


"DECLARE @dc TABLE(Hour int,Date Date,CDatetime Datetime, Clicks int, Cost decimal(18,2), Revenue decimal(18,2), Country varchar(2),CampaignId int,AffiliateId int,URLPreviewId int,BannerId int, Referrer varchar(max) " + subIdTdeclaretext + ") insert into @dc " + Environment.NewLine +
"select   DATEPART(hh, DATEADD(hh, @offset, c.Date)) as Hour ,convert(date,DATEADD(hh, @offset, c.Date)) as Date ,DATEADD(hh, @offset, c.DATE) as CDatetime, Count(Distinct c.TransactionId) as Clicks , SUM(c.Cost) as Cost" + Environment.NewLine +
", SUM(c.Revenue) as Revenue, ISNULL((case when @ct = 0 then '' else c.Country end), '') as Country, c.CampaignId, c.AffiliateId, c.URLPreviewId, case when c.BannerId = 0 then null else  c.BannerId end as BannerId ,c.Referrer  " + InnerSelectMinSubIds + " " + Environment.NewLine +
"from @Clicks c group by DATEPART(hh, DATEADD(hh, @offset, c.Date)),convert(date,DATEADD(hh, @offset, c.Date)) ,DATEADD(hh, @offset, c.DATE),c.CampaignId,c.AffiliateId,c.URLPreviewId," + Environment.NewLine +
"case when @ct = 0 then '' else c.Country end, case when c.BannerId = 0 then null else  c.BannerId end ,c.Referrer" + Environment.NewLine +


"DECLARE @dco TABLE(Hour int,Date Date,CDatetime datetime,Conversions int,  Cost decimal(18,2), Revenue decimal(18,2), Country varchar(2),CampaignId int,AffiliateId int,URLPreviewId int,BannerId int,Referrer varchar(max) " + subIdTdeclaretext + ") insert into @dco " + Environment.NewLine +
"select DATEPART(hh, DATEADD(hh, @offset, c.Date)) as Hour ,convert(date,DATEADD(hh, @offset, c.Date)) as Date ,DATEADD(hh, @offset, c.DATE) as CDatetime, Count(c.ConversionId) as Conversions , SUM(c.Cost) as Cost" + Environment.NewLine +
", SUM(c.Revenue) as Revenue , ISNULL((case when @ct = 0 then '' else c.Country end), '') as Country ,c.CampaignId ,c.AffiliateId ,c.URLPreviewId ,case when c.BannerId = 0 then null else  c.BannerId end as BannerId,c.Referrer " + InnerSelectMinSubIds + " " + Environment.NewLine +
"from @Conversions c group by DATEPART(hh, DATEADD(hh, @offset, c.Date)),convert(date,DATEADD(hh, @offset, c.Date)) ,DATEADD(hh, @offset, c.DATE),c.CampaignId,c.AffiliateId,c.URLPreviewId" + Environment.NewLine +
",case when @ct = 0 then '' else c.Country end, case when c.BannerId = 0 then null else  c.BannerId end ,c.Referrer" + Environment.NewLine +


"DECLARE @di TABLE(Hour int,Date Date,CDatetime datetime,Impressions int,  Cost decimal(18,2), Revenue decimal(18,2), Country varchar(2),CampaignId int,AffiliateId int,URLPreviewId int,BannerId int,Referrer varchar(max) " + subIdTdeclaretext + ") insert into @di" + Environment.NewLine +
"select  DATEPART(hh, DATEADD(hh, @offset, c.Date)) as Hour ,convert(date,DATEADD(hh, @offset, c.Date)) as Date ,DATEADD(hh, @offset, c.DATE) as CDatetime, Count(c.ImpressionId) as Impressions , SUM(c.Cost) as Cost" + Environment.NewLine +
", SUM(c.Revenue) as Revenue , ISNULL((case when @ct = 0 then '' else c.Country end), '') as Country ,c.CampaignId ,c.AffiliateId ,c.URLPreviewId ,case when c.BannerId = 0 then null else  c.BannerId end as BannerId,c.Referrer " + InnerSelectMinSubIds + " " + Environment.NewLine +
"from @Impressions c group by DATEPART(hh, DATEADD(hh, @offset, c.Date)),convert(date,DATEADD(hh, @offset, c.Date)) ,DATEADD(hh, @offset, c.DATE),c.CampaignId,c.AffiliateId,c.URLPreviewId" + Environment.NewLine +
", case when @ct = 0 then '' else c.Country end, case when c.BannerId = 0 then null else  c.BannerId end ,c.Referrer " + Environment.NewLine +


"declare  @temptable table(" + Environment.NewLine +
"Hour int, Date Date ,CDatetime Datetime , Country varchar(2), Company varchar(max), CampaignName varchar(max), Clicks int,  Impressions int, Conversions int,  Cost decimal(18,2), Revenue decimal(18,2), CampaignID int, AffiliateId int, URLPreviewId int, OfferUrl varchar(max), BannerID int, Referrer varchar(max) " + subIdTdeclaretext + " " + TempTableSelect + ")" + Environment.NewLine +


"DECLARE @tempSubIds table(Hour int, Date Date ,CDatetime Datetime, Country varchar(2), CampaignId int, AffiliateId int, URLPreviewId int, BannerId int, Referrer varchar(max), ID int " + subIdTdeclaretext + " ) ;" + Environment.NewLine +

"INSERT INTO @tempSubIds" + Environment.NewLine +

"SELECT Hour,Date ,CDatetime,Country,  CampaignId, AffiliateId,  URLPreviewId,  BannerId, Referrer, Clicks  " + UnoionSelectSubIds + "  FROM @dc" + Environment.NewLine +
"UNION SELECT Hour,Date ,CDatetime, Country,  CampaignId, AffiliateId,  URLPreviewId,  BannerId, Referrer, Conversions " + UnoionSelectSubIds + "  FROM @dco " + Environment.NewLine +
"UNION SELECT Hour,Date ,CDatetime, Country,  CampaignId, AffiliateId,  URLPreviewId,  BannerId, Referrer, Impressions " + UnoionSelectSubIds + "  FROM @di " + Environment.NewLine +






//";with hourly (Hour,Date,Country,CampaignId,AffiliateId,URLPreviewId,BannerId) as ( select Hour,Date,Country,CampaignId,AffiliateId,URLPreviewId,BannerId from @dc union select Hour,Date, Country,CampaignId ,AffiliateId,URLPreviewId,BannerId from @dco union select Hour,Date, Country,CampaignId ,AffiliateId,URLPreviewId,BannerId from @di )" + Environment.NewLine +
";WITH hourly (Hour,Date ,CDatetime,Country,CampaignId,AffiliateId,URLPreviewId,BannerId,Referrer,ID " + UnoionSelectSubIds + " ) AS " + Environment.NewLine +
"(SELECT Hour,Date ,CDatetime,Country, CampaignId,  AffiliateId,  URLPreviewId, BannerId,Referrer,ID " + HourlySelectSubIds + " " + Environment.NewLine +
"FROM @tempSubIds  group  by Hour,Date ,CDatetime,Country, CampaignId,  AffiliateId, URLPreviewId,BannerId,Referrer,ID )" + Environment.NewLine +

"insert into @temptable  (Hour 	,DATE 	,CDatetime ,Country	,Company 	,CampaignName	,Clicks 	,Impressions 	,Conversions 	,Cost 	,Revenue 	,CampaignID 	,AffiliateId 	,URLPreviewId 	,OfferUrl 	,BannerID, Referrer " + insertparam + ") select d.Hour ,d.Date  ,d.CDatetime , d.Country , af.Company  , ca.CampaignName , ISNULL(c.Clicks, 0) as Clicks , ISNULL(i.Impressions, 0) as Impressions , ISNULL(co.Conversions, 0) as Conversions , ISNULL(c.Cost, 0) + ISNULL(co.Cost, 0) + ISNULL(i.Cost, 0)  as Cost , ISNULL(c.Revenue, 0) + ISNULL(co.Revenue, 0) + ISNULL(i.Revenue, 0)  as Revenue , d.CampaignId ,d.AffiliateId ,d.URLPreviewId " + Environment.NewLine +
sbofferurl + "  ,d.BannerId, d.Referrer  " + TempTableSelectSubIds + " from hourly d" + Environment.NewLine +
"inner join Affiliates af on d.AffiliateId = af.AffiliateId inner join Campaigns ca on d.CampaignId = ca.CampaignId " + actiionjoined + " left join @dc c on d.Hour = c.Hour and d.Date = c.Date AND d.CDatetime = c.CDatetime and d.Country = c.Country and d.CampaignId = c.CampaignId and d.AffiliateId = c.AffiliateId and d.URLPreviewId = c.URLPreviewId  AND d.Referrer = c.Referrer AND d.ID = c.Clicks " + Environment.NewLine +
"left join @dco co on d.Hour = co.Hour and d.Date = co.Date AND d.CDatetime = co.CDatetime and d.Country = co.Country and d.CampaignId = co.CampaignId and d.AffiliateId = co.AffiliateId and d.URLPreviewId = co.URLPreviewId AND d.Referrer = co.Referrer AND  d.ID = co.Conversions " + Environment.NewLine +
"left join @di i on d.Hour = i.Hour and d.Date = i.Date AND d.CDatetime = i.CDatetime and d.Country = i.Country and d.CampaignId = i.CampaignId and d.AffiliateId = i.AffiliateId and d.URLPreviewId = i.URLPreviewId AND d.Referrer = i.Referrer AND  d.ID = i.Impressions " + Environment.NewLine +
"where af.CustomerId = @CustomerId and ca.CustomerId = @CustomerId order by d.Hour" + Environment.NewLine +

"option (maxrecursion 0, recompile)"
 


);

            //            "If		@GroupName  = 'Hour' BEGIN  select t.Hour,t.Country,sum(t.Clicks) as Clicks,sum(t.Impressions) as Impressions,sum(t.Conversions) as Conversions,sum(t.Cost) as Cost,sum(t.Revenue) as Revenue from @temptable as t group by t.Hour,t.Country END" + Environment.NewLine +
            //"Else If @GroupName  = 'Daily' BEGIN select Convert(varchar,t.Date,101) as Date,t.Country,sum(t.Clicks) as Clicks,sum(t.Impressions) as Impressions,sum(t.Conversions) as Conversions,sum(t.Cost) as Cost,sum(t.Revenue) as Revenue from @temptable as t group by Convert(varchar,t.Date,101),t.Country END" + Environment.NewLine +
            //"Else If @GroupName  = 'Affiliate' BEGIN select t.AffiliateId,t.Company,sum(t.Clicks) as Clicks,sum(t.Conversions) as Conversions,sum(t.Impressions) as Impressions ,sum(t.Cost) as Cost,sum(t.Revenue) as Revenue from @temptable as t group by t.AffiliateId,t.Company END" + Environment.NewLine +
            //"Else If @GroupName  = 'Campaign' BEGIN select t.CampaignId,t.CampaignName,sum(t.Clicks) as Clicks,sum(t.Conversions) as Conversions,sum(t.Impressions) as Impressions ,sum(t.Cost) as Cost,sum(t.Revenue) as Revenue from @temptable as t group by t.CampaignId,t.CampaignName END" + Environment.NewLine +
            //"Else If @GroupName  = 'AdCampaign' BEGIN select t.BannerId,t.Company,t.CampaignId,t.CampaignName,sum(t.Clicks) as Clicks,sum(t.Conversions) as Conversions,sum(t.Impressions) as Impressions ,sum(t.Cost) as Cost,sum(t.Revenue) as Revenue from @temptable as t group by t.BannerId, t.Company,t.CampaignId,t.CampaignName having t.BannerId is not null END" + Environment.NewLine +
            //"Else If @GroupName  = 'Traffic' BEGIN select t.AffiliateId,t.CampaignId,t.CampaignName,sum(t.Clicks) as Clicks,t.Company,sum(t.Conversions) as Conversions,sum(t.Cost) as Cost,t.Country,sum(t.Impressions) as Impressions ,t.OfferUrl,sum(t.Revenue) as Revenue,t.URLPreviewId from @temptable as t group by t.AffiliateId,t.CampaignId,t.CampaignName,t.Company,t.Country,t.OfferUrl,t.URLPreviewId END" + Environment.NewLine +
            //"Else If @GroupName  = 'Conversion' BEGIN select  t.AffiliateId,t.CampaignId,t.CampaignName,t.Company,sum(t.Cost) as Cost,sum(t.Revenue) as Revenue from @temptable as t group by t.AffiliateId,t.CampaignId,t.CampaignName,t.Company END"



            sb = sb.Replace("@CustomerId", "'" + customerid.ToString() + "'");
            sb = sb.Replace("@fromdate", "'" + ufdate.ToString("MM/dd/yyyy HH:mm:ss") + "'");
            sb = sb.Replace("@todate", "'" + utdate.ToString("MM/dd/yyyy HH:mm:ss") + "'");
            sb = sb.Replace("@offset", offset.ToString());
            sb = sb.Replace("@UserId", "'" + userid.ToString() + "'");
            // sb = sb.Replace("@AffiliateId", (faffiliateids != null) ? string.Join(",", faffiliateids) : "null");
            sb = sb.Replace("@CampaignId", (fcampaignid != null) ? "'" + fcampaignid.ToString() + "'" : "null");
            sb = sb.Replace("@Country", (fcountrycode != null) ? "'" + fcountrycode + "'" : "null");
            sb = sb.Replace("@ct", (gby_country) ? "1" : "0");
            // sb = sb.Replace("@GroupName", "'Affiliate'");
            var selb = new StringBuilder();

            var select = new StringBuilder();
            var groupby = new StringBuilder();
            var orderby = new StringBuilder();

            if (gby_hour)
            {
                select.Append(",t.Hour");
                groupby.Append(",t.Hour");
                orderby.Append(",t.Hour");
            }
            if (gby_date)
            {
                select.Append(",Convert(varchar,t.Date,101) as Date");
                groupby.Append(",Convert(varchar,t.Date,101)");
                orderby.Append(",Date DESC");
            }

            if (gby_campaign)
            {
                select.Append(",t.CampaignId,t.CampaignName");
                groupby.Append(",t.CampaignId,t.CampaignName");
                orderby.Append(",t.CampaignId");
            }


            if (gby_affiliate)
            {
                select.Append(",t.AffiliateId,t.Company");
                groupby.Append(",t.AffiliateId,t.Company");
                orderby.Append(",t.AffiliateId");
            }

            if (gby_url)
            {
                select.Append(",t.OfferUrl,t.URLPreviewId");
                groupby.Append(",t.OfferUrl,t.URLPreviewId");
                orderby.Append(",t.OfferUrl");
            }

            if (gby_country)
            {
                select.Append(",t.Country");
                groupby.Append(",t.Country");
                orderby.Append(",t.Country");
            }
            if (by_subid != null)
            {
                foreach (var item in by_subid)
                {
                    select.AppendFormat(",ISNULL(t.SubId{0},'') as SubId{0}", item);
                    groupby.AppendFormat(",t.SubId{0}", item);
                    orderby.AppendFormat(",t.SubId{0}", item);
                }

            }
            if (gby_status)
            {
                select.Append(",ISNULL(t.Cstatus, 0)  as Status");
                groupby.Append(",t.Cstatus");
                orderby.Append(",t.Cstatus");
            }
            if (gby_pixel)
            {
                select.AppendFormat(",convert(DATETIME, DATEADD(hh, {0},t.Pixel)) as Pixel", offset);
                groupby.Append(",t.Pixel");
                orderby.Append(",t.Pixel");
            }
            if (gby_postback)
            {
                select.AppendFormat(",convert(DATETIME, DATEADD(hh, {0},t.Postback)) as Postback", offset);
                groupby.Append(",t.Postback");
                orderby.Append(",t.Postback");
            }
            if (gby_ip)
            {
                select.Append(",t.IPAddress  as IP");
                groupby.Append(",t.IPAddress");
                orderby.Append(",t.IPAddress");
            }
            if (gby_transactionid)
            {
                select.Append(",t.CTransactionId  as TransactionId");
                groupby.Append(",t.CTransactionId");
                orderby.Append(",t.CTransactionId");
            }
            if (gby_conversiontype)
            {
                select.Append(",ISNULL(t.Type, 0)  as ConversionType");
                groupby.Append(",t.Type");
                orderby.Append(",t.Type");
            }
            if (gby_referrer)
            {
                select.Append(",t.Referrer ");
                groupby.Append(",t.Referrer ");
                orderby.Append(",t.Referrer ");
            }
            if (gby_statusdesc)
            {
                select.Append(",ISNULL(t.CStatusDesc,'') as StatusDescription");
                groupby.Append(",t.CStatusDesc");
                orderby.Append(",t.CStatusDesc");
            }
            if (gby_source)
            {
                select.Append(",ISNULL(t.Csource,'') as Source");
                groupby.Append(",t.Csource");
                orderby.Append(",t.Csource");
            }
            if (gby_action)
            {
                select.Append(",ISNULL(t.ActionId,'') as ActionId , ISNULL(t.ActionName,'') as ActionName");
                groupby.Append(",t.ActionId , t.ActionName");
                orderby.Append(",t.ActionId");
            }
            if (gby_parenturl)
            {
                select.Append(",t.ParentURLId as ParentURL ");
                groupby.Append(",t.ParentURLId ");
                orderby.Append(",t.ParentURLId ");
            }
            ///////----------select  device Info-------------///////////////
            if (gby_DeviceId)
            {
                select.Append(",t.DeviceId as DeviceId ");
                groupby.Append(",t.DeviceId ");
                orderby.Append(",t.DeviceId ");
            }
            if (gby_IsSmartphone)
            {
                select.Append(",t.IsSmartphone as IsSmartphone ");
                groupby.Append(",t.IsSmartphone ");
                orderby.Append(",t.IsSmartphone ");
            }
            if (gby_IsiOS)
            {
                select.Append(",t.IsiOS as IsiOS ");
                groupby.Append(",t.IsiOS ");
                orderby.Append(",t.IsiOS ");
            }
            if (gby_IsAndroid)
            {
                select.Append(",t.IsAndroid as IsAndroid ");
                groupby.Append(",t.IsAndroid ");
                orderby.Append(",t.IsAndroid ");
            }
            if (gby_OS)
            {
                select.Append(",t.OS as OS ");
                groupby.Append(",t.OS ");
                orderby.Append(",t.OS ");
            }
            if (gby_Browser)
            {
                select.Append(",t.Browser as Browser ");
                groupby.Append(",t.Browser ");
                orderby.Append(",t.Browser ");
            }
            if (gby_Device_os)
            {
                select.Append(",t.Device_os as Device_os ");
                groupby.Append(",t.Device_os ");
                orderby.Append(",t.Device_os ");
            }
            if (gby_Pointing_method)
            {
                select.Append(",t.Pointing_method as Pointing_method ");
                groupby.Append(",t.Pointing_method ");
                orderby.Append(",t.Pointing_method ");
            }
            if (gby_Is_tablet)
            {
                select.Append(",t.Is_tablet as Is_tablet ");
                groupby.Append(",t.Is_tablet ");
                orderby.Append(",t.Is_tablet ");
            }
            if (gby_Model_name)
            {
                select.Append(",t.Model_name as Model_name ");
                groupby.Append(",t.Model_name ");
                orderby.Append(",t.Model_name ");
            }
            if (gby_Device_os_version)
            {
                select.Append(",t.Device_os_version as Device_os_version ");
                groupby.Append(",t.Device_os_version ");
                orderby.Append(",t.Device_os_version ");
            }
            if (gby_Is_wireless_device)
            {
                select.Append(",t.Is_wireless_device as Is_wireless_device ");
                groupby.Append(",t.Is_wireless_device ");
                orderby.Append(",t.Is_wireless_device ");
            }
            if (gby_Brand_name)
            {
                select.Append(",t.Brand_name as Brand_name ");
                groupby.Append(",t.Brand_name ");
                orderby.Append(",t.Brand_name ");
            }
            if (gby_Marketing_name)
            {
                select.Append(",t.Marketing_name as Marketing_name ");
                groupby.Append(",t.Marketing_name ");
                orderby.Append(",t.Marketing_name ");
            }
            if (gby_Resolution_height)
            {
                select.Append(",t.Resolution_height as Resolution_height ");
                groupby.Append(",t.Resolution_height ");
                orderby.Append(",t.Resolution_height ");
            }
            if (gby_Resolution_width)
            {
                select.Append(",t.Resolution_width as Resolution_width ");
                groupby.Append(",t.Resolution_width ");
                orderby.Append(",t.Resolution_width ");
            }
            if (gby_Canvas_support)
            {
                select.Append(",t.Canvas_support as Canvas_support ");
                groupby.Append(",t.Canvas_support ");
                orderby.Append(",t.Canvas_support ");
            }
            if (gby_Viewport_width)
            {
                select.Append(",t.Viewport_width as Viewport_width ");
                groupby.Append(",t.Viewport_width ");
                orderby.Append(",t.Viewport_width ");
            }
            if (gby_Isviewport_supported)
            {
                select.Append(",t.Isviewport_supported as Isviewport_supported ");
                groupby.Append(",t.Isviewport_supported ");
                orderby.Append(",t.Isviewport_supported ");
            }
            if (gby_Ismobileoptimized)
            {
                select.Append(",t.Ismobileoptimized as Ismobileoptimized ");
                groupby.Append(",t.Ismobileoptimized ");
                orderby.Append(",t.Ismobileoptimized ");
            }
            if (gby_Ishandheldfriendly)
            {
                select.Append(",t.Ishandheldfriendly as Ishandheldfriendly ");
                groupby.Append(",t.Ishandheldfriendly ");
                orderby.Append(",t.Ishandheldfriendly ");
            }
            if (gby_Is_smarttv)
            {
                select.Append(",t.Is_smarttv as Is_smarttv ");
                groupby.Append(",t.Is_smarttv ");
                orderby.Append(",t.Is_smarttv ");
            }
            if (gby_Isux_full_desktop)
            {
                select.Append(",t.Isux_full_desktop as Isux_full_desktop ");
                groupby.Append(",t.Isux_full_desktop ");
                orderby.Append(",t.Isux_full_desktop ");
            }

            ///////----------select device Info-------------///////////////
            if (gby_ctr)
            {
                select.Append(",t.URLPreviewId  , Min(t.URLId) as ULId , 0 as CTR");

                groupby.Append(",t.URLPreviewId ");

                if (!gby_parenturl)
                {
                    select.Append(" ,t.ParentURLId as ParentURL");
                    groupby.Append(",t.ParentURLId ");
                }
                if (!gby_campaign)
                {
                    select.Append(" , t.campaignId ");

                    groupby.Append(" ,t.campaignId ");
                }
            }
            if (gby_banner)
            { select.Append(", (select top 1 name from banners where BannerId=Min(t.BannerId) and CustomerId='" + customerid.ToString() + "') as Banner"); }
            select.Append(",ISNULL(sum(t.Clicks), 0) as Clicks,ISNULL(sum(t.Conversions), 0)  as Conversions,ISNULL(sum(t.Impressions), 0)  as Impressions ,ISNULL(sum(t.Cost), 0)  as Cost,ISNULL(sum(t.Revenue), 0)  as Revenue  from @temptable as t ");
            //to remove first comma,
            select.Remove(0, 1);
            select.Insert(0, " select ");

            if (groupby.Length > 0)
            {
                groupby.Remove(0, 1);
                groupby.Insert(0, " group by ");
            }
            if (orderby.Length > 0)
            {
                orderby.Remove(0, 1);
                orderby.Insert(0, " order by ");
            }

            selb.AppendFormat("{0}{1}{2}", select, groupby, orderby);
            //sb.Append("  SET NOCOUNT ON; SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; IF NOT NULL IS NULL BEGIN DECLARE @afftable TABLE (id INT NOT NULL) INSERT INTO @afftable SELECT item FROM dbo.SplitInts(NULL, ',') END DECLARE @Clicks TABLE ( DATE DATETIME ,TransactionId VARCHAR(50) ,Cost DECIMAL(18, 2) ,Revenue DECIMAL(18, 2) ,Country VARCHAR(2) ,CampaignId INT ,AffiliateId INT ,URLPreviewId INT ,BannerId INT ,camid INT ) INSERT INTO @Clicks SELECT Min(c.ClickDate) AS DATE ,c.TransactionId ,Min(c.Cost) ,Min(c.Revenue) ,Min(c.Country) ,Min(ca.CampaignId) ,Min(c.AffiliateId) ,Min(c.URLPreviewId) ,Min(c.BannerId) ,Min(ca.Id) FROM Clicks c INNER JOIN Campaigns ca ON c.CampaignId = ca.CampaignId LEFT JOIN Conversions con ON c.ClickId = con.ClickId WHERE c.CustomerId = '7' AND c.UserAgent NOT LIKE '%bot%' AND c.ClickDate BETWEEN '06/04/2015 04:00:00' AND '06/05/2015 03:59:59' AND ca.STATUS = 1 AND ca.CustomerId = '7' AND ca.Id NOT IN ( SELECT ca.campaignid FROM userhiddencampaigns h INNER JOIN campaigns ca ON ca.id = h.campaignid WHERE h.userid = '13' ) AND ( NULL IS NULL OR c.AffiliateId IN ( SELECT id FROM @afftable ) ) AND ( NULL IS NULL OR c.CampaignId IN ( SELECT campaignid FROM campaigns WHERE customerid = '7' AND STATUS = 1 ) ) AND ( NULL IS NULL OR c.Country = NULL ) GROUP BY c.TransactionId ,convert(DATE, DATEADD(hh, - 4, c.ClickDate)) DECLARE @Conversions TABLE ( DATE DATETIME ,ConversionId INT ,Cost DECIMAL(18, 2) ,Revenue DECIMAL(18, 2) ,Country VARCHAR(2) ,CampaignId INT ,AffiliateId INT ,URLPreviewId INT ,BannerId INT ,camid INT ) INSERT INTO @Conversions SELECT c.ConversionDate AS DATE ,c.ConversionId ,c.Cost ,c.Revenue ,c.Country ,ca.CampaignId ,c.AffiliateId ,( SELECT TOP 1 k.URLPreviewId FROM Clicks k WHERE k.ClickId = c.ClickId ) AS URLPreviewId ,c.BannerId ,ca.Id FROM Conversions c INNER JOIN Campaigns ca ON c.CampaignId = ca.CampaignId LEFT JOIN Clicks clc ON c.ClickId = clc.ClickId WHERE c.CustomerId = '7' AND c.UserAgent NOT LIKE '%bot%' AND c.ConversionDate BETWEEN '06/04/2015 04:00:00' AND '06/05/2015 03:59:59' AND c.STATUS = 1 AND ca.STATUS = 1 AND ca.CustomerId = '7' AND ca.Id NOT IN ( SELECT ca.campaignid FROM userhiddencampaigns h INNER JOIN campaigns ca ON ca.id = h.campaignid WHERE h.userid = '13' ) AND ( NULL IS NULL OR c.AffiliateId IN ( SELECT id FROM @afftable ) ) AND ( NULL IS NULL OR c.CampaignId IN ( SELECT campaignid FROM campaigns WHERE customerid = '7' AND STATUS = 1 ) ) AND ( NULL IS NULL OR c.Country = NULL ) DECLARE @Impressions TABLE ( DATE DATETIME ,ImpressionId INT ,Cost DECIMAL(18, 2) ,Revenue DECIMAL(18, 2) ,Country VARCHAR(2) ,CampaignId INT ,AffiliateId INT ,URLPreviewId INT ,BannerId INT ,camid INT ) INSERT INTO @Impressions SELECT c.ImpressionDate AS DATE ,c.ImpressionId ,c.Cost ,c.Revenue ,c.Country ,ca.CampaignId ,c.AffiliateId ,c.URLPreviewId ,c.BannerId ,ca.Id FROM Impressions c INNER JOIN Campaigns ca ON c.CampaignId = ca.CampaignId WHERE c.CustomerId = '7' AND c.UserAgent NOT LIKE '%bot%' AND c.ImpressionDate BETWEEN '06/04/2015 04:00:00' AND '06/05/2015 03:59:59' AND ca.STATUS = 1 AND ca.CustomerId = '7' AND ca.Id NOT IN ( SELECT ca.campaignid FROM userhiddencampaigns h INNER JOIN campaigns ca ON ca.id = h.campaignid WHERE h.userid = '13' ) AND ( NULL IS NULL OR c.AffiliateId IN ( SELECT id FROM @afftable ) ) AND ( NULL IS NULL OR c.CampaignId IN ( SELECT campaignid FROM campaigns WHERE customerid = '7' AND STATUS = 1 ) ) AND ( NULL IS NULL OR c.Country = NULL ) DECLARE @dc TABLE ( Hour INT ,DATE DATE ,Clicks INT ,Cost DECIMAL(18, 2) ,Revenue DECIMAL(18, 2) ,Country VARCHAR(2) ,CampaignId INT ,AffiliateId INT ,URLPreviewId INT ,BannerId INT ,camid INT ) INSERT INTO @dc SELECT DATEPART(hh, DATEADD(hh, - 4, c.DATE)) AS Hour ,convert(DATE, DATEADD(hh, - 4, c.DATE)) AS DATE ,Count(DISTINCT c.TransactionId) AS Clicks ,SUM(c.Cost) AS Cost ,SUM(c.Revenue) AS Revenue ,ISNULL(( CASE WHEN 0 = 0 THEN '' ELSE c.Country END ), '') AS Country ,c.CampaignId ,c.AffiliateId ,c.URLPreviewId ,CASE WHEN c.BannerId = 0 THEN NULL ELSE c.BannerId END AS BannerId ,Min(c.camid) FROM @Clicks c GROUP BY DATEPART(hh, DATEADD(hh, - 4, c.DATE)) ,convert(DATE, DATEADD(hh, - 4, c.DATE)) ,c.CampaignId ,c.AffiliateId ,c.URLPreviewId ,CASE WHEN 0 = 0 THEN '' ELSE c.Country END ,CASE WHEN c.BannerId = 0 THEN NULL ELSE c.BannerId END DECLARE @dco TABLE ( Hour INT ,DATE DATE ,Conversions INT ,Cost DECIMAL(18, 2) ,Revenue DECIMAL(18, 2) ,Country VARCHAR(2) ,CampaignId INT ,AffiliateId INT ,URLPreviewId INT ,BannerId INT ,camid INT ) INSERT INTO @dco SELECT DATEPART(hh, DATEADD(hh, - 4, c.DATE)) AS Hour ,convert(DATE, DATEADD(hh, - 4, c.DATE)) AS DATE ,Count(c.ConversionId) AS Conversions ,SUM(c.Cost) AS Cost ,SUM(c.Revenue) AS Revenue ,ISNULL(( CASE WHEN 0 = 0 THEN '' ELSE c.Country END ), '') AS Country ,c.CampaignId ,c.AffiliateId ,c.URLPreviewId ,CASE WHEN c.BannerId = 0 THEN NULL ELSE c.BannerId END AS BannerId ,Min(c.camid) FROM @Conversions c GROUP BY DATEPART(hh, DATEADD(hh, - 4, c.DATE)) ,convert(DATE, DATEADD(hh, - 4, c.DATE)) ,c.CampaignId ,c.AffiliateId ,c.URLPreviewId ,CASE WHEN 0 = 0 THEN '' ELSE c.Country END ,CASE WHEN c.BannerId = 0 THEN NULL ELSE c.BannerId END DECLARE @di TABLE ( Hour INT ,DATE DATE ,Impressions INT ,Cost DECIMAL(18, 2) ,Revenue DECIMAL(18, 2) ,Country VARCHAR(2) ,CampaignId INT ,AffiliateId INT ,URLPreviewId INT ,BannerId INT ,camid INT ) INSERT INTO @di SELECT DATEPART(hh, DATEADD(hh, - 4, c.DATE)) AS Hour ,convert(DATE, DATEADD(hh, - 4, c.DATE)) AS DATE ,Count(c.ImpressionId) AS Impressions ,SUM(c.Cost) AS Cost ,SUM(c.Revenue) AS Revenue ,ISNULL(( CASE WHEN 0 = 0 THEN '' ELSE c.Country END ), '') AS Country ,c.CampaignId ,c.AffiliateId ,c.URLPreviewId ,CASE WHEN c.BannerId = 0 THEN NULL ELSE c.BannerId END AS BannerId ,Min(c.camid) FROM @Impressions c GROUP BY DATEPART(hh, DATEADD(hh, - 4, c.DATE)) ,convert(DATE, DATEADD(hh, - 4, c.DATE)) ,c.CampaignId ,c.AffiliateId ,c.URLPreviewId ,CASE WHEN 0 = 0 THEN '' ELSE c.Country END ,CASE WHEN c.BannerId = 0 THEN NULL ELSE c.BannerId END DECLARE @temptable TABLE ( Hour INT ,DATE DATE ,Country VARCHAR(2) ,Company VARCHAR(max) ,CampaignName VARCHAR(max) ,Clicks INT ,Impressions INT ,Conversions INT ,Cost DECIMAL(18, 2) ,Revenue DECIMAL(18, 2) ,CampaignID INT ,AffiliateId INT ,URLPreviewId INT ,OfferUrl VARCHAR(max) ,BannerID INT ,camid INT ,ParentURLId INT , URLId Int ) DECLARE @tempSubIds TABLE ( Hour INT ,DATE DATE ,Country VARCHAR(2) ,CampaignId INT ,AffiliateId INT ,URLPreviewId INT ,BannerId INT ,camid INT ); INSERT INTO @tempSubIds SELECT Hour ,DATE ,Country ,CampaignId ,AffiliateId ,URLPreviewId ,BannerId ,camid FROM @dc UNION SELECT Hour ,DATE ,Country ,CampaignId ,AffiliateId ,URLPreviewId ,BannerId ,camid FROM @dco UNION SELECT Hour ,DATE ,Country ,CampaignId ,AffiliateId ,URLPreviewId ,BannerId ,camid FROM @di; WITH hourly ( Hour ,DATE ,Country ,CampaignId ,AffiliateId ,URLPreviewId ,BannerId ,camid ) AS ( SELECT Hour ,DATE ,Country ,CampaignId ,AffiliateId ,URLPreviewId ,BannerId ,Min(camid) FROM @tempSubIds GROUP BY Hour ,DATE ,Country ,CampaignId ,AffiliateId ,URLPreviewId ,BannerId ) INSERT INTO @temptable SELECT d.Hour ,d.DATE ,d.Country ,af.Company ,ca.CampaignName ,ISNULL(c.Clicks, 0) AS Clicks ,ISNULL(i.Impressions, 0) AS Impressions ,ISNULL(co.Conversions, 0) AS Conversions ,ISNULL(c.Cost, 0) + ISNULL(co.Cost, 0) + ISNULL(i.Cost, 0) AS Cost ,ISNULL(c.Revenue, 0) + ISNULL(co.Revenue, 0) + ISNULL(i.Revenue, 0) AS Revenue ,d.CampaignId ,d.AffiliateId ,d.URLPreviewId ,( SELECT TOP 1 PreviewUrl FROM URLs u WHERE u.CampaignId = ca.Id AND u.PreviewId = d.URLPreviewId ) AS OfferUrl ,d.BannerId ,d.camid ,ul.ParentURLId ,ul.Id FROM hourly d INNER JOIN Affiliates af ON d.AffiliateId = af.AffiliateId INNER JOIN Campaigns ca ON d.CampaignId = ca.CampaignId LEFT JOIN URLs ul ON d.urlpreviewid = ul.previewid AND d.camid = ul.campaignid LEFT JOIN @dc c ON d.Hour = c.Hour AND d.DATE = c.DATE AND d.Country = c.Country AND d.CampaignId = c.CampaignId AND d.AffiliateId = c.AffiliateId AND d.URLPreviewId = c.URLPreviewId LEFT JOIN @dco co ON d.Hour = co.Hour AND d.DATE = co.DATE AND d.Country = co.Country AND d.CampaignId = co.CampaignId AND d.AffiliateId = co.AffiliateId AND d.URLPreviewId = co.URLPreviewId LEFT JOIN @di i ON d.Hour = i.Hour AND d.DATE = i.DATE AND d.Country = i.Country AND d.CampaignId = i.CampaignId AND d.AffiliateId = i.AffiliateId AND d.URLPreviewId = i.URLPreviewId WHERE af.CustomerId = '7' AND ca.CustomerId = '7' ORDER BY d.Hour OPTION ( MAXRECURSION 0 ,RECOMPILE ) SELECT t.OfferUrl ,t.URLPreviewId ,t.ParentURLId AS ParentURL ,t.campaignId ,ISNULL(sum(t.Clicks), 0) AS Clicks ,ISNULL(sum(t.Conversions), 0) AS Conversions ,ISNULL(sum(t.Impressions), 0) AS Impressions ,ISNULL(sum(t.Cost), 0) AS Cost ,ISNULL(sum(t.Revenue), 0) AS Revenue ,Min(t.URLId) as ULId ,0 as CTR FROM @temptable AS t GROUP BY t.OfferUrl ,t.URLPreviewId ,t.ParentURLId,t.campaignId ORDER BY t.OfferUrl ,t.ParentURLId");
            return sb.Append(selb.ToString()).ToString();
        }


        public static string ClicksDetailsQuery(int customerid, int userid, DateTime ufdate, DateTime utdate, int offset = 0, int? cp = null, int[] aff = null, string source = null, int[] by_subid = null, string deviceid = null, string deviceos = null, string browser = null, string os = null, string modelname = null, string brandname = null, string marketingname = null, string resolution = null)
        {
            var sb = new StringBuilder();
            var ua = new StringBuilder();
            var SelectSubIds = new StringBuilder();
            var QuerysubIds = new StringBuilder();

            if (by_subid.Length > 0)
            {
                QuerysubIds.Append("LEFT JOIN (SELECT DISTINCT cs.ClickId  ");
                foreach (var item in by_subid)
                {
                    QuerysubIds.AppendFormat(", (SELECT SubValue FROM ClickSubIds  WHERE SubIndex = {0}  AND ClickSubIds.ClickId = cs.ClickId) AS 'SubId{0}' ", item);
                    SelectSubIds.AppendFormat(",min(csu.SubId{0}) as SubId{0}", item);
                }
                QuerysubIds.Append("FROM ClickSubIds AS cs ) AS csu ON c.ClickId = csu.ClickId");
            }

            if (cp != null)
            { ua.AppendFormat("AND ca.CampaignId = '{0}'", cp); }
            if (aff != null && aff.Length > 0)
            { string affs = string.Join(",", aff); ; ua.AppendFormat("AND c.AffiliateId in({0}) ", affs); }
            if (source != null)
            { ua.AppendFormat("AND LTRIM(c.Source) = '{0}'", source.Trim().ToLower()); }
            if (deviceid != null)
            { ua.AppendFormat("AND LTRIM(dev.DeviceId) = '{0}'", deviceid.Trim().ToLower()); }
            if (deviceos != null)
            { ua.AppendFormat("AND LTRIM(dev.Device_os) = '{0}'", deviceos.Trim().ToLower()); }
            if (browser != null)
            { ua.AppendFormat("AND LTRIM(dev.Browser) = '{0}'", browser.Trim().ToLower()); }
            if (os != null)
            { ua.AppendFormat("AND LTRIM(dev.OS) = '{0}'", os.Trim().ToLower()); }
            if (modelname != null)
            { ua.AppendFormat("AND LTRIM(dev.Model_name) = '{0}'", modelname.Trim().ToLower()); }
            if (brandname != null)
            { ua.AppendFormat("AND LTRIM(dev.Brand_name) = '{0}'", brandname.Trim().ToLower()); }
            if (marketingname != null)
            { ua.AppendFormat("AND LTRIM(dev.Marketing_name) = '{0}'", marketingname.Trim().ToLower()); }
            if (resolution != null)
            { ua.AppendFormat("AND LTRIM(dev.Resolution_height) = '{0}'", resolution.Trim().ToLower()); }

            sb.AppendFormat("SELECT Min(ca.CampaignId) as CampaignId ,Min(c.AffiliateId) as AffiliateId ,Min(c.BannerId) as BannerId ,Min(convert(DATETIME, DATEADD(hh, {4}, c.ClickDate))) AS ClickDate , Min(c.UserAgent) AS UserAgent , Min(c.IPAddress) as IPAddress , Min(c.Referrer) as Referrer , Min(c.Source) as Source , c.TransactionId as TransactionId , Min(c.Country) as Country , Min(c.Revenue) as Revenue , Min(dev.DeviceId) as DeviceId , Min(CAST( dev.IsSmartphone AS VARCHAR(max))) as IsSmartphone , Min(CAST(dev.IsiOS AS VARCHAR(max))) as IsiOS , Min(CAST(dev.IsAndroid AS VARCHAR(max))) as IsAndroid , Min(dev.OS) as OS , Min(dev.Browser) as Browser , Min(dev.Device_os) as Device_os , Min(dev.Pointing_method) as Pointing_method , Min(CAST(dev.Is_tablet AS VARCHAR(max))) as Is_tablet , Min(dev.Model_name) as Model_name , Min(dev.Device_os_version) as Device_os_version , Min(CAST(dev.Is_wireless_device AS VARCHAR(max))) as Is_wireless_device , Min(dev.Brand_name) as Brand_name , Min(dev.Marketing_name) as Marketing_name , Min(CAST(dev.Is_assign_phone_number AS VARCHAR(max))) as Is_assign_phone_number , Min(dev.Xhtmlmp_mime_type) as Xhtmlmp_mime_type , Min(dev.Xhtml_support_level) as Xhtml_support_level , Min(dev.Resolution_height) as Resolution_height , Min(dev.Resolution_width) as Resolution_width , Min(dev.Canvas_support) as Canvas_support , Min(dev.Viewport_width) as Viewport_width , Min(dev.Html_preferred_dtd ) as Html_preferred_dtd , Min(CAST(dev.Isviewport_supported AS VARCHAR(max))) as Isviewport_supported , Min(CAST(dev.Ismobileoptimized AS VARCHAR(max))) as Ismobileoptimized , Min(CAST(dev.Isimage_inlining AS VARCHAR(max))) as Isimage_inlining , Min(CAST(dev.Ishandheldfriendly AS VARCHAR(max))) as Ishandheldfriendly , Min(CAST(dev.Is_smarttv AS VARCHAR(max))) as Is_smarttv , Min(CAST(dev.Isux_full_desktop AS VARCHAR(max))) as Isux_full_desktop" + SelectSubIds.ToString() +
            " FROM Clicks c INNER JOIN Campaigns ca ON c.CampaignId = ca.CampaignId LEFT JOIN DeviceInfoes dev ON c.UserAgentId = dev.Id " + QuerysubIds.ToString() +
            " WHERE c.CustomerId = '{0}'" +
                ua
                + " AND c.bot = 0 AND c.ClickDate BETWEEN '{1}' AND '{2}' AND ca.STATUS = 1 AND ca.CustomerId = '{0}' AND ca.Id NOT IN ( SELECT ca.campaignid FROM userhiddencampaigns h INNER JOIN campaigns ca ON ca.id = h.campaignid WHERE h.userid = '{3}' ) AND ( NULL IS NULL OR c.CampaignId IN (NULL) ) AND ( NULL IS NULL OR c.Country = NULL ) GROUP BY c.TransactionId ,convert(DATE, DATEADD(hh, {4}, c.ClickDate)) order by ClickDate", customerid, ufdate, utdate, userid, offset);

            return sb.ToString();
        }











        public static string CampaignReportQuery(int customerid, int userid, int offset = 0,
            bool by_affiliate = false, bool by_source = false, int[] by_subid = null,
            bool by_url = false, bool by_country = false,
            int? fcampaignid = null, int[] faffiliateids = null, string fcountrycode = null)
        {

            var selectfunction = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                aux.Append(by_affiliate ? string.Format(", {0}AffiliateId ", tname) : string.Empty);
                aux.Append(by_url ? string.Format(", {0}UrlPreviewId ", tname) : string.Empty);
                aux.Append(by_source ? string.Format(", {0}Source ", tname) : string.Empty);
                aux.Append(by_country ? string.Format(", {0}Country ", tname) : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat(", {1}SubId{0} ", item, tname);

                return aux.ToString();
            });

            var function = new Func<string, string>(tname =>
            {
                var aux = new StringBuilder();
                aux.Append(by_affiliate ? string.Format("and t.AffiliateId = {0}.AffiliateId ", tname) : string.Empty);
                aux.Append(by_url ? string.Format("and t.UrlPreviewId = {0}.UrlPreviewId ", tname) : string.Empty);
                aux.Append(by_source ? string.Format("and t.Source = {0}.Source ", tname) : string.Empty);
                aux.Append(by_country ? string.Format("and t.Country = {0}.Country ", tname) : string.Empty);
                if (by_subid != null)
                    foreach (var item in by_subid)
                        aux.AppendFormat("and t.SubId{0} = {1}.SubId{0} ", item, tname);

                return aux.ToString();
            });

            var sb = new StringBuilder();

            sb.AppendFormat(
                "SET NOCOUNT ON; " +
                "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " +
               "WITH tunion (CampaignId {0}) AS ( " +
               "select CampaignId {0} from ({1}) as c " +
               "union " +
               "select CampaignId {0} from ({2}) as co " +
               "union " +
               "select CampaignId {0} from ({3}) as i ) " +

               "select t.CampaignId, ca.CampaignName,  " +
               "ISNULL(c.Clicks, 0) as Clicks,  " +
               "ISNULL(co.Conversions, 0) as Conversions, " +
               "ISNULL(i.Impressions, 0) as Impressions, " +
               "ISNULL(c.Revenue, 0) + ISNULL(co.Revenue, 0) + ISNULL(i.Revenue, 0) as Revenue, " +
               "ISNULL(c.Cost, 0) + ISNULL(co.Cost, 0) + ISNULL(i.Cost, 0) as Cost {7} {6}" +

               "from tunion as t {5} " +
               "inner join (select CampaignId, CampaignName from Campaigns where CustomerId = {4}) as ca on t.CampaignId = ca.CampaignId " +

               "left join ({8}) as c on t.CampaignId = c.CampaignId " + function("c") +
               "left join ({2}) as co on t.CampaignId = co.CampaignId " + function("co") +
               "left join ({3}) as i on t.CampaignId = i.CampaignId " + function("i") +

               "order by t.CampaignId {6} " +
               "OPTION (MAXRECURSION 0)"

                   , selectfunction(null)
                   , ClickQuery(customerid, userid, true,
                       true, by_affiliate, by_source, by_subid, by_url, by_country, fcampaignid, faffiliateids, fcountrycode)

                   , ConversionQuery(customerid, userid, true,
                       true, by_affiliate, by_source, by_subid, by_url, by_country, fcampaignid, faffiliateids, fcountrycode)

                   , ImpressionQuery(customerid, userid, true,
                       true, by_affiliate, by_source, by_subid, by_url, by_country, fcampaignid, faffiliateids, fcountrycode)

                   , customerid
                   , by_affiliate ? string.Format("inner join (select AffiliateId, Company from Affiliates where CustomerId = {0}) as af on t.AffiliateId = af.AffiliateId ", customerid) : string.Empty
                   , selectfunction("t.")
                   , by_affiliate ? ", af.Company " : string.Empty
                   , ClickCampaignQuery(customerid, userid, true,
                       true, by_affiliate, by_source, by_subid, by_url, by_country, fcampaignid, faffiliateids, fcountrycode)
                   );

            return sb.ToString();
        }

        public static string ConversionQuery(int customerid, int? userid = null, bool activecampaigns = false,
            bool gby_campaign = false, bool gby_affiliate = false, bool gby_source = false, int[] gby_subid = null,
            bool gby_url = false, bool gby_country = false,
            int? fcampaign = null, int[] faffiliateids = null, string fcountrycode = null)
        {
            var sb = new StringBuilder();

            var select = new StringBuilder();
            var groupby = new StringBuilder();

            if (gby_campaign)
            {
                select.Append(",Conversions.CampaignId");
                groupby.Append(",Conversions.CampaignId");
            }

            if (gby_affiliate)
            {
                select.Append(",Conversions.AffiliateId");
                groupby.Append(",Conversions.AffiliateId");
            }

            if (gby_url)
            {
                select.Append(",URLPreviewId");
                groupby.Append(",URLPreviewId");
            }

            if (gby_source)
            {
                select.Append(",ISNULL(Source, '') as Source"); // not show because it is already show in the click query
                groupby.Append(",ISNULL(Source, '')");
            }

            if (gby_country)
            {
                select.Append(",ISNULL(Conversions.Country, '') as Country");
                groupby.Append(",ISNULL(Conversions.Country, '')");
            }

            var subidquery = new StringBuilder();
            if (gby_subid != null)
            {
                subidquery.Append("select distinct cs.ClickId ");
                foreach (var item in gby_subid)
                {
                    subidquery.AppendFormat(",(select SubValue from ClickSubIds where SubIndex = {0} and ClickSubIds.ClickId = cs.ClickId) as 'SubId{0}' ", item);
                    //subidquery.Append();

                    select.AppendFormat(", ISNULL(csu.SubId{0}, '') as SubId{0} ", item);
                    groupby.AppendFormat(", ISNULL(csu.SubId{0}, '') ", item);
                }
                subidquery.Append("from ClickSubIds as cs ");
            }

            groupby.Remove(0, 1);
            groupby.Insert(0, "group by ");

            sb.AppendFormat("select count(Conversions.ConversionId) as Conversions, sum(Conversions.Cost) as Cost, sum(Conversions.Revenue) as Revenue {0} ", select);
            sb.Append("from Conversions ");
            if (gby_url || gby_source || gby_subid != null)
            {
                sb.Append("inner join Clicks on Clicks.ClickId = Conversions.ClickId ");
            }
            sb.Append(gby_subid == null ? string.Empty : string.Format("left join ({0}) as csu on Clicks.ClickId = csu.ClickId ", subidquery));
            sb.AppendFormat("where Conversions.CustomerId = {0} ", customerid);
            sb.Append("and ConversionDate between @fromdate and @todate and Conversions.Status = 1 ");
            sb.Append(fcampaign == null ? string.Empty : string.Format("and Conversions.CampaignId={0} ", fcampaign));
            sb.Append(userid.HasValue ? string.Format("and Conversions.CampaignId not in (select ca.CampaignId from UserHiddenCampaigns h inner join Campaigns ca on ca.Id = h.CampaignId where h.UserId = {0}) ", userid) : string.Empty);
            sb.Append(activecampaigns ? string.Format("and Conversions.CampaignId in (select CampaignId from Campaigns where CustomerId = {0} and Status = 1) ", customerid) : string.Empty);
            sb.Append(fcountrycode == null ? string.Empty : string.Format("and Conversions.Country = '{0}' ", fcountrycode));

            if (faffiliateids != null)
            {
                var sbaux = new StringBuilder();
                foreach (var item in faffiliateids)
                {
                    sbaux.AppendFormat("OR Conversions.AffiliateId = {0} ", item);
                }

                sbaux.Remove(0, 2);
                sb.AppendFormat("and ({0}) ", sbaux);
            }


            sb.Append(groupby.ToString());

            return sb.ToString();
        }

        public static string ImpressionQuery(int customerid, int? userid = null, bool activecampaigns = false,
            bool gby_campaign = false, bool gby_affiliate = false, bool gby_source = false, int[] gby_subid = null,
            bool gby_url = false, bool gby_country = false,
            int? fcampaign = null, int[] faffiliateids = null, string fcountrycode = null)
        {
            var sb = new StringBuilder();

            var select = new StringBuilder();
            var groupby = new StringBuilder();

            if (gby_campaign)
            {
                select.Append(",Impressions.CampaignId");
                groupby.Append(",Impressions.CampaignId");
            }

            if (gby_affiliate)
            {
                select.Append(",Impressions.AffiliateId");
                groupby.Append(",Impressions.AffiliateId");
            }

            if (gby_url)
            {
                select.Append(",Impressions.URLPreviewId");
                groupby.Append(",Impressions.URLPreviewId");
            }

            if (gby_source)
            {
                select.Append(",ISNULL(Source, '') as Source");
                groupby.Append(",ISNULL(Source, '')");
            }

            if (gby_country)
            {
                select.Append(",ISNULL(Impressions.Country, '') as Country");
                groupby.Append(",ISNULL(Impressions.Country, '')");
            }

            var subidquery = new StringBuilder();
            if (gby_subid != null)
            {
                subidquery.Append("select distinct cs.ImpressionId ");
                foreach (var item in gby_subid)
                {
                    subidquery.AppendFormat(",(select SubValue from ImpressionSubIds where SubIndex = {0} and ImpressionSubIds.ImpressionId = cs.ImpressionId) as 'SubId{0}' ", item);
                    //subidquery.Append();

                    select.AppendFormat(", ISNULL(csu.SubId{0}, '') as SubId{0} ", item);
                    groupby.AppendFormat(", ISNULL(csu.SubId{0}, '') ", item);
                }
                subidquery.Append("from ImpressionSubIds as cs ");
            }


            groupby.Remove(0, 1);
            groupby.Insert(0, "group by ");

            sb.AppendFormat("select count(Impressions.ImpressionId) as Impressions, sum(Cost) as Cost, sum(Revenue) as Revenue {0} ", select);
            sb.Append("from Impressions ");
            sb.Append(gby_subid == null ? string.Empty : string.Format("left join ({0}) as csu on Impressions.ImpressionId = csu.ImpressionId ", subidquery));
            sb.AppendFormat("where CustomerId = {0} and ImpressionDate between @fromdate and @todate ", customerid);
            sb.Append(fcampaign != null ? string.Format("and CampaignId={0} ", fcampaign) : string.Empty);
            sb.Append(userid.HasValue ? string.Format("and CampaignId not in (select ca.CampaignId from UserHiddenCampaigns h inner join Campaigns ca on ca.Id = h.CampaignId where h.UserId = {0}) ", userid) : string.Empty);
            sb.Append(activecampaigns ? string.Format("and CampaignId in (select CampaignId from Campaigns where CustomerId = {0} and Status = 1) ", customerid) : string.Empty);
            sb.Append(fcountrycode == null ? string.Empty : string.Format("and Impressions.Country = '{0}' ", fcountrycode));


            if (faffiliateids != null)
            {
                var sbaux = new StringBuilder();
                foreach (var item in faffiliateids)
                {
                    sbaux.AppendFormat("OR AffiliateId = {0} ", item);
                }

                sbaux.Remove(0, 2);
                sb.AppendFormat("and ({0}) ", sbaux);
            }

            sb.Append(groupby.ToString());

            return sb.ToString();
        }



    }
}