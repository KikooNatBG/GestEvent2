using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    class ThemeRepository : GenericRepository<Theme>
    {
        public ThemeRepository(Context context) : base(context)
        {

        }
    }
}
