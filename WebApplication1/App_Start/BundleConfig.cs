using System.Web;
using System.Web.Optimization;

namespace AllEngineers
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/facebook").Include(
                      "~/Scripts/facebook.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                      "~/Plugins/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/extenders").Include(
                      "~/Scripts/extend-1.0.js",
                      "~/extenders"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Css/bootstrap.css",
                      "~/Css/site.css"));
        }
    }
}
