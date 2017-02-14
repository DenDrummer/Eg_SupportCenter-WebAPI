using SC.UI.Web.MVC.App_GlobalResources;
using SC.UI.Web.MVC.Helpers;
using System.Web.Mvc;

namespace SC.UI.Web.MVC.Controllers
{
    public class HomeController : BaseController
    {
        public virtual ActionResult Index()
        {
            /*
             * 1) is cookie?
             * 2) JA :
             * 2.1) read cookie language setting and applay
             * 2) NEE:
             * 2.2) choose language:
             * 2.2.1) browser, hardcoded, web.config.....
             * */
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