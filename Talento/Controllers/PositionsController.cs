using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Talento.Models;
using Talento.Entities;
using Microsoft.AspNet.Identity;
using PagedList;
using Talento.Core.Utilities;
using System.Security;
using System.Web.Security;
using System.Web.Helpers;
using Talento.Core;

namespace Talento.Controllers
{
    [HandleError]
    [Authorize(Roles = "Admin, PM, TAG, RMG, TL")]
    public class PositionsController : Controller
    {
        IPosition PositionHelper;
        ICustomUser UserHelper;
        ICandidate CandidateHelper;
        IComment CommentHelper;
        IUtilityApplicationSettings ApplicationSettings;
        IApplicationSetting SettingsHelper;

        public PositionsController(Core.IPosition positionHelper, Core.ICustomUser userHelper, Core.ICandidate candidateHelper, IComment commentHelper, IUtilityApplicationSettings appSettings, IApplicationSetting settingsHelper)
        {
            UserHelper = userHelper;
            CommentHelper = commentHelper;
            PositionHelper = positionHelper;
            CandidateHelper = candidateHelper;
            ApplicationSettings = appSettings;
            SettingsHelper = settingsHelper;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Position, PositionModel>()
                    .ForMember(t => t.ApplicationUser_Id, opt => opt.MapFrom(s => s.ApplicationUser_Id))
                    .ForMember(s => s.PositionCandidates, opt => opt.MapFrom(p => p.PositionCandidates))
                     .ForMember(s => s.Logs, opt => opt.MapFrom(p => p.Logs))
                ;
                cfg.CreateMap<Position, EditPositionViewModel>();
                cfg.CreateMap<Log, PositionLogViewModel>();
                cfg.CreateMap<EditPositionViewModel, Position>();
                cfg.CreateMap<Candidate, CandidateModel>();
                cfg.CreateMap<ApplicationSetting, ApplicationSettingModel>()
                    .ForMember(apsm => apsm.ApplicationSettingId, aps => aps.MapFrom(s => s.ApplicationSettingId));

            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddComment(string Comment, int PositionId)
        {
            CommentHelper.Create(new Comment
            {
                CandidateId = null,
                Content = Comment,
                User = UserHelper.GetUserByEmail(User.Identity.Name),
                PositionId = PositionId
            });

            return RedirectToAction("Details", "Positions", new { id = PositionId });
        }
        
        // GET: Positions/Details/5
        [Authorize(Roles = "Admin, PM, TL, TAG, RMG")]
        public ActionResult Details(int? id, int? page)
        {
            ViewBag.userRole = "";
            if (Roles.IsUserInRole("Admin"))
            {
                ViewBag.userRole = "Admin";
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PositionModel position = AutoMapper.Mapper.Map<PositionModel>(PositionHelper.Get(id.Value));

            if (position == null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                position.Comments = CommentHelper.GetAll(id.Value).OrderByDescending(x => x.Date).ToList();

                if (position.Comments.Count > 0)
                {
                    ApplicationSettingModel applicationParameter = AutoMapper.Mapper.Map<ApplicationSettingModel>(SettingsHelper.GetByName("Comments"));
                    int commentCount;

                    //get application settings value for comment count. If application parameter is not found, the default is 10
                    if (applicationParameter != null)
                    {
                        commentCount = Convert.ToInt32(applicationParameter.ParameterValue);
                    }
                    else
                    {
                        commentCount = 10;
                    }

                    if (position.Comments.Count > 10)
                    {
                        position.Comments = position.Comments.Take(commentCount).ToList();
                    }
                }
                else
                {
                    position.Comments = new List<Comment>();
                }

            }

            // Pagination
            var pageSizeValue = ApplicationSettings.GetSetting("pagination", "pagesize"); // Setting Parameter
            int pageSize = 0;

            if (pageSizeValue != null)
            {
                pageSize = Convert.ToInt32(pageSizeValue);
            }
            else
            {
                pageSize = 25;
            }

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfCandidatePositions = position.PositionCandidates.OrderByDescending(x => x.Candidate.CreatedOn).ToPagedList(pageNumber, pageSize); // will only contain 5 products max because of the pageSize
            ViewBag.page = pageNumber;
            ViewBag.onePageOfCandidatePositions = onePageOfCandidatePositions;
            if (User != null)
            {
                if (User.Identity != null)
                {
                    ViewData["Image"] = UserHelper.GetUserByEmail(User.Identity.Name).ImageProfile;
                }

            }
            return View(position);
        }

        // GET: Positions/Create

        public ActionResult Create()
        {
            return View();
        }

        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        protected class ValidateJsonAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
                try
                {
                    if (filterContext == null)
                    {
                        throw new ArgumentNullException("filterContext");
                    }
                    var httpContext = filterContext.HttpContext;
                    var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
                    AntiForgery.Validate(cookie?.Value, httpContext.Request.Params["__RequestVerificationToken"]);
                }
                catch (Exception)
                {
                    //throw;
                }
            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult PMExists(string email)
        {
            if (UserHelper.SearchPM(email).Equals(null))
            {
                return null;
            }
            else
            {
                return Json(true);
            }
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
                    Status = PositionStatus.Open,
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
                if (position.Status.Equals(PositionStatus.Removed))
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

            if (position.Status == PositionStatus.Removed)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            string uId = User.Identity.GetUserId();
            PositionHelper.Delete(id.Value, uId);
            return RedirectToAction("Index", "Dashboard");
        }

        [Authorize(Roles = "PM, TL")]
        public ActionResult DeleteCandidate(int? idPosition, int? idCandidate)
        {
            if (idPosition == null || idCandidate == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionModel position = AutoMapper.Mapper.Map<PositionModel>(PositionHelper.Get(idPosition.Value));
            if (position == null)
            {
                return HttpNotFound();
            }
            if (position.Status == PositionStatus.Removed)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ApplicationUser currentUser = UserHelper.GetUserByEmail(User.Identity.Name);
            Candidate candidate = CandidateHelper.Get(idCandidate.Value);
            PositionHelper.DeleteCandidate(PositionHelper.Get(idPosition.Value), candidate, currentUser);
            return RedirectToAction("Detail", "Positions", new { id = idPosition, page = 1 });
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
                    return View("Error");
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

                // Pagination
                var pageSizeValue = ApplicationSettings.GetSetting("pagination", "pagesize"); // Setting Parameter

                if (pageSizeValue != null)
                {
                    pagesize = Convert.ToInt32(pageSizeValue);
                }
                else
                {
                    pagesize = 25;
                }

                // Get List of PositionLogs and the Pagination
                var containerLogs = PositionHelper.PaginateLogs(logs, pagex, pagesize, url);
                // No logs with the ID return 404
                if (containerLogs == null)
                {
                    return View("Error");
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
                return View("Error");
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
