using BO;
using System.Collections.Generic;

namespace GestEvent.Models
{
    public class ConviveViewModel
    {
        public List<Event> LstEvents { get; set; }

        public Event Event { get; set; }

        public List<Parking> LstParkings { get; set; }

        public Parking Parking { get; set; }

        public string ViewRubricUrl { get; set; }

        public string Rubric { get; set; }

        public ConviveViewModel()
        {
            LstParkings = new List<Parking>();
            Event = new Event();
            LstEvents = new List<Event>();
            Parking = new Parking();
        }
    }
}