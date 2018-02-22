using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestEvent.ViewModel
{
    public class ConviveViewModel
    {
        public List<Event> LstEvents { get; set; }

        public List<Parking> LstParkings { get; set; }

        public Event Event { get; set; }

        public string ViewRubricUrl { get; set; }

        public string rubric { get; set; }

        public ConviveViewModel()
        {
            LstParkings = new List<Parking>();
            LstEvents = new List<Event>();
            Event = new Event();
        }
    }
}