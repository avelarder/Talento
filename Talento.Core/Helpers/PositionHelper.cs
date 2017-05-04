using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;
using System.Data;
using System.Data.Entity;
using Talento.Core.Data;
using Talento.Core.Utilities;

namespace Talento.Core.Helpers
{
    public class PositionHelper : BaseHelper, IPosition
    {
        public PositionHelper(Core.Data.ApplicationDbContext db) : base(db)
        {
        }

        public void Create(Position position)
        {
            string description = string.Format("Position Created by {0} at {1}", position.Owner.Email, DateTime.Now.ToShortDateString());
            Log CreateLog = new Log()
            {
                Action = Entities.Action.Create,
                ActualStatus = position.Status,
                PreviousStatus = 0,
                Description = description,
                Date = DateTime.Now,
                ApplicationUser_Id = position.ApplicationUser_Id
            };

            position.Logs = new List<Log>
            {
                CreateLog
            };

            Db.Positions.Add(position);
            Db.SaveChanges();
        }

        public void Delete(int Id, string uId)
        {
            ApplicationUser cu = Db.Users.Single(u => u.Id.Equals(uId)); //Get Current User
            var p = Db.Positions.Where(x => x.PositionId == Id).Single();
            // Add Log to Position
            string description = string.Format("Position Deleted by {0} at {1}", cu.Email, DateTime.Now.ToShortDateString());
            Log log = new Log()
            {
                Date = DateTime.Now,
                User = cu,
                Action = Entities.Action.Delete,
                PreviousStatus = p.Status,
                Description = description,
                ActualStatus = PositionStatus.Removed,
                ApplicationUser_Id = cu.Id
            };
            p.Logs.Add(log);
            p.Status = PositionStatus.Removed;
            Db.SaveChanges();
        }

        public bool Edit(Position log, ApplicationUser modifier)
        {
            try
            {
                //Obtaining the position in its original state
                Position position = Db.Positions.Single(p => p.PositionId == log.PositionId);
                //And obtaining the user that is modifying the Position
                var previousStatus = position.Status;
                //Modifying the position info from the edit form
                position.Area = log.Area;
                position.Status = log.Status;
                position.Title = log.Title;
                position.Description = log.Description;
                position.EngagementManager = log.EngagementManager;
                position.RGS = log.RGS;
                position.ApplicationUser_Id = position.ApplicationUser_Id;
                position.PortfolioManager_Id = position.PortfolioManager_Id;
                position.Owner = position.Owner;
                position.PortfolioManager = position.PortfolioManager;

                switch (position.Status = log.Status)
                {
                    case PositionStatus.Cancelled:
                        position.LastCancelledDate = DateTime.Now;
                        position.LastCancelledBy = modifier;
                        break;
                    case PositionStatus.Open:
                        position.LastOpenedDate = DateTime.Now;
                        position.LastOpenedBy = modifier;
                        break;
                    case PositionStatus.Closed:
                        position.LastClosedDate = DateTime.Now;
                        position.LastClosedBy = modifier;
                        break;

                }
                string description = string.Format("Position Edited by {0} at {1}", modifier.Email, DateTime.Now.ToShortDateString());
                Log CreateLog = new Log()
                {
                    Action = Entities.Action.Edit,
                    ActualStatus = log.Status,
                    PreviousStatus = previousStatus,
                    Description = description,
                    Date = DateTime.Now,
                    ApplicationUser_Id = modifier.Id,
                    User = modifier,

                };
                position.Logs.Add(CreateLog);
                Db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Position Get(int Id)
        {
            var position = Db.Positions
                .Include("Owner")
                .Include("PortfolioManager")
                .Single(x => x.PositionId == Id);

            return position;
        }

        public async Task<List<Position>> GetAll()
        {
            return await Db.Positions.ToListAsync();
        }

        public bool DeleteCandidate(Position position, Candidate candidate, ApplicationUser modifier)
        {
            try
            {
                PositionCandidates positionCandidate = Db.PositionCandidates.Where(pc => pc.CandidateID == candidate.CandidateId && pc.PositionID == position.PositionId).Single();
                positionCandidate.Status = PositionCandidatesStatus.Mannualy_Removed;

                Log log = new Log()
                {
                    Action = Entities.Action.Edit,
                    ActualStatus = positionCandidate.Position.Status,
                    PreviousStatus = positionCandidate.Position.Status,
                    Description = String.Format("Candidate {0} has been removed from position.", candidate.Email),
                    Date = DateTime.Now,
                    ApplicationUser_Id = modifier.Id,
                    User = modifier,
                };
                positionCandidate.Position.Logs.Add(log);

                Db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        #region PositionLogs
        public Tuple<List<Log>, Pagination> PaginateLogs(List<Log> logs, int page = 1, int pageSize = 5, string url = "#")
        {
            try
            {
                // Count of PositionsLogs
                int totalCount = logs.Count();
                // Count of Pages
                int totalPages = (totalCount - 1) / pageSize + 1;
                // Null if page requested doesnt exist
                if (page > totalPages || page < 1)
                {
                    return null;
                }
                // PositionsLogs to Skip : [PageSize] 12 * ([CurrentPage] 2  - [SkipPreviousPageAlways] 1)
                int skipLogs = pageSize * (page - 1);
                // If pagination is necessary 
                bool paginate = skipLogs < totalCount;

                if (paginate)
                {
                    logs = logs.Skip(skipLogs).Take(pageSize).ToList();
                }
                // Create Pagination for the List of PositionsLogs
                Pagination pagination = new Pagination()
                {
                    Prev = (page > 1) ? (page - 1) : 0,
                    Next = (page < totalPages) ? (page + 1) : 0,
                    Current = page,
                    Total = totalPages,
                    Url = url
                };
                // Populate tuple
                Tuple<List<Log>, Pagination> retu = new Tuple<List<Log>, Pagination>(logs.ToList(), pagination);
                return retu;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
