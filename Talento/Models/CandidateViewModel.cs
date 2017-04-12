using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Talento.Entities;

namespace Talento.Models
{
    public class CandidateViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public DateTime? CratedOn { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public CandidateStatus Status { get; set; }
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
}