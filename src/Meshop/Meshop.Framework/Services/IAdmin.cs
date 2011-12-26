using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meshop.Framework.Services
{
    /// <summary>
    /// Admin services
    /// </summary>
    public interface IAdmin
    {
        IEnumerable<MenuItem> Menu();
    }

    public class MenuItem
    {
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Title { get; set; }
        public string Action { get; set; }
    }

}
