using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.DI;
using Meshop.Framework.Model;
using Meshop.Framework.Module;
using Meshop.Framework.Security;
using Meshop.Framework.Services;

namespace Meshop.Core.DefaultServices
{
    public class DefaultFront : IFront
    {

        private DatabaseConnection2 _db = new DatabaseConnection2();

        public IEnumerable<MenuItem> Menu()
        {
            var list = new List<MenuItem>();
            var ctrlers = Modules.Controllers;
            foreach (var ctrler in ctrlers)
            {

                //check methods for MenuAttribute
                foreach (var method in ctrler.GetMethods())
                {
                    foreach (MenuAttribute attr in method.GetCustomAttributes(typeof(MenuAttribute),false))
                    {
                       //not the Menu for Admin
                        if (ctrler.GetCustomAttributes(typeof(AdminAttribute), true).Length > 0) continue;

                        //search pattern Meshop.Core.Areas.Admin.Controllers
                        var area = "";
                        var action = method.Name;
                        if (ctrler.Namespace != null)
                        {
                            const string pattern = @"Areas\.(\w+)\.Controllers";
                            var rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                            var match = rgx.Match(ctrler.Namespace);
                            if (match.Success)
                            {
                                area = match.Groups[1].Value;
                            }
                        }

                        var actionNames = method.GetCustomAttributes(typeof (ActionNameAttribute), false);
                        if (actionNames.Length > 0)
                        {
                            action = ((ActionNameAttribute) actionNames.First()).Name;
                        }

                        var link = new MenuItem
                        {

                            Area = area,
                            Controller = ctrler.Name.Remove(ctrler.Name.IndexOf("Controller")),
                            Action = action,
                            Title = (string)HttpContext.GetGlobalResourceObject("Global", attr.Name)

                        };
                        list.Add(link);
                    }
                }

            }

            return list;
        }

        public bool IsAdmin(IPrincipal customer)
        {
            var userRole = _db.Customers.Find(customer.Identity.Name).Role;

            return userRole.ToLower() == "admin";
        }

        private void BuildTree(BasicCategory category, List<BasicCategory> root)
        {
            category.Children = new List<BasicCategory>();


            foreach (BasicCategory basicCategory in root.FindAll(c => c.Parent != null && c.Parent.CategoryID == category.CategoryID))
            {
                BuildTree(basicCategory, root);
                category.Children.Add(basicCategory);
            }
        }

        public IEnumerable<BasicCategory> CategoryTree()
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
    }
}