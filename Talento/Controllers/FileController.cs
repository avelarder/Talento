using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Talento.Core;
using Talento.Entities;
using Talento.Models;

namespace Talento.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        ICandidate CandidateHelper;
        public FileController(ICandidate candidateHelper)
        {
            CandidateHelper = candidateHelper;
        }

        public ActionResult Index()
        {
            if (Session["files"] != null)
            {
                Session["files"] = new HashSet<FileBlob>();
            }
            return PartialView("~/Views/Shared/File/Index.cshtml");
        }

        public ActionResult Edit(int candidateId)
        {
            Session["files"] = CandidateHelper.Get(candidateId).FileBlobs;
            return PartialView("~/Views/Shared/File/Index.cshtml");
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Add()
        {
            if (Session["files"] == null)
            {
                Session["files"] = new HashSet<FileBlob>();
            }
            HttpPostedFileBase file = Request.Files.Get(0);
            byte[] uploadFile = new byte[file.InputStream.Length];
            file.InputStream.Read(uploadFile, 0, uploadFile.Length);
            if ((new List<string> { "doc", "docx", "zip", "pdf" }).Contains(file.FileName.Split('.')[file.FileName.Split('.').Length-1])) //is valid extension?
            {
                bool isValidFile = true;
                ((IEnumerable<FileBlob>)Session["files"]).ToList().ForEach(x =>
                {
                    if (x.FileName.Equals(file.FileName))
                    {
                        isValidFile = false;
                    }
                });
                if (isValidFile)
                {
                    ((HashSet<FileBlob>)Session["files"]).Add(new FileBlob
                    {
                        FileName = file.FileName,
                        Blob = uploadFile,

                    });
                }
            }

            return new EmptyResult();
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult ListCurrentFiles()
        {
            List<FileBlobViewModel> result = new List<FileBlobViewModel>();
            if (Session["files"] == null)
            {
                Session["files"] = new HashSet<FileBlob>();
            }

            ((IEnumerable<FileBlob>)Session["files"]).ToList().ForEach(x => result.Add(new FileBlobViewModel() { FileName = x.FileName }));

            return Json(result);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Delete(string filename)
        {
            ((HashSet<FileBlob>)Session["files"]).Remove(((HashSet<FileBlob>)Session["files"]).Single(x => x.FileName.Equals(filename)));

            return new EmptyResult();
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult DeleteEdit(string filename, int candidateid)
        {
            ((HashSet<FileBlob>)Session["files"]).Remove(((HashSet<FileBlob>)Session["files"]).Single(x => x.FileName.Equals(filename)));
            return new EmptyResult();
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult EmptyList()
        {
            Session["files"] = null;
            return new EmptyResult();
        }

        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        protected class ValidateJsonAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
                try
                {
                    if (filterContext == null)
                    {
                        throw new ArgumentNullException("filterContext");
                    }
                    var httpContext = filterContext.HttpContext;
                    var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
                    AntiForgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Params["__RequestVerificationToken"]);
                }
                catch (Exception)
                {
                }
            }
        }

    }
}