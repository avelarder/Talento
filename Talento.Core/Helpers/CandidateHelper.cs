using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class CandidateHelper : BaseHelper, ICandidate
    {
        IPosition PositionHelper;
        ICustomUser UserHelper;
        IPositionLog LogHelper;

        public CandidateHelper(Core.Data.ApplicationDbContext db, IPositionLog logHelper, ICustomUser userHelper, IPosition positionHelper) : base(db)
        {
            PositionHelper = positionHelper;
            UserHelper = userHelper;
            LogHelper = logHelper;
        }

        public int Create(Candidate newCandidate)
        {
            try
            {
                using (var tx = new TransactionScope(TransactionScopeOption.Required))
                {
                    Position currentPosition = PositionHelper.Get(newCandidate.PositionCandidates.First().Position.PositionId);

                    foreach (PositionCandidates c in currentPosition.PositionCandidates)
                    {
                        //If Email is already in the position return -1
                        if (c.Candidate.Email.Trim().ToLower().Contains(newCandidate.Email.Trim().ToLower()))
                        {
                            return -1;
                        }
                    }
                    Db.Candidates.Add(newCandidate);
                    Log log = new Log
                    {
                        Action = Entities.Action.Edit,
                        ActualStatus = currentPosition.Status,
                        User = newCandidate.CreatedBy,
                        Date = DateTime.Now,
                        Description = String.Format("Candidate {0} was attached to the position", newCandidate.Email),
                        PreviousStatus = currentPosition.Status,
                        Position = currentPosition
                    };

                    LogHelper.Add(log);
                    currentPosition.OpenStatus = PositionOpenStatus.Screening;
                    int result = Db.SaveChanges();
                    tx.Complete();
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Edit(Candidate editCandidate, HashSet<FileBlob> files, ApplicationUser currentUser)
        {
            try
            {
                Db.Candidates.Single(x => x.CandidateId == editCandidate.CandidateId).Competencies = editCandidate.Competencies;
                Db.Candidates.Single(x => x.CandidateId == editCandidate.CandidateId).Description = editCandidate.Description;
                Db.Candidates.Single(x => x.CandidateId == editCandidate.CandidateId).Name = editCandidate.Name;
                Db.Candidates.Single(x => x.CandidateId == editCandidate.CandidateId).IsTcsEmployee = editCandidate.IsTcsEmployee;
                List<FileBlob> actualFiles = Db.Candidates.Single(x => x.CandidateId == editCandidate.CandidateId).FileBlobs.ToList();
                actualFiles.ForEach(f => Db.Candidates.Single(x => x.CandidateId == editCandidate.CandidateId).FileBlobs.Remove(f));

                Db.SaveChanges();
                Db.Candidates.Single(x => x.CandidateId == editCandidate.CandidateId).FileBlobs = files;

                Db.SaveChanges();

                Position positionToLog = editCandidate.PositionCandidates.Last().Position;

                Log log = new Log
                {
                    Action = Entities.Action.Edit,
                    ActualStatus = positionToLog.Status,
                    User = currentUser,
                    Date = DateTime.Now,
                    Description = String.Format("Candidate {0} has been updated.", editCandidate.Email),
                    PreviousStatus = positionToLog.Status,
                    Position = positionToLog
                };

                LogHelper.Add(log);
                Db.SaveChanges();

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
                var candidate = Db.Candidates.Single(x => x.CandidateId == Id);
                return candidate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TechnicalInterview> GetCandidateComments(int CandidateId)
        {
            try
            {
                return Db.TechnicalInterviews.Where(x => x.PositionCandidate.CandidateID.Equals(CandidateId)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddTechnicalInterview(TechnicalInterview technicalInterview, ApplicationUser currentUser, int positionId, string candidateEmail)
        {
            try
            {
                using (var tx = new TransactionScope(TransactionScopeOption.Required))
                {
                    int candidateId = Db.Candidates.Where(x => x.Email.Equals(candidateEmail)).Select(y=>y.CandidateId).ToList().FirstOrDefault(z=> {
                        return Db.PositionCandidates.Where(a => a.PositionID.Equals(positionId)).Select(b => b.CandidateID).ToList().Contains(z);
                    });
                    technicalInterview.PositionCandidate = Db.PositionCandidates.FirstOrDefault(x => x.CandidateID.Equals(candidateId) && x.PositionID.Equals(positionId));
                    Db.TechnicalInterviews.Add(technicalInterview);
                    Db.Candidates.FirstOrDefault(x => x.CandidateId.Equals(candidateId)).FileBlobs.Add(technicalInterview.FeedbackFile);
                    Log log = new Log
                    {
                        Action = Entities.Action.Edit,
                        ActualStatus = technicalInterview.PositionCandidate.Position.Status,
                        User = currentUser,
                        Date = DateTime.Now,
                        Description = String.Format("A new technical interview feedback was added for {0}", technicalInterview.PositionCandidate.Candidate.Email),
                        PreviousStatus = technicalInterview.PositionCandidate.Position.Status,
                        Position = technicalInterview.PositionCandidate.Position
                    };

                    if (technicalInterview.IsAccepted)
                    {
                        technicalInterview.PositionCandidate.Status = PositionCandidatesStatus.Interview_Accepted;
                    }
                    else
                    {
                        technicalInterview.PositionCandidate.Status = PositionCandidatesStatus.Interview_Rejected;
                    }

                    LogHelper.Add(log);
                    int result = Db.SaveChanges();
                    tx.Complete();
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ChangeStatus(int Id, PositionCandidatesStatus newStatus, ApplicationUser currentUser)
        {
            try
            {
                PositionCandidates pc = Db.PositionCandidates.Single(x => x.CandidateID == Id);
                // Check if Update Status is Valid Conditions
                if (pc.Status == PositionCandidatesStatus.Interview_Accepted || pc.Status == PositionCandidatesStatus.Conditional_Offer_Negotiating)
                {
                    if (newStatus != PositionCandidatesStatus.Conditional_Offer_Accepted && newStatus != PositionCandidatesStatus.Conditional_Offer_Rejected && newStatus != PositionCandidatesStatus.Conditional_Offer_Negotiating)
                    {
                        throw new Exception();
                    }
                }
                else if (pc.Status == PositionCandidatesStatus.Conditional_Offer_Accepted)
                {
                    if (newStatus != PositionCandidatesStatus.Customer_Approved && newStatus != PositionCandidatesStatus.Customer_Rejected)
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }

                Candidate candidateToLog = pc.Candidate;
                Position positionToLog = pc.Position;

                pc.Status = newStatus;
                var name = Enum.GetName(typeof(PositionCandidatesStatus), newStatus).Replace("_", " ");

                Log log = new Log
                {
                    Action = Entities.Action.Edit,
                    ActualStatus = positionToLog.Status,
                    User = currentUser,
                    Date = DateTime.Now,
                    Description = String.Format("Candidate {0} Status has been updated to {1}.", candidateToLog.Email, name),
                    Position = positionToLog,
                    PreviousStatus = positionToLog.Status
                };

                LogHelper.Add(log);

                Db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
