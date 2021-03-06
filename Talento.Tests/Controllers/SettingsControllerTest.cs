﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class SettingsControllerTest
    {
        [TestMethod]
        public void CreateSettingTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            Mock<IApplicationSetting> mockSettingsHelper = mocks.Create<IApplicationSetting>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            var mockContext = new Mock<ControllerContext>();
            Mock<ApplicationUser> mockUser = mocks.Create<ApplicationUser>();

            // Mock SettingsUtilities
            var mockSettingsUtils = new Mock<IUtilityApplicationSettings>();

            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("pablo@example.com");
            mockContext.Setup(p => p.HttpContext.User.Identity.Name).Returns(mockPrincipal.Object.Identity.Name);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            var userTest = new ApplicationUser()
            {
                Id = "pablo@example.com",
                Email = "pablo@example.com"
            };
            ApplicationSettingCreateModel aS = new ApplicationSettingCreateModel
            {
                SettingName = "aName",
                ParameterName = "aParameterName",
                ParameterValue = "Some parameters values"
            };

            ApplicationSetting appSetting = new ApplicationSetting();
            mockSettingsHelper.Setup(x => x.Create(appSetting));
            mockUserHelper.Setup(p => p.GetUserByEmail("pablo@example.com")).Returns(userTest);
            SettingsController controller = new SettingsController(mockSettingsHelper.Object, mockUserHelper.Object, mockSettingsUtils.Object)
            {
                ControllerContext = mockContext.Object
            };

            var result = controller.Create(aS);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.IsTrue(((HttpStatusCodeResult)result).StatusCode == 200);
        }

        [TestMethod]
        public void ListSettingTest()
        {
            // Principal Mock
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);

            // Mock Controller of Context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            // Mock User
            Mock<ICustomUser> mUser = new Mock<ICustomUser>();
            
            // Mock SettingsUtilities
            var mockSettingsUtils = new Mock<IUtilityApplicationSettings>();
            mockSettingsUtils.Setup(x => x.GetSetting("pagination", "pagesize")).Returns("5");
            
            //Mock Request HTTP
            var request = new Mock<HttpRequestBase>();

            // Set isAjax Request
            request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                                {"X-Requested-With", "XMLHttpRequest"}
                });

            // Set isLocal
            request.SetupGet(x => x.IsLocal).Returns(true);
            
            // Mock Context
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            
            // Controller
            Mock<IApplicationSetting> appSettings = new Mock<IApplicationSetting>();
            appSettings.Setup(x => x.GetPagination("", "")).Returns(GetApplicationParameters().ToList());
            SettingsController controller = new SettingsController(appSettings.Object, mUser.Object, mockSettingsUtils.Object)
            {
                ControllerContext = mockContext.Object                
            };

            // Load Mock request to Controller
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            var result = controller.List(5, 1, "", "");

            // Asserts
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        [TestMethod]
        public void EditSettingTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);
            var userTest = new ApplicationUser()
            {
                Id = "pablo@example.com",
                Email = "pablo@example.com"
            };

            mockPrincipal.Setup(p => p.Identity.Name).Returns("pablo@example.com");
            // Moq User
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
           
            mockUserHelper.Setup(p => p.GetUserByEmail("pablo@example.com")).Returns(userTest);
            // Moq AppSettings
            Mock<IApplicationSetting> mockSettingsHelper = mocks.Create<IApplicationSetting>();
            ApplicationSetting appParam = new ApplicationSetting
            {
                SettingName = "aSettingName",
                ParameterName = "aName",
                ParameterValue = "someValues",
                CreationDate = DateTime.Now,
                ApplicationUser_Id = "1",
                ApplicationSettingId = 1,
                CreatedBy = new ApplicationUser { Email = "admin@example.com", UserName = "admin@example.com" }
            };
            mockSettingsHelper.Setup( x=> x.GetById(1)).Returns(appParam);
            Mock<ApplicationSetting> mockAppSetting = mocks.Create<ApplicationSetting>();
            mockPrincipal.Setup(p => p.IsInRole("Admin")).Returns(true);
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            var request = new Mock<HttpRequestBase>();

            // Mock SettingsUtilities
            var mockSettingsUtils = new Mock<IUtilityApplicationSettings>();

            ApplicationSettingModel appParamVM = new ApplicationSettingModel
            {
                SettingName = "aSettingName",
                ParameterName = "aName",
                ParameterValue = "someValues",
                CreatedOn = DateTime.Now,
                CreatedBy_Id = "1",
                ApplicationSettingId = 1,
                CreatedBy = new ApplicationUser { Email = "admin@example.com", UserName = "admin@example.com" }
            };

            SettingsController controller = new SettingsController(mockSettingsHelper.Object, mockUserHelper.Object, mockSettingsUtils.Object)
            {
                ControllerContext = mockContext.Object
            };

            var result = controller.Edit(appParamVM);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
        }

        // Dummy Data for ApplicationSettings
        private List<ApplicationSetting> GetApplicationParameters()
        {
            ApplicationUser user = new ApplicationUser { Email = "admin@example.com", UserName = "admin@example.com" };

            // ApplicationSetting Pagination 
            var listParam = new List<ApplicationSetting> {
                    new ApplicationSetting {
                        ApplicationSettingId = 1,
                        CreatedBy = user,
                        CreationDate = DateTime.Now.AddHours(1),
                        ParameterName = "PageSize",
                        ParameterValue = "10",
                        SettingName = "Pagination"
                    },
                    new ApplicationSetting {
                        ApplicationSettingId = 1,
                        CreatedBy = user,
                        CreationDate = DateTime.Now.AddHours(2),
                        ParameterName = "Status",
                        ParameterValue = "enabled",
                        SettingName = "Pagination"
                    },
                    new ApplicationSetting {
                        ApplicationSettingId = 2,
                        CreatedBy = user,
                        CreationDate = DateTime.Now.AddHours(1),
                        ParameterName = "DefaultFilterBy",
                        ParameterValue = "CreationTime",
                        SettingName = "Filtering"
                    },
                    new ApplicationSetting {
                        ApplicationSettingId = 2,
                        CreatedBy = user,
                        CreationDate = DateTime.Now.AddHours(2),
                        ParameterName = "Status",
                        ParameterValue = "enabled",
                        SettingName = "Filtering"
                    }
                };
            return listParam;
        }


    }
}
