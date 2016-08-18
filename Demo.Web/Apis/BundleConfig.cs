using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Optimization;
using Sail.Common;
using Sail.Web;

namespace Demo.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jplugin").Include(
                "~/scripts/jquery.min.js",
                "~/scripts/bootstrap.min.js",
                "~/scripts/jquery.slimscroll.js",
                "~/scripts/jsviews.js",
                "~/scripts/jplugin.js",
                "~/scripts/district.js",
                 "~/scripts/Extend.js",
                 "~/scripts/other/highCharts.js",
                  "~/scripts/uploadify/jquery.uploadifive.js",
                   "~/scripts/Calendar.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css/css").Include(
                "~/Content/css/bootstrap.css",
                "~/Content/css/font-awesome.min.css",
                "~/Content/css/components.css",
                "~/Content/css/layout.css",
                "~/Content/css/light.css",
                "~/Content/css/custom.css",
                "~/Content/css/plugins.css"
                ));
        }
    }
}
