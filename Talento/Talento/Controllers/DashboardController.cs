using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Talento.Models;
using PagedList;

namespace Talento.Controllers
{
    //[Authorize(Roles = "Basic")]
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page)
        {
            string Dashboard = "_PartialContent.cshtml";
            string Role = "basic";

            if (Roles.IsUserInRole("Admin"))
            {
                Dashboard = "_PartialContentAdmin.cshtml";
                Role = "admin";
                IPagedList<Position> Model = AdminTable(sortOrder, FilterBy, currentFilter, searchString, page);
                ViewData["RoleClass"] = Role + "-role";
                ViewData["Dashboard"] = Dashboard;
                ViewData["Role"] = Role;
                return View(Model);
            }
            else
            {
                IPagedList<Position> Model = BasicTable(sortOrder, FilterBy, currentFilter, searchString, page);
                ViewData["RoleClass"] = Role + "-role";
                ViewData["Dashboard"] = Dashboard;
                ViewData["Role"] = Role;
                return View(Model);
            }
        }

        public IPagedList<Position> AdminTable(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page)
        {

            //Setting headings parameters and their probable values
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_asc" : ""; //Default date ascending
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.EMSortParm = sortOrder == "EM" ? "em_desc" : "EM";
            ViewBag.OwnerSortParm = sortOrder == "Owner" ? "owner_desc" : "Owner";

            //Keeping paging and sorting
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //Linq query that lists the positions
            var query = from p in db.Positions
                        select p;

            //Filtering the positions by the parameters given
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (FilterBy)
                {
                    case "Status":
                        query = query.Where(p => p.Status.ToString().Contains(searchString));
                        break;
                    case "Title":
                        query = query.Where(p => p.Title.Contains(searchString));
                        break;
                    case "Owner":
                        query = query.Where(p => p.Owner.UserName.ToString().Contains(searchString));
                        break;
                    default:
                        break;
                }
            }

            //Sorting the list by the heading parameter given
            switch (sortOrder)
            {
                case "title_desc":
                    query = query.OrderByDescending(p => p.Title);
                    break;
                case "Title":
                    query = query.OrderBy(p => p.Title);
                    break;
                case "date_asc":
                    query = query.OrderBy(p => p.CreationDate);
                    break;
                case "Status":
                    query = query.OrderBy(p => p.Status);
                    break;
                case "status_desc":
                    query = query.OrderByDescending(p => p.Status);
                    break;
                case "EM":
                    query = query.OrderBy(p => p.EngagementManager);
                    break;
                case "em_desc":
                    query = query.OrderByDescending(p => p.EngagementManager);
                    break;
                case "Owner":
                    query = query.OrderBy(p => p.Owner.UserName);
                    break;
                case "owner_desc":
                    query = query.OrderByDescending(p => p.Owner.UserName);
                    break;

                default:  // Date ascending 
                    query = query.OrderBy(p => p.CreationDate);
                    break;
            }

            //Sending the query to the list (25 positions per page)
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return query.ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<Position> BasicTable(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page)
        {
            //Setting headings parameters and their probable values
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : ""; //Default date ascending
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";

            //Keeping paging and sorting
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //Linq query that lists the positions
            var query = from p in db.Positions
                        where p.Status != Status.Removed
                        select p;

            //Filtering the positions by the parameters given
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (FilterBy)
                {
                    case "Status":
                        query = query.Where(p => p.Status.ToString().Contains(searchString));
                        break;
                    case "Title":
                        query = query.Where(p => p.Title.Contains(searchString));
                        break;
                    case "Owner":
                        query = query.Where(p => p.Owner.UserName.ToString().Contains(searchString));
                        break;
                    case "EM":
                        query = query.Where(p => p.EngagementManager.Contains(searchString));
                        break;
                    default:
                        break;
                }
            }

            //Sorting the list by the heading parameter given
            switch (sortOrder)
            {
                case "title_desc":
                    query = query.OrderByDescending(p => p.Title);
                    break;
                case "Title":
                    query = query.OrderBy(p => p.Title);
                    break;
                case "date_desc":
                    query = query.OrderByDescending(p => p.CreationDate);
                    break;
                case "Status":
                    query = query.OrderBy(p => p.Status);
                    break;
                case "status_desc":
                    query = query.OrderByDescending(p => p.Status);
                    break;
                case "Id":
                    query = query.OrderBy(p => p.Id);
                    break;
                case "id_desc":
                    query = query.OrderByDescending(p => p.Id);
                    break;
                //case "EM":
                //    query = query.OrderBy(p => p.EngagementManager);
                //    break;
                //case "em_desc":
                //    query = query.OrderByDescending(p => p.EngagementManager);
                //    break;
                //case "Owner":
                //    query = query.OrderBy(p => p.Owner.UserName);
                //    break;
                //case "owner_desc":
                //    query = query.OrderByDescending(p => p.Owner.UserName);
                //    break;
                default:  // Date descending 
                    query = query.OrderByDescending(p => p.CreationDate);
                    break;
            }

            //Sending the query to the list (25 positions per page)
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return query.ToPagedList(pageNumber, pageSize);
        }
    }
}