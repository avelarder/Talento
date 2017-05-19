using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.Entities
{
    public class TechnicalInterview
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TechnicalInterviewId { get; set; }

        [Required]
        public virtual PositionCandidates PositionCandidate { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public bool IsAccepted { get; set; }

        [StringLength(500, ErrorMessage = "Comment must have 500 characters maximum")]
        public string Comment { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Interviewer Id must have 10 characters maximum")]
        public string InterviewerId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Interviewer Name must have 50 characters maximum")]
        public string InterviewerName { get; set; }

        [Required]
        public FileBlob FeedbackFile { get; set; }
    }
}
