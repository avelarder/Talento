using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Talento.Models.Entities
{
    public class PositionLog
    {
        public int Id { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Date { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public Action Action { get; set; }

        public Status PreviousStatus { get; set; }

        [Required]
        public Status ActualStatus { get; set; }
    }

    public enum Action
    {
        Create,
        Edit,
        Delete,
        ChangeStatus
    }
}


