using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Price
    {
        public int Id { get; set; }
        public double Tarif { get; set; }

        public double Tarif01h { get; set; }

        public double Tarif12h { get; set; }

        public double Tarif23h { get; set; }

        public double Tarif34h { get; set; }

        public double Tarif4Plus { get; set; }

        public Plage Plage { get; set; }

    }
}
