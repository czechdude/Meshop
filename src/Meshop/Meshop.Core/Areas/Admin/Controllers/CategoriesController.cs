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

namespace Meshop.Core.Areas.Admin.Controllers
{ 
    [Menu(Name = "Categories")]
    public class CategoriesController : AdminController
    {
        


        public CategoriesController(IAdmin adminsvc,ICommon commonsvc) : base(adminsvc,commonsvc)
        {
        }

        private void BuildTree(BasicCategory category, List<BasicCategory> root)
        {
            category.Children = new List<BasicCategory>();
            
            
            foreach (BasicCategory basicCategory in root.FindAll(c => c.Parent !=null && c.Parent.CategoryID == category.CategoryID))
            {
                BuildTree(basicCategory, root);
                category.Children.Add(basicCategory);
            }
        }

        private List<BasicCategory> GetCategoryTree()
        {
            var root = db.Categories.ToList();
            var tree = new List<BasicCategory>();

            foreach (BasicCategory basicCategory in root.FindAll(c => c.Parent == null))
            {
                BuildTree(basicCategory, root);
                tree.Add(basicCategory);
            }

            
            return tree;
        }

        private List<BasicCategory> GetLinearCategoryTree()
        {

            var root = db.Categories.ToList();
            var tree = new List<BasicCategory>();

            foreach (BasicCategory basicCategory in root.FindAll(c => c.Parent == null))
            {
                basicCategory.Name = "- " + basicCategory.Name;
                tree.Add(basicCategory);
                tree.AddRange(BuildLinearTree(basicCategory, root,2));

            }

            tree.Reverse();

            return tree;
        }

        private IEnumerable<BasicCategory> BuildLinearTree(BasicCategory category, List<BasicCategory> root, int level)
        {
            var children = new List<BasicCategory>();


            foreach (BasicCategory basicCategory in root.FindAll(c => c.Parent != null && c.Parent.CategoryID == category.CategoryID))
            {
                basicCategory.Name = new string('-',level) + ' ' + basicCategory.Name;
                children.Add(basicCategory);
                children.AddRange(BuildLinearTree(basicCategory, root,level + 1));
            }

            return children;
        }


        private void PopulateCategoriesSelectList( BasicCategory selectedCategory)
        {

            var selCatInt = 0;

            if (selectedCategory != null) selCatInt = selectedCategory.CategoryID;

            var stack = new Stack<BasicCategory>(GetLinearCategoryTree());
            stack.Push(new BasicCategory(){Name = "Root"});
            var select = new SelectList(stack, "CategoryID", "Name", selCatInt);
            ViewBag.SelectCategory = select;
        }

        //
        // JSON: /Admin/Move/?nodeId=5&position=after&targetId=2

        public ActionResult Move(int nodeId, string position, int targetId)
        {
            string result;
            
            var node = db.Categories.Include(p =>p.Parent).Where(c => c.CategoryID == nodeId).Single();
            var target = db.Categories.Include(p =>p.Parent).Where(c => c.CategoryID == targetId).Single();

            var par = target.Parent;
            
            try
            {
                //UpdateModel(node);

                switch (position)
                {
                    case "after":
                        //dela bordel pri posunu z podkategorie s childs>1 do rootu
                       
                        node.Parent = par;
                        break;
                    case "last":
                        //okay
                        node.Parent = target;
                        break;
                    case "before":
                        //dela bordel pri posunu z podkategorie s childs>1 do rootu
                        
                        node.Parent = par;
                        break;
                    default:
                        throw new DataException("Position " + position + " not implemented!");
                        
                }

                db.Entry(node).State = EntityState.Modified;
                db.SaveChanges();
                result = "OK";
            }
            catch (DataException e)
            {

                result = e.Message;
            }
            return Json(new {result,nodeId,position,targetId},JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Admin/Categories/

        public ViewResult Index()
        {
            

            

            return View(GetCategoryTree());
        }

        //
        // GET: /Admin/Categories/Details/5

        public ViewResult Details(int id)
        {
            BasicCategory basiccategory = db.Categories.Find(id);
            return View(basiccategory);
        }

        //
        // GET: /Admin/Categories/Create

        public ActionResult Create(int? id)
        {
            var category = db.Categories.Find(id);
            PopulateCategoriesSelectList(category);
            return View();
        }

 

        //
        // POST: /Admin/Categories/Create

        [HttpPost]
        public ActionResult Create(BasicCategory basiccategory, string parentVal)
        {

            try
            {
                if (ModelState.IsValid && parentVal != null)
                {
   
                    if(Convert.ToInt32(parentVal) > 0)
                        basiccategory.Parent = db.Categories.Find(Convert.ToInt32(parentVal));
 
                    db.Categories.Add(basiccategory);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException e)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator. (" + e.Message + ")");
            }
            PopulateCategoriesSelectList(basiccategory.Parent);
            return View(basiccategory);
        }
        
        //
        // GET: /Admin/Categories/Edit/5
 
        public ActionResult Edit(int id)
        {
            BasicCategory basiccategory = db.Categories.Find(id);
            return View(basiccategory);
        }

        //
        // POST: /Admin/Categories/Edit/5

        [HttpPost]
        public ActionResult Edit(BasicCategory basiccategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basiccategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(basiccategory);
        }

        //
        // GET: /Admin/Categories/Delete/5
 
        public ActionResult Delete(int id)
        {
            BasicCategory basiccategory = db.Categories.Find(id);
            return View(basiccategory);
        }

        //
        // POST: /Admin/Categories/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            BasicCategory basiccategory = db.Categories.Find(id);
            db.Categories.Remove(basiccategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}