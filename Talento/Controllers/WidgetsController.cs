using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talento.Models;
using PagedList;
using Talento.Entities;

namespace Talento.Controllers
{
    [Authorize]
    public class WidgetsController : Controller
    {
        Core.ICustomPagingList DasboardHelper;

        public WidgetsController(Core.ICustomPagingList dasboardHelper)
        {
            DasboardHelper = dasboardHelper;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<IPagedList<Position>, IPagedList<PositionModel>>();
            });
        }

        // GET: Widget
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Setting headings parameters and their probable values
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : ""; //Default date ascending
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";
            ViewBag.EMSortParm = sortOrder == "EM" ? "em_desc" : "EM";
            ViewBag.CurrentFilter = searchString;

            var rawData = DasboardHelper.GetByWidget(sortOrder, currentFilter, searchString, page);
            IPagedList<Models.PositionModel> model = AutoMapper.Mapper.Map<IPagedList<Models.PositionModel>>(rawData);
            return View();
        }


        [ChildActionOnly]
        public ActionResult DataTable(IList Data)
        {
            ViewBag.Data = Data;
            return View();
        }
    }
}