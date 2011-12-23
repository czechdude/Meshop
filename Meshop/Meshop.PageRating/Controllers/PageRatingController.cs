using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Meshop.Framework.Module;


namespace Meshop.PageRating.Controllers
{
    
    public class PageRatingController : PluginController
    {
       
        [Menu(Name = "Page Rating")]
        public ActionResult Index()
        {
            ViewBag.Title = "Page Rating";
            return View();
        }

        //testing class
        public ActionResult Settings()
        {
            ViewBag.Title = "Settings";
            ViewBag.header = "Settings";

            return View(new SettingsViewModel());
        }
        [HttpPost]
        public ActionResult Settings(SettingsViewModel model)
        {
            return View(model);
        }

    }
}