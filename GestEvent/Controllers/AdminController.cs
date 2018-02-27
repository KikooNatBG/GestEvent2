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
        public ActionResult AjouterEvenement(int pID=0)
        {
            
            AdminViewModels vm = new AdminViewModels();
            vm.Title = "Ajouter un évènement";
            List<Theme> maListe = themeService.FindAll();
            if (pID!=0) { vm.MonEvent = eventService.Get(pID); vm.Title = "Modification d'un évènement"; vm.IdThemeSelected = vm.MonEvent.Theme.Id;   }
            vm.ListTheme = maListe;
            
            return View(vm);
        }

        //[HttpPost]
        public ActionResult AjoutEvent(AdminViewModels pVm)
        {
            if (pVm.IdThemeSelected != 0) { pVm.MonEvent.Theme = themeService.Get(pVm.IdThemeSelected); }
            if(pVm.IdThemeSelected != 0) { pVm.MonEvent.Theme = themeService.Get(pVm.IdThemeSelected); }
            pVm.MonEvent.Images = new List<EventImage>();
            if (pVm.MonEvent.Id == 0) { ModelState.Remove("MonEvent.Id"); }



            //  JL ASSocier les Images a l'event avant l'ajout !
         /*   HttpFileCollectionBase photos = Request.Files;
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
                    pVm.MonEvent.Images.Add(image);
                }
            }
            */




            Event MonEvent = pVm.MonEvent;
            if (ModelState.IsValid){
          
                //HttpFileCollectionBase photos = Request.Files;
                //if (null != photos)
                //{
                //    List<EventImage> images = new List<EventImage>();

                //    DataTable dt = new DataTable { Columns = { new DataColumn("Path") } };
                //    for (int i = 0; i < photos.Count; i++)
                //    {
                //        HttpPostedFileBase photo = photos[i];
                //        string path = Server.MapPath("~") + "\\Images\\" + photo.FileName;
                //        dt.Rows.Add(photo.FileName);
                //        photo.SaveAs(path);

                //        EventImage image = new EventImage();
                //        image.Name = photo.FileName;
                //        image.Path = path;
                //        image.Event = pVm.MonEvent;

                //        images.Add(image);
                //    }

                //    pVm.MonEvent.Images = images;
                //}

                if (pVm.MonEvent.Id != 0) { eventService.Update(pVm.MonEvent); }
                else { eventService.Create(pVm.MonEvent); }

            }
            else {
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