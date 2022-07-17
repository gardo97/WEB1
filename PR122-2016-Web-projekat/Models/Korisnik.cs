using PR122_2016_Web_projekat.Models.Enumeracija;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR122_2016_Web_projekat.Models
{
    public class Korisnik
    {
        public string KorisnickoIme { get; set; } //Treba da je jedinstveno
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Pol { get; set; }
        public string Email { get; set; }
        public string DatumRodjenja { get; set; } //format dd/MM/yyyy 
        public UlogaKorisnika Uloga { get; set; }
        public bool Izbrisan { get; set; }
        public Korisnik()
        {

        }
      
        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, string pol, string email, string datumRodjenja, UlogaKorisnika uloga, bool izbrisan)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            Izbrisan = izbrisan;
        }
    }
}