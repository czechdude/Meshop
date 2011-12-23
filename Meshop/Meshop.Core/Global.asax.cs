using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Meshop.Core.Models.DeleteInFuture;
using Meshop.Framework.ViewEngine;

namespace Meshop.Core
{

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            //first Plugins
            Framework.DI.Modules.RegisterRoutes(RouteTable.Routes);

            //then Core Areas
            AreaRegistration.RegisterAllAreas();

            //Universal route
            routes.MapRoute(
                            "Default", // Route name
                            "{controller}/{action}/{id}", // URL with parameters
                            new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                        );
        }


        protected void Application_Start()
        {
            
            Framework.DI.Modules.Initialize();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //Register custom Attribute Adapters           
            Framework.DI.Modules.RegisterAdapters();
   
            // Register Controller factory
            ControllerBuilder.Current.SetControllerFactory(Framework.DI.Modules.GetControllerFactory());

            // Register custom View Engine
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new MeshopViewEngine());

            //Database changed? refill with those data
            Database.SetInitializer(new DatabaseInitializer());
        }

        protected void Application_End()
        {
            Framework.DI.Modules.Dispose();
        }
    }
}