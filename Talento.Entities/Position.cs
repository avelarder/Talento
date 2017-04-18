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
    public class Position
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Title must have 50 characters maximum")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Descriptiom must have 500 characters maximum")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [StringLength(20, ErrorMessage = "Area must have 20 characters maximum")]
        [Required(ErrorMessage = "Area is required")]
        public string Area { get; set; }

        [StringLength(50, ErrorMessage = "Enagagement Manager must have 20 characters maximum")]
        [Required(ErrorMessage = "Engagement Manager is required")]
        public string EngagementManager { get; set; }

        public string PortfolioManager_Id { get; set; }

        [Required]
        [ForeignKey("PortfolioManager_Id")]
        public virtual ApplicationUser PortfolioManager { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid RGS")]
        public string RGS { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [DefaultValue("Open")]
        public Status Status { get; set; }

        public List<Tag> Tags { get; set; }

        public string ApplicationUser_Id { get; set; }

        [Required]
        [ForeignKey("ApplicationUser_Id")]
        public virtual ApplicationUser Owner { get; set; }

        public string LastOpenedBy_Id { get; set; }

        [ForeignKey("LastOpenedBy_Id")]
        public virtual ApplicationUser LastOpenedBy { get; set; }

        public string LastCancelledBy_Id { get; set; }

        [ForeignKey("LastCancelledBy_Id")]
        public virtual ApplicationUser LastCancelledBy { get; set; }

        public string LastClosedBy_Id { get; set; }

        [ForeignKey("LastClosedBy_Id")]
        public virtual ApplicationUser LastClosedBy { get; set; }

        public DateTime? LastOpenedDate { get; set; }

        public DateTime? LastCancelledDate { get; set; }

        public DateTime? LastClosedDate { get; set; }
        
        [Required(ErrorMessage = "OpenStatus is required")]
        [DefaultValue("Open")]
        public OpenStatus OpenStatus { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }
    }

    public enum Status
    {
        Removed = 1,
        Open = 2,
        Cancelled = 3,
        Closed = 4
    }

    public enum OpenStatus
    {
        Screening = 1,
        Interviewing = 2,
        Offered = 3   
    }
}
