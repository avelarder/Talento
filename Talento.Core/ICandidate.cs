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
        Task<List<Candidate>> GetAll();
        int Create(Candidate log);
        int Edit(Candidate log, HashSet<FileBlob> files, ApplicationUser currentUser);
        void Delete(int Id, string uId);
        PositionCandidates GetPositionCandidate(int Id);
        void ChangeStatus(int Id, PositionCandidatesStatus newStatus, ApplicationUser currentUser);
        int AddTechnicalInterview(TechnicalInterview technicalInterview, ApplicationUser currentUser, int positionId, string candidateEmail);
        List<TechnicalInterview> GetCandidateComments(int CandidateId);
    }
}
