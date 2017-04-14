using System;
using System.Collections.Generic;
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
        IPositionCandidate PositionsCandidatesHelper;
        IFileManagerHelper FileManagerHelper;

        public CandidateController(ICandidate candidateHelper, ICustomUser userHelper, IPositionCandidate positionsCandidatesHelper, IFileManagerHelper fileManagerHelper, IPosition positionHelper)
        {
            CandidateHelper = candidateHelper;
            UserHelper = userHelper;
            PositionsCandidatesHelper = positionsCandidatesHelper;
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

        //GET: Edit Candidate
        [Authorize(Roles = "PM, TL")]
        public ActionResult Edit(int id, int positionId)
        {
            EditCandidateViewModel candidate = AutoMapper.Mapper.Map<EditCandidateViewModel>(CandidateHelper.Get(id));
            List<PositionCandidate> positionsCandidates = PositionsCandidatesHelper.GetCandidatesByPositionId(positionId);

            if (!positionsCandidates.Any(x => x.Candidate.Id.Equals(candidate.Id)))
            {
                return HttpNotFound();
            }

            PositionCandidate item = positionsCandidates[0];
            {
                if (item.Position.Status == Status.Cancelled || item.Position.Status == Status.Closed)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The information you are looking for is not available");
                }
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
        public ActionResult New(CreateCandidateViewModel candidate)
        {
            List<FileBlob> files = ((List<FileBlob>)Session["files"]);

            ApplicationUser user = UserHelper.GetUserByEmail(User.Identity.Name);

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
                CreatedBy_Id = user.Id
            };
            Position position = PositionHelper.Get(candidate.Position_Id);
            PositionsCandidatesHelper.Create(newCandidate, position);

            if (files != null)
            {
                files.ForEach(x => x.Candidate = newCandidate);
            }
            int result = CandidateHelper.Create(newCandidate, files);
            switch (result)
            {
                case -1:
                    ModelState.AddModelError("", "The designated Candidate already exists");
                    break;
            }

            return RedirectToAction("Index", "Dashboard", null);
        }

    }
}