using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Meshop.Framework.Module
{
    public class MenuAttribute : Attribute
    {

        private string _name;

        public string Name
        {
            get { return (string)HttpContext.GetGlobalResourceObject("Global", _name); }
            set { _name = value; }
        }

    }
}
