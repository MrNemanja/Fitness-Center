using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FitnesCentar.Models;

namespace FitnesCentar
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            List<Fitnes_Centar> centri = Data.GetFitnesCentar("~/App_Data/fitnes_centri.txt");
            HttpContext.Current.Application["centri"] = centri;

            List<GrupniTrening> treninzi = Data.GetTreninzi("~/App_Data/grupni_treninzi.txt");
            HttpContext.Current.Application["treninzi"] = treninzi;

            List<Komentar> komentari = Data.GetKomentari("~/App_Data/komentari.txt");
            HttpContext.Current.Application["komentari"] = komentari;

            List<Korisnik> korisnici = Data.GetKorisnici("~/App_Data/korisnici.txt");
            HttpContext.Current.Application["korisnici"] = korisnici;

            string path = Path.Combine(Server.MapPath("~/Files/"));
            List<UploadedFile> files = new List<UploadedFile>();
            foreach (string file in Directory.GetDirectories(path))
            {
                if (Directory.GetFiles(file).Length != 0)
                {
                    files.Add(new UploadedFile(Path.GetFileName(Directory.GetFiles(file)[0]), Directory.GetFiles(file)[0]));
                }
            }
            HttpContext.Current.Application["Files"] = files;



        }
    }
}
