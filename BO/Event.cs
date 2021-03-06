﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BO
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PlaceName { get; set; }

        [NotMapped]
        public double Longitude { get; set; }

        [NotMapped]
        public double Latitude { get; set; }

        public String Address { get; set; }

        public DateTime Date { get; set; }

        public double Duration { get; set; }

        public string Description { get; set; }

        public virtual Theme Theme { get; set; }

        public virtual List<EventImage> Images { get; set; }

    }
}
