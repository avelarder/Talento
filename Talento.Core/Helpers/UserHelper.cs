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
            try
            {
                return Db.Users.Single(x => x.Id == user.ToString());
            }
            catch(Exception e)
            {
                return null;
            }
            
            
        }
    }
}
