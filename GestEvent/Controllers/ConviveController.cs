using BO;
using GestEvent.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestEvent.Controllers
{
    public class ConviveController : Controller
    {
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

            ConviveViewModel conviveVM = new ConviveViewModel();

            conviveVM.ViewRubricUrl = "~/Views/Convive/Research.cshtml";

            conviveVM.LstEvents.Add(evenement);
            conviveVM.LstEvents.Add(evenement2);

            return View(conviveVM);
        }

        public ActionResult ResearchParking(ConviveViewModel conviveViewModel)
        {
            Parking parking = new Parking();

            parking.Id = 1;
            parking.Name = "Gare Sud";
            parking.TotalPlaces = 200;

            Parking parking2 = new Parking();

            parking.Id = 2;
            parking.Name = "Colombier";
            parking.TotalPlaces = 300;

            Parking parking3 = new Parking();

            parking.Id = 3;
            parking.Name = "Places des lices";
            parking.TotalPlaces = 2500;

            List<Parking> lstParking = new List<Parking>();

            lstParking.Add(parking);
            lstParking.Add(parking2);
            lstParking.Add(parking3);

            ConviveViewModel conviveVM = new ConviveViewModel();

            conviveVM.ViewRubricUrl = "~/Views/Convive/Parking.cshtml";

            conviveVM.LstParkings = lstParking;

            return View("~/Views/Convive/Index.cshtml", conviveVM);
        }

        public ActionResult DisplayRubric(ConviveViewModel conviveViewModel, string rubric)
        {
            conviveViewModel.ViewRubricUrl = "~/View/Convive/";

            if (rubric.IndexOf("Reshearch") != 0)
            {
                conviveViewModel.ViewRubricUrl += rubric;
            }
            else
            {
                conviveViewModel.ViewRubricUrl += rubric;
            }

            return View(conviveViewModel);
        }
    }
}