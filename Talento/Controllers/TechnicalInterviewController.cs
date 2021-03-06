﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talento.Core;
using Talento.Entities;
using Talento.Models;

namespace Talento.Controllers
{
    [HandleError]
    [Authorize(Roles = "TAG, RMG, PM, TL")]
    public class TechnicalInterviewController : Controller
    {
        IPosition PositionHelper;
        ICustomUser UserHelper;
        ICandidate CandidateHelper;
        IComment CommentHelper;

        public TechnicalInterviewController(Core.IPosition positionHelper, Core.ICandidate candidateHelper, ICustomUser userHelper, IComment commentHelper)
        {
            CommentHelper = commentHelper;
            PositionHelper = positionHelper;
            CandidateHelper = candidateHelper;
            UserHelper = userHelper;
        }

        //Call this action when a feedback interview change status
        public ActionResult InterviewFeedbackNewStatus(string candidateEmail, Position toapply, List<ApplicationUser> recipients)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            string callbackUrl = Url.Action("Details", "Positions", new { id = toapply.PositionId }, protocol: Request.Url.Scheme);
            string body = candidateEmail + " has been accepted for " + toapply.Title + ", reported by " + User.Identity.Name + "." +
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

        public ActionResult InterviewFeedback(Position toapply, List<ApplicationUser> recipients)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            var callbackUrl = Url.Action("Details", "Positions", new { id = toapply.PositionId }, protocol: Request.Url.Scheme);
            string body = " A profile's interview feedback form has been added to " + toapply.Title + " by " + User.Identity.Name + "." +
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

        // GET: 
        public ActionResult NewTechnicalInterview(string candidateEmail, int positionId, int candidateId)
        {
            if (PositionHelper.Get(positionId).Status.Equals(PositionStatus.Open))
            {
                return View(new CreateTechnicalInterviewViewModel()
                {
                    CandidateEmail = candidateEmail,
                    PositionId = positionId,
                    CandidateId = candidateId
                });
            }
            else
            {
                return RedirectToAction("Details", "Positions", new { id = positionId });
            }
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewTechnicalInterview(CreateTechnicalInterviewViewModel model)
        {
            PositionCandidates positionCandidate = PositionHelper.Get(model.PositionId).PositionCandidates.Where(x => x.Candidate.Email.Contains(model.CandidateEmail)).Single();
            TechnicalInterview newTechnicalInterview = new TechnicalInterview()
            {
                Date = model.Date,
                FeedbackFile = new FileBlob() { Blob = new BinaryReader(model.File.InputStream).ReadBytes(model.File.ContentLength), FileName = model.CandidateEmail.Split('@')[0] + "_" + model.Date.Year + model.Date.Month + model.Date.Day +"_"+ model.File.FileName },
                InterviewerId = model.InterviewerId + "",
                InterviewerName = model.InterviewerName,
                IsAccepted = model.Result,
            };

            CandidateHelper.AddTechnicalInterview(newTechnicalInterview, UserHelper.GetUserByEmail(User.Identity.Name), model.PositionId, model.CandidateEmail);
            CommentHelper.Create(new Comment { CandidateId = model.CandidateId, Content = "Technical Interview Comment: \""+model.Comment+"\" by " +model.InterviewerName+". \n" ,PositionId = model.PositionId,User = UserHelper.GetUserByEmail(User.Identity.Name)});

            if (model.Result)
            {
                return InterviewFeedbackNewStatus(model.CandidateEmail, PositionHelper.Get(model.PositionId), UserHelper.GetByRoles(new List<string>() { "RMG", "TAG" }));
            }
            else
            {
                return InterviewFeedback(PositionHelper.Get(model.PositionId), UserHelper.GetByRoles(new List<string>() { "RMG", "TAG" }));
            }
        }
    }
}