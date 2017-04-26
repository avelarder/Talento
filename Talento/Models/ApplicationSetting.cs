using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Talento.Entities;

namespace Talento.Models
{
    public class ApplicationSetting
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Parameter name must have 50 characters maximum")]
        [Required(ErrorMessage = "Parameter name is required")]
        public string KeyGroup { get; set; }

        public IList<Parameter> Parameters { get; set; }

        [StringLength(300, ErrorMessage = "Values must have 300 characters maximum")]
        [Required(ErrorMessage = "Values field is required")]
        public List<string> Vaues { get; set; }

        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
    }
}
/*
A key group,
A key name,
A key value(free text)
Who and When got created.
*/