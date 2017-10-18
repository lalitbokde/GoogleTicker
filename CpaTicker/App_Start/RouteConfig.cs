using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CpaTicker
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "SiteDomain",
                "",
                new { controller = "Home", action = "Index" },
                namespaces: new string[] { "CPATicker.Controllers" }
            );

            routes.MapRoute(
                "OnlyAction",
                "{action}",
                new { controller = "Home", action = "Index" },
                namespaces: new string[] { "CPATicker.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id= UrlParameter.Optional}, 
                namespaces: new string[] { "CPATicker.Controllers" }
            );
        }
    }
}