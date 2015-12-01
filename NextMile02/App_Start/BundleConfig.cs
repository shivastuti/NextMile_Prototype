using System.Web;
using System.Web.Optimization;

namespace NextMile02
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/javascript/bootstrap.js",
                      "~/Scripts/javascript/Facebook.js",
                      "~/Scripts/javascript/index.js",
                      "~/Scripts/javascript/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/styles.css",
                        "~/Content/bootstrap.min.css",
                        "~/Content/bootstrap-glyphicons.css",
                        "~/Content/bootstrap.css"));
                     
        }
    }
}
