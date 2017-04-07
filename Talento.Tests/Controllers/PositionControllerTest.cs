using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Principal;
using System.Web.Mvc;
using Talento.Core;
using Talento.Entities;
using Talento.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talento.Models;
using System.Security.Claims;
using System.Net;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class PositionControllerTest
    {
        [TestMethod]
        public void GetPositionByIdTest()
        {
            ApplicationUser appUser = new ApplicationUser();
            Position posPoco = new Position();
            posPoco.PortfolioManager = appUser;

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            Mock<IPosition> position = new Mock<IPosition>();
            Position posParam = new Position()
            {
                Id = 1,
                Area = "fafaf",
                CreationDate = DateTime.MaxValue,
                Description = "lala",
                Status = Status.Open,
                EngagementManager = "lala",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "kjh",
                Tags = null,
                Title = ""
            };

            position.Setup(x => x.Get(1)).Returns((posParam));

            // create controller
            PositionsController controller = new PositionsController(position.Object);

            var result = controller.Details(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(ViewResult));

            var viewModel = (PositionModel)(((ViewResult)(((Task<ActionResult>)result).Result)).Model);
            Assert.IsNotNull(viewModel);
            Assert.IsTrue(viewModel is PositionModel);
        }

        [TestMethod]
        public void GetPositionIdNotFoundTest()
        {
            ApplicationUser appUser = new ApplicationUser();
            Position posPoco = new Position();
            posPoco.PortfolioManager = appUser;

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            Mock<IPosition> position = new Mock<IPosition>();
            Position posParam = new Position()
            {
                Id = 1,
                Area = "fafaf",
                CreationDate = DateTime.MaxValue,
                Description = "lala",
                Status = Status.Open,
                EngagementManager = "lala",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "kjh",
                Tags = null,
                Title = ""
            };

            // create controller
            PositionsController posController = new PositionsController(position.Object);

            var result = posController.Details(-1);
            var httpStatusCodeResult = ((HttpStatusCodeResult)(((Task<ActionResult>)result).Result));
            Assert.IsNotNull(httpStatusCodeResult);
            Assert.IsInstanceOfType(httpStatusCodeResult, typeof(HttpStatusCodeResult));
            Assert.IsTrue(((HttpStatusCodeResult)result.Result).StatusCode == 404);
        }

        [TestMethod]
        public void GetPositionIdIsNullTest()
        {
            ApplicationUser appUser = new ApplicationUser();
            Position posPoco = new Position();
            posPoco.PortfolioManager = appUser;

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            Mock<IPosition> position = new Mock<IPosition>();
            Position posParam = new Position()
            {
                Id = 1,
                Area = "fafaf",
                CreationDate = DateTime.MaxValue,
                Description = "lala",
                Status = Status.Open,
                EngagementManager = "lala",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "kjh",
                Tags = null,
                Title = ""
            };

            // create controller
            PositionsController posController = new PositionsController(position.Object);

            var result = posController.Details(null);
            var httpStatusCodeResult = ((HttpStatusCodeResult)(((Task<ActionResult>)result).Result));
            Assert.IsNotNull(httpStatusCodeResult);
            Assert.IsInstanceOfType(httpStatusCodeResult, typeof(HttpStatusCodeResult));
            Assert.IsTrue(((HttpStatusCodeResult)result.Result).StatusCode == 400);
        }

        [TestMethod]
        public void GetRemovedPositionStatusTest()
        {
            ApplicationUser appUser = new ApplicationUser();
            Position posPoco = new Position();
            posPoco.PortfolioManager = appUser;

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            Mock<IPosition> position = new Mock<IPosition>();
            Position posParam = new Position()
            {
                Id = 1,
                Area = "fafaf",
                CreationDate = DateTime.MaxValue,
                Description = "lala",
                Status = Status.Removed,
                EngagementManager = "lala",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "kjh",
                Tags = null,
                Title = ""
            };

            // create controller
            PositionsController posController = new PositionsController(position.Object);

            var result = posController.Details(1);
            var httpStatusCodeResult = ((HttpStatusCodeResult)(((Task<ActionResult>)result).Result));
            Assert.IsNotNull(httpStatusCodeResult);
            Assert.IsInstanceOfType(httpStatusCodeResult, typeof(HttpStatusCodeResult));
            Assert.IsTrue(((HttpStatusCodeResult)result.Result).StatusCode == 404);
        }

        public void DeleteTest()
        {
            var claim = new Claim("test", "UserTestId");
            var mockIdentity = Mock.Of<ClaimsIdentity>(id => id.FindFirst(It.IsAny<string>()) == claim);
            Mock<IPosition> positionhelper = new Mock<IPosition>();
            var mockContext = Mock.Of<ControllerContext>(c => c.HttpContext.User == mockIdentity);
            PositionsController controller = new PositionsController(positionhelper.Object)
            {
                ControllerContext = mockContext
            };

            ApplicationUser appUser = new ApplicationUser();
            Position posPoco = new Position();
            posPoco.PortfolioManager = appUser;
            Mock<IPosition> position = new Mock<IPosition>();
            Position posParam = new Position()
            {
                Id = 1,
                Area = "fafaf",
                CreationDate = DateTime.MaxValue,
                Description = "lala",
                Status = Status.Open,
                EngagementManager = "lala",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "kjh",
                Tags = null,
                Title = ""
            };

            position.Setup(x => x.Get(1)).Returns(posParam);
            var result = await controller.Delete(posParam.Id);

            Assert.IsNotNull(result);
            Assert.IsNotInstanceOfType(result, typeof(RedirectToRouteResult));
        }

    }
}