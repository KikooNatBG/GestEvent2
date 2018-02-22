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
            List<Event> maListe = eventService.findAll();
            vm.maListe = maListe;
            return View(vm);
        }

        [HttpGet]
        public ActionResult AjouterEvenement(int pID=0)
        {
            AdminViewModels vm = new AdminViewModels();
            if (pID!=0) { vm.monEvent = eventService.get(pID); }
            List<Theme> maListe = themeService.findAll();  
            vm.listTheme = maListe;
            return View(vm);
        }

        public ActionResult AjoutEvent(AdminViewModels pVm)
        {
            if(pVm.idThemeSelected != 0) { pVm.monEvent.Theme = themeService.get(pVm.idThemeSelected); }
            Event monEvent = pVm.monEvent;
            if (ModelState.IsValid){
                if (pVm.monEvent.Id != 0) { eventService.update(pVm.monEvent); }
                else { eventService.create(pVm.monEvent); } 
            }else{
                return RedirectToAction("AjouterEvenement", new { pID = pVm.monEvent.Id });
            } 
            return RedirectToAction("IndexEvenement");
        }

        public ActionResult SupprimerEvent(int pID = 0)
        {
            if (pID == 0) { return RedirectToAction("IndexEvenement"); }
            else {  eventService.delete(eventService.get(pID)); }
            return RedirectToAction("IndexEvenement");
        }
    }
}