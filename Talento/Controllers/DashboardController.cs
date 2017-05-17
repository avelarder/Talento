using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Talento.Models;
using Talento.Core;
using Talento.Entities;
using Talento.Core.Utilities;

namespace Talento.Controllers
{
    [HandleError]
    [Authorize]
    public class DashboardController : Controller
    {
        ICustomPagingList DashboardPagingHelper;
        IUtilityApplicationSettings ApplicationSettings;

        public DashboardController(ICustomPagingList dashboardPagingHelper, IUtilityApplicationSettings appSettings)
        {
            DashboardPagingHelper = dashboardPagingHelper;
            ApplicationSettings = appSettings;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Position, PositionModel>()
                    .ForMember(t => t.ApplicationUser_Id, opt => opt.MapFrom(s => s.ApplicationUser_Id));
            });
        }

        // GET: Dashboard
        public ActionResult Index(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page = 1)
        {
            string Dashboard = "_PartialContent.cshtml";

            string test = ModelState.IsValid.ToString();

            List<Position> rawData = new List<Position>();

            if (User.IsInRole("Admin"))
            {
                Dashboard = "_PartialContentAdmin.cshtml";
                rawData = DashboardPagingHelper.GetAdminTable(sortOrder, FilterBy, currentFilter, searchString, page);

            }
            if (!User.IsInRole("Admin"))
            {
                rawData = DashboardPagingHelper.GetBasicTable(sortOrder, FilterBy, currentFilter, searchString, page);
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_asc" : ""; //Default date descending
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";
            ViewBag.OwnerSortParm = sortOrder == "Owner" ? "owner_desc" : "Owner";
            ViewBag.EMSortParm = sortOrder == "EM" ? "em_desc" : "EM";
            ViewBag.CurrentFilter = searchString;

            ViewData["Dashboard"] = Dashboard;

            var temp = AutoMapper.Mapper.Map<List<PositionModel>>(rawData.ToList());

            foreach (PositionModel item in temp)
            {
                switch (item.Status)
                {
                    case PositionStatus.Open:
                        item.OpenDays = (DateTime.Now - item.LastOpenedDate).Days;
                        break;
                    case PositionStatus.Closed:
                        item.OpenDays = (item.LastClosedDate - item.LastOpenedDate).Days;
                        break;
                    case PositionStatus.Cancelled:
                        item.OpenDays = (item.LastCancelledDate - item.LastOpenedDate).Days;
                        break;
                }
            }

            // Pagination
            var pageSizeValue = ApplicationSettings.GetSetting("pagination", "pagesize"); // Setting Parameter
            int pageSize = 0;

            if (pageSizeValue != null)
            {
                pageSize = Convert.ToInt32(pageSizeValue);
            }
            else {
                pageSize = 25;
            }

            return View(new DashBoardViewModel()
            {
                Positions = new PositionsPagedList(temp, page.Value, pageSize)
            });
        }

        // Dashboard Partials
        [ChildActionOnly]
        public ActionResult TopNavigation()
        {
            string role = GetRole();
            ViewData["Role"] = role;
            ViewData["RoleClass"] = role + "-role";

            return PartialView("~/Views/Shared/Dashboard/_PartialTopNavigation.cshtml");
        }

        [ChildActionOnly]
        public ActionResult SideNavigation()
        {
            string role = GetRole();
            ViewData["Role"] = role;
            ViewData["RoleClass"] = role + "-role";

            return PartialView("~/Views/Shared/Dashboard/_PartialSidebarNavigation.cshtml");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ManageUser()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AppSettings()
        {
            return View();
        }

        // Helpers
        private string GetRole()
        {
            string role = "basic";
            if( Roles.IsUserInRole("Admin"))
            {
                role = "admin";
            }
            return role;
        }

        public ActionResult AddSettingsForm()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View("~/Shared/Error.cshtml");
        }

        [Authorize]
        [HttpGet]
        public FileResult DownloadTiffTemplate()
        {
            FileResult aux = new FilePathResult("~/Content/Files/Template_TIFF.doc", "application/msword");
            aux.FileDownloadName = "TiffTemplate.doc";
            return aux;
        }

        [Authorize]
        [HttpGet]
        public FileResult DownloadXl(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page = 1)
        {
            List<Position> ListToExport = DashboardPagingHelper.GetBasicTable(sortOrder, FilterBy, currentFilter, searchString, page);
            FileResult aux = new FilePathResult(DashboardPagingHelper.CreateXl(ListToExport), "application/msexcel");
            aux.FileDownloadName = "OpenPositions.xls";
            return aux;
        }

    }
}