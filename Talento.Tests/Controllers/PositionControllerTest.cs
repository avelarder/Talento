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

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class PositionControllerTest
    {
        [TestMethod]
        public void GetPositionById()
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

            // create controller
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

            position.Setup(x => x.Get(1)).Returns(Task.FromResult(posParam));

            PositionsController controller = new PositionsController(position.Object);

            var result = controller.Details(1);

            //Assert.IsNotNull(result);
            //Assert.IsInstanceOfType(result, typeof(ViewResult));
            //Assert.IsTrue(((ViewResult) result).Model is DashBoardViewModel);
            //var viewmodel = ((DashBoardViewModel)((ViewResult)result).Model).Positions;
            //Assert.IsTrue(viewmodel.TotalCount == 1);
            //Assert.IsTrue(viewmodel.Subset is List<PositionModel>);
        }
    }
}














