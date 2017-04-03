using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Talento.Entities
{
    public class PositionLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string ApplicationUser_Id { get; set; }

        [Required]
        [ForeignKey("ApplicationUser_Id")]
        public virtual ApplicationUser User { get; set; }

        public int Position_Id { get; set; }

        [Required]
        [ForeignKey("Position_Id")]
        public virtual Position Position { get; set; }

        [Required]
        public Action Action { get; set; }

        public Status PreviousStatus { get; set; }

        [Required]
        public Status ActualStatus { get; set; }
    }

    public enum Action
    {
        Create = 1,
        Edit = 2,
        Delete = 3,
        ChangeStatus = 4
    }
}


