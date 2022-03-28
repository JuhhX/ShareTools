using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using Tcc2._1.App_Start;
using System.Data.Entity;
using Tcc2._1.DAL;
using System.Web.Helpers;
using System.Security.Claims;

namespace Tcc2._1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer(new ToolsInitializer());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}