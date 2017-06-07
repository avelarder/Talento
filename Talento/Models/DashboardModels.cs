using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Talento.Entities;

namespace Talento.Models
{

    public class DashBoardViewModel
    {
        public PositionsPagedList Positions { get; set; }
    }


    public class ApplicationUserPagedList : PagedList<ApplicationUser>
    {
        public new int TotalItemCount { get; set; }
        public new List<ApplicationUser> Subset { get; set; }

        public ApplicationUserPagedList(IEnumerable<ApplicationUser> positions, int pageNumber, int pageSize) : base(positions.AsQueryable(), pageNumber, pageSize)
        {
            TotalItemCount = base.TotalItemCount;
            Subset = base.Subset;
        }
    }

    public class ApplicationUserViewModel
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? Createdon { get; set; }
        public string Role { get; set; }

    }

    public class UsersTableViewModel
    {
        public List<ApplicationUserViewModel> Users { get; set; }
    }

}