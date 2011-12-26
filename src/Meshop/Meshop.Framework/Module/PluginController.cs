using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;
using Meshop.Framework.DI;
using Meshop.Framework.Model;
using Meshop.Framework.Translation;

namespace Meshop.Framework.Module
{
    public abstract class PluginController : Controller
    {
 
        protected DatabaseConnection2 db = new DatabaseConnection2();

        /**
         * This series of custom PartialView overloads maps the controller View file in the plugin structure ModuleName/Views/ControllerName/ViewName.cshtml
        */
        public new PartialViewResult PartialView()
        {
            return base.PartialView(ConstructViewPath());
        }

        public new PartialViewResult PartialView(object model)
        {
            return base.PartialView(ConstructViewPath(), model);
        }

        //concats the path to the PartialView in plugin structure
        public new PartialViewResult PartialView(string viewName, object model)
        {

            return base.PartialView(ConstructViewPath(viewName), model);
        }



        /**
         * This series of custom View overloads maps the controller View file in the plugin structure ModuleName/Views/ControllerName/ViewName.cshtml
        */
        public new ViewResult View()
        {
            return base.View(ConstructViewPath());
        }

        public new ViewResult View(object model)
        {
            return base.View(ConstructViewPath(), model);
        }

        //concats the path to the view in plugin structure
        public new ViewResult View(string viewName, object model)
        {
            
            return base.View(ConstructViewPath(viewName), model);
        }

        private string ConstructViewPath(string viewName = "")
        {
            string viewPath = Modules.Path + GetAssemblyName() +"/";
           
            string ctrler = (string)RouteData.Values["Controller"];
            if (ctrler == null) throw new NotSupportedException("Controller value empty, cannot locate module View file.");

            if (viewName == "") viewName = (string)RouteData.Values["Action"];
            if (viewName == null) throw new NotSupportedException("Action value empty, cannot locate module View file.");

            viewPath += "Views/" + ctrler  + "/" + viewName + ".cshtml";

            return viewPath;
        }

        protected virtual string GetAssemblyName()
        {
            return Assembly.GetAssembly(GetType()).GetName().Name;
        }


        
    }

   


    
}