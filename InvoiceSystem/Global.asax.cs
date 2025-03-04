using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using InvoiceSystem.Models;

namespace InvoiceSystem
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            // Initialize roles and admin user
            InitializeRolesAndUsers();
        }
        
        private void InitializeRolesAndUsers()
        {
            // Create roles
            var context = new ApplicationDbContext();
            IdentityHelper.SeedRoles(context);
            
            // Create admin user
            var manager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            IdentityHelper.SeedAdminUser(manager);
        }
    }
}