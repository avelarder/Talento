using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Talento.Controllers;
using Talento.Core;
using Talento.Entities;
using Talento.Models;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class SettingsControllerTest
    {
        [TestMethod]
        public void CreateTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            Mock<IApplicationSetting> mockSettingsHelper = mocks.Create<IApplicationSetting>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            var mockContext = new Mock<ControllerContext>();
            Mock<ApplicationUser> mockUser = mocks.Create<ApplicationUser>();

            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("pablo@example.com");
            mockContext.Setup(p => p.HttpContext.User.Identity.Name).Returns(mockPrincipal.Object.Identity.Name);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            var userTest = new ApplicationUser()
            {
                Id = "pablo@example.com",
                Email = "pablo@example.com"
            };
            CreateApplicationSettingsViewModel aS = new CreateApplicationSettingsViewModel
            {
                SettingName = "aName",
                ParameterName = "aParameterName",
                ParameterValue = "Some parameters values",
                CreatedOn = DateTime.Now,
                CreatedBy = userTest
            };

            mockUserHelper.Setup(p => p.GetUserByEmail("pablo@example.com")).Returns(userTest);
            SettingsController controller = new SettingsController(mockSettingsHelper.Object, mockUserHelper.Object)
            {
                ControllerContext = mockContext.Object
            };

            var result = controller.Create(aS);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.IsTrue(((HttpStatusCodeResult)result).StatusCode == 200);
        }
    }
}
