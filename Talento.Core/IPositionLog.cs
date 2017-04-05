using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;
using System.Data;
using System.Data.Entity;

namespace Talento.Core
{
    public interface IPositionLog
    {
        Task<PositionLog> Get(int Id);
        List<PositionLog> GetAll(int? Id);
        void Create(PositionLog log);
        Task Edit(PositionLog log);
        Task Delete(int Id);

    }
}
