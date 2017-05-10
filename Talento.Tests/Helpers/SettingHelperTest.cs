using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data;

namespace Talento.Tests.Helpers
{
    [TestClass]
    public class SettingHelperTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\SettingHelperTestData.xml",
                "AddApplicationSettingTest",
                DataAccessMethod.Sequential)]
        public void AddSettingHelperTest()
        {
            //Test Data
            DataRow[] applicationSettingsData = TestContext.DataRow.GetChildRows("AddApplicationSettingTest_ApplicationSetting");

            List<ApplicationSetting> listAppSettings = new List<ApplicationSetting>();
            foreach (DataRow d in applicationSettingsData)
            {
                listAppSettings.Add(new ApplicationSetting
                {
                    ApplicationSettingId = Convert.ToInt32(d["ApplicationSettingId"]),
                    CreatedBy = new ApplicationUser { Email = d["CreatedByEmail"].ToString(), Id = d["ApplicationUser_Id"].ToString(), UserName = d["CreatedByUserName"].ToString() },
                    CreationDate = Convert.ToDateTime(d["CreationDate"]),
                    ApplicationUser_Id = d["ApplicationUser_Id"].ToString(),
                    ParameterName = d["ParameterName"].ToString(),
                    ParameterValue = d["ParameterValue"].ToString(),
                    SettingName = d["SettingName"].ToString()
                });
            }

            var set = new Mock<DbSet<ApplicationSetting>>().SetupData(new List<ApplicationSetting>());

            // mock dbContext
            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.ApplicationSetting).Returns(set.Object);
            context.Setup(x => x.SaveChanges()).Returns(1);

            SettingsHelper settingHelper = new SettingsHelper(context.Object);

            // execute helper query
            int result = settingHelper.Create(listAppSettings.First());

            Assert.IsTrue(result == 1);
            Assert.AreEqual(listAppSettings.First(), set.Object.Where(c => c.ApplicationSettingId == listAppSettings.First().ApplicationSettingId).Single());
            set.Verify(m => m.Add(It.IsAny<ApplicationSetting>()), Times.Once());
            context.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\SettingHelperTestData.xml",
                "GetAllSettingHelperTest",
                DataAccessMethod.Sequential)]
        public void GetAllSettingHelperTest()
        {
            //Test Data
            DataRow[] applicationSettingsData = TestContext.DataRow.GetChildRows("GetAllSettingHelperTest_ApplicationSetting");

            List<ApplicationSetting> listAppSettings = new List<ApplicationSetting>();
            foreach (DataRow d in applicationSettingsData)
            {
                listAppSettings.Add(new ApplicationSetting
                {
                    ApplicationSettingId = Convert.ToInt32(d["ApplicationSettingId"]),
                    CreatedBy = new ApplicationUser { Email = d["CreatedByEmail"].ToString(), Id = d["ApplicationUser_Id"].ToString(), UserName = d["CreatedByUserName"].ToString() },
                    CreationDate = Convert.ToDateTime(d["CreationDate"]),
                    ApplicationUser_Id = d["ApplicationUser_Id"].ToString(),
                    ParameterName = d["ParameterName"].ToString(),
                    ParameterValue = d["ParameterValue"].ToString(),
                    SettingName = d["SettingName"].ToString()
                });
            }

            var set = new Mock<DbSet<ApplicationSetting>>().SetupData(listAppSettings);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.ApplicationSetting).Returns(set.Object);
            context.Setup(x => x.SaveChanges()).Returns(1);

            SettingsHelper settingHelper = new SettingsHelper(context.Object);
            var result = settingHelper.GetAll();

            Assert.IsInstanceOfType(result, typeof(List<ApplicationSetting>));
            Assert.IsTrue(result.ToList<ApplicationSetting>().Count == 3);
            Assert.IsTrue(result.ToList<ApplicationSetting>().First().ApplicationSettingId == 1);
            Assert.IsTrue(result.ToList<ApplicationSetting>().Last().ApplicationSettingId == 3);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
        "Resources\\Tests\\SettingHelperTestData.xml",
        "GetByNameSettingHelperTest",
        DataAccessMethod.Sequential)]
        public void GetByNameSettingHelperTest()
        {
            //Test Data
            DataRow[] applicationSettingsData = TestContext.DataRow.GetChildRows("GetByNameSettingHelperTest_ApplicationSetting");

            List<ApplicationSetting> listAppSettings = new List<ApplicationSetting>();
            foreach (DataRow d in applicationSettingsData)
            {
                listAppSettings.Add(new ApplicationSetting
                {
                    ApplicationSettingId = Convert.ToInt32(d["ApplicationSettingId"]),
                    CreatedBy = new ApplicationUser { Email = d["CreatedByEmail"].ToString(), Id = d["ApplicationUser_Id"].ToString(), UserName = d["CreatedByUserName"].ToString() },
                    CreationDate = Convert.ToDateTime(d["CreationDate"]),
                    ApplicationUser_Id = d["ApplicationUser_Id"].ToString(),
                    ParameterName = d["ParameterName"].ToString(),
                    ParameterValue = d["ParameterValue"].ToString(),
                    SettingName = d["SettingName"].ToString()
                });
            }

            var set = new Mock<DbSet<ApplicationSetting>>().SetupData(listAppSettings);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.ApplicationSetting).Returns(set.Object);
            context.Setup(x => x.SaveChanges()).Returns(1);

            SettingsHelper settingHelper = new SettingsHelper(context.Object);
            var result = settingHelper.GetByName(listAppSettings.First().SettingName);

            Assert.IsInstanceOfType(result, typeof(ApplicationSetting));
            Assert.IsTrue(((ApplicationSetting)result).Equals(listAppSettings.First()));
            Assert.AreEqual(listAppSettings.First(), set.Object.Where(c => c.ApplicationSettingId == listAppSettings.First().ApplicationSettingId).Single());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
        "Resources\\Tests\\SettingHelperTestData.xml",
        "GetByIdSettingHelperTest",
        DataAccessMethod.Sequential)]
        public void GetByIdSettingHelperTest()
        {
            //Test Data
            DataRow[] applicationSettingsData = TestContext.DataRow.GetChildRows("GetByIdSettingHelperTest_ApplicationSetting");

            List<ApplicationSetting> listAppSettings = new List<ApplicationSetting>();
            foreach (DataRow d in applicationSettingsData)
            {
                listAppSettings.Add(new ApplicationSetting
                {
                    ApplicationSettingId = Convert.ToInt32(d["ApplicationSettingId"]),
                    CreatedBy = new ApplicationUser { Email = d["CreatedByEmail"].ToString(), Id = d["ApplicationUser_Id"].ToString(), UserName = d["CreatedByUserName"].ToString() },
                    CreationDate = Convert.ToDateTime(d["CreationDate"]),
                    ApplicationUser_Id = d["ApplicationUser_Id"].ToString(),
                    ParameterName = d["ParameterName"].ToString(),
                    ParameterValue = d["ParameterValue"].ToString(),
                    SettingName = d["SettingName"].ToString()
                });
            }

            var set = new Mock<DbSet<ApplicationSetting>>().SetupData(listAppSettings);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.ApplicationSetting).Returns(set.Object);
            context.Setup(x => x.SaveChanges()).Returns(1);

            SettingsHelper settingHelper = new SettingsHelper(context.Object);
            var result = settingHelper.GetById(listAppSettings.First().ApplicationSettingId);

            Assert.IsInstanceOfType(result, typeof(ApplicationSetting));
            Assert.IsTrue(((ApplicationSetting)result).Equals(listAppSettings.First()));
            Assert.AreEqual(listAppSettings.First(), set.Object.Where(c => c.ApplicationSettingId == listAppSettings.First().ApplicationSettingId).Single());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
       "Resources\\Tests\\SettingHelperTestData.xml",
       "EditSettingHelperTest",
       DataAccessMethod.Sequential)]
        public void EditSettingHelperTest()
        {
            //Test Data
            DataRow[] applicationSettingsData = TestContext.DataRow.GetChildRows("EditSettingHelperTest_ApplicationSetting");

            List<ApplicationSetting> listAppSettings = new List<ApplicationSetting>();
            foreach (DataRow d in applicationSettingsData)
            {
                listAppSettings.Add(new ApplicationSetting
                {
                    ApplicationSettingId = Convert.ToInt32(d["ApplicationSettingId"]),
                    CreatedBy = new ApplicationUser { Email = d["CreatedByEmail"].ToString(), Id = d["ApplicationUser_Id"].ToString(), UserName = d["CreatedByUserName"].ToString() },
                    CreationDate = Convert.ToDateTime(d["CreationDate"]),
                    ApplicationUser_Id = d["ApplicationUser_Id"].ToString(),
                    ParameterName = d["ParameterName"].ToString(),
                    ParameterValue = d["ParameterValue"].ToString(),
                    SettingName = d["SettingName"].ToString()
                });
            }

            var set = new Mock<DbSet<ApplicationSetting>>().SetupData(listAppSettings);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.ApplicationSetting).Returns(set.Object);
            context.Setup(x => x.SaveChanges()).Returns(1);

            SettingsHelper settingHelper = new SettingsHelper(context.Object);
            var result = settingHelper.Edit(listAppSettings.First());

            Assert.IsTrue(result.Equals(1));
            Assert.AreEqual(listAppSettings.First(), set.Object.Where(c => c.ApplicationSettingId == listAppSettings.First().ApplicationSettingId).Single());
            set.Verify(m => m.Add(It.IsAny<ApplicationSetting>()), Times.Once());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
       "Resources\\Tests\\SettingHelperTestData.xml",
       "GetPaginationSettingHelperTest",
       DataAccessMethod.Sequential)]
        public void GetPaginationSettingHelperTest()
        {
            //Test Data
            DataRow[] applicationSettingsData = TestContext.DataRow.GetChildRows("GetPaginationSettingHelperTest_ApplicationSetting");

            List<ApplicationSetting> listAppSettings = new List<ApplicationSetting>();
            foreach (DataRow d in applicationSettingsData)
            {
                listAppSettings.Add(new ApplicationSetting
                {
                    ApplicationSettingId = Convert.ToInt32(d["ApplicationSettingId"]),
                    CreatedBy = new ApplicationUser { Email = d["CreatedByEmail"].ToString(), Id = d["ApplicationUser_Id"].ToString(), UserName = d["CreatedByUserName"].ToString() },
                    CreationDate = Convert.ToDateTime(d["CreationDate"]),
                    ApplicationUser_Id = d["ApplicationUser_Id"].ToString(),
                    ParameterName = d["ParameterName"].ToString(),
                    ParameterValue = d["ParameterValue"].ToString(),
                    SettingName = d["SettingName"].ToString()
                });
            }

            var set = new Mock<DbSet<ApplicationSetting>>().SetupData(listAppSettings);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.ApplicationSetting).Returns(set.Object);

            SettingsHelper settingHelper = new SettingsHelper(context.Object);
            var result = settingHelper.GetPagination("CreationDate", "");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 3);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
       "Resources\\Tests\\SettingHelperTestData.xml",
       "GetParametersSettingHelperTest",
       DataAccessMethod.Sequential)]
        public void GetParametersSettingHelperTest()
        {
            //Test Data
            DataRow[] applicationSettingsData = TestContext.DataRow.GetChildRows("GetParametersSettingHelperTest_ApplicationSetting");

            List<ApplicationSetting> listAppSettings = new List<ApplicationSetting>();
            foreach (DataRow d in applicationSettingsData)
            {
                listAppSettings.Add(new ApplicationSetting
                {
                    ApplicationSettingId = Convert.ToInt32(d["ApplicationSettingId"]),
                    CreatedBy = new ApplicationUser { Email = d["CreatedByEmail"].ToString(), Id = d["ApplicationUser_Id"].ToString(), UserName = d["CreatedByUserName"].ToString() },
                    CreationDate = Convert.ToDateTime(d["CreationDate"]),
                    ApplicationUser_Id = d["ApplicationUser_Id"].ToString(),
                    ParameterName = d["ParameterName"].ToString(),
                    ParameterValue = d["ParameterValue"].ToString(),
                    SettingName = d["SettingName"].ToString()
                });
            }

            var set = new Mock<DbSet<ApplicationSetting>>().SetupData(listAppSettings);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.ApplicationSetting).Returns(set.Object);

            SettingsHelper settingHelper = new SettingsHelper(context.Object);
            var result = settingHelper.GetParameters("Filtering");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual("Filtering", result[0].ToString());
        }
    }
}
