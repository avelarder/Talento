using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Talento.Core.Data;

namespace Talento.Providers
{
    public class CustomRoleProvider : RoleProvider, IDisposable
    {
        ApplicationDbContext DbContext;

        public CustomRoleProvider()
        {
            DbContext = new ApplicationDbContext();
        }

        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return (from r in DbContext.Roles
                    select r.Name).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            var userId = DbContext.Users.SingleOrDefault(x => x.UserName == username).Id;

            return (from r in DbContext.Roles
                    where r.Users.Any(x => x.UserId == userId)
                    select r.Name).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var user = DbContext.Users.SingleOrDefault(x => x.UserName == username);
            return DbContext.Roles.SingleOrDefault(x => x.Name == roleName).Users.Any(x => x.UserId == user.Id);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
        }
        #endregion
    }
}