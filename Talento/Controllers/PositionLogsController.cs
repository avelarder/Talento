using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Talento.Models;
using Talento.Core;
using Talento.Entities;
using Talento.Core.Utilities;
using System.Security;

namespace Talento.Controllers
{
    [Authorize(Roles = "PM, TL, TAG, RMG")]
    [HandleError]
    public class PositionLogsController : Controller
    {
        IPositionLog LogHelper;

        public PositionLogsController(IPositionLog logHelper)
        {
            LogHelper = logHelper;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.PositionLog, Models.PositionLogViewModel>();
            });
        }

        // Show: PositionLogs
        public ActionResult List(int? id, int pagex = 1, int pagesize = 5, string clase = "slide-right")
        {
            try
            {
                if (!Request.IsLocal)
                {
                    throw new SecurityException();
                }
                // No ID return 404
                if ( id == null )
                {
                    return HttpNotFound();
                }
                // Check if it's Ajax request, View check for this viewData
                ViewData["AjaxTrue"] = false;
                if (Request.IsAjaxRequest())
                {
                    ViewData["AjaxTrue"] = true;
                }
                // Url for the pagination Helper
                string url = Url.Action("List", "PositionLogs");
                // Get List of PositionLogs and the Pagination
                var containerLogs = LogHelper.PaginateLogs(id, pagex, pagesize, url);
                // No logs with the ID return 404
                if( containerLogs == null)
                {
                    return HttpNotFound();
                }
                // Maps PositionLogs to PositionLogViewModel
                var logs = AutoMapper.Mapper.Map<List<PositionLogViewModel>>(containerLogs.Item1);
                // Pagination
                var pagination = containerLogs.Item2;
                // General ViewData
                ViewData["AnimationClass"] = clase;
                ViewData["Count"] = logs.Count;
                ViewData["Pagination"] = pagination;
                
                return PartialView(logs);
            }
            catch (Exception e)
            {
                return HttpNotFound(e.Message);
            }
        }

        [ChildActionOnly]
        public ActionResult Pagination(Pagination pagination)
        {
            return PartialView(pagination);
        }
    }
}
