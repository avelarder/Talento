﻿using PagedList;
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

        public string PortfolioManager_Id { get; set; }

        [Required]
        public ApplicationUser PortfolioManager { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid RGS")]
        public string RGS { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public Status Status { get; set; }

        public string ApplicationUser_Id { get; set; }

        public List<Tag> Tags { get; set; }

        [Required]
        public ApplicationUser Owner { get; set; }
    }

    public class PositionLogViewModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Entities.Action Action { get; set; }

        [Required]
        public Entities.Status PreviousStatus { get; set; }

        [Required]
        public Entities.Status ActualStatus { get; set; }

        [Required]
        public int Position_Id { get; set; }

        [Required]
        public string User_Id { get; set; }

    }

    public class EditPositionViewModel
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

        /*
        This could be useful in future in case of needing to edit the owner user account.It is not yet requested in the Edit user story 295
        4/4/2017 - Charlie
        public string OwnerEmail { get; set; }
        */
    }

    public class PositionsPagedList : PagedList<PositionModel>
    {
        public int TotalCount { get; set; }
        public List<PositionModel> Subset { get; set; }

        public PositionsPagedList(IEnumerable<PositionModel> positions, int pageNumber, int pageSize) : base(positions.AsQueryable(), pageNumber, pageSize)
        {
            TotalCount = base.TotalItemCount;
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
        public Status PreviousStatus { get; set; }

        [Required(ErrorMessage = "Actual Status is required")]
        public Status ActualStatus { get; set; }
    }
}