using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.Core
{
    public interface ISendEmail
    {
        void SendEmailProfile(string userSendEmail);
        void SendEmailFeedback(string userSendEmail);
    }
}
