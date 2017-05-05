using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Talento.Entities
{
    [XmlRoot("ApplicationSetting")]
    public class ApplicationSettingXml
    {
        [System.Xml.Serialization.XmlElement("ApplicationSettingId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationSettingId { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Setting Name is required")]
        [System.Xml.Serialization.XmlElement("SettingName")]
        public string SettingName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Parameter Name is required")]
        [System.Xml.Serialization.XmlElement("ParameterName")]
        public string ParameterName { get; set; }

        [Required]
        [StringLength(160, ErrorMessage = "Setting Value is required")]
        [System.Xml.Serialization.XmlElement("ParameterValue")]
        public string ParameterValue { get; set; }

        [Required]
        [System.Xml.Serialization.XmlElement("CreationDate")]
        public DateTime CreationDate { get; set; }

        [System.Xml.Serialization.XmlElement("ApplicationUser_Id")]
        public string ApplicationUser_Id { get; set; }

        [System.Xml.Serialization.XmlElement("CreatedByUserName")]
        public string CreatedByUserName { get; set; }

        [System.Xml.Serialization.XmlElement("CreatedByEmail")]
        public string CreatedByEmail { get; set; }
    }
}
