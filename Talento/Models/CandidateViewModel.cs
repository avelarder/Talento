﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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

        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        public bool IsTcsEmployee { get; set; }

        public IList<PositionCandidates> PositionCandidates { get; set; }
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

        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        public string IsTcsEmployee { get; set; }

        public IList<PositionCandidates> PositionCandidates { get; set; }
    }

    public class CreateCandidateViewModel
    {
        public int Position_Id { get; set; }

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

        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        public string IsTcsEmployee { get; set; }

        public IList<Position> Positions { get; set; }
    }
}