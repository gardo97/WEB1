using PR122_2016_Web_projekat.Models.Enumeracija;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PR122_2016_Web_projekat.Models
{
    public class Korisnici
    {
        public Dictionary<string,Korisnik> listaKorisnika;
        //public Dictionary<UlogaKorisnika, List<Korisnik>> listeKorisnika;
        //public List<FitnesCentar> fitnesCentri;
        public Dictionary<string, FitnesCentar> fitnesCentri;
        // public List<GrupniTrening> grupniTreninzi;
        public Dictionary<string, GrupniTrening> grupniTreninzi;
        public Dictionary<string,List<string>> grupniTreninziPosetioca; // <korisnickoImePosetioca,lista treninga<naziv>> 
        public Dictionary<string,List<string>> grupniTreninziTrenera; // <korisnickoImeTrenera,lista treninga<naziv,datumTreninga>>
        public Dictionary<string,List<string>> fitnesCentriVlasnika; // <korisnickoImeVlasnika,lista naziva fitnes centara>
        public Dictionary<string,List<string>> posetiociGrupnogTreninga; // <nazivGrupnogTreninga,lista posetioca treninga>
        public Dictionary<string,List<Komentar>> komentariFitnesCentra; // <nazivFitnesCentra,lista komentara>
        //public Dictionary<string, List<Korisnik>> treneriVlasnika;
        //public Dictionary<string, string> treneriFitnesCentar; // <korisnickoImeTrenera,nazivfitnesCentra>
        //public Dictionary<Korisnik, FitnesCentar> treneriFitnesCentar;
        public List<Tuple<string, string>> trenerFitnesCentar; //<korisnickoImeTrenera,nazivfitnesCentra>
        

        //test
        public Korisnici()
        {
            listaKorisnika = new Dictionary<string, Korisnik>();
            grupniTreninziPosetioca = new Dictionary<string, List<string>>();
            trenerFitnesCentar = new List<Tuple<string, string>>();
            fitnesCentriVlasnika = new Dictionary<string, List<string>>();
            fitnesCentri = new Dictionary<string, FitnesCentar>();
            grupniTreninzi = new Dictionary<string, GrupniTrening>();
            posetiociGrupnogTreninga = new Dictionary<string, List<string>>();
            grupniTreninziTrenera = new Dictionary<string, List<string>>();
            komentariFitnesCentra = new Dictionary<string, List<Komentar>>();   
        }
        public void SacuvajUBazuKorisnika(Korisnik k)
        {
            string putanja = "~/App_Data/Korisnici.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //KorisnickoIme[0]?Lozinka[1]?Ime[2]?Prezime[3]?Pol[4]?Email[5]?Datum[6]?Uloga[7]??Izbrisan(default false)[8]
            sw.WriteLine($"{k.KorisnickoIme}?{k.Lozinka}?{k.Ime}?{k.Prezime}?{k.Pol}?{k.Email}?{k.DatumRodjenja}?{k.Uloga}?{k.Izbrisan}");
            listaKorisnika.Add(k.KorisnickoIme,k);
            sw.Close();
            fs.Close();
        }
        public void SacuvajUBazuFitnesCentar(FitnesCentar fc)
        {
            string putanja = "~/App_Data/FitnesCentri.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //Naziv[0]?Adresa[1]?GodinaOtvaranja[2]?Vlasnik[3]?Mesecna[4]?Godisnja[5]?JedanT[6]?Grupni[7]?Personalni[8]?Izbrisan(default false)[9]
            sw.WriteLine($"{fc.Naziv}?{fc.Adresa}?{fc.GodinaOtvaranja}?{fc.KorisnickoImeVlasnika}?{fc.MesecnaClanarina}?{fc.GodisnjaClanarina}?{fc.CenaJednogTreninga}?{fc.CenaGrupnogTreninga}?{fc.CenaPersonalniTrener}?{fc.Izbrisan}");
            fitnesCentri.Add(fc.Naziv, fc);
           
            sw.Close();
            fs.Close();
            if (!fitnesCentriVlasnika.ContainsKey(fc.KorisnickoImeVlasnika))
            {
                List<string> novi = new List<string>();
                novi.Add(fc.Naziv);
                fitnesCentriVlasnika.Add(fc.KorisnickoImeVlasnika, novi);
                putanja = "~/App_Data/FitnesCentriVlasnika.txt";
                putanja = HostingEnvironment.MapPath(putanja);
                fs = new FileStream(putanja, FileMode.Append);
                sw = new StreamWriter(fs);
                //Vlasnik[0]?FitnesCentar[1]
                sw.WriteLine($"{fc.KorisnickoImeVlasnika}?{fc.Naziv}");
                
                sw.Close();
                fs.Close();
            }
            else
            {
                fitnesCentriVlasnika[fc.KorisnickoImeVlasnika].Add(fc.Naziv);
                putanja = "~/App_Data/FitnesCentriVlasnika.txt";
                putanja = HostingEnvironment.MapPath(putanja);
                fs = new FileStream(putanja, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string[] data = new string[200];

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = sr.ReadLine();
                    if (data[i] == null)
                    {
                        break;
                    }

                }
                sr.Close();
                using ( sw = new StreamWriter(putanja))
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i] == null)
                        {
                            break;
                        }

                        if (data[i].Contains(fc.KorisnickoImeVlasnika))
                        {
                            string temp = "";
                            foreach(var nazivFC in fitnesCentriVlasnika[fc.KorisnickoImeVlasnika])
                            {
                                temp += $"?{nazivFC}"; 
                            }
                            data[i] = $"{fc.KorisnickoImeVlasnika}{temp}";
                        }
                        sw.WriteLine(data[i]);
                    }
                    sw.Close();
                }
            }
        }
        public void SacuvajUBazuTrenerFitnesCentar(string Trener,string FitnesCentar)
        {
            string putanja = "~/App_Data/TrenerFitnesCentar.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //KorisnickoImeTrenera[0]?NazivFitnesCentra[1]
            sw.WriteLine($"{Trener}?{FitnesCentar}");
            trenerFitnesCentar.Add(Tuple.Create(Trener, FitnesCentar));           
            sw.Close();
            fs.Close();
        }
        public void SacuvajUBazuGrupniTrening(GrupniTrening gp,string KorisnickoIme)
        {
            string putanja = "~/App_Data/GrupniTreninzi.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //Naziv[0]?Trening[1]?NazivFC[2]?TrajanjeTreninga[3]?DatumTreninga[4]?MaxPosetioci[5]?Izbrisan(default false)[6]
            sw.WriteLine($"{gp.Naziv}?{gp.Trening}?{gp.NazivFitnesCentra}?{gp.TrajanjeTreninga}?{gp.DatumTreninga}?{gp.MaxBrojPosetilaca}?{gp.Izbrisan}");
            grupniTreninzi.Add(gp.Naziv, gp);

            sw.Close();
            fs.Close();

            if (grupniTreninziTrenera.ContainsKey(KorisnickoIme))
            {
                grupniTreninziTrenera[KorisnickoIme].Add(gp.Naziv);
                putanja = "~/App_Data/GrupniTreninziTrenera.txt";
                putanja = HostingEnvironment.MapPath(putanja);
                fs = new FileStream(putanja, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string[] data = new string[200];

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = sr.ReadLine();
                    if (data[i] == null)
                    {
                        break;
                    }

                }
                sr.Close();
                using (sw = new StreamWriter(putanja))
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i] == null)
                        {
                            break;
                        }

                        if (data[i].Contains(KorisnickoIme))
                        {
                            string temp = "";
                            foreach (var nazivGP in grupniTreninziTrenera[KorisnickoIme])
                            {
                                temp += $"?{nazivGP}";
                            }
                            data[i] = $"{KorisnickoIme}{temp}";
                        }
                        sw.WriteLine(data[i]);
                    }
                    sw.Close();
                }

            }
            else
            {
                List<string> novaLista = new List<string>();
                novaLista.Add(gp.Naziv);
                grupniTreninziTrenera.Add(KorisnickoIme, novaLista);
                putanja = "~/App_Data/GrupniTreninziTrenera.txt";
                putanja = HostingEnvironment.MapPath(putanja);
                fs = new FileStream(putanja, FileMode.Append);
                sw = new StreamWriter(fs);
                //KorisnickoIme[0]?Naziv[1]
                sw.WriteLine($"{KorisnickoIme}?{gp.Naziv}");

                sw.Close();
                fs.Close();
            }
        }
        public void SacuvajUBazuKomentar(Komentar k)
        {
            string putanja = "~/App_Data/Komentari.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //KorisnickoImePosetilac[0]?NazivFC[1]?Tekst[2]?Ocena[3]?Vidljivost[4]?
            sw.WriteLine($"{k.KorisnickoImePosetioca}?{k.NazivFitnesCentra}?{k.TekstKomentara}?{k.Ocena}?{k.Vidljivost}");          
            if (komentariFitnesCentra.ContainsKey(k.NazivFitnesCentra))
            {
                komentariFitnesCentra[k.NazivFitnesCentra].Add(k);
            }
            else
            {
                List<Komentar> komentari = new List<Komentar>();
                komentari.Add(k);
                komentariFitnesCentra.Add(k.NazivFitnesCentra, komentari);
            }
            sw.Close();
            fs.Close();
        }
        public Korisnik PodaciKorisnika(string korisnickoIme)
        {
            Korisnik korisnik = new Korisnik();
            listaKorisnika.TryGetValue(korisnickoIme,out korisnik);
            return korisnik;
        }
        public void IzmeniKorisnika(Korisnik noviKorisnik,Korisnik stariKorisnik)
        {
            string putanja = "~/App_Data/Korisnici.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            List<string> linije = new List<string>();
            string temp = "";

            while ((temp = sr.ReadLine()) != null)
            {

                string[] data = temp.Split('?');
                if (stariKorisnik.KorisnickoIme != data[0])
                {
                    linije.Add(temp);
                }

            }
            sr.Close();
            fs.Close();
            File.Delete(putanja);
            FileStream filestream = new FileStream(putanja, FileMode.Append, FileAccess.Write); 
            using (StreamWriter sw = new StreamWriter(filestream))
            {
                foreach (var line in linije)
                {
                    sw.WriteLine(line);
                }
                //KorisnickoIme[0]?Lozinka[1]?Ime[2]?Prezime[3]?Pol[4]?Email[5]?Datum[6]?Uloga[7]?Izbrisan(default false)[8]
                sw.WriteLine($"{noviKorisnik.KorisnickoIme}?{noviKorisnik.Lozinka}?{noviKorisnik.Ime}?{noviKorisnik.Prezime}?{noviKorisnik.Pol}?{noviKorisnik.Email}?{noviKorisnik.DatumRodjenja}?{noviKorisnik.Uloga}?{noviKorisnik.Izbrisan}");
            }
            filestream.Close();

            listaKorisnika[noviKorisnik.KorisnickoIme] = noviKorisnik;           
        }
        public void IzmeniFitnesCentar(FitnesCentar noviFC)
        {
            string putanja = "~/App_Data/FitnesCentri.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            List<string> linije = new List<string>();
            string temp = "";

            while ((temp = sr.ReadLine()) != null)
            {

                string[] data = temp.Split('?');
                if (noviFC.Naziv != data[0])
                {
                    linije.Add(temp);
                }

            }
            sr.Close();
            fs.Close();
            File.Delete(putanja);
            FileStream filestream = new FileStream(putanja, FileMode.Append, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(filestream))
            {
                foreach (var line in linije)
                {
                    sw.WriteLine(line);
                }
                //Naziv[0]?Adresa[1]?GodinaOtvaranja[2]?Vlasnik[3]?Mesecna[4]?Godisnja[5]?JedanT[6]?Grupni[7]?Personalni[8]?Izbrisan(default false)[9]
                sw.WriteLine($"{noviFC.Naziv}?{noviFC.Adresa}?{noviFC.GodinaOtvaranja}?{noviFC.KorisnickoImeVlasnika}?{noviFC.MesecnaClanarina}?{noviFC.GodisnjaClanarina}?{noviFC.CenaJednogTreninga}?{noviFC.CenaGrupnogTreninga}?{noviFC.CenaPersonalniTrener}?{noviFC.Izbrisan}");
            }
            filestream.Close();

            fitnesCentri[noviFC.Naziv] = noviFC;        
        }
        public void IzmeniGrupniTrening(GrupniTrening noviGT)
        {
            string putanja = "~/App_Data/GrupniTreninzi.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            List<string> linije = new List<string>();
            string temp = "";

            while ((temp = sr.ReadLine()) != null)
            {

                string[] data = temp.Split('?');
                if (noviGT.Naziv != data[0])
                {
                    linije.Add(temp);
                }

            }
            sr.Close();
            fs.Close();
            File.Delete(putanja);
            FileStream filestream = new FileStream(putanja, FileMode.Append, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(filestream))
            {
                foreach (var line in linije)
                {
                    sw.WriteLine(line);
                }
                //Naziv[0]?Trening[1]?NazivFC[2]?TrajanjeTreninga[3]?DatumTreninga[4]?MaxPosetioci[5]?Izbrisan(default false)[6]
                sw.WriteLine($"{noviGT.Naziv}?{noviGT.Trening}?{noviGT.NazivFitnesCentra}?{noviGT.TrajanjeTreninga}?{noviGT.DatumTreninga}?{noviGT.MaxBrojPosetilaca}?{noviGT.Izbrisan}");
            }
            filestream.Close();
            grupniTreninzi[noviGT.Naziv] = noviGT;            
        }
        public void IzmeniKomentar(Komentar k, string KorisnickoIme,string Vidljivost)
        {
            string komentar = $"{k.KorisnickoImePosetioca}?{k.NazivFitnesCentra}?{k.TekstKomentara}?{k.Ocena}?{k.Vidljivost}";
            string putanja = "~/App_Data/Komentari.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            List<string> linije = new List<string>();
            string temp = "";

            while ((temp = sr.ReadLine()) != null)
            {

                string[] data = temp.Split('?');
                if (komentar != temp)
                {
                    linije.Add(temp);
                }

            }
            sr.Close();
            fs.Close();
            File.Delete(putanja);
            FileStream filestream = new FileStream(putanja, FileMode.Append, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(filestream))
            {
                foreach (var line in linije)
                {
                    sw.WriteLine(line);
                }
                //Naziv[0]?Adresa[1]?GodinaOtvaranja[2]?Vlasnik[3]?Mesecna[4]?Godisnja[5]?JedanT[6]?Grupni[7]?Personalni[8]?Izbrisan(default false)[9]
                sw.WriteLine($"{k.KorisnickoImePosetioca}?{k.NazivFitnesCentra}?{k.TekstKomentara}?{k.Ocena}?{Vidljivost}");
            }
            filestream.Close();
            

            if (komentariFitnesCentra.ContainsKey(k.NazivFitnesCentra))
            {
                foreach(var stariKomentar in komentariFitnesCentra[k.NazivFitnesCentra])
                {
                    if(stariKomentar.TekstKomentara == k.TekstKomentara && stariKomentar.KorisnickoImePosetioca == k.KorisnickoImePosetioca && stariKomentar.NazivFitnesCentra == k.NazivFitnesCentra)
                    {
                        stariKomentar.Vidljivost = Vidljivost;
                    }
                }
            }
        }
        public void BlokirajTrenera(string KorisnickoIme)
        {
            string putanja = "~/App_Data/Korisnici.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            StreamReader sr = new StreamReader(putanja);
            string[] data = new string[200];
            
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = sr.ReadLine();
                if (data[i] == null)
                {
                    break;
                }

            }
            sr.Close();
            using (StreamWriter sw = new StreamWriter(putanja))
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == null)
                    {
                        break;
                    }

                    if (data[i].Contains(KorisnickoIme))
                    {
                        data[i] = data[i].Replace("False", "True");
                        //data[i].Remove(i);
                    }
                    sw.WriteLine(data[i]);
                }
                sw.Close();
            }
            listaKorisnika.Remove(KorisnickoIme);
            putanja = "~/App_Data/TrenerFitnesCentar.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            sr = new StreamReader(putanja);
            data = new string[200];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = sr.ReadLine();
                if (data[i] == null)
                {
                    break;
                }

            }
            sr.Close();
            using (StreamWriter sw = new StreamWriter(putanja))
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == null)
                    {
                        break;
                    }

                    if (data[i].Contains(KorisnickoIme))
                    {
                        data[i].Remove(i);
                    }
                    else 
                    {
                        sw.WriteLine(data[i]);
                    }                  
                }
                sw.Close();
            }
            trenerFitnesCentar.Remove(trenerFitnesCentar.Find(x => x.Item1.Equals(KorisnickoIme)));
        }
        public void ObrisiFitnesCentar(string nazivFC,string KorisnickoIme)
        {
            List<Tuple<string,string>> trenerFC = new List<Tuple<string, string>>();
            List<string> izbrisaniTreneri = new List<string>();
            string putanja = "~/App_Data/FitnesCentri.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            StreamReader sr = new StreamReader(putanja);
            string[] data = new string[200];
            
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = sr.ReadLine();
                if (data[i] == null)
                {
                    break;
                }

            }
            sr.Close();
            using (StreamWriter sw = new StreamWriter(putanja))
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == null)
                    {
                        break;
                    }

                    if (data[i].Contains(nazivFC))
                    {
                        data[i] = data[i].Replace("False", "True");                       
                    }
                    sw.WriteLine(data[i]);
                }
                sw.Close();
            }
            fitnesCentri.Remove(nazivFC);




            putanja = "~/App_Data/TrenerFitnesCentar.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            //FileStream fs = new FileStream(putanja, FileMode.Open);
            sr = new StreamReader(putanja);
            data = new string[200];
            List<string> linije = new List<string>();
            string temp = "";

            while ((temp = sr.ReadLine()) != null)
            {

                data = temp.Split('?');
                if (nazivFC!= data[1])
                {
                    linije.Add(temp);
                }

            }
            sr.Close();
            //fs.Close();
            File.Delete(putanja);
            FileStream filestream = new FileStream(putanja, FileMode.Append, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(filestream))
            {
                foreach (var line in linije)
                {
                    sw.WriteLine(line);
                }
                sw.Close();
            }
            filestream.Close();
            foreach (var tuples in trenerFitnesCentar)
            {
                if(tuples.Item2 == nazivFC)
                {
                    putanja = "~/App_Data/Korisnici.txt";
                    putanja = HostingEnvironment.MapPath(putanja);
                    sr = new StreamReader(putanja);
                    data = new string[200];

                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = sr.ReadLine();
                        if (data[i] == null)
                        {
                            break;
                        }

                    }
                    sr.Close();
                    using (StreamWriter sw = new StreamWriter(putanja))
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (data[i] == null)
                            {
                                break;
                            }

                            if (data[i].Contains(tuples.Item1))
                            {
                                data[i] = data[i].Replace("False", "True");
                                izbrisaniTreneri.Add(tuples.Item1);
                                trenerFC.Add(tuples);
                                
                            }
                            sw.WriteLine(data[i]);
                        }
                        sw.Close();
                    }
                    listaKorisnika.Remove(tuples.Item1);
                    
                }
            }
            foreach(var t in trenerFC)
            {
                if (trenerFitnesCentar.Contains(t))
                {
                    trenerFitnesCentar.Remove(t);
                }
            }




            putanja = "~/App_Data/FitnesCentriVlasnika.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            sr = new StreamReader(putanja);
            data = new string[200];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = sr.ReadLine();
                if (data[i] == null)
                {
                    break;
                }

            }
            sr.Close();
            using (StreamWriter sw = new StreamWriter(putanja))
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == null)
                    {
                        break;
                    }

                    if (data[i].Contains(nazivFC))
                    {
                        temp = "";
                        fitnesCentriVlasnika[KorisnickoIme].Remove(nazivFC);
                        foreach (var fc in fitnesCentriVlasnika[KorisnickoIme])
                        {
                            temp += $"?{fc}";
                        }
                        sw.WriteLine($"{KorisnickoIme}{temp}");
                        
                    }
                    else
                    {
                        sw.WriteLine(data[i]);
                    }
                }
                sw.Close();
            }
            
        }
        public void ObrisiGrupniTrening(string nazivGT,string KorisnickoIme)
        {
            string putanja = "~/App_Data/GrupniTreninzi.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            StreamReader sr = new StreamReader(putanja);
            string[] data = new string[200];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = sr.ReadLine();
                if (data[i] == null)
                {
                    break;
                }

            }
            sr.Close();
            using (StreamWriter sw = new StreamWriter(putanja))
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == null)
                    {
                        break;
                    }

                    if (data[i].Contains(nazivGT))
                    {
                        data[i] = data[i].Replace("False", "True");
                    }
                    sw.WriteLine(data[i]);
                }
                sw.Close();
            }
            grupniTreninzi.Remove(nazivGT);


            putanja = "~/App_Data/GrupniTreninziTrenera.txt";
            putanja = HostingEnvironment.MapPath(putanja);
            sr = new StreamReader(putanja);
            data = new string[200];
            string temp = "";

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = sr.ReadLine();
                if (data[i] == null)
                {
                    break;
                }

            }
            sr.Close();
            using (StreamWriter sw = new StreamWriter(putanja))
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == null)
                    {
                        break;
                    }

                    if (data[i].Contains(nazivGT))
                    {
                        temp = "";
                        grupniTreninziTrenera[KorisnickoIme].Remove(nazivGT);
                        foreach (var fc in grupniTreninziTrenera[KorisnickoIme])
                        {
                            temp += $"?{fc}";
                        }
                        sw.WriteLine($"{KorisnickoIme}{temp}");

                    }
                    else
                    {
                        sw.WriteLine(data[i]);
                    }
                }
                sw.Close();
            }

        }
        public void ListaKorisnika()
        {
            //List<Korisnik> korisnici = new List<Korisnik>();

            string putanja = "~/App_Data/Korisnici.txt";
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = "";
            while ((temp = sr.ReadLine()) != null)
            {
                string[] data = temp.Split('?');
                Korisnik korisnik = new Korisnik();
                if (data[8] == "False")
                {
                    //KorisnickoIme[0]?Lozinka[1]?Ime[2]?Prezime[3]?Pol[4]?Email[5]?DatumRodjenja[6]?Uloga[7]?Izbrisan[8]
                    korisnik.KorisnickoIme = data[0];
                    korisnik.Lozinka = data[1];
                    korisnik.Ime = data[2];
                    korisnik.Prezime = data[3];
                    korisnik.Pol = data[4];
                    korisnik.Email = data[5];
                    korisnik.DatumRodjenja = data[6];
                    if(data[7] == "POSETILAC")
                    {
                        korisnik.Uloga = UlogaKorisnika.POSETILAC;
                    }
                    else if (data[7] == "TRENER")
                    {
                        korisnik.Uloga = UlogaKorisnika.TRENER;
                    }
                    else if (data[7] == "VLASNIK")
                    {
                        korisnik.Uloga = UlogaKorisnika.VLASNIK;
                    }
                    
                    if (data[8] == "False")
                    {
                        korisnik.Izbrisan = false;
                        listaKorisnika.Add(korisnik.KorisnickoIme, korisnik);
                    }
                    else
                    {
                        korisnik.Izbrisan = true;
                    }
                }
            }
            sr.Close();
            fs.Close();          
        }
        public void ListaFitnesCentara()
        {
            string putanja = "~/App_Data/FitnesCentri.txt";
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = "";
            while ((temp = sr.ReadLine()) != null)
            {
                string[] data = temp.Split('?');
                FitnesCentar fc = new FitnesCentar();
                
                //Naziv[0]?Adresa[1]?GodinaOtvaranja[2]?KorisnickoImeVlasnika[3]?Mesecna[4]?Godisnja[5]?JedanTrening[6]?Grupni[7]?Personalni[8]?Izbrisan[9]?
                fc.Naziv = data[0];
                fc.Adresa = data[1];
                fc.GodinaOtvaranja =  Int32.Parse(data[2]);
                fc.KorisnickoImeVlasnika = data[3];
                fc.MesecnaClanarina = Int32.Parse(data[4]);
                fc.GodisnjaClanarina = Int32.Parse(data[5]);
                fc.CenaJednogTreninga = Int32.Parse(data[6]);
                fc.CenaGrupnogTreninga = Int32.Parse(data[7]);
                fc.CenaPersonalniTrener = Int32.Parse(data[8]);

                if (data[9] == "False")
                {
                    fc.Izbrisan = false;
                    fitnesCentri.Add(fc.Naziv, fc);
                }
                else
                {
                    fc.Izbrisan = true;
                }
                
            }
            sr.Close();
            fs.Close();
        }
        public void ListaGrupnihTreninga()
        {
            string putanja = "~/App_Data/GrupniTreninzi.txt";
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = "";
            while ((temp = sr.ReadLine()) != null)
            {
                string[] data = temp.Split('?');
                GrupniTrening gt = new GrupniTrening();

                //Naziv[0]?TipTreninga[1]?FitnesCentar[2]TrajanjeTreninga[3]?DatumTreninga[4]?MaxBrojPosetilaca[5]?BrojPrijavljenih[6]?Izbrisan[7]
                gt.Naziv = data[0];
                if (data[1] == "YOGA")
                {
                    gt.Trening = TipTreninga.YOGA;
                }
                else if (data[1] == "BODYPUMP")
                {
                    gt.Trening = TipTreninga.BODYPUMP;
                }
                else
                {
                    gt.Trening = TipTreninga.CARDIO;
                }
                
                gt.NazivFitnesCentra = data[2];
                gt.TrajanjeTreninga = data[3];
                gt.DatumTreninga = data[4];
                gt.MaxBrojPosetilaca = Int32.Parse(data[5]);
               

                if (data[6] == "False")
                {
                    gt.Izbrisan = false;
                    grupniTreninzi.Add(gt.Naziv, gt);
                }
                else
                {
                    gt.Izbrisan = true;
                }

            }
            sr.Close();
            fs.Close();
        }
        public void ListaKomentara()
        {
            string putanja = "~/App_Data/Komentari.txt";
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = "";
            while ((temp = sr.ReadLine()) != null)
            {
                string[] data = temp.Split('?');
                Komentar k = new Komentar();

                //KorisnickoImePosetilac[0]?nazivFC[1]?Tekst[2]Ocena[3]?Vidljivost[4]
                k.KorisnickoImePosetioca = data[0];
                k.NazivFitnesCentra = data[1];
                k.TekstKomentara = data[2];
                k.Ocena = Int32.Parse(data[3]);
                k.Vidljivost = data[4];
                if (komentariFitnesCentra.ContainsKey(k.NazivFitnesCentra))
                {
                    komentariFitnesCentra[k.NazivFitnesCentra].Add(k);
                }
                else
                {
                    List<Komentar> komentari = new List<Komentar>();
                    komentari.Add(k);
                    komentariFitnesCentra.Add(k.NazivFitnesCentra, komentari);
                }

            }
            sr.Close();
            fs.Close();
        }
        public List<GrupniTrening> GrupniTreninziPosetioca(string KorisnickoIme)
        {
            //List<string> lista = new List<string>();
            List<GrupniTrening> treninzi = new List<GrupniTrening>();
            if (grupniTreninziPosetioca.ContainsKey(KorisnickoIme))
            {
                //lista = grupniTreninziPosetioca[KorisnickoIme];
                foreach(var nazivGP in grupniTreninziPosetioca[KorisnickoIme])
                {
                    treninzi.Add(grupniTreninzi[nazivGP]);
                }
            }         
            return treninzi;
        }
        public string TrenerFitnesCentar(string KorisnickoIme)
        {
            foreach(var par in trenerFitnesCentar)
            {
                if(par.Item1 == KorisnickoIme)
                {
                    if(par.Item2 != "")
                    {
                        return par.Item2;
                    }
                    else
                    {
                        return "Ne radi jos uvek nigde";
                    }
                    
                }
            }
            return "Ne postoji trener??";
        }
        public void GrupniTreninziTrenera()
        {
            string putanja = "~/App_Data/GrupniTreninziTrenera.txt";
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = "";
            while ((temp = sr.ReadLine()) != null)
            {
                List<string> treninzi = new List<string>();
                string[] data = temp.Split('?');

                //KorisnickoIme[0]?[naziv][1]?
                for (int i = 1; i < data.Length; ++i)
                {
                    treninzi.Add(data[i]);
                }
                grupniTreninziTrenera.Add(data[0], treninzi);

            }
            sr.Close();
            fs.Close();
        }
        public void PosetiociGrupnogTreninga()
        {
            string putanja = "~/App_Data/PosetiociGrupnogTreninga.txt";
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = "";
            while ((temp = sr.ReadLine()) != null)
            {
                List<string> korisnici = new List<string>();
                string[] data = temp.Split('?');

                //naziv[0]?[KorisnickoIme][1]?
                for (int i = 1; i < data.Length; ++i)
                {
                    korisnici.Add(data[i]);
                }
                posetiociGrupnogTreninga.Add(data[0], korisnici);

            }
            sr.Close();
            fs.Close();
        }
        public List<string> FitnesCentriVlasnika(string KorisnickoIme)
        {
            List<string> fitnesCentri = new List<string>();
            fitnesCentriVlasnika.TryGetValue(KorisnickoIme, out fitnesCentri);
            return fitnesCentri;
        }
        public List<FitnesCentar> FitnesCentriVlasnika2(string KorisnickoIme)
        {
            List<FitnesCentar> fc = new List<FitnesCentar>();
            foreach(var fitnesCentar in fitnesCentri.Values)
            {
                if(fitnesCentar.KorisnickoImeVlasnika == KorisnickoIme)
                {
                    fc.Add(fitnesCentar);
                }
            }
            return fc;
        }
        public List<GrupniTrening> GrupniTreninziFitnesCentra(string NazivFitnesCentra)
        {
            List<GrupniTrening> gp = new List<GrupniTrening>();
            foreach(var trening in grupniTreninzi.Values)
            {
                if(trening.NazivFitnesCentra == NazivFitnesCentra)
                {
                    gp.Add(trening);
                }
            }
            return gp;
        }
        public List<GrupniTrening> GrupniTreninziTrenera(string KorisnickoIme)
        {
            List<GrupniTrening> gp = new List<GrupniTrening>();
            if (grupniTreninziTrenera.ContainsKey(KorisnickoIme))
            {
                foreach (var s in grupniTreninziTrenera[KorisnickoIme])
                {
                    foreach (var grupniTrening in grupniTreninzi.Values)
                    {
                        if (grupniTrening.Naziv == s)
                        {
                            gp.Add(grupniTrening);
                            break;
                        }
                    }
                }
            }           
            return gp;
        }
        public List<Korisnik> TreneriVlasnika(string KorisnickoIme)
        {
            List<Korisnik> treneri = new List<Korisnik>();
            Korisnik trener = new Korisnik();
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
            List<string> fc = new List<string>();
            fitnesCentriVlasnika.TryGetValue(KorisnickoIme, out fc);
            if (fc != null)
            {
                foreach (var item in fc)
                {
                    foreach(var t in trenerFitnesCentar)
                    {
                        if(t.Item2 == item)
                        {
                            listaKorisnika.TryGetValue(t.Item1, out trener);
                            treneri.Add(trener);
                        }
                    }
                }
            }
            
            return treneri;
        }
        public List<FitnesCentar> SortiranjeFC(string tipSortiranja,string sortiranjePrema)
        {
            List<FitnesCentar> fc = new List<FitnesCentar>();           
            fc = fitnesCentri.Values.ToList();
            if (tipSortiranja == "RASTUCE")
            {
                if(sortiranjePrema == "Naziv")
                {
                    fc.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));
                }
                else if(sortiranjePrema == "Adresa")
                {
                    //fc.Sort((x, y) => x.Adresa.Split(',')[1].CompareTo(y.Adresa.Split(',')[1]));
                    fc.Sort((x, y) => x.Adresa.CompareTo(y.Adresa));
                }
                else if(sortiranjePrema == "GodinaOtvaranja")
                {
                    fc.Sort((x, y) => x.GodinaOtvaranja.CompareTo(y.GodinaOtvaranja));
                }
               
            }
            else if(tipSortiranja == "OPADAJUCE")
            {
               
                if (sortiranjePrema == "Naziv")
                {
                    fc.Sort((x, y) => y.Naziv.CompareTo(x.Naziv));
                }
                else if (sortiranjePrema == "Adresa")
                {
                   //fc.Sort((x, y) => y.Adresa.Split(',')[1].CompareTo(x.Adresa.Split(',')[1]));
                   fc.Sort((x, y) => y.Adresa.CompareTo(x.Adresa));
                }
                else if (sortiranjePrema == "GodinaOtvaranja")
                {
                    fc.Sort((x, y) => y.GodinaOtvaranja.CompareTo(x.GodinaOtvaranja));
                }
            }
            else
            {
                fc.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));
            }
            
            return fc;
        } 
        public List<FitnesCentar> PretragaFC(string Naziv,string Adresa,string DonjaGranica,string GornjaGranica)
        {
            List<FitnesCentar> centri = new List<FitnesCentar>();
            List<FitnesCentar> noviFC = new List<FitnesCentar>();
            centri = fitnesCentri.Values.ToList();

            
            foreach(var fc in centri)
            {
                if (!string.IsNullOrEmpty(Naziv))
                {
                    if(!(fc.Naziv == Naziv))
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(Adresa))
                {
                    if (!(fc.Adresa.Contains(Adresa)))
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(DonjaGranica))
                {
                    if ((fc.GodinaOtvaranja < Int32.Parse(DonjaGranica)))
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(GornjaGranica))
                {
                    if ((fc.GodinaOtvaranja > Int32.Parse(GornjaGranica)))
                    {
                        continue;
                    }
                }
                noviFC.Add(fc);
            }
          
            return noviFC;
        }
        public List<GrupniTrening> SortiranjeGT(string tipSortiranja, string sortiranjePrema, string KorisnickoIme)
        {
            List<GrupniTrening> gt = new List<GrupniTrening>();
            if (listaKorisnika[KorisnickoIme].Uloga == UlogaKorisnika.POSETILAC)
            {
                gt = GrupniTreninziPosetioca(KorisnickoIme);
            }
            else
            {
                gt = GrupniTreninziTrenera(KorisnickoIme);
            }
            
            if (tipSortiranja == "RASTUCE")
            {
                if (sortiranjePrema == "Naziv")
                {
                    gt.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));
                }
                else if (sortiranjePrema == "TipTreninga")
                {
                    
                    gt.Sort((x, y) => x.Trening.ToString().CompareTo(y.Trening.ToString()));
                }
                else if (sortiranjePrema == "DatumOdrzavanja")
                {
                    gt.Sort((x, y) => x.DatumTreninga.CompareTo(y.DatumTreninga));
                }

            }
            else if (tipSortiranja == "OPADAJUCE")
            {

                if (sortiranjePrema == "Naziv")
                {
                    gt.Sort((x, y) => y.Naziv.CompareTo(x.Naziv));
                }
                else if (sortiranjePrema == "TipTreninga")
                {
                    gt.Sort((x, y) => y.Trening.ToString().CompareTo(x.Trening.ToString()));
                }
                else if (sortiranjePrema == "DatumOdrzavanja")
                {
                    gt.Sort((x, y) => y.DatumTreninga.CompareTo(x.DatumTreninga));
                }
            }
            else
            {
                gt.Sort((x, y) => x.Naziv.CompareTo(y.Naziv));
            }

            return gt;
        }
        public List<GrupniTrening> PretragaGT(string Naziv, string TipTreninga, string DonjaGranica, string GornjaGranica,string KorisnickoIme)
        {
            List<GrupniTrening> treninzi = new List<GrupniTrening>();
            List<GrupniTrening> noviGT = new List<GrupniTrening>();

            if (listaKorisnika[KorisnickoIme].Uloga == UlogaKorisnika.POSETILAC)
            {
                treninzi = GrupniTreninziPosetioca(KorisnickoIme);
            }
            else
            {
                treninzi = GrupniTreninziTrenera(KorisnickoIme);
            }

            foreach (var gt in treninzi)
            {
                if (!string.IsNullOrEmpty(Naziv))
                {
                    if (!(gt.Naziv == Naziv))
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(TipTreninga))
                {
                    if (!(gt.Trening.ToString() == TipTreninga))
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(DonjaGranica))
                {
                    if ((DateTime.Parse(gt.DatumTreninga) < DateTime.Parse(DonjaGranica)))
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(GornjaGranica))
                {
                    if ((DateTime.Parse(gt.DatumTreninga) > DateTime.Parse(GornjaGranica)))
                    {
                        continue;
                    }
                }
                noviGT.Add(gt);
            }

            return noviGT;
        }
        public void FitnesCentriVlasnika()
        {   
            string putanja = "~/App_Data/FitnesCentriVlasnika.txt";
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = "";
            while ((temp = sr.ReadLine()) != null)
            {
                List<string> centri = new List<string>();
                string[] data = temp.Split('?');              
                
                //KorisnickoIme[0]?[naziv,datumTreninga][1]?
                for (int i = 1; i < data.Length; ++i)
                {
                    centri.Add(data[i]);
                }
                fitnesCentriVlasnika.Add(data[0], centri);
               
            }
            sr.Close();
            fs.Close();
        }
        public void GrupniTreninziPosetioca()
        {
            string putanja = "~/App_Data/GrupniTreninziPosetioca.txt";
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = "";
            while ((temp = sr.ReadLine()) != null)
            {
                string[] data = temp.Split('?');
               // string[] data2;
                List<string> treninzi = new List<string>();
                for (int i = 1; i < data.Length; ++i)
                {
                    //data2 = data[i].Split(',');
                    treninzi.Add(data[i]);
                    //grupniTreninziPosetioca[data[0]].Add(data[i]);
                }
                grupniTreninziPosetioca.Add(data[0], treninzi);

            }
            sr.Close();
            fs.Close();
        }
        public void TrenerFitnesCentar()
        {
            string putanja = "~/App_Data/TrenerFitnesCentar.txt";
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream fs = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = "";
            while ((temp = sr.ReadLine()) != null)
            {
                string[] data = temp.Split('?');              
                //Trener[0]?FitnesCentar[1]
                trenerFitnesCentar.Add(Tuple.Create(data[0], data[1]));
            }
            sr.Close();
            fs.Close();
        }

        public List<Komentar> ListaKomentara(string nazivFC)
        {
            if (komentariFitnesCentra.ContainsKey(nazivFC))
            {
                return komentariFitnesCentra[nazivFC]
;           }
            return new List<Komentar>();
        }
        public List<Komentar> ListaKomentaraVlasnika(string KorisnickoIme)
        {
            List<Komentar> lista = new List<Komentar>();
            foreach (var fc in fitnesCentriVlasnika[KorisnickoIme])
            {
                if (komentariFitnesCentra.ContainsKey(fc))
                {
                    foreach(var k in komentariFitnesCentra[fc])
                    {
                        lista.Add(k);
                    }
                }
            }
            return lista;
        }
        public void DodajGrupniTreningPosetilac(string KorisnickoIme,string NazivGP)
        {
            string putanja = "~/App_Data/GrupniTreninziPosetioca.txt";
            putanja = HostingEnvironment.MapPath(putanja);
          
            //StreamWriter sw = new StreamWriter(fs);         

            if (grupniTreninziPosetioca.ContainsKey(KorisnickoIme))
            {
                grupniTreninziPosetioca[KorisnickoIme].Add(NazivGP);
                FileStream fs = new FileStream(putanja, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string[] data = new string[200];

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = sr.ReadLine();
                    if (data[i] == null)
                    {
                        break;
                    }

                }
                sr.Close();
                using (StreamWriter sw = new StreamWriter(putanja))
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i] == null)
                        {
                            break;
                        }

                        if (data[i].Contains(KorisnickoIme))
                        {
                            string temp = "";
                            foreach (var nazivGP in grupniTreninziPosetioca[KorisnickoIme])
                            {
                                temp += $"?{nazivGP}";
                            }
                            data[i] = $"{KorisnickoIme}{temp}";
                        }
                        sw.WriteLine(data[i]);
                    }
                    sw.Close();
                }

            }
            else
            {
                List<string> novaLista = new List<string>();
                novaLista.Add(NazivGP);
                grupniTreninziPosetioca.Add(KorisnickoIme, novaLista);
                FileStream fs = new FileStream(putanja, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //KorisnickoIme[0]?Naziv[1]
                sw.WriteLine($"{KorisnickoIme}?{NazivGP}");

                sw.Close();
                fs.Close();
            }

            if (posetiociGrupnogTreninga.ContainsKey(NazivGP))
            {
                posetiociGrupnogTreninga[NazivGP].Add(KorisnickoIme);
                putanja = "~/App_Data/PosetiociGrupnogTreninga.txt";
                putanja = HostingEnvironment.MapPath(putanja);
                FileStream fs = new FileStream(putanja, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string[] data = new string[200];

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = sr.ReadLine();
                    if (data[i] == null)
                    {
                        break;
                    }

                }
                sr.Close();
                using (StreamWriter sw = new StreamWriter(putanja))
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i] == null)
                        {
                            break;
                        }

                        if (data[i].Contains(NazivGP))
                        {
                            string temp = "";
                            foreach (var ime in posetiociGrupnogTreninga[NazivGP])
                            {
                                temp += $"?{ime}";
                            }
                            data[i] = $"{NazivGP}{temp}";
                        }
                        sw.WriteLine(data[i]);
                    }
                    sw.Close();
                }

            }
            else
            {
                List<string> novaLista = new List<string>();
                novaLista.Add(KorisnickoIme);
                posetiociGrupnogTreninga.Add(NazivGP, novaLista);
                putanja = "~/App_Data/PosetiociGrupnogTreninga.txt";
                putanja = HostingEnvironment.MapPath(putanja);
                FileStream fs = new FileStream(putanja, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //KorisnickoIme[0]?Naziv[1]
                sw.WriteLine($"{NazivGP}?{KorisnickoIme}");

                sw.Close();
                fs.Close();
            }

        }
        public bool ProveraPrijavaTreninga(string KorisnickoIme,string NazivGP)
        {
            //maksimalno jednom da se prijavi i provera da li je prijavljen maksimalan broj ucesnika
            if (posetiociGrupnogTreninga.ContainsKey(NazivGP))
            {
                if (posetiociGrupnogTreninga[NazivGP].Count() == grupniTreninzi[NazivGP].MaxBrojPosetilaca)
                {
                    return true;
                }
            }

            if (grupniTreninziPosetioca.ContainsKey(KorisnickoIme))
            {
                if (grupniTreninziPosetioca[KorisnickoIme].Contains(NazivGP))
                {
                    return true;
                }
            }
            return false;
        }
        public bool ProveraPosetilacKomentar(string Posetilac,string NazivFC)
        {
            List<GrupniTrening> lista = new List<GrupniTrening>();
            lista = GrupniTreninziFitnesCentra(NazivFC);
            foreach (var nazivGT in grupniTreninziPosetioca[Posetilac])
            {
                foreach(var tr in lista)
                {
                    if(tr.Naziv == nazivGT)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool ProveraBuduciTreninziFC(string NazivFC)
        {
            List<GrupniTrening> gp = new List<GrupniTrening>();
            gp = GrupniTreninziFitnesCentra(NazivFC);
            foreach (var trening in gp)
            {
                if(trening.NazivFitnesCentra == NazivFC)
                {
                    if(DateTime.Parse(trening.DatumTreninga) > DateTime.Now)
                    {
                        return true;
                    }
                    
                }
            }
            return false;
        }
        public bool UserNameCheck(string korisnickoIme)
        {
            if (listaKorisnika.ContainsKey(korisnickoIme))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ValidacijaDatuma(string Datum)
        {
            DateTime result;
            if (DateTime.TryParse(Datum, out result))
            {
                return true;
            }
            return false;

        }
        public bool ProveraUlogovanogKorisnika(string KorisnickoIme,string sifra)
        {
            foreach (var user in listaKorisnika.Values)
            {
                if (user.Lozinka == sifra && user.KorisnickoIme == KorisnickoIme)
                {
                    return true;
                }
            }
            return false;
               
        }
        public string UlogaCheck(string KorisnickoIme)
        {
            foreach (var user in listaKorisnika.Values)
            {
                if(user.KorisnickoIme == KorisnickoIme)
                {
                    return user.Uloga.ToString();
                }
               
            }
            return "";
        }

    }
}