using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Talento.Models
{
    public class CreateTechnicalInterviewViewModel
    {

        [Display(Name = "Candidate Email")]
        public string CandidateEmail { get; set; }

        [Display(Name = "PositionId")]
        public int PositionId { get; set; }

        [Display(Name = "CandidateId")]
        public int CandidateId { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Result")]
        public bool Result { get; set; }

        [Display(Name = "Interviewer's Name")]
        [Required]
        [StringLength(50, ErrorMessage = "Interviewer Name must have 50 characters maximum")]
        public string InterviewerName { get; set; }

        [Display(Name = "Interviewer's Id Number")]
        [Required]
        public int InterviewerId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Comment must have 500 characters maximum")]
        public string Comment { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }
    }
}