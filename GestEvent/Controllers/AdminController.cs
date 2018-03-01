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
        private ImageService imageService;

        public AdminController()
        {
            this.context = new Context();
            eventService = new EventService(new EventRepository(context), new ImageRepository(context));
            themeService = new ThemeService(new ThemeRepository(context));
            imageService = new ImageService(new ImageRepository(context));
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
            }

            if (ModelState.IsValid)
            {
                HttpFileCollectionBase photos = Request.Files;

                if (MonEvent.Images == null) {
                    MonEvent.Images = new List<EventImage>();
                }
                
                for (int i = 0; i < photos.Count; i++)
                {
                    HttpPostedFileBase photo = photos[i];
                    if (photo.ContentLength > 0)
                    {
                        string name = MonEvent.Name + "_" + photo.FileName;
                        string path = Server.MapPath("~") + "\\Images\\" + name;
                        photo.SaveAs(path);

                        EventImage image = new EventImage();
                        image.Name = name;
                        image.Path = "\\Images\\" + name;

                        MonEvent.Images.Add(image);
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

        public ActionResult ImageDelete(int imageId, int monEventId)
        {
            //récupérer l'image par son id
            EventImage image = imageService.Get(imageId);

            //récupérer l'évènement
            Event monEvent = eventService.Get(monEventId);

            //supprimer l'image de l'évènement
            monEvent.Images.Remove(image);

            //mettre à jour l'évènement en bdd
            eventService.Update(monEvent);

            //supprimer l'image
            imageService.Delete(image);

            //retourner sur la modif de l'évènement
            return RedirectToAction("AjouterEvenement", new { pID = monEventId });
        }
    }
}