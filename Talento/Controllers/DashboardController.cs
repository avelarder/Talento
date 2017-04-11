﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Talento.Models;
using PagedList;
using Talento.Core.Data;
using Talento.Core;
using Talento.Entities;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Talento.Core.Helpers;

namespace Talento.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        ICustomPagingList DashboardPagingHelper;

        public DashboardController(ICustomPagingList dashboardPagingHelper)
        {
            DashboardPagingHelper = dashboardPagingHelper;

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

            List<Position> rawData;
            if (Roles.IsUserInRole("Admin"))
            {
                Dashboard = "_PartialContentAdmin.cshtml";
                rawData = DashboardPagingHelper.GetAdminTable(sortOrder, FilterBy, currentFilter, searchString, page);

            }
            else
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
                    case Status.Open:
                        item.OpenDays = (DateTime.Now - item.LastOpenedDate).Days;
                        break;
                    case Status.Closed:
                        item.OpenDays = (item.LastClosedDate - item.LastOpenedDate).Days;
                        break;
                    case Status.Canceled:
                        item.OpenDays = (item.LastCancelledDate - item.LastOpenedDate).Days;
                        break;
                }
            }

            return View(new DashBoardViewModel()
            {
                Positions = new PositionsPagedList(temp, page.Value, 25)
            });
        }

        // Dashboard Partials
        public ActionResult TopNavigation()
        {
            string role = GetRole();
            ViewData["Role"] = role;
            ViewData["RoleClass"] = role + "-role";

            return PartialView("~/Views/Shared/Dashboard/_PartialTopNavigation.cshtml");
        }

        public ActionResult SideNavigation()
        {
            string role = GetRole();
            ViewData["Role"] = role;
            ViewData["RoleClass"] = role + "-role";

            return PartialView("~/Views/Shared/Dashboard/_PartialSidebarNavigation.cshtml");
        }
        
        // Helpers
        public string GetRole()
        {
            string role = "basic";
            if( Roles.IsUserInRole("Admin"))
            {
                role = "admin";
            }
            return role;
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
    }
}