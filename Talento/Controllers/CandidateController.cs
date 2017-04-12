using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talento.Entities;
using Talento.Models;

namespace Talento.Controllers
{
    public class CandidateController : Controller
    {
        // GET: Candidate
        public ActionResult Edit()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult New(NewCandidateViewModel candidate)
        {

            List<FileBlob> files = ((List<FileBlob>)Session["files"]);



            return new EmptyResult();
        }

    }
}