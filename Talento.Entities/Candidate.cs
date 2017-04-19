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
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name must have 50 characters maximum")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        [StringLength(50, ErrorMessage = "Email must have 50 characters maximum")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [StringLength(300, ErrorMessage = "Competencies must have 300 characters maximum")]
        [Required(ErrorMessage = "Competencies is required")]
        public string Competencies { get; set; }

        [StringLength(300, ErrorMessage = "Description must have 300 characters maximum")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        [ForeignKey("CreatedBy_Id")]
        public virtual ApplicationUser CreatedBy { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [DefaultValue("New")]
        public CandidateStatus Status { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsTcsEmployee { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
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
