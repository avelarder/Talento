using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core
{
    public interface IFileManagerHelper
    {
        List<FileBlob> GetAll(Candidate owner);
        void RemoveAll(Candidate owner);
        void Create(FileBlob file);
        void Delete(FileBlob file);
        void CleanCandidateFiles(Candidate owner);
        void AddNewFile(FileBlob owner);
    }
}
