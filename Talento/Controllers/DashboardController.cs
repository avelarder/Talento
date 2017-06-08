using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Talento.Models;
using Talento.Core;
using Talento.Entities;
using Talento.Core.Utilities;
using System.Web.Helpers;

namespace Talento.Controllers
{
    [HandleError]
    public class DashboardController : Controller
    {
        ICustomPagingList DashboardPagingHelper;
        IUtilityApplicationSettings ApplicationSettings;
        ICustomUser UserHelper;

        public DashboardController(ICustomPagingList dashboardPagingHelper, IUtilityApplicationSettings appSettings, ICustomUser userHelper)
        {
            DashboardPagingHelper = dashboardPagingHelper;
            ApplicationSettings = appSettings;
            UserHelper = userHelper;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Position, PositionModel>()
                    .ForMember(t => t.ApplicationUser_Id, opt => opt.MapFrom(s => s.ApplicationUser_Id));
            });
        }

        [Authorize]
        // GET: Dashboard
        public ActionResult Index(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page = 1)
        {
            string Dashboard = "_PartialContent.cshtml";
            List<Position> rawData = DashboardPagingHelper.GetTable(sortOrder, FilterBy, currentFilter, searchString, page);
            if (User.IsInRole("Admin"))
            {
                Dashboard = "_PartialContentAdmin.cshtml";                
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_asc" : ""; //Default date descending
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";
            ViewBag.OwnerSortParm = sortOrder == "Owner" ? "owner_desc" : "Owner";
            ViewBag.EMSortParm = sortOrder == "EM" ? "em_desc" : "EM";
            ViewBag.RGSSortParm = sortOrder == "RGS" ? "rgs_desc" : "RGS";
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
            int pageSize;

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

        //[Authorize]
        // Dashboard Partials
        [ChildActionOnly]
        public ActionResult TopNavigation()
        {
            if (User.Identity.Name != "")
            {

            string role = GetRole();
            ViewData["Role"] = role;
            ViewData["RoleClass"] = role + "-role";
            ViewData["Image"] = UserHelper.GetUserByEmail(User.Identity.Name).ImageProfile;

            return PartialView("~/Views/Shared/Dashboard/_PartialTopNavigation.cshtml");
            }
            else
            {
                return null;
            }
        }

        //[Authorize]
        [ChildActionOnly]
        public ActionResult SideNavigation()
        {
            if (User.Identity.Name != "")
            {
                string role = GetRole();
                ViewData["Role"] = role;
                ViewData["RoleClass"] = role + "-role";

                //First I get the roles that are allowed by the settings
                List<string> roles = ApplicationSettings.GetAllSettings()
                    .Where(x => x.SettingName.Trim().Equals("AllowUsersActivation") && x.ParameterValue.Trim().Equals("Enabled"))
                    .Select(y => y.ParameterName.Substring(5)).ToList();
                //Then I get the role of the currently logged user
                string loggedRole = UserHelper.GetRoleName(UserHelper.GetUserByEmail(User.Identity.Name).Roles.First().RoleId);
                ViewData["UserTableVisible"] = roles.Contains(loggedRole);

                return PartialView("~/Views/Shared/Dashboard/_PartialSidebarNavigation.cshtml");
            }
            else
            {
                return null;
            }
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

        [Authorize]
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

        [Authorize]
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
        public FileResult DownloadXl(string TableSortOrder, string TableFilterBy, string TableSearchString)
        {
            FileResult aux = new FilePathResult(DashboardPagingHelper.CreateXML(TableSortOrder, TableFilterBy, null, TableSearchString, null), "application/msexcel");
            aux.FileDownloadName = "OpenPositions.xlsx";
            return aux;
        }

        [AllowAnonymous]
        public ActionResult AboutUs() {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult SoftwareTimeline()
        {
            return PartialView();
        }

    }
}