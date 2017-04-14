﻿using System;
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
        ICustomUser UserHelper;
        IPositionCandidate PositionsCandidatesHelper;
        IFileManagerHelper FileManagerHelper;

        public CandidateController(ICandidate candidateHelper, ICustomUser userHelper, IPositionCandidate positionsCandidatesHelper, IFileManagerHelper fileManagerHelper)
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

        public ActionResult AttachProfile(CandidateViewModel model, Position toApply)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            var callbackUrl = Url.Action("Details", "Positions", new { toApply.Id }, protocol: Request.Url.Scheme);

            tw.WriteLine("A profile has been added to " + toApply.Title + " by " + User.Identity.Name + " . " +
                            "Please visit the following URL for more information: " + callbackUrl);
            tw.Write("Recipients: ");

            foreach (ApplicationUser user in UserHelper.GetUsersForNewProfileMail())
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
        public ActionResult InterviewFeedback(CandidateViewModel model, Position toapply)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);

            var callbackUrl = Url.Action("Details", "Positions", new { toapply.Id }, protocol: Request.Url.Scheme);

            tw.WriteLine("A profile's interview feedback form has been added to " + toapply.Title  + " by " + User.Identity.Name + "." +
                            " Please visit the following URL for more information: " + callbackUrl);

            tw.Write("Recipients: ");

            foreach (ApplicationUser user in UserHelper.GetUsersForNewFeedbackMail())
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

    }
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
        public ActionResult New(CandidateModel candidate)
        {

            List<FileBlob> files = ((List<FileBlob>)Session["files"]);



            return new EmptyResult();
        }

    }
}
