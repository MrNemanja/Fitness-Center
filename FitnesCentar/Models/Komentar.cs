using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnesCentar.Models
{
    public class Komentar
    {
        private Korisnik posetilac;
        private Fitnes_Centar fitnesCentar;
        private string opis;
        private int ocena;
        private bool blokiran;

        public Komentar() {}

        public Komentar(Korisnik posetilac, Fitnes_Centar fitnesCentar, string opis, int ocena, bool blokiran)
        {
            this.Posetilac = posetilac;
            this.FitnesCentar = fitnesCentar;
            this.Opis = opis;
            this.Ocena = ocena;
            this.Blokiran = blokiran;
        }

        public Korisnik Posetilac { get => posetilac; set => posetilac = value; }
        public Fitnes_Centar FitnesCentar { get => fitnesCentar; set => fitnesCentar = value; }
        public string Opis { get => opis; set => opis = value; }
        public int Ocena { get => ocena; set => ocena = value; }
        public bool Blokiran { get => blokiran; set => blokiran = value; }
    }
}