using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using Talento.Core;
using Talento.Entities;
using Talento.Models;
using Talento.EmailManager;
using Microsoft.AspNet.Identity;

namespace Talento.Controllers
{
    [HandleError]
    public class CandidateController : Controller
    {
        ICandidate CandidateHelper;
        IPosition PositionHelper;
        ICustomUser UserHelper;
        IMessenger EmailManager;
        IComment CommentHelper;

        public CandidateController(ICandidate candidateHelper, ICustomUser userHelper, IPosition positionHelper, IMessenger emailManager, IComment commentHelper)
        {
            CommentHelper = commentHelper;
            EmailManager = emailManager;
            CandidateHelper = candidateHelper;
            UserHelper = userHelper;
            PositionHelper = positionHelper;
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Candidate, CandidateModel>()
                    .ForMember(t => t.CreatedBy_Id, opt => opt.MapFrom(s => s.CreatedBy_Id))
                    .ForMember(t => t.CreatedOn, opt => opt.MapFrom(s => s.CreatedOn))
                ;
                cfg.CreateMap<Candidate, EditCandidateViewModel>()
                    .ForMember(s => s.PositionCandidates, opt => opt.MapFrom(p => p.PositionCandidates))
                ;

            });
        }

        private ActionResult AttachProfile(CreateCandidateViewModel model, Position toApply, List<ApplicationUser> recipients)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            var callbackUrl = Url.Action("Details", "Positions", new { toApply.PositionId }, protocol: Request.Url.Scheme);
            string body = "A profile has been added to " + toApply.Title + " by " + User.Identity.Name + " . " +
                            "Please visit the following URL for more information: " + callbackUrl;
            tw.WriteLine(body);
            tw.Write("Recipients: ");
            List<string> recip = new List<string>();
            List<string> cc = new List<string>
            {
                toApply.Owner.Email
            };
            List<string> bcc = new List<string>();
            foreach (ApplicationUser user in recipients)
            {
                tw.Write(user.Email + ", ");
                recip.Add(user.Email);
            }
            tw.Flush();
            tw.Close();
            //emailManager.SendEmail( recip, "talento@tcs.com", bcc, cc, "Talento notification messenger", body);
            return File(ms.GetBuffer(), "application/octet-stream", "MailNotification.txt");
        }
        
        //GET: Edit Candidate
        [Authorize]
        public ActionResult Edit(int id, int positionId)
        {
            Session["files"] = null;
            EditCandidateViewModel candidate = AutoMapper.Mapper.Map<EditCandidateViewModel>(CandidateHelper.Get(id));
            candidate.Position_Id = positionId;
            Position currentPosition = PositionHelper.Get(positionId);

            if (!currentPosition.PositionCandidates.Any(x => x.CandidateID.Equals(candidate.CandidateId)))
            {
                return HttpNotFound();
            }

            if (candidate == null)
            {
                return HttpNotFound();
            }

            return View(candidate);
        }

        // POST: Candidate/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCandidateViewModel candidate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HashSet<FileBlob> files = ((HashSet<FileBlob>)Session["files"]);
                    string email = CandidateHelper.Get(candidate.CandidateId).Email;
                    Candidate newCandidate = new Candidate
                    {
                        CandidateId = candidate.CandidateId,
                        Description = candidate.Description,
                        Competencies = candidate.Competencies,
                        Name = candidate.Name,
                        IsTcsEmployee = candidate.IsTcsEmployee,
                        Email = email,
                        PositionCandidates = CandidateHelper.Get(candidate.CandidateId).PositionCandidates
                    };

                    int result = CandidateHelper.Edit(newCandidate, files, UserHelper.GetUserByEmail(User.Identity.Name));
                    switch (result)
                    {
                        case -1:
                            ModelState.AddModelError("", "The designated Candidate already exists");
                            break;
                    }
                }
                return RedirectToAction("Details", "Positions", new { id = candidate.Position_Id });
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error ocurred. Please contact system administrator.");
            }
        }
        
        [Authorize(Roles = "Admin, PM, TL, TAG, RMG")]
        public JsonResult ValidEmail(string emailCandidate, string positionId)
        {
            try
            {
                int id = int.Parse(positionId);
                if (PositionHelper.Get(id).PositionCandidates.Where(x => x.Candidate.Email.Trim().ToLower().Equals(emailCandidate.Trim().ToLower())).Count() > 0)
                {
                    return null;
                }
                else
                {
                    return Json(new { valid = true });
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        private ActionResult New(CreateCandidateViewModel candidate, ActionResult actionError)
        {
            if (ModelState.IsValid)
            {
                HashSet<FileBlob> files = ((HashSet<FileBlob>)Session["files"]);
                ApplicationUser user = UserHelper.GetUserByEmail(User.Identity.Name);
                Position position = PositionHelper.Get(candidate.Position_Id);
                if (position.Status == PositionStatus.Open)
                {
                    Candidate newCandidate = new Candidate
                    {
                        Competencies = candidate.Competencies,
                        CreatedOn = DateTime.Now,
                        CreatedBy = user,
                        Description = candidate.Description,
                        Email = candidate.Email,
                        Name = candidate.Name,
                        IsTcsEmployee = candidate.IsTcsEmployee,
                        CreatedBy_Id = user.Id,
                        PositionCandidates = new List<PositionCandidates>
                        {
                            new PositionCandidates
                            {
                                Position = PositionHelper.Get(candidate.Position_Id),
                                Status = PositionCandidatesStatus.New,
                            }
                        },
                        FileBlobs = files
                    };
                    int result = CandidateHelper.Create(newCandidate);
                    if (result != -1)
                    {
                        return AttachProfile(candidate, position, UserHelper.GetByRoles(new List<string> { "PM", "TL", "TAG", "RMG" }));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The position is not opened");
                }
                return actionError;
            }
            else
            {
                return actionError;
            }
        }

        public ActionResult Create(int id)
        {
            return View(new CreateCandidateViewModel()
            {
                Position_Name = PositionHelper.Get(id).Title,
                Position_Id = id
            });
        }

        public ActionResult Details(int id, int positionId)
        {
            CandidateModel aux = AutoMapper.Mapper.Map<CandidateModel>(CandidateHelper.Get(id));
            aux.isadmin = User.IsInRole("Admin");
            aux.PositionId = positionId;
            aux.Comments = CommentHelper.Get(id,positionId).OrderByDescending(x => x.Date).ToList();
            aux.isopenposition = PositionHelper.Get(positionId).Status.Equals(PositionStatus.Open);
            if (User != null)
            {
                if (User.Identity != null)
                {
                    ViewData["Image"] = UserHelper.GetUserByEmail(User.Identity.Name).ImageProfile;
                }
            }
            return View(aux);
        }

        [HttpPost]
        public ActionResult Create(CreateCandidateViewModel candidate)
        {
            return New(candidate, RedirectToAction("Index", "Dashboard", null));
        }

        [HttpPost]
        public ActionResult CreateDetails(CreateCandidateViewModel candidate)
        {
            return New(candidate, RedirectToAction("Details", "Positions", new { id = candidate.Position_Id }));
        }

        [ChildActionOnly]
        [Authorize]
        public ActionResult Status(PositionModel positionModel, int candidateId)
        {
            PositionCandidatesStatus status = positionModel.PositionCandidates.FirstOrDefault(x => x.CandidateID == candidateId).Status;

            var name = Enum.GetName(typeof(PositionCandidatesStatus), status);
            name = name.Replace("_", " ");

            if (User.IsInRole("TAG") || User.IsInRole("RMG")) 
            {
                if ((int)status == 1 || (int)status == 5)
                {
                    ViewData["Status"] = status;
                    return PartialView();
                }
            }
            else if (User.IsInRole("PM") || User.IsInRole("TL"))
            {
                if ((int)status == 3)
                {
                    ViewData["Status"] = status;
                    return PartialView();
                }
            }            
            return Content(name);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "PM, TL, TAG, RMG")]
        public ActionResult AddComment(string Comment, int PositionId, int CandidateId)
        {
            CommentHelper.Create(new Comment
            {
                CandidateId = CandidateId,
                Content = Comment,
                User = UserHelper.GetUserByEmail(User.Identity.Name),
                PositionId = PositionId
            });

            return RedirectToAction("Details", "Candidate", new { id = CandidateId, positionId = PositionId });
        }
        
        [HttpPost]
        [ChildAndAjaxActionOnly]
        [Authorize]
        public ActionResult ChangeStatus(int positionCandidateId, int status)
        {
            try
            {
                var positionCandidateStatus = (PositionCandidatesStatus)status;
                string currentUserId = User.Identity.GetUserId();
                var currentUser = UserHelper.GetUserById(currentUserId);
                
                CandidateHelper.ChangeStatus(positionCandidateId, positionCandidateStatus, currentUser);

                return new HttpStatusCodeResult(200);
            }
            catch (Exception)
            {
                throw;
            }
        }        
    }
}
