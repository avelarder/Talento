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
    public class CandidateController : Controller
    {
        ICustomUser UserHelper;
        ISendEmail SendEmailHelper;
       
        public CandidateController(ICustomUser userHelper, ISendEmail sendEmailHelper)
        {
            SendEmailHelper = sendEmailHelper;
            UserHelper = userHelper;
        }

        public ActionResult SimulateAttachProfile()
        {
            CandidateViewModel model = new CandidateViewModel()
            {

            };

            Position toApply = new Position()
            {
                Id = 5,
                Title = "THE TITLE"
            };

            return InterviewFeedback(model,toApply);
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
