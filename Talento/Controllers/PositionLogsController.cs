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
        public ActionResult List(int? id, int page = 1, int pagesize = 5 )
        {
            try
            {
                if (!Request.IsLocal)
                {
                    throw new SecurityException();
                }
                if ( id == null )
                {
                    return HttpNotFound();
                }
                string url = Url.Action("List", "PositionLogs");
                var containerLogs = LogHelper.PaginateLogs(id, page, pagesize, url);
                var logs = AutoMapper.Mapper.Map<List<PositionLogViewModel>>(containerLogs.Item1);
                var pagination = containerLogs.Item2;

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
