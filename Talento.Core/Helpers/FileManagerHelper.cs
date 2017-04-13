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
        public FileManagerHelper(Core.Data.ApplicationDbContext db)
            : base(db)
        {
        }

        public void AddNewFile(FileBlob file)
        {
            try
            {
                Db.FileBlobs.Add(file);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public void CleanCandidateFiles(Candidate owner)
        {
            try
            {
                Db.FileBlobs.RemoveRange(Db.FileBlobs.Where(x => x.Candidate.Equals(owner)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Create(FileBlob file)
        {
            try
            {   
                Db.FileBlobs.Add(file);
                Db.SaveChanges();
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
                Db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAll(Candidate owner)
        {
            try { 
            Db.FileBlobs.RemoveRange(Db.FileBlobs.Where(x => x.Candidate.Equals(owner)));
            Db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public List<FileBlob> GetAll(Candidate owner)
        {
            try
            {
                return Db.FileBlobs.Where(x => x.Candidate.Equals(owner)).ToList();                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
