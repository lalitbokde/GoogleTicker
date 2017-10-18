using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CpaTicker.Areas.admin.Classes.Helpers;
using CpaTicker.Areas.admin.Classes;
using Moq;
using System.Web;
using CpaTicker.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using System.Text;

namespace CpaTicker.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        //[TestMethod]
        //public void LimeLightResponse_RecordOneTransaction()
        //{
        //    #region Mocking ... Arrange
        //    var mockrepo = new Mock<ICpaTickerRepository>();

        //    // mocking post data
        //    var str = "parent_order_id=12345&order_id=123457&order_status=1";
        //    //byte[] bytes = new byte[str.Length * sizeof(char)];
        //    //Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        //    byte[] bytes = Encoding.UTF8.GetBytes(str);
        //    var stream = new MemoryStream(bytes);

        //    // mocking the request
        //    var request = new Mock<HttpRequestBase>();
        //    request.SetupGet(x => x.InputStream).Returns(stream);
        //    request.Setup(x => x.InputStream).Returns(stream);
                        
        //    // mocking the context
        //    var context = new Mock<HttpContextBase>();
        //    context.SetupGet(x => x.Request).Returns(request.Object);

        //    // creating the controller and setting the moking context
        //    var controller = new AccountController(mockrepo.Object);
        //    controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
        //    #endregion

        //    // act 
        //    RedirectToRouteResult result = controller.LimeLightResponse() as RedirectToRouteResult;

        //    // assert
        //    mockrepo.Verify(r => r.AddTransaction(It.IsAny<Transaction>()), Times.Once());
        //}

        //[TestMethod]
        //public void LimeLightResponse_AddDeclineIfStatus0()
        //{
        //    #region Mocking ... Arrange
        //    var mockrepo = new Mock<ICpaTickerRepository>();

        //    // mocking post data
        //    var str = "parent_order_id=12345&order_id=123457&order_status=0";
        //    //byte[] bytes = new byte[str.Length * sizeof(char)];
        //    //Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        //    byte[] bytes = Encoding.UTF8.GetBytes(str);
        //    var stream = new MemoryStream(bytes);

        //    // mocking the request
        //    var request = new Mock<HttpRequestBase>();
        //    request.SetupGet(x => x.InputStream).Returns(stream);
        //    request.Setup(x => x.InputStream).Returns(stream);

        //    // mocking the context
        //    var context = new Mock<HttpContextBase>();
        //    context.SetupGet(x => x.Request).Returns(request.Object);

        //    // creating the controller and setting the moking context
        //    var controller = new AccountController(mockrepo.Object);
        //    controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
        //    #endregion

        //    // act
        //    RedirectToRouteResult result = controller.LimeLightResponse() as RedirectToRouteResult;

        //    // assert
        //    mockrepo.Verify(r => r.AddOrderDecline(It.IsAny<int>()), Times.Once());
        //}

        //[TestMethod]
        //public void LimeLightResponse_AddProperTransactionParameters()
        //{
        //    #region Mocking ... Arrange
        //    var mockrepo = new Mock<ICpaTickerRepository>();

        //    // store the parameter to calling back
        //    Transaction transaction = null;
        //    mockrepo.Setup(r => r.AddTransaction(It.IsAny<Transaction>())).Callback<Transaction>(t => transaction = t);

        //    // mocking post data
        //    int parent_order_id = 1270067;
        //    int order_id = 11111;
        //    int order_status = 0; //1 For Approvals, 0 For Declines
        //    var str = string.Format("parent_order_id={0}&order_id={1}&order_status={2}", parent_order_id, order_id, order_status);
        //    //byte[] bytes = new byte[str.Length * sizeof(char)];
        //    //Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        //    byte[] bytes = Encoding.UTF8.GetBytes(str);
        //    var stream = new MemoryStream(bytes);

        //    // mocking the request
        //    var request = new Mock<HttpRequestBase>();
        //    request.SetupGet(x => x.InputStream).Returns(stream);
        //    request.Setup(x => x.InputStream).Returns(stream);

        //    // mocking the context
        //    var context = new Mock<HttpContextBase>();
        //    context.SetupGet(x => x.Request).Returns(request.Object);

        //    // creating the controller and setting the moking context
        //    var controller = new AccountController(mockrepo.Object);
        //    controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
        //    #endregion

        //    // act
        //    RedirectToRouteResult result = controller.LimeLightResponse() as RedirectToRouteResult;

        //    // assert
        //    mockrepo.Verify(r => r.AddTransaction(It.IsAny<Transaction>()));

        //    Assert.AreEqual(false, transaction.Status); // false because 0 == false
        //    Assert.AreEqual(parent_order_id, transaction.OrderId, "parent_order_id doesn't match");
        //    Assert.AreEqual(order_id, transaction.TransactionId, "order_id doesn't match");

        //}


    }
}
