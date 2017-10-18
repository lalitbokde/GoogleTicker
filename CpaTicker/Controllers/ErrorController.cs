using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return View("Error");
        }

        public ViewResult NotFound()
        {
            //Response.StatusCode = 404;  //you may want to set this to 200
            //return View("NotFound");

            Response.StatusCode = 200;
            return View("NotFound");
        }

    }
}
