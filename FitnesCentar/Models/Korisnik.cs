using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnesCentar.Models
{
    public class Korisnik
    {
        private string username;
        private string password;
        private string ime;
        private string prezime;
        private Pol pol;
        private string email;
        private DateTime datum_rodjenja;
        private Uloga uloga;
        private List<GrupniTrening> treninzi_prijavljen;
        private List<GrupniTrening> trener_angazovanje;
        private Fitnes_Centar fitnesCentar;
        private List<Fitnes_Centar> fitnescentri;
        private bool blokiran;

        public Korisnik() {}

        public Korisnik(string username, string password, string ime, string prezime, Pol pol, string email, DateTime datum_rodjenja, Uloga uloga, List<GrupniTrening> treninzi_prijavljen, List<GrupniTrening> trener_angazovanje, Fitnes_Centar fitnesCentar, List<Fitnes_Centar> fitnescentri,
            bool blokiran)
        {
            this.Username = username;
            this.Password = password;
            this.Ime = ime;
            this.Prezime = prezime;
            this.Pol = pol;
            this.Email = email;
            this.Datum_rodjenja = datum_rodjenja;
            this.Uloga = uloga;
            this.Treninzi_prijavljen = treninzi_prijavljen;
            this.Trener_angazovanje = trener_angazovanje;
            this.FitnesCentar = fitnesCentar;
            this.Fitnescentri = fitnescentri;
            this.Blokiran = blokiran;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public Pol Pol { get => pol; set => pol = value; }
        public string Email { get => email; set => email = value; }
        public DateTime Datum_rodjenja { get => datum_rodjenja; set => datum_rodjenja = value; }
        public Uloga Uloga { get => uloga; set => uloga = value; }
        public List<GrupniTrening> Treninzi_prijavljen { get => treninzi_prijavljen; set => treninzi_prijavljen = value; }
        public List<GrupniTrening> Trener_angazovanje { get => trener_angazovanje; set => trener_angazovanje = value; }
        public Fitnes_Centar FitnesCentar { get => fitnesCentar; set => fitnesCentar = value; }
        public List<Fitnes_Centar> Fitnescentri { get => fitnescentri; set => fitnescentri = value; }
        public bool Blokiran { get => blokiran; set => blokiran = value; }
    }
}