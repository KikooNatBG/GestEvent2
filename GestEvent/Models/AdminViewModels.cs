using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BO;

namespace GestEvent.Models
{
    public class AdminViewModels
    {
        public List<Event> maListe { get; set; }

        public Event monEvent { get; set; }

        public int idThemeSelected { get; set; }

        public List<Theme> listTheme { get; set; }


    }
}