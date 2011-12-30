using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Module;

namespace Meshop.Project1.Controllers
{
    public class Project1Controller : PluginController
    {
        //
        // GET: /Project1/


        [Menu(Name="Project 1")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
