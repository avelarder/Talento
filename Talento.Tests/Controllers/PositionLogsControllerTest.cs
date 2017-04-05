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
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);

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
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(((ViewResult)result).Model is PositionLogViewModel);
            var viewModel = ((PositionLogViewModel)((ViewResult)result).Model);
            //Assert.IsTrue(viewModel. == 1);
            //Assert.IsTrue(viewModel is List<PositionModel>);
        }
    }
}
