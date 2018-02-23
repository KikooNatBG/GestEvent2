using BLL.Services;
using BO;
using DAL;
using DAL.Repository;
using GestEvent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestEvent.Controllers
{
    public class ConviveController : Controller
    {
        static List<Parking> _lstParking = new List<Parking>();
        static List<Event> _lstEvent = new List<Event>();

        private Context _context;
        private EventService _eventService;
        private ParkingService _parkingService;

        public ConviveController()
        {
            _context = new Context();
            _eventService = new EventService(new EventRepository(_context));
            _parkingService = new ParkingService();
        }

        // GET: Convive
        public ActionResult Index()
        {
            List<Event> lstEvent = new List<Event>();

            lstEvent = _eventService.FindAll();

            _lstEvent = lstEvent;

            ConviveViewModel conviveVM = new ConviveViewModel();

            conviveVM.ViewRubricUrl = "~/Views/Convive/Research.cshtml";

            conviveVM.LstEvents = _lstEvent;

            return View(conviveVM);
        }

        public ActionResult ResearchParking(ConviveViewModel conviveViewModel)
        {
            List<Parking> lstParking = new List<Parking>();

            Event evenement = _eventService.GetGeolocalisation(conviveViewModel.Event.Address);

            lstParking = _parkingService.GetNearerParkings(evenement.Lagitude, evenement.Longitude);

            _lstParking = lstParking;

            ConviveViewModel conviveVM = new ConviveViewModel();

            if (_lstParking.Count != 0)
            {
                conviveVM.Parking = _lstParking[0];
            }

            conviveVM.ViewRubricUrl = "~/Views/Convive/Parking.cshtml";
          
            return View("~/Views/Convive/Index.cshtml", conviveVM);
            
        }

        public ActionResult DisplayRubric(string rubric)
        {
            ConviveViewModel conviveViewModel = new ConviveViewModel();

            conviveViewModel.ViewRubricUrl = "~/Views/Convive/";

            if (rubric.IndexOf("Research") != -1)
            {
                conviveViewModel.ViewRubricUrl += "Research.cshtml";
                conviveViewModel.LstEvents = _lstEvent;
                conviveViewModel.LstParkings = _lstParking;
            }
            else
            {
                conviveViewModel.ViewRubricUrl += "parking.cshtml";
                if (rubric.IndexOf("ParkingA") != -1)
                {
                    conviveViewModel.Parking = _lstParking[0];
                }
                else if (rubric.IndexOf("ParkingB") != -1)
                {
                    conviveViewModel.Parking = _lstParking[1];
                }
                else
                {
                    conviveViewModel.Parking = _lstParking[2];
                }
            }
            return Json(conviveViewModel.Parking, JsonRequestBehavior.AllowGet);
        }
    }
}