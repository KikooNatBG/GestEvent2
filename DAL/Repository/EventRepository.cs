using BO;
using System;
using System.Collections.Generic;
using System.IO;
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

        public List<Event> GetEventsByIDTheme(int pIDTheme)
        {
            return this.Context.Events.Where(c => c.Theme.Id == pIDTheme).ToList();
        }

        public override void Delete(Event evenement)
        {
            if (null != evenement.Images)
            {
                foreach (var image in evenement.Images)
                {
                    if (File.Exists(image.Path))
                    {
                        File.Delete(image.Path);
                    }
                }
                evenement.Images.Clear();                
            }
            base.Delete(evenement);
        }
    }
}
