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

        private int CandidateToPosition(Candidate candidate, Position position)
        {
            try
            {
                Db.Candidates.SingleOrDefault(x => x.Id.Equals(candidate.Id)).Positions.Add(position);
                Db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                //Candidate already exists in specified position
                return -2;
            }
        }

        public int Create(Candidate newCandidate)
        {
            try
            {
                using (var tx = new TransactionScope(TransactionScopeOption.Required))
                {
                    Position currentPosition = PositionHelper.Get(newCandidate.Positions.First().Id);

                    foreach (Candidate c in currentPosition.Candidates)
                    {
                        //If Email is already in the position return -1
                        if (c.Email.Trim().ToLower().Contains(newCandidate.Email.Trim().ToLower()))
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
                    };

                    currentPosition.Logs.Add(log);
                    currentPosition.OpenStatus = OpenStatus.Screening;
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

        public void Delete(int Id, string uId)
        {
            throw new NotImplementedException();
        }

        public int Edit(Candidate editCandidate, HashSet<FileBlob> files, ApplicationUser currentUser)
        {
            try
            {
                Db.Candidates.Single(x => x.Id == editCandidate.Id).Competencies = editCandidate.Competencies;
                Db.Candidates.Single(x => x.Id == editCandidate.Id).Description = editCandidate.Description;
                Db.Candidates.Single(x => x.Id == editCandidate.Id).Name = editCandidate.Name;
                Db.Candidates.Single(x => x.Id == editCandidate.Id).Status = editCandidate.Status;
                Db.Candidates.Single(x => x.Id == editCandidate.Id).IsTcsEmployee = editCandidate.IsTcsEmployee;
                Db.Candidates.Single(x => x.Id == editCandidate.Id).FileBlobs.Clear();
                Db.Candidates.Single(x => x.Id == editCandidate.Id).FileBlobs = files;

                Db.SaveChanges();

                Position positionToLog = editCandidate.Positions.Last();

                Log log = new Log
                {
                    Action = Entities.Action.Edit,
                    ActualStatus = positionToLog.Status,
                    User = currentUser,
                    Date = DateTime.Now,
                    Description = String.Format("Candidate {0} has been updated.", editCandidate.Email),
                    PreviousStatus = positionToLog.Status
                };

                PositionHelper.Get(positionToLog.Id).Logs.Add(log);

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
