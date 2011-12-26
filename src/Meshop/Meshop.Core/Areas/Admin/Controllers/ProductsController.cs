using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Meshop.Framework.Model;
using Meshop.Framework.Module;
using Meshop.Framework.Security;
using Meshop.Framework.Services;
using PagedList;

namespace Meshop.Core.Areas.Admin.Controllers
{ 
    
    [Menu(Name = "Products")]
    public class ProductsController : AdminController
    {
        

        //
        // GET: /Admin/Products/

        public ProductsController(IAdmin adminsvc,ICommon commonsvc) : base(adminsvc,commonsvc)
        {
        }

        public ViewResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentFilter = searchString;
            
            var products = from p in db.Products.Include(c => c.Categories)
                           select p;



            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "Name desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            int pageSize = 2;
            int pageIndex = page ?? 1;

            return View(products.ToPagedList(pageIndex,pageSize));
        }

        //
        // GET: /Admin/Products/Details/5

        public ViewResult Details(int id)
        {
            BasicProduct product = db.Products.Find(id);
            return View(product);
        }

        //
        // GET: /Admin/Products/Create

        public ActionResult Create()
        {

            PopulateCategoriesMultiSelectList();
            return View();
        }

        private void PopulateCategoriesMultiSelectList(IEnumerable<BasicCategory> selectedCategories = null)
        {
            var query = from c in db.Categories
                                   orderby c.Name
                                   select c;
            
            IEnumerable<int> selectedInts = null;

            if (selectedCategories != null)
            {
                selectedInts = selectedCategories.Select(c => c.CategoryID);

            }

            var multi = new MultiSelectList(query, "CategoryID", "Name", selectedInts);
            ViewBag.MultiCategory = multi;
        }

        //
        // POST: /Admin/Products/Create

        [HttpPost]
        public ActionResult Create(BasicProduct product,string[] selectedCategories)
        {
            try
            {
                if (ModelState.IsValid && selectedCategories != null)
                {
                    
                    product.Categories = new List<BasicCategory>();
                        
                    var selectedCategoriesHS = new HashSet<string>(selectedCategories);
                       
                    foreach (var category in db.Categories)
                    {
                        if (selectedCategoriesHS.Contains(category.CategoryID.ToString()))
                        {
                               
                            product.Categories.Add(category);
                        }
                    }          

                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(DataException e)
            {           
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator. (" + e.Message +")");
            }
            PopulateCategoriesMultiSelectList(product.Categories);
            return View(product);
        }
        
        //
        // GET: /Admin/Products/Edit/5
 
        public ActionResult Edit(int id)
        {
            BasicProduct product = db.Products.Find(id);
            db.Entry(product).Collection(x => x.Categories).Load();
            PopulateCategoriesMultiSelectList(product.Categories);
            return View(product);
        }

        //
        // POST: /Admin/Products/Edit/5

        [HttpPost]
        public ActionResult Edit(int id,FormCollection formCollection,string[] selectedCategories)
        {

            var productToUpdate = db.Products
                                    .Include(c => c.Categories)
                                    .Where(p => p.ProductID == id)
                                    .Single();

            try
            {
                

                UpdateModel(productToUpdate, "", null, new string[] {"Categories"});

                if (selectedCategories == null)
                {
                    productToUpdate.Categories = new List<BasicCategory>();

                }
                else
                {
                    var selectedCategoriesHS = new HashSet<string>(selectedCategories);
                    var productCategories = new HashSet<int>
                        (productToUpdate.Categories.Select(c => c.CategoryID));
                    foreach (var category in db.Categories)
                    {
                        if (selectedCategoriesHS.Contains(category.CategoryID.ToString()))
                        {
                            if (!productCategories.Contains(category.CategoryID))
                            {
                                productToUpdate.Categories.Add(category);
                            }
                        }
                        else
                        {
                            if (productCategories.Contains(category.CategoryID))
                            {
                                productToUpdate.Categories.Remove(category);

                            }
                        }
                    }

                }



                db.Entry(productToUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    
            }
            catch (DataException e)
            {

                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator. (" + e.Message + ")");
            }

            PopulateCategoriesMultiSelectList(productToUpdate.Categories);
            return View(productToUpdate);
        }

        //
        // GET: /Admin/Products/Delete/5
 
        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            BasicProduct product = db.Products.Find(id);
            return View(product);
        }

        //
        // POST: /Admin/Products/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                BasicProduct product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
              
            }
            catch (DataException)
            {
                return RedirectToAction("Delete",
                    new System.Web.Routing.RouteValueDictionary { 
                        { "id", id }, 
                        { "saveChangesError", true } });
                
            }
              return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}