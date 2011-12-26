using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Model;
using Meshop.Framework.Module;
using Meshop.Framework.Translation;
using Meshop.Framework.Widgets;
using Meshop.Reviews.Models;

namespace Meshop.Reviews.Controllers
{
    
    public class ReviewsController : PluginController
    {


        //
        // GET: /Reviews/

        [Menu(Name="Reviews")]
        public ActionResult Index()
        {
            var shit = db.Set<Review>().Include(r => r.Product).First();

            return View(shit);
        }

        [ChildActionOnly]
        [Placement(Position = PagePosition.Document, Template = PageTemplate.Index)]
        public ActionResult Widget()
        {
            var shit = db.Set<Review>().First();
            ViewBag.shit = shit.Customer.Email;
            return PartialView(shit);
        }
    }
}