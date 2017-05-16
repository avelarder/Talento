using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Talento.Entities;

namespace Talento.Core
{
    public interface ICustomPagingList
    {
        List<Position> GetAdminTable(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page);
        List<Position> GetBasicTable(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page);
        string CreateXl();
    }
}
