using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talento.Controllers;
using System.Web.Mvc;
using Moq;
using System.Web.Security;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void Login()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<ControllerContext> mockContext = mocks.Create<ControllerContext>();
            mockContext.Setup(s => s.HttpContext.Request.IsAuthenticated).Returns(false);
            AccountController controller = new AccountController()
            {
                ControllerContext = mockContext.Object
            };

            // Act
            var result = controller.Login("Google.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Register()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<ControllerContext> mockContext = mocks.Create<ControllerContext>();
            mockContext.Setup(s => s.HttpContext.Request.IsAuthenticated).Returns(true);

            AccountController controller = new AccountController()
            {
                ControllerContext = mockContext.Object
            };


            // Act
            var result = controller.Register();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public async void SendmeanEmail()
        {
            AccountController controller = new AccountController();
            Models.ForgotPasswordViewModel model = new Models.ForgotPasswordViewModel();
            
            //Act
            var result = await controller.SendmeanEmail(model);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
