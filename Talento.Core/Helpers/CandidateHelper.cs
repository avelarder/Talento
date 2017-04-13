using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class CandidateHelper : BaseHelper, ICandidate
    {
        IPosition PositionHelper;
        ICustomUser UserHelper;
        IFileManagerHelper FileManagerHelper;

        public CandidateHelper(Core.Data.ApplicationDbContext db, ICustomUser userHelper, IPosition positionHelper, IFileManagerHelper fileManagerHelper) : base(db)
        {
            PositionHelper = positionHelper;
            UserHelper = userHelper;
            FileManagerHelper = fileManagerHelper;
        }

        public int Create(Candidate newCandidate, List<FileBlob> files)
        {
            try
            {
                if (Db.Candidates.Any(x=>x.Email.Equals(newCandidate.Email)))
                {
                    return -1;
                }
                else
                {
                    Db.Candidates.Add(newCandidate);
                    if (files != null)
                    {
                        files.ForEach(f => {
                            FileManagerHelper.AddNewFile(f);
                        });
                    }
                    return Db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void Delete(int Id, string uId)
        {
            throw new NotImplementedException();
        }

        public bool Edit(Candidate log)
        {
            try
            {
                var candidate = Db.Candidates.Single(x => x.Id == log.Id);

                candidate.Competencies = log.Competencies;
                candidate.Description = log.Description;
                candidate.Name = log.Name;
                candidate.Status = log.Status;

                Db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Candidate Get(int Id)
        {
            try
            {
                var candidate = Db.Candidates.Single(x => x.Id == Id);
                return candidate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Candidate>> GetAll()
        {
            return await Db.Candidates.ToListAsync();
        }
    }
}
