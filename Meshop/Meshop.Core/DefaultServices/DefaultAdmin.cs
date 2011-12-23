using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using Meshop.Framework.DI;
using Meshop.Framework.Model;
using Meshop.Framework.Module;
using Meshop.Framework.Security;
using Meshop.Framework.Services;
using MenuItem = Meshop.Framework.Services.MenuItem;

namespace Meshop.Core.DefaultServices
{
    public class DefaultAdmin : IAdmin
    {
        private DatabaseConnection2 _db = new DatabaseConnection2();

        public IEnumerable<MenuItem> Menu()
        {
            var list = new List<MenuItem>();
            var ctrlers = Modules.Controllers;
            foreach (var ctrler in ctrlers)
            {
                foreach (MenuAttribute attr in ctrler.GetCustomAttributes(typeof(MenuAttribute),false))
                {
                    if(ctrler.GetCustomAttributes(typeof(AdminAttribute),true).Length == 0) continue;
                    if (attr == null) continue;

                    //search pattern Meshop.Core.Areas.Admin.Controllers
                    var area = "";
                    if (ctrler.Namespace != null)
                    {
                        const string pattern = @"Areas\.(\w+)\.Controllers";
                        var rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                        var match = rgx.Match(ctrler.Namespace);
                        if(match.Success )
                        {
                            area = match.Groups[1].Value;
                        }
                    }

                    var action = new MenuItem
                                     {
                                         
                                         Area = area,
                                         Controller = ctrler.Name.Remove(ctrler.Name.IndexOf("Controller")),

                                         Title = (string) HttpContext.GetGlobalResourceObject("Global", attr.Name)

                                     };
                    list.Add(action);
                }
            }

            return list;
        }

    }
}