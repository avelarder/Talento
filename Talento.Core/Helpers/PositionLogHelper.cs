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
                Db.SaveChanges();

            } catch ( Exception e)
            {
                throw new Exception(e.Message);
            }
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

        public List<PositionLog> GetAll(int? Id)
        {
            try { 
                List<PositionLog> log = Db.PositionLogs
                    .Where(p => p.Position_Id == Id)
                    .OrderByDescending(p => p.Date)
                    .ToList();
                return log;

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
                
        }
        
    }
}
