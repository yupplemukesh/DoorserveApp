using System.Web;
using System.Web.Optimization;

namespace TogoFogo
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
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/customBundle").Include(
               
                      "~/vendors/fastclick/lib/fastclick.js",
                      "~/vendors/nprogress/nprogress.js",
                      "~/vendors/gauge.js/dist/gauge.min.js",
                      "~/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js",
                      "~/vendors/skycons/skycons.js",
                      "~/vendors/Flot/jquery.flot.js",
                      "~/vendors/Flot/jquery.flot.pie.js",
                      "~/vendors/Flot/jquery.flot.time.js",
                      "~/vendors/Flot/jquery.flot.stack.js",
                      "~/vendors/Flot/jquery.flot.resize.js",
                      "~/vendors/flot.orderbars/js/jquery.flot.orderBars.js",
                      "~/vendors/flot-spline/js/jquery.flot.spline.min.js",
                      "~/vendors/flot.curvedlines/curvedLines.js",
                      "~/vendors/jqvmap/dist/jquery.vmap.js",
                      "~/vendors/jqvmap/dist/maps/jquery.vmap.world.js",
                      "~/vendors/jqvmap/examples/js/jquery.vmap.sampledata.js",
                       "~/vendors/datatables.net/js/jquery.dataTables.min.js",
                       "~/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js",
                       "~/vendors/datatables.net-buttons/js/dataTables.buttons.min.js",
                       "~/vendors/datatables.net-buttons-bs/js/buttons.bootstrap.min.js",
                       "~/vendors/datatables.net-buttons/js/buttons.flash.min.js",
                       "~/vendors/datatables.net-buttons/js/buttons.html5.min.js",
                       "~/vendors/datatables.net-buttons/js/buttons.print.min.js",
                       "~/vendors/datatables.net-fixedheader/js/dataTables.fixedHeader.min.js",
                       "~/vendors/datatables.net-keytable/js/dataTables.keyTable.min.js",
                       "~/vendors/datatables.net-responsive/js/dataTables.responsive.min.js",
                       "~/vendors/datatables.net-responsive-bs/js/responsive.bootstrap.js",
                       "~/vendors/datatables.net-scroller/js/dataTables.scroller.min.js",
                       //"~/vendors/moment/min/moment.min.js",
                       "~/vendors/bootstrap-daterangepicker/daterangepicker.js",
                        "~/Custom_CssandScript/jquery.dataTables.min.js",
                    
                       //"~/Custom_CssandScript/bootstrap-datetimepicker.css",
                       "~/Custom_CssandScript/bootstrap-datetimepicker.js",
                        //"~/build/js/custom.min.js",
                       "~/vendors/iCheck/icheck.min.js"
                         
                       ));
        }
    }
}
