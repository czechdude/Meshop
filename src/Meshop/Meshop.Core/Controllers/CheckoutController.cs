using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Model;
using Meshop.Framework.Security;
using Meshop.Framework.Services;

namespace Meshop.Core.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICart _cartService;

        public CheckoutController(ICart cart)
        {
            _cartService = cart;
        }
        
        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            _cartService.StartCart(ctx.HttpContext);

            var account = new AccountManagement();
            ViewBag.UserRole = account.IsInRole(User.Identity.Name, "Customer") ? "Customer" : "none";
        }
        
        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);
            try
            {
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                //Process the order
                _cartService.CreateOrder(order);
                return RedirectToAction("Complete", new {id = order.OrderID});
            }
            catch
            {
                //Invalid - redisplay with errors   
                return View(order);
            }
        }

        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            var isValid = _cartService.ValidateOrder(User.Identity.Name, id);
            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}