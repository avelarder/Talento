using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core
{
    public interface ICustomUser
    {
        ApplicationUser SearchPM(string userName);
        ApplicationUser GetUser(string user);
    }
}
