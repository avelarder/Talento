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
using Talento.Core.Data;
using Talento.Entities;

namespace Talento.Controllers
{
    public class PositionsController : Controller
    {
        Core.IPosition PositionHelper;

        public PositionsController(Core.IPosition positionHelper)
        {
            PositionHelper = positionHelper;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Position, Models.PositionModel>();
            });
        }
        
        // GET: Positions
        public async Task<ActionResult> Index()
        {
            return View(await PositionHelper.GetAll());
        }

        [Authorize(Roles = "PM, TL, TAG, RMG")]
        // GET: Positions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PositionModel position = AutoMapper.Mapper.Map<PositionModel>(await PositionHelper.Get(id.Value));
            if (position == null)
            {
                return HttpNotFound();
            }

            return View(position);
        }

        // GET: Positions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Id,Title,Description,CreationDate,Area,EngagementManager,RGS,Status")]
            PositionModel position)
        {
            if (ModelState.IsValid)
            {
                await PositionHelper.Create(AutoMapper.Mapper.Map<Position>(position));
                return RedirectToAction("Index");
            }

            return View(position);
        }

        // GET: Positions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionModel position = AutoMapper.Mapper.Map<PositionModel>(await PositionHelper.Get(id.Value));
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,CreationDate,Area,EngagementManager,RGS,Status")] Position position)
        {
            if (ModelState.IsValid)
            {
                await PositionHelper.Edit(AutoMapper.Mapper.Map<Position>(position));
                return RedirectToAction("Index");
            }
            return View(position);
        }

        // GET: Positions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionModel position = AutoMapper.Mapper.Map<PositionModel>(await PositionHelper.Get(id.Value));
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await PositionHelper.Delete(id);
            return RedirectToAction("Index");
        }

      
    }
}
