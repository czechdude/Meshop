using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Model;
using Meshop.Framework.Services;

namespace Meshop.Core.DefaultServices
{
    public class DefaultCategory : ICategory
    {
        private DatabaseConnection2 _db = new DatabaseConnection2();

        private void BuildTree(BasicCategory category, List<BasicCategory> root)
        {
            category.Children = new List<BasicCategory>();


            foreach (BasicCategory basicCategory in root.FindAll(c => c.Parent != null && c.Parent.CategoryID == category.CategoryID))
            {
                BuildTree(basicCategory, root);
                category.Children.Add(basicCategory);
            }
        }

        public IEnumerable CategoryTree()
        {
            var root = _db.Categories.ToList();
            var tree = new List<BasicCategory>();

            foreach (BasicCategory basicCategory in root.FindAll(c => c.Parent == null))
            {
                BuildTree(basicCategory, root);
                tree.Add(basicCategory);
            }


            return tree;
        }

        public BasicCategory Find(int id)
        {
            return _db.Categories.Find(id);
        }

        public IEnumerable<BasicCategory> FindSubCategories(BasicCategory cat)
        {
            return GetSubCategories(cat);
        }

        private IEnumerable<BasicCategory> GetSubCategories(BasicCategory cat)
        {
            var list = new List<BasicCategory> ();
            var q = _db.Categories.Include(c => c.Children).Where(c => c.CategoryID == cat.CategoryID).SingleOrDefault();

            if (q != null && q.Children != null)
                foreach (var ch in q.Children)
                {
                    list.Add(ch);
                    list.AddRange(GetSubCategories(ch));
                }
            return list;
        }
    }
}
