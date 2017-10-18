using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;
using RouteDebug;
using CpaTicker.Areas.admin.Classes.Helpers;
using System.Web.Helpers;
using log4net.Core;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using WURFL;
using WURFL.Aspnet.Extensions.Config;

namespace CpaTicker
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Simple Membership Initialization

            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: false);
            AreaRegistration.RegisterAllAreas();
            SqlDependency.Start(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString); 
            // create enum for all actions methods
            // this is very resources consuming thats why i commented 
            if (CpaTickerConfiguration.CreateDynamicAssembly)
            {
                DynamicAssembly.Create();
                //DynamicAssembly1.Create();
            }
            // for register a user even if there is one log in
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            ResponseSerializerConfig.Register(GlobalConfiguration.Configuration);

            DtoMapperConfig.CreateMaps();
            // for Useragent Details
            RegisterWurfl();
            //



            log4net.Config.XmlConfigurator.Configure();
        }
        protected void Application_End()
        {
            SqlDependency.Stop(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var ex = Server.GetLastError();
            var controller = new CpaTicker.Controllers.ErrorController();
            var routeData = new RouteData();
            var action = "Index";

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;

                switch (httpEx.GetHttpCode())
                {
                    case 404:
                        action = "NotFound";
                        break;

                    // others if any
                }
            }


            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            // logging the exception
            log4net.ILog log = log4net.LogManager.GetLogger(currentController);

            Type declaringType = typeof(log4net.LogManager);

            LoggingEvent loggingEvent = new LoggingEvent(declaringType, log.Logger.Repository, log.Logger.Name, Level.Error, ex.Message, ex); //null = Message, Exception    

            var stacktrace = new StackTrace(ex, true);
            var frame = stacktrace.GetFrame(0);

            //int l = frame.GetFileLineNumber();
            //var f = frame.GetFileName();

            loggingEvent.Properties["line_number"] = frame.GetFileLineNumber();
            loggingEvent.Properties["file_name"] = frame.GetFileName();

            log.Logger.Log(loggingEvent);
            //log.Error(ex.Message, ex);

            controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            
            // might occur in redirects
            //Exception ex = Server.GetLastError();
            //if (ex is System.Threading.ThreadAbortException)
            //    return;

            // send email to support


            //Logger.Error(LoggerType.Global, ex, "Exception");
            //Response.Redirect("unexpectederror.htm");

            
        }
        public static void RegisterWurfl()
        {
            WURFLManagerBuilder.Build(new ApplicationConfigurer());
        }
    }
}