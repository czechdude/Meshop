using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Meshop.Framework.DI;

namespace Meshop.Framework.ViewEngine
{
    public class MeshopViewEngine : RazorViewEngine
    {
        public MeshopViewEngine() 
            : this(null) { 
        }
 
        
        public MeshopViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator) 
        {
            AreaMasterLocationFormats = new string[]
                                            {
                                                "~/Areas/{2}/Views/{1}/{0}.cshtml",                                              
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml"
                                            };
            AreaViewLocationFormats = AreaMasterLocationFormats;
            AreaPartialViewLocationFormats = AreaMasterLocationFormats;

            MasterLocationFormats = new string[]
                                        {
                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/Shared/{0}.cshtml"//,
                                            //Modules.Path + ModuleDirs + "/Views/{1}/{0}.cshtml"
                                        };
            ViewLocationFormats = MasterLocationFormats;
            PartialViewLocationFormats = MasterLocationFormats;
        }
    }
}