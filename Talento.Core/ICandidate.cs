using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core
{
    public interface ICandidate
    {
        Candidate Get(int Id);
        Task<List<Candidate>> GetAll();
        void Create(Position log, string EmailModifier);
        bool Edit(Candidate log);
        void Delete(int Id, string uId);
    }
}
