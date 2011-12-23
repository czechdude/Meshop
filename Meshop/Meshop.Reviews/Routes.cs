using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Meshop.Framework.Module;

namespace Meshop.Reviews
{
    public class Routes : IRoutes
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                 "Modules.Reviews", // Route name
                "reviews/{action}", // URL with parameters
                new { controller = "Reviews" ,action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute(
                 "Modules.Reviews.Admin", // Route name
                "admin/reviews/{action}", // URL with parameters
                new { controller = "AdminReviews", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
        }
    }
}
