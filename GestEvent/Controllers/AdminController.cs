using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BO;
using GestEvent.Models;
using DAL;
using BLL.Services;
using DAL.Repository;
using System.Web.Helpers;

namespace GestEvent.Controllers
{
    public class AdminController : Controller
    {
        // NATHAN LA GROSSE PUTE
        private Context context;
        private EventService eventService;
        private ThemeService themeService;

        public AdminController()
        {
            this.context = new Context();
            eventService = new EventService(new EventRepository(context));
            themeService = new ThemeService(new ThemeRepository(context));
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexEvenement()
        {
            AdminViewModels vm = new AdminViewModels();
            List<Event> maListe = eventService.FindAll();
            vm.MaListe = maListe;
            return View(vm);
        }

        [HttpGet]
        public ActionResult AjouterEvenement(int pID=0)
        {
            
            AdminViewModels vm = new AdminViewModels();
            vm.Title = "Ajouter un évènement";
            List<Theme> maListe = themeService.FindAll();
            if (pID!=0) { vm.MonEvent = eventService.Get(pID); vm.Title = "Modification d'un évènement"; vm.IdThemeSelected = vm.MonEvent.Theme.Id;   }
            vm.ListTheme = maListe;
            
            return View(vm);
        }

        public ActionResult AjoutEvent(AdminViewModels pVm)
        {
            if(pVm.IdThemeSelected != 0) { pVm.MonEvent.Theme = themeService.Get(pVm.IdThemeSelected); }
            Event MonEvent = pVm.MonEvent;
            if (ModelState.IsValid){
                if (pVm.MonEvent.Id != 0) { eventService.Update(pVm.MonEvent); }
                else { eventService.Create(pVm.MonEvent); }
                HttpFileCollectionBase photos = Request.Files;
                if (null != photos)
                {
                    foreach (var photo in photos)
                    {
                        EventImage image = new EventImage();
                        image.Name = "nom de l'image";
                        image.Path = @"Images\" + image.Name;
                        //photo.save(@"~\" + image.Path);
                        image.Event = pVm.MonEvent;
                        //imageService.create(image)
                    }
                }
            }
            else{
                return RedirectToAction("AjouterEvenement", new { pID = pVm.MonEvent.Id });
            } 
            return RedirectToAction("IndexEvenement");
        }

        public ActionResult SupprimerEvent(int pID = 0)
        {
            if (pID == 0) { return RedirectToAction("IndexEvenement"); }
            else {  eventService.Delete(eventService.Get(pID)); }
            return RedirectToAction("IndexEvenement");
        }

        public ActionResult IndexThemes()
        {
            AdminViewModels vm = new AdminViewModels();
            List<Theme> maListe = themeService.FindAll();
            vm.ListTheme = maListe;
            return View(vm);
        }

        public ActionResult AjoutTheme(AdminViewModels pVm)
        {
            if (ModelState.IsValid)
            {
                themeService.Create(pVm.MonTheme);
            }

            return RedirectToAction("IndexThemes");
        }


        public JsonResult RemplirModal(string pID)
        {
            int ID = Convert.ToInt32(pID);
            List<Event> MaListe = eventService.GetEventByIDTheme(ID);
            return Json(MaListe);
        }
    }
}