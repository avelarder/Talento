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
            string Role = "basic";

            List<Position> rawData;
            if (Roles.IsUserInRole("Admin"))
            {
                Dashboard = "_PartialContentAdmin.cshtml";
                Role = "admin";
                rawData = DashboardPagingHelper.GetAdminTable(sortOrder, FilterBy, currentFilter, searchString, page);

            }
            else
            {
                rawData = DashboardPagingHelper.GetBasicTable(sortOrder, FilterBy, currentFilter, searchString, page);
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : ""; //Default date ascending
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.IdSortParm = sortOrder == "Id" ? "id_desc" : "Id";
            ViewBag.CurrentFilter = searchString;

            ViewData["RoleClass"] = Role + "-role";
            ViewData["Dashboard"] = Dashboard;
            ViewData["Role"] = Role;

            var temp = AutoMapper.Mapper.Map<List<PositionModel>>(rawData.ToList());

            return View(new DashBoardViewModel()
            {
                Positions = new PositionsPagedList(temp, page.Value, 25)
            });
        }
    }
}