using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Meshop.Framework.Module
{
    public interface IDbSeed
    {
        void Plant(DbContext context);
    }
}
