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
        public PositionLogHelper(Core.Data.ApplicationDbContext db) : base(db)
        {

        }

        public void Create(PositionLog log)
        {
            Db.PositionLogs.Add(log);
            Db.SaveChanges();
        }

        public Task Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Task Edit(PositionLog log)
        {
            throw new NotImplementedException();
        }

        public Task<PositionLog> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PositionLog>> GetAll()
        {
            return await Db.PositionLogs.ToListAsync();
        }
    }
}
