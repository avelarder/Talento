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
        [ChildActionOnly]
        public ActionResult List(int? Id)
        {
            try
            {
                if( Id == null )
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var logs = AutoMapper.Mapper.Map<List<PositionLogViewModel>>(LogHelper.GetAll(Id).ToList());
                ViewData["Count"] = logs.Count;

                return View(logs);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
    }
}
