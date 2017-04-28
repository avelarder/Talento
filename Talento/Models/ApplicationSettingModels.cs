using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Talento.Entities;

namespace Talento.Models
{
    public class ApplicationSettingModel
    {
        public int ApplicationSettingId { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Setting Name is required")]
        public string SettingName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter name is required")]
        [DisplayName("Parameter name")]
        public string ParameterName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter value is required")]
        [DisplayName("Parameter Value")]
        public string ParameterValue { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy_Id { get; set; }

        public ApplicationUser CreatedBy { get; set; }
    }

    public class ApplicationSettingCreateModel
    {
        [Required]
        [StringLength(60, ErrorMessage = "Setting Name is required")]
        public string SettingName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter name is required")]
        [DisplayName("Parameter name")]
        public string ParameterName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter value is required")]
        [DisplayName("Parameter Value")]
        public string ParameterValue { get; set; }
    }

    public class ApplicationSettingEditModel
    {
        public int ApplicationSettingId { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Setting Name is required")]
        public string SettingName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter name is required")]
        [DisplayName("Parameter name")]
        public string ParameterName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter value is required")]
        [DisplayName("Parameter Value")]
        public string ParameterValue { get; set; }
    }
}
    