
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
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
            cookie.Expires = DateTime.Now.AddDays(7);
            HttpContext.Response.SetCookie(cookie);

            string uri = Request.UrlReferrer.PathAndQuery;
            string[] uriParams = uri.Split('/');
            StringBuilder newUri = new StringBuilder();
            newUri.Append(uriParams[0] + '/');
            newUri.Append(newLang + '/');
            Regex langRegex = new Regex("^[a-z]{2}-[A-Z]{2}");
            if (langRegex.IsMatch(uriParams[1]))
            {
                for (int i = 2; i < uriParams.Length; i++)
                {
                    newUri.Append(uriParams[i]);
                    if (i != uriParams.Length - 1)
                    {
                        newUri.Append('/');
                    }
                }
            }
            else
            {
                for (int i = 1; i < uriParams.Length; i++)
                {
                    newUri.Append(uriParams[i]);
                    if (i != uriParams.Length - 1)
                    {
                        newUri.Append('/');
                    }
                }
            }
            return Redirect(newUri.ToString());
        }
    }
}