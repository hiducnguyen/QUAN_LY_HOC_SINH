using System.Web.Optimization;

namespace QUAN_LY_HOC_SINH
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
                      "~/Scripts/popper.js",
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.min.css"));

            // Select2
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                      "~/Scripts/select2.min.js"));

            bundles.Add(new StyleBundle("~/Content/select2").Include(
                      "~/Content/css/select2.min.css"));

            // Custom
            bundles.Add(new StyleBundle("~/Content/custom").Include(
                      "~/Content/css/common.css",
                      "~/Content/css/layout.css"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/custom.js"));
        }
    }
}
