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
using System.Transactions;

namespace Talento.Core.Helpers
{
    public class PositionHelper : BaseHelper, IPosition
    {
        IPositionLog LogHelper;

        public PositionHelper(Core.Data.ApplicationDbContext db, IPositionLog logHelper) : base(db)
        {
            LogHelper = logHelper;
        }

        /// <summary>
        /// Create a new position. Status will be open.
        /// </summary>
        /// <param name="position"></param>
        public void Create(Position position)
        {
            using (var tx = new TransactionScope(TransactionScopeOption.Required))
            {
                string description = string.Format("Position Created by {0} at {1}", position.Owner.Email, DateTime.Now.ToShortDateString());

                Db.Positions.Add(position);

                Log CreateLog = new Log()
                {
                    Action = Entities.Action.Create,
                    ActualStatus = position.Status,
                    PreviousStatus = 0,
                    Description = description,
                    Date = DateTime.Now,
                    ApplicationUser_Id = position.ApplicationUser_Id,
                    Position = position
                };

                LogHelper.Add(CreateLog);

                Db.SaveChanges();
                tx.Complete();
            }
        }


        /// <summary>
        /// Delete a position. Status will be changed to removed
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="uId"></param>
        public void Delete(int Id, string uId)
        {
            using (var tx = new TransactionScope(TransactionScopeOption.Required))
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
                    ApplicationUser_Id = cu.Id,
                    Position = p
                };
                LogHelper.Add(log);
                p.Status = PositionStatus.Removed;
                Db.SaveChanges();
                tx.Complete();
            }
        }

        /// <summary>
        /// Edit a position
        /// </summary>
        /// <param name="log"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public bool Edit(Position log, ApplicationUser modifier)
        {
            try
            {
                using (var tx = new TransactionScope(TransactionScopeOption.Required))
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
                        Position = position
                    };
                    LogHelper.Add(CreateLog);
                    Db.SaveChanges();
                    tx.Complete();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get a position by its Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Position Get(int Id)
        {
            var position = Db.Positions
                .Include("Owner")
                .Include("PortfolioManager")
                .Single(x => x.PositionId == Id);

            return position;
        }

        /// <summary>
        /// Delete a candidate from a position. The relation status will change to Mannualy Removed
        /// </summary>
        /// <param name="position"></param>
        /// <param name="candidate"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public bool DeleteCandidate(Position position, Candidate candidate, ApplicationUser modifier)
        {
            try
            {
                using (var tx = new TransactionScope(TransactionScopeOption.Required))
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
                        Position = positionCandidate.Position
                    };

                    LogHelper.Add(log);

                    Db.SaveChanges();
                    tx.Complete();
                }
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
