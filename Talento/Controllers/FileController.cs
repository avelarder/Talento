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
        IFileManagerHelper FileManagerHelper;
        ICandidate CandidateHelper;
        public FileController(IFileManagerHelper fileManagerHelper, ICandidate candidateHelper)
        {
            FileManagerHelper = fileManagerHelper;
            CandidateHelper = candidateHelper;
        }

        public ActionResult Index()
        {
            if (Session["files"] != null)
            {
                Session["files"] = new List<FileBlob>();
            }
            return PartialView("~/Views/Shared/File/Index.cshtml");
        }

        public ActionResult Edit(int candidateId)
        {
            return PartialView("~/Views/Shared/File/Index.cshtml");
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Add()
        {
            if (Session["files"] == null)
            {
                Session["files"] = new List<FileBlob>();
            }
            HttpPostedFileBase file = Request.Files.Get(0);
            byte[] uploadFile = new byte[file.InputStream.Length];
            file.InputStream.Read(uploadFile, 0, uploadFile.Length);

            bool isValid = true;
            if (Request.Params.Get("candidateId") != null)
            {
                int candidateid = int.Parse(Request.Params.Get("candidateId"));
                List<FileBlob> current = FileManagerHelper.GetAll(CandidateHelper.Get(candidateid));

                current.ForEach(x =>
                {
                    if (x.FileName.Equals(file.FileName))
                    {
                        isValid = false;
                    }
                });
            }
            if (isValid)
            {
                ((List<FileBlob>)Session["files"]).Add(new FileBlob
                {
                    FileName = file.FileName,
                    Blob = uploadFile,
                });
                bool isValidFile = true;

                ((List<FileBlob>)Session["files"]).ForEach(x =>
                {
                    if (x.FileName.Equals(file.FileName))
                    {
                        isValidFile = false;
                    }
                });
                if (isValidFile)
                {
                    ((List<FileBlob>)Session["files"]).Add(new FileBlob
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
            if (Session["files"] != null)
            {
                ((List<FileBlob>)Session["files"]).ForEach(x => result.Add(new FileBlobViewModel() { FileName = x.FileName }));
            }
            result = result.GroupBy(x => x.FileName).Select(y => y.FirstOrDefault()).ToList();
            return Json(result);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult ListCandidateFiles(int candidateId)
        {
            List<FileBlobViewModel> result = new List<FileBlobViewModel>();
            if (Session["files"] != null)
            {
                ((List<FileBlob>)Session["files"]).ForEach(x => result.Add(new FileBlobViewModel() { FileName = x.FileName }));
            }

            List<FileBlob> current = FileManagerHelper.GetAll(CandidateHelper.Get(candidateId));
            current.ForEach(x => result.Add(new FileBlobViewModel() { FileName = x.FileName }));

            return Json(result);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Delete(string filename)
        {
            ((List<FileBlob>)Session["files"]).Remove(((List<FileBlob>)Session["files"]).Single(x => x.FileName.Equals(filename)));

            return new EmptyResult();
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult DeleteEdit(string filename, int candidateid)
        {
            if (Session["files"] == null)
            {
                FileManagerHelper.Delete(FileManagerHelper.GetAll(CandidateHelper.Get(candidateid)).Single(x => x.FileName.Equals(filename)));
            }
            else
            {
                ((List<FileBlob>)Session["files"]).Remove(((List<FileBlob>)Session["files"]).Single(x => x.FileName.Equals(filename)));
            }

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