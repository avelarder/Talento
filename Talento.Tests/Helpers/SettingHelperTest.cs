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
using Talento.Tests.Utilities;

namespace Talento.Tests.Helpers
{
    [TestClass]
    public class SettingHelperTest
    {
        private MockRepository mocks;
        private Mock<IPrincipal> mockPrincipal;
        private Mock<IApplicationSetting> mockSettingsHelper;
        private Mock<ICustomUser> mockUserHelper;
        private Mock mockContext;
        private Mock<ApplicationUser> mockUser;
        private string xmlSource;

        public SettingHelperTest()
        {
            xmlSource = "ApplicationSettings.xml";
            mocks = new MockRepository(MockBehavior.Default);
            mockPrincipal = mocks.Create<IPrincipal>();
            mockSettingsHelper = mocks.Create<IApplicationSetting>();
            mockUserHelper = mocks.Create<ICustomUser>();
            mockContext = new Mock<ControllerContext>();
            mockUser = mocks.Create<ApplicationUser>();
        }

        [TestMethod]
        public void AddSettingTest()
        {
            // get deserealized object
            ApplicationSettingXml dataXml = XmlDeserializer.DeserealizeXml(xmlSource);

            ApplicationSetting data = new ApplicationSetting
            {
                ApplicationSettingId = dataXml.ApplicationSettingId,
                ApplicationUser_Id = dataXml.ApplicationUser_Id,
                CreatedBy = new ApplicationUser { Email = dataXml.CreatedByEmail, UserName = dataXml.CreatedByUserName },
                CreationDate = dataXml.CreationDate,
                SettingName = dataXml.SettingName,
                ParameterName = dataXml.ParameterName,
                ParameterValue = dataXml.ParameterValue
            };

            var set = new Mock<DbSet<ApplicationSetting>>();

            // mock dbContext
            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.ApplicationSetting).Returns(set.Object);
            context.Setup(x => x.SaveChanges()).Returns(1);

            SettingsHelper settingHelper = new SettingsHelper(context.Object);

            // execute helper query
            int result = settingHelper.Create(data);

            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void AddSettingNotWorkingTest()
        {
            ApplicationSettingXml dataXml = XmlDeserializer.DeserealizeXml(this.xmlSource);

            ApplicationSetting data = new ApplicationSetting
            {
                ApplicationSettingId = dataXml.ApplicationSettingId,
                ApplicationUser_Id = dataXml.ApplicationUser_Id,
                CreatedBy = new ApplicationUser { Email = dataXml.CreatedByEmail, UserName = dataXml.CreatedByUserName },
                CreationDate = dataXml.CreationDate,
                SettingName = dataXml.SettingName,
                ParameterName = dataXml.ParameterName,
                ParameterValue = dataXml.ParameterValue
            };

            var set = new Mock<DbSet<ApplicationSetting>>();

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.ApplicationSetting).Returns(set.Object);
            context.Setup(x => x.SaveChanges()).Returns(0);

            SettingsHelper settingHelper = new SettingsHelper(context.Object);

            int result = settingHelper.Create(data);
            Assert.IsTrue(result == 0);
        }
    }
}
