
namespace Meshop.Framework.Helpers
{
    /// <summary>
    /// All helpers CSHTML need to '@inherits HtmlPage'
    /// </summary>
    public class HelperPage : System.Web.WebPages.HelperPage
    {
        /// <summary>
        /// Workaround - exposes the MVC HtmlHelper instead of the normal helper
        /// </summary>
        public static new System.Web.Mvc.HtmlHelper Html
        {
            get { return ((System.Web.Mvc.WebViewPage)System.Web.WebPages.WebPageContext.Current.Page).Html; }
        }
    }
}