using BLL.Entities;
using BO;
using System;
using System.Collections.Generic;

namespace GestEvent.Models
{
    public class ConviveViewModel
    {
        public List<Event> LstEvents { get; set; }

        public Event Event { get; set; }

        public List<ParkingDTO> LstParkings { get; set; }

        public ParkingDTO Parking { get; set; }

        public List<Double> LatLongAdresseDepartUser { get; set; }

        public List<Double> LatlongParkingDest { get; set; }

        public List<Double> LatLongEvent { get; set; }

        public string AddresseUser { get; set; }
        public string ViewRubricUrl { get; set; }

        public string Rubric { get; set; }

        public ConviveViewModel()
        {
            LstParkings = new List<ParkingDTO>();
            Event = new Event();
            LstEvents = new List<Event>();
            Parking = new ParkingDTO();
        }
    }
}