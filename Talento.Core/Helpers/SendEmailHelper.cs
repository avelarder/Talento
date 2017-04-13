using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;
using System.Web;
using System.Web.Security;
using System.IO;

namespace Talento.Core.Helpers
{
    public class SendEmailHelper : BaseHelper, ISendEmail
    {
        public SendEmailHelper(Core.Data.ApplicationDbContext db): base(db)
        {

        }

        public void SendEmailProfile(string userMailFrom)
        {
#if DEBUG == false
            List<ApplicationUser> recipients = Db.Users.Where(x => x.Roles.Equals("TL") || x.Roles.Equals("TM") ||
                                                x.Roles.Equals("RMG") || x.Roles.Equals("TAG")).ToList();

            MailAddressCollection recipientsList = new MailAddressCollection();

            foreach (var item in recipients.ToString().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                MailAddress myToAddress = new MailAddress(item);
                recipientsList.Add(myToAddress);
            }


            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 465;
            SmtpServer.Credentials = new System.Net.NetworkCredential("apikey", "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");
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(userMailFrom);
            mail.To.Add("arturo.velarde@tcs.com");
            mail.Subject = "New profile added.";
            mail.Body = "A profile has been added to  /*Position Tittle*/  by  /* < User Name who added >*/ please visit the following URL for more information.";
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
#endif

        }

        public void SendEmailFeedback(string userMailFrom)
        {
#if DEBUG == false
            List<ApplicationUser> recipients = Db.Users.Where(x => x.Roles.Equals("RMG") || 
                                                                    x.Roles.Equals("TAG")).ToList();

            MailAddressCollection recipientsList = new MailAddressCollection();

            foreach (var item in recipients.ToString().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                MailAddress myToAddress = new MailAddress(item);
                recipientsList.Add(myToAddress);
            }

            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.net");
            SmtpServer.Port = 465;
            SmtpServer.Credentials = new System.Net.NetworkCredential("apikey", "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");
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(userMailFrom);
            mail.To.Add(recipientsList.ToString());
            mail.Subject = "New profile added.";
            mail.Body = "A profile's interview feedback form has been added to "<Position Tittle>" by <User Name who added> please visit the following URL for more information.";
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

#endif
        }

    }
}
