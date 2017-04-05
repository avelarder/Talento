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

        public void Delete(int Id, string uId)
        {
            //Search for the position
            var query = from p in Db.Positions
                        where p.Id == Id
                        select p;

            foreach (Position p in query)
            {
                // Create log on Delete
                PositionLog log = new PositionLog()
                {
                    Date = DateTime.Today,
                    User = Db.Users.Single(u=>u.Id.Equals(uId)), //Get Current User
                    Position = p,
                    Action = Entities.Action.Delete,
                    PreviousStatus = p.Status,
                    ActualStatus = Status.Removed
                };
                p.Status = Status.Removed;
            }
            Db.SaveChanges();
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
