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
using System.Web;

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

            Mock<ICustomUser> mUser = new Mock<ICustomUser>();
            mUser.Setup(p => p.SearchPM("test@test.com")).Returns(new ApplicationUser() { Email = "test@test.com", UserName = "test@test.com" });
            mUser.Setup(p => p.GetUser("test@test.com")).Returns(new ApplicationUser() { Email = "test@test.com", UserName = "test@test.com" });

            PositionsController controller = new PositionsController(position.Object, mUser.Object);

            var result = controller.Details(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(ViewResult));

            //Assert.IsTrue(((Task<ActionResult>)result).Result is DashBoardViewModel);
            //var viewmodel = ((DashBoardViewModel)((ViewResult)result).Model).Positions;
            //Assert.IsTrue(viewmodel.TotalCount == 1);
            //Assert.IsTrue(viewmodel.Subset is List<PositionModel>);
        }

        

        [TestMethod]
        public void CreateUsingPositionViwModel()
        {
            ApplicationUser appUser = new ApplicationUser();
            Position posPoco = new Position();
            posPoco.PortfolioManager = appUser;
            var mIIdentity = new Mock<IIdentity>();
            mIIdentity.Setup(p => p.Name).Returns("test@test.com");

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("PM")).Returns(true);
            mockPrincipal.Setup(p => p.Identity).Returns(mIIdentity.Object);

            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            Mock<IPosition> position = new Mock<IPosition>();
            Mock<ICustomUser> mUser= new Mock<ICustomUser>();
            mUser.Setup(p => p.SearchPM("test@test.com")).Returns(new ApplicationUser() { Email = "test@test.com", UserName = "test@test.com" });
            mUser.Setup(p => p.GetUser("test@test.com")).Returns(new ApplicationUser() { Email = "test@test.com", UserName = "test@test.com" });
            var positionViewModel = new CreatePositionViewModel()
            {
                Area = "TestArea",
                Description = "TestDescription",
                EngagementManager = "EM",
                RGS = "TestRGS",
                Title = "TestTitle",
                EmailPM = "test@test.com"                
            };

            var user = "test@test.com";
            appUser.UserName = user;

            Position positionCreate = new Position()
            {
                Owner = appUser,
                Id = 1,
                Area = positionViewModel.Area,
                CreationDate = DateTime.MaxValue,
                Description = positionViewModel.Description,
                Status = Status.Open,
                EngagementManager = positionViewModel.Description,
                //PortfolioManager = user,
                RGS = positionViewModel.RGS,
                Tags = null,
                Title = positionViewModel.Title
            };
            
            position.Setup(x => x.Create(positionCreate));
            
            var mController = new PositionsController(position.Object, mUser.Object);
            mController.ControllerContext = mockContext.Object;
            //mController.IsStateValid = () => { return true; };

            var result = mController.Create(positionViewModel);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, (typeof(RedirectToRouteResult)));
        }
       
    }
}