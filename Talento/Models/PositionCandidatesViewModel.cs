using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Talento.Entities;

namespace Talento.Models
{
    public class PositionCandidatesViewModel
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