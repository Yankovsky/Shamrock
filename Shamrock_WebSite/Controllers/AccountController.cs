using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Shamrock_WebSite.App_GlobalResources;
using System.Web;

namespace Shamrock_WebSite.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(string userName, string password, string returnUrl)
        {
            if (FormsAuthentication.Authenticate(userName, password))
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                return Redirect(returnUrl ?? Url.Action("Index", "Home"));
            }
            else
            {        
                ModelState.AddModelError("", Resource.InvalidLogin);
                return View();
            }
        }

        public ActionResult LogOff(string returnUrl)
        {
            FormsAuthentication.SignOut();
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            var culture = lang.ToLower() == SupportedCulture.En ? SupportedCulture.En : SupportedCulture.Ru;
            Session["Culture"] = new CultureInfo(culture);
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }
    }
}