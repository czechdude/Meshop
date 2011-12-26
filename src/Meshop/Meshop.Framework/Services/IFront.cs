using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meshop.Framework.Model;

namespace Meshop.Framework.Services
{
    /// <summary>
    /// Front web page services
    /// </summary>
    public interface IFront
    {
        IEnumerable<MenuItem> Menu();
        bool IsAdmin(System.Security.Principal.IPrincipal customer);


        IEnumerable<BasicCategory> CategoryTree();
    }

}
