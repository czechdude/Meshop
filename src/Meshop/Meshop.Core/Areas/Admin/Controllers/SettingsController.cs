using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Model;
using Meshop.Framework.Module;
using Meshop.Framework.Services;

namespace Meshop.Core.Areas.Admin.Controllers
{
    [Menu(Name = "Settings")]
    public class SettingsController : AdminController
    {
        
        // GET: /Admin/Settings/

        public SettingsController(IAdmin adminsvc,ICommon commonsvc) : base(adminsvc,commonsvc)
        {
        }

        public ActionResult Index()
        {
            var list = db.Settings.ToList();
            return View(list);
        }


        [HttpPost]
        public ActionResult Index(IList<Setting> settings,FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                foreach (var setting in settings)
                {
                    db.Entry(setting).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                _commonService.Message = "Success!";
                return RedirectToAction("Index");
            }
            return View(settings);
        }

    }
}
