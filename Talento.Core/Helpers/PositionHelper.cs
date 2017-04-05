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
        public PositionHelper(Core.Data.ApplicationDbContext db) : base(db)
        {

        }

        public Task Create(Position log)
        {
            Db.Positions.Add(log);
            Db.SaveChanges();

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
                PositionLog CreateLog = new PositionLog()
                {
                    Action = Entities.Action.Edit,
                    ActualStatus = log.Status,
                    PreviousStatus = previousStatus,
                    Date = DateTime.Now,
                    ApplicationUser_Id = User.Id,
                    Position_Id = position.Id,
                    Position = position,
                    User = User,

                };
                PositionLoghelper.Create(CreateLog);
              
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

        public ApplicationUser SearchPM(string userName)
        {

            var PM = Db.Roles.Single(r => r.Name == "PM");
            if (userName != null)
            {
                var usuario = Db.Users.Single(x => x.UserName == userName);
                if (usuario.Roles.Where(x => x.RoleId == PM.Id).Count() > 0)
                {
                    return usuario;
                }
            }

            return null;
        }

        public ApplicationUser GetUser(string user)
        {
           return Db.Users.Single(x => x.Id == user.ToString());
        }

    }
}
