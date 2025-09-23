using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnesCentar.Models;

namespace FitnesCentar.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            List<UploadedFile> files = (List<UploadedFile>)HttpContext.Application["Files"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik korisnik = (Korisnik)Session["logovani"];

            if (TempData["sacuvan"] == null)
            {

                Korisnik k = new Korisnik();
                
                string path = "";

                foreach (Korisnik kor in korisnici)
                {
                    if (kor.Username.Equals(korisnik.Username))
                    {
                        k = kor;

                        foreach (UploadedFile file in files)
                        {
                            string[] delovi;
                            delovi = file.DirectoryPath.Split('\\');
                            if (delovi[delovi.Length - 2].Equals(kor.Username)) path = delovi[delovi.Length - 2] + "/" + file.Filename;
                        }

                    }
                }

                ViewBag.korisnik = k;
                ViewBag.files = path;
                ViewBag.errors = TempData["error"];
            }
            else
            {
 
                Korisnik k = new Korisnik();
                string path = "";

                foreach (Korisnik kor in korisnici)
                {
                    if (kor.Username.Equals(TempData["sacuvan"]))
                    {
                        k = kor;

                        foreach (UploadedFile file in files)
                        {
                            string[] delovi;
                            delovi = file.DirectoryPath.Split('\\');
                            if (delovi[delovi.Length - 2].Equals(kor.Username)) path = delovi[delovi.Length - 2] + "/" + file.Filename;
                        }
                    }
                }

                ViewBag.korisnik = k;
                ViewBag.files = path;
                ViewBag.errors = TempData["error"];
            }


            return View();
        }

        [HttpPost]
        public ActionResult Izmeni(Korisnik k, string pol, HttpPostedFileBase image)
        {
            List<UploadedFile> files = (List<UploadedFile>)HttpContext.Application["Files"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik izmeni = (Korisnik)Session["logovani"];
            string directory = "C:\\Users\\Nebojsa\\Desktop\\projekat\\pr091-2016-web-projekat-master\\pr091-2016-web-projekat-master\\FitnesCentar\\Files\\" + izmeni.Username;
            string whole = "";
            string[] errors;

            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            if (image != null)
            {
                try
                {
                    if (image.ContentLength > 0)
                    {

                        string fileName = Path.GetFileName(image.FileName);
                        string path = Path.Combine(Server.MapPath("~\\Files\\" + izmeni.Username), fileName);
                        DirectoryInfo di = new DirectoryInfo("C:\\Users\\Nebojsa\\Desktop\\projekat\\pr091-2016-web-projekat-master\\pr091-2016-web-projekat-master\\FitnesCentar\\Files\\" + izmeni.Username);

                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                        image.SaveAs(path);
                        files.Add(new UploadedFile(fileName, path));
                    }

                }
                catch
                {
                    whole = "Niste izabrali fajl, ili je neka druga greska u pitanju;";
                }
            }

            if (k.Ime == null || k.Ime == "") whole = whole + "Niste uneli ime;";

            if (k.Prezime == null || k.Prezime == "") whole = whole + "Niste uneli prezime;";

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
                return RedirectToAction("Index");
            }

            foreach(Korisnik korisnik in korisnici)
            {
                if(korisnik.Username.Equals(izmeni.Username))
                {
                    korisnik.Ime = k.Ime;
                    korisnik.Prezime = k.Prezime;
                    if (pol.Equals("PolM")) korisnik.Pol = Pol.MUSKI;
                    else if(pol.Equals("PolZ")) korisnik.Pol = Pol.ZENSKI;
                    korisnik.Datum_rodjenja = k.Datum_rodjenja;
                    Data.IzmenaKorisnik(korisnik, "~/App_Data/korisnici.txt", true);
                    break;
                }

            }

            TempData["sacuvan"] = izmeni;

            if(izmeni.Uloga == Uloga.POSETILAC) return RedirectToAction("Index2", "Home2");
            else if(izmeni.Uloga == Uloga.TRENER) return RedirectToAction("Index3", "Home3");
            else return RedirectToAction("Index4", "Home4");



        }
    }
}