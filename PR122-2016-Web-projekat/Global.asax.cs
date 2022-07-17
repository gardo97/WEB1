using PR122_2016_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PR122_2016_Web_projekat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //ovde odmah da ubacim pocetne podatke
            Korisnici korisnici = new Korisnici();
            korisnici.ListaKorisnika();
            korisnici.GrupniTreninziPosetioca();
            korisnici.TrenerFitnesCentar();
            korisnici.FitnesCentriVlasnika();
            korisnici.ListaFitnesCentara();
            korisnici.ListaGrupnihTreninga();
            korisnici.GrupniTreninziTrenera();
            korisnici.PosetiociGrupnogTreninga();
            korisnici.ListaKomentara();
            HttpContext.Current.Application["korisnici"] = korisnici;
        }
    }
}
