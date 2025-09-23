using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;

namespace FitnesCentar.Controllers
{
    public class DetailsController : Controller
    {
        // GET: Details
        public ActionResult Index()
        {
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)HttpContext.Application["centri"];
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];

            ViewBag.komentar = TempData["kliknuo"];
            ViewBag.prosledjen = TempData["prosledjen"];
            ViewBag.grupni = TempData["grupni"];
            ViewBag.prosledjena2 = TempData["prosledjena2"];


            return View();
        }
        [HttpPost]
        public ActionResult Choose(string detalji)
        {
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)HttpContext.Application["centri"];
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik logovani = (Korisnik)Session["logovani"];
            List<GrupniTrening> prosledjena = new List<GrupniTrening>();
            List<Komentar> prosledjena2 = new List<Komentar>();

                        foreach(Fitnes_Centar centar in centri)
                        {
                            if(centar.Naziv.Equals(detalji))
                            {
                                foreach(GrupniTrening trening in treninzi)
                                {
                                    if (trening.FitnesCentar.Naziv.Equals(centar.Naziv))
                                    {

                                        if(trening.Date_time >= DateTime.Now && trening.IsDeleted == false)
                                        prosledjena.Add(trening);
                                    }
                                }

                                foreach(Komentar komentar in komentari)
                                {
                                    if (komentar.FitnesCentar.Naziv.Equals(centar.Naziv))
                                    {
                                        foreach(Korisnik korisnik in korisnici)
                                        {
                                            if(korisnik.Username.Equals(komentar.Posetilac.Username))
                                            {
                                                if (komentar.Blokiran == false)
                                                {
                                                    komentar.Posetilac.Ime = korisnik.Ime;
                                                    komentar.Posetilac.Prezime = korisnik.Prezime;
                                                    prosledjena2.Add(komentar);
                                                    break;
                                                }
                                                else
                                                {
                                        if (logovani != null)
                                        {
                                            if (logovani.Uloga == Uloga.VLASNIK)
                                            {
                                                foreach (Fitnes_Centar fitnes in logovani.Fitnescentri)
                                                {
                                                    if(fitnes.Naziv.Equals(centar.Naziv))
                                                    {
                                                        komentar.Posetilac.Ime = korisnik.Ime;
                                                        komentar.Posetilac.Prezime = korisnik.Prezime;
                                                        prosledjena2.Add(komentar);
                                                        break;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                                }
                                            }
                                            
                                        }
                                        
                                    }
                                }

                                TempData["prosledjen"] = centar;
                                TempData["prosledjena2"] = prosledjena2;
                                TempData["grupni"] = prosledjena;
                            }
                        }
            return RedirectToAction("Index", "Details");
        }
    }
            
}
