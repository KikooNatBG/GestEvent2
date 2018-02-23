using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BO;

namespace GestEvent.Models
{
    public class AdminViewModels
    {
        public List<Event> MaListe { get; set; }

        public Event MonEvent { get; set; }

        public int IdThemeSelected { get; set; }

        public List<Theme> ListTheme { get; set; }

        public Theme MonTheme { get; set; }

        public String Title { get; set; }


    }
}