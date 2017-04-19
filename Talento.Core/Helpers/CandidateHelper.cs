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

        public int Create(Candidate newCandidate, List<FileBlob> files)
        {
            try
            {
                if (Db.Candidates.Any(x => x.Email.Equals(newCandidate.Email)))
                {
                    return -1;
                }
                else
                {
                    Db.Candidates.Add(newCandidate);
                    if (files != null)
                    {
                        files.ForEach(f =>
                        {
                            FileManagerHelper.AddNewFile(f);
                        });
                    }

                    Position positionToLog = newCandidate.Positions.Last();

                    Log log = new Log
                    {
                        Action = Entities.Action.Edit,
                        ActualStatus = positionToLog.Status,
                        User = newCandidate.CreatedBy,
                        Date = DateTime.Now,
                        Description = String.Format("Candidate {0} was attached to the position", newCandidate.Email),
                        PreviousStatus = positionToLog.Status
                    };

                    PositionHelper.Get(positionToLog.Id).Logs.Add(log);

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

        public int Edit(Candidate editCandidate, List<FileBlob> files, ApplicationUser currentUser)
        {
            try
            {
                Db.Candidates.Single(x => x.Id == editCandidate.Id).Competencies = editCandidate.Competencies;
                Db.Candidates.Single(x => x.Id == editCandidate.Id).Description = editCandidate.Description;
                Db.Candidates.Single(x => x.Id == editCandidate.Id).Name = editCandidate.Name;
                Db.Candidates.Single(x => x.Id == editCandidate.Id).Status = editCandidate.Status;
                Db.Candidates.Single(x => x.Id == editCandidate.Id).IsTcsEmployee = editCandidate.IsTcsEmployee;

                Db.SaveChanges();

                Candidate candidate = Db.Candidates.Single(x => x.Id == editCandidate.Id);

                FileManagerHelper.RemoveAll(candidate);
                if (files != null)
                {
                    files.ForEach(x => FileManagerHelper.AddNewFile(x));
                }

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
