using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CpaTicker.Areas.admin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CpaTicker.Tests.Controllers
{
    [TestClass]
    public class EnumTest
    {
        [TestMethod]
        public void StatisticsEnum_Tests()
        {
            /*Country*/
            Assert.AreEqual(StatisticsEnum.Country, (StatisticsEnum)0x200);
            Assert.AreEqual(StatisticsEnum.Country, (StatisticsEnum)512);

            /*Campaign*/
            Assert.AreEqual(StatisticsEnum.Campaign, (StatisticsEnum)0x80000);
            Assert.AreEqual(StatisticsEnum.Campaign, (StatisticsEnum)524288);

            /*Source*/
            Assert.AreEqual(StatisticsEnum.Source, (StatisticsEnum)0x1000000);
            Assert.AreEqual(StatisticsEnum.Source, (StatisticsEnum)16777216);

            /*Affiliate*/
            Assert.AreEqual(StatisticsEnum.Affiliate, (StatisticsEnum)0x100000);
            Assert.AreEqual(StatisticsEnum.Affiliate, (StatisticsEnum)1048576);

            /*URL*/
            Assert.AreEqual(StatisticsEnum.URL, (StatisticsEnum)0x400000);
            Assert.AreEqual(StatisticsEnum.URL, (StatisticsEnum)4194304);

            /*URLId*/
            Assert.AreEqual(StatisticsEnum.URLId, (StatisticsEnum)0x800000);
            Assert.AreEqual(StatisticsEnum.URLId, (StatisticsEnum)8388608);

            /*CTR*/
            Assert.AreEqual(StatisticsEnum.CTR, (StatisticsEnum)0x8000000000);
            Assert.AreEqual(StatisticsEnum.CTR, (StatisticsEnum)549755813888);
        }

        [TestMethod]
        public void CustomCustomStatisticsEnum_Tests()
        {
            /*Clicks*/
            Assert.AreEqual(CustomStatisticsEnum.Clicks, (CustomStatisticsEnum)0x2);
            Assert.AreEqual(CustomStatisticsEnum.Clicks, (CustomStatisticsEnum)2);

            /*Conversions*/
            Assert.AreEqual(CustomStatisticsEnum.Conversions, (CustomStatisticsEnum)0x4);
            Assert.AreEqual(CustomStatisticsEnum.Conversions, (CustomStatisticsEnum)4);

            /*Impressions*/
            Assert.AreEqual(CustomStatisticsEnum.Impressions, (CustomStatisticsEnum)0x1);
            Assert.AreEqual(CustomStatisticsEnum.Impressions, (CustomStatisticsEnum)1);

            /*Country*/
            Assert.AreEqual(CustomStatisticsEnum.Country, (CustomStatisticsEnum)0x200);
            Assert.AreEqual(CustomStatisticsEnum.Country, (CustomStatisticsEnum)512);

            /*Campaign*/
            Assert.AreEqual(CustomStatisticsEnum.Campaign, (CustomStatisticsEnum)0x80000);
            Assert.AreEqual(CustomStatisticsEnum.Campaign, (CustomStatisticsEnum)524288);

            /*Source*/
            Assert.AreEqual(CustomStatisticsEnum.Source, (CustomStatisticsEnum)0x1000000);
            Assert.AreEqual(CustomStatisticsEnum.Source, (CustomStatisticsEnum)16777216);

            /*Affiliate*/
            Assert.AreEqual(CustomStatisticsEnum.Affiliate, (CustomStatisticsEnum)0x100000);
            Assert.AreEqual(CustomStatisticsEnum.Affiliate, (CustomStatisticsEnum)1048576);

            /*URL*/
            Assert.AreEqual(CustomStatisticsEnum.URL, (CustomStatisticsEnum)0x400000);
            Assert.AreEqual(CustomStatisticsEnum.URL, (CustomStatisticsEnum)4194304);

            /*URLId*/
            Assert.AreEqual(CustomStatisticsEnum.URLId, (CustomStatisticsEnum)0x800000);
            Assert.AreEqual(CustomStatisticsEnum.URLId, (CustomStatisticsEnum)8388608);

            /*ParentURL*/
            Assert.AreEqual(CustomStatisticsEnum.ParentURL, (CustomStatisticsEnum)0x4000000000);
            Assert.AreEqual(CustomStatisticsEnum.ParentURL, (CustomStatisticsEnum)274877906944);

            /*CTR*/
            Assert.AreEqual(CustomStatisticsEnum.CTR, (CustomStatisticsEnum)0x8000000000);
            Assert.AreEqual(CustomStatisticsEnum.CTR, (CustomStatisticsEnum)549755813888);

            /*PAGE*/
            Assert.AreEqual(CustomStatisticsEnum.PAGE, (CustomStatisticsEnum)0x40000000);
            Assert.AreEqual(CustomStatisticsEnum.PAGE, (CustomStatisticsEnum)1073741824);

            /*Redirect*/
            Assert.AreEqual(CustomStatisticsEnum.Redirect, (CustomStatisticsEnum)0x80000000);
            Assert.AreEqual(CustomStatisticsEnum.Redirect, (CustomStatisticsEnum)2147483648);

            /*IsSmartphone*/
            Assert.AreEqual(CustomStatisticsEnum.IsSmartphone, (CustomStatisticsEnum)0x20000000000);
            Assert.AreEqual(CustomStatisticsEnum.IsSmartphone, (CustomStatisticsEnum)2199023255552);

            /*Browser*/
            Assert.AreEqual(CustomStatisticsEnum.Browser, (CustomStatisticsEnum)0x200000000000);
            Assert.AreEqual(CustomStatisticsEnum.Browser, (CustomStatisticsEnum)35184372088832);

            /*ClickDate*/
            Assert.AreEqual(CustomStatisticsEnum.ClickDate, (CustomStatisticsEnum)0x400);
            Assert.AreEqual(CustomStatisticsEnum.ClickDate, (CustomStatisticsEnum)1024);

            /*ConversionDate*/
            Assert.AreEqual(CustomStatisticsEnum.ConversionDate, (CustomStatisticsEnum)0x800);
            Assert.AreEqual(CustomStatisticsEnum.ConversionDate, (CustomStatisticsEnum)2048);

            /*ImpressionDate*/
            Assert.AreEqual(CustomStatisticsEnum.ImpressionDate, (CustomStatisticsEnum)0x1000);
            Assert.AreEqual(CustomStatisticsEnum.ImpressionDate, (CustomStatisticsEnum)4096);
        }
    }
}
