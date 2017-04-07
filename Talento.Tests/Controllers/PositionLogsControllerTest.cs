using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Principal;
using System.Web.Mvc;
using Talento.Core;
using Talento.Entities;
using System.Collections.Generic;
using Talento.Controllers;
using Talento.Models;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class PositionLogsControllerTest
    {
        [TestMethod]
        public void List()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("PM")).Returns(true);

            // Mock Controller of Context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            // Controller
            Mock<IPositionLog> logsList = new Mock<IPositionLog>();
            logsList.Setup(x => x.GetAll(1)).Returns(new List<PositionLog>() { new PositionLog { Id = 1 } });
            PositionLogsController controller = new PositionLogsController(logsList.Object);

            var result = controller.List(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.IsTrue(((PartialViewResult)result).Model is List<PositionLogViewModel>);
            var viewModel = ((List<PositionLogViewModel>)(((PartialViewResult)result).Model));
            Assert.IsTrue(viewModel.Count == 1);
        }

        [TestMethod]
        public void ListIdNotFound()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("PM")).Returns(true);

            // Mock Controller of Context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            // Controller
            Mock<IPositionLog> logsList = new Mock<IPositionLog>();
            PositionLogsController controller = new PositionLogsController(logsList.Object);

            var result = controller.List(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.IsTrue(((HttpNotFoundResult)result).StatusCode == 404);
        }

        [TestMethod]
        public void ListIdNull()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("PM")).Returns(true);

            // Mock Controller of Context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            // Controller
            Mock<IPositionLog> logsList = new Mock<IPositionLog>();
            PositionLogsController controller = new PositionLogsController(logsList.Object);

            var result = controller.List(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.IsTrue(((HttpNotFoundResult)result).StatusCode == 404);
        }
    }
}
