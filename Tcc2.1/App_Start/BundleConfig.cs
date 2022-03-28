using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Tcc2._1.App_Start
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundle/js").Include("~/Scripts/functionsGlobals.js", "~/Scripts/artyom.window.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/css/style.css"));
        }

    }
}