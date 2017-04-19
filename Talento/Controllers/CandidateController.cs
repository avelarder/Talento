using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Talento.Core;
using Talento.Entities;
using Talento.Models;

namespace Talento.Controllers
{
    public class CandidateController : Controller
    {

        ICandidate CandidateHelper;
        IPosition PositionHelper;
        ICustomUser UserHelper;
        IFileManagerHelper FileManagerHelper;

        public CandidateController(ICandidate candidateHelper, ICustomUser userHelper, IFileManagerHelper fileManagerHelper, IPosition positionHelper)
        {
            CandidateHelper = candidateHelper;
            UserHelper = userHelper;
            FileManagerHelper = fileManagerHelper;
            PositionHelper = positionHelper;
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Candidate, CandidateModel>()
                    .ForMember(t => t.CreatedBy_Id, opt => opt.MapFrom(s => s.CreatedBy_Id))
                ;
                cfg.CreateMap<Candidate, EditCandidateViewModel>();//.ForMember(c => c.IsTcsEmployee, opt => opt.MapFrom(s => s.IsTcsEmployee));

            });
        }

        private ActionResult AttachProfile(CreateCandidateViewModel model, Position toApply, List<ApplicationUser> recipients)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            var callbackUrl = Url.Action("Details", "Positions", new { toApply.Id }, protocol: Request.Url.Scheme);

            tw.WriteLine("A profile has been added to " + toApply.Title + " by " + User.Identity.Name + " . " +
                            "Please visit the following URL for more information: " + callbackUrl);
            tw.Write("Recipients: ");

            foreach (ApplicationUser user in recipients)
            {
                tw.Write(user.Email + ", ");
            }

            tw.Flush();
            tw.Close();
            return File(ms.GetBuffer(), "application/octet-stream", "MailNotification.txt");

#if DEBUG == false

            string currentUser = User.Identity.Name;
            SendEmailHelper.SendEmailProfile(currentUser);
#endif

        }

        //Charlie: call this action when generate a feedback interview
        private ActionResult InterviewFeedback(CandidateModel model, Position toapply, List<ApplicationUser> recipients)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);

            var callbackUrl = Url.Action("Details", "Positions", new { toapply.Id }, protocol: Request.Url.Scheme);

            tw.WriteLine("A profile's interview feedback form has been added to " + toapply.Title + " by " + User.Identity.Name + "." +
                            " Please visit the following URL for more information: " + callbackUrl);

            tw.Write("Recipients: ");

            foreach (ApplicationUser user in recipients)
            {
                tw.Write(user.Email + ", ");
            }
            tw.Flush();
            tw.Close();
            return File(ms.GetBuffer(), "application/octet-stream", "MailExample.txt");

#if DEBUG == false

            string currentUser = User.Identity.Name;
            SendEmailHelper.SendEmailFeedback(currentUser);
#endif
        }


        public virtual bool IsStateValid()
        {
            return ModelState.IsValid;
        }
        //GET: Edit Candidate
        [Authorize(Roles = "PM, TL")]
        public ActionResult Edit(int id, int positionId)
        {
            EditCandidateViewModel candidate = AutoMapper.Mapper.Map<EditCandidateViewModel>(CandidateHelper.Get(id));
            Position currentPosition = PositionHelper.Get(positionId);

            if (!currentPosition.Candidates.Any(x => x.Id.Equals(candidate.Id)))
            {
                return HttpNotFound();
            }

            if (currentPosition.Status == Status.Cancelled || currentPosition.Status == Status.Closed)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The information you are looking for is not available");
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
        [Authorize(Roles = "PM, TL")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCandidateViewModel candidate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<FileBlob> files = ((List<FileBlob>)Session["files"]);

                    string email = CandidateHelper.Get(candidate.Id).Email;

                    Candidate newCandidate = new Candidate
                    {
                        Id = candidate.Id,
                        Description = candidate.Description,
                        Competencies = candidate.Competencies,
                        Name = candidate.Name,
                        IsTcsEmployee = candidate.IsTcsEmployee.Equals("on"),
                        Status = candidate.Status,
                        Email = email
                    };

                    if (files != null)
                    {
                        files.ForEach(x => x.Candidate_Id = newCandidate.Id);
                    }
                    int result = CandidateHelper.Edit(newCandidate, files);
                    switch (result)
                    {
                        case -1:
                            ModelState.AddModelError("", "The designated Candidate already exists");
                            break;
                    }

                }
                return RedirectToAction("Index", "Dashboard", null);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(CreateCandidateViewModel candidate)
        {
            if(ModelState.IsValid)
            {
                List<FileBlob> files = ((List<FileBlob>)Session["files"]);
                ApplicationUser user = UserHelper.GetUserByEmail(User.Identity.Name);
                List<Position> position = new List<Position> { PositionHelper.Get(candidate.Position_Id) };
                if (position[0].Status == Status.Open)
                {
                    Candidate newCandidate = new Candidate
                    {
                        Competencies = candidate.Competencies,
                        CratedOn = DateTime.Now,
                        CreatedBy = user,
                        Description = candidate.Description,
                        Email = candidate.Email,
                        Name = candidate.Name,
                        IsTcsEmployee = candidate.IsTcsEmployee.Equals("on"),
                        Status = candidate.Status,
                        CreatedBy_Id = user.Id,
                        Positions = position
                    };
                    if (files != null)
                    {
                        files.ForEach(x => x.Candidate = newCandidate);
                    }
                    int result = CandidateHelper.Create(newCandidate, files);
                    position[0].OpenStatus = OpenStatus.Screening;
                    result = CandidateHelper.CandidateToPosition(newCandidate, position[0]);
                    switch (result)
                    {
                        case 0: return AttachProfile(candidate, position.First(), UserHelper.GetByRoles(new List<string> { "PM", "TL", "TAG", "RMG" }));
                            break;
                        case -1: ModelState.AddModelError("", "The designated Candidate already exists");
                            break;
                        case -2: ModelState.AddModelError("", "The desired Candidate already exists in selected position");
                            break;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The position is not opened");
                }
                return RedirectToAction("Index", "Dashboard", null);
            }
            else
            {
                ModelState.AddModelError("", "The designated Candidate already exists");
                return RedirectToAction("Index", "Dashboard", null);
            }
        }

    }
}
