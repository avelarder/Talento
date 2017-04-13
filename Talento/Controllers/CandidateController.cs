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
        ICustomUser UserHelper;
        IPositionCandidates PositionsCandidatesHelper;
        IFileManagerHelper FileManagerHelper;

        public CandidateController(ICandidate candidateHelper, ICustomUser userHelper, IPositionCandidates positionsCandidatesHelper, IFileManagerHelper fileManagerHelper)
        {
            CandidateHelper = candidateHelper;
            UserHelper = userHelper;
            PositionsCandidatesHelper = positionsCandidatesHelper;
            FileManagerHelper = fileManagerHelper;

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Candidate, CandidateModel>()
                    .ForMember(t => t.CreatedBy_Id, opt => opt.MapFrom(s => s.CreatedBy_Id))
                ;
                cfg.CreateMap<EditCandidateViewModel, Candidate>();
            });
        }

        public virtual bool IsStateValid()
        {
            return ModelState.IsValid;
        }

        // GET: Candidate
        public ActionResult Index()
        {
            return View();
        }

        //GET: Edit Candidate
        [Authorize(Roles = "PM, TL")]
        public ActionResult Edit(int id, PositionModel position)
        {
            try
            {
                EditCandidateViewModel candidate = AutoMapper.Mapper.Map<EditCandidateViewModel>(CandidateHelper.Get(id));
                if (candidate == null)
                {
                    return HttpNotFound();
                }

                if (position.Status == Status.Closed || position.Status == Status.Cancelled || position.Status == Status.Removed)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The information you are looking for is not available");
                }

                return View(candidate);
            }
            catch (InvalidOperationException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The designated candidate does not have a valid ID");
            }
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
                if (this.IsStateValid())
                {
                    if (CandidateHelper.Edit(AutoMapper.Mapper.Map<Candidate>(candidate)))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return View(candidate);
                    }

                }
                return View(candidate);
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
        public ActionResult New(NewCandidateViewModel candidate)
        {

            List<FileBlob> files = ((List<FileBlob>)Session["files"]);



            return new EmptyResult();
        }

    }
}