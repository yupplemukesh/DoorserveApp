﻿using System.Web;
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
            // Ck editor js
            bundles.Add(new ScriptBundle("~/bundles/ckEditorJS").Include(
                      "~/ckeditor/ckeditor.js",
                                             "~/ckeditor/samples/js/sample.js"
              ));          
            bundles.Add(new ScriptBundle("~/bundles/deshboard").Include(
                     "~/Content/js/dashboard-2.min.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/vendor.css",
                                  "~/Content/css/application.css",                                
                                  "~/Content/css/bootstrap-datetimepicker.css",
                                     "~/Content/css/bootstrap-select.min.css",
                      "~/Content/css/style.css",
                      "~/Content/css/jquery-ui.css"));
            // ck editor css
            bundles.Add(new StyleBundle("~/Content/CKEditorCSS").Include(
                     "~/ckeditor/samples/css/samples.css",
                                          "~/ckeditor/samples/toolbarconfigurator/lib/codemirror/neo.css"
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
