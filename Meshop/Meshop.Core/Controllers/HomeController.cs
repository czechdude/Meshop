using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Meshop.Core.Models;
using Meshop.Framework.Model;
using Meshop.Framework.Module;
using Meshop.Framework.Security;
using Meshop.Framework.Services;
using Meshop.Framework.ViewEngine;
using Meshop.Framework.Widgets;

namespace Meshop.Core.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ICategory _categoryService;
        private readonly IProduct _productService;
        private readonly ICart _cartService;
        private readonly ICheckout _checkoutService;
        private readonly ICommon _commonService;

        public HomeController(ICategory catsvc,IProduct prodsvc,ICart cartsvc, ICheckout checkoutsvc, ICommon commonsvc)
        {
            _categoryService = catsvc;
            _productService = prodsvc;
            _cartService = cartsvc;
            _checkoutService = checkoutsvc;
            _commonService = commonsvc;
        }


        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            _cartService.StartCart(ctx.HttpContext);

            var account = new AccountManagement();
            ViewBag.UserRole = account.IsInRole(User.Identity.Name,"Customer")?"Customer":"none";
            ViewBag.Settings = _commonService.Settings;
        }

        [Menu(Name = "Home")]
        public ActionResult Index()
        {

            var products = _productService.ProductsOfTheDay();

            return View(new HomeViewModel(products as List<BasicProduct>));
        }

        [Menu(Name = "About")]
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Product(int id)
        {
            var product = _productService.Find(id);


            return View(product);
        }

        public ActionResult Category(int id)
        {
            var cat = _categoryService.Find(id);
            var products = _productService.FindByCategory(cat);
            var categories = _categoryService.FindSubCategories(cat);

            return View(new CategoryModel(cat,products,categories));
        }




        // GET: /ShoppingCart/
        public ActionResult Cart()
        {
            //_cartService.StartCart(HttpContext);
            // Set up our ViewModel
            var viewModel = new CartViewModel
            {
                CheckoutService = _checkoutService,
                CartItems = _cartService.GetCartItems(),
                CartTotal = _cartService.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            //_cartService.StartCart(HttpContext);
            _cartService.AddToCart(id);

            // Go back to the main store page for more shopping
            return RedirectToAction("Cart");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {

            // Get the name of the product to display confirmation
            string productName = _productService.Find(id).Name;

            // Remove from cart
            //_cartService.StartCart(HttpContext);
            int itemCount = _cartService.RemoveFromCart(id);

            // Display the confirmation message
            var results = new CartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) + " has been removed from your shopping cart.",
                CartTotal = _cartService.GetTotal(),
                CartCount = _cartService.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            //_cartService.StartCart(HttpContext);
            ViewData["CartCount"] = _cartService.GetCount();
            return PartialView("CartSummary");
        }






        [ChildActionOnly]
        [Placement(Position = PagePosition.Document, Template = PageTemplate.Index)]
        public ActionResult RandomProduct()
        {
            var product = _productService.Random(1);
            return PartialView(product);
        }


        // widget actions

        [ChildActionOnly]
        [Placement(Position=PagePosition.Document,Template = PageTemplate.Index)]
        public ActionResult TopSelled()
        {
            var product = _productService.Find(1);
            return PartialView(product);
        }
        
        [HttpPost]
        public ActionResult TopSelled(FormCollection collection)
        {
            var product = new BasicProduct();
            TryUpdateModel(product);
            if (ModelState.IsValid)
            {
                _productService.Update(product);
                return PartialView(product);
            }
            return PartialView(product);
        }

    }


}
