using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace DreamLeague.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString EmbedCss(this HtmlHelper htmlHelper, string path)
        {
            var cssFilePath = HttpContext.Current.Server.MapPath(path);

            try
            {
                var cssText = File.ReadAllText(cssFilePath);
                return htmlHelper.Raw("<style>\n" + cssText + "\n</style>");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}