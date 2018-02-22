using System.Net.Http;
using BO;
using System.Threading.Tasks;
using System;
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

        public Parkings GetParkings()
        {
            var response = new HttpClient().GetStringAsync(_parkUrl).Result;
            //var jsonResponse = await response;
            parkings = JsonConvert.DeserializeObject<Parkings>(response);
            //Console.WriteLine(parkings);
            return parkings;
        }

        public Parkings sortParkingsByDistance()
        {
            
        }
    }
}
