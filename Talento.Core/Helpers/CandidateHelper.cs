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

        public void Create(Position log, string EmailModifier)
        {
            throw new NotImplementedException();
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
                //position.Tags = Tags.GetByPositionId(position.Id);

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
