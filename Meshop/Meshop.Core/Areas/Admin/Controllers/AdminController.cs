using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Model;
using Meshop.Framework.Security;
using Meshop.Framework.Services;

namespace Meshop.Core.Areas.Admin.Controllers
{
    [Admin]
    public abstract class AdminController : Controller
    {
        protected readonly IAdmin _adminService;
        protected readonly ICommon _commonService;
        protected DatabaseConnection2 db = new DatabaseConnection2();

        protected AdminController(IAdmin adminsvc,ICommon commonsvc)
        {
            _adminService = adminsvc;
            _commonService = commonsvc;
            _commonService.TempData = TempData;
        }

        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);

            ViewBag.Settings = _commonService.Settings;
        }

    }
}
