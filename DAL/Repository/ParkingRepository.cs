using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ParkingRepository : GenericRepository<Parking>
    {
        public ParkingRepository(Context context) : base(context)
        {

        }

        public Parking GetByName(string Name)
        {
            return this.Context.Parkings.Where(p => p.Name == Name).First();
        }
    }
}
