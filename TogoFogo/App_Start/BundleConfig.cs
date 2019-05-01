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
                        "~/Content/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Content/Scripts/bootstrap-datetimepicker.js",
                                              "~/Content/js/bootstrap-select.js"
               
               ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Content/Scripts/bootstrap-datetimepicker.js",
                                              "~/Content/Scripts/bootstrap-datepicker.js",
                                              "~/Content/js/bootstrap-select.js"

               ));

            // Ck editor js
            bundles.Add(new ScriptBundle("~/bundles/ckEditorJS").Include(
                      "~/ckeditor/ckeditor.js",
                                             "~/ckeditor/samples/js/sample.js"
              ));
            // js tree plugin js
            bundles.Add(new ScriptBundle("~/bundles/appjs").Include(
                      "~/Content/js/jstree.js"
              ));
            // Grid MVC  js
            bundles.Add(new ScriptBundle("~/bundles/js/Gridmvc").Include(
                      "~/Content/js/gridmvc.min.js"
              ));

            // form-Material  js
            bundles.Add(new ScriptBundle("~/bundles/js/form-m").Include(
                      "~/Content/js/forms-material-form.min.js"
              ));

            // ui button  js
            bundles.Add(new ScriptBundle("~/bundles/js/ui-buttons").Include(
                      "~/Content/js/ui-buttons.min.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/deshboard").Include(
                     "~/Content/js/dashboard-2.min.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/vendor.css",
                                  "~/Content/css/application.css",                                                           
                                              "~/Content/css/datatables.min.css",

                      "~/Content/css/style.css"));
            // ck editor css
            bundles.Add(new StyleBundle("~/Content/CKEditorCSS").Include(
                     "~/ckeditor/samples/css/samples.css",
                                          "~/ckeditor/samples/toolbarconfigurator/lib/codemirror/neo.css"
                                ));


            // js tree plugin css

            bundles.Add(new StyleBundle("~/Content/plugintree").Include(
                     "~/Content/css/plugin/style.css"
                                ));

            // Grid MVC CSS
            bundles.Add(new StyleBundle("~/Content/css/Gridmvc").Include(
                     "~/Content/css/Gridmvc.css",
                                          "~/Content/css/gridmvc.datepicker.css"

                                ));

            // Grid MVC CSS
            bundles.Add(new StyleBundle("~/Content/css/bootstap").Include(
                     "~/Content/bootstrap.min.css",
                                               "~/Content/css/bootstrap-datetimepicker.css",
                                     "~/Content/css/bootstrap-select.min.css"
                                ));
            bundles.Add(new ScriptBundle("~/bundles/customBundle").Include(

                      "~/content/js/vendor.min.js",
                      "~/content/js/cosmos.min.js",
                      "~/content/js/application.min.js",
                      "~/content/js/tables-datatables.min.js",
                      "~/content/js/ui-notifications.min.js",
                     "~/content/scripts/jquery.table2excel.js"
                    

                       ));
        }
    }
}
