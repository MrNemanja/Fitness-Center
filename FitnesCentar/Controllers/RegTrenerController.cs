using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;

namespace FitnesCentar.Controllers
{
    public class RegTrenerController : Controller
    {
        // GET: RegTrener
        public ActionResult Index()
        {
            Korisnik logovani = (Korisnik)Session["logovani"];
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)HttpContext.Application["centri"];
            List<Fitnes_Centar> pomocna = new List<Fitnes_Centar>();

            if (logovani != null && logovani.Uloga == Uloga.VLASNIK)
            {

                foreach (Fitnes_Centar centar in centri)
                {
                    foreach (Fitnes_Centar fitnes_Centar in logovani.Fitnescentri)
                    {
                        if (fitnes_Centar.Naziv.Equals(centar.Naziv) && centar.IsDeleted == false) pomocna.Add(centar);
                    }
                }

                ViewBag.Centri = pomocna;
                ViewBag.error = TempData["error"];

                return View();
            }

            else return View();
        }
        [HttpPost]
        public ActionResult Register(Korisnik k, string[] pol, string fitnes)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            string[] errors;
            string whole = "";
            EmailAddressAttribute emailAddress = new EmailAddressAttribute();
            bool valid;

            if (k.Username == null || k.Username == "") whole = whole + "Niste uneli username;";

            foreach (Korisnik korisnik in korisnici)
            {
                if (k.Username == korisnik.Username) whole = whole + "Ovaj username vec postoji;";
            }

            if (k.Password == null || k.Password == "") whole = whole + "Niste uneli password;";
            else if (k.Password.Length < 6) whole = whole + "password mora imati vise od 6 karaktera;";

            if (k.Ime == null || k.Ime == "") whole = whole + "Niste uneli ime;";

            if (k.Prezime == null || k.Prezime == "") whole = whole + "Niste uneli prezime;";

            if (k.Email == null || k.Email == "") whole = whole + "Niste uneli email;";
            valid = emailAddress.IsValid(k.Email);
            if (valid == false) whole = whole + "Email nije u pravilnom formatu;";

            foreach (Korisnik korisnik in korisnici)
            {
                if (k.Email == korisnik.Email) whole = whole + "Ovaj email vec postoji;";
            }

            if (pol == null) whole = whole + "Morate izabrati pol;";

            else if (pol.Contains("PolM") && pol.Contains("PolZ")) whole = whole + "Ne mozete izabrati oba pola;";

            if (k.Datum_rodjenja.Day == 1 && k.Datum_rodjenja.Month == 1 && k.Datum_rodjenja.Year == 0001)
                whole = whole + "Morate izabrati datum rodjenja;";

            if (whole != String.Empty)
            {
                errors = whole.Split(';');
                errors = errors.Reverse().Skip(1).Reverse().ToArray();
                TempData["error"] = errors;
                return RedirectToAction("Index");
            }

            Korisnik novi = new Korisnik();
            novi.Username = k.Username;
            novi.Password = k.Password;
            novi.Ime = k.Ime;
            novi.Prezime = k.Prezime;
            if (pol[0].Equals("PolM")) novi.Pol = Pol.MUSKI;
            else novi.Pol = Pol.ZENSKI;
            novi.Email = k.Email;
            novi.Datum_rodjenja = k.Datum_rodjenja;
            novi.Uloga = Uloga.TRENER;
            novi.Trener_angazovanje = new List<GrupniTrening>();
            Fitnes_Centar centar = new Fitnes_Centar();
            centar.Naziv = fitnes;
            novi.FitnesCentar = centar;
            novi.Blokiran = false;

            korisnici.Add(novi);
            Data.PutKorisnik(novi, "~/App_Data/korisnici.txt");

            return View();

        }
    }
}