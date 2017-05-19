using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Talento.Core.Data;
using Talento.Entities;

namespace Talento.Core.Helpers
{
    public class CommentHelper : BaseHelper,IComment
    {
        IPositionLog LogHelper;
        ICandidate CandidateHelper;
        IPosition PositionHelper;

        public CommentHelper(Core.Data.ApplicationDbContext db, IPositionLog logHelper, IPosition positionHelper, ICandidate candidateHelper) : base(db)
        {
            LogHelper = logHelper;
            PositionHelper = positionHelper;
            CandidateHelper = candidateHelper;
        }

        /// <summary>
        /// Create a new comment either for a position or a candidate within in a position.
        /// </summary>
        /// <param name="newComment"></param>
        /// <returns></returns>
        public int Create(Comment newComment)
        {
            if (newComment.Content.Length <= 500)
            {
                using (var tx = new TransactionScope(TransactionScopeOption.Required))
                {
                    newComment.Date = DateTime.Now;
                    Position related = PositionHelper.Get(newComment.PositionId);
                    string logdescription;

                    if (newComment.CandidateId == null)
                    {
                        logdescription = string.Format("Comment Created by {0} at {1}", newComment.User.Email, related.Title);
                    }
                    else
                    {
                        logdescription = string.Format("Comment Created by {0} at {1} in {2}", newComment.User.Email, related.Title, CandidateHelper.Get(newComment.CandidateId.Value).Name);
                    }

                    Db.Comments.Add(newComment);

                    Log CreateLog = new Log()
                    {
                        Action = Entities.Action.Create,
                        ActualStatus = related.Status,
                        PreviousStatus = related.Status,
                        Description = logdescription,
                        Date = DateTime.Now,
                        ApplicationUser_Id = newComment.UserId,
                        Position = related
                    };

                    LogHelper.Add(CreateLog);

                    int result = Db.SaveChanges();
                    tx.Complete();
                    return result;
                }
            }
            else
                return -1;
                
        }

        /// <summary>
        /// Get a list of comments of a candidate within a position.
        /// </summary>
        /// <param name="CandidateId"></param>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<Comment> Get(int CandidateId, int PositionId)
        {
            return Db.Comments.Where(x => x.CandidateId == CandidateId && x.PositionId == PositionId).ToList();
        }

        /// <summary>
        /// get a list of comments related to a position. Candidate is not required.
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<Comment> Get(int PositionId)
        {
            return Db.Comments.Where(x => x.CandidateId.Equals(null) && x.PositionId.Equals(PositionId)).ToList();
        }

        /// <summary>
        /// get a list of all comments of a position.
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<Comment> GetAll(int PositionId)
        {
            return Db.Comments.Where(x => x.PositionId.Equals(PositionId)).ToList();
        }
    }
}
