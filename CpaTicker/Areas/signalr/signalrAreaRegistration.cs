using System.Web.Mvc;

namespace CpaTicker.Areas.signalr
{
    public class signalrAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "signalr";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "signalr_default",
                "signalr/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}