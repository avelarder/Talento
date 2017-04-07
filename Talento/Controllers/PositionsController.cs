﻿using System;
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
    [Authorize(Roles = "PM, TAG, RMG, TL")]
    public class PositionsController : Controller
    {
        Core.IPosition PositionHelper;
        public PositionsController(Core.IPosition positionHelper)
        {

            PositionHelper = positionHelper;
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Position, PositionModel>()
                    .ForMember(t => t.ApplicationUser_Id, opt => opt.MapFrom(s => s.ApplicationUser_Id))
                ;
                cfg.CreateMap<Position, EditPositionViewModel>();
                cfg.CreateMap<EditPositionViewModel, Position>();

                /*
               This could be useful in future in case of needing to edit the owner user account. It is not yet requested in the Edit user story 295
               4 / 4 / 2017 - Charlie
               cfg.CreateMap<Position, EditPositionViewModel>()
                    .ForMember(t => t.OwnerEmail, opt => opt.MapFrom(s => s.Owner.Email))
                ;
                */
            });
        }

        // GET: Positions
        public async Task<ActionResult> Index()
        {
            return View(await PositionHelper.GetAll());
        }

        // GET: Positions/Details/5
        [Authorize(Roles = "PM, TL, TAG, RMG")]
        public async Task<ActionResult> Details(int? id)
        {
            string role = "basic";
            ViewData["Role"] = role;
            ViewData["RoleClass"] = role + "-role";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PositionModel position = AutoMapper.Mapper.Map<PositionModel>(PositionHelper.Get(id.Value));
            if (position == null || position.Status == Status.Removed)
            {
                return HttpNotFound();
            }

            return View(position);
        }

        // GET: Positions/Create

        public ActionResult Create()
        {
            string role = "basic";
            ViewData["Role"] = role;
            ViewData["RoleClass"] = role + "-role";
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
                    ApplicationUser_Id= User.Identity.Name,
                    LastOpenedBy = PositionHelper.GetUser(user),
                    LastOpenedDate = DateTime.Now
                };
                //PositionHelper.Create(AutoMapper.Mapper.Map<Position>(pos));
                PositionHelper.Create(pos, User.Identity.Name);
                return RedirectToAction("Index","Dashboard");

            }

            return View(position);

        }

        // GET: Positions/Edit/5
        [Authorize(Roles = "PM, TL")]
        public ActionResult Edit(int id)
        {
            string role = "basic";
            ViewData["Role"] = role;
            ViewData["RoleClass"] = role + "-role";

            try {
                EditPositionViewModel position = AutoMapper.Mapper.Map<EditPositionViewModel>(PositionHelper.Get(id));
                if (position == null)
                {
                    return HttpNotFound();
                }
                if (position.Status.Equals(Status.Removed))
                {
                    return HttpNotFound();
                }
                return View(position);
            }
            catch (InvalidOperationException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The designated Position does not have a valid ID");
            }
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "PM, TL")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditPositionViewModel position)
        {
            if (this.IsStateValid())
            {
                if (PositionHelper.Edit(AutoMapper.Mapper.Map<Position>(position), User.Identity.Name))
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return View(position);
                }

            }
            return View(position);
        }

        // GET: Positions/Delete/5
        [Authorize(Roles = "PM, TL")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionModel position = AutoMapper.Mapper.Map<PositionModel>(PositionHelper.Get(id.Value));
            if (position == null) 
            {
                return HttpNotFound();
            }
            if (position.Status == Status.Removed)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            string uId = User.Identity.GetUserId();
            PositionHelper.Delete(id.Value, uId);
            return RedirectToAction("Index", "Dashboard");
        }

        public virtual bool IsStateValid()
        {
            return this.ModelState.IsValid;
        }

    }
}
