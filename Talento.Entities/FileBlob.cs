using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talento.Entities
{
    public class FileBlob
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key, Column(Order = 1)]
        public int Candidate_Id { get; set; }

        [Required]
        [ForeignKey("Candidate_Id")]
        public virtual Candidate Candidate { get; set; }
        
        [Required]
        public string FileName { get; set; }

        public byte[] Blob { get; set; }
    }
}
