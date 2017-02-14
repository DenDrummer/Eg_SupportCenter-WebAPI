using SC.UI.Web.MVC.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;


namespace SC.UI.Web.MVC.Controllers
{
    [Internationalization]
    public class BaseController : Controller
    {
        public ActionResult CookieMonster()
        {

            //1) is er een cookie?
            HttpCookie aCookie = Request.Cookies["lang"];
            string lang = Server.HtmlEncode(aCookie.Value);

            if (lang != null)
            {
                //Ja
                //Read cookie
                Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }else
            {
                //neen 
                HttpCookie cookie = new HttpCookie("language");
                cookie.Value = lang;
                Response.Cookies.Add(cookie);
            }
           
            return Redirect("Index");
        }
        
    }
}