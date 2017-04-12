using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talento.Entities;
using Talento.Models;

namespace Talento.Controllers
{
    public class FileController : Controller
    {
        public ActionResult Index()
        {
            if (Session["files"] != null)
            {
                Session["files"] = new List<FileBlob>();
            }
            return PartialView("~/Views/Shared/File/Index.cshtml");
        }

        public ActionResult Add()
        {
            if (Session["files"] == null)
            {
                Session["files"] = new List<FileBlob>();
            }
            HttpPostedFileBase file = Request.Files.Get(0);
            byte[] uploadFile = new byte[file.InputStream.Length];
            file.InputStream.Read(uploadFile, 0, uploadFile.Length);

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
            return new EmptyResult();
        }

        public JsonResult ListCurrentFiles()
        {
            List<FileBlobViewModel> result = new List<FileBlobViewModel>();
            if (Session["files"] != null)
            {
                ((List<FileBlob>)Session["files"]).ForEach(x => result.Add(new FileBlobViewModel() { FileName = x.FileName }));
            }
            return Json(result);
        }

        public ActionResult Delete(string filename)
        {
            ((List<FileBlob>)Session["files"]).Remove(((List<FileBlob>)Session["files"]).Single(x => x.FileName.Equals(filename)));
            return new EmptyResult();
        }

        public ActionResult EmptyList()
        {
            Session["files"] = null;
            return new EmptyResult();
        }
    }
}