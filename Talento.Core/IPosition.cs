using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Core.Utilities;
using Talento.Entities;

namespace Talento.Core
{
    public interface IPosition
    {
        Position Get(int Id);
        Task<List<Position>> GetAll();
        void Create(Position log);
        bool Edit(Position log, ApplicationUser modifier);
        void Delete(int Id, string uId);
        Tuple<List<Log>, Pagination> PaginateLogs(List<Log> logs, int page, int pageSize, string url);
        bool DeleteCandidate(Position log, Candidate candidate, ApplicationUser modifier);
        void BeginScreening(Position position);
    }
}
