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
        [Key, Column(Order =0)]
        public int Candidate_Id { get; set; }

        [Required]
        [ForeignKey("Candidate_Id")]
        public virtual Candidate Candidate { get; set; }

        [Key, Column(Order = 1)]
        public int Position_Id { get; set; }

        [Required]
        [ForeignKey("Position_Id")]
        public virtual Position Position { get; set; }
    }
}
