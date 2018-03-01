using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ImageRepository : GenericRepository<EventImage>
    {
        public ImageRepository(Context context) : base(context)
        {

        }
    }
}
