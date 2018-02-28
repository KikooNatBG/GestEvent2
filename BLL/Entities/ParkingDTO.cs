using BO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Entities
{
    public class ParkingDTO
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public ParkingInfo ParkingInfo { get; set; }

        public Parking Parking { get; set; }

    }

 
}
