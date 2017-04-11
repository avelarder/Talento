using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;
using System.Web;
using System.Web.Security;

namespace Talento.Core.Helpers
{
    public class SendEmailHelper : BaseHelper, ISendEmail 
    {
        public SendEmailHelper(Core.Data.ApplicationDbContext db): base(db)
        {

        }

        public void SendEmail(string userMailFrom)
        {
            List<string> listEmailRoles = new List<string>();

            List<ApplicationRole> Roles = new List<ApplicationRole>();
            ApplicationUser a = new ApplicationUser();
            var userName = a.UserName;

            foreach (var item in Roles)
            {
                if (item.Name == "TAG" || item.Name == "RMG" || item.Name == "TM")
                {
                    listEmailRoles.Add(userName);
                }
            }


#if DEBUG == false
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.net");
            SmtpServer.Port = 465;
            SmtpServer.Credentials = new System.Net.NetworkCredential("apikey", "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");
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(userMailFrom);
            mail.To.Add(listEmailRoles.ToString());
            mail.Subject = "New profile added.";
            mail.Body = "A profile has been added to  /*Position Tittle*/  by  /* < User Name who added >*/ please visit the following URL for more information.";
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

#endif
        }
    }
}
