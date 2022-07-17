using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR122_2016_Web_projekat.Models
{
    public class Komentar
    {
        public string KorisnickoImePosetioca { get; set; }
        public string NazivFitnesCentra { get; set; }
        public string TekstKomentara { get; set; }
        public int Ocena { get; set; }
        public string Vidljivost { get; set; } //odbijen/prihvacen
        public Komentar()
        {

        }

        public Komentar(string korisnickoImePosetioca, string nazivFitnesCentra, string tekstKomentara, int ocena, string vidljivost)
        {
            KorisnickoImePosetioca = korisnickoImePosetioca;
            NazivFitnesCentra = nazivFitnesCentra;
            TekstKomentara = tekstKomentara;
            Ocena = ocena;
            Vidljivost = vidljivost;
        }
    }
}