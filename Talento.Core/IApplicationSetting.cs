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
        List<ApplicationParameter> GetAll();
        List<string> GetParameters(string prefix);
        List<ApplicationParameter> GetPagination(string orderBy, string filter);
        ApplicationSetting GetByName(string name);
        ApplicationParameter GetById(int id);
        void Create(ApplicationSetting aS);
        void Edit(ApplicationSetting aS);
    }
}
