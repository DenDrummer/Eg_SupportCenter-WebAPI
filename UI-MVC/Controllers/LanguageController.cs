using SC.UI.Web.MVC.Helpers;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SC.UI.Web.MVC.Controllers
{
    [Internationalization]
    public class LanguageController : Controller
    {
        public ActionResult ChangeLang(string newLang)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(newLang);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            /*Response.Cookies["lang"].Value = newLang;
            Response.Cookies["lang"].Expires = DateTime.Now.AddDays(14);*/
            HttpCookie cookie = new HttpCookie("cookie");
            cookie.Value = newLang;
            cookie.Expires = DateTime.Now.AddDays(14);
            Response.Cookies.Add(cookie);
            
            //Settings s = new Settings();
            //s.Language = newLang;
            //s.Save();
            string uri = Request.UrlReferrer.PathAndQuery;
            string[] uriParams = uri.Split('/');
            uriParams[1] = newLang;
            StringBuilder newUri = new StringBuilder();
            for (int i = 0; i < uriParams.Length; i++)
            {
                newUri.Append(uriParams[i]);
                if(i != uriParams.Length - 1)
                {
                    newUri.Append('/');
                }
            }
            return Redirect(newUri.ToString());
        }

        /*public ActionResult LoadLang()
        {
            string lang = Request.Cookies["lang"].Value;
            if (lang != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
            var r = Request;
            var urlR = r.UrlReferrer;
            string uri = urlR.PathAndQuery;//.OriginalString;
            string[] uriParams = uri.Split('/');
            StringBuilder newUri = new StringBuilder();
            if (!lang.Equals(uriParams[1]))
            {
                uriParams[1] = lang;
                for (int i = 0; i < uriParams.Length; i++)
                {
                    newUri.Append(uriParams[i]);
                    if (i != uriParams.Length - 1)
                    {
                        newUri.Append('/');
                    }
                }
                return Redirect(newUri.ToString());
            }
            return Redirect(uri);
        }*/

        private void readCookie()
        {
            if(Request.Cookies["cookie"].Value != null) { 
            var cookieValue = Request.Cookies["cookie"].Value;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cookieValue);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
        }
    }
}
