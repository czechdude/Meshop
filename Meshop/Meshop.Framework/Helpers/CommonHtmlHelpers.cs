using System.Globalization;
using System.Web.Mvc;

namespace Meshop.Framework.Helpers
{
    public static class CommonHtmlExtensions
    {
        
        public static object GetGlobalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey)
        {
            return htmlHelper.ViewContext.HttpContext.GetGlobalResourceObject(classKey, resourceKey);
        }

        public static object GetGlobalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey, CultureInfo culture)
        {
            return htmlHelper.ViewContext.HttpContext.GetGlobalResourceObject(classKey, resourceKey, culture);
        }

        public static object GetLocalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey)
        {
            return htmlHelper.ViewContext.HttpContext.GetLocalResourceObject(classKey, resourceKey);
        }

        public static object GetLocalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey, CultureInfo culture)
        {
            return htmlHelper.ViewContext.HttpContext.GetLocalResourceObject(classKey, resourceKey, culture);
        }



    }
}