using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Meshop.Framework.Module;
using Meshop.Framework.Translation;

namespace Meshop.Framework.DI
{
    public static class Modules
    {

        public static string Path { get { return "~/extensions/temp/"; } }
        
        // 
        private static readonly IWindsorContainer _container = new WindsorContainer();

        private static IEnumerable<Type> _serviceIfaces;

        public static HashSet<Type> Controllers { get; private set; }

        public static IWindsorContainer Container { get { return _container; } }

        public static void Initialize()
        {
            Controllers = new HashSet<Type>();

            // register 'normal' controllers
            _container.Register(AllTypes.FromAssemblyNamed("Meshop.Core").BasedOn<IController>().If(t => t.Name.EndsWith("Controller")).Configure(c => c.LifeStyle.Transient));



            // location of the plugins
            var allPluginsDir = new DirectoryInfo(HostingEnvironment.MapPath(Path));

            foreach (var dir in allPluginsDir.GetDirectories())
            {
                //"~/extensions/temp/{0}/"
                //string pluginDir = string.Format(Path + "{0}/", dir.Name);

                // loop through all dll files
                foreach (var dll in dir.GetFiles("*.dll"))
                {
                    var assemblyTypes = Assembly.LoadFrom(dll.FullName).GetTypes();

                    // MANY controllers per plugin
                    var controllerTypeList = assemblyTypes.Where(t => typeof(PluginController).IsAssignableFrom(t)).AsEnumerable();
                    foreach (Type controllerType in controllerTypeList)
                    {
                        if (controllerType == null) continue;

                        // register controller
                        _container.Register(Component.For(controllerType).LifeStyle.Transient);
                        Controllers.Add(controllerType);
                    }


                    //Scanning loaded assembly for implementation of services
                    ScanAssemblyForImplementations(assemblyTypes);

                    //Scan assembly for database Seed
                    ScanAssemblyForDbSeed(assemblyTypes);
                }
            }

            //Default service implementation registration from Meshop.Core!
            var executingAssemblyTypes = Assembly.LoadFrom(HostingEnvironment.MapPath("~/bin/Meshop.Core.dll")).GetTypes();
            ScanAssemblyForImplementations(executingAssemblyTypes);
            ScanAssemblyForControllers(executingAssemblyTypes);
        }

        private static void ScanAssemblyForControllers(IEnumerable<Type> assemblyTypes)
        {
            foreach (var type in assemblyTypes.Where(t => typeof(IController).IsAssignableFrom(t)))
            {
                Controllers.Add(type);
            }
        }

        private static void ScanAssemblyForDbSeed(IEnumerable<Type> assemblyTypes)
        {
            var type = assemblyTypes.Where(t=> typeof(IDbSeed).IsAssignableFrom(t)).SingleOrDefault();
            if (type != null)
                _container.Register(Component.For(type).LifeStyle.Transient);
        }

        public static IEnumerable<Type> ListOfServices(string servicesNamespace = "Meshop.Framework.Services")
        {
            if(_serviceIfaces != null) 
                return _serviceIfaces;

            return _serviceIfaces = Assembly.GetExecutingAssembly()
                                                .GetTypes()
                                                .Where(type => type.IsInterface && type.Namespace == servicesNamespace)
                                                .AsEnumerable();
        }

        private static void ScanAssemblyForImplementations(IEnumerable<Type> assemblyTypes)
        {
            foreach (Type service in ListOfServices())
            {
                Type service1 = service;
                var type = assemblyTypes.Where(service1.IsAssignableFrom).SingleOrDefault();
                if (type != null)
                    _container.Register(Component.For(service1).ImplementedBy(type).LifeStyle.Transient);
            }
        }

        public static IControllerFactory GetControllerFactory()
        {
            return new ControllerFactory(_container);
        }

        public static void Dispose()
        {
            _container.Dispose();
        }

        

        public static void RegisterRoutes(RouteCollection routeCollection)
        {
            // location of the plugins
            var allPluginsDir = new DirectoryInfo(HostingEnvironment.MapPath(Path));
            var routes = new List<IRoutes>();
            
            foreach (var dir in allPluginsDir.GetDirectories())
            {

                foreach (var dll in dir.GetFiles("*.dll"))
                {
                    var assembly = Assembly.LoadFrom(dll.FullName);
                
                    var routesType = assembly.GetTypes().Where(t => typeof(IRoutes).IsAssignableFrom(t)).SingleOrDefault();

                    if (routesType == null) continue;

                    var route = Activator.CreateInstance(routesType) as IRoutes;
                    routes.Add(route);
                }
            }


            routes.ForEach(x => x.RegisterRoutes(routeCollection));
        }

        public static void InjectSeed(DbContext context)
        {
            var seeds = _container.ResolveAll<IDbSeed>().ToList();
            foreach (var seed in seeds)
            {
                seed.Plant(context);
            }
        }

        public static void RegisterAdapters()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(TranslateRequiredAttribute), typeof(TranslateRequiredAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(TranslateRegularExpressionAttribute), typeof(TranslateRegularExpressionAttributeAdapter));
        }
    }
}
