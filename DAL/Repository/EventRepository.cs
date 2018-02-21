using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class EventRepository : GenericRepository<Event>
    {
        public EventRepository(Context context) : base(context)
        {

        }
    }
}
