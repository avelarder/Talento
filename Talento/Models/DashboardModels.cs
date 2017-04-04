using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Talento.Entities;

namespace Talento.Models
{
    public class TagModel
    {
        public string Name { get; set; }
    }

    public class PositionModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Engagement Manager is required")]
        public string EngagementManager { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid RGS")]
        public string RGS { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public Status Status { get; set; }

        public string ApplicationUser_Id { get; set; }

        public List<Tag> Tags { get; set; }

        [Required]
        public ApplicationUser Owner { get; set; }
    }

    public class PositionsPagedList : PagedList<PositionModel>
    {
        public PositionsPagedList(IEnumerable<PositionModel> positions, int pageNumber, int pageSize) : base(positions.AsQueryable(), pageNumber, pageSize)
        {
        }
    }

    public class DashBoardViewModel
    {
        public PositionsPagedList Positions { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int TotalItemCount { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        //and other props from ipagedlist
    }

}