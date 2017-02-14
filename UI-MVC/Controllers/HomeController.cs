using SC.UI.Web.MVC.App_GlobalResources;
using System.Web.Mvc;

namespace SC.UI.Web.MVC.Controllers
{

    public class HomeController : BaseController
    {
      
        public ActionResult Index()
        {
            CookieMonster();
            return View();
        }

        public ActionResult About()
        {
            CookieMonster();
            ViewBag.Message = Resources.AboutMessage;

            return View();
        }

        public ActionResult Contact()
        {
            CookieMonster();
            ViewBag.Message = Resources.ContactMessage;

            return View();
        }
    }
}