using System.Collections;
using System.Collections.Generic;
using Meshop.Framework.Model;

namespace Meshop.Framework.Services
{
    /**
     * Interface for Dependency injection 
     * for category methods
     */
    public interface ICategory
    {
        IEnumerable CategoryTree();

        BasicCategory Find(int id);
        IEnumerable<BasicCategory> FindSubCategories(BasicCategory cat);
    }

}
