using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SC.UI.Web.MVC.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
       /*public ActionResult SetLanguage(string name)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(name);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            HttpContext.Current.Session["culture"] = name;
        }*/
    }
}