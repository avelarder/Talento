using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;
using System.Data;
using System.Data.Entity;
using Talento.Core.Utilities;

namespace Talento.Core.Helpers
{
    public class PositionLogHelper : BaseHelper, IPositionLog
    {
        public PositionLogHelper(Data.ApplicationDbContext db) : base(db)
        {

        }

        public void Create(PositionLog log)
        {
            try
            {
                Db.PositionLogs.Add(log);
                Db.SaveChanges();

            } catch ( Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Task Edit(PositionLog log)
        {
            throw new NotImplementedException();
        }

        public Task<PositionLog> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public List<PositionLog> GetAll(int? Id)
        {
            try { 
                List<PositionLog> log = Db.PositionLogs
                    .Where(p => p.Position_Id == Id)
                    .OrderByDescending(p => p.Date)
                    .ToList();
                return log;

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
                
        }

        public Tuple<List<PositionLog>, Pagination> PaginateLogs(int? Id, int page = 1, int pageSize = 5, string url = "#")
        {
            try
            {
                // PositionLogs of the Current Position 
                var logs = Db.PositionLogs
                            .OrderByDescending(p => p.Date)
                            .Where(p => p.Position_Id == Id);
                // Count of PositionsLogs
                int totalCount = logs.Count();
                // Count of Pages
                int totalPages = (totalCount - 1) / pageSize + 1;
                // PositionsLogs to Skip : [PageSize] 12 * ([CurrentPage] 2  - [SkipPreviousPageAlways] 1)
                int skipLogs = pageSize * (page - 1);
                // If pagination is necessary 
                bool paginate = skipLogs < totalCount;


                if (paginate)
                {
                    logs = logs.Skip(skipLogs).Take(pageSize);
                }
                // Create Pagination for the List of PositionsLogs
                Pagination pagination = new Pagination()
                {
                    Prev = (page > 1) ? (page - 1) : 0,
                    Next = (page < totalPages) ? ( page +1 ) : 0,
                    Current = page,
                    Total = totalPages,
                    Url = url
                };
                // Populate tuple
                Tuple<List<PositionLog>, Pagination> retu = new Tuple<List<PositionLog>, Pagination>(logs.ToList(), pagination);

                return retu;
            }
            catch (Exception)
            {
                Tuple<List<PositionLog>, Pagination> error = new Tuple<List<PositionLog>, Pagination>(null,null);
                return error;
            }
        }

    }
}
