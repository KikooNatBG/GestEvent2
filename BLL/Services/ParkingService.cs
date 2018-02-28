using System.Net.Http;
using BO;
using System.Threading.Tasks;
using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using BLL.Entities;
using DAL.Repository;

namespace BLL.Services
{
    public class ParkingService
    {
        static string _parkUrl = "https://data.rennesmetropole.fr/api/records/1.0/search/?dataset=export-api-parking-citedia";

        private Parkings parkings = new Parkings();

        private readonly ParkingRepository _parkingRepository;

        public ParkingService(ParkingRepository parkingRepository)
        {
            this._parkingRepository = parkingRepository;
        }

        public void callParkingAPI()
        {
            var response = new HttpClient().GetStringAsync(_parkUrl).Result;
            //var jsonResponse = await response;
            parkings = JsonConvert.DeserializeObject<Parkings>(response);
        }

        public List<ParkingDTO> GetNearerParkings(double latitudeEvent, double longitudeEvent, double latitudeStart, double longitudeStart,Event e)
        {
            //TODO : Get db parkings for each parking
            foreach (var parking in parkings.ParkingList)
            {
                parking.Parking = _parkingRepository.GetByName(parking.ParkingInfo.Name);
            }
            
            


            AddDistanceInParkings(latitudeEvent, longitudeEvent,latitudeStart,longitudeStart);
            RemoveParkingWhenLess10();
            parkings.ParkingList = parkings.ParkingList.OrderBy(p => p.ParkingInfo.DistanceFromEvent).Take(3).ToList();
            parkings.ParkingList = parkings.ParkingList.OrderBy(p => p.ParkingInfo.DistanceFromStart).ToList();

            //calculatePriceForEvent(e);

            return parkings.ParkingList;
        }


        public void RemoveParkingWhenLess10()
        {
            foreach (ParkingDTO p in parkings.ParkingList){ if (p.ParkingInfo.FreePlaces < 10) { parkings.ParkingList.Remove(p); } }
        }
        public void RemoveParkingByParkingDisponibility(Event e)
        {
            foreach (ParkingDTO p in parkings.ParkingList) {
                if (!p.Parking.IsAlwaysOpen) {
                    parkings.ParkingList.Remove(p);
                }
            }
        }


        //TODO : Ajouter long lat adresse de depart
        public void AddDistanceInParkings(double latitudeEvent, double longitudeEvent, double latitudeStart, double longitudeStart)
        {
            foreach (ParkingDTO p in parkings.ParkingList)
            {
                p.ParkingInfo.DistanceFromEvent = DistanceBetweenPoints(latitudeEvent, longitudeEvent, p.ParkingInfo.Coordinates[0], p.ParkingInfo.Coordinates[1]);
                p.ParkingInfo.DistanceFromStart = DistanceBetweenPoints(latitudeStart, longitudeStart, p.ParkingInfo.Coordinates[0], p.ParkingInfo.Coordinates[1]);
            }
        }

        public double calculatePriceForEvent(Event e)
        {
            var eventDay =  (int)e.Date.DayOfWeek;
            var eventStartTime = e.Date.TimeOfDay;

            var eventDurationTime = TimeSpan.FromHours(e.Duration);
            var eventEndTime = eventStartTime.Add(eventDurationTime);

            
            foreach (var p in parkings.ParkingList)
            {
                var parkOpenTime = p.Parking.OpenHours.Where(h => h.DayNumber == eventDay).Select(h => h.StartHour.TimeOfDay).First();
                var parkCloseTime = p.Parking.OpenHours.Where(h => h.DayNumber == eventDay).Select(h => h.EndHour.TimeOfDay).First();


                if(parkOpenTime < eventStartTime && parkCloseTime > eventEndTime)
                {
                    // TODO : Get parking plage
                    // TODO : Get duration passed in parking
                    // TODO : Get price for plage 
                    // TODO : calculate price for each parking
                   
                }
                else
                {
                    parkings.ParkingList.Remove(p);
                }
            }

            return 0;
        }

        public double DistanceBetweenPoints(double latitudeA, double longitudeA, double latitudeB, double longitudeB)
        {
            var earthRadius = 6371000;

            var lat1 = DegreesToRadians(latitudeA);
            var lat2 = DegreesToRadians(latitudeB);
            var dLat = DegreesToRadians(latitudeB - latitudeA);
            var dLong = DegreesToRadians(longitudeB - longitudeA);

            var formula = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                          Math.Sin(dLong / 2) * Math.Sin(dLong / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var calcul = 2 * Math.Atan2(Math.Sqrt(formula), Math.Sqrt(1 - formula));
            return earthRadius * calcul;
        }

        public double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

    }
}