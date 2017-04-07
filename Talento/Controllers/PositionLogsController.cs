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
                    return HttpNotFound();
                }

                var logs = AutoMapper.Mapper.Map<List<PositionLogViewModel>>(LogHelper.GetAll(Id).ToList());
                ViewData["Count"] = logs.Count;

                return PartialView(logs);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        // GET: PositionLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PositionLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PositionLogViewModel positionLog)
        {
            if (ModelState.IsValid)
            {
                LogHelper.Create(AutoMapper.Mapper.Map<PositionLog>(positionLog));
                return RedirectToAction("Index");
            }

            return View(positionLog);
        }

        // GET: PositionLogs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionLog positionLog = await LogHelper.Get(id.Value);
            if (positionLog == null)
            {
                return HttpNotFound();
            }
            return View(positionLog);
        }

        // POST: PositionLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,Date,Action,PreviousStatus,ActualStatus")] PositionLog positionLog)
        {
            if (ModelState.IsValid)
            {
                await LogHelper.Edit(positionLog);
                return RedirectToAction("Index");
            }
            return View(positionLog);
        }

        // GET: PositionLogs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionLog positionLog = await LogHelper.Get(id.Value);
            if (positionLog == null)
            {
                return HttpNotFound();
            }
            return View(positionLog);
        }

        // POST: PositionLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await LogHelper.Delete(id);
            return RedirectToAction("Index");
        }
       
    }
}
