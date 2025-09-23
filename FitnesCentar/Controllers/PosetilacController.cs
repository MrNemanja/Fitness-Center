using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;

namespace FitnesCentar.Controllers
{
    public class PosetilacController : Controller
    {
        // GET: Posetilac
        public ActionResult Index()
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik logovani = (Korisnik)Session["logovani"];

            if (logovani != null && logovani.Uloga == Uloga.POSETILAC)
            {

                if (TempData["prosledjena"] == null)
                {

                    List<GrupniTrening> grupni = new List<GrupniTrening>();
                    Korisnik korisnik = new Korisnik();
                    string username = logovani.Username;

                    foreach (Korisnik k in korisnici)
                    {
                        if (username.Equals(k.Username)) korisnik = k;
                    }

                    foreach (GrupniTrening trening in treninzi)
                    {
                        if (trening.Date_time < DateTime.Now)
                        {
                            foreach (Korisnik k in trening.Posetioci)
                            {
                                if (k.Username.Equals(korisnik.Username)) grupni.Add(trening);
                            }
                        }
                    }

                    ViewBag.treninzi = grupni;
                    ViewBag.error = TempData["komentar"];
                    TempData["old"] = grupni;
                }

                else
                {
                    ViewBag.treninzi = TempData["prosledjena"];
                    ViewBag.error = TempData["komentar"];
                }

                return View();
            }
            else return View();
        }
        [HttpPost]
        public ActionResult Search(string izbor)
        {
            List<GrupniTrening> pomocna = new List<GrupniTrening>();
            List<GrupniTrening> grupni = (List<GrupniTrening>)TempData["old"];

            if (izbor.Equals("pretrazi"))
            {

                var nazivt = Request["nazivt"];
                var nazivc = Request["nazivc"];
                var tip = Request["tip"];

                foreach (GrupniTrening trening in grupni)
                {
                    if (trening.Naziv.Equals(nazivt)) pomocna.Add(trening);
                    if (trening.FitnesCentar.Naziv.Equals(nazivc) && !(pomocna.Contains(trening))) pomocna.Add(trening);
                    if (trening.TipTreninga.ToString() == tip && !(pomocna.Contains(trening))) pomocna.Add(trening);
                }

                TempData["prosledjena"] = pomocna;

                return RedirectToAction("Index", "Posetilac");
            }
            else
            {

                TempData["prosledjena"] = grupni;
                return RedirectToAction("Index", "Posetilac");
            }
        }

        [HttpPost]
        public ActionResult Sort(string submit)
        {
            List<GrupniTrening> grupni = (List<GrupniTrening>)TempData["old"];
            List<GrupniTrening> pomocna = grupni;
            GrupniTrening temp = null;

            switch (submit)
            {
                case "NazivA":
                    {
                        for (int i = 0; i < pomocna.Count - 1; i++)
                        {
                            for (int j = i + 1; j < pomocna.Count; j++)
                            {
                                if (String.Compare(pomocna[i].Naziv, pomocna[j].Naziv) > 0)
                                {
                                    temp = pomocna[i];
                                    pomocna[i] = pomocna[j];
                                    pomocna[j] = temp;
                                }
                            }

                        }
                    }
                    break;

                case "TipA":
                    {
                        for (int i = 0; i < pomocna.Count - 1; i++)
                        {
                            for (int j = i + 1; j < pomocna.Count; j++)
                            {
                                if (pomocna[i].TipTreninga > pomocna[j].TipTreninga)
                                {
                                    temp = pomocna[i];
                                    pomocna[i] = pomocna[j];
                                    pomocna[j] = temp;
                                }
                            }

                        }
                    }
                    break;

                case "GodinaA":
                    {
                        for (int i = 0; i < pomocna.Count - 1; i++)
                        {
                            for (int j = i + 1; j < pomocna.Count; j++)
                            {
                                if (pomocna[i].Date_time > pomocna[j].Date_time)
                                {
                                    temp = pomocna[i];
                                    pomocna[i] = pomocna[j];
                                    pomocna[j] = temp;
                                }
                            }

                        }
                    }
                    break;
            }

            TempData["prosledjena"] = pomocna;

            return RedirectToAction("Index", "Posetilac");

        }

        [HttpPost]
        public ActionResult SortDESC(string submit)
        {
            List<GrupniTrening> grupni = (List<GrupniTrening>)TempData["old"];
            List<GrupniTrening> pomocna = grupni;
            GrupniTrening temp = null;

            switch (submit)
            {
                case "NazivD":
                    {
                        for (int i = 0; i < pomocna.Count - 1; i++)
                        {
                            for (int j = i + 1; j < pomocna.Count; j++)
                            {
                                if (String.Compare(pomocna[i].Naziv, pomocna[j].Naziv) < 0)
                                {
                                    temp = pomocna[i];
                                    pomocna[i] = pomocna[j];
                                    pomocna[j] = temp;
                                }
                            }

                        }
                    }
                    break;

                case "TipD":
                    {
                        for (int i = 0; i < pomocna.Count - 1; i++)
                        {
                            for (int j = i + 1; j < pomocna.Count; j++)
                            {
                                if (pomocna[i].TipTreninga < pomocna[j].TipTreninga)
                                {
                                    temp = pomocna[i];
                                    pomocna[i] = pomocna[j];
                                    pomocna[j] = temp;
                                }
                            }

                        }
                    }
                    break;

                case "GodinaD":
                    {
                        for (int i = 0; i < pomocna.Count - 1; i++)
                        {
                            for (int j = i + 1; j < pomocna.Count; j++)
                            {
                                if (pomocna[i].Date_time < pomocna[j].Date_time)
                                {
                                    temp = pomocna[i];
                                    pomocna[i] = pomocna[j];
                                    pomocna[j] = temp;
                                }
                            }

                        }
                    }
                    break;
            }


            TempData["prosledjena"] = pomocna;

            return RedirectToAction("Index", "Posetilac");

        }
        [HttpPost]
        public ActionResult Komentar(string komentar)
        {
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            Korisnik korisnik = (Korisnik)Session["logovani"];
            string error = "";

            foreach (Komentar kom in komentari)
            {
                if (kom.FitnesCentar.Naziv.Equals(komentar) && kom.Posetilac.Username.Equals(korisnik.Username))
                {
                    error = "Vec ste ostavili komentar za ovaj fitnes centar";
                }
            }

            if(error != String.Empty)
            {
                TempData["komentar"] = error;
                return RedirectToAction("Index");
            }

            TempData["naziv_centra"] = komentar;
            return RedirectToAction("Index", "Komentar");
        }
    }
}