using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class PositionLogHelper : BaseHelper, IPositionLog
    {
        public PositionLogHelper(Core.Data.ApplicationDbContext db) : base(db)
        {

        }

        public int Add(Log log)
        {
            try
            {
                Db.PositionLogs.Add(log);
                return Db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
