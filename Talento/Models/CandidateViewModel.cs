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

    public class FileBlobViewModel
    {
        /*
        public int Id { get; set; }

        public int Candidate_Id { get; set; }

        public virtual Candidate Candidate { get; set; }
        */
        public string FileName { get; set; }

        public byte[] Blob { get; set; }
    }

    public class FilesViewModel
    {
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
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

}