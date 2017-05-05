
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Talento.Controllers;
using Talento.Core;
using Talento.Entities;
using Talento.Models;
using Talento.EmailManager;
using System.Security.Claims;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class CandidateControllerTest
    {
        [TestMethod]
        public void NewTest()
        {
            //Mocking interfaces and entities needed
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<ICandidate> mockCandidateHelper = mocks.Create<ICandidate>();
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            Mock<IMessenger> mockEmailManager = mocks.Create<IMessenger>();
            var mockContext = new Mock<ControllerContext>();
            Mock<ApplicationUser> mockUser = mocks.Create<ApplicationUser>();

            //Setting up the context
            mockPrincipal.Setup(p => p.IsInRole("PM")).Returns(true);
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("pablo@example.com");
            mockContext.Setup(p => p.HttpContext.User.Identity.Name).Returns(mockPrincipal.Object.Identity.Name);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            //Creating test data
            var userTest = new ApplicationUser()
            {
                Id = "pablo@example.com",
                Email = "pablo@example.com"
            };
            var positionTest = new Position()
            {
                PositionId = 1,
                Title = "aTitle",
            };
            Candidate candidate = new Candidate();
            byte[] blob = new byte[1];
            HashSet<FileBlob> files = new HashSet<FileBlob>()
            {
                new FileBlob { Id = 1,  FileName = "aFile", Blob = blob },
                new FileBlob { Id = 2,  FileName = "aFile1", Blob = blob },
                new FileBlob { Id = 3,  FileName = "aFile2",  Blob = blob }
            };
            candidate.FileBlobs = files;
            CreateCandidateViewModel candidateViewModel = new CreateCandidateViewModel
            {
                Position_Id = 1,
                Competencies = "someCompetencies",
                CratedOn = DateTime.Now,
                CreatedBy = mockUser.Object,
                Description = "aDescription",
                Email = "aMail@tcs.com",
                Name = "pepito",
                IsTcsEmployee = "on",
                CreatedBy_Id = "1"
            };

            //Creating the controller and sending de test data to it
            mockUserHelper.Setup(p => p.GetUserByEmail("pablo@example.com")).Returns(userTest);
            mockPositionHelper.Setup(p => p.Get(1)).Returns(positionTest);
            CandidateController controller = new CandidateController(mockCandidateHelper.Object, mockUserHelper.Object,
                                                mockPositionHelper.Object, mockEmailManager.Object)
            {
                ControllerContext = mockContext.Object
            };
            mockContext.SetupGet(c => c.HttpContext.Session["files"]).Returns(files);

            //Act
            var result = controller.Create(candidateViewModel);

            //Asserts
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void EditTest()
        {
            //Mocking interfaces and entities needed
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<ICandidate> mockCandidateHelper = mocks.Create<ICandidate>();
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            Mock<IMessenger> mockEmailManager = mocks.Create<IMessenger>();
            Mock<HttpContextBase> mockHttpcontext = mocks.Create<HttpContextBase>();
            var mockContext = new Mock<ControllerContext>();
            Mock<ApplicationUser> mockUser = mocks.Create<ApplicationUser>();

            //Setting up the context
            mockPrincipal.Setup(p => p.IsInRole("PM")).Returns(true);
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("pablo@example.com");
            mockContext.Setup(p => p.HttpContext.User.Identity.Name).Returns(mockPrincipal.Object.Identity.Name);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            mockContext.SetupGet(c => c.HttpContext.Session).Returns(mockHttpcontext.Object.Session);

            //Creating test data
            var userTest = new ApplicationUser()
            {
                Id = "pablo@example.com",
                Email = "pablo@example.com"
            };
            var positionTest = new Position()
            {
                PositionId = 1,
                Title = "aTitle",
            };
            byte[] blob = new byte[1];
            HashSet<FileBlob> files = new HashSet<FileBlob>
            {
                new FileBlob { Id = 1, FileName = "aFile", Blob = blob },
                new FileBlob { Id = 2, FileName = "aFile1", Blob = blob },
                new FileBlob { Id = 3, FileName = "aFile2", Blob = blob }
            };
            Candidate candidate = new Candidate
            {
                Email = "Candidate00@Example.com",
                FileBlobs = files
            };
            EditCandidateViewModel candidateViewModel = new EditCandidateViewModel
            {
                CandidateId = 1,
                Position_Id = 1,
                Competencies = "someCompetencies",
                CratedOn = DateTime.Now,
                CreatedBy = mockUser.Object,
                Description = "aDescription",
                Email = "aMail@tcs.com",
                Name = "pepito",
                IsTcsEmployee = "on",
                CreatedBy_Id = "1"
            };

            //Creating the controller and sending de test data to it
            mockUserHelper.Setup(p => p.GetUserByEmail("pablo@example.com")).Returns(userTest);
            mockPositionHelper.Setup(p => p.Get(1)).Returns(positionTest);
            mockCandidateHelper.Setup(p => p.Get(1)).Returns(candidate);
            CandidateController controller = new CandidateController(mockCandidateHelper.Object, mockUserHelper.Object,
                                                mockPositionHelper.Object, mockEmailManager.Object)
            {
                ControllerContext = mockContext.Object
            };
            mockContext.SetupGet(c => c.HttpContext.Session["files"]).Returns(files);

            //Act
            var result = controller.Edit(candidateViewModel);

            //Asserts
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

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
            EditCandidateViewModel candidateViewModel = new EditCandidateViewModel
            {
                Position_Id = 1,
                Competencies = "someCompetencies",
                CratedOn = DateTime.Now,
                CreatedBy = mockUser.Object,
                Description = "aDescription",
                Email = "aMail@tcs.com",
                Name = "pepito",
                IsTcsEmployee = "on",
                CreatedBy_Id = "1",
                PositionCandidates = new List<PositionCandidates>
                {
                    new PositionCandidates{
                        Candidate = candidate,
                        Position = positionTest,
                        Status = PositionCandidatesStatus.Interview_Accepted
                    }
                },
                CandidateId = 1
            };

            mockUserHelper.Setup(p => p.GetUserByEmail("pablo@example.com")).Returns(userTest);
            mockPositionHelper.Setup(p => p.Get(1)).Returns(positionTest);
            mockCandidateHelper.Setup(p => p.Get(1)).Returns(candidate);
            CandidateController controller = new CandidateController(mockCandidateHelper.Object, mockUserHelper.Object,
                                                mockPositionHelper.Object, mockEmailManager.Object)
            {
                ControllerContext = mockContext.Object
            };
            controller.Url = new UrlHelper(new System.Web.Routing.RequestContext { HttpContext = controller.HttpContext, RouteData = new System.Web.Routing.RouteData() });
            mockContext.SetupGet(c => c.HttpContext.Session["files"]).Returns(files);

            //Act
            var result = controller.InterviewFeedbackNewStatus(candidateViewModel, positionTest, recipients);

            //Asserts
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.IsTrue(((FileResult)result).FileDownloadName == "MailExample.txt");
        }

        [TestMethod]
        public void StatusTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<ICandidate> mockCandidateHelper = mocks.Create<ICandidate>();
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            Mock<IMessenger> mockEmailManager = mocks.Create<IMessenger>();
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            Mock<ControllerContext> mockContext = mocks.Create<ControllerContext>();
            mockPrincipal.Setup(x => x.IsInRole("TAG")).Returns(true);
            mockContext.Setup(p => p.HttpContext.User.IsInRole("TAG")).Returns(true);

            CandidateController controller = new CandidateController(mockCandidateHelper.Object, mockUserHelper.Object, mockPositionHelper.Object, mockEmailManager.Object)
            {
                ControllerContext = mockContext.Object
            };

            Position aPosition = new Position()
            {
                PositionId = 1,
                Status=PositionStatus.Open
            };

            Candidate aCandidate = new Candidate() { CandidateId = 1 };

            List<PositionCandidates> list = new List<PositionCandidates>();

            PositionCandidates pc = new PositionCandidates()
            {
                PositionID = 1,
                Position = aPosition,
                CandidateID = 1,
                Candidate = aCandidate,
                Status = PositionCandidatesStatus.Interview_Accepted
            };

            list.Add(pc);

            ApplicationUser aPm = new ApplicationUser();

            PositionModel Pm = new PositionModel()
            {
                PositionId = 1,
                Title = "aTitle",
                Description = "aDescription",
                CreationDate = DateTime.Now,
                Area = "someArea",
                EngagementManager = "anEm",
                PortfolioManager_Id = "1",
                PortfolioManager = aPm,
                RGS = "1",
                Status = PositionStatus.Open,
                Owner = aPm,
                PositionCandidates = list
            };

            var result = controller.Status(Pm, 1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsTrue(((PartialViewResult)result).ViewData.ContainsKey("Status"));
        }

    }
}