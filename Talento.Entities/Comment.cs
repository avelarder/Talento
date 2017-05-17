using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.Entities
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [StringLength(500, ErrorMessage = "Comment must have 500 characters maximum")]
        public string Content { get; set; }
        
        public int?CandidateId { get; set; }

        [Required]
        public int PositionId { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        public string UserId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
