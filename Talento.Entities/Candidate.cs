using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.Entities
{
    public abstract class Candidate
    {
        //(Email, CreatedOn, CreatedBy, Status (New, Technical Interview, Conditional Offer, Customer Interview, Accepted, Rejected, Canceled))

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        public DateTime? CratedOn { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public CandidateStatus Status { get; set; }
    }

    public enum CandidateStatus
    {
        New = 0,
        Technical_Interview = 1,
        Conditional_Offer = 2,
        Customer_Interview = 3,
        Accepted = 4,
        Rejected = 5,
        Cancelled = 6
    }

}
