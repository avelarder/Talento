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

        public Task Create(Position log)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int Id)
        {
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

        public Task<Position> Get(int Id)
        {
            var query = from p in Db.Positions
                        select p;
            return query.SingleAsync(p => p.Id == Id);
        }

        public async Task<List<Position>> GetAll()
        {
            return await Db.Positions.ToListAsync();
        }
    }
}
