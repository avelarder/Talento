using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.Entities
{
    public class SettingGroup
    {
        [Key]
        public int SettingGroupId { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string SettingGroupName { get; set; } //Pagination, Sorting

        public virtual List<SettingName> SettingNames { get; set; } //FK
    }

    public class SettingName
    {
        [Key, Column("SettingNameId", Order = 0)]
        public int SettingNameId { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string SettingNameName { get; set; } // Filter_by, Order_by

        public virtual SettingGroup SettingGroup { get; set; } //FK

        [Key, ForeignKey("SettingGroup"), Column(Order = 1)]
        public int SettingGroupId { get; set; }

        public string SettingValues { get; set; }

        public DateTime CratedOn { get; set; }

        public int CreatedBy_Id { get; set; }
    }
}