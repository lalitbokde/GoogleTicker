using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.signalra.Controllers
{
    public class AjaxRequestController : Controller
    {
        public AjaxRequestController()
            : base()
        { }

        [NonAction]
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // if this is not an ajax request then redirect to it except if it's 
                // a report export
                bool state = filterContext.HttpContext.Request.QueryString.AllKeys.Contains("state");
                state = state ? filterContext.HttpContext.Request.QueryString["state"] == "1" : false;
                if (!(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Reports" && state))
                {
                    var host = filterContext.HttpContext.Request.Url.Authority;
                    var sheme = filterContext.HttpContext.Request.Url.Scheme;
                    var url = filterContext.HttpContext.Request.Url.AbsolutePath;
                    //string url = Url.Action(filterContext.ActionDescriptor.ActionName);
                    filterContext.Result = Redirect(string.Format("{0}://{1}/admin#{2}", sheme, host, url));
                }

            }

        }

    }

    public class BaseController : AjaxRequestController
    {
        public BaseController()
            : base()
        {
            this.ActionInvoker = new CustomActionInvoker();
        }

    }

    public class BaseController1 : AjaxRequestController
    {
        public BaseController1()
            : base()
        {
            this.ActionInvoker = new CustomActionInvoker1();
        }
    }
}
