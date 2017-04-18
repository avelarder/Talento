
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Talento.Controllers;
using Talento.Core;
using Talento.Entities;
using Talento.Models;

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
            Mock<IPositionCandidate> mockPositionCandidateHelper = mocks.Create<IPositionCandidate>();
            Mock<IFileManagerHelper> mockFileManagerHelper = mocks.Create<IFileManagerHelper>();
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
                Id = 1,
                Title = "aTitle",
            };
            Candidate candidate = new Candidate();
            byte[] blob = new byte[1];
            List<FileBlob> files = new List<FileBlob>()
            {
                new FileBlob { Id = 1, Candidate_Id = 1, FileName = "aFile", Candidate = candidate, Blob = blob },
                new FileBlob { Id = 2, Candidate_Id = 1, FileName = "aFile1", Candidate = candidate, Blob = blob },
                new FileBlob { Id = 3, Candidate_Id = 1, FileName = "aFile2", Candidate = candidate, Blob = blob }
            };
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
                Status = CandidateStatus.New,
                CreatedBy_Id = "1"
            };

            //Creating the controller and sending de test data to it
            mockUserHelper.Setup(p => p.GetUserByEmail("pablo@example.com")).Returns(userTest);
            mockPositionHelper.Setup(p => p.Get(1)).Returns(positionTest);
            CandidateController controller = new CandidateController(mockCandidateHelper.Object, mockUserHelper.Object,
                                                mockPositionCandidateHelper.Object, mockFileManagerHelper.Object,
                                                mockPositionHelper.Object)
            {
                ControllerContext = mockContext.Object
            };
            mockContext.SetupGet(c => c.HttpContext.Session["files"]).Returns(files);

            //Act
            var result = controller.New(candidateViewModel);

            //Asserts
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
    }
}