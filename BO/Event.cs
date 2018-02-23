using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PlaceName { get; set; }

        public String Address { get; set; }

        public DateTime Date { get; set; }

        public double Duration { get; set; }

        public string Description { get; set; }

        public virtual Theme Theme { get; set; }

    }
}
