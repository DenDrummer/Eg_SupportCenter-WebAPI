using SC.UI.Web.MVC.Helpers;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace SC.UI.Web.MVC.Controllers
{
    public class LanguageController : BaseController
    {
        public ActionResult ChangeLang(string newLang)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(newLang);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            Response.Cookies["lang"].Value = newLang;
            Response.Cookies["lang"].Expires = DateTime.Now.AddDays(14);

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

        public ActionResult LoadLang()
        {
            var r = Request;
            if (r != null)
            {
                string lang = r.Cookies["lang"].Value;
                if (lang != null)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                }
                var urlR = r.UrlReferrer;
                if (urlR != null)
                {
                    string uri = urlR.PathAndQuery;
                    //string uri = urlR.OriginalString;
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
                }
                var rawUrl = r.RawUrl;
                if (rawUrl != null)
                {
                    string newUri = $"{rawUrl}{lang}";
                    return Redirect(newUri);
                }
            }
            return View();
        }
    }
}
