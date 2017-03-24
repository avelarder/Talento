using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talento.Controllers;
using System.Web.Mvc;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void Login()
        {
            AccountController controller = new AccountController();

            // Act
            var result = controller.Login("Google.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Register()
        {
            AccountController controller = new AccountController();

            // Act
            var result = controller.Register();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
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
