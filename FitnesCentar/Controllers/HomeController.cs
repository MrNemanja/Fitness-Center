using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;

namespace FitnesCentar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)HttpContext.Application["centri"];

            if (TempData["prosledjena"] == null)
            {

                List<Fitnes_Centar> pocetna = new List<Fitnes_Centar>();

                foreach(Fitnes_Centar centar in centri)
                {
                    if (centar.IsDeleted == false) pocetna.Add(centar);
                }

                TempData["novi"] = pocetna;
                Fitnes_Centar temp = null;

                for (int i = 0; i < pocetna.Count - 1; i++)
                {
                    for (int j = i + 1; j < pocetna.Count; j++)
                    {
                        if (String.Compare(pocetna[i].Naziv, pocetna[j].Naziv) > 0)
                        {
                            temp = pocetna[i];
                            pocetna[i] = pocetna[j];
                            pocetna[j] = temp;
                        }
                    }

                }

                centri = pocetna;
                ViewBag.centri = centri;

            }
            else
            {
                ViewBag.centri = TempData["prosledjena"];
                TempData["prosledjena"] = null;
            }

            return View();
            
        }
        [HttpPost]
        public ActionResult Search(string izbor)
        {
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)TempData["novi"];
            List<Fitnes_Centar> pomocna = new List<Fitnes_Centar>();

            if (izbor.Equals("pretrazi"))
            {

                var naziv = Request["naziv"];
                var adresa = Request["adresa"];
                int min_godina;
                int max_godina;
                bool isNumeric = false;

                isNumeric = int.TryParse(Request["mingran"], out min_godina);
                isNumeric = int.TryParse(Request["maxgran"], out max_godina);

                foreach (Fitnes_Centar centar in centri)
                {
                    if (centar.Naziv.Equals(naziv)) pomocna.Add(centar);
                    if (centar.Adresa.Equals(adresa) && !(pomocna.Contains(centar))) pomocna.Add(centar);
                    if (centar.Godina_otvaranja > min_godina && centar.Godina_otvaranja < max_godina && !(pomocna.Contains(centar)))
                        pomocna.Add(centar);
                }

                TempData["prosledjena"] = pomocna;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                pomocna = centri;

                Fitnes_Centar temp = null;

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

                centri = pomocna;
                TempData["prosledjena"] = centri;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult Sort(string submit)
        {
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)TempData["novi"];
            List<Fitnes_Centar> pomocna = centri;
            Fitnes_Centar temp = null;

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

                case "AdresaA":
                    {
                        for (int i = 0; i < pomocna.Count - 1; i++)
                        {
                            for (int j = i + 1; j < pomocna.Count; j++)
                            {
                                if (String.Compare(pomocna[i].Adresa, pomocna[j].Adresa) > 0)
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
                                if (pomocna[i].Godina_otvaranja > pomocna[j].Godina_otvaranja)
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

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public ActionResult SortDESC(string submit)
        {
            List<Fitnes_Centar> centri = (List<Fitnes_Centar>)TempData["novi"];
            List<Fitnes_Centar> pomocna = centri;
            Fitnes_Centar temp = null;
;
           
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

                case "AdresaD":
                    {
                        for (int i = 0; i < pomocna.Count - 1; i++)
                        {
                            for (int j = i + 1; j < pomocna.Count; j++)
                            {
                                if (String.Compare(pomocna[i].Adresa, pomocna[j].Adresa) < 0)
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
                                if (pomocna[i].Godina_otvaranja < pomocna[j].Godina_otvaranja)
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
          
            return RedirectToAction("Index", "Home");
            
        }

        [HttpPost]
        public ActionResult Choice(string prihvati)
        {

            switch (prihvati)
            {
                case "Reg":
                    {
                        return RedirectToAction("RegisterView", "Register");
                    }

                case "Log":
                    {
                        return RedirectToAction("LogInView", "Register");
                    }
            }

            return null;

        }

    }
}