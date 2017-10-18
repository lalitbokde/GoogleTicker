using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CpaTicker;
using CpaTicker.Controllers;
using System.Web.Mvc;
using CpaTicker.Areas.admin.Classes;
using CpaTicker.Areas.admin.Classes.Helpers;
using Moq;
using System.Web;
using System.Web.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CpaTicker.Tests.Controllers
{
    [TestClass]
    public class CPAControllerTest
    {
        /// <summary>
        /// Test that only one click is added every times click action is called
        /// </summary>
        [TestMethod]
        public async Task Click_ClickAddedOnce()
        {
            // arrange
            #region Mocking ...
            int campaignid = 1;
            var mockrepo = new Mock<ICpaTickerRepository>();
            mockrepo.Setup(foo => foo.GetCampaignById(It.IsAny<int>(), It.IsAny<int>())).Returns(
                new Campaign()
                {
                    CampaignId = campaignid,
                    //OfferUrl = "url"
                });

            //mockrepo.Setup(foo => foo.GetBlockFromInt32IP(It.IsAny<long>())).Returns
            //(
            //    new Block()
            //    {

            //        Location = new Location() { region = "", country = "" }
            //    }
            //);

            mockrepo.Setup(f => f.GetCustomerId(It.IsAny<Uri>())).Returns(1);

            mockrepo.Setup(f => f.GetCurrentCustomer(It.IsAny<int>())).Returns(new Customer { CustomerId = 1 });

            mockrepo.Setup(f => f.GetAffiliate(It.IsAny<int>(), It.IsAny<int>())).Returns(new Affiliate { AffiliateId = 1001 });

            var blockTask = Task<Block>.Factory.StartNew(() =>
                                  {
                                      return new Block {  locId = 1, Location = new Location { city_name = "", country_iso_code = "" } };
                                  });


            mockrepo.Setup(foo => foo.FindBlockAsync(It.IsAny<uint>())).Returns(blockTask);

            mockrepo.Setup(foo => foo.GetCampaignURLs(It.IsAny<int>())).Returns
            (
                (new List<URL>() 
                { new URL 
                    { 
                        PreviewId = 1,
                        OfferUrl = "http://utest.com",
                    } 
                }).AsQueryable()
            );
            mockrepo.Setup(foo => foo.GetAffiliate(It.IsAny<int>(), It.IsAny<int>())).Returns
            (
                new Affiliate()
            );

            
            // mocking the request
            var request = new Mock<HttpRequestBase>();
            //request.Setup(x => x.Url).Returns(url);
            request.Setup(x => x.UserHostAddress).Returns("74.208.194.190");
            // mocking the request querystring
            request.SetupGet(r => r.QueryString).Returns(HttpUtility.ParseQueryString(""));
            // mocking browser cookies capability 
            request.Setup(b => b.Browser.Cookies).Returns(true);
            // moking reques url.host
            request.SetupGet(r => r.Url).Returns(new Uri("http://utest.com"));

            // mocking the cookie
            HttpCookie cookie = new HttpCookie("cpaticker");
            cookie[campaignid.ToString()] = It.IsAny<string>();

            // mocking the respond
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(x => x.Cookies).Returns(new HttpCookieCollection(){ cookie });

            // mocking the context
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);

            // creating the controller and setting the moking context
            var controller = new CPAController(mockrepo.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            #endregion

            // act
            RedirectToRouteResult result = await controller.Click(It.IsAny<int>(), campaignid) as RedirectToRouteResult;
            //await controller.Click(It.IsAny<int>(), campaignid);

            // assert
            mockrepo.Verify(r => r.AddClick(It.IsAny<Click>()), Times.Once());
        }

        //[TestMethod]
        //public void Click_ParametersAddedToClick()
        //{
        //    #region Mocking ...
        //    Click ck = null;
        //    Campaign camp = new Campaign() { CampaignId = 1, PayoutType = PayoutType.CPC, OfferUrl = "url" };
        //    var mockrepo = new Mock<ICpaTickerRepository>();
        //    mockrepo.Setup(foo => foo.GetCampaignById(It.IsAny<int>(), It.IsAny<int>())).Returns(camp);
        //    // capture the created click
        //    mockrepo.Setup(f => f.AddClick(It.IsAny<Click>())).Callback<Click>(c => ck = c);

        //    // mock parameters
        //    int bannerid = 1;
        //    string hitid = "hitid";
        //    string subid = "subid";
        //    string subid2 = "subid2";
        //    string subid3 = "subid3";
        //    string subid4 = "subid4";
        //    string subid5 = "subid5";

        //    // mocking the request
        //    var request = new Mock<HttpRequestBase>();

        //    // mocking the cookie
        //    HttpCookie cookie = new HttpCookie("cpaticker");
        //    cookie[camp.CampaignId.ToString()] = It.IsAny<string>();

        //    // mocking the respond
        //    var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
        //    response.Setup(x => x.Cookies).Returns(new HttpCookieCollection() { cookie });

        //    // mocking the context
        //    var context = new Mock<HttpContextBase>();
        //    context.SetupGet(x => x.Request).Returns(request.Object);
        //    context.SetupGet(x => x.Response).Returns(response.Object);

        //    // creating the controller and setting the moking context
        //    var controller = new CPAController(mockrepo.Object);
        //    controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
        //    #endregion

        //    // act
        //    RedirectResult result = controller.Click(It.IsAny<int>(), It.IsAny<int>(), bannerid, hitid, subid, subid2, subid3, subid4, subid5) as RedirectResult;
            
        //    // assert
        //    Assert.AreEqual(ck.BannerId, bannerid);
        //    Assert.AreEqual(ck.HitId, hitid);
        //    Assert.AreEqual(ck.Subid1, subid);
        //    Assert.AreEqual(ck.Subid2, subid2);
        //    Assert.AreEqual(ck.Subid3, subid3);
        //    Assert.AreEqual(ck.Subid4, subid4);
        //    Assert.AreEqual(ck.Subid5, subid5);
        //}

        /// <summary>
        /// Test a permanet redirect to offer url from the url object
        /// </summary>
        [TestMethod]
        public async Task Click_PermanentRedirectToProperUrl()
        {
            // arrange
            #region Mocking ...
            int campaignid = 1;
            var mockrepo = new Mock<ICpaTickerRepository>();
            mockrepo.Setup(foo => foo.GetCampaignById(It.IsAny<int>(), It.IsAny<int>())).Returns(
                new Campaign()
                {
                    CampaignId = campaignid,
                    //OfferUrl = "url"
                });

            //mockrepo.Setup(foo => foo.GetBlockFromInt32IP(It.IsAny<long>())).Returns
            //(
            //    new Block()
            //    {

            //        Location = new Location() { region = "", country = "" }
            //    }
            //);

            mockrepo.Setup(f => f.GetCustomerId(It.IsAny<Uri>())).Returns(1);

            mockrepo.Setup(f => f.GetCurrentCustomer(It.IsAny<int>())).Returns(new Customer { CustomerId = 1 });

            mockrepo.Setup(f => f.GetAffiliate(It.IsAny<int>(), It.IsAny<int>())).Returns(new Affiliate { AffiliateId = 1001 });

            var blockTask = Task<Block>.Factory.StartNew(() =>
            {
                return new Block { locId = 1, Location = new Location { city_name = "", country_iso_code = "" } };
            });


            mockrepo.Setup(foo => foo.FindBlockAsync(It.IsAny<uint>())).Returns(blockTask);




            URL u = new URL { PreviewId = 1, OfferUrl = "http://utest.com", };
            mockrepo.Setup(foo => foo.GetCampaignURLs(It.IsAny<int>())).Returns
            (
                (new List<URL>() 
                { 
                    u
                }).AsQueryable()
            );
            mockrepo.Setup(foo => foo.GetAffiliate(It.IsAny<int>(), It.IsAny<int>())).Returns
            (
                new Affiliate()
            );


            // mocking the request
            var request = new Mock<HttpRequestBase>();
            //request.Setup(x => x.Url).Returns(url);
            request.Setup(x => x.UserHostAddress).Returns("74.208.194.190");
            // mocking the request querystring
            request.SetupGet(r => r.QueryString).Returns(HttpUtility.ParseQueryString(""));
            // mocking browser cookies capability 
            request.Setup(b => b.Browser.Cookies).Returns(true);
            // moking reques url.host
            request.SetupGet(r => r.Url).Returns(new Uri("http://utest.com"));

            // mocking the cookie
            HttpCookie cookie = new HttpCookie("cpaticker");
            cookie[campaignid.ToString()] = It.IsAny<string>();

            // mocking the respond
            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(x => x.Cookies).Returns(new HttpCookieCollection() { cookie });

            // mocking the context
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);

            // creating the controller and setting the moking context
            var controller = new CPAController(mockrepo.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            #endregion

            // act
            RedirectResult result = await controller.Click(It.IsAny<int>(), It.IsAny<int>()) as RedirectResult;

            // assert
            Assert.IsTrue(result.Permanent);
            Assert.AreEqual<Uri>(new Uri(u.OfferUrl), new Uri(result.Url)); 
        }

        //[TestMethod]
        //public void Click_ProperRevenueCostCalculation()
        //{
        //    // arrange
        //    #region Mocking ...
        //    int campaignid = 1;
        //    var mockrepo = new Mock<ICpaTickerRepository>();
        //    mockrepo.Setup(foo => foo.GetCampaignById(It.IsAny<int>(), It.IsAny<int>())).Returns(
        //        new Campaign()
        //        {
        //            CampaignId = campaignid,
        //            PayoutType = PayoutType.CPC,
        //            OfferUrl = "url"
        //        });

        //     // callback the parameter of  GetCampaignCost
        //    // mock for this test
        //    ActionType costtype = It.IsNotIn(ActionType.Click);
        //    ActionType revtype = It.IsNotIn(ActionType.Click);
        //    mockrepo.Setup(x => x.GetCampaignCost(It.IsAny<Campaign>(), It.IsAny<ActionType>(), It.IsAny<string>())).Callback<Campaign, ActionType, string>((Campaign p, ActionType t, string s) => costtype = t);
        //    mockrepo.Setup(x => x.GetCampaignRevenue(It.IsAny<Campaign>(), It.IsAny<ActionType>(), It.IsAny<string>())).Callback<Campaign, ActionType, string>((Campaign p, ActionType t, string s) => revtype = t);

        //    // mocking the request
        //    var request = new Mock<HttpRequestBase>();
        //    //request.Setup(x => x.Url).Returns(url);
        //    //request.Setup(x => x.UserHostAddress).Returns("127.0.0.1");

        //    // mocking the cookie
        //    HttpCookie cookie = new HttpCookie("cpaticker");
        //    cookie[campaignid.ToString()] = It.IsAny<string>();

        //    // mocking the respond
        //    var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
        //    response.Setup(x => x.Cookies).Returns(new HttpCookieCollection() { cookie });

        //    // mocking the context
        //    var context = new Mock<HttpContextBase>();
        //    context.SetupGet(x => x.Request).Returns(request.Object);
        //    context.SetupGet(x => x.Response).Returns(response.Object);

        //    // creating the controller and setting the moking context
        //    var controller = new CPAController(mockrepo.Object);
        //    controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
        //    #endregion

        //    // act
        //    RedirectToRouteResult result = controller.Click(It.IsAny<int>(), campaignid) as RedirectToRouteResult;
        //    // assert
        //    Assert.AreEqual(ActionType.Click, costtype, "The cost is being miscalculated");
        //    Assert.AreEqual(ActionType.Click, revtype, "The revenue is being miscalculated"); 
        //}

        
    }

    
}
