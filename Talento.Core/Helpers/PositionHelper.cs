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

        public Task Create(Position position)
        {
            // Create log on Creation
            PositionLog log = new PositionLog()
            {
                Date = DateTime.Today,
                User = new ApplicationUser(), // Modify to get current User
                Position = position,
                Action = Entities.Action.Create,
                PreviousStatus = Status.Closed,
                ActualStatus = Status.Open
            };

            throw new NotImplementedException();
        }

        public Task Delete(int Id)
        {
            // Create log on Delete
            Position position = (Position)Db.Positions.Where(p => p.Id == Id);
            PositionLog log = new PositionLog()
            {
                Date = DateTime.Today,
                User = new ApplicationUser(), // Modify to get current User
                Position = position,
                Action = Entities.Action.Delete,
                PreviousStatus = position.Status,
                ActualStatus = Status.Removed
            };
            throw new NotImplementedException();
        }

        public Task Edit(Position position)
        {
            // Create log on Edit
            PositionLog log = new PositionLog()
            {
                Date = DateTime.Today,
                User = new ApplicationUser(), // Modify to get current User
                Position = position,
                Action = Entities.Action.Edit,
                PreviousStatus = position.Status,
                ActualStatus = Status.Open
            };
            throw new NotImplementedException();
        }

        public async Task<Position> Get(int Id)
        {
            try
            {
                var position = await Db.Positions.SingleAsync(x => x.Id == Id);
                return position;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Position>> GetAll()
        {
            return await Db.Positions.ToListAsync();
        }
    }
}
