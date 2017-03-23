using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Talento.Controllers
{
    //[Authorize(Roles = "Basic")]
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            string Dashboard = "_PartialContent.cshtml";
            string Role = "basic";

            if (Roles.IsUserInRole("Admin"))
            {
                Dashboard = "_PartialContentAdmin.cshtml";
                Role = "admin";
            }
            ViewData["RoleClass"] = Role + "-role";
            ViewData["Dashboard"] = Dashboard;
            ViewData["Role"] = Role;
            return View();
        }
    }
}