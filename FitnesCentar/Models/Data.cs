using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace FitnesCentar.Models
{
    public class Data
    {
        public static List<Fitnes_Centar> GetFitnesCentar(string path)
        {
            List<Fitnes_Centar> centri = new List<Fitnes_Centar>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                string[] delovi = line.Split(';');
                Fitnes_Centar centar = new Fitnes_Centar();
                centar.Naziv = delovi[0];
                centar.Adresa = delovi[1];

                try
                {

                    centar.Godina_otvaranja = Int32.Parse(delovi[2]);

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }

                string[] delovi_vlasnik = delovi[3].Split(',');
                Korisnik korisnik = new Korisnik();
                korisnik.Ime = delovi_vlasnik[0];
                korisnik.Prezime = delovi_vlasnik[1];
                korisnik.Email = delovi_vlasnik[2];
                korisnik.Uloga = Uloga.VLASNIK;

                centar.Vlasnik = korisnik;

                try
                {

                    centar.Cena_mesecne_clanarine = Int32.Parse(delovi[4]);

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }

                try
                {

                    centar.Cena_godisnje_clanarine = Int32.Parse(delovi[5]);

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }

                try
                {

                    centar.Cena_jednog_treninga = Int32.Parse(delovi[6]);

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }

                try
                {

                    centar.Cena_jednog_grupni = Int32.Parse(delovi[7]);

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }

                try
                {

                    centar.Cena_jednog_trener = Int32.Parse(delovi[8]);

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }

                if (delovi[9].Equals("False")) centar.IsDeleted = false;
                else centar.IsDeleted = true;

                centri.Add(centar);

            }
            reader.Close();
            stream.Close();

            return centri;
        }

        public static List<GrupniTrening> GetTreninzi(string path)
        {

            List<GrupniTrening> treninzi = new List<GrupniTrening>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                string[] delovi = line.Split(';');
                GrupniTrening trening = new GrupniTrening();
                trening.Naziv = delovi[0];

                if (delovi[1].Contains("PUMP")) trening.TipTreninga = TipTreninga.BODY_PUMP;
                else if (delovi[1].Contains("BALANCE")) trening.TipTreninga = TipTreninga.LES_MILLS_BODYBALANCE;
                else if (delovi[1].Contains("TONE")) trening.TipTreninga = TipTreninga.LES_MILLS_TONE;
                else if (delovi[1].Contains("YOGA")) trening.TipTreninga = TipTreninga.YOGA;
                else trening.TipTreninga = TipTreninga.CARDIO;

                Fitnes_Centar centar = new Fitnes_Centar();
                centar.Naziv = delovi[2];
                trening.FitnesCentar = centar;

                try
                {

                    trening.Trajanje = Int32.Parse(delovi[3]);

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }


                string[] both = delovi[4].Split(' ');
                string[] date = both[0].Split('/');
                string[] time = both[1].Split(':');
                DateTime dateTime;

                try
                {

                    dateTime = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]),
                    Int32.Parse(time[0]), Int32.Parse(time[1]), 0);

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }

                trening.Date_time = dateTime;

                try
                {

                    trening.Max_posetilaca = Int32.Parse(delovi[5]);

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }

                if (delovi[6].Equals("False")) trening.IsDeleted = false;
                else trening.IsDeleted = true;


                if (delovi.Length > 7)
                {
                    List<Korisnik> posetioci = new List<Korisnik>();
                    string[] korisnici = delovi[7].Split(',');
                    foreach (string s in korisnici)
                    {
                        Korisnik k = new Korisnik();
                        k.Username = s;
                        posetioci.Add(k);
                    }
                    trening.Posetioci = posetioci;
                }
                else
                {
                    List<Korisnik> posetioci = new List<Korisnik>();
                    trening.Posetioci = posetioci;
                }


                treninzi.Add(trening);
            }
            reader.Close();
            stream.Close();

            return treninzi;
        }

        public static List<Komentar> GetKomentari(string path)
        {
            List<Komentar> komentari = new List<Komentar>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                string[] delovi = line.Split(';');
                Komentar komentar = new Komentar();
                Korisnik korisnik = new Korisnik();
                korisnik.Username = delovi[0];
                komentar.Posetilac = korisnik;

                Fitnes_Centar centar = new Fitnes_Centar();
                centar.Naziv = delovi[1];
                komentar.FitnesCentar = centar;

                komentar.Opis = delovi[2];

                try
                {

                    komentar.Ocena = Int32.Parse(delovi[3]);

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }

                if (delovi[4].Equals("False")) komentar.Blokiran = false;
                else komentar.Blokiran = true;

                komentari.Add(komentar);
            }
            reader.Close();
            stream.Close();

            return komentari;
        }

        public static List<Korisnik> GetKorisnici(string path)
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                string[] delovi = line.Split(';');
                Korisnik korisnik = new Korisnik();
                korisnik.Username = delovi[0];
                korisnik.Password = delovi[1];
                korisnik.Ime = delovi[2];
                korisnik.Prezime = delovi[3];

                if (delovi[4].Equals("M")) korisnik.Pol = Pol.MUSKI;
                else korisnik.Pol = Pol.ZENSKI;

                korisnik.Email = delovi[5];

                string[] date = delovi[6].Split(':');
                DateTime dateTime;

                try
                {

                    dateTime = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]));

                }
                catch (Exception e)
                {
                    throw new Exception("Greska pri parsiranju", e);
                }


                korisnik.Datum_rodjenja = dateTime;

                if (delovi[7].Equals("VLASNIK"))
                {
                    korisnik.Uloga = Uloga.VLASNIK;
                    List<Fitnes_Centar> centar = new List<Fitnes_Centar>();
                    string[] centri = delovi[8].Split(',');

                    foreach (string s in centri)
                    {
                        Fitnes_Centar fitnes_Centar = new Fitnes_Centar();
                        fitnes_Centar.Naziv = s;
                        centar.Add(fitnes_Centar);
                    }
                    korisnik.Fitnescentri = centar;
                }
                else if (delovi[7].Equals("TRENER"))
                {
                    if (delovi.Length == 11)
                    {

                        korisnik.Uloga = Uloga.TRENER;
                        Fitnes_Centar fitnes_Centar = new Fitnes_Centar();
                        fitnes_Centar.Naziv = delovi[9];
                        korisnik.FitnesCentar = fitnes_Centar;
                        string[] treninzi = delovi[8].Split(',');
                        List<GrupniTrening> grupni = new List<GrupniTrening>();

                        foreach (string s in treninzi)
                        {
                            GrupniTrening trening = new GrupniTrening();
                            trening.Naziv = s;
                            grupni.Add(trening);
                        }

                        korisnik.Trener_angazovanje = grupni;
                        if (delovi[10].Equals("False")) korisnik.Blokiran = false;
                        else korisnik.Blokiran = true;
                    }
                    else
                    {
                        korisnik.Uloga = Uloga.TRENER;
                        Fitnes_Centar fitnes_Centar = new Fitnes_Centar();
                        fitnes_Centar.Naziv = delovi[8];
                        korisnik.FitnesCentar = fitnes_Centar;
                        List<GrupniTrening> grupni = new List<GrupniTrening>();
                        korisnik.Trener_angazovanje = grupni;
                        if (delovi[9].Equals("False")) korisnik.Blokiran = false;
                        else korisnik.Blokiran = true;
                    }

                }
                else if (delovi[7].Equals("POSETILAC") && delovi.Length > 8)
                {
                    korisnik.Uloga = Uloga.POSETILAC;
                    List<GrupniTrening> treninzi = new List<GrupniTrening>();
                    string[] nazivi = delovi[8].Split(',');
                    foreach (string s in nazivi)
                    {
                        GrupniTrening trening = new GrupniTrening();
                        trening.Naziv = s;
                        treninzi.Add(trening);
                    }
                    korisnik.Treninzi_prijavljen = treninzi;

                }
                korisnici.Add(korisnik);


            }
            reader.Close();
            stream.Close();

            return korisnici;
        }
        public static void PutKorisnik(Korisnik k, string path)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            string data = "";
            string pol = "";
            string datum = "";
            string uloga = "";

            data += k.Username + ";" + k.Password + ";" + k.Ime + ";" + k.Prezime + ";";

            if (k.Pol == Pol.MUSKI) pol = "M";
            else pol = "Z";

            data += pol + ";" + k.Email + ";";

            datum = k.Datum_rodjenja.Day + ":" + k.Datum_rodjenja.Month + ":" + k.Datum_rodjenja.Year;

            if (k.Uloga == Uloga.POSETILAC) uloga = "POSETILAC";
            else if (k.Uloga == Uloga.TRENER) uloga = "TRENER";

            data += datum + ";" + uloga;

            if (k.Uloga == Uloga.POSETILAC)
            {
                if (k.Treninzi_prijavljen.Any())
                {
                    foreach (GrupniTrening trening in k.Treninzi_prijavljen)
                    {
                        data += trening.Naziv + ",";
                    }
                    data += ";";
                }
            }
            else if (k.Uloga == Uloga.TRENER)
            {
                if (k.Trener_angazovanje.Any())
                {
                    foreach (GrupniTrening trening in k.Trener_angazovanje)
                    {
                        data += trening.Naziv + ",";
                    }
                    data += ";";
                }
                data += ";" + k.FitnesCentar.Naziv + ";" + k.Blokiran;
            }
            else
            {
                if (k.Fitnescentri.Any())
                {
                    foreach (Fitnes_Centar centar in k.Fitnescentri)
                    {
                        data += centar.Naziv + ",";
                    }
                    data += ";";
                }
            }
            writer.WriteLine(data);

            writer.Close();
            stream.Close();

        }
        public static void IzmenaKorisnik(Korisnik k, string path, bool izbor)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            string tempFile = Path.GetTempFileName();
            StreamWriter writer = new StreamWriter(tempFile);
            StreamReader reader = new StreamReader(stream);

            string line = "";
            string trenutni = "";
            string temp = "";
            string temp2 = "";
            string[] delovi;
            while ((line = reader.ReadLine()) != null)
            {
                delovi = line.Split(';');
                if (!delovi[0].Equals(k.Username))
                {
                    writer.WriteLine(line);
                }
                else trenutni = line;
            }

            delovi = trenutni.Split(';');

            if (k.Uloga == Uloga.POSETILAC)
            {
                if (izbor == false)
                {

                    if (k.Treninzi_prijavljen != null)
                    {
                        if (k.Treninzi_prijavljen.Count == 1)
                        {
                            trenutni += ";" + k.Treninzi_prijavljen[0].Naziv;
                        }
                        else
                        {
                            trenutni += "," + k.Treninzi_prijavljen[k.Treninzi_prijavljen.Count - 1].Naziv;
                        }
                    }
                }
                else
                {
                    if (!delovi[2].Equals(k.Ime) || !delovi[3].Equals(k.Prezime) || !delovi[4].Equals(k.Pol.ToString()[0]) || !delovi[6].Equals(k.Datum_rodjenja))
                    {
                        delovi[2] = k.Ime;
                        delovi[3] = k.Prezime;
                        delovi[4] = k.Pol.ToString()[0].ToString();
                        delovi[6] = k.Datum_rodjenja.Day + ":" + k.Datum_rodjenja.Month + ":" + k.Datum_rodjenja.Year;
                    }
                }
            }
            else if(k.Uloga == Uloga.TRENER)
            {
                if (izbor == false)
                {

                    if (k.Trener_angazovanje != null)
                    {
                        if (k.Trener_angazovanje.Count == 1)
                        {
                            temp = delovi[9];
                            delovi[9] = k.Trener_angazovanje[0].Naziv;
                        }
                        else
                        {
                            delovi[8] += "," + k.Trener_angazovanje[k.Trener_angazovanje.Count - 1].Naziv;
                        }
                    }
                }
                else
                {
                    if (k.Trener_angazovanje.Count != 0)
                    {
                        if (!delovi[2].Equals(k.Ime) || !delovi[3].Equals(k.Prezime) || !delovi[4].Equals(k.Pol.ToString()[0]) || !delovi[6].Equals(k.Datum_rodjenja)
                            || !delovi[10].Equals(k.Blokiran))
                        {
                            delovi[2] = k.Ime;
                            delovi[3] = k.Prezime;
                            delovi[4] = k.Pol.ToString()[0].ToString();
                            delovi[6] = k.Datum_rodjenja.Day + ":" + k.Datum_rodjenja.Month + ":" + k.Datum_rodjenja.Year;
                            delovi[10] = k.Blokiran.ToString();
                        }
                    }
                    else
                    {
                        if (!delovi[2].Equals(k.Ime) || !delovi[3].Equals(k.Prezime) || !delovi[4].Equals(k.Pol.ToString()[0]) || !delovi[6].Equals(k.Datum_rodjenja)
                            || !delovi[9].Equals(k.Blokiran))
                        {
                            delovi[2] = k.Ime;
                            delovi[3] = k.Prezime;
                            delovi[4] = k.Pol.ToString()[0].ToString();
                            delovi[6] = k.Datum_rodjenja.Day + ":" + k.Datum_rodjenja.Month + ":" + k.Datum_rodjenja.Year;
                            delovi[9] = k.Blokiran.ToString();
                        }
                    }
                }
                trenutni = String.Join(";", delovi);

            }
            else if(k.Uloga == Uloga.VLASNIK)
            {
                if (izbor == false)
                {

                    if (k.Fitnescentri != null)
                    {
                        if (k.Fitnescentri.Count == 1)
                        {
                            temp2 += ";" + k.Fitnescentri[0].Naziv;
                        }
                        else
                        {
                            temp2 += "," + k.Fitnescentri[k.Fitnescentri.Count - 1].Naziv;
                        }
                    }
                }
                else
                {
                    if (!delovi[2].Equals(k.Ime) || !delovi[3].Equals(k.Prezime) || !delovi[4].Equals(k.Pol.ToString()[0]) || !delovi[6].Equals(k.Datum_rodjenja))
                    {
                        delovi[2] = k.Ime;
                        delovi[3] = k.Prezime;
                        delovi[4] = k.Pol.ToString()[0].ToString();
                        delovi[6] = k.Datum_rodjenja.Day + ":" + k.Datum_rodjenja.Month + ":" + k.Datum_rodjenja.Year;
                    }
                }
                trenutni = String.Join(";", delovi);
            }


            if(temp != String.Empty)
            {
                trenutni += ";" + temp;
            }
            if(temp2 != String.Empty)
            {
                trenutni += temp2;
            }

            writer.WriteLine(trenutni);

            writer.Close();
            reader.Close();
            stream.Close();

            File.WriteAllText(path, String.Empty);
            string[] content = File.ReadAllLines(tempFile);
            File.WriteAllLines(path, content);

        }
        public static void IzmenaGrupniTrening(GrupniTrening trening, string path)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            string tempFile = Path.GetTempFileName();
            StreamWriter writer = new StreamWriter(tempFile);
            StreamReader reader = new StreamReader(stream);

            string line = "";
            string trenutni = "";
            string[] delovi;

            while ((line = reader.ReadLine()) != null)
            {
                delovi = line.Split(';');
                if (!delovi[0].Equals(trening.Naziv))
                {
                    writer.WriteLine(line);
                }
                else trenutni = line;
            }

            if (trening.Posetioci != null)
            {
                if (trening.Posetioci.Count == 1)
                {
                    trenutni += ";" + trening.Posetioci[0].Username;
                }
                else if (trening.Posetioci.Count > 1)
                {
                    trenutni += "," + trening.Posetioci[trening.Posetioci.Count - 1].Username;
                }
            }

            delovi = trenutni.Split(';');

            if (!delovi[1].Equals(trening.TipTreninga.ToString()) || !delovi[3].Equals(trening.Trajanje)
                || !delovi[4].Equals(trening.Date_time) || !delovi[5].Equals(trening.Max_posetilaca) || trening.IsDeleted == true)
            {
                delovi[1] = trening.TipTreninga.ToString();
                delovi[3] = trening.Trajanje.ToString();
                delovi[4] = trening.Date_time.Day + "/" + trening.Date_time.Month + "/" + trening.Date_time.Year + " " + 
                    trening.Date_time.Hour + ":" + trening.Date_time.Minute;
                delovi[5] = trening.Max_posetilaca.ToString();
                delovi[6] = trening.IsDeleted.ToString();
            }

            trenutni = String.Join(";", delovi);

            writer.WriteLine(trenutni);

            writer.Close();
            reader.Close();
            stream.Close();

            File.WriteAllText(path, String.Empty);
            string[] content = File.ReadAllLines(tempFile);
            File.WriteAllLines(path, content);

        }
        public static void PutKomentar(Komentar komentar, Korisnik korisnik, string path)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            string data = "";

            data += korisnik.Username + ";" + komentar.FitnesCentar.Naziv + ";" + komentar.Opis + ";" + komentar.Ocena + ";" + komentar.Blokiran;

            writer.WriteLine(data);

            writer.Close();
            stream.Close();
        }
        public static void PutGrupniTrening(GrupniTrening trening, string path)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            string data = "";
            string datum = "";

            data += trening.Naziv + ";" + trening.TipTreninga.ToString() + ";" + trening.FitnesCentar.Naziv + ";" + trening.Trajanje + ";";

            datum = trening.Date_time.Day + "/" + trening.Date_time.Month + "/" + trening.Date_time.Year
                     + " " + trening.Date_time.Hour + ":" + trening.Date_time.Minute;

            data += datum + ";" + trening.Max_posetilaca + ";" + trening.IsDeleted;

            writer.WriteLine(data);

            writer.Close();
            stream.Close();
        }
        public static void ObrisiFitnesCentar(Fitnes_Centar centar, string path)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            string tempFile = Path.GetTempFileName();
            StreamWriter writer = new StreamWriter(tempFile);
            StreamReader reader = new StreamReader(stream);

            string line = "";
            string trenutni = "";
            string[] delovi;
            while ((line = reader.ReadLine()) != null)
            {
                delovi = line.Split(';');
                if (!delovi[0].Equals(centar.Naziv))
                {
                    writer.WriteLine(line);
                }
                else trenutni = line;
            }

            delovi = trenutni.Split(';');
            delovi[delovi.Length - 1] = centar.IsDeleted.ToString();

            trenutni = String.Join(";", delovi);

            writer.WriteLine(trenutni);

            writer.Close();
            reader.Close();
            stream.Close();

            File.WriteAllText(path, String.Empty);
            string[] content = File.ReadAllLines(tempFile);
            File.WriteAllLines(path, content);
        }
        public static void IzmenaFItnesCentar(Fitnes_Centar centar, string path)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            string tempFile = Path.GetTempFileName();
            StreamWriter writer = new StreamWriter(tempFile);
            StreamReader reader = new StreamReader(stream);

            string line = "";
            string trenutni = "";
            string[] delovi;

            while ((line = reader.ReadLine()) != null)
            {
                delovi = line.Split(';');
                if (!delovi[0].Equals(centar.Naziv))
                {
                    writer.WriteLine(line);
                }
                else trenutni = line;
            }

            delovi = trenutni.Split(';');

            if (!delovi[1].Equals(centar.Adresa) || !delovi[2].Equals(centar.Godina_otvaranja) || !delovi[4].Equals(centar.Cena_mesecne_clanarine)
                || !delovi[5].Equals(centar.Cena_godisnje_clanarine) || !delovi[6].Equals(centar.Cena_jednog_treninga) ||
                !delovi[7].Equals(centar.Cena_jednog_grupni) || !delovi[8].Equals(centar.Cena_jednog_trener) || centar.IsDeleted == true)
            {
                delovi[1] = centar.Adresa;        
                delovi[2] = centar.Godina_otvaranja.ToString();
                delovi[4] = centar.Cena_mesecne_clanarine.ToString();
                delovi[5] = centar.Cena_godisnje_clanarine.ToString();
                delovi[6] = centar.Cena_jednog_treninga.ToString();
                delovi[7] = centar.Cena_jednog_grupni.ToString();
                delovi[8] = centar.Cena_jednog_trener.ToString();
                delovi[9] = centar.IsDeleted.ToString();

            }

            trenutni = String.Join(";", delovi);

            writer.WriteLine(trenutni);

            writer.Close();
            reader.Close();
            stream.Close();

            File.WriteAllText(path, String.Empty);
            string[] content = File.ReadAllLines(tempFile);
            File.WriteAllLines(path, content);

        }
        public static void PutFitnesCentar(Fitnes_Centar centar, Korisnik korisnik, string path)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            string data = "";

            data += centar.Naziv + ";" + centar.Adresa + ";" + centar.Godina_otvaranja + ";" + korisnik.Ime + ","
                + korisnik.Prezime + "," + korisnik.Email + ";" + centar.Cena_mesecne_clanarine + ";" + centar.Cena_godisnje_clanarine + ";"
                + centar.Cena_jednog_treninga + ";" + centar.Cena_jednog_grupni + ";" + centar.Cena_jednog_trener + ";" + centar.IsDeleted;

            writer.WriteLine(data);

            writer.Close();
            stream.Close();
        }
        public static void IzmenaKomentar(Komentar komentar, string path)
        {
            path = HostingEnvironment.MapPath(path);
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            string tempFile = Path.GetTempFileName();
            StreamWriter writer = new StreamWriter(tempFile);
            StreamReader reader = new StreamReader(stream);

            string line = "";
            string trenutni = "";
            string[] delovi;

            while ((line = reader.ReadLine()) != null)
            {
                delovi = line.Split(';');
                if (delovi[0].Equals(komentar.Posetilac.Username) && delovi[1].Equals(komentar.FitnesCentar.Naziv))
                {
                    trenutni = line;

                }
                else writer.WriteLine(line);
            }

            delovi = trenutni.Split(';');

            if(!delovi[4].Equals(komentar.Blokiran.ToString()))
            {
                delovi[4] = komentar.Blokiran.ToString();
            }

            trenutni = String.Join(";", delovi);

            writer.WriteLine(trenutni);

            writer.Close();
            reader.Close();
            stream.Close();

            File.WriteAllText(path, String.Empty);
            string[] content = File.ReadAllLines(tempFile);
            File.WriteAllLines(path, content);
        }


    }
}