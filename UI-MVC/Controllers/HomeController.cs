using SC.UI.Web.MVC.App_GlobalResources;
using SC.UI.Web.MVC.Helpers;
using System.Web.Mvc;

namespace SC.UI.Web.MVC.Controllers
{
    [Internationalization]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = Resources.AboutMessage;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = Resources.ContactMessage;

            return View();
        }
    }
}