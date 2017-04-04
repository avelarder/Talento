using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;
using System.Data;
using System.Data.Entity;

namespace Talento.Core.Helpers
{
    public class PositionLogHelper : BaseHelper, IPositionLog
    {
        public PositionLogHelper(Data.ApplicationDbContext db) : base(db)
        {

        }

        public void Create(PositionLog log)
        {
            try
            {
                Db.PositionLogs.Add(log);
                Db.SaveChangesAsync();

            } catch ( Exception )
            {
                throw new Exception();
            }
        }

        public List<PositionLog> GetAll(int Id)
        {
            return Db.PositionLogs
                .Where(p => p.Position_Id == Id)
                .OrderByDescending(p => p.Date)
                .ToList();
        }
    }
}
