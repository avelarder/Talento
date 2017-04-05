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
        IPositionLog PositionLoghelper;
        public PositionHelper(Core.Data.ApplicationDbContext db, IPositionLog positionLoghelper) : base(db)
        {
            PositionLoghelper = positionLoghelper;
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
            Position position = (Position) Db.Positions.Where(p => p.Id == Id);
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

        public void Edit(Position log, string EmailModifier)
        {
            try
            {
                //Obtaining the position in its original state
                Position position = Db.Positions.Single(p => p.Id == log.Id);
                //And obtaining the user that is modifying the Position
                ApplicationUser User = Db.Users.Single(u => u.Email.Equals(EmailModifier));

                //Modifying the position info from the edit form
                position.Area = log.Area;
                position.Status = log.Status;
                position.Title = log.Title;
                position.Description = log.Description;
                position.EngagementManager = log.EngagementManager;
                position.RGS = log.RGS;

                Db.SaveChanges();
                //I create the log containing the pertinent information
                PositionLog CreateLog = new PositionLog()
                {
                    Action = Entities.Action.Edit,
                    ActualStatus = log.Status,
                    PreviousStatus = position.Status,
                    Date = DateTime.Now,
                    ApplicationUser_Id = User.Id,
                    Position_Id = position.Id,
                    Position = position,
                    User = User,

                };
                PositionLoghelper.Create(CreateLog);
                /*
                Db.PositionLogs.Add(CreateLog);
                Db.SaveChanges();
                    */
            }
            catch (Exception e)
            {
                System.Threading.Thread.Sleep(1000);
            }
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
