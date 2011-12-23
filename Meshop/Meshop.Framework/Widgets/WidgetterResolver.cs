using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Meshop.Framework.Widgets
{
    public static class WidgetterResolver
    {
        public static Widgetter Resolve(HtmlHelper html)
        {
            var defaultWidgetter = new DefaultWidgetter(html);
            return defaultWidgetter.Compose;
        }
    }
}
