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
using Talento.Core.Utilities;
using System.Web.Routing;
using System.Web;

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

            // Data
            List<Log> position = new List<Log>() {
                new Log()
                {
                    Id=1,
                } 
            };
            Pagination pagination = new Pagination()
            {
                Prev = 1,
                Next = 3,
                Current = 2,
                Total = 6,
                Url = "#"
            };

            // Mock Request HTTP
            var request = new Mock<HttpRequestBase>();
            // Set isAjax Request
            request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    {"X-Requested-With", "XMLHttpRequest"}
                });
            // Set isLocal
            request.SetupGet(x => x.IsLocal).Returns(true);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            // Controller
            Mock<IPositionLog> logsList = new Mock<IPositionLog>();
            logsList.Setup(x => x.PaginateLogs(1,1,1,"#")).Returns(new Tuple<List<Log>, Pagination>(position,pagination));
            PositionLogsController controller = new PositionLogsController(logsList.Object);
            // Load Mock request to Controller
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            
            // HTTP context to use URL.Action
            var UrlHelperMock = new Mock<UrlHelper>();
            controller.Url = UrlHelperMock.Object;
            UrlHelperMock.Setup(x => x.Action("List", "PositionLogs")).Returns("#");

            // Call controller Mocked
            var result = controller.List(1, 1, 1, "#");

            // Asserts
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

            // Data
            List<Log> position = new List<Log>() {
                new Log()
                {
                    Id=1,
                }
            };
            Pagination pagination = new Pagination()
            {
                Prev = 1,
                Next = 3,
                Current = 2,
                Total = 6,
                Url = "#"
            };

            // Mock Request HTTP
            var request = new Mock<HttpRequestBase>();
            // Set isAjax Request
            request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    {"X-Requested-With", "XMLHttpRequest"}
                });
            // Set isLocal
            request.SetupGet(x => x.IsLocal).Returns(true);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            // Controller
            Mock<IPositionLog> logsList = new Mock<IPositionLog>();
            logsList.Setup(x => x.PaginateLogs(1, 1, 1, "#")).Returns(new Tuple<List<Log>, Pagination>(position, pagination));
            PositionLogsController controller = new PositionLogsController(logsList.Object);

            // Load Mock request to Controller
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            // HTTP context to use URL.Action
            var UrlHelperMock = new Mock<UrlHelper>();
            controller.Url = UrlHelperMock.Object;
            UrlHelperMock.Setup(x => x.Action("List", "PositionLogs")).Returns("#");

            var result = controller.List(2, 1, 1, "#");

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

            // Data
            List<Log> position = new List<Log>() {
                new Log()
                {
                    Id=1,
                }
            };
            Pagination pagination = new Pagination()
            {
                Prev = 1,
                Next = 3,
                Current = 2,
                Total = 6,
                Url = "#"
            };

            // Mock Request HTTP
            var request = new Mock<HttpRequestBase>();
            // Set isAjax Request
            request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    {"X-Requested-With", "XMLHttpRequest"}
                });
            // Set isLocal
            request.SetupGet(x => x.IsLocal).Returns(true);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            // Controller
            Mock<IPositionLog> logsList = new Mock<IPositionLog>();
            logsList.Setup(x => x.PaginateLogs(1, 1, 1, "#")).Returns(new Tuple<List<Log>, Pagination>(position, pagination));
            PositionLogsController controller = new PositionLogsController(logsList.Object);

            // Load Mock request to Controller
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            // HTTP context to use URL.Action
            var UrlHelperMock = new Mock<UrlHelper>();
            controller.Url = UrlHelperMock.Object;
            UrlHelperMock.Setup(x => x.Action("List", "PositionLogs")).Returns("#");

            var result = controller.List(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.IsTrue(((HttpNotFoundResult)result).StatusCode == 404);
        }

        [TestMethod]
        public void ListPageNotExist()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("PM")).Returns(true);

            // Mock Controller of Context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            // Data
            List<Log> position = new List<Log>() {
                new Log()
                {
                    Id=1,
                }
            };
            Pagination pagination = new Pagination()
            {
                Prev = 1,
                Next = 3,
                Current = 2,
                Total = 6,
                Url = "#"
            };

            // Mock Request HTTP
            var request = new Mock<HttpRequestBase>();
            // Set isAjax Request
            request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    {"X-Requested-With", "XMLHttpRequest"}
                });
            // Set isLocal
            request.SetupGet(x => x.IsLocal).Returns(true);

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            // Controller
            Mock<IPositionLog> logsList = new Mock<IPositionLog>();
            logsList.Setup(x => x.PaginateLogs(1, 10, 1, "#")).Returns(new Tuple<List<Log>, Pagination>(position, pagination));
            PositionLogsController controller = new PositionLogsController(logsList.Object);

            // Load Mock request to Controller
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            // HTTP context to use URL.Action
            var UrlHelperMock = new Mock<UrlHelper>();
            controller.Url = UrlHelperMock.Object;
            UrlHelperMock.Setup(x => x.Action("List", "PositionLogs")).Returns("#");

            var result = controller.List(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.IsTrue(((HttpNotFoundResult)result).StatusCode == 404);
        }
    }
}
