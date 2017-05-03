using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.Entities
{
    public class Candidate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        [ForeignKey("CreatedBy_Id")]
        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsTcsEmployee { get; set; }

        public virtual ICollection<PositionCandidates> PositionCandidates { get; set; }

        public virtual ICollection<FileBlob> FileBlobs { get; set; }
    }
}
