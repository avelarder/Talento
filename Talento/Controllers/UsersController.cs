using System;
using System.Collections.Generic;
using System.Linq;
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

        public UsersController(ICustomUser userhelper)
        {
            UserHelper = userhelper;
        }

        // GET: Users
        [Authorize]
        public ActionResult UsersTable()
        {
            List<Entities.ApplicationUser> users = new List<Entities.ApplicationUser>();
            users.Add(UserHelper.GetUserByEmail(User.Identity.Name));
            users.Add(UserHelper.GetUserByEmail(User.Identity.Name));
            users.Add(UserHelper.GetUserByEmail(User.Identity.Name));
            users.Add(UserHelper.GetUserByEmail(User.Identity.Name));
            users.Add(UserHelper.GetUserByEmail(User.Identity.Name));
            return View(new Models.UsersTableViewModel() { users = users});
        }
    }
}