using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talento.Models;
using PagedList;

namespace Talento.Controllers
{
    [Authorize]
    public class WidgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Widget
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Setting headings parameters and their probable values
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : ""; //Default date ascending
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.EMSortParm = sortOrder == "EM" ? "em_desc" : "EM";

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

            //Filtering the positions by the parameter given
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Title.Contains(searchString)
                                       || p.Owner.UserName.Contains(searchString)
                                       || p.Status.ToString().Contains(searchString));
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
                case "EM":
                    query = query.OrderBy(p => p.EngagementManager);
                    break;
                case "em_desc":
                    query = query.OrderByDescending(p => p.EngagementManager);
                    break;
                default:  // Date ascending 
                    query = query.OrderBy(p => p.CreationDate);
                    break;
            }

            //Sending the query to the list (25 positions per page)
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageNumber, pageSize));
        }


        [ChildActionOnly]
        public ActionResult DataTable(IList Data)
        {
            ViewBag.Data = Data;
            return View();
        }
    }
}