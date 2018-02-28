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
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsAlwaysOpen { get; set; }

        public List<OpenHour> OpenHours { get; set; }

        public List<Price> Prices { get; set; }
    }
}
