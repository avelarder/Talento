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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Talento.Controllers
{
    [Authorize(Roles ="PM, TAG, RMG, TM")]
    public class PositionsController : Controller
    {
        Core.IPosition PositionHelper;
        ApplicationUser appUser;
        public PositionsController(Core.IPosition positionHelper)
        {
            PositionHelper = positionHelper;
            appUser = new ApplicationUser();
        }

        // GET: Positions
        public async Task<ActionResult> Index()
        {
            return View(await PositionHelper.GetAll());
        }

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
        public ActionResult Create(CreatePositionViewModel position)
        {
            ApplicationUser pmUser = PositionHelper.SearchPM(position.EmailPM);

            if (pmUser == null)
            {
                ModelState.AddModelError(string.Empty, "PM is not valid");
                
            }

            var user = User.Identity.GetUserId();
            //ApplicationDbContext db = new ApplicationDbContext();
            //ApplicationUser appUser1 = db.Users.FirstOrDefault(x => x.Id == user);

            if (ModelState.IsValid)
            {
                Position pos = new Position()
                {
                    Owner = PositionHelper.GetUser(user),
                    Area = position.Area,
                    EngagementManager = position.EngagementManager,
                    Title = position.Title,
                    CreationDate = DateTime.Now,
                    Description = position.Description,
                    PortfolioManager = pmUser,
                    RGS = position.RGS,
                    Status = Status.Open,
                    PortfolioManager_Id = position.EmailPM,
                    ApplicationUser_Id = appUser.Id
                                                            
                };
                //PositionHelper.Create(AutoMapper.Mapper.Map<Position>(pos));
                PositionHelper.Create(pos);
                return RedirectToAction("Index","Dashboard");

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
