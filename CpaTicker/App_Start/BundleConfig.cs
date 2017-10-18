using System.Web;
using System.Web.Optimization;

namespace CpaTicker
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));



            bundles.Add(new StyleBundle("~/Content/admincss")
                .Include("~/Content/js/plugin/jquery-timepicker-addon/jquery-ui-timepicker-addon.css")
                .Include("~/Content/css/colorpicker.css")
                .Include("~/Areas/admin/content/admin.css", new CssRewriteUrlTransform())
                .Include("~/Content/css/clickticker.css")
                    );

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            /****njhones bundles***/

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                         "~/Content/css/bootstrap.min.css",
                         "~/Content/css/font-awesome.min.css"));


            bundles.Add(new StyleBundle("~/Content/css/smartadmin").Include(
                        "~/Content/css/smartadmin-production.css",
                        "~/Content/css/smartadmin-skins.css"));

            bundles.Add(new ScriptBundle("~/Content/js/bootstrap").Include(
                       "~/Content/js/bootstrap/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/notification").Include(
                       "~/Content/js/notification/SmartNotification.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/widgets").Include(
                       "~/Content/js/smartwidgets/jarvis.widget.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/plugins/pie").Include(
                      "~/Content/js/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/plugins/sparkline").Include(
                      "~/Content/js/plugin/sparkline/jquery.sparkline.min.js"));


            //js/plugin/jquery-validate/jquery.validate.min.js

            bundles.Add(new ScriptBundle("~/Content/js/plugins/masked").Include(
                     "~/Content/js/plugin/masked-input/jquery.maskedinput.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/plugins/select2").Include(
                     "~/Content/js/plugin/select2/select2.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/plugins/slider").Include(
                //"~/Content/js/plugin/bootstrap-slider/bootstrap-slider.min.js"
                       "~/Content/js/plugin/bootstrap-slider/bootstrap-slider.js" // modified for allow several sliders
                     ));

            bundles.Add(new ScriptBundle("~/Content/js/plugins/msie").Include(
                      "~/Content/js/plugin/msie-fix/jquery.mb.browser.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/plugins/smartclick").Include(
                      "~/Content/js/plugin/smartclick/smartclick.js"));

            bundles.Add(new ScriptBundle("~/Content/js/mainapp").Include(
                      "~/Content/js/app.js"));
            //"~/Content/js/demo.js"));

            bundles.Add(new ScriptBundle("~/bundler/js/jqueryplugins").Include(
                      "~/Content/js/jquery.form.js"
                      , "~/Content/js/jquery.textfill.min.js"
                      , "~/Content/js/plugin/jquery-timepicker-addon/jquery-ui-timepicker-addon.js"
                      , "~/Content/js/plugin/jquery-timepicker-addon/jquery-ui-sliderAccess.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundler/js/reportopts").Include(
                      "~/Content/js/reportopts.js"));


            bundles.Add(new ScriptBundle("~/bundles/charts").Include(
                      "~/Scripts/Highcharts-3.0.1/js/highcharts.js"));

            bundles.Add(new ScriptBundle("~/bundles/ticker").Include(
                     "~/Content/js/plugin/pause/jquery.pause.min.js"
                //, "~/Content/js/plugin/colorpicker/bootstrap-colorpicker.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/datables").Include(
                       "~/Content/js/plugin/datatables/jquery.dataTables.min.js"
                     , "~/Content/js/plugin/datatables/dataTables.colVis.min.js"
                     , "~/Content/js/plugin/datatables/dataTables.tableTools.min.js"
                     , "~/Content/js/plugin/datatables/dataTables.bootstrap.min.js"
                     , "~/Content/js/plugin/datatable-responsive/datatables.responsive.min.js"
                     ));
            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                     "~/Scripts/jquery.signalR-2.2.0.min.j"));
        }


        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new System.ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }
    }
}