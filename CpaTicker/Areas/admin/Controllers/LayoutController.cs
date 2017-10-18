using CpaTicker.Areas.admin.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CpaTicker.Areas.admin.Controllers
{
    [Authorize]
    public class LayoutController : Controller
    {
        private ICpaTickerRepository repo;

        public LayoutController()
        {
            this.repo = new EFCpatickerRepository();
        }

        //
        // GET: /admin/Layout/

        public ActionResult Index()
        {
            ViewBag.TickerSetting = repo.GetTickerSettings() ?? CPAHelper.GetDefaultTickerSettings();
            ViewBag.CustomReportData = repo.GETUserCustomReport();
            int userid = repo.GetCurrentUserId();

            ViewBag.user = repo.UserProfile().SingleOrDefault(u => u.UserId == userid);

            try
            {
                if (repo.GetProfilePic() != null)
                {
                    string imageBase64Data = Convert.ToBase64String(repo.GetProfilePic());
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageData = imageDataURL;
                }
                else
                {
                    ViewBag.ImageData = "../img/avatars/sunny.png";
                }

            }
            catch (Exception ex)
            {

            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }


    }
}
