using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Talento.Entities;

namespace Talento.Models
{

    public class DashBoardViewModel
    {
        public PositionsPagedList Positions { get; set; }
    }

   
}