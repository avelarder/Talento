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
using Talento.Entities;

namespace Talento.Controllers
{
    public class PositionLogsController : Controller
    {
        Core.IPositionLog LogHelper;

        public PositionLogsController(Core.IPositionLog logHelper)
        {
            LogHelper = logHelper;
        }
        // GET: PositionLogs
        public async Task<ActionResult> Index()
        {
            return View(await LogHelper.GetAll());
        }

        // GET: PositionLogs/Details/5
        public async Task<ActionResult> Details(int? id)
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Date,Action,PreviousStatus,ActualStatus")] PositionLog positionLog)
        {
            if (ModelState.IsValid)
            {
                await LogHelper.Create(positionLog);
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
