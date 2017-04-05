using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talento.Controllers;
using System.Web.Mvc;
using Moq;
using Talento.Models;

namespace Talento.Tests.Controllers
{

    [TestClass]
    public class PositionControllerTest
    {
        Mock<Talento.Core.Data.ApplicationDbContext> mockedDb = new Mock<Core.Data.ApplicationDbContext>();
        Mock<Talento.Core.Helpers.PositionHelper> mockePh = new Mock<Core.Helpers.PositionHelper>();

        [TestMethod]
        public async void CreateFail()
        {
            PositionsController controller = new PositionsController(mockePh.Object);
            CreatePositionViewModel pm = new CreatePositionViewModel();

            var result = controller.Create(pm);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatePositionViewModel));
        }
    }

}

    

