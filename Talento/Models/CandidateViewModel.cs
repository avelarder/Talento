using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Talento.Entities;

namespace Talento.Models
{
    public class CandidateModel
    {
        public int CandidateId { get; set; }

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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        public bool IsTcsEmployee { get; set; }

        public int PositionId { get; set; }

        public IList<PositionCandidates> PositionCandidates { get; set; }

        public List<TechnicalInterview> Comments { get; set; }

        public bool isadmin { get; set; }

        public bool isopenposition{ get; set; }

    }

    public class FileBlobViewModel
    {
    
        public byte[] Blob { get; set; }

        public string FileName { get; set; }

    }

    public class EditCandidateViewModel
    {
        public int CandidateId { get; set; }

        public int Position_Id { get; set; }

        [StringLength(50, ErrorMessage = "Name must have 50 characters maximum")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "The email must be a valid email address")]
        public string Email { get; set; }

        [StringLength(300, ErrorMessage = "Competencies must have 300 characters maximum")]
        [Required(ErrorMessage = "Competencies is required")]
        public string Competencies { get; set; }

        [StringLength(300, ErrorMessage = "Competencies must have 300 characters maximum")]
        [Required(ErrorMessage = "Competencies is required")]
        public string Description { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        public bool IsTcsEmployee { get; set; }

        public IList<PositionCandidates> PositionCandidates { get; set; }
    }

    public class CreateCandidateViewModel
    {
        public int Position_Id { get; set; }

        public string Position_Name { get; set; }

        [StringLength(50, ErrorMessage = "Name must have 50 characters maximum")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Email must have 50 characters maximum")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "The email must be a valid email address")]
        public string Email { get; set; }

        [StringLength(300, ErrorMessage = "Competencies must have 300 characters maximum")]
        [Required(ErrorMessage = "Competencies is required")]
        public string Competencies { get; set; }

        [StringLength(300, ErrorMessage = "Description must have 300 characters maximum")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        public bool IsTcsEmployee { get; set; }

        public IList<Position> Positions { get; set; }
    }

    public class TechnicalInterviewModel
    {
        public int TechnicalInterviewId { get; set; }

        [Required(ErrorMessage = "Position and candidate is required")]
        public virtual PositionCandidates PositionCandidate { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Date { get; set; }

        [Required]
        public bool IsAccepted { get; set; }

        [StringLength(500, ErrorMessage = "Comment must have 500 characters maximum")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Interview ID is required")]
        [StringLength(10, ErrorMessage = "Interviewer Id must have 10 characters maximum")]
        public string InterviewerId { get; set; }

        [Required(ErrorMessage = "Interviewer Name is required")]
        [StringLength(50, ErrorMessage = "Interviewer Name must have 50 characters maximum")]
        public string InterviewerName { get; set; }

        [Required(ErrorMessage = "File is required")]
        public FileBlob FeedbackFile { get; set; }
    }
}