using PR122_2016_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace PR122_2016_Web_projekat.Controllers
{
    public class PosetilacController : Controller
    {
        // GET: Posetilac
        public ActionResult ProfilPosetioca(string KorisnickoIme,string Sacuvaj,Korisnik korisnik)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];

            if (korisnici == null)
            {
                korisnici = new Korisnici();
                Session["korisnici"] = korisnici;
            }
            if(Sacuvaj != null)
            {
                Korisnik stariKorisnik = new Korisnik();
                stariKorisnik = korisnici.PodaciKorisnika(KorisnickoIme);
                korisnici.IzmeniKorisnika(korisnik, stariKorisnik);
                Session["korisnici"] = korisnici;
                MessageBox.Show("Uspesno izmenjen korisnik");               
            }                 
            ViewBag.korisnicko_ime = KorisnickoIme;
            ViewBag.PosetilacPodaci = korisnici.PodaciKorisnika(KorisnickoIme);
            return View();
        }
        
        public ActionResult GrupniTreninziPosetilac(string KorisnickoIme,string Opcija,string tipSortiranja,string sortiranjePrema, string Naziv, string TipTreninga, string DonjaGranica, string GornjaGranica)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            if (Opcija == "Sortiraj")
            {
                ViewBag.GrupniTreninzi = korisnici.SortiranjeGT(tipSortiranja, sortiranjePrema,KorisnickoIme);
            }
            else if (Opcija == "Pretrazi")
            {
                DateTime date = new DateTime();
                if (DonjaGranica == "" && GornjaGranica == "")
                {
                    if (Naziv == "" && TipTreninga == "")
                    {
                        ViewBag.GrupniTreninzi = korisnici.SortiranjeGT("", "", KorisnickoIme);
                    }
                    else
                    {
                        ViewBag.GrupniTreninzi = korisnici.PretragaGT(Naziv, TipTreninga, DonjaGranica, GornjaGranica, KorisnickoIme);
                    }

                }
                else if (DonjaGranica != "" && GornjaGranica == "")
                {
                    if (DateTime.TryParse(DonjaGranica, out date))
                    {
                        ViewBag.GrupniTreninzi = korisnici.PretragaGT(Naziv, TipTreninga, DonjaGranica, GornjaGranica, KorisnickoIme);
                    }
                    else
                    {
                        ViewBag.GrupniTreninzi = korisnici.SortiranjeGT("", "", KorisnickoIme);
                        MessageBox.Show("Morate uneti datum za DonjuGranicu");
                    }
                }
                else if (DonjaGranica == "" && GornjaGranica != "")
                {
                    if (DateTime.TryParse(GornjaGranica, out date))
                    {
                        ViewBag.GrupniTreninzi = korisnici.PretragaGT(Naziv, TipTreninga, DonjaGranica, GornjaGranica, KorisnickoIme);
                    }
                    else
                    {
                        ViewBag.GrupniTreninzi = korisnici.SortiranjeGT("", "", KorisnickoIme);
                        MessageBox.Show("Morate uneti datum za GornjuGranicu");
                    }
                }
                else if (DonjaGranica != "" && GornjaGranica != "")
                {
                    if (DateTime.TryParse(DonjaGranica, out date) && DateTime.TryParse(GornjaGranica, out date))
                    {
                        ViewBag.GrupniTreninzi = korisnici.PretragaGT(Naziv, TipTreninga, DonjaGranica, GornjaGranica, KorisnickoIme);
                    }
                    else
                    {
                        ViewBag.GrupniTreninzi = korisnici.SortiranjeGT("", "", KorisnickoIme);
                        MessageBox.Show("Morate uneti broj");
                    }
                }
            }
            else
            {
               ViewBag.GrupniTreninzi = korisnici.GrupniTreninziPosetioca(KorisnickoIme);
            }
            ViewBag.korisnicko_ime = KorisnickoIme;            
            return View();
        }
        public ActionResult DetaljanPrikazPosetilac(string Naziv,string NazivGP,string KorisnickoIme,Komentar Komentar,string Opcija)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];        
            if (Opcija == "Komentarisi")
            {
                if (korisnici.ProveraPosetilacKomentar(KorisnickoIme, Naziv))
                {
                    Komentar.Vidljivost = "CekaOdobrenje";
                    korisnici.SacuvajUBazuKomentar(Komentar);
                    Session["korisnici"] = korisnici;
                    MessageBox.Show($"Komentar ceka odobrenje");
                }
                else
                {
                    MessageBox.Show($"Nisam bio u {Naziv}");
                }
               
            }
            else if(Opcija == "Prijavi")
            {
                if (!korisnici.ProveraPrijavaTreninga(KorisnickoIme, NazivGP))
                {
                    korisnici.DodajGrupniTreningPosetilac(KorisnickoIme,NazivGP);
                    Session["korisnici"] = korisnici;
                    MessageBox.Show($"Uspesno prijavljen trening");
                }
                else
                {
                    MessageBox.Show($"Popunjena mesta {NazivGP} ili sam se vec prijavio");
                }
            }

            FitnesCentar fc = new FitnesCentar();
            ViewBag.korisnicko_ime = KorisnickoIme;
            korisnici.fitnesCentri.TryGetValue(Naziv, out fc);
            ViewBag.FitnesCentar = fc;
            ViewBag.GrupniTreninzi = korisnici.GrupniTreninziFitnesCentra(Naziv);          
            ViewBag.Komentari = korisnici.ListaKomentara(Naziv);            
           
            return View();
        }
    }
}