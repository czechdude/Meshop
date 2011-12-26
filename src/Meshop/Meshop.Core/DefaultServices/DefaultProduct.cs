using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Xml.Linq;
using Castle.Components.DictionaryAdapter;
using Meshop.Framework.Model;
using Meshop.Framework.Services;

namespace Meshop.Core.DefaultServices
{
    public static class Extensions
    {

        // Y Combinator generic implementation
        private delegate Func<A, R> Recursive<A, R>(Recursive<A, R> r);
        private static Func<A, R> Y<A, R>(Func<Func<A, R>, Func<A, R>> f)
        {
            Recursive<A, R> rec = r => a => f(r(r))(a);
            return rec(rec);
        }

        public static IEnumerable<BasicCategory> Traverse(this IEnumerable<BasicCategory> source, Func<BasicCategory, bool> predicate)
        {
            var traverse = Extensions.Y<IEnumerable<BasicCategory>, IEnumerable<BasicCategory>>(
                f => items =>
                {
                    var r = new List<BasicCategory>(items.Where(predicate));
                    r.AddRange(items.SelectMany(i => f(i.Children)));
                    return r;
                });

            return traverse(source);
        }
    }


    public class DefaultProduct : IProduct
    {
        private DatabaseConnection2 _db = new DatabaseConnection2();

        public IEnumerable<BasicProduct> ProductsOfTheDay()
        {
            return _db.Products.ToList();
        }

        public BasicProduct Find(int id)
        {
            return _db.Products.Find(id);
        }

        public IEnumerable<BasicProduct> FindByCategory(int id)
        {
            var cat = _db.Categories.Find(id);
            return FindByCategory(cat);
        }

        public IEnumerable<BasicProduct> FindByCategory(BasicCategory cat)
        {
            var cats = GetSubCategories(cat);
            //!!! Include needs System.Data.Entity ns
             var prods = _db.Products.Include(p => p.Categories).AsEnumerable();
            return FilterByCategories(prods,cats);
            
        }

        private IEnumerable<BasicProduct> FilterByCategories(IEnumerable<BasicProduct> prods, IEnumerable<BasicCategory> cats)
        {
            var filtered = new List<BasicProduct>();
            foreach (var basicProduct in prods)
            {
                foreach (var cat in basicProduct.Categories)
                {
                    foreach (var basicCategory in cats)
                    {
                        if (basicCategory.CategoryID == cat.CategoryID)
                        {
                            filtered.Add(basicProduct);
                            break;
                        } 
                    }
                    if (filtered.Contains(basicProduct))
                    {
                        break;
                    }
                }
            }
            return filtered;
        }

        private IEnumerable<BasicCategory> GetSubCategories(BasicCategory cat)
        {
            var list = new List<BasicCategory>{cat};
            var q = _db.Categories.Include(c => c.Children).Where(c => c.CategoryID == cat.CategoryID).SingleOrDefault();

            if(q != null && q.Children != null)
            foreach (var ch in q.Children)
            {
                list.AddRange(GetSubCategories(ch));
            }
            return list;
        }

        public IEnumerable<BasicProduct> Random(int i)
        {
            return _db.Products.OrderBy(p => p.ProductID).Take(i).AsEnumerable();
        }

        public void Update(BasicProduct product)
        {
            _db.Entry(product).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}