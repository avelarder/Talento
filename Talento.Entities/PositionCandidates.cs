using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.Entities
{
    public class PositionCandidates
    {
        [Key, Column(Order = 0)]
        public int PositionID { get; set; }
        [Key, Column(Order = 1)]
        public int CandidateID { get; set; }

        public virtual Position Position { get; set; }
        public virtual Candidate Candidate { get; set; }

        [Required]
        public virtual PositionCandidatesStatus Status { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public enum PositionCandidatesStatus
    {
        New = 0,
        Interview_Accepted = 1,
        Interview_Rejected = 2,
        Conditional_Offer_Accepted = 3,
        Conditional_Offer_Rejected = 4,
        Conditional_Offer_Negotiating = 5,
        Customer_Approved = 6,
        Customer_Rejected = 7,
        Mannualy_Removed = 8
    }
}
