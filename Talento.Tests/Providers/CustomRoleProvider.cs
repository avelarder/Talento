using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Talento.Core.Data;

namespace Talento.Tests.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public CustomRoleProvider()
        {
           
        }

        public override string ApplicationName { get ; set ; }

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
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            //var userId = DbContext.Users.SingleOrDefault(x => x.UserName == username).Id;

            //return (from r in DbContext.Roles
            //           where r.Users.Any(x => x.UserId == userId)
            //           select r.Name).ToArray();
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (roleName == "Admin" || roleName == "Basic")
                return true;
            else
                return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}