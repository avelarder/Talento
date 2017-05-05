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
    public class TagModel
    {
        public string Name { get; set; }
    }

    public class CreatePositionViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Engagement Manager is required")]
        [DisplayName("Engagement Manager")]
        [MaxLength(20)]
        public string EngagementManager { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid RGS")]
        public string RGS { get; set; }

        [Required(ErrorMessage = "Portfolio Manager Email is required")]
        [DisplayName("Portfolio Manager")]
        [EmailAddress]
        public string EmailPM { get; set; }

    }

    public class PositionModel
    {
        public int PositionId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Engagement Manager is required")]
        [DisplayName("Engagement Manager")]
        [MaxLength(20)]
        public string EngagementManager { get; set; }

        public string PortfolioManager_Id { get; set; }

        [Required]
        public ApplicationUser PortfolioManager { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid RGS")]
        public string RGS { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public PositionStatus Status { get; set; }

        public string ApplicationUser_Id { get; set; }

        public List<Tag> Tags { get; set; }

        [Required]
        public ApplicationUser Owner { get; set; }

        public string LastOpenedBy_Id { get; set; }

        public virtual ApplicationUser LastOpenedBy { get; set; }

        public string LastCancelledBy_Id { get; set; }

        public virtual ApplicationUser LastCancelledBy { get; set; }

        public string LastClosedBy_Id { get; set; }

        public virtual ApplicationUser LastClosedBy { get; set; }

        public DateTime LastOpenedDate { get; set; }

        public DateTime LastCancelledDate { get; set; }

        public DateTime LastClosedDate { get; set; }

        public int OpenDays { get; set; }

        public IList<Log> Logs { get; set; }

        public IList<PositionCandidates> PositionCandidates { get; set; }
    }
    
    public class EditPositionViewModel
    {
        public int PositionId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string Area { get; set; }

        [Required(ErrorMessage = "Engagement Manager is required")]
        [MaxLength(20)]
        public string EngagementManager { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid RGS")]
        [Range(1,999999999)]
        public string RGS { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Range(2, 4,ErrorMessage ="Positions can only be opened, cancelled or closed")]
        public PositionStatus Status { get; set; }

        /*
        This could be useful in future in case of needing to edit the owner user account.It is not yet requested in the Edit user story 295
        4/4/2017 - Charlie
        public string OwnerEmail { get; set; }
        */
    }

    public class PositionsPagedList : PagedList<PositionModel>
    {
        public new int TotalItemCount { get; set; }
        public new List<PositionModel> Subset { get; set; }

        public PositionsPagedList(IEnumerable<PositionModel> positions, int pageNumber, int pageSize) : base(positions.AsQueryable(), pageNumber, pageSize)
        {
            TotalItemCount = base.TotalItemCount;
            Subset = base.Subset;
        }
    }

    public class DashBoardViewModel
    {
        public PositionsPagedList Positions { get; set; }
    }

    public class PositionLogViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        public string ApplicationUser_Id { get; set; }

        [Required(ErrorMessage = "User is required")]
        public virtual ApplicationUser User { get; set; }

        public int Position_Id { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public virtual Position Position { get; set; }

        [Required(ErrorMessage = "Action is required")]
        public Entities.Action Action { get; set; }

        [Required(ErrorMessage = "Previous Status is required")]
        public PositionStatus PreviousStatus { get; set; }

        [Required]
        public string Description { get; set; }

        [Required(ErrorMessage = "Actual Status is required")]
        public PositionStatus ActualStatus { get; set; }
    }
}