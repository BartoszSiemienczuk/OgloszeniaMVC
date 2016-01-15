using System;
using System.Web;
using System.Web.Optimization;

namespace Ogloszenia
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/templateScripts").Include(
                        "~/Assets/js/*.js"));

            bundles.Add(new StyleBundle("~/Assets/css/bootstrap").Include(
                      "~/Assets/css/bootstrap.css",
                      "~/Assets/css/animate.css",
                      "~/Assets/css/font-awesome.css",
                      "~/Assets/css/datepicker.css"));

            bundles.Add(new StyleBundle("~/Assets/css").Include(
                      "~/Assets/css/main.css",
                      new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Assets/bluecss").Include(
                      "~/Assets/css/greenSkin.css",
                      new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Assets/yellowcss").Include(
                      "~/Assets/css/yellowSkin.css",
                      new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Assets/css/responsive").Include(
                      "~/Assets/css/responsive.css",
                      new CssRewriteUrlTransform()));
        }
        
    }
}
