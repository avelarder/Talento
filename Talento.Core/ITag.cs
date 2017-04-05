using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core
{
    public interface ITag
    {
        Task<Tag> Get(int Id);
        List<Tag> GetByPositionId(int Id);
        Task<List<Tag>> GetAll();
        Task Create(Tag log);
        Task Edit(Tag log);
        Task Delete(int Id);
    }
}
