using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Module;
using Meshop.Framework.Services;

namespace Meshop.Core.Areas.Admin.Controllers
{
    [Menu(Name = "Orders")]
    public class OrdersController : AdminController
    {
        //
        // GET: /Admin/Orders/

        public OrdersController(IAdmin adminsvc, ICommon commonsvc) : base(adminsvc, commonsvc)
        {
        }

        public ActionResult Index()
        {
            var orders = db.Orders.Select(o => o).ToList();

            return View(orders);
        }

        //
        // GET: /Admin/Orders/Details/5

        public ActionResult Details(int id)
        {
            var order = db.Orders.Find(id);
            db.Entry(order).Collection(x => x.OrderDetails).Load();
            return View(order);
        }

 

        //
        // GET: /Admin/Orders/Delete/5
 
        public ActionResult Delete(int id)
        {
            var order = db.Orders.Find(id);
            return View(order);
        }

        //
        // POST: /Admin/Orders/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
