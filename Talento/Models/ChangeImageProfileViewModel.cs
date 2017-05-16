using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Talento.Models
{
    public class ChangeImageProfileViewModel
    {
        public Image profileImage { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }
    }
}