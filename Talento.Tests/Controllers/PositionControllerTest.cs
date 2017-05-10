
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
using Talento.Core.Utilities;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class PositionControllerTest
    {
        [TestMethod]
        public void GetPositionByIdTest()
        {
            ApplicationUser appUser = new ApplicationUser();
            Position posPoco = new Position()
            {
                PortfolioManager = appUser
            };
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            Mock<IPosition> position = new Mock<IPosition>();
            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();

            Position posParam = new Position()
            {
                PositionId = 5,
                Area = "IT",
                CreationDate = DateTime.MaxValue,
                Description = ".Net Developer",
                Status = PositionStatus.Open,
                EngagementManager = "Arthur",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "Unknown101",
                Tags = null,
                Title = "Need a Developer to do stuff"
            };

            position.Setup(x => x.Get(5)).Returns((posParam));

            Mock<ICustomUser> mUser = new Mock<ICustomUser>();
            mUser.Setup(u => u.SearchPM("")).Returns(new ApplicationUser());
            mUser.Setup(p => p.SearchPM("test@test.com")).Returns(new ApplicationUser()
            {
                Email = "test@test.com",
                UserName = "test@test.com"

            });
            mUser.Setup(p => p.GetUserByEmail("test@test.com")).Returns(new ApplicationUser()
            {
                Email = "test@test.com",
                UserName = "test@test.com"

            });

            Mock<ICandidate> mCandidate = new Mock<ICandidate>();
            PositionsController controller = new PositionsController(position.Object, mUser.Object, mCandidate.Object, utilAppSetting.Object);

            var result = controller.Details(5, 1);
            var viewModel = (PositionModel)(((ViewResult)result).Model);

            Assert.IsNotNull(result);
            Assert.IsNotNull(viewModel);
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
            Mock<ICustomUser> user = new Mock<ICustomUser>();
            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();

            Position posParam = new Position()
            {
                PositionId = 1,
                Area = "IT",
                CreationDate = DateTime.MaxValue,
                Description = ".Net Developer",
                Status = PositionStatus.Open,
                EngagementManager = "Arthur",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "Unknown101",
                Tags = null,
                Title = "Need a Developer to do stuff"
            };

            // create controller
            Mock<ICandidate> mCandidate = new Mock<ICandidate>();
            PositionsController posController = new PositionsController(position.Object, user.Object, mCandidate.Object, utilAppSetting.Object);

            var result = posController.Details(-1, 1);
            //var httpStatusCodeResult = ((HttpStatusCodeResult)(((ActionResult)result)));
            //Assert.IsNotNull(httpStatusCodeResult);
            //Assert.IsInstanceOfType(httpStatusCodeResult, typeof(HttpStatusCodeResult));
            //Assert.IsTrue(((HttpStatusCodeResult)result).StatusCode == 404);
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
            Mock<ICustomUser> user = new Mock<ICustomUser>();
            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();

            Position posParam = new Position()
            {
                PositionId = 1,
                Area = "IT",
                CreationDate = DateTime.MaxValue,
                Description = ".Net Developer",
                Status = PositionStatus.Open,
                EngagementManager = "Arthur",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "Unknown101",
                Tags = null,
                Title = "Need a Developer to do stuff"
            };

            // create controller
            Mock<ICandidate> mCandidate = new Mock<ICandidate>();
            PositionsController posController = new PositionsController(position.Object, user.Object, mCandidate.Object, utilAppSetting.Object);

            var result = posController.Details(null, null);
            var httpStatusCodeResult = ((HttpStatusCodeResult)(((ActionResult)result))); // .Result
            Assert.IsNotNull(httpStatusCodeResult);
            Assert.IsInstanceOfType(httpStatusCodeResult, typeof(HttpStatusCodeResult));
            Assert.IsTrue(((HttpStatusCodeResult)result).StatusCode == 400);
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
            Mock<ICustomUser> user = new Mock<ICustomUser>();
            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();

            Position posParam = new Position()
            {
                PositionId = 1,
                Area = "IT",
                CreationDate = DateTime.MaxValue,
                Description = ".Net Developer",
                Status = PositionStatus.Open,
                EngagementManager = "Arthur",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "Unknown101",
                Tags = null,
                Title = "Need a Developer to do stuff"
            };

            // create controller
            Mock<ICandidate> mCandidate = new Mock<ICandidate>();
            PositionsController posController = new PositionsController(position.Object, user.Object, mCandidate.Object, utilAppSetting.Object);

            var result = posController.Details(5, 1);
            //var httpStatusCodeResult = ((HttpStatusCodeResult)(((ActionResult)result))); // .Result
            //Assert.IsNotNull(httpStatusCodeResult);
            //Assert.IsInstanceOfType(httpStatusCodeResult, typeof(HttpStatusCodeResult));
            //Assert.IsTrue(((HttpStatusCodeResult)result).StatusCode == 404); // .Result
        }

        [TestMethod]
        public void DeleteTest()
        {
            ApplicationUser appUser = new ApplicationUser();
            Position posPoco = new Position();
            posPoco.PortfolioManager = appUser;

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns(mockPrincipal.Name);
            Mock<IPosition> positionhelper = new Mock<IPosition>();
            Mock<ICustomUser> userhelper = new Mock<ICustomUser>();
            var mockContext = Mock.Of<ControllerContext>(c => c.HttpContext.User == mockPrincipal.Object);

            Mock<ICandidate> mCandidate = new Mock<ICandidate>();
            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();

            PositionsController controller = new PositionsController(positionhelper.Object, userhelper.Object, mCandidate.Object, utilAppSetting.Object)
            {
                ControllerContext = mockContext
            };

            Position posParam = new Position()
            {
                PositionId = 1,
                Area = "IT",
                CreationDate = DateTime.MaxValue,
                Description = ".Net Developer",
                Status = PositionStatus.Open,
                EngagementManager = "Arthur",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "Unknown101",
                Tags = null,
                Title = "Need a Developer to do stuff"
            };

            positionhelper.Setup(x => x.Get(1)).Returns(posParam);
            var result = controller.Delete(posParam.PositionId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void CreateUsingPositionViewModel()
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
            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();
            Mock<ICustomUser> mUser = new Mock<ICustomUser>();
            mUser.Setup(p => p.SearchPM("test@test.com")).Returns(new ApplicationUser()
            {
                Email = "test@test.com",
                UserName = "test@test.com"

            });
            mUser.Setup(p => p.GetUserByEmail("test@test.com")).Returns(new ApplicationUser()
            {
                Email = "test@test.com",
                UserName = "test@test.com"

            });
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
                PositionId = 1,
                Area = positionViewModel.Area,
                CreationDate = DateTime.MaxValue,
                Description = positionViewModel.Description,
                Status = PositionStatus.Open,
                EngagementManager = positionViewModel.Description,
                //PortfolioManager = user,
                RGS = positionViewModel.RGS,
                Tags = null,
                Title = positionViewModel.Title
            };

            position.Setup(x => x.Create(positionCreate));

            Mock<ICandidate> mCandidate = new Mock<ICandidate>();
            var mController = new PositionsController(position.Object, mUser.Object, mCandidate.Object, utilAppSetting.Object);
            mController.ControllerContext = mockContext.Object;

            //mController.IsStateValid = () => { return true; };

            mController.IsStateValid().Equals(true);

            var result = mController.Create(positionViewModel);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, (typeof(RedirectToRouteResult)));
        }

        [TestMethod]
        public void GetCandidatesByPositionIdTest()
        {
            ApplicationUser appUser = new ApplicationUser();
            Position posPoco = new Position()
            {
                PortfolioManager = appUser
            };
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
                PositionId = 5,
                Area = "IT",
                CreationDate = DateTime.MaxValue,
                Description = ".Net Developer",
                Status = PositionStatus.Open,
                EngagementManager = "Arthur",
                PortfolioManager = posPoco.PortfolioManager,
                Owner = appUser,
                RGS = "Unknown101",
                Tags = null,
                Title = "Need a Developer to do stuff",
                PositionCandidates = new List<PositionCandidates> {
                    new PositionCandidates{
                        Candidate = new Candidate{
                            Email = "candidate1@tcs.com"
                        }
                    },
                    new PositionCandidates{
                        Candidate = new Candidate{
                            Email = "candidate2@tcs.com"
                        }
                    },
                }
            };

            position.Setup(x => x.Get(5)).Returns((posParam));

            Mock<IUtilityApplicationSettings> utilAppSetting = new Mock<IUtilityApplicationSettings>();
            Mock<ICustomUser> mUser = new Mock<ICustomUser>();
            mUser.Setup(u => u.SearchPM("")).Returns(new ApplicationUser());
            mUser.Setup(p => p.SearchPM("test@test.com")).Returns(new ApplicationUser()
            {
                Email = "test@test.com",
                UserName = "test@test.com"

            });
            mUser.Setup(p => p.GetUserByEmail("test@test.com")).Returns(new ApplicationUser()
            {
                Email = "test@test.com",
                UserName = "test@test.com"
            });

            Mock<ICandidate> mCandidate = new Mock<ICandidate>();
            PositionsController controller = new PositionsController(position.Object, mUser.Object, mCandidate.Object, utilAppSetting.Object);

            var result = controller.Details(5, 1);

            Assert.IsNotNull(result);
            var viewmodel = (PositionModel)(((ViewResult)result).Model);

            Assert.IsNotNull(viewmodel);
            Assert.IsInstanceOfType(viewmodel, typeof(PositionModel));
            Assert.AreEqual(viewmodel.PositionId, posParam.PositionId);
        }
    }
}