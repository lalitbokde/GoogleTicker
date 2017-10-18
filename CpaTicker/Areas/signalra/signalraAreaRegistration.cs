using System.Web.Mvc;

namespace CpaTicker.Areas.signalra
{
    public class signalraAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "signalra";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "signalra_default",
                "signalra/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                name: "signalr404",
                url: "404",
                defaults: new { controller = "Home", action = "HttpStatus404" }
            );

            context.MapRoute(
                name: "signalr2Parameters",
                url: "signalra/{controller}/{action}/{id}/{id2}",
                defaults: new { controller = "helper" }
            );

            ////with manage
            //context.MapRoute(
            //    name: "2Parameters",
            //    url: "admin/{adminarea}/{controller}/{action}/{id}/{id2}",
            //    defaults: new { adminarea = "manage", controller = "helper" }
            //);

            //context.MapRoute(
            //    name: "Admin",
            //    url: "admin/{adminarea}/{controller}/{action}/{id}",
            //    defaults: new { adminarea = "manage", controller = "Layout", action = "Index", id = UrlParameter.Optional }
            //);

            context.MapRoute(
                "signalradmin_default",
                "signalra/{controller}/{action}/{id}",
                new { controller = "Layout", action = "Index",adminarea = "signalra", id = UrlParameter.Optional }
            );
        }
    }
}