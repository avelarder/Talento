using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class PositionCandidateHelper : BaseHelper, IPositionCandidate
    {
        public PositionCandidateHelper(Core.Data.ApplicationDbContext db) : base(db)
        {
        }

        public List<PositionCandidate> GetCandidatesByPositionId(int? positionId)
        {
            var positionCandidates = Db.PositionsCandidates.Where(x => x.Position_Id == positionId).OrderByDescending(t => t.Candidate.CratedOn).ToList();

            return positionCandidates;
        }
    }
}
