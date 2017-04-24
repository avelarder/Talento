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
        IPagedList<ApplicationParameter> GetPagination(int pageSize, int page, string orderBy, string filter);
        ApplicationSetting Get(string id);
        void Create(ApplicationSetting aS);
    }
}
