using System.Web;
using System.Web.Optimization;

namespace IkoulaACDF
{
    public class BundleConfig
    {
        // Pour plus d’informations sur le regroupement, rendez-vous sur http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        //"~/Scripts/holder.js",
                        "~/Scripts/jquery.metisMenu.js",
                        "~/Scripts/sb-admin.js",
                        "~/Scripts/jquery-cdf54-imagePreview.js"));

            bundles.Add(new ScriptBundle("~/bundles/fancybox").Include(
                        "~/Scripts/jquery.fancybox.pack.js",
                        "~/Scripts/jquery.mousewheel-{version}.pack.js",
                        "~/Scripts/jquery.fancybox-buttons.js",
                        "~/Scripts/jquery.fancybox-thumbs.js",
                        "~/Scripts/jquery.fancybox-media.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/elastislide").Include(
                        "~/Scripts/jquery.tmpl.min.js",
                        "~/Scripts/jquery.easing.1.3.js",
                        "~/Scripts/jquery.elastislide.js",
                        "~/Scripts/gallery.js"));

            bundles.Add(new ScriptBundle("~/bundles/flexslider").Include(
            "~/Scripts/jquery.flexslider.js"));

            bundles.Add(new ScriptBundle("~/bundles/tablesorter").Include(
                        "~/Scripts/jquery.tablesorter.js",
                        "~/Scripts/jquery.tablesorter.widgets.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
            "~/Scripts/jquery.datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour développer et apprendre. Puis, lorsque vous êtes
            // prêt pour la production, utilisez l’outil de génération sur http://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/locales/bootstrap-datepicker.fr.js"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
          "~/Scripts/jquery.inputmask/jquery.inputmask-2.5.0.js",
          "~/Scripts/jquery.inputmask/jquery.inputmask.date.extensions-2.5.0.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/bootstrap-datepicker.css",
                    "~/Content/jquery.fancybox.css",
                    "~/Content/flexslider.css",
                    "~/Content/font-awesome.css",
                    "~/Content/elastislidestyle.css",
                    "~/Content/elastislide.css",
                    //"~/Content/sb-admin.css",
                      "~/Content/site.css"));
        }
    }
}
