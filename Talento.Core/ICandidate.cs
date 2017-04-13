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
        int Create(Candidate log, List<FileBlob> files);
        bool Edit(Candidate log);
        void Delete(int Id, string uId);
    }
}
