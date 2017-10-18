using System.Web.Mvc;

namespace CpaTicker.Areas.admin
{
    public class adminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "404",
                url: "404",
                defaults: new{ controller = "Home", action = "HttpStatus404" }
            );

            context.MapRoute(
                name: "2Parameters",
                url: "admin/{controller}/{action}/{id}/{id2}",
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
                "admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller = "Layout", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
