using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Entities
{
    public class Parkings
    {
        [JsonProperty(PropertyName = "records")]
        public List<ParkingDTO> ParkingList { get; set; }
    }
}
