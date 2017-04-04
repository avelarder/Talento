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
        ITag Tags;
        public PositionHelper(Core.Data.ApplicationDbContext db, ITag tags) : base(db)
        {
            Tags = tags;
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

        public async Task<Position> Get(int Id)
        {
            var position = await Db.Positions.SingleAsync(x => x.Id == Id);
            //position.Tags = Tags.GetByPositionId(position.Id);

            return position;
        }

        public async Task<List<Position>> GetAll()
        {
            return await Db.Positions.ToListAsync();
        }
    }
}
