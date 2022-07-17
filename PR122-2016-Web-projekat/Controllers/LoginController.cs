using PR122_2016_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace PR122_2016_Web_projekat.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult IndexLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string KorisnickoIme, string Lozinka)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
      
            if (korisnici.ProveraUlogovanogKorisnika(KorisnickoIme, Lozinka))
            {
                if (korisnici.UlogaCheck(KorisnickoIme) == "POSETILAC")
                {
                    ViewBag.korisnicko_ime = KorisnickoIme;
                    //ViewBag.FitnesCentri = korisnici.fitnesCentri.Values.ToList();
                    ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                    return View("LoginPosetilac");
                }
                else if (korisnici.UlogaCheck(KorisnickoIme) == "TRENER")
                {
                    ViewBag.korisnicko_ime = KorisnickoIme; ;
                    //ViewBag.FitnesCentri = korisnici.fitnesCentri.Values.ToList();
                    ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                    return View("LoginTrener");
                }
                else
                {
                    ViewBag.korisnicko_ime = KorisnickoIme;
                    //ViewBag.FitnesCentri = korisnici.fitnesCentri.Values.ToList();
                    ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                    return View("LoginVlasnik");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Wrong userName or password! ","ERROR", MessageBoxButton.OK);
                return View("IndexLogin");
            }
            
        }
        public ActionResult LoginPosetilac(string KorisnickoIme,string Opcija,string tipSortiranja,string sortiranjePrema,string Naziv,string Adresa,string DonjaGranica,string GornjaGranica)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];         
            if (Opcija == "Sortiraj")
            {
               ViewBag.FitnesCentri = korisnici.SortiranjeFC(tipSortiranja,sortiranjePrema);
            }
            else if(Opcija == "Pretrazi")
            {
                int number = 0;
                if (DonjaGranica == "" && GornjaGranica == "")
                {
                    if (Naziv == "" && Adresa =="")
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("","");
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }
                    
                }
                else if(DonjaGranica != "" && GornjaGranica == "")
                {
                    if (Int32.TryParse(DonjaGranica, out number))
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                        MessageBox.Show("Morate uneti broj za DonjuGranicu");
                    }
                }
                else if (DonjaGranica == "" && GornjaGranica != "")
                {
                    if (Int32.TryParse(GornjaGranica, out number))
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                        MessageBox.Show("Morate uneti broj za GornjuGranicu");
                    }
                }
                else if(DonjaGranica != "" && GornjaGranica != "")
                {
                    if (Int32.TryParse(DonjaGranica, out number) && Int32.TryParse(GornjaGranica, out number))
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                        MessageBox.Show("Morate uneti broj");
                    }
                }
                
                
            }
            else
            {
                ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
            }
            ViewBag.korisnicko_ime = KorisnickoIme;
            //ViewBag.FitnesCentri = korisnici.SortirajPoNazivuFC("Rastuci");          
            return View();
        }
        public ActionResult LoginTrener(string KorisnickoIme, string Opcija, string tipSortiranja, string sortiranjePrema, string Naziv, string Adresa, string DonjaGranica, string GornjaGranica)
        {           
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            if (!korisnici.UserNameCheck(KorisnickoIme))
            {
                return RedirectToAction("PocetnaNeprijavljen","Neprijavljen");
            }
            if (Opcija == "Sortiraj")
            {
                ViewBag.FitnesCentri = korisnici.SortiranjeFC(tipSortiranja, sortiranjePrema);
            }
            else if (Opcija == "Pretrazi")
            {
                int number = 0;
                if (DonjaGranica == "" && GornjaGranica == "")
                {
                    if (Naziv == "" && Adresa == "")
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }

                }
                else if (DonjaGranica != "" && GornjaGranica == "")
                {
                    if (Int32.TryParse(DonjaGranica, out number))
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                        MessageBox.Show("Morate uneti broj za DonjuGranicu");
                    }
                }
                else if (DonjaGranica == "" && GornjaGranica != "")
                {
                    if (Int32.TryParse(GornjaGranica, out number))
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                        MessageBox.Show("Morate uneti broj za GornjuGranicu");
                    }
                }
                else if (DonjaGranica != "" && GornjaGranica != "")
                {
                    if (Int32.TryParse(DonjaGranica, out number) && Int32.TryParse(GornjaGranica, out number) && DonjaGranica != "")
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                        MessageBox.Show("Morate uneti broj");
                    }
                }


            }
            else
            {
                ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
            }
            ViewBag.korisnicko_ime = KorisnickoIme;
           // ViewBag.FitnesCentri = korisnici.SortirajPoNazivuFC("Rastuci");
            return View();
        }
        public ActionResult LoginVlasnik(string KorisnickoIme, string Opcija, string tipSortiranja, string sortiranjePrema, string Naziv, string Adresa, string DonjaGranica, string GornjaGranica)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            
            if (Opcija == "Sortiraj")
            {
                ViewBag.FitnesCentri = korisnici.SortiranjeFC(tipSortiranja, sortiranjePrema);
               

            }
            else if (Opcija == "Pretrazi")
            {
                int number = 0;
                if (DonjaGranica == "" && GornjaGranica == "")
                {
                    if (Naziv == "" && Adresa == "")
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }

                }
                else if (DonjaGranica != "" && GornjaGranica == "")
                {
                    if (Int32.TryParse(DonjaGranica, out number))
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                        MessageBox.Show("Morate uneti broj za DonjuGranicu");
                    }
                }
                else if (DonjaGranica == "" && GornjaGranica != "")
                {
                    if (Int32.TryParse(GornjaGranica, out number))
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                        MessageBox.Show("Morate uneti broj za GornjuGranicu");
                    }
                }
                else if (DonjaGranica != "" && GornjaGranica != "")
                {
                    if (Int32.TryParse(DonjaGranica, out number) && Int32.TryParse(GornjaGranica, out number) && DonjaGranica != "")
                    {
                        ViewBag.FitnesCentri = korisnici.PretragaFC(Naziv, Adresa, DonjaGranica, GornjaGranica);
                    }
                    else
                    {
                        ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
                        MessageBox.Show("Morate uneti broj");
                    }
                }
            }
            else
            {
                ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
            }
            ViewBag.korisnicko_ime = KorisnickoIme;
           // ViewBag.FitnesCentri = korisnici.SortirajPoNazivuFC("Rastuci");
            return View();
        }
    }
}