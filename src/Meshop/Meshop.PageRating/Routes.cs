using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Meshop.Framework.Module;

namespace Meshop.PageRating
{
    class Routes : IRoutes
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                 "Modules.PageRating", // Route name
                "pagerating/{action}", // URL with parameters
                new { controller = "PageRating", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
        }
    }
}
