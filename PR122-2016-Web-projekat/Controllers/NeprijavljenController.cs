using PR122_2016_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using System.Windows.Forms;

namespace PR122_2016_Web_projekat.Controllers
{
    public class NeprijavljenController : Controller
    {
        // GET: Neprijavljen
        public ActionResult PocetnaNeprijavljeni(string Opcija, string tipSortiranja, string sortiranjePrema, string Naziv, string Adresa, string DonjaGranica, string GornjaGranica)
        {
            if(Session["korisnici"] == null)
            {
                Session["korisnici"] = HttpContext.Application["korisnici"];
            }
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
                        System.Windows.MessageBox.Show("Morate uneti broj za DonjuGranicu");
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
                        System.Windows.MessageBox.Show("Morate uneti broj za GornjuGranicu");
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
                        System.Windows.MessageBox.Show("Morate uneti broj");
                    }
                }
            }
            else
            {
                ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
            }
      
            //ViewBag.FitnesCentri = korisnici.SortiranjeFC("", "");
            return View();
        }
        [HttpPost]
        public ActionResult DetaljanPrikaz(string Naziv)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            FitnesCentar fc = new FitnesCentar();
            korisnici.fitnesCentri.TryGetValue(Naziv, out fc);
            ViewBag.FitnesCentar = fc;
            ViewBag.GrupniTreninzi = korisnici.GrupniTreninziFitnesCentra(Naziv);
            ViewBag.Komentari = korisnici.ListaKomentara(Naziv);
            return View();
        }        
        public ActionResult Registracija()
        {
            return View();
        }
        public ActionResult RegistracijaKorisnika(Korisnik korisnik,string DatumRodjenja)
        {
            Korisnici korisnici = (Korisnici)Session["korisnici"];
            if (korisnici == null)
            {
                korisnici = new Korisnici();
                Session["korisnici"] = korisnici;
            }

            var temp = DatumRodjenja;
            if (korisnici.UserNameCheck(korisnik.KorisnickoIme))
            {
                System.Windows.MessageBox.Show("KorisnickoIme vec postoji!");
                
            }
            else if (!korisnici.ValidacijaDatuma(korisnik.DatumRodjenja))
            {
                System.Windows.MessageBox.Show("Datum pogresan!");
            }
            else
            {
                korisnici.SacuvajUBazuKorisnika(korisnik);
                Session["korisnici"] = korisnici;
            }

            return View("Registracija");
        }      
    }
}