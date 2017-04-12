using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Talento.Models
{
    public class NewCandidateViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsTcsEmployee { get; set; }

        public string Competencies { get; set; }

        public string Description { get; set; }

    }

    public class FileBlobViewModel
    {
        public string FileName { get; set; }

        //public byte[] Blob { get; set; }
    }

}