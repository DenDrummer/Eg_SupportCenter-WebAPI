using SC.UI.Web.MVC.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SC.UI.Web.MVC.Controllers
{
    [Internationalization]
    public class BaseController : Controller
    {
        public ActionResult Index()
        {
            /*
             * 1) is cookie?
             * 2) JA :
             * 2.1) read cookie language setting and apply
             * 2) NEE:
             * 2.2) choose language:
             * 2.2.1) browser, hardcoded, web.config.....
             * */
            Regex langRegex = new Regex("[a-z][a-z]-[A-Z][A-Z]");
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
                    string uri2 = urlR.OriginalString;
                    string[] uriParams = uri.Split('/');
                    //string[] uriParams2 = uri2.Split('/');
                    StringBuilder newUri = new StringBuilder();
                    int? langLoc = null;
                    for (int i = 0; i < uriParams.Length; i++)
                    {
                        if (langRegex.IsMatch(uriParams[i]))
                        {
                            langLoc = i;
                        }
                    }
                    if (langLoc != null)
                    {
                        for (int j = 0; j < uriParams.Length; j++)
                        {
                            if (j == langLoc)
                            {
                                newUri.Append(lang);
                            }
                            else
                            {
                                newUri.Append(uriParams[j]);
                            }
                            if (j < uriParams.Length - 1)
                            {
                                newUri.Append('/');
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < uriParams.Length + 1; j++)
                        {
                            if (j == 1)
                            {
                                newUri.Append(lang);
                            }
                            else if (j < 1)
                            {
                                newUri.Append(uriParams[j]);
                            }
                            else
                            {
                                newUri.Append(uriParams[j - 1]);
                            }
                            if (j < uriParams.Length)
                            {
                                newUri.Append('/');
                            }
                        }
                    }
                    return Redirect(newUri.ToString());
                }
                var rawUrl = r.RawUrl;
                var url = r.Url;
                var path = r.Path;
                if (rawUrl != null)
                {
                    string newUri = $"{url.Authority}/{lang}{rawUrl}";
                    return Redirect(newUri);
                }
            }
            return View();
        }
    }
}