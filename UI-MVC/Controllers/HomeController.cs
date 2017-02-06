using SC.UI.Web.MVC.App_GlobalResources;
using System.Web.Mvc;

namespace SC.UI.Web.MVC.Controllers
{
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