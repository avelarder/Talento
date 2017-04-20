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

    public class CreateApplicationSettingsViewModel
    {
        public int ApplicationSettingId { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Setting Name is required")]
        public string SettingName { get; set; }

        public IList<ApplicationParameter> ApplicationParameter { get; set; }

        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
    }

    //crear viewmodel para ApplicationParameters para bindear a la vista
    public class CreateApplicationParametersViewModel
    {
        public IList<ApplicationParameter> ApplicationParameter { get; set; }
    }
}