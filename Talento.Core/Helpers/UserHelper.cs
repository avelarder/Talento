using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class UserHelper : BaseHelper, ICustomUser
    {
        public UserHelper(Core.Data.ApplicationDbContext db): base(db)
        {

        }
        
        public ApplicationUser SearchPM(string userName)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            try
            {
                return Db.Users.SingleOrDefault(user => user.Email.Contains(email));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ApplicationUser GetUserById(string id)
        {
            try
            {
                return Db.Users.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ApplicationUser> GetByRoles(List<string> roles)
        {
            try
            {
                List<string> matchingUserIds = new List<string>();
                roles.ForEach(rol => matchingUserIds.AddRange(Db.Roles.SingleOrDefault(r => r.Name.Equals(rol)).Users.Select(u=>u.UserId)));
                List<ApplicationUser> matchingUsers = new List<ApplicationUser>();
                matchingUserIds.ForEach(id => matchingUsers.Add(GetUserById(id)));

                return matchingUsers;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
