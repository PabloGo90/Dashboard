using System.Web;
using System.Web.Optimization;

namespace Dashboard
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

            bundles.Add(new ScriptBundle("~/bundles/validaCampos").Include(
                        "~/Scripts/validaCampos.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.min.js",
                      "~/Scripts/bootbox.min.js",
                      "~/Scripts/color-modes.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/Scripts/DataTables/datatables.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Scripts/DataTables/datatables.min.css",
                      "~/Content/dashboardSite.css",
                      "~/Content/Site.css",
                      "~/Content/jsoneditor/jsoneditor.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/jsoneditor").Include(
                        "~/Scripts/jsoneditor/jsoneditor.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                        "~/Scripts/chart.min.js"));

            bundles.Add(new StyleBundle("~/Content/cssLogin").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/loginSite.css"));

            bundles.Add(new ScriptBundle("~/bundles/serializejson").Include("~/Scripts/jquery.serializejson.js"));
        }
    }
}
