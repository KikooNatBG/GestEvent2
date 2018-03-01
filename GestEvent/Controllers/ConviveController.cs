using BLL.Entities;
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
        static List<ParkingDTO> _lstParking = new List<ParkingDTO>();
        static List<Event> _lstEvent = new List<Event>();

        private Context _context;
        private EventService _eventService;
        private ParkingService _parkingService;

        public ConviveController()
        {
            _context = new Context();
            _eventService = new EventService(new EventRepository(_context));
            _parkingService = new ParkingService(new ParkingRepository(_context));
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
            Event evenement = _eventService.Get(conviveViewModel.Event.Id);
            List<Double> LatLongEvent = _eventService.GetGeolocalisation(evenement.Address);
            List<Double> latLongAdressUser = _eventService.GetGeolocalisation(conviveViewModel.AddresseUser);

            List<ParkingDTO> lstParking = _parkingService.GetNearerParkings(LatLongEvent[0], LatLongEvent[1], latLongAdressUser[0], latLongAdressUser[1],conviveViewModel.Event);
            
            _lstParking = lstParking;

            ConviveViewModel conviveVM = new ConviveViewModel();

            if (_lstParking.Count != 0)
            {
                conviveVM.Parking = _lstParking[0];

                List<Double> latlongParkingDest = new List<double>();
                latlongParkingDest.AddRange(_lstParking[0].ParkingInfo.Coordinates);
                conviveVM.LatlongParkingDest = latlongParkingDest;
                conviveVM.LatLongEvent = LatLongEvent;
                conviveVM.Event = evenement;
                conviveVM.LatLongAdresseDepartUser = latLongAdressUser;
            }

            conviveVM.ViewRubricUrl = "~/Views/Convive/Parking.cshtml";
            
            return View("~/Views/Convive/Index.cshtml", conviveVM);
        }

        public ActionResult DisplayRubric(string rubric, string idEvent)
        {
            ConviveViewModel conviveViewModel = new ConviveViewModel();

            Event evenement = _eventService.Get(Convert.ToInt32(idEvent));

            conviveViewModel.Event = evenement;

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

                List<Double> latlongParkingDest = new List<double>();
                latlongParkingDest.AddRange(conviveViewModel.Parking.ParkingInfo.Coordinates);
                conviveViewModel.LatlongParkingDest = latlongParkingDest;
            }

            return Json(conviveViewModel, JsonRequestBehavior.AllowGet);
        }
    }
}