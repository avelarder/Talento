using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Talento.Entities;

namespace Talento.Models
{
    public class CandidateModel
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name must have 50 characters maximum")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Email must have 50 characters maximum")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(300, ErrorMessage = "Competencies must have 300 characters maximum")]
        [Required(ErrorMessage = "Competencies is required")]
        public string Competencies { get; set; }

        [StringLength(300, ErrorMessage = "Description must have 300 characters maximum")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Range(0, 6, ErrorMessage = "Status should be a valid one.")]
        public CandidateStatus Status { get; set; }

        [Required]
        public bool IsTcsEmployee { get; set; }
    }

    public class FileBlobViewModel
    {
    
        public byte[] Blob { get; set; }

        public string FileName { get; set; }

    }

    public class PositionCandidateViewModel
    {
        //(Email, CreatedOn, CreatedBy, Status (New, Technical Interview, Conditional Offer, Customer Interview, Accepted, Rejected, Canceled))

        public int Candidate_Id { get; set; }

        [Required]
        public virtual Candidate Candidate { get; set; }

        public int Position_Id { get; set; }

        [Required]
        public virtual Position Position { get; set; }
    }

    public class EditCandidateViewModel
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name must have 50 characters maximum")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Email must have 50 characters maximum")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(300, ErrorMessage = "Competencies must have 300 characters maximum")]
        [Required(ErrorMessage = "Competencies is required")]
        public string Competencies { get; set; }

        [StringLength(300, ErrorMessage = "Competencies must have 300 characters maximum")]
        [Required(ErrorMessage = "Competencies is required")]
        public string Description { get; set; }

        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Range(0, 6, ErrorMessage = "Status should be a valid one.")]
        public CandidateStatus Status { get; set; }
    }

    public class CreateCandidateViewModel
    {
        public int Position_Id { get; set; }

        [StringLength(50, ErrorMessage = "Name must have 50 characters maximum")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Email must have 50 characters maximum")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(300, ErrorMessage = "Competencies must have 300 characters maximum")]
        [Required(ErrorMessage = "Competencies is required")]
        public string Competencies { get; set; }

        [StringLength(300, ErrorMessage = "Description must have 300 characters maximum")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Range(0, 6, ErrorMessage = "Status should be a valid one.")]
        public CandidateStatus Status { get; set; }

        [Required]
        public string IsTcsEmployee { get; set; }
    }

}