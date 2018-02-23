using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BO
{
    public class Parkings
    {
        [JsonProperty(PropertyName = "records")]
        public List<Parking> ParkingsList { get; set; }
    }
}
