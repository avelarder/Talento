using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talento.Controllers;
using System.Web.Mvc;
using Moq;
using Talento.Models;

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
    }
}
