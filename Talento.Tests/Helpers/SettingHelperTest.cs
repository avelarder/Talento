using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Talento.Controllers;
using Talento.Core;
using Talento.Entities;
using Talento.Models;
using Talento.Core.Utilities;
using System.Web.Routing;
using System.Web;
using Talento.Core.Helpers;
using Talento.Core.Data;
using System.Data.Entity;

namespace Talento.Tests.Helpers
{
    [TestClass]
    public class SettingHelperTest
    {
        private Mock<ApplicationDbContext> dbContext;
        SettingsHelper settingsHelper;

        private MockRepository mocks;
        private Mock<IPrincipal> mockPrincipal;
        private Mock<IApplicationSetting> mockSettingsHelper;
        private Mock<ICustomUser> mockUserHelper;
        private Mock mockContext;
        private Mock<ApplicationUser> mockUser;

        public SettingHelperTest()
        {
            mocks = new MockRepository(MockBehavior.Default);
            mockPrincipal = mocks.Create<IPrincipal>();
            mockSettingsHelper = mocks.Create<IApplicationSetting>();
            mockUserHelper = mocks.Create<ICustomUser>();
            mockContext = new Mock<ControllerContext>();
            mockUser = mocks.Create<ApplicationUser>();
            //dbContext = new Mock<ApplicationDbContext>();
            //settingsHelper = new SettingsHelper(dbContext.Object);
        }

        [TestMethod]
        public void CreateSettingTest()
        {
            ApplicationSetting data = new ApplicationSetting
            {
                ApplicationSettingId = 1,
                ApplicationUser_Id = "1",
                CreatedBy = new ApplicationUser { Email = "admin@example.com", UserName = "admin@example.com" },
                CreationDate = DateTime.Now,
                SettingName = "Filter",
                ParameterName = "FilterBy",
                ParameterValue = "Name"
            };

            var set = new Mock<DbSet<ApplicationSetting>>();

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.ApplicationSetting).Returns(set.Object);

            SettingsHelper setting = new SettingsHelper(context.Object);

            int result = setting.Create(data);
            Assert.IsTrue(result == 1);
        }
    }
}
