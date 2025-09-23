using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.UI;

namespace FitnesCentar.Controllers
{
    public class RegisterController : Controller
    {
        public ActionResult RegisterView()
        {
            ViewBag.error = TempData["error"];
            
            return View();
        }

        [HttpPost]
        public ActionResult Register(Korisnik k, string[] pol)
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

            if (k.Datum_rodjenja >= DateTime.Now)
                whole = whole + "Ne mozete izabrati datum u buducnosti;";

            if (whole != String.Empty)
            {
                errors = whole.Split(';');
                errors = errors.Reverse().Skip(1).Reverse().ToArray();
                TempData["error"] = errors;
                return RedirectToAction("RegisterView");
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
            novi.Uloga = Uloga.POSETILAC;
            novi.Treninzi_prijavljen = new List<GrupniTrening>();

            korisnici.Add(novi);
            Data.PutKorisnik(novi, "~/App_Data/korisnici.txt");

            return View();
            
        }

        public ActionResult LogInView()
        {
            ViewBag.error = TempData["error"];

            return View();
        }

        public ActionResult LogIn(string username, string password)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)HttpContext.Application["centri"];
            Korisnik logovani = (Korisnik)Session["logovani"];

            string[] errors;
            string whole = "";

            if (username == null || username == "") whole = whole + "Niste uneli username;";
            if (password == null || password == "") whole = whole + "Niste uneli password;";

            int i = 0;
            foreach (Korisnik korisnik in korisnici)
            {
                if (username.Equals(korisnik.Username) && password.Equals(korisnik.Password))
                {
                    if (korisnik.Uloga == Uloga.POSETILAC)
                    {
                        if (logovani == null)
                        {

                            logovani = new Korisnik();
                            logovani = korisnik;
                            Session["logovani"] = logovani;
                            return RedirectToAction("Index2", "Home2");

                        }
                        else
                        {
                            whole = whole + "Neko je vec ulogovan;";
                            break;
                        }
                    }
                    else if (korisnik.Uloga == Uloga.TRENER)
                    {
                        if (logovani == null)
                        {
                            foreach(Fitnes_Centar centar in centri)
                            {
                                if (centar.Naziv.Equals(korisnik.FitnesCentar.Naziv) && centar.IsDeleted == false)
                                {
                                    if (korisnik.Blokiran == false)
                                    {
                                        logovani = new Korisnik();
                                        logovani = korisnik;
                                        Session["logovani"] = logovani;
                                        return RedirectToAction("Index3", "Home3");
                                    }
                                }

                            }
                            
                        }
                        else
                        {
                            whole = whole + "Neko je vec ulogovan;";
                            break;
                        }
                    }
                    else if (korisnik.Uloga == Uloga.VLASNIK)
                    {
                        if (logovani == null)
                        {
                            logovani = new Korisnik();
                            logovani = korisnik;
                            Session["logovani"] = logovani;
                            return RedirectToAction("Index4", "Home4");

                        }
                        else
                        {
                            whole = whole + "Neko je vec ulogovan;";
                            break;
                        }
                    }
                    
                }
                i++;
            }
            if(i == korisnici.Count) whole = whole + "Los username ili password, probajte ponovo;";

            errors = whole.Split(';');
            if(errors.Length > 1) errors = errors.Reverse().Skip(1).Reverse().ToArray();
            TempData["error"] = errors;
            return RedirectToAction("LogInView");
           

        }

        // GET: Register

    }
}