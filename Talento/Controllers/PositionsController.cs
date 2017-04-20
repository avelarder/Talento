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
using Talento.Core.Helpers;
using PagedList;
using Talento.Core.Utilities;
using System.Security;
using System.Web.Security;

namespace Talento.Controllers
{
    [Authorize(Roles = "Admin, PM, TAG, RMG, TL")]
    public class PositionsController : Controller
    {
        Core.IPosition PositionHelper;
        Core.ICustomUser UserHelper;

        public PositionsController(Core.IPosition positionHelper, Core.ICustomUser userHelper)
        {
            UserHelper = userHelper;
            PositionHelper = positionHelper;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Position, PositionModel>()
                    .ForMember(t => t.ApplicationUser_Id, opt => opt.MapFrom(s => s.ApplicationUser_Id))
                    .ForMember(s => s.Candidates, opt => opt.MapFrom(p => p.Candidates))
                     .ForMember(s => s.Logs, opt => opt.MapFrom(p => p.Logs))
                ;
                cfg.CreateMap<Position, EditPositionViewModel>();
                cfg.CreateMap<Log, PositionLogViewModel>();
                cfg.CreateMap<EditPositionViewModel, Position>();
                cfg.CreateMap<Candidate, CandidateModel>();

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
        [Authorize(Roles = "Admin, PM, TL, TAG, RMG")]
        public ActionResult Details(int? id, int? page)
        {
            ViewBag.userRole = "";
            if (Roles.IsUserInRole("Admin")) {
                ViewBag.userRole = "Admin";
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PositionModel position = AutoMapper.Mapper.Map<PositionModel>(PositionHelper.Get(id.Value));
            if (position == null || position.Status == Status.Removed)
            {
                return HttpNotFound();
            }

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfCandidatePositions = position.Candidates.OrderByDescending(x=>x.CratedOn).ToPagedList(pageNumber, 5); // will only contain 5 products max because of the pageSize
            ViewBag.page = pageNumber;
            ViewBag.onePageOfCandidatePositions = onePageOfCandidatePositions;

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
            //Search EmailPM in SearchPM Helper
            ApplicationUser pmUser = UserHelper.SearchPM(position.EmailPM);

            //If EmailPM is null return AddModelError
            if (pmUser == null)
            {
                ModelState.AddModelError(string.Empty, "PM is not valid");
            }

            string user = User.Identity.GetUserId();

            if (IsStateValid())
            {
                Position pos = new Position()
                {
                    Owner = UserHelper.GetUserById(user),
                    Area = position.Area,
                    EngagementManager = position.EngagementManager,
                    Title = position.Title,
                    CreationDate = DateTime.Now,
                    Description = position.Description,
                    PortfolioManager = pmUser,
                    RGS = position.RGS,
                    Status = Status.Open,
                    PortfolioManager_Id = position.EmailPM,
                    ApplicationUser_Id = user,
                    LastOpenedBy = UserHelper.GetUserById(user),
                    LastOpenedDate = DateTime.Now
                };
                PositionHelper.Create(pos);
                return RedirectToAction("Index", "Dashboard");
            }

            return View(position);
        }

        // GET: Positions/Edit/5
        [Authorize(Roles = "PM, TL")]
        public ActionResult Edit(int id)
        {

            try
            {
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

        public virtual bool IsStateValid()
        {
            return ModelState.IsValid;
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "PM, TL")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPositionViewModel position)
        {
            ApplicationUser currentUser = UserHelper.GetUserByEmail(User.Identity.Name);
            if (this.IsStateValid())
            {
                if (PositionHelper.Edit(AutoMapper.Mapper.Map<Position>(position), currentUser))
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

        #region PositionLogs
        [ChildAndAjaxActionOnly]
        public ActionResult List(int? id, int pagex = 1, int pagesize = 5, string clase = "slide-right")
        {
            try
            {
                if (!Request.IsLocal)
                {
                    throw new SecurityException();
                }
                // No ID return 404
                if (id == null)
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
                string url = Url.Action("List", "Positions");
                // Get Position With Logs
                PositionModel position = AutoMapper.Mapper.Map<PositionModel>(PositionHelper.Get(id.Value));
                var logs = position.Logs.OrderByDescending(x => x.Date).ToList();
                // Get List of PositionLogs and the Pagination
                var containerLogs = PositionHelper.PaginateLogs(logs, pagex, pagesize, url);
                // No logs with the ID return 404
                if (containerLogs == null)
                {
                    return HttpNotFound();
                }
                var logx = AutoMapper.Mapper.Map<List<PositionLogViewModel>>(containerLogs.Item1);
                // Pagination
                var pagination = containerLogs.Item2;
                // General ViewData
                ViewData["AnimationClass"] = clase;
                ViewData["Count"] = logs.Count;
                ViewData["Pagination"] = pagination;

                return PartialView(logx);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        [ChildActionOnly]
        public ActionResult Pagination(Pagination pagination)
        {
            return PartialView(pagination);
        }
        #endregion  

    }
}
