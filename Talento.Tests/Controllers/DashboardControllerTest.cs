using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Talento.Controllers;
using System.Security.Principal;
using System.Web.Mvc;
using Talento.Models;
using Talento.Core;
using System.Collections.Generic;
using Talento.Entities;
using Talento.Core.Utilities;

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
            Mock<ICustomUser> mockUser = mocks.Create<ICustomUser>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            mockContext.Setup(p => p.HttpContext.User.IsInRole("Admin")).Returns(true);

            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();

            // create controller
            Mock<ICustomPagingList> mCustomPagingList = new Mock<ICustomPagingList>();
            mCustomPagingList.Setup(x => x.GetTable("", "Status", "", "", 1)).Returns(new List<Position>() { new Position { PositionId = 1 } });
            DashboardController controller = new DashboardController(mCustomPagingList.Object, utilAppSetting.Object, mockUser.Object)
            {
                ControllerContext = mockContext.Object
            };

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
            Mock<ICustomUser> mockUser = mocks.Create<ICustomUser>();
            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            mockContext.Setup(p => p.HttpContext.User.IsInRole("Basic")).Returns(true);

            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();
            // create controller
            Mock<ICustomPagingList> mCustomPagingList = new Mock<ICustomPagingList>();
            mCustomPagingList.Setup(x => x.GetTable("", "Status", "", "", 1)).Returns(new List<Position>() { new Position { PositionId = 1 } });
            DashboardController controller = new DashboardController(mCustomPagingList.Object, utilAppSetting.Object,mockUser.Object)
            {
                ControllerContext = mockContext.Object
            };

            var result = controller.Index("", "Status", "", "", 1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(((ViewResult)result).Model is DashBoardViewModel);
            var viewmodel = ((DashBoardViewModel)((ViewResult)result).Model).Positions;
            Assert.IsTrue(viewmodel.TotalItemCount == 1);
            Assert.IsTrue(viewmodel.Subset is List<PositionModel>);
        }
        [TestMethod]
        public void Edit()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);
            Mock<ICustomUser> mockUser = mocks.Create<ICustomUser>();
            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            mockContext.Setup(p => p.HttpContext.User.IsInRole("Admin")).Returns(true);

            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();
            // create controller
            Mock<ICustomPagingList> mCustomPagingList = new Mock<ICustomPagingList>();
            mCustomPagingList.Setup(x => x.GetTable("", "Status", "", "", 1)).Returns(new List<Position>() { new Position { PositionId = 1 } });
            DashboardController controller = new DashboardController(mCustomPagingList.Object, utilAppSetting.Object, mockUser.Object)
            {
                ControllerContext = mockContext.Object
            };

            var result = controller.Index("", "Status", "", "", 1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsTrue(((ViewResult)result).Model is DashBoardViewModel);
            var viewmodel = ((DashBoardViewModel)((ViewResult)result).Model).Positions;
            Assert.IsTrue(viewmodel.TotalItemCount == 1);
            Assert.IsTrue(viewmodel.Subset is List<PositionModel>);
        }

        [TestMethod]
        public void ManageUsersTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();
            Mock<ICustomPagingList> dashboardPagingHelper = new Mock<ICustomPagingList>();
            Mock<ICustomUser> mockUser = mocks.Create<ICustomUser>();
            DashboardController controller = new DashboardController(dashboardPagingHelper.Object, utilAppSetting.Object,mockUser.Object);

            var result = controller.ManageUser();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void AppSettingsTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<ICustomUser> mockUser = mocks.Create<ICustomUser>();
            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();
            Mock<ICustomPagingList> dashboardPagingHelper = new Mock<ICustomPagingList>();
            DashboardController controller = new DashboardController(dashboardPagingHelper.Object, utilAppSetting.Object, mockUser.Object);

            var result = controller.AppSettings();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void DownloadXlTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();
            Mock<ICustomPagingList> dashboardPagingHelper = new Mock<ICustomPagingList>();
            dashboardPagingHelper.Setup(x => x.CreateXML("id_desc", "Status", null, "", null)).Returns("filepath/");
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            DashboardController controller = new DashboardController(dashboardPagingHelper.Object, utilAppSetting.Object, mockUserHelper.Object)
            {
                ControllerContext = mockContext.Object
            };

            var result = controller.DownloadXl("id_desc", "Status", "");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FilePathResult));
            Assert.IsTrue(((FilePathResult)result).ContentType == "application/msexcel");
            Assert.IsTrue(((FilePathResult)result).FileDownloadName == "OpenPositions.xlsx");
            Assert.IsTrue(((FilePathResult)result).FileName == "filepath/");
        }

        //        [TestMethod]

        //public void DownloadTiffTest()
        //{
        //var mocks = new MockRepository(MockBehavior.Default);
        //Mock<ICustomPagingList> mockPagingList = mocks.Create<ICustomPagingList>();
        //Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
        //Mock<IApplicationSetting> mockSettingsHelper = mocks.Create<IApplicationSetting>();
        //Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
        //mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
        //DashboardController controller = new DashboardController(mockPagingList.Object)
        //{
        //    ControllerContext = mockContext.Object
        //};

        //var result = controller.DownloadTiffTemplate();
        //Assert.IsNotNull(result);
        //Assert.IsInstanceOfType(result, typeof(FilePathResult));
        //Assert.IsTrue(((FilePathResult)result).ContentType == "application/ms-word");
        //Assert.IsTrue(((FilePathResult)result).FileName == "~/Content/Files/Template_TIFF.doc");
        //}
    }
}
