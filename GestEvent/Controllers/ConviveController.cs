using BO;
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

        // GET: Convive
        public ActionResult Index()
        {
            List<Event> lstEvent = new List<Event>();

            Event evenement = new Event();

            evenement.Id = 1;
            evenement.Description = "Reunion distric";
            evenement.Date = new DateTime(2018, 05, 02);
            evenement.Duration = 2;

            Event evenement2 = new Event();

            evenement2.Id = 2;
            evenement2.Description = "Conference babouin";
            evenement2.Date = new DateTime(2018, 05, 06);
            evenement2.Duration = 3;

            _lstEvent = new List<Event>();
            _lstEvent.Add(evenement);
            _lstEvent.Add(evenement2);

            ConviveViewModel conviveVM = new ConviveViewModel();

            conviveVM.ViewRubricUrl = "~/Views/Convive/Research.cshtml";

            conviveVM.LstEvents = _lstEvent;

            return View(conviveVM);
        }

        public ActionResult ResearchParking(ConviveViewModel conviveViewModel)
        {
            List<Parking> lstParking = new List<Parking>();

            Parking parking = new Parking();

            parking.Id = 1;
            parking.Name = "Gare Sud";
            parking.TotalPlaces = 200;
            parking.FreePlaces = 100;

            lstParking.Add(parking);

            Parking parking2 = new Parking();

            parking2.Id = 2;
            parking2.Name = "Colombier";
            parking2.TotalPlaces = 300;
            parking2.FreePlaces = 200;

            lstParking.Add(parking2);

            Parking parking3 = new Parking();

            parking3.Id = 3;
            parking3.Name = "Places des lices";
            parking3.TotalPlaces = 2500;
            parking3.FreePlaces = 2000;

            lstParking.Add(parking3);
            
            _lstParking = lstParking;
            
            ConviveViewModel conviveVM = new ConviveViewModel();

            conviveVM.Parking = _lstParking[0];

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