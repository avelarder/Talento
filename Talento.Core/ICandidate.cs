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
        int Create(Candidate log);
        int Edit(Candidate log, HashSet<FileBlob> files, ApplicationUser currentUser);
        void ChangeStatus(int Id, PositionCandidatesStatus newStatus, ApplicationUser currentUser);
        int AddTechnicalInterview(TechnicalInterview technicalInterview, ApplicationUser currentUser, int positionId, string candidateEmail);
    }
}
