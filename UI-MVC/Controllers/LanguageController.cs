
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SC.UI.Web.MVC.Controllers
{
    public class LanguageController : BaseController
    {
        public ActionResult ChangeLang(string newLang)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(newLang);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            HttpCookie cookie = new HttpCookie("lang");
            cookie.Value = newLang;
            cookie.Expires = DateTime.Now.AddDays(14);
            HttpContext.Response.SetCookie(cookie);

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
    }
}
