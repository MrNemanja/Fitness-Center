using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;

namespace FitnesCentar.Controllers
{
    public class ZavTrenerController : Controller
    {
        // GET: ZavTrener
        public ActionResult Index()
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<GrupniTrening> pomocna = new List<GrupniTrening>();
            Korisnik korisnik = (Korisnik)Session["logovani"];
            List<GrupniTrening> sesija = (List<GrupniTrening>)Session["treninzi"];

            if (korisnik != null && korisnik.Uloga == Uloga.TRENER)
            {

                if (TempData["prosledjena"] == null && sesija == null)
                {

                    foreach (GrupniTrening trening in korisnik.Trener_angazovanje)
                    {
                        foreach (GrupniTrening grupni in treninzi)
                        {
                            if (grupni.Naziv.Equals(trening.Naziv))
                            {
                                if (grupni.Date_time < DateTime.Now) pomocna.Add(grupni);
                            }
                        }
                    }

                    if (TempData["posetioci"] != null) ViewBag.posetioci = TempData["posetioci"];

                    ViewBag.treninzi = pomocna;
                    TempData["trenutna"] = pomocna;
                }
                else if (TempData["prosledjena"] != null && sesija == null)
                {
                    Session["treninzi"] = TempData["prosledjena"];
                    if (TempData["posetioci"] != null) ViewBag.posetioci = TempData["posetioci"];
                    ViewBag.treninzi = TempData["prosledjena"];
                }
                else if (TempData["prosledjena"] == null && sesija != null)
                {
                    if (TempData["posetioci"] != null) ViewBag.posetioci = TempData["posetioci"];
                    ViewBag.treninzi = sesija;
                }
                else if (TempData["prosledjena"] != null && sesija != null)
                {
                    Session["treninzi"] = TempData["prosledjena"];
                    if (TempData["posetioci"] != null) ViewBag.posetioci = TempData["posetioci"];
                    ViewBag.treninzi = TempData["prosledjena"];
                }

                return View();
            }
            else return View();
        }
        [HttpPost]
        public ActionResult Spisak(string spisak)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            List<GrupniTrening> treninzi = (List<GrupniTrening>)TempData["trenutna"];
            List<GrupniTrening> sesija = (List<GrupniTrening>)Session["treninzi"];
            List<Korisnik> posetioci = new List<Korisnik>();

            if (sesija == null)
            {
                foreach (GrupniTrening trening in treninzi)
                {
                    if (trening.Naziv.Equals(spisak))
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
            else
            {
                foreach (GrupniTrening trening in sesija)
                {
                    if (trening.Naziv.Equals(spisak))
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
        }
        [HttpPost]
        public ActionResult Search(string izbor)
        {
            Korisnik korisnik = (Korisnik)Session["logovani"];
            List<GrupniTrening> treninzi = (List<GrupniTrening>)TempData["trenutna"];
            List<GrupniTrening> pomocna = new List<GrupniTrening>();

            if (izbor.Equals("pretrazi"))
            {

                var naziv = Request["nazivt"];
                var tip = Request["tip"];
                DateTime mingran;
                DateTime maxgran;
                bool isNumeric = false;

                isNumeric = DateTime.TryParse(Request["mingran"], out mingran);
                isNumeric = DateTime.TryParse(Request["maxgran"], out maxgran);

                foreach (GrupniTrening trening in treninzi)
                {
                    if (trening.Naziv.Equals(naziv)) pomocna.Add(trening);
                    if (trening.TipTreninga.ToString() == tip && !(pomocna.Contains(trening))) pomocna.Add(trening);
                    if (trening.Date_time > mingran && trening.Date_time < maxgran && mingran.Year > korisnik.FitnesCentar.Godina_otvaranja
                        && maxgran.Year > korisnik.FitnesCentar.Godina_otvaranja && !(pomocna.Contains(trening))) pomocna.Add(trening);
                }

                TempData["prosledjena"] = pomocna;

                return RedirectToAction("Index", "ZavTrener");
            }
            else
            {

                TempData["prosledjena"] = treninzi;
                return RedirectToAction("Index", "ZavTrener");
            }

        }
        [HttpPost]
        public ActionResult Sort(string submit)
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)TempData["trenutna"];
            List<GrupniTrening> pomocna = treninzi;
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

                case "DatumA":
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

            return RedirectToAction("Index", "ZavTrener");

        }

        [HttpPost]
        public ActionResult SortDESC(string submit)
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)TempData["trenutna"];
            List<GrupniTrening> pomocna = treninzi;
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

                case "DatumD":
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

            return RedirectToAction("Index", "ZavTrener");

        }

    }
}