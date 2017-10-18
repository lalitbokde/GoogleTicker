using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CpaTicker.Areas.admin.Classes.Helpers;

namespace CpaTicker.Areas.admin.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : Controller
    {
        ICpaTickerRepository repo;

        public ValidationController()
        {
            this.repo = new EFCpatickerRepository();
        }

        public JsonResult CheckDomainExists(string Domainname)
        {
            if (CpaTicker.Areas.admin.Classes.Helpers.CPAHelper.IsDomainExists(Domainname))
            {
                //string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture,
                //    "The domain {0} is already taken.", Domainname);
                string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "This domain is already taken.");
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmailExists(string email)
        {
            //if (CPAHelper.EmailExists(email))
            if (repo.UserProfile().Count(u => u.Email == email) > 0)
            {
                string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "This email is already taken.");
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckFieldNameExists(string fieldname)
        {
            if (repo.FieldNameExists(fieldname))
            {
                string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture, "This field name is already taken.");
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckAccountIdExists(string accountid)
        {
            if (CPAHelper.AccountIdExists(accountid))
            {
                string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "Sorry, that sub domain has already been taken.");
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmailExistsNotMine(string email)
        {
            //if (CPAHelper.EmailExistsNotMine(email))
            if (repo.UserProfile().Count(u => u.Email == email && u.UserId != WebMatrix.WebData.WebSecurity.CurrentUserId) > 0)
            {
                string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "This email is already taken.");
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckActionName(string name, int campaignid)
        {
            if (repo.Actions().Where(a => a.CampaignId == campaignid).Count(a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) > 0)
            {
                string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture, "There is already an action with this name.");
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckActionNameForEdit(string name, int id)
        {
            var action = repo.FindAction(id);
            if (repo.Actions().Where(a => a.CampaignId == action.CampaignId && a.Id != id).Count(a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) > 0)
            {
                string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture, "There is already an action with this name.");
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckUserEmailForEdit(string email, int userid)
        {
            if (repo.UserProfile().Where(u => u.UserId != userid).Count(u => u.Email.Equals(email)) > 0)
            {
                string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture, "This email is already taken.");
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckUserNameForEdit(string username, int userid)
        {
            if (repo.UserProfile().Where(u => u.UserId != userid).Count(u => u.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase)) > 0)
            {
                string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture, "This username is already taken.");
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckUserName(string username)
        {
            if (repo.UserProfile().Count(u => u.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase)) > 0)
            {
                string errormessage = String.Format(System.Globalization.CultureInfo.InvariantCulture, "This username is already taken.");
                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }

    }
}