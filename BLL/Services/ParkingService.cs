using System.Net.Http;
using BO;
using System.Threading.Tasks;
using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

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
        
        public Parkings GetNearerParkings(double latitude,double longitude)
        {
            var response = new HttpClient().GetStringAsync(_parkUrl).Result;
            //var jsonResponse = await response;
            parkings = JsonConvert.DeserializeObject<Parkings>(response);
            //Console.WriteLine(parkings);
            // Sort
            AddDistanceInParkings(latitude,longitude);
            parkings.ParkingsList = parkings.ParkingsList.OrderBy(p => p.Distance).Take(3).ToList();
            return parkings;
        }

        public void AddDistanceInParkings(double latitude,double longitude)
        {
            foreach (Parking p in parkings.ParkingsList)
            {
                p.Distance = DistanceBetweenPoints(latitude,longitude,p.ParkingInfo.Coordinates[0],p.ParkingInfo.Coordinates[1]);
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
