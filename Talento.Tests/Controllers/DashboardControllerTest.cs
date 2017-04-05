using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Talento.Controllers;
using Talento.Core.Data;
using Talento.Core.Helpers;
using System.Web;
using System.Security.Principal;
using System.Web.Mvc;
using Talento.Models;
using System.Web.Security;
using Talento.Tests.Providers;
using Talento.Core;
using System.Collections.Generic;
using Talento.Entities;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class DashboardControllerTest
    {
        [TestMethod]
        public void IndexAdmin()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            // create controller
            Mock<ICustomPagingList> mCustomPagingList = new Mock<ICustomPagingList>();
            mCustomPagingList.Setup(x => x.GetAdminTable("", "Status", "", "", 1)).Returns(new List<Position>() { new Position { Id = 1 } });
            DashboardController controller = new DashboardController(mCustomPagingList.Object);

            var result = controller.Index("", "Status", "", "", 1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(((ViewResult)result).Model is DashBoardViewModel);
            var viewmodel = ((DashBoardViewModel)((ViewResult)result).Model).Positions;
            Assert.IsTrue(viewmodel.TotalItemCount == 1);
            Assert.IsTrue(viewmodel.Subset is List<PositionModel>);
        }

        [TestMethod]
        public void IndexBasic()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Basic")).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            // create controller
            Mock<ICustomPagingList> mCustomPagingList = new Mock<ICustomPagingList>();
            mCustomPagingList.Setup(x => x.GetAdminTable("", "Status", "", "", 1)).Returns(new List<Position>() { new Position { Id = 1 } });
            DashboardController controller = new DashboardController(mCustomPagingList.Object);

            var result = controller.Index("", "Status", "", "", 1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(((ViewResult)result).Model is DashBoardViewModel);
            var viewmodel = ((DashBoardViewModel)((ViewResult)result).Model).Positions;
            Assert.IsTrue(viewmodel.TotalItemCount == 1);
            Assert.IsTrue(viewmodel.Subset is List<PositionModel>);
        }
    }
}
