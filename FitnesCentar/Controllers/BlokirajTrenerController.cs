using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;

namespace FitnesCentar.Controllers
{
    public class BlokirajTrenerController : Controller
    {
        // GET: SpisakTrener
        public ActionResult Index()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            List<Korisnik> pomocna = new List<Korisnik>();
            Korisnik korisnik = (Korisnik)Session["logovani"];

            if (korisnik != null && korisnik.Uloga == Uloga.VLASNIK)
            {

                foreach (Korisnik k in korisnici)
                {
                    if (k.Uloga == Uloga.TRENER)
                    {
                        foreach (Fitnes_Centar centar in korisnik.Fitnescentri)
                        {
                            if (centar.Naziv.Equals(k.FitnesCentar.Naziv))
                            {
                                pomocna.Add(k);
                            }
                        }
                    }
                }

                ViewBag.treneri = pomocna;
                ViewBag.error = TempData["error"];

                return View();
            }
            else return View();
        }
        [HttpPost]
        public ActionResult Blokiraj(string izbor)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik korisnik = (Korisnik)Session["logovani"];
            string whole = "";
            string[] errors;
            var username = Request["username"];
            int i = 0;

            if (izbor.Equals("Blokiraj"))
            {

                if (username == null || username == "") whole += "Niste uneli username;";

                foreach (Korisnik k in korisnici)
                {
                    if (k.Username.Equals(username) && k.Uloga == Uloga.TRENER)
                    {
                        foreach(Fitnes_Centar centar in korisnik.Fitnescentri)
                        {
                            if(centar.Naziv.Equals(k.FitnesCentar.Naziv))
                            {
                                k.Blokiran = true;
                                Data.IzmenaKorisnik(k, "~/App_Data/korisnici.txt", true);
                                i++;
                                break;
                            }
                            
                        }
                        break;
                    }
                }

                if (i <= 0)
                {
                    whole += "Ne mozete blokirati onog koji nije u vasem fitnes centru;";
                }

                if (whole != String.Empty)
                {
                    errors = whole.Split(';');
                    errors = errors.Reverse().Skip(1).Reverse().ToArray();
                    TempData["error"] = errors;
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            else
            {
                if (username == null || username == "") whole += "Niste uneli username;";

                foreach (Korisnik k in korisnici)
                {
                    if (k.Username.Equals(username) && k.Uloga == Uloga.TRENER)
                    {
                        foreach (Fitnes_Centar centar in korisnik.Fitnescentri)
                        {
                            if (centar.Naziv.Equals(k.FitnesCentar.Naziv))
                            {
                                k.Blokiran = false;
                                Data.IzmenaKorisnik(k, "~/App_Data/korisnici.txt", true);
                                i++;
                                break;
                            }
                            else whole += "Ne mozete odblokirati trenera koji nije u vasem fitnes centru;";
                        }
                        break;
                    }
                }

                if (i <= 0)
                {
                    whole += "Korisnik sa ovim username ne postoji;";
                }

                if (whole != String.Empty)
                {
                    errors = whole.Split(';');
                    errors = errors.Reverse().Skip(1).Reverse().ToArray();
                    TempData["error"] = errors;
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
        }
    }
    
    
}