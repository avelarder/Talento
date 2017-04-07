using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core
{
    public interface IPosition
    {
        Position Get(int Id);
        Task<List<Position>> GetAll();
        void Create(Position log,string EmailModifier);
        bool Edit(Position log, string EmailModifier);
        void Delete(int Id, string uId);
        ApplicationUser SearchPM(string userName);
        ApplicationUser GetUser(string user);
    }
}
