using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.Core
{
    public abstract class BaseHelper
    {
        protected Talento.Core.Data.ApplicationDbContext Db;

        public BaseHelper(Talento.Core.Data.ApplicationDbContext db )
        {
            Db = db;                
        }
    }
}
