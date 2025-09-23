using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;

namespace FitnesCentar.Controllers
{
    public class PredTrenerController : Controller
    {
        // GET: PredTrener
        public ActionResult Index()
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<GrupniTrening> pomocna = new List<GrupniTrening>();
            Korisnik korisnik = (Korisnik)Session["logovani"];

            if (korisnik != null && korisnik.Uloga == Uloga.TRENER)
            {

                foreach (GrupniTrening trening in korisnik.Trener_angazovanje)
                {
                    foreach (GrupniTrening grupni in treninzi)
                    {
                        if (grupni.Naziv.Equals(trening.Naziv))
                        {
                            if (grupni.Date_time > DateTime.Now && grupni.IsDeleted == false) pomocna.Add(grupni);
                        }
                    }
                }

                if (TempData["posetioci"] != null) ViewBag.posetioci = TempData["posetioci"];

                ViewBag.treninzi = pomocna;

                return View();
            }
            else return View();
        }
        [HttpPost]
        public ActionResult Radnja(string radnja)
        {

            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            string[] delovi;
            delovi = radnja.Split('/');


            switch (delovi[0])
            {

                case "posetioci":
              {
                        List<Korisnik> posetioci = new List<Korisnik>();

                        foreach (GrupniTrening trening in treninzi)
                        {
                            if (trening.Naziv.Equals(delovi[1]))
                            {
                                foreach (Korisnik k in trening.Posetioci)
                                {
                                    foreach (Korisnik korisnik in korisnici)
                                    {
                                        if (korisnik.Username.Equals(k.Username)) posetioci.Add(korisnik);
                                    }
                                }
                            }

                        }

                        TempData["posetioci"] = posetioci;
                        return RedirectToAction("Index");
              }
                case "mod":
                    {
                        Session["trening"] = delovi[1];
                        return RedirectToAction("ModifikujView", "PredTrener");
                    }
                    

                case "obrisi":
                    {
                        List<GrupniTrening> pomocna = treninzi;

                        foreach(GrupniTrening trening in pomocna)
                        {
                            if (trening.Naziv.Equals(delovi[1]))
                            {
                                trening.IsDeleted = true;
                                Data.IzmenaGrupniTrening(trening, "~/App_Data/grupni_treninzi.txt");
                                break;
                            }
                        }

                        treninzi = pomocna;
                        return RedirectToAction("Index");
                    }
                    
            }
            return null;
        }
        public ActionResult ModifikujView()
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            GrupniTrening trening = new GrupniTrening();

            foreach(GrupniTrening grupni in treninzi)
            {
                if (grupni.Naziv.Equals(Session["trening"])) trening = grupni;
            }

            ViewBag.trening = trening;
            ViewBag.error = TempData["error"];
            return View();
        }
        [HttpPost]
        public ActionResult Modifikacija(GrupniTrening trening, string naziv)
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            string[] errors;
            string whole = "";
      

            if (trening.Trajanje == 0) whole = whole + "Trajanje treninga ne moze biti nula;";

            if (trening.Max_posetilaca == 0) whole = whole + "Max posetilaca ne moze biti nula;";

            if (trening.Date_time.Day == 1 && trening.Date_time.Month == 1 && trening.Date_time.Year == 0001)
                whole = whole + "Morate izabrati datum i vreme odrzavanja;";

            if (trening.Date_time < DateTime.Now) whole = whole + "Datum i vreme treninga moraju biti datum i vreme u buducnosti;";

            if (whole != String.Empty)
            {
                errors = whole.Split(';');
                errors = errors.Reverse().Skip(1).Reverse().ToArray();
                TempData["error"] = errors;
                return RedirectToAction("ModifikujView", "PredTrener");
            }

            foreach(GrupniTrening grupni in treninzi)
            {
                if (grupni.Naziv.Equals(naziv))
                {
                    grupni.Naziv = naziv;
                    grupni.TipTreninga = trening.TipTreninga;
                    grupni.Trajanje = trening.Trajanje;
                    grupni.Max_posetilaca = trening.Max_posetilaca;
                    grupni.Date_time = trening.Date_time;
                    break;
                }
            }
            trening.Naziv = naziv;
            Data.IzmenaGrupniTrening(trening, "~/App_Data/grupni_treninzi.txt");
            return RedirectToAction("Index");
        }
        public ActionResult KreirajView()
        {
            ViewBag.error = TempData["errors"];
            return View();
        }
        [HttpPost]
        public ActionResult Kreiraj(GrupniTrening trening)
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            Korisnik korisnik = (Korisnik)Session["logovani"];
            string[] errors;
            string whole = "";

            foreach(GrupniTrening grupniTrening in treninzi)
            {
                if(grupniTrening.Naziv.Equals(trening.Naziv)) whole = whole + "Ne mozete kreirati trening sa istim nazivom;";
            }

            if (trening.Naziv == null || trening.Naziv == "") whole = whole + "Niste uneli naziv;";

            if (trening.Trajanje == 0) whole = whole + "Trajanje treninga ne moze biti nula;";

            if (trening.Max_posetilaca == 0) whole = whole + "Max posetilaca ne moze biti nula;";

            if (trening.Date_time.Day == 1 && trening.Date_time.Month == 1 && trening.Date_time.Year == 0001)
                whole = whole + "Morate izabrati datum i vreme odrzavanja;";

            if ((trening.Date_time - DateTime.Now).TotalDays < 3) whole = whole + "Ne mozete rezervisati taj datum i vreme;";

            if (whole != String.Empty)
            {
                errors = whole.Split(';');
                errors = errors.Reverse().Skip(1).Reverse().ToArray();
                TempData["errors"] = errors;
                return RedirectToAction("KreirajView", "PredTrener");
            }

            GrupniTrening grupni = new GrupniTrening(trening.Naziv, trening.TipTreninga, korisnik.FitnesCentar, trening.Trajanje,
                trening.Date_time,trening.Max_posetilaca, new List<Korisnik>(), false);

            treninzi.Add(grupni);
            korisnik.Trener_angazovanje.Add(grupni);
            Data.IzmenaKorisnik(korisnik, "~/App_Data/korisnici.txt", false);
            Data.PutGrupniTrening(grupni, "~/App_Data/grupni_treninzi.txt");
            return RedirectToAction("Index");

        }

    }
             
    }
