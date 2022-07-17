using PR122_2016_Web_projekat.Models;
using PR122_2016_Web_projekat.Models.Enumeracija;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace PR122_2016_Web_projekat.Controllers
{
    public class VlasnikController : Controller
    {
        // GET: Vlasnik
        public ActionResult ProfilVlasnika(string KorisnickoIme,string Sacuvaj,Korisnik korisnik)
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
                korisnik.Uloga = stariKorisnik.Uloga;
                korisnici.IzmeniKorisnika(korisnik, stariKorisnik);
                Session["korisnici"] = korisnici;
                MessageBox.Show("Uspesno izmenjen korisnik");
            }
            ViewBag.korisnicko_ime = KorisnickoIme;
            ViewBag.VlasnikPodaci = korisnici.PodaciKorisnika(KorisnickoIme);
            ViewBag.FitnesCentriVlasnika = korisnici.FitnesCentriVlasnika(KorisnickoIme);
            return View();
        }       
        public ActionResult Komentari(string KorisnickoIme,string Opcija,Komentar k)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            if (Opcija == "Odobri")
            {
                korisnici.IzmeniKomentar(k, KorisnickoIme, "Odobreno");
                Session["korisnici"] = korisnici;
            }
            else if(Opcija == "Odbij")
            {
                korisnici.IzmeniKomentar(k, KorisnickoIme, "Odbijeno");
                Session["korisnici"] = korisnici;
            }
            ViewBag.ListaKomentara = korisnici.ListaKomentaraVlasnika(KorisnickoIme);
            ViewBag.korisnicko_ime = KorisnickoIme;
            return View();
        }
        [HttpPost]
        public ActionResult FitnesCentriVlasnik(string KorisnickoIme,string Opcija,FitnesCentar fc)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];           
            if (Opcija == "Kreiraj")
            {
                fc.KorisnickoImeVlasnika = KorisnickoIme;              
                korisnici.SacuvajUBazuFitnesCentar(fc);              
                MessageBox.Show($"Uspesno dodat {fc.Naziv}");
                Session["korisnici"] = korisnici;
            }
            else if(Opcija == "Obrisi")
            {
                if (!korisnici.ProveraBuduciTreninziFC(fc.Naziv))
                {
                    korisnici.ObrisiFitnesCentar(fc.Naziv, fc.KorisnickoImeVlasnika);
                    MessageBox.Show($"Uspesno obrisan {fc.Naziv}");
                    Session["korisnici"] = korisnici;
                }
                else
                {
                    MessageBox.Show($"{fc.Naziv} ima treninge u buducnosti");
                }
                
            }
            else if(Opcija == "Izmeni")
            {
                korisnici.IzmeniFitnesCentar(fc);
                MessageBox.Show($"Uspesno izmenjen {fc.Naziv}");
                Session["korisnici"] = korisnici;
            }
            ViewBag.FitnesCentri = korisnici.FitnesCentriVlasnika2(KorisnickoIme);
            ViewBag.korisnicko_ime = KorisnickoIme;
            return View();
        }
        public ActionResult DetaljanPrikazVlasnik(string Naziv,string KorisnickoIme)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            FitnesCentar fc = new FitnesCentar();
            ViewBag.korisnicko_ime = KorisnickoIme;
            korisnici.fitnesCentri.TryGetValue(Naziv, out fc);
            ViewBag.FitnesCentar = fc;
            ViewBag.GrupniTreninzi = korisnici.GrupniTreninziFitnesCentra(Naziv);
            ViewBag.Komentari = korisnici.ListaKomentara(Naziv);
            return View();
        }
        
        [HttpPost]
        public ActionResult PregledTrenera(string KorisnickoIme,string KorisnickoImeVlasnika,Korisnik korisnik,string Kreiraj,string FitnesCentar,string Blokiraj)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            if(Kreiraj == "Kreiraj")
            {
                korisnik.Uloga = UlogaKorisnika.TRENER;
                if (!korisnici.UserNameCheck(korisnik.KorisnickoIme))
                {
                    korisnici.SacuvajUBazuKorisnika(korisnik);
                    korisnici.SacuvajUBazuTrenerFitnesCentar(KorisnickoIme, FitnesCentar);                  
                    MessageBox.Show($"Uspesno dodat {KorisnickoIme}");
                    Session["korisnici"] = korisnici;
                }
                else {
                    MessageBox.Show("Postoji korisnik sa tim korisnickim imenom!");
                }              
            }
            if(Blokiraj == "Blokiraj")
            {
                korisnici.BlokirajTrenera(KorisnickoIme);
                Session["korisnici"] = korisnici;
                MessageBox.Show($"Uspesno blokiran {KorisnickoIme}");
            }
            ViewBag.FitnesCentri = korisnici.FitnesCentriVlasnika(KorisnickoImeVlasnika);
            ViewBag.korisnicko_ime = KorisnickoImeVlasnika;
            ViewBag.Treneri = korisnici.TreneriVlasnika(KorisnickoImeVlasnika);
            return View();
        }      
    }
}