using System.Net.Http;
using BO;
using System.Threading.Tasks;
using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ParkingService
    {
        static string _parkUrl = "https://data.rennesmetropole.fr/api/records/1.0/search/?dataset=export-api-parking-citedia";
        static string _googleApiUrl = "";
        static string _googleApiKey = "";
        private Parkings parkings = new Parkings();

        public ParkingService()
        {

        }

        public List<Parking> GetNearerParkings(double latitudeEvent, double longitudeEvent, double latitudeStart, double longitudeStart)
        {
            var response = new HttpClient().GetStringAsync(_parkUrl).Result;
            //var jsonResponse = await response;
            parkings = JsonConvert.DeserializeObject<Parkings>(response);
            //Console.WriteLine(parkings);
            //TODO : Ajouter long lat adresse de depart
            AddDistanceInParkings(latitudeEvent, longitudeEvent);
            parkings.ParkingsList = parkings.ParkingsList.OrderBy(p => p.ParkingInfo.DistanceFromEvent).Take(3).ToList();
            //TODO : Sort by start point distance
            return parkings.ParkingsList;
        }

        //TODO : Ajouter long lat adresse de depart
        public void AddDistanceInParkings(double latitudeEvent, double longitudeEvent)
        {
            foreach (Parking p in parkings.ParkingsList)
            {
                p.ParkingInfo.DistanceFromEvent = DistanceBetweenPoints(latitudeEvent, longitudeEvent, p.ParkingInfo.Coordinates[0], p.ParkingInfo.Coordinates[1]);
                // p.ParkingInfo.DistanceFromStart = DistanceBetweenPoints(latitudeStart, longitudeStart, p.ParkingInfo.Coordinates[0], p.ParkingInfo.Coordinates[1]);
            }
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