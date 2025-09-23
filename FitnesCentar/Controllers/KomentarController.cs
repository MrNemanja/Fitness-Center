using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;

namespace FitnesCentar.Controllers
{
    public class KomentarController : Controller
    {
        // GET: Komentar
        public ActionResult Index()
        {
            ViewBag.errors = TempData["error"];

            return View();
        }

        public ActionResult Ostavi(Komentar komentar, int Ocena)
        {
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];
            Korisnik korisnik = (Korisnik)Session["logovani"];
            string whole = "";
            string[] errors;

            switch(Ocena)
            {
                case 1:
                    komentar.Ocena = 1;
                    break;

                case 2:
                    komentar.Ocena = 2;
                    break;

                case 3:
                    komentar.Ocena = 3;
                    break;

                case 4:
                    komentar.Ocena = 4;
                    break;

                case 5:
                    komentar.Ocena = 5;
                    break;
            }

            if (komentar.Opis == null || komentar.Opis == "") whole += "Morate uneti neki komentar;";

            if (komentar.Ocena == 0) whole += "Morate uneti ocenu;";

            if (whole != String.Empty)
            {
                errors = whole.Split(';');
                errors = errors.Reverse().Skip(1).Reverse().ToArray();
                TempData["error"] = errors;
                return RedirectToAction("Index");
            }
            else
            {
                komentar.FitnesCentar = new Fitnes_Centar();
                komentar.FitnesCentar.Naziv = TempData["naziv_centra"].ToString();
                komentar.Blokiran = false;
                komentar.Posetilac = korisnik;
                komentari.Add(komentar);
                Data.PutKomentar(komentar, korisnik, "~/App_Data/komentari.txt");

                return RedirectToAction("Index", "Posetilac");
            }
        }

    }
}