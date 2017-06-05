using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talento.Core;
using Moq;
using Talento.Controllers;
using System.Web.Mvc;
using Talento.Entities;
using System.IO;
using Talento.Models;
using System.Web;
using System.Drawing;
using System.Security.Principal;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class ChangeImageProfileControllerTest
    {
        Mock<ICustomUser> mUser;

        [TestInitialize]
        public void Initialize()
        {
            mUser = new Mock<ICustomUser>();
        }

        [TestMethod]
        public void Index_ChangeProfileImagePage()
        {
            ApplicationUser appUser = new ApplicationUser
            {
                Id = "1",
                Email = "email@example.com",
                UserName = "email@example.com",
                ImageProfile = File.ReadAllBytes(Path.GetFullPath(@"Files\originalImage.png"))
            };

            var mIIdentity = new Mock<IIdentity>();
            mIIdentity.Setup(p => p.Name).Returns("email@example.com");

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.Identity).Returns(mIIdentity.Object);

            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            mUser.Setup(x => x.GetUserById(null)).Returns(appUser);

            var controller = new ChangeImageProfileController(mUser.Object)
            {
                ControllerContext = mockContext.Object
            };
            var viewResult = controller.Index();

            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOfType(viewResult, typeof(ViewResult));
        }

        [TestMethod]
        public void ChangeProfileImage_ChangeProfileImagePage()
        {
            ApplicationUser appUser = new ApplicationUser
            {
                Id = "1",
                Email = "email@example.com",
                UserName = "email@example.com",
                ImageProfile = File.ReadAllBytes(Path.GetFullPath(@"Files\originalImage.png"))
            };

            var mIIdentity = new Mock<IIdentity>();
            mIIdentity.Setup(p => p.Name).Returns("email@example.com");

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.Identity).Returns(mIIdentity.Object);

            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            mUser.Setup(x => x.GetUserById(null)).Returns(appUser);

            //Mock file
            string filePath = Path.GetFullPath(@"Files\editedImage.png");
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            Mock<HttpPostedFileBase> uploadedFile = new Mock<HttpPostedFileBase>();

            uploadedFile
                .Setup(f => f.ContentLength)
                .Returns(10);

            uploadedFile
                .Setup(f => f.FileName)
                .Returns("editedImage.png");

            uploadedFile
                .Setup(f => f.ContentType)
                .Returns("image/png");

            uploadedFile
                .Setup(f => f.InputStream)
                .Returns(fileStream);

            ChangeImageProfileViewModel model = new ChangeImageProfileViewModel
            {
                File = uploadedFile.Object,
                profileImage = Image.FromStream(fileStream)

            };

            var controller = new ChangeImageProfileController(mUser.Object)
            {
                ControllerContext = mockContext.Object
            };
            var result = controller.ChangeProfileImage(model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
    }
}
