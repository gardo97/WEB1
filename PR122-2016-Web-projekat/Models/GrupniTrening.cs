using PR122_2016_Web_projekat.Models.Enumeracija;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR122_2016_Web_projekat.Models
{
    public class GrupniTrening
    {
        public string Naziv { get; set; }
        public TipTreninga Trening { get; set; }
        public string NazivFitnesCentra { get; set; }
        public string TrajanjeTreninga { get; set; } //combobox za ovo,15m 30m,45m,60m
        public string DatumTreninga { get; set; }  //(čuvati u formatu dd/MM/yyyy HH:mm)       
        public int MaxBrojPosetilaca { get; set; }
        public bool Izbrisan { get; set; }

        public GrupniTrening()
        {

        }

        public GrupniTrening(string naziv, TipTreninga trening, string nazivFitnesCentra, string trajanjeTreninga, string datumTreninga, int maxBrojPosetilaca,bool izbrisan)
        {
            Naziv = naziv;
            Trening = trening;
            NazivFitnesCentra = nazivFitnesCentra;
            TrajanjeTreninga = trajanjeTreninga;
            DatumTreninga = datumTreninga;
            MaxBrojPosetilaca = maxBrojPosetilaca;          
            Izbrisan = izbrisan;
        }
    }
}