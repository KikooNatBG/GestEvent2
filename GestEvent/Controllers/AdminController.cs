using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BO;
using GestEvent.Models;

namespace GestEvent.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexEvenement()
        {
            AdminViewModels vm = new AdminViewModels();
            //Get all des evenements
            List <Event> maListe = new List<Event>();

            for (int i = 0; i<25; i++)
            {
                Event monEvent = new Event();
                monEvent.Address = "adresse" + i.ToString();
                monEvent.Date = DateTime.Now;
                monEvent.Description = "Une Description";
                monEvent.Duration = 1+i;
                Theme monTheme = new Theme();
                monTheme.Name = "NOM " + i.ToString();
                monEvent.Theme = monTheme;
                maListe.Add(monEvent);
            }

            vm.maListe = maListe;
            return View(vm);
        }

        [HttpGet]
        public ActionResult AjouterEvenement()
        {
            // get all de thèmes
            List<Theme> maListe = new List<Theme>();
            AdminViewModels vm = new AdminViewModels();

            for (int i=0; i<5; i++)
            {
                Theme monTheme = new Theme();
                monTheme.Id = i;
                monTheme.Name = "Nom " + i.ToString();
                maListe.Add(monTheme);
            }
            vm.listTheme = maListe;
            return View(vm);
        }

        public ActionResult AjoutEvent(AdminViewModels vm)
        {
            Event monEvent = vm.monEvent;
            return RedirectToAction("IndexEvenement");
           // return View(vm);
        }
    }
}