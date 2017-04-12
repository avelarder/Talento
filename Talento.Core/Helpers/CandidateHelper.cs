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

        public CandidateHelper(Core.Data.ApplicationDbContext db, ICustomUser userHelper, IPosition positionHelper) : base(db)
        {
            PositionHelper = positionHelper;
            UserHelper = userHelper;
        }

        public void Create(Position log, string EmailModifier)
        {
            throw new NotImplementedException();
        }

        public void Delete(int Id, string uId)
        {
            throw new NotImplementedException();
        }

        public bool Edit(Position log, string EmailModifier)
        {
            throw new NotImplementedException();
        }

        public Task<Candidate> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Candidate>> GetAll()
        {
            return await Db.Candidates.ToListAsync();
        }
    }
}
