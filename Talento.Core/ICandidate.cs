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
        int Edit(Candidate log, List<FileBlob> files);
        void Delete(int Id, string uId);
    }
}
