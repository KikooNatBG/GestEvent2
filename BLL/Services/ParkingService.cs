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

            callParkingAPI();

            //TODO : Get db parkings for each parking
            foreach (var parking in parkings.ParkingList)
            {
                parking.Parking = _parkingRepository.GetByName(parking.ParkingInfo.Name.ToLower());
            }

            AddDistanceInParkings(latitudeEvent, longitudeEvent,latitudeStart,longitudeStart);
            RemoveParkingWhenLess10();


            calculatePriceForEvent(e);
            parkings.ParkingList = parkings.ParkingList.OrderBy(p => p.ParkingInfo.DistanceFromEvent).Take(3).ToList();
            parkings.ParkingList = parkings.ParkingList.OrderBy(p => p.ParkingInfo.DistanceFromStart).ToList();


            return parkings.ParkingList;
        }


        public void RemoveParkingWhenLess10()
        {
            foreach (ParkingDTO p in parkings.ParkingList){ if (p.ParkingInfo.FreePlaces < 10) { parkings.ParkingList.Remove(p); } }
        }
        


        //TODO : Ajouter long lat adresse de depart
        public void AddDistanceInParkings(double latitudeEvent, double longitudeEvent, double latitudeStart, double longitudeStart)
        {
            foreach (ParkingDTO p in parkings.ParkingList)
            {
                p.DistanceFromEvent = Math.Round(DistanceBetweenPoints(latitudeEvent, longitudeEvent, p.ParkingInfo.Coordinates[0], p.ParkingInfo.Coordinates[1]),1);
                p.DistanceFromStart = Math.Round(DistanceBetweenPoints(latitudeStart, longitudeStart, p.ParkingInfo.Coordinates[0], p.ParkingInfo.Coordinates[1]),1);
            }
        }

        public void calculatePriceForEvent(Event e)
        {
            var eventDay =  (int)e.Date.DayOfWeek;
            var eventStartTime = e.Date.TimeOfDay;

            var eventDurationTime = TimeSpan.FromHours(e.Duration);
            var eventEndTime = eventStartTime.Add(eventDurationTime);

            for (int i = parkings.ParkingList.Count -1; i>=0; i--)
            {
                if(parkings.ParkingList[i].Parking.OpenHours.Count > 0)
                {
                    var parkOpenTime = parkings.ParkingList[i].Parking.OpenHours.Where(h => h.DayNumber == eventDay).Select(h => h.StartHour.TimeOfDay).FirstOrDefault();
                    var parkCloseTime = parkings.ParkingList[i].Parking.OpenHours.Where(h => h.DayNumber == eventDay).Select(h => h.EndHour.TimeOfDay).FirstOrDefault();


                    if (parkOpenTime < eventStartTime && parkCloseTime > eventEndTime)
                    {
                        // TODO : Get parking plage
                        if (eventStartTime > TimeSpan.FromHours(7) && eventEndTime < TimeSpan.FromHours(21))
                        {
                            var parkingPrice = parkings.ParkingList[i].Parking.Prices.Where(price => price.Plage.StartHour.Hour == TimeSpan.FromHours(7).Hours).FirstOrDefault();

                            parkings.ParkingList[i].CalculatedParkingPrice = getParkingPrice(parkingPrice, eventDurationTime);


                        }
                        else {
                            var parkingPrice = parkings.ParkingList[i].Parking.Prices.Where(price => price.Plage.StartHour.Hour == TimeSpan.FromHours(21).Hours).FirstOrDefault();

                            parkings.ParkingList[i].CalculatedParkingPrice = getParkingPrice(parkingPrice, eventDurationTime);
                        }

                    }
                    else
                    {
                        parkings.ParkingList.RemoveAt(i);
                    }
                } else
                {

                    if (eventStartTime > TimeSpan.FromHours(7) && eventEndTime < TimeSpan.FromHours(21))
                    {
                        var parkingPrice = parkings.ParkingList[i].Parking.Prices.Where(price => price.Plage.StartHour.Hour == TimeSpan.FromHours(7).Hours).FirstOrDefault();

                        parkings.ParkingList[i].CalculatedParkingPrice = getParkingPrice(parkingPrice, eventDurationTime);


                    }
                    else {
                        var parkingPrice = parkings.ParkingList[i].Parking.Prices.Where(price => price.Plage.StartHour.Hour == TimeSpan.FromHours(21).Hours).FirstOrDefault();

                        parkings.ParkingList[i].CalculatedParkingPrice = getParkingPrice(parkingPrice, eventDurationTime);
                    }
                }
               
            }

        }

        public double getParkingPrice(Price parkingPrice, TimeSpan eventDurationTime)
        {
            double calculatedParkingPrice = 0;
            if (!parkingPrice.Tarif01h.HasValue || parkingPrice.Tarif01h == null)
            {
                calculatedParkingPrice = calculatedParkingPrice + eventDurationTime.Hours * parkingPrice.Tarif + (eventDurationTime.Minutes / 60) * parkingPrice.Tarif;
            }
            else
            {
                if (eventDurationTime.Hours < 1)
                {
                    if (parkingPrice.Tarif01h.HasValue) {
                        calculatedParkingPrice = calculatedParkingPrice + (eventDurationTime.Minutes / 60) * parkingPrice.Tarif01h.Value;
                    }
                    else
                    {
                        calculatedParkingPrice = calculatedParkingPrice + (eventDurationTime.Minutes / 60) * parkingPrice.Tarif;
                    }
                }

                if (eventDurationTime.Hours >= 1 && eventDurationTime.Hours < 2)
                {
                    if (parkingPrice.Tarif01h.HasValue)
                    {
                        calculatedParkingPrice = calculatedParkingPrice +  parkingPrice.Tarif01h.Value;

                        if(parkingPrice.Tarif12h.HasValue)
                        {
                            calculatedParkingPrice = calculatedParkingPrice + (eventDurationTime.Minutes / 60) * parkingPrice.Tarif12h.Value;
                        }
                    }
                    else
                    {
                        calculatedParkingPrice = calculatedParkingPrice + parkingPrice.Tarif + (eventDurationTime.Minutes / 60) * parkingPrice.Tarif;
                    }
 
                }

                if (eventDurationTime.Hours > 2 && eventDurationTime.Hours <= 3)
                {

                    if (parkingPrice.Tarif01h.HasValue)
                    {
                        calculatedParkingPrice = calculatedParkingPrice + parkingPrice.Tarif01h.Value;

                        if (parkingPrice.Tarif12h.HasValue)
                        {
                            calculatedParkingPrice = calculatedParkingPrice + parkingPrice.Tarif12h.Value;

                            if (parkingPrice.Tarif23h.HasValue)
                            {
                                calculatedParkingPrice = calculatedParkingPrice + (eventDurationTime.Minutes / 60) * parkingPrice.Tarif23h.Value;
                            }
                        }
                    }
                    else
                    {
                        calculatedParkingPrice = calculatedParkingPrice + 2* parkingPrice.Tarif + (eventDurationTime.Minutes / 60) * parkingPrice.Tarif;
                    }

                }

                if (eventDurationTime.Hours > 3 && eventDurationTime.Hours <= 4)
                {
                    if (parkingPrice.Tarif01h.HasValue)
                    {
                        calculatedParkingPrice = calculatedParkingPrice + parkingPrice.Tarif01h.Value;

                        if (parkingPrice.Tarif12h.HasValue)
                        {
                            calculatedParkingPrice = calculatedParkingPrice + parkingPrice.Tarif12h.Value;

                            if (parkingPrice.Tarif23h.HasValue)
                            {
                                calculatedParkingPrice = calculatedParkingPrice + parkingPrice.Tarif23h.Value;

                                if(parkingPrice.Tarif34h.HasValue)
                                {
                                    calculatedParkingPrice = calculatedParkingPrice + (eventDurationTime.Minutes / 60) * parkingPrice.Tarif34h.Value;
                                }
                            }
                        }
                    }
                    else
                    {
                        calculatedParkingPrice = calculatedParkingPrice + 3 * parkingPrice.Tarif + (eventDurationTime.Minutes / 60) * parkingPrice.Tarif;
                    }
                }

                if (eventDurationTime.Hours > 4)
                {
                    if (parkingPrice.Tarif01h.HasValue)
                    {
                        calculatedParkingPrice = calculatedParkingPrice + parkingPrice.Tarif01h.Value;

                        if (parkingPrice.Tarif12h.HasValue)
                        {
                            calculatedParkingPrice = calculatedParkingPrice + parkingPrice.Tarif12h.Value;

                            if (parkingPrice.Tarif23h.HasValue)
                            {
                                calculatedParkingPrice = calculatedParkingPrice + parkingPrice.Tarif23h.Value;

                                if (parkingPrice.Tarif34h.HasValue)
                                {
                                    calculatedParkingPrice = calculatedParkingPrice + parkingPrice.Tarif34h.Value;

                                    if (parkingPrice.Tarif4Plus.HasValue)
                                    {
                                        calculatedParkingPrice = calculatedParkingPrice + (eventDurationTime.Hours - 4) * parkingPrice.Tarif4Plus.Value;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        calculatedParkingPrice = calculatedParkingPrice + eventDurationTime.Hours * parkingPrice.Tarif;
                    }
                }
            }
    
            return Math.Round(calculatedParkingPrice,2);
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