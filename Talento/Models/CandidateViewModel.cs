using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Talento.Models
{
    public class CandidateViewModel
    {
    }

    public class FileBlobViewModel
    {
        /*
        public int Id { get; set; }

        public int Candidate_Id { get; set; }

        public virtual Candidate Candidate { get; set; }
        */
        public string FileName { get; set; }

        public byte[] Blob { get; set; }
    }

    public class FilesViewModel
    {
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }

}