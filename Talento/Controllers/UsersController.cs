using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Talento.Core;

namespace Talento.Controllers
{
    public class UsersController : Controller
    {
        ICustomUser UserHelper;


        public UsersController()
        {
            
        }
        public UsersController(ICustomUser userHelper)
        {
            
            UserHelper = userHelper;
        }

        [Authorize]
        public ActionResult UsersTable()
        {
            List<Entities.ApplicationUser> users = UserHelper.GetPendingUsers();
            Models.UsersTableViewModel aux = new Models.UsersTableViewModel() { users = new List<Models.ApplicationUserViewModel>()};
            users.ForEach(x => aux.users.Add(new Models.ApplicationUserViewModel() {
                createdon = x.CreatedDate,
                email = x.Email,
                id = x.Id,
                name = x.UserName,
                role = UserHelper.GetRoleName(x.Roles.First().RoleId)
            }));
            return View(aux);
        }

        //
        // GET: /Account/ConfirmEmail
        //TODO: Refactor this to be used by the admin in order to activate an account
        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userId)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            string code = UserManager.GenerateEmailConfirmationToken(userId);
            if (userId == null)
            {
                ModelState.AddModelError("", "The operation you are trying to execute is not valid.");
            }

            if (UserManager.IsEmailConfirmed(userId))
            {
                ModelState.AddModelError("", "Account is activated already.");
            }

            var result = UserManager.ConfirmEmail(userId, code);
            if (result.Succeeded)
            {
                ModelState.AddModelError("", "Your Account has been activated successfully.");
            }
            else
            {
                ModelState.AddModelError("", "The operation you are trying to execute is not valid.");
            }
            return RedirectToAction("UsersTable", "Users", null);
        }

        //
        // GET: /Account/RejectEmail
        //TODO: Refactor this to be used by the admin in order to reject an account
        [AllowAnonymous]
        public async Task<ActionResult> RejectEmail(string userId)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (userId == null)
            {
                ModelState.AddModelError("", "The operation you are trying to execute is not valid.");
            }

            if (UserManager.IsEmailConfirmed(userId))
            {
                ModelState.AddModelError("", "Account is activated already.");
            }

            var user = await UserManager.FindByIdAsync(userId);
            var logins = user.Logins;
            var rolesForUser = await UserManager.GetRolesAsync(userId);

            foreach (var login in logins.ToList())
            {
                await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }

            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser.ToList())
                {
                    await UserManager.RemoveFromRoleAsync(user.Id, item);
                }
            }

            var remove = await UserManager.DeleteAsync(user);

            if (remove.Succeeded)
            {
                ModelState.AddModelError("", "Account has been deleted successfully.");
            }
            else
            {
                ModelState.AddModelError("", "The operation you are trying to execute is not valid.");
            }
            return RedirectToAction("UsersTable", "Users", null);
        }

    }
}