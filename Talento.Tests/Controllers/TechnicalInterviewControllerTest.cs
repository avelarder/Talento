using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Talento.Controllers;
using Talento.Entities;
using System.Collections.Generic;
using Talento.Models;
using Talento.Core;
using Moq;
using System.Security.Principal;
using Talento.EmailManager;
using System.Web;
using System.IO;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class TechnicalInterviewControllerTest
    {
        [TestMethod]
        public void InterviewFeedbackNewStatusTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<ICandidate> mockCandidateHelper = mocks.Create<ICandidate>();
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            Mock<IMessenger> mockEmailManager = mocks.Create<IMessenger>();
            Mock<HttpContextBase> mockHttpcontext = mocks.Create<HttpContextBase>();
            var mockContext = new Mock<ControllerContext>();
            Mock<ApplicationUser> mockUser = mocks.Create<ApplicationUser>();
            Mock<UrlHelper> mockUrlHelper = mocks.Create<UrlHelper>();

            //Mock file
            string filePath = Path.GetFullPath(@"Files\pdftest.pdf");
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            Mock<HttpPostedFileBase> uploadedFile = new Mock<HttpPostedFileBase>();

            uploadedFile
                .Setup(f => f.ContentLength)
                .Returns(10);

            uploadedFile
                .Setup(f => f.FileName)
                .Returns("pdftest.pdf");

            uploadedFile
                .Setup(f => f.InputStream)
                .Returns(fileStream);

            //Setting up the context
            mockPrincipal.Setup(p => p.IsInRole("PM")).Returns(true);
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("pablo@example.com");
            mockContext.Setup(p => p.HttpContext.User.Identity.Name).Returns(mockPrincipal.Object.Identity.Name);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            mockContext.SetupGet(c => c.HttpContext.Session).Returns(mockHttpcontext.Object.Session);
            mockContext.Setup(c => c.HttpContext.Request.Url).Returns(new Uri("http://Talento.com"));
            mockHttpcontext.SetupGet(x => x.Request.ApplicationPath).Returns("/");
            mockHttpcontext.SetupGet(x => x.Request.Url).Returns(new Uri("http://Talento.com", UriKind.Absolute));
            mockUrlHelper
                .Setup(m => m.Action("Details", "Position", It.IsAny<object>(), It.IsAny<string>()))
                .Returns("http://Talento.com");

            //Test data
            var userTest = new ApplicationUser()
            {
                Id = "pmuser1@example.com",
                Email = "pmuser1@example.com"
            };
            var recipients = new List<ApplicationUser> {
                new ApplicationUser {
                    Email = "recipient1@example.com"
                },
                new ApplicationUser {
                    Email = "recipient2@example.com"
                }
            };
            var positionTest = new Position()
            {
                PositionId = 1,
                Title = "aTitle",
                Owner = userTest
            };
            Candidate candidate = new Candidate
            {
                CandidateId = 1
            };
            byte[] blob = new byte[1];
            HashSet<FileBlob> files = new HashSet<FileBlob>()
            {
                new FileBlob { Id = 1,  FileName = "aFile", Blob = blob },
                new FileBlob { Id = 2,  FileName = "aFile1", Blob = blob },
                new FileBlob { Id = 3,  FileName = "aFile2",  Blob = blob }
            };
            candidate.FileBlobs = files;
            CreateTechnicalInterviewViewModel model = new CreateTechnicalInterviewViewModel
            {
                CandidateEmail = "pablo@example.com",
                Comment = "A comment",
                Date = DateTime.Now,
                File = uploadedFile.Object,
                InterviewerId = 123456,
                InterviewerName = "Interviewer",
                PositionId = positionTest.PositionId,
                Result = true
            };

            mockUserHelper.Setup(p => p.GetUserByEmail("pablo@example.com")).Returns(userTest);
            mockPositionHelper.Setup(p => p.Get(1)).Returns(positionTest);
            mockCandidateHelper.Setup(p => p.Get(1)).Returns(candidate);
            TechnicalInterviewController controller = new TechnicalInterviewController(mockPositionHelper.Object, mockCandidateHelper.Object, mockUserHelper.Object)
            {
                ControllerContext = mockContext.Object
            };
            controller.Url = new UrlHelper(new System.Web.Routing.RequestContext { HttpContext = controller.HttpContext, RouteData = new System.Web.Routing.RouteData() });
            mockContext.SetupGet(c => c.HttpContext.Session["files"]).Returns(files);

            //Act
            var result = controller.InterviewFeedbackNewStatus(candidate.Email, positionTest, recipients);

            //Asserts
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.IsTrue(((FileResult)result).FileDownloadName == "MailExample.txt");
        }

        [TestMethod]
        public void NewTechnicalInterviewTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<ICandidate> mockCandidateHelper = mocks.Create<ICandidate>();
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            Mock<IMessenger> mockEmailManager = mocks.Create<IMessenger>();
            Mock<HttpContextBase> mockHttpcontext = mocks.Create<HttpContextBase>();
            var mockContext = new Mock<ControllerContext>();
            Mock<ApplicationUser> mockUser = mocks.Create<ApplicationUser>();
            Mock<UrlHelper> mockUrlHelper = mocks.Create<UrlHelper>();

            //Mock file
            string filePath = Path.GetFullPath(@"Files\pdftest1.pdf");
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            Mock<HttpPostedFileBase> uploadedFile = new Mock<HttpPostedFileBase>();

            uploadedFile
                .Setup(f => f.ContentLength)
                .Returns(10);

            uploadedFile
                .Setup(f => f.FileName)
                .Returns("pdftest1.pdf");

            uploadedFile
                .Setup(f => f.InputStream)
                .Returns(fileStream);


            //Setting up the context
            mockPrincipal.Setup(p => p.IsInRole("PM")).Returns(true);
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("pablo@example.com");
            mockContext.Setup(p => p.HttpContext.User.Identity.Name).Returns(mockPrincipal.Object.Identity.Name);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            mockContext.SetupGet(c => c.HttpContext.Session).Returns(mockHttpcontext.Object.Session);
            mockContext.Setup(c => c.HttpContext.Request.Url).Returns(new Uri("http://Talento.com"));
            mockHttpcontext.Setup(x => x.Request.ApplicationPath).Returns("/");
            mockHttpcontext.Setup(x => x.Request.Url).Returns(new Uri("http://Talento.com", UriKind.Absolute));
            mockUrlHelper
                .Setup(m => m.Action("Details", "Position", It.IsAny<object>(), It.IsAny<string>()))
                .Returns("http://Talento.com");

            //Test data
            var userTest = new ApplicationUser()
            {
                Id = "pmuser1@example.com",
                Email = "pmuser1@example.com"
            };
            var recipients = new List<ApplicationUser> {
                new ApplicationUser {
                    Email = "recipient1@example.com"
                },
                new ApplicationUser {
                    Email = "recipient2@example.com"
                }
            };
            Candidate candidate = new Candidate
            {
                CandidateId = 1,
                Email = "pablo@example.com"
            };
            var positionTest = new Position()
            {
                PositionId = 1,
                Title = "aTitle",
                Owner = userTest,
                PositionCandidates = new List<PositionCandidates> {
                    new PositionCandidates{
                        Candidate = candidate
                    }
                }
            };
            byte[] blob = new byte[1];
            HashSet<FileBlob> files = new HashSet<FileBlob>()
            {
                new FileBlob { Id = 1,  FileName = "aFile", Blob = blob },
                new FileBlob { Id = 2,  FileName = "aFile1", Blob = blob },
                new FileBlob { Id = 3,  FileName = "aFile2",  Blob = blob }
            };
            candidate.FileBlobs = files;
            CreateTechnicalInterviewViewModel model = new CreateTechnicalInterviewViewModel
            {
                CandidateEmail = "pablo@example.com",
                Comment = "A comment",
                Date = DateTime.Now,
                File = uploadedFile.Object,
                InterviewerId = 123456,
                InterviewerName = "Interviewer",
                PositionId = positionTest.PositionId,
                Result = true
            };

            mockUserHelper.Setup(p => p.GetUserByEmail("pablo@example.com")).Returns(userTest);
            mockUserHelper.Setup(p => p.GetByRoles(new List<string>() { "RMG", "TAG" })).Returns(new List<ApplicationUser>
            {
                new ApplicationUser { Id="1", Email="email1@example.com" },
                new ApplicationUser { Id="2", Email="email2@example.com" }
            });
            mockPositionHelper.Setup(p => p.Get(1)).Returns(positionTest);
            mockCandidateHelper.Setup(p => p.Get(1)).Returns(candidate);
            TechnicalInterviewController controller = new TechnicalInterviewController(mockPositionHelper.Object, mockCandidateHelper.Object, mockUserHelper.Object)
            {
                ControllerContext = mockContext.Object
            };

            controller.Url = new UrlHelper(new System.Web.Routing.RequestContext { HttpContext = controller.HttpContext, RouteData = new System.Web.Routing.RouteData() });

            var result = controller.NewTechnicalInterview(model);

            //Asserts
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileContentResult));
            Assert.IsTrue(((FileResult)result).FileDownloadName == "MailExample.txt");
        }
    }
}
