using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public int CandidateToPosition(Candidate candidate, Position position)
        {
            try
            {
                Db.Candidates.SingleOrDefault(x=>x.Id.Equals(candidate.Id)).Positions.Add(position);
                Db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                //Candidate already exists in specified position
                return -2;
            }
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

        public int Edit(Candidate log, List<FileBlob> files)
        {
            try
            {
                Db.Candidates.Single(x => x.Id == log.Id).Competencies = log.Competencies;
                Db.Candidates.Single(x => x.Id == log.Id).Description = log.Description;
                Db.Candidates.Single(x => x.Id == log.Id).Name = log.Name;
                Db.Candidates.Single(x => x.Id == log.Id).Status = log.Status;
                Db.Candidates.Single(x => x.Id == log.Id).IsTcsEmployee = log.IsTcsEmployee;

                Db.SaveChanges();

                Candidate candidate = Db.Candidates.Single(x => x.Id == log.Id);

                FileManagerHelper.RemoveAll(candidate);
                if (files != null)
                {
                    files.ForEach(x => FileManagerHelper.AddNewFile(x));
                }

                return 0;
            }
            catch (Exception)
            {
                throw;
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
