using System.Web.Mvc;

namespace Shamrock_WebSite.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}