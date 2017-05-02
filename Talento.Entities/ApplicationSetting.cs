using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talento.Entities
{
    public class ApplicationSetting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationSettingId { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Setting Name is required")]
        public string SettingName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter Name is required")]
        public string ParameterName { get; set; }

        [Required]
        [StringLength(160, ErrorMessage = "Setting Value is required")]
        public string ParameterValue { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public string ApplicationUser_Id { get; set; }
        [Required]
        [ForeignKey("ApplicationUser_Id")]
        public virtual ApplicationUser CreatedBy { get; set; }
    }
}
