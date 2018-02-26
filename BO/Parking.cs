using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BO
{
    public class Parking
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public ParkingInfo ParkingInfo { get; set; }

    }

    public class ParkingInfo
    {
        [JsonProperty(PropertyName = "key")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "max")]
        public int MaxPlaces { get; set; }

        [JsonProperty(PropertyName = "free")]
        public int FreePlaces { get; set; }

        [JsonProperty(PropertyName = "geo")]
        public List<double> Coordinates { get; set; }

        public double DistanceFromEvent { get; set; }

        public double DistanceFromStart { get; set; }

        public double HourPriceDay { get; set; }

        public double HourPriceNight { get; set; }


    }

}
