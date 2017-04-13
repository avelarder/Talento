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

        public List<ApplicationUser> GetUsersForNewProfileMail()
        {
           
            List<ApplicationUser> recipients = Db.Users.Where(x => x.Roles.Any(p => p.RoleId == "62bd6c0f-cbd2-49f1-ab59-dd5034ea8341"/*TL*/) || x.Roles.Any(p => p.RoleId == "95df772e-9444-4ff3-89a8-d2db9d2cfe66"/*RMG*/) || x.Roles.Any(p => p.RoleId == "65ceda4f-8de4-4b2b-b6ab-2d4934934e9c"/*TAG*/)).ToList();

            return recipients;

        }

        public List<ApplicationUser> GetUsersForNewFeedbackMail()
        {
            List<ApplicationUser> recipients = Db.Users.Where(x => x.Roles.Any(p => p.RoleId == "95df772e-9444-4ff3-89a8-d2db9d2cfe66"/*RMG*/) || x.Roles.Any(p => p.RoleId == "65ceda4f-8de4-4b2b-b6ab-2d4934934e9c"/*TAG*/)).ToList();

            return recipients;
        }
    }
}
