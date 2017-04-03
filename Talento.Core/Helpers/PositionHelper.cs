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
    public class PositionHelper : BaseHelper, IPosition
    {
        public PositionHelper(Core.Data.ApplicationDbContext db) : base(db)
        {

        }

        public Task Create(Position log)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Task Edit(Position log)
        {
            throw new NotImplementedException();
        }

        public Task<Position> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Position>> GetAll()
        {
            return await Db.Positions.ToListAsync();
        }
    }
}
