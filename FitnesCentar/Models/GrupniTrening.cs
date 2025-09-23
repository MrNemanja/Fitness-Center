using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnesCentar.Models
{
    public class GrupniTrening
    {
        private string naziv;
        private TipTreninga tipTreninga;
        private Fitnes_Centar fitnesCentar;
        private int trajanje;
        private DateTime date_time;
        private int max_posetilaca;
        private List<Korisnik> posetioci;
        private bool isDeleted;

        public GrupniTrening() {}

        public GrupniTrening(string naziv, TipTreninga tipTreninga, Fitnes_Centar fitnesCentar, int trajanje, DateTime date_time, int max_posetilaca, List<Korisnik> posetioci, bool isDeleted)
        {
            this.Naziv = naziv;
            this.TipTreninga = tipTreninga;
            this.FitnesCentar = fitnesCentar;
            this.Trajanje = trajanje;
            this.Date_time = date_time;
            this.Max_posetilaca = max_posetilaca;
            this.Posetioci = posetioci;
            this.IsDeleted = isDeleted;
        }

        public string Naziv { get => naziv; set => naziv = value; }
        public TipTreninga TipTreninga { get => tipTreninga; set => tipTreninga = value; }
        public Fitnes_Centar FitnesCentar { get => fitnesCentar; set => fitnesCentar = value; }
        public int Trajanje { get => trajanje; set => trajanje = value; }
        public DateTime Date_time { get => date_time; set => date_time = value; }
        public int Max_posetilaca { get => max_posetilaca; set => max_posetilaca = value; }
        public List<Korisnik> Posetioci { get => posetioci; set => posetioci = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
    }
}