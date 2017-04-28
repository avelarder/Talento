using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Talento.Entities
{
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Description { get; set; }

        public string ApplicationUser_Id { get; set; }

        [Required]
        [ForeignKey("ApplicationUser_Id")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public Action Action { get; set; }

        public Status PreviousStatus { get; set; }

        [Required]
        public Status ActualStatus { get; set; }

        public virtual Position Position { get; set; }
    }

    public enum Action
    {
        Create = 1,
        Edit = 2,
        Delete = 3,
    }
}


