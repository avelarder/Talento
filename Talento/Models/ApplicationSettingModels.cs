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
        [Required]
        [StringLength(60, ErrorMessage = "Setting name is required")]
        public string SettingName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter name is required")]
        public string ParameterName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter value is required")]
        public string ParameterValue { get; set; }

        public DateTime? CratedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public ApplicationUser CreatedBy { get; set; }
    }

    public class ApplicationParameterModels
    {
        public int ApplicationParameterId { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter Name is required")]
        public string ParameterName { get; set; }

        [Required]
        [StringLength(160, ErrorMessage = "Parameter Value is required")]
        public string ParameterValue { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public string ApplicationUser_Id { get; set; }
        [Required]
        public ApplicationUser CreatedBy { get; set; }

        [StringLength(160, ErrorMessage = "Setting Name is required")]
        string SettingName { get; set; }

        [Required]
        public int ApplicationSettingId { get; set; }
        public ApplicationSetting ApplicationSetting { get; set; }
    }
}