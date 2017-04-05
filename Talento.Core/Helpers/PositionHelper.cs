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
        //public ApplicationDbContext Dba; // Db = new ApplicationDbContext();

        public PositionHelper(Core.Data.ApplicationDbContext db) : base(db)
        {
            Db = db;
        }

        public void Create(Position log)
        {
            Db.Positions.Add(log);
            Db.SaveChanges();

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
