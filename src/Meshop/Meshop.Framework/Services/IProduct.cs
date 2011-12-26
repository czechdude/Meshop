using System.Collections;
using System.Collections.Generic;
using Meshop.Framework.Model;

namespace Meshop.Framework.Services
{
    /**
     * Interface for Dependency injection 
     * of Products 
     */
    public interface IProduct
    {
        IEnumerable<BasicProduct> ProductsOfTheDay();
        BasicProduct Find(int id);
        IEnumerable<BasicProduct> FindByCategory(int id);
        IEnumerable<BasicProduct> FindByCategory(BasicCategory cat);           
        IEnumerable<BasicProduct> Random(int i);
        void Update(BasicProduct product);
    }
}
