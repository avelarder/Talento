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


        public ActionResult AttachProfile(CandidateViewModel model)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);

            var callbackUrl = Url.Action("Details", "Position", new { positionId = 1 }, protocol: Request.Url.Scheme);

            tw.WriteLine("A profile has been added to " + "" + "by " + User.Identity.Name +
                            "please visit the following URL for more information: " + "http://"  + callbackUrl);

            tw.WriteLine("Recipients: ");

            foreach (ApplicationUser user in UserHelper.GetUsersForNewProfileMail())
            {
                tw.Write(user.Email + ", ");
            }
            tw.Flush();
            tw.Close();
            return File(ms.GetBuffer(), "application/octet-stream", "MailExample.txt");

#if DEBUG == false

            string currentUser = User.Identity.Name;
            SendEmailHelper.SendEmailProfile(currentUser);
#endif

        }


        public ActionResult InterviewFeedback(CandidateViewModel model)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);

            var callbackUrl = Url.Action("Details", "Position", new { positionId = 1 }, protocol: Request.Url.Scheme);

            tw.WriteLine("A profile's interview feedback form has been added to " + "position title" + "by " + User.Identity.Name +
                            "please visit the following URL for more information: " + "http://"  + callbackUrl);

            tw.WriteLine("Recipients: ");

            foreach (ApplicationUser user in UserHelper.GetUsersForNewProfileMail())
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
