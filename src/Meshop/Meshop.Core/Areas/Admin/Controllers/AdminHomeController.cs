using System.Web.Mvc;
using Meshop.Framework.Module;
using Meshop.Framework.Security;
using Meshop.Framework.Services;

namespace Meshop.Core.Areas.Admin.Controllers
{
    
    [Menu(Name = "Index")]
    public class AdminHomeController : AdminController
    {


        public AdminHomeController(IAdmin adminsvc,ICommon commonsvc) : base(adminsvc,commonsvc)
        {
            
        }

        //
        // GET: /Admin/AdminHome/
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

    }
}
