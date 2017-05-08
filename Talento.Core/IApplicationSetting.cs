using PagedList;
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
        List<string> GetParameters(string prefix);
        List<ApplicationSetting> GetPagination(string orderBy, string filter);
        ApplicationSetting GetByName(string name);
        ApplicationSetting GetById(int id);
        int Create(ApplicationSetting aS);
        int Edit(ApplicationSetting aS);
    }
}
