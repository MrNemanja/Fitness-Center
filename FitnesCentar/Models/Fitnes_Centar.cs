using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnesCentar.Models
{
    public class Fitnes_Centar
    {
        private string naziv;
        private string adresa;
        private int godina_otvaranja;
        private Korisnik vlasnik;
        private int cena_mesecne_clanarine;
        private int cena_godisnje_clanarine;
        private int cena_jednog_treninga;
        private int cena_jednog_grupni;
        private int cena_jednog_trener;
        private bool isDeleted;

        public Fitnes_Centar() {}

        public Fitnes_Centar(string naziv, string adresa, int godina_otvaranja, Korisnik vlasnik, int cena_mesecne_clanarine, int cena_godisnje_clanarine, int cena_jednog_treninga, int cena_jednog_grupni, int cena_jednog_trener, bool deleted)
        {
            this.Naziv = naziv;
            this.Adresa = adresa;
            this.Godina_otvaranja = godina_otvaranja;
            this.Vlasnik = vlasnik;
            this.Cena_mesecne_clanarine = cena_mesecne_clanarine;
            this.Cena_godisnje_clanarine = cena_godisnje_clanarine;
            this.Cena_jednog_treninga = cena_jednog_treninga;
            this.Cena_jednog_grupni = cena_jednog_grupni;
            this.Cena_jednog_trener = cena_jednog_trener;
            this.IsDeleted = deleted;
        }

        public string Naziv { get => naziv; set => naziv = value; }
        public string Adresa { get => adresa; set => adresa = value; }
        public int Godina_otvaranja { get => godina_otvaranja; set => godina_otvaranja = value; }
        public Korisnik Vlasnik { get => vlasnik; set => vlasnik = value; }
        public int Cena_mesecne_clanarine { get => cena_mesecne_clanarine; set => cena_mesecne_clanarine = value; }
        public int Cena_godisnje_clanarine { get => cena_godisnje_clanarine; set => cena_godisnje_clanarine = value; }
        public int Cena_jednog_treninga { get => cena_jednog_treninga; set => cena_jednog_treninga = value; }
        public int Cena_jednog_grupni { get => cena_jednog_grupni; set => cena_jednog_grupni = value; }
        public int Cena_jednog_trener { get => cena_jednog_trener; set => cena_jednog_trener = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}