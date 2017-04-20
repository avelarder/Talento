using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.Entities
{
    public class ApplicationSetting
    {       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationSettingId { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Setting Name is required")]
        [Index(IsUnique = true)]
        public string SettingName { get; set; }

        public virtual ICollection<ApplicationParameter> ApplicationParameter { get; set; }
    }

    public class ApplicationParameter
    { 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationParameterId { get; set; }

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

        [Required]
        public int ApplicationSettingId { get; set; }
        [ForeignKey("ApplicationSettingId")]
        public virtual ApplicationSetting ApplicationSetting { get; set; }
    }
}
