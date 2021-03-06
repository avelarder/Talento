﻿using System.Web;
using System.Web.Optimization;

namespace Talento
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
          bundles.Add(new StyleBundle("~/Content/css").Include(
          "~/Content/bootstrap.css",
          "~/Content/propeller.min.css",
          "~/Content/styles.css",
          "~/Content/PagedList.css",
          "~/Content/toastr.min.css",
          "~/Content/alertify.min.css",
          "~/Content/select2-bootstrap.css",
          "~/Content/round-about.css",
          "~/Content/reset.css",
          "~/Content/style.css",
          "~/Content/styleIE.css"
          ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/propeller.min.js",
                      "~/Scripts/toastr.min.js",
                      "~/Scripts/typeahead.js",
                      "~/Scripts/alertify.min.js",
                      "~/Scripts/covervid-scripts.js",
                      "~/Scripts/covervid.min.js",
                      "~/Scripts/jquery.easing.1.3.js",
                      "~/Scripts/modernizr.custom.11333.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/app.js"));
        }
    }
}
