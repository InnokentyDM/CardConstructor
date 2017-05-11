using System.Web;
using System.Web.Optimization;

namespace WebApplication9
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/fabricjs").Include(
                "~/Scripts/fabric.js-1.5.0/dist/fabric.js",
                "~/Scripts/fabric.js-1.5.0/lib/aligning_guidelines.js",
                "~/Scripts/fabric.js-1.5.0/lib/centering_guidelines.js"));

         

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/Canvas.css",
                      "~/Content/Main.css",
                      "~/Content/font-awesome-4.6.3/css/font-awesome.css"));
            bundles.Add(new StyleBundle("~/Content/Loading").Include(
                      "~/Content/Style.css"
                    ));
        }
    }
}
