using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Entities;
using Microsoft.Office.Core;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Talento.Core.Helpers
{
    public class DashboardPagingHelper : BaseHelper, ICustomPagingList
    {
        public DashboardPagingHelper(Talento.Core.Data.ApplicationDbContext _db) : base(_db)
        {
        }
        Excel.Application xl = new Excel.Application();
        Excel.Workbook xlworkBook;
        Excel.Worksheet xlSheet;
        object misValue = System.Reflection.Missing.Value;
        Excel.Range xlCellrange;

        public List<Position> GetAdminTable(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page)
        {
            //Keeping paging and sorting
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //Linq query that lists the positions
            var query = from p in Db.Positions
                        select p;

            //Filtering the positions by the parameters given
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                switch (FilterBy)
                {
                    case "Status":
                        query = query.Where(p => p.Status.ToString().Contains(searchString));
                        break;
                    case "Title":
                        query = query.Where(p => p.Title.Contains(searchString));
                        break;
                    case "Owner":
                        query = query.Where(p => p.Owner.UserName.ToString().Contains(searchString));
                        break;
                    case "EM":
                        query = query.Where(p => p.EngagementManager.Contains(searchString));
                        break;
                    default:
                        break;
                }
            }

            //Sorting the list by the heading parameter given
            switch (sortOrder)
            {
                case "title_desc":
                    query = query.OrderByDescending(p => p.Title);
                    break;
                case "Title":
                    query = query.OrderBy(p => p.Title);
                    break;
                case "date_asc":
                    query = query.OrderBy(p => p.CreationDate);
                    break;
                case "Status":
                    query = query.OrderBy(p => p.Status);
                    break;
                case "status_desc":
                    query = query.OrderByDescending(p => p.Status);
                    break;
                case "Id":
                    query = query.OrderBy(p => p.PositionId);
                    break;
                case "id_desc":
                    query = query.OrderByDescending(p => p.PositionId);
                    break;
                case "EM":
                    query = query.OrderBy(p => p.EngagementManager);
                    break;
                case "em_desc":
                    query = query.OrderByDescending(p => p.EngagementManager);
                    break;
                case "Owner":
                    query = query.OrderBy(p => p.Owner.UserName);
                    break;
                case "owner_desc":
                    query = query.OrderByDescending(p => p.Owner.UserName);
                    break;

                default:  // Date descending 
                    query = query.OrderByDescending(p => p.CreationDate);
                    break;
            }

            int pageNumber = (page ?? 1);
            return query.ToList();
        }

        public List<Position> GetBasicTable(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page)
        {
            //Keeping paging and sorting
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //Linq query that lists the positions
            var query = from p in Db.Positions
                        where p.Status != PositionStatus.Removed
                        select p;

            //Filtering the positions by the parameters given
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                switch (FilterBy)
                {
                    case "Status":
                        query = query.Where(p => p.Status.ToString().Contains(searchString));
                        break;
                    case "Title":
                        query = query.Where(p => p.Title.Contains(searchString));
                        break;
                    case "Owner":
                        query = query.Where(p => p.Owner.UserName.ToString().Contains(searchString));
                        break;
                    case "EM":
                        query = query.Where(p => p.EngagementManager.Contains(searchString));
                        break;
                    case "PM":
                        query = query.Where(p => p.PortfolioManager.UserName.Contains(searchString));
                        break;
                    default:
                        break;
                }
            }

            //Sorting the list by the heading parameter given
            switch (sortOrder)
            {
                case "title_desc":
                    query = query.OrderByDescending(p => p.Title);
                    break;
                case "Title":
                    query = query.OrderBy(p => p.Title);
                    break;
                case "date_asc":
                    query = query.OrderBy(p => p.CreationDate);
                    break;
                case "Status":
                    query = query.OrderBy(p => p.Status);
                    break;
                case "status_desc":
                    query = query.OrderByDescending(p => p.Status);
                    break;
                case "EM":
                    query = query.OrderBy(p => p.EngagementManager);
                    break;
                case "em_desc":
                    query = query.OrderByDescending(p => p.EngagementManager);
                    break;
                case "Owner":
                    query = query.OrderBy(p => p.Owner.UserName);
                    break;
                case "owner_desc":
                    query = query.OrderByDescending(p => p.Owner.UserName);
                    break;
                case "Id":
                    query = query.OrderBy(p => p.PositionId);
                    break;
                case "id_desc":
                    query = query.OrderByDescending(p => p.PositionId);
                    break;

                default:  // Date ascending 
                    query = query.OrderByDescending(p => p.CreationDate);
                    break;
            }
            int pageNumber = (page ?? 1);
            return query.ToList();
        }

        public string CreateXl(string sortOrder, string FilterBy, string currentFilter, string searchString, int? page)
        {
            try
            {
                //Format the excel
                xlCellrange = xlSheet.get_Range("a1");
                xlCellrange.EntireRow.Font.Bold = true;
                xlCellrange.EntireRow.Font.Size = 18;
                xlCellrange.EntireRow.Font.Color = ConsoleColor.DarkGreen;
                xlCellrange.Interior.Color = ConsoleColor.Gray;


                List<Position> ListToExport = GetBasicTable(sortOrder, FilterBy, currentFilter, searchString, page);
                xl.Visible = false;
                xl.DisplayAlerts = false;
                xlworkBook = xl.Workbooks.Add(misValue);
                xlSheet = (Excel.Worksheet)xlworkBook.ActiveSheet;
                xlSheet.Name = "Open Positions";
                xlSheet.Cells[1, 1] = "Area";
                xlSheet.Cells[1, 2] = "Position Skill/Role";
                xlSheet.Cells[1, 3] = "Local Manager";
                xlSheet.Cells[1, 4] = "Onsite contact";
                xlSheet.Cells[1, 5] = "RGS ID";
                xlSheet.Cells[1, 6] = "Status on Position";
                xlSheet.Cells[1, 7] = "Created Date";
                xlSheet.Cells[1, 8] = "Days Open";
                xlSheet.Cells[1, 9] = "Candidates";
                xlSheet.Cells[1, 10] = "Comments";
                int count = 2;
                int OpenDays = 0;
                foreach (Position p in ListToExport)
                {
                    xlSheet.Cells[count, 1] = p.Area;
                    xlSheet.Cells[count, 2] = p.Title;
                    xlSheet.Cells[count, 3] = p.Owner.Email;
                    xlSheet.Cells[count, 4] = p.PortfolioManager.Email;
                    xlSheet.Cells[count, 5] = p.RGS;
                    xlSheet.Cells[count, 6] = p.Status.ToString();
                    xlSheet.Cells[count, 7] = p.CreationDate.ToString();
                    switch (p.Status)
                    {
                        case PositionStatus.Open:
                            OpenDays = (DateTime.Today - (DateTime)p.LastOpenedDate).Days;
                            break;
                        case PositionStatus.Closed:
                            OpenDays = ((DateTime)p.LastClosedDate - (DateTime)p.LastOpenedDate).Days;
                            break;
                        case PositionStatus.Cancelled:
                            OpenDays = ((DateTime)p.LastCancelledDate - (DateTime)p.LastOpenedDate).Days;
                            break;
                    }
                    xlSheet.Cells[count, 8] = OpenDays.ToString();
                    int count2 = 0;
                    string[] candidates = new string[p.PositionCandidates.Count];
                    foreach(PositionCandidates pc in p.PositionCandidates)
                    {
                        candidates[count2] = pc.Candidate.Name;
                        count2++;
                    }
                    xlSheet.Cells[count, 9] = string.Join(", ", candidates);
                    count++;
                }
                xl.Cells.Select();
                xl.Cells.EntireColumn.AutoFit();
                var filePath = System.IO.Path.GetTempFileName();
                xlworkBook.SaveAs(filePath);
                xl.Quit();
                Marshal.ReleaseComObject(xlSheet);
                Marshal.ReleaseComObject(xlworkBook);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                return filePath;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
