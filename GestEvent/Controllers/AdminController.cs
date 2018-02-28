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
using System.Data;

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
        public ActionResult AjouterEvenement(int pID = 0)
        {

            AdminViewModels vm = new AdminViewModels();
            vm.Title = "Ajouter un évènement";
            List<Theme> maListe = themeService.FindAll();
            if (pID != 0) { vm.MonEvent = eventService.Get(pID); vm.Title = "Modification d'un évènement"; vm.IdThemeSelected = vm.MonEvent.Theme.Id; }
            vm.ListTheme = maListe;

            return View(vm);
        }

        [HttpPost]
        public ActionResult AjoutEvent(AdminViewModels pVm)
        {
            Event MonEvent = pVm.MonEvent;

            if (pVm.IdThemeSelected != 0) { pVm.MonEvent.Theme = themeService.Get(pVm.IdThemeSelected); }

            if (MonEvent.Id == 0) { ModelState.Remove("MonEvent.Id"); }
            else
            {
                MonEvent = eventService.Get(MonEvent.Id);
                MonEvent.Images = eventService.Get(MonEvent.Id).Images;
            }

            if (ModelState.IsValid)
            {
                HttpFileCollectionBase photos = Request.Files;

                if (pVm.MonEvent.Images == null) {
                    pVm.MonEvent.Images = new List<EventImage>();
                }
                
                for (int i = 0; i < photos.Count; i++)
                {
                    HttpPostedFileBase photo = photos[i];
                    if (photo.ContentLength > 0)
                    {
                        string path = Server.MapPath("~") + "\\Images\\" + photo.FileName;
                        photo.SaveAs(path);

                        EventImage image = new EventImage();
                        image.Name = photo.FileName;
                        image.Path = "\\Images\\" + photo.FileName;
                        image.Event = pVm.MonEvent;

                        pVm.MonEvent.Images.Add(image);
                    }
                }
                
                if (MonEvent.Id != 0) { eventService.Update(MonEvent); }
                else { eventService.Create(MonEvent); }
            }
            else {
                return RedirectToAction("AjouterEvenement", new { pID = pVm.MonEvent.Id });
            }
            return RedirectToAction("IndexEvenement");
        }

        public ActionResult SupprimerEvent(int pID = 0)
        {
            if (pID == 0) { return RedirectToAction("IndexEvenement"); }
            else { eventService.Delete(eventService.Get(pID)); }
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