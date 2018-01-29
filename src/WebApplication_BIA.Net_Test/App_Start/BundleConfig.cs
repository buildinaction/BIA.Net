using System.Web;
using System.Web.Optimization;

namespace WebApplication_BIA.Net_Test
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*Add for BIA.Net.MVC*/
            bundles.Add(new ScriptBundle("~/bundles/BIA.Net").Include(
                  /*Add for BIA.Net.MVC.Dialog*/
                  "~/Scripts/jquery-ui-1.12.1.js",
                  "~/Scripts/BIA.Net/Dialog.js"
                  /*End add for BIA.Net.MVC.Dialog */));
            /*Add for BIA.Net.MVC*/
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      /*Add for BIA.Net.MVC.Dialog*/
                      "~/Content/themes/base/jquery-ui.min.css",
                      "~/Content/themes/base/core.css",
                      "~/Content/themes/base/dialog.css",
                      "~/Content/themes/base/theme.css",
                      "~/Content/BIA.Net/Dialog.css"
                      /*End add for BIA.Net.MVC.Dialog */
                      ));
        }
    }
}
