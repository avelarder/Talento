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
            List<FileBlobViewModel> vm = new List<FileBlobViewModel>();
            if (Session["files"] != null)
            {
                ((List<FileBlob>)Session["files"]).ForEach(file =>
                {
                    vm.Add(new FileBlobViewModel
                    {
                        FileName = file.FileName,
                        Blob = file.Blob
                    });
                });
            }

            return View("~/Views/Shared/File/Index.cshtml", new Tuple<FilesViewModel, List<FileBlobViewModel>>(new FilesViewModel(), vm));
        }

        public ActionResult Add(FilesViewModel fileBlobModel)
        {
            if (Session["files"] == null)
            {
                Session["files"] = new List<FileBlob>();
            }

            foreach (var item in fileBlobModel.Files)
            {
                byte[] uploadFile = new byte[item.InputStream.Length];
                item.InputStream.Read(uploadFile, 0, uploadFile.Length);

                bool isValidFile = true;
                ((List<FileBlob>)Session["files"]).ForEach(x =>
                {
                    if (x.FileName.Equals(item.FileName))
                    {
                        isValidFile = false;
                    }
                });

                if (isValidFile)
                {
                    ((List<FileBlob>)Session["files"]).Add(new FileBlob
                    {
                        FileName = item.FileName,
                        Blob = uploadFile,
                    });
                }
            }

            return RedirectToAction("Index", "Dashboard", null);
        }


        public ActionResult Delete(string filename)
        {
            ((List<FileBlob>)Session["files"]).Remove(((List<FileBlob>)Session["files"]).Single(x => x.FileName.Equals(filename)));
            return RedirectToAction("Index", "Dashboard", null);
        }
    }
}