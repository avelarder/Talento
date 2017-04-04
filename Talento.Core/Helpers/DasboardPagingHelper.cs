using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class DashboardPagingHelper : BaseHelper, ICustomPagingList
    {
        public DashboardPagingHelper(Talento.Core.Data.ApplicationDbContext _db) : base(_db)
        {
        }

        public List<Position> GetAdminTable(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page)
        {
            ////Setting headings parameters and their probable values
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : ""; //Default date ascending
            //ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            //ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            //ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";

            //Keeping paging and sorting
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //ViewBag.CurrentFilter = searchString;

            //Linq query that lists the positions
            var query = from p in Db.Positions
                        select p;

            //Filtering the positions by the parameters given
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
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
                default:  // Date descending 
                    query = query.OrderByDescending(p => p.CreationDate);
                    break;
            }

            //Sending the query to the list (25 positions per page)
            int pageNumber = (page ?? 1);
            return query.ToList();
        }

        public List<Position> GetBasicTable(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page)
        {
            ////Setting headings parameters and their probable values
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_asc" : ""; //Default date ascending
            //ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            //ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            //ViewBag.EMSortParm = sortOrder == "EM" ? "em_desc" : "EM";
            //ViewBag.OwnerSortParm = sortOrder == "Owner" ? "owner_desc" : "Owner";

            //Keeping paging and sorting
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //ViewBag.CurrentFilter = searchString;

            //Linq query that lists the positions
            var query = from p in Db.Positions
                        where p.Status != Status.Removed
                        select p;

            //Filtering the positions by the parameters given
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
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
                    case "PM":
                        query = query.Where(p => p.PortfolioManager.UserName.Contains(searchString));
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
            int pageNumber = (page ?? 1);
            return query.ToList();
        }

        public List<Position> GetByWidget(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Setting headings parameters and their probable values
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : ""; //Default date ascending
            //ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            //ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            //ViewBag.EMSortParm = sortOrder == "EM" ? "em_desc" : "EM";

            //Keeping paging and sorting
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //ViewBag/*.*/CurrentFilter = searchString;

            //Linq query that lists the positions
            var query = from p in Db.Positions
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
            return query.ToList();
        }
    }
}
