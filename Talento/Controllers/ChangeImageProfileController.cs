using Microsoft.AspNet.Identity;
using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using Talento.Core;
using Talento.Entities;
using Talento.Models;

namespace Talento.Controllers
{
    public class ChangeImageProfileController : Controller
    {
        ICustomUser UserHelper;

        public ChangeImageProfileController(ICustomUser userHelper)
        {
            UserHelper = userHelper;
        }

        // GET: ChangeImageProfile
        [HandleError]
        [Authorize(Roles = "Admin, PM, TAG, RMG, TL")]
        public ActionResult Index()
        {
            try
            {
                Image i;

                ApplicationUser appUser = UserHelper.GetUserById(User.Identity.GetUserId());

                if (appUser.ImageProfile == null)
                {
                    i = Image.FromFile(Server.MapPath("~/Content/Images/alien1.png"));
                }
                else
                {
                    MemoryStream ms = new MemoryStream(appUser.ImageProfile);
                    i = Image.FromStream(ms);
                }

                return View(new ChangeImageProfileViewModel
                {
                    profileImage = i
                });
            }
            catch (ArgumentException)
            {
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfileImage(ChangeImageProfileViewModel model)
        {
            if(model.File!=null)
            {
                bool isvalidfile = true;
                if (model.File.ContentType.ToLower() != "image/jpg" &&
                    model.File.ContentType.ToLower() != "image/jpeg" &&
                    model.File.ContentType.ToLower() != "image/pjpeg" &&
                    model.File.ContentType.ToLower() != "image/gif" &&
                    model.File.ContentType.ToLower() != "image/x-png" &&
                    model.File.ContentType.ToLower() != "image/png")
                {
                    isvalidfile= false;
                }


                if (ModelState.IsValid && isvalidfile)
                {
                    ApplicationUser appUser = UserHelper.GetUserById(User.Identity.GetUserId());

                    Byte[] newImage = new BinaryReader(model.File.InputStream).ReadBytes(model.File.ContentLength);

                    appUser.ImageProfile = newImage;

                    UserHelper.ChangeImageProfile(appUser);
                }
            }
            return RedirectToAction("Index", "Dashboard");

        }
    }
}