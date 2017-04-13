using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class FileManagerHelper : BaseHelper, IFileManagerHelper
    {
        ICandidate CandidateHelper;
        public FileManagerHelper(Core.Data.ApplicationDbContext db, ICandidate candidateHelper)
            : base(db)
        {
            CandidateHelper = candidateHelper;
        }

        public void Create(FileBlob file)
        {
            try
            {
                Db.FileBlobs.Add(file);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(FileBlob file)
        {
            try
            {
                Db.FileBlobs.Remove(file);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<FileBlob> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<FileBlob>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
