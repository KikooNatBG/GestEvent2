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
        static string _parkUrl = "http://data.citedia.com/r1/parks/";

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

            callParkingAPI();

            //TODO : Get db parkings for each parking
            foreach (var parking in parkings.ParkingList)
            {
                parking.Parking = _parkingRepository.GetByName(parking.ParkingInfo.Name);
            }

            AddDistanceInParkings(latitudeEvent, longitudeEvent,latitudeStart,longitudeStart);
            RemoveParkingWhenLess10();

            parkings.ParkingList = parkings.ParkingList.OrderBy(p => p.ParkingInfo.DistanceFromEvent).Take(3).ToList();
            parkings.ParkingList = parkings.ParkingList.OrderBy(p => p.ParkingInfo.DistanceFromStart).ToList();

            calculatePriceForEvent(e);

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

        public void calculatePriceForEvent(Event e)
        {
            var eventDay =  (int)e.Date.DayOfWeek;
            var eventStartTime = e.Date.TimeOfDay;

            var eventDurationTime = TimeSpan.FromHours(e.Duration);
            var eventEndTime = eventStartTime.Add(eventDurationTime);

            foreach (var p in parkings.ParkingList)
            {
                var parkOpenTime = p.Parking.OpenHours.Where(h => h.DayNumber == eventDay).Select(h => h.StartHour.TimeOfDay).First();
                var parkCloseTime = p.Parking.OpenHours.Where(h => h.DayNumber == eventDay).Select(h => h.EndHour.TimeOfDay).First();
                

                if (parkOpenTime < eventStartTime && parkCloseTime > eventEndTime)
                {
                    // TODO : Get parking plage
                    if (eventStartTime > TimeSpan.FromHours(7) && eventEndTime < TimeSpan.FromHours(21))
                    {
                        var parkingPrice = p.Parking.Prices.Where(price => price.Plage.StartHour.Hour == TimeSpan.FromHours(7).Hours).First();

                        p.CalculatedParkingPrice = getParkingPrice(parkingPrice, eventDurationTime);


                    }
                    else {
                        var parkingPrice = p.Parking.Prices.Where(price => price.Plage.StartHour.Hour == TimeSpan.FromHours(21).Hours).First();

                        p.CalculatedParkingPrice = getParkingPrice(parkingPrice, eventDurationTime);
                    }

                }
                else
                {
                    parkings.ParkingList.Remove(p);
                }
            }

        }

        public double getParkingPrice(Price parkingPrice, TimeSpan eventDurationTime)
        {
            if (parkingPrice.Tarif01h == null)
            {
                return eventDurationTime.Hours * parkingPrice.Tarif;
            }
            else
            {
                if (eventDurationTime.Hours < 1)
                {
                    if (parkingPrice.Tarif01h.HasValue) {
                        return eventDurationTime.Hours * parkingPrice.Tarif01h.Value;
                    }
                }

                if (eventDurationTime.Hours > 1 && eventDurationTime.Hours < 2)
                {
                    if (parkingPrice.Tarif01h.HasValue && parkingPrice.Tarif12h.HasValue)
                    {
                        return parkingPrice.Tarif01h.Value + (eventDurationTime.Hours - TimeSpan.FromHours(1).Hours) * parkingPrice.Tarif12h.Value;
                    }
                }

                if (eventDurationTime.Hours > 2 && eventDurationTime.Hours < 3)
                {

                    if (parkingPrice.Tarif01h.HasValue && parkingPrice.Tarif12h.HasValue)
                    {
                        return parkingPrice.Tarif01h.Value + parkingPrice.Tarif12h.Value + (eventDurationTime.Hours - TimeSpan.FromHours(2).Hours) * parkingPrice.Tarif23h.Value;
                    }
          
                }

                if (eventDurationTime.Hours > 3 && eventDurationTime.Hours < 4)
                {
                    if (parkingPrice.Tarif01h.HasValue && parkingPrice.Tarif12h.HasValue)
                    {
                        return parkingPrice.Tarif01h.Value + parkingPrice.Tarif12h.Value + parkingPrice.Tarif23h.Value + (eventDurationTime.Hours - TimeSpan.FromHours(3).Hours) * parkingPrice.Tarif34h.Value;
                    }
                }
                
                if(eventDurationTime.Hours > 4)
                {
                    if (parkingPrice.Tarif01h.HasValue && parkingPrice.Tarif12h.HasValue)
                    {
                        return parkingPrice.Tarif01h.Value + parkingPrice.Tarif12h.Value + parkingPrice.Tarif23h.Value + parkingPrice.Tarif34h.Value + (eventDurationTime.Hours - TimeSpan.FromHours(4).Hours) * parkingPrice.Tarif4Plus.Value;
                    }
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