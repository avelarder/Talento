using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core
{
    public interface IApplicationSetting
    {
        List<ApplicationSetting> GetAll();
        List<ApplicationSetting> GetParameters(string prefix);
        ApplicationSetting Get(string id);
        void Create(ApplicationSetting aS);
    }
}
