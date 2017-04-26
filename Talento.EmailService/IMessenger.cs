using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.EmailManager
{
    public interface IMessenger
    {
        void SendEmail(string toEmail, string fromEmail, string bcc, string cc, string subject, string body);
        void SendConfirmationEmail(string toEmail, string subject, string body);
    }
}
