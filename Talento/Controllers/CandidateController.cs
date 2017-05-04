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

namespace Talento.Controllers
{
    public class CandidateController : Controller
    {
        ICandidate CandidateHelper;
        IPosition PositionHelper;
        ICustomUser UserHelper;
        IMessenger EmailManager;

        public CandidateController(ICandidate candidateHelper, ICustomUser userHelper, IPosition positionHelper, IMessenger emailManager)
        {
            EmailManager = emailManager;
            CandidateHelper = candidateHelper;
            UserHelper = userHelper;
            PositionHelper = positionHelper;
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Candidate, CandidateModel>()
                    .ForMember(t => t.CreatedBy_Id, opt => opt.MapFrom(s => s.CreatedBy_Id))
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

        //Call this action when generate a feedback interview
        private ActionResult InterviewFeedback(CandidateModel model, Position toapply, List<ApplicationUser> recipients)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            var callbackUrl = Url.Action("Details", "Positions", new { toapply.PositionId }, protocol: Request.Url.Scheme);
            string body = "A profile's interview feedback form has been added to " + toapply.Title + " by " + User.Identity.Name + "." +
                            " Please visit the following URL for more information: " + callbackUrl;
            tw.WriteLine(body);

            tw.Write("Recipients: ");
            List<string> recip = new List<string>();
            List<string> cc = new List<string>
            {
                toapply.Owner.Email
            };
            List<string> bcc = new List<string>();
            foreach (ApplicationUser user in recipients)
            {
                tw.Write(user.Email + ", ");
                recip.Add(user.Email);
            }
            tw.Flush();
            tw.Close();
            //emailManager.SendEmail(recip, "talento@tcs.com", bcc, cc, "Talento notification messenger", body);
            return File(ms.GetBuffer(), "application/octet-stream", "MailExample.txt");
        }

        //Call this action when a feedback interview change status
        public ActionResult InterviewFeedbackNewStatus(EditCandidateViewModel model, Position toapply, List<ApplicationUser> recipients)
        {

            if (model.PositionCandidates.FirstOrDefault(x => x.Position.PositionId == toapply.PositionId && x.Candidate.CandidateId == model.CandidateId).Status == PositionCandidatesStatus.Interview_Accepted)
            {
                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                string callbackUrl = Url.Action("Details", "Positions", new { toapply.PositionId }, protocol: Request.Url.Scheme);
                string body = model.Email + "has been accepted for " + toapply.Title + ", reported by " + User.Identity.Name + "." +
                                " Please visit the following URL for more information: " + callbackUrl;
                tw.WriteLine(body);

                tw.Write("Recipients: ");
                List<string> recip = new List<string>();
                List<string> cc = new List<string>
                {
                    toapply.Owner.Email
                };
                List<string> bcc = new List<string>();
                foreach (ApplicationUser user in recipients)
                {
                    tw.Write(user.Email + ", ");
                    recip.Add(user.Email);
                }
                tw.Flush();
                tw.Close();
                //emailManager.SendEmail(recip, "talento@tcs.com", bcc, cc, "Talento notification messenger", body);
                return File(ms.GetBuffer(), "application/octet-stream", "MailExample.txt");
            }
            else
            {
                return null;
            }
        }


        public virtual bool IsStateValid()
        {
            return ModelState.IsValid;
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

            return PartialView(candidate);
        }

        // POST: Candidate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
                        IsTcsEmployee = candidate.IsTcsEmployee.Equals("on"),
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
        // GET: Candidate
        public ActionResult Manage()
        {
            return PartialView();
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
                if (position.Status == Status.Open)
                {
                    Candidate newCandidate = new Candidate
                    {
                        Competencies = candidate.Competencies,
                        CreatedOn = DateTime.Now,
                        CreatedBy = user,
                        Description = candidate.Description,
                        Email = candidate.Email,
                        Name = candidate.Name,
                        IsTcsEmployee = candidate.IsTcsEmployee.Equals("on"),
                        CreatedBy_Id = user.Id,
                        PositionCandidates = new List<PositionCandidates>
                        {
                            new PositionCandidates
                            {
                                Position = PositionHelper.Get(candidate.Position_Id),
                                Status = PositionCandidatesStatus.New
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


        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Create(CreateCandidateViewModel candidate)
        {
            return New(candidate, RedirectToAction("Index", "Dashboard", null));
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult CreateDetails(CreateCandidateViewModel candidate)
        {
            return New(candidate, RedirectToAction("Details", "Positions", new { id = candidate.Position_Id }));
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
                    throw;
                }
            }
        }
    }
}
