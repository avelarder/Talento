using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;
using System.Data;
using System.Data.Entity;
using Talento.Core.Data;

namespace Talento.Core.Helpers
{
    public class PositionHelper : BaseHelper, IPosition
    {
        IPositionLog PositionLoghelper;
        public PositionHelper(Core.Data.ApplicationDbContext db, IPositionLog positionLoghelper) : base(db)
        {
            PositionLoghelper = positionLoghelper;

        }

        public void Create(Position log)
        {
            Db.Positions.Add(log);
            Db.SaveChanges();

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

        public bool Edit(Position log, string EmailModifier)
        {
            try
            {
                //Obtaining the position in its original state
                Position position = Db.Positions.Single(p => p.Id == log.Id);
                //And obtaining the user that is modifying the Position
                ApplicationUser User = Db.Users.Single(u => u.Email.Equals(EmailModifier));
                var previousStatus = position.Status;
                //Modifying the position info from the edit form
                position.Area = log.Area;
                position.Status = log.Status;
                position.Title = log.Title;
                position.Description = log.Description;
                position.EngagementManager = log.EngagementManager;
                position.RGS = log.RGS;
                position.ApplicationUser_Id = position.ApplicationUser_Id;
                position.PortfolioManager_Id = position.PortfolioManager_Id;
                position.Owner = position.Owner;
                position.PortfolioManager = position.PortfolioManager;

                Db.SaveChanges();
                //I create the log containing the pertinent information
                //PositionLog CreateLog = new PositionLog()
                //{
                //    Action = Entities.Action.Edit,
                //    ActualStatus = log.Status,
                //    PreviousStatus = previousStatus,
                //    Date = DateTime.Now,
                //    ApplicationUser_Id = User.Id,
                //    Position_Id = position.Id,
                //    Position = position,
                //    User = User,

                //};
                //PositionLoghelper.Create(CreateLog);
              
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
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
