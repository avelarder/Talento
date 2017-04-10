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

        public void Create(Position position, string EmailModifier)
        {
            ApplicationUser User = Db.Users.Single(u => u.Email.Equals(EmailModifier));

            Db.Positions.Add(position);
            Db.SaveChanges();

            PositionLog CreateLog = new PositionLog()
            {
                Action = Entities.Action.Create,
                ActualStatus = position.Status,
                PreviousStatus = 0,
                Date = DateTime.Now,
                ApplicationUser_Id = User.Id,
                Position_Id = position.Id,
                Position = position,
                User = User,

            };
            PositionLoghelper.Create(CreateLog);

        }

        public void Delete(int Id, string uId)
        {
            ApplicationUser cu = Db.Users.Single(u => u.Id.Equals(uId)); //Get Current User
            var p = Db.Positions.Where(x => x.Id == Id).Single();
            PositionLog log = new PositionLog()
            {
                Date = DateTime.Now,
                User = cu,
                Position = p,
                Action = Entities.Action.Delete,
                PreviousStatus = p.Status,
                ActualStatus = Status.Removed,
                ApplicationUser_Id = cu.Id,
                Position_Id = p.Id,
            };
            PositionLoghelper.Create(log);
            p.Status = Status.Removed;
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

                switch (position.Status = log.Status)
                {
                    case Status.Canceled:
                        position.LastCancelledDate = DateTime.Now;
                        position.LastCancelledBy = User;
                        break;
                    case Status.Open:
                        position.LastOpenedDate = DateTime.Now;
                        position.LastOpenedBy = User;
                        break;
                    case Status.Closed:
                        position.LastClosedDate = DateTime.Now;
                        position.LastClosedBy = User;
                        break;

                }

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
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Position Get(int Id)
        {
            var position = Db.Positions.Single(x => x.Id == Id);
            //position.Tags = Tags.GetByPositionId(position.Id);

            return position;
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
