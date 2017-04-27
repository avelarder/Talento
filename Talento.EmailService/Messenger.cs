using System.Collections.Generic;
using System.Net.Mail;

namespace Talento.EmailManager
{
    public class Messenger : IMessenger
    {
        SmtpClient smtpServer = new SmtpClient("smtp.sendgrid.net", 465);
        string key = "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";

        public void SendEmail(List<string> toEmail, string fromEmail, List<string> bcc,List<string> cc, string subject, string body)
        {
            MailMessage mMailMessage = new MailMessage();

            if (!string.IsNullOrEmpty(fromEmail))
            {
                mMailMessage.From = new MailAddress(fromEmail);
            }
            foreach (string s in toEmail)
            {
                mMailMessage.To.Add(new MailAddress(s));
            }
            
            foreach (string s in bcc)
            {
                mMailMessage.Bcc.Add(new MailAddress(s));
            }
            
            foreach (string s in cc)
            {
                mMailMessage.CC.Add(new MailAddress(s));
            }
            
            if (!string.IsNullOrEmpty(subject))
            {
                mMailMessage.Subject = "Talento Web Application Notification";
            }
            else
            {
                mMailMessage.Subject = subject;
            }

            mMailMessage.Body = body;
            mMailMessage.IsBodyHtml = false;
            mMailMessage.Priority = MailPriority.Normal;
            smtpServer.Credentials = new System.Net.NetworkCredential("apikey", key);
            smtpServer.UseDefaultCredentials = false;
            smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpServer.Send(mMailMessage);
        }

        public void SendConfirmationEmail(string toEmail, string subject, string body)
        {
            MailMessage mMailMessage = new MailMessage();
            mMailMessage.To.Add(new MailAddress(toEmail));
            if (!string.IsNullOrEmpty(subject))
            {
                mMailMessage.Subject = "Talento Web Application Notification";
            }
            else
            {
                mMailMessage.Subject = subject;
            }

            mMailMessage.Body = body;
            mMailMessage.IsBodyHtml = false;
            mMailMessage.Priority = MailPriority.Normal;
            smtpServer.Credentials = new System.Net.NetworkCredential("apikey", key);
            smtpServer.UseDefaultCredentials = false;
            smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpServer.Send(mMailMessage);
        }
    }
}
