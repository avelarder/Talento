using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talento.Entities;
using Talento.Core.Utilities;

namespace Talento.Core
{
    public interface IPositionLog
    {
        List<PositionLog> GetAll(int? Id);
        void Create(PositionLog log);
        Tuple<List<PositionLog>, Pagination> PaginateLogs(int? Id, int page, int pageSize, string url);
    }
}
