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

        public String Address { get; set; }

        public DateTime Date { get; set; }

        public Double Duration { get; set; }

        public String Description { get; set; }

        public Theme Theme { get; set; }

    }
}
