using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Module;
using Meshop.Framework.Security;

namespace Meshop.Reviews.Controllers
{
    [Admin]
    [Menu(Name="Reviews")]
    public class AdminReviewsController : PluginController
    {


        //
        // GET: /AdminReviews/

        public ActionResult Index()
        {
            return View(new object());
        }


    }


}
