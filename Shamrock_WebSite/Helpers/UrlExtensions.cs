using System.Web.Mvc;

namespace Shamrock_WebSite.Helpers
{
    public static class UrlHelperExtension
    {
        public static string Home(this UrlHelper helper)
        {
            return helper.Content("~/");
        }
    }
}