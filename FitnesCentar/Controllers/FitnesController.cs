using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;

namespace FitnesCentar.Controllers
{
    public class FitnesController : Controller
    {
        // GET: Fitnes
        public ActionResult Index()
        {
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)HttpContext.Application["centri"];
            List<Fitnes_Centar> pomocna = new List<Fitnes_Centar>();
            Korisnik korisnik = (Korisnik)Session["logovani"];

            if (korisnik != null && korisnik.Uloga == Uloga.VLASNIK)
            {

                foreach (Fitnes_Centar centar in korisnik.Fitnescentri)
                {
                    foreach (Fitnes_Centar fitnes in centri)
                    {
                        if (fitnes.Naziv.Equals(centar.Naziv) && fitnes.IsDeleted == false)
                        {
                            pomocna.Add(fitnes);
                        }
                    }
                }

                ViewBag.error = TempData["error"];
                ViewBag.centri = pomocna;

                return View();
            }
            else return View();

        }
        [HttpPost]
        public ActionResult Radnja(string radnja)
        {

            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)HttpContext.Application["centri"];
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            string[] delovi;
            int i = 0;
            delovi = radnja.Split('/');


            switch (delovi[0])
            {
                case "mod":
                    {
                        Session["centar"] = delovi[1];
                        return RedirectToAction("ModifikujView", "Fitnes");
                    }


                case "obrisi":
                    {
                        List<Fitnes_Centar> pomocna = centri;
                        List<GrupniTrening> pomocna2 = treninzi;

                        foreach (Fitnes_Centar centar in pomocna)
                        {
                            if (centar.Naziv.Equals(delovi[1]))
                            {
                                foreach (GrupniTrening trening in pomocna2)
                                {
                                    if (trening.FitnesCentar.Naziv.Equals(centar.Naziv))
                                    {
                                        if (trening.Date_time > DateTime.Now)
                                        {
                                            i++;
                                        }
                                    }

                                }
                                if (i >= 1)
                                {
                                    TempData["error"] = "Nije moguce obrisati fitnes centar, postoje neki treninzi koji" +
                                                " treba da se odrze.";
                                }
                                else
                                {
                                    centar.IsDeleted = true;
                                    Data.ObrisiFitnesCentar(centar, "~/App_Data/fitnes_centri.txt");
                                    break;
                                }


                            }
                        }

                        centri = pomocna;
                        return RedirectToAction("Index");
                    }

            }
            return null;
        }
        public ActionResult ModifikujView()
        {
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)HttpContext.Application["centri"];
            Fitnes_Centar centar = new Fitnes_Centar();

            foreach (Fitnes_Centar fitnes_Centar in centri)
            {
                if (fitnes_Centar.Naziv.Equals(Session["centar"])) centar = fitnes_Centar;
            }

            ViewBag.centar = centar;
            ViewBag.error = TempData["error"];

            return View();
        }
        [HttpPost]
        public ActionResult Modifikacija(Fitnes_Centar centar, string naziv, string ulica, string broj, string grad, string postanski)
        {
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)HttpContext.Application["centri"];
            string[] errors;
            string whole = "";

            if (ulica == null || ulica == "") whole = whole + "Niste uneli ulicu;";
            if (broj == null || broj == "") whole = whole + "Niste uneli broj;";
            if (grad == null || grad == "") whole = whole + "Niste uneli grad;";
            if (postanski == null || postanski == "") whole = whole + "Niste uneli postanski broj;";

            if (centar.Cena_mesecne_clanarine == 0) whole = whole + "Niste uneli cenu mesecne clanarine;";

            if (centar.Cena_godisnje_clanarine == 0) whole = whole + "Niste uneli cenu godisnje clanarine;";

            if (centar.Cena_jednog_treninga == 0) whole = whole + "Niste uneli cenu dnevnog treninga;";

            if (centar.Cena_jednog_grupni == 0) whole = whole + "Niste uneli cenu jednog grupnog treninga;";

            if (centar.Cena_jednog_trener == 0) whole = whole + "Niste uneli cenu treninga sa trenerom;";

            if (whole != String.Empty)
            {
                errors = whole.Split(';');
                errors = errors.Reverse().Skip(1).Reverse().ToArray();
                TempData["error"] = errors;
                return RedirectToAction("ModifikujView", "Fitnes");
            }

            foreach (Fitnes_Centar fitnes in centri)
            {
                if (fitnes.Naziv.Equals(naziv))
                {
                    fitnes.Naziv = naziv;
                    string[] delovi_copy = new string[3];
                    delovi_copy[0] = ulica + broj;
                    delovi_copy[1] = grad;
                    delovi_copy[2] = postanski.ToString();
                    fitnes.Adresa = String.Join(",", delovi_copy);
                    fitnes.Godina_otvaranja = centar.Godina_otvaranja;
                    fitnes.Cena_mesecne_clanarine = centar.Cena_mesecne_clanarine;
                    fitnes.Cena_godisnje_clanarine = centar.Cena_godisnje_clanarine;
                    fitnes.Cena_jednog_treninga = centar.Cena_jednog_treninga;
                    fitnes.Cena_jednog_grupni = centar.Cena_jednog_grupni;
                    fitnes.Cena_jednog_trener = centar.Cena_jednog_trener;
                    break;
                }
            }
            centar.Naziv = naziv;
            string[] delovi = new string[3];
            delovi[0] = ulica + broj;
            delovi[1] = grad;
            delovi[2] = postanski.ToString();
            centar.Adresa = String.Join(",", delovi);
            
            Data.IzmenaFItnesCentar(centar, "~/App_Data/fitnes_centri.txt");
            return RedirectToAction("Index");
        }
        public ActionResult KreirajView()
        {
            ViewBag.error = TempData["error"];
            return View();
        }
        [HttpPost]
        public ActionResult Kreiraj(Fitnes_Centar centar, string ulica, string broj, string grad, string postanski)
        {
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)HttpContext.Application["centri"];
            Korisnik korisnik = (Korisnik)Session["logovani"];
            string[] errors;
            string whole = "";

            foreach(Fitnes_Centar _Centar in centri)
            {
                if(_Centar.Naziv.Equals(centar.Naziv)) whole = whole + "Ne mozete kreirati fitnes centar sa istim nazivom;";
            }

            if (centar.Naziv == null || centar.Naziv == "") whole = whole + "Niste uneli naziv;";

            if (ulica == null || ulica == "") whole = whole + "Niste uneli ulicu;";
            if (broj == null || broj == "") whole = whole + "Niste uneli broj;";
            if (grad == null || grad == "") whole = whole + "Niste uneli grad;";
           if (postanski == null || postanski == "") whole = whole + "Niste uneli postanski broj;";

            if (centar.Godina_otvaranja == 0) whole = whole + "Niste uneli godinu otvaranja;";

            if (centar.Cena_mesecne_clanarine == 0) whole = whole + "Niste uneli cenu mesecne clanarine;";

            if (centar.Cena_godisnje_clanarine == 0) whole = whole + "Niste uneli cenu godisnje clanarine;";

            if (centar.Cena_jednog_treninga == 0) whole = whole + "Niste uneli cenu dnevnog treninga;";

            if (centar.Cena_jednog_grupni == 0) whole = whole + "Niste uneli cenu jednog grupnog treninga;";

            if (centar.Cena_jednog_trener == 0) whole = whole + "Niste uneli cenu treninga sa trenerom;";

            if (whole != String.Empty)
            {
                errors = whole.Split(';');
                errors = errors.Reverse().Skip(1).Reverse().ToArray();
                TempData["error"] = errors;
                return RedirectToAction("KreirajView", "Fitnes");
            }

            string[] delovi = new string[3];
            delovi[0] = ulica + " " + broj;
            delovi[1] = grad;
            delovi[2] = postanski.ToString();

            Fitnes_Centar fitnes = new Fitnes_Centar(centar.Naziv, String.Join(",", delovi), centar.Godina_otvaranja, korisnik, centar.Cena_mesecne_clanarine,
                centar.Cena_godisnje_clanarine, centar.Cena_jednog_treninga, centar.Cena_jednog_grupni, centar.Cena_jednog_trener, false);

            centri.Add(fitnes);
            korisnik.Fitnescentri.Add(fitnes);
            Data.IzmenaKorisnik(korisnik, "~/App_Data/korisnici.txt", false);
            Data.PutFitnesCentar(fitnes,korisnik, "~/App_Data/fitnes_centri.txt");
            return RedirectToAction("Index");

        }
    }
}