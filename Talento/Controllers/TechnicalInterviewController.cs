using Microsoft.AspNet.Identity;
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
    [Authorize(Roles = "PM, TL")]
    public class TechnicalInterviewController : Controller
    {
        IPosition PositionHelper;
        ICustomUser UserHelper;
        ICandidate CandidateHelper;

        public TechnicalInterviewController(Core.IPosition positionHelper, Core.ICandidate candidateHelper, ICustomUser userHelper)
        {
            PositionHelper = positionHelper;
            CandidateHelper = candidateHelper;
            UserHelper = userHelper;
        }

        //Call this action when a feedback interview change status
        private ActionResult InterviewFeedbackNewStatus(string candidateEmail, Position toapply, List<ApplicationUser> recipients)
        {
                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                string callbackUrl = Url.Action("Details", "Positions", new { id=toapply.PositionId }, protocol: Request.Url.Scheme);
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

        private ActionResult InterviewFeedback(Position toapply, List<ApplicationUser> recipients)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            var callbackUrl = Url.Action("Details", "Positions", new { id=toapply.PositionId }, protocol: Request.Url.Scheme);
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
        public ActionResult NewTechnicalInterview(string candidateEmail, int positionId)
        {
            if (PositionHelper.Get(positionId).Status.Equals(Status.Open))
            {
                return View(new CreateTechnicalInterviewViewModel()
                {
                    CandidateEmail = candidateEmail,
                    PositionId = positionId,

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
            TechnicalInterview newTechnicalInterview = new TechnicalInterview()
            {
                Comment = model.Comment,
                Date = model.Date,
                FeedbackFile = new FileBlob() { Blob = new BinaryReader(model.File.InputStream).ReadBytes(model.File.ContentLength), FileName = model.CandidateEmail.Split('@')[0] + "_" + model.Date.Year + model.Date.Month + model.Date.Day + ".doc" },
                InterviewerId = model.InterviewerId + "",
                InterviewerName = model.InterviewerName,
                IsAccepted = model.Result,
            };

            CandidateHelper.AddTechnicalInterview(newTechnicalInterview,UserHelper.GetUserByEmail(User.Identity.Name),model.PositionId,model.CandidateEmail);

            if (model.Result)
            {
                return InterviewFeedbackNewStatus(model.CandidateEmail, PositionHelper.Get(model.PositionId), UserHelper.GetByRoles(new List<string>() { "RMG", "TAG" }));
            }
            else
            {
                return InterviewFeedback(PositionHelper.Get(model.PositionId),UserHelper.GetByRoles(new List<string>(){"RMG","TAG"}));
            }
        }
    }
}