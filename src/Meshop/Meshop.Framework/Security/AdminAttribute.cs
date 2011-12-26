using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Model;

namespace Meshop.Framework.Security
{
   
    public class AdminAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }


            IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            } 

            string userRole = "";

            /*var cookie = httpContext.Request.Cookies["UserRole"];
            if (cookie != null)
            {
                userRole = cookie.Value;
            }*/

            //TODO: caching
            var db = new DatabaseConnection2();

            userRole = db.Customers.Find(user.Identity.Name).Role;

            return userRole.ToLower() == "admin";
        }

    }
}
