using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;

namespace Talento.Core
{
   public interface IPositionCandidate
    {
        List<PositionCandidate> GetCandidatesByPositionId(int? positionId);
        bool Create(Candidate candidate, Position position);
    }
}
