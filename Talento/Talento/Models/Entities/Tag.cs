﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Talento.Models
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }
    }
}