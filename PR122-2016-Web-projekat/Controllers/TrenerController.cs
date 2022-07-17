using PR122_2016_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace PR122_2016_Web_projekat.Controllers
{
    public class TrenerController : Controller
    {
        // GET: Trener
        public ActionResult ProfilTrenera(string KorisnickoIme,string Sacuvaj,Korisnik korisnik)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            if (!korisnici.UserNameCheck(KorisnickoIme))
            {
                return RedirectToAction("PocetnaNeprijavljeni","Neprijavljen");
            }
            if (Sacuvaj != null)
            {
                Korisnik stariKorisnik = new Korisnik();
                stariKorisnik = korisnici.PodaciKorisnika(KorisnickoIme);
                korisnik.Uloga = stariKorisnik.Uloga;
                korisnici.IzmeniKorisnika(korisnik, stariKorisnik);
                Session["korisnici"] = korisnici;
                MessageBox.Show("Uspesno izmenjen korisnik");               
            }
            ViewBag.korisnicko_ime = KorisnickoIme;
            ViewBag.FitnesCentar = korisnici.TrenerFitnesCentar(KorisnickoIme);
            ViewBag.TrenerPodaci = korisnici.PodaciKorisnika(KorisnickoIme);         
            return View();
        }
       
        public ActionResult ZavrseniTreninziTrener(string KorisnickoIme,string Opcija,string tipSortiranja,string sortiranjePrema,string Naziv,string TipTreninga,string DonjaGranica,string GornjaGranica)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            if (!korisnici.UserNameCheck(KorisnickoIme))
            {
                return RedirectToAction("PocetnaNeprijavljeni", "Neprijavljen");
            }
            if(Opcija == "Sortiraj")
            {
                ViewBag.GrupniTreninzi = korisnici.SortiranjeGT(tipSortiranja, sortiranjePrema, KorisnickoIme);
            }
            else if(Opcija == "Pretrazi")
            {
                DateTime date = new DateTime();
                if (DonjaGranica == "" && GornjaGranica == "")
                {
                    if (Naziv == "" && TipTreninga == "")
                    {
                        ViewBag.GrupniTreninzi = korisnici.SortiranjeGT("", "",KorisnickoIme);
                    }
                    else
                    {
                        ViewBag.GrupniTreninzi = korisnici.PretragaGT(Naziv, TipTreninga, DonjaGranica, GornjaGranica,KorisnickoIme);
                    }

                }
                else if (DonjaGranica != "" && GornjaGranica == "")
                {
                    if (DateTime.TryParse(DonjaGranica, out date))
                    {
                        ViewBag.GrupniTreninzi = korisnici.PretragaGT(Naziv, TipTreninga, DonjaGranica, GornjaGranica,KorisnickoIme);
                    }
                    else
                    {
                        ViewBag.GrupniTreninzi = korisnici.SortiranjeGT("", "",KorisnickoIme);
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
                        ViewBag.GrupniTreninzi = korisnici.SortiranjeGT("", "",KorisnickoIme);
                        MessageBox.Show("Morate uneti datum za GornjuGranicu");
                    }
                }
                else if (DonjaGranica != "" && GornjaGranica != "")
                {
                    if (DateTime.TryParse(DonjaGranica, out date) && DateTime.TryParse(GornjaGranica, out date))
                    {
                        ViewBag.GrupniTreninzi = korisnici.PretragaGT(Naziv, TipTreninga, DonjaGranica, GornjaGranica,KorisnickoIme);
                    }
                    else
                    {
                        ViewBag.GrupniTreninzi = korisnici.SortiranjeGT("", "",KorisnickoIme);
                        MessageBox.Show("Morate uneti broj");
                    }
                }
            }
            else
            {
                ViewBag.GrupniTreninzi = korisnici.GrupniTreninziTrenera(KorisnickoIme);
            }           

            ViewBag.Posetioci = korisnici.posetiociGrupnogTreninga;
            ViewBag.korisnicko_ime = KorisnickoIme;           
            return View();
        }
        public ActionResult PredstojeciTreninziTrener(string KorisnickoIme,GrupniTrening GrupniTrening,string NazivFitnesCentra,string Naziv,string Opcija,string Datum,string Vreme)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            if (!korisnici.UserNameCheck(KorisnickoIme))
            {
                return RedirectToAction("PocetnaNeprijavljeni", "Neprijavljen");
            }
            if (Opcija == "Kreiraj")
            {               
                GrupniTrening.DatumTreninga = Datum +" "+Vreme;
                GrupniTrening.NazivFitnesCentra = korisnici.TrenerFitnesCentar(KorisnickoIme);               
                if (DateTime.Parse(GrupniTrening.DatumTreninga) > DateTime.Parse("2022/07/10 00:00"))
                {
                    korisnici.SacuvajUBazuGrupniTrening(GrupniTrening, KorisnickoIme);
                    MessageBox.Show($"Uspesno dodat {GrupniTrening.Naziv}");
                    Session["korisnici"] = korisnici;
                }
                else
                {
                    MessageBox.Show($"{GrupniTrening.Naziv} nedovoljno daleko u buducnost");
                }
                
            }
            else if (Opcija == "Obrisi")
            {
                if (korisnici.posetiociGrupnogTreninga.ContainsKey(GrupniTrening.Naziv))
                {
                    MessageBox.Show($"{GrupniTrening.Naziv} ima prijavljene posetioce");
                }
                else
                {
                    korisnici.ObrisiGrupniTrening(GrupniTrening.Naziv, KorisnickoIme);
                    MessageBox.Show($"Uspesno obrisan {GrupniTrening.Naziv}");
                    Session["korisnici"] = korisnici;
                }
               
            }
            else if (Opcija == "Izmeni")
            {
                korisnici.IzmeniGrupniTrening(GrupniTrening);
                MessageBox.Show($"Uspesno izmenjen {GrupniTrening.Naziv}");
                Session["korisnici"] = korisnici;
            }

            ViewBag.GrupniTreninzi = korisnici.GrupniTreninziTrenera(KorisnickoIme);
            ViewBag.Posetioci = korisnici.posetiociGrupnogTreninga;            
            ViewBag.korisnicko_ime = KorisnickoIme;
            return View();
        }
        public ActionResult DetaljanPrikazTrener(string Naziv,string KorisnickoIme)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            if (!korisnici.UserNameCheck(KorisnickoIme))
            {
                return RedirectToAction("PocetnaNeprijavljeni", "Neprijavljen");
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