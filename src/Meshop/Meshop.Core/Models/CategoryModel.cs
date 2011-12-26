using System.Collections.Generic;
using Meshop.Framework.Model;

namespace Meshop.Core.Models
{
    public class CategoryModel
    {
        public BasicCategory Category { get; set; }
        public IEnumerable<BasicCategory> Categories { get; set; }
        public IEnumerable<BasicProduct> Products { get; set; } 
        public CategoryModel(BasicCategory c,IEnumerable<BasicProduct> p, IEnumerable<BasicCategory>  cs )
        {
            Categories = cs;
            Category = c;
            Products = p;
        }
    }
}