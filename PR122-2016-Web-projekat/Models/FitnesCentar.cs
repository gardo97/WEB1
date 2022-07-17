using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR122_2016_Web_projekat.Models
{
    public class FitnesCentar
    {
        public string Naziv { get; set; }
        public string Adresa { get; set; } // mozda da bude string[], format ulica i broj,mesto/grad,postanski broj
        public int GodinaOtvaranja { get; set; }
        public string KorisnickoImeVlasnika { get; set; }
        public int MesecnaClanarina { get; set; }
        public int GodisnjaClanarina { get; set; }
        public int CenaJednogTreninga { get; set; }
        public int CenaGrupnogTreninga { get; set; }
        public int CenaPersonalniTrener { get; set; }
        public bool Izbrisan { get; set; }

        public FitnesCentar()
        {

        }

        public FitnesCentar(string naziv, string adresa, int godinaOtvaranja, string korisnickoImeVlasnika, int mesecnaClanarina, int godisnjaClanarina, int cenaJednogTreninga, int cenaGrupnogTreninga, int cenaPersonalniTrener, bool izbrisan)
        {
            Naziv = naziv;
            Adresa = adresa;
            GodinaOtvaranja = godinaOtvaranja;
            KorisnickoImeVlasnika = korisnickoImeVlasnika;
            MesecnaClanarina = mesecnaClanarina;
            GodisnjaClanarina = godisnjaClanarina;
            CenaJednogTreninga = cenaJednogTreninga;
            CenaGrupnogTreninga = cenaGrupnogTreninga;
            CenaPersonalniTrener = cenaPersonalniTrener;
            Izbrisan = izbrisan;
        }

        
    }
}