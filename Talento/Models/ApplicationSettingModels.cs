using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Talento.Entities;

namespace Talento.Models
{
    public class ApplicationSettingModels
    {

        public int ApplicationSettingId { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Setting Name is required")]
        public string SettingName { get; set; }

        public IList<ApplicationParameter> ApplicationParameter { get; set; }
    }
}