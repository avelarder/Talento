using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Talento.Entities;
using System.Data;
using Moq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Talento.Core.Data;
using Talento.Core.Helpers;
using System.IO;

namespace Talento.Tests.Helpers
{
    [TestClass]
    public class UserHelperTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\UserHelperTestData.xml",
                "SearchPMUserHelperTest",
                DataAccessMethod.Sequential)]
        public void SearchPMUserHelperTest()
        {
            DataRow[] applicationUserData = TestContext.DataRow.GetChildRows("SearchPMUserHelperTest_ApplicationUser");
            DataRow roleData = TestContext.DataRow.GetChildRows("SearchPMUserHelperTest_Role").Single();

            ApplicationRole role = new ApplicationRole
            {
                Name = roleData["Name"].ToString(),
                Id = "1"
            };

            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (DataRow d in applicationUserData)
            {
                users.Add(new ApplicationUser
                {
                    Id = d["ApplicationUserId"].ToString(),
                    Email = d["Email"].ToString(),
                    UserName = d["Email"].ToString()
                });
            }

            foreach (ApplicationUser au in users)
            {
                IdentityUserRole idr = new IdentityUserRole
                {
                    RoleId = role.Id,
                    UserId = au.Id
                };
                au.Roles.Add(idr);
                role.Users.Add(idr);
            }

            var applicationUserSet = new Mock<DbSet<ApplicationUser>>().SetupData(users);
            var rolesSet = new Mock<DbSet<IdentityRole>>().SetupData(new List<IdentityRole> { role });

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Roles).Returns(rolesSet.Object);
            context.Setup(c => c.Users).Returns(applicationUserSet.Object);

            UserHelper userHelper = new UserHelper(context.Object);

            var result = userHelper.SearchPM(users.First().UserName);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ApplicationUser));
            Assert.AreEqual(users.First(), (ApplicationUser)result);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
               "Resources\\Tests\\UserHelperTestData.xml",
               "GetUserByEmailUserHelperTest",
               DataAccessMethod.Sequential)]
        public void GetUserByEmailUserHelperTest()
        {
            DataRow applicationUserData = TestContext.DataRow.GetChildRows("GetUserByEmailUserHelperTest_ApplicationUser").Single();

            ApplicationUser user = new ApplicationUser
            {
                Id = applicationUserData["ApplicationUserId"].ToString(),
                Email = applicationUserData["Email"].ToString(),
                UserName = applicationUserData["Email"].ToString()
            };

            var applicationUserSet = new Mock<DbSet<ApplicationUser>>().SetupData(new List<ApplicationUser> { user });

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Users).Returns(applicationUserSet.Object);

            UserHelper userHelper = new UserHelper(context.Object);

            var result = userHelper.GetUserByEmail(user.Email);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ApplicationUser));
            Assert.AreEqual(user, (ApplicationUser)result);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
               "Resources\\Tests\\UserHelperTestData.xml",
               "GetUserByIdUserHelperTest",
               DataAccessMethod.Sequential)]
        public void GetUserByIdUserHelperTest()
        {
            DataRow applicationUserData = TestContext.DataRow.GetChildRows("GetUserByIdUserHelperTest_ApplicationUser").Single();

            ApplicationUser user = new ApplicationUser
            {
                Id = applicationUserData["ApplicationUserId"].ToString(),
                Email = applicationUserData["Email"].ToString(),
                UserName = applicationUserData["Email"].ToString()
            };

            var applicationUserSet = new Mock<DbSet<ApplicationUser>>().SetupData(new List<ApplicationUser> { user });

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Users).Returns(applicationUserSet.Object);

            UserHelper userHelper = new UserHelper(context.Object);

            var result = userHelper.GetUserById(user.Id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ApplicationUser));
            Assert.AreEqual(user, (ApplicationUser)result);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
              "Resources\\Tests\\UserHelperTestData.xml",
              "GetByRolesUserHelperTest",
              DataAccessMethod.Sequential)]
        public void GetByRolesUserHelperTest()
        {
            DataRow[] applicationUserPMData = TestContext.DataRow.GetChildRows("GetByRolesUserHelperTest_ApplicationUserPM");
            DataRow[] applicationUserTAGData = TestContext.DataRow.GetChildRows("GetByRolesUserHelperTest_ApplicationUserTAG");
            DataRow[] roleData = TestContext.DataRow.GetChildRows("GetByRolesUserHelperTest_Role");

            List<ApplicationRole> applicationRoles = new List<ApplicationRole>();
            List<ApplicationUser> usersPM = new List<ApplicationUser>();
            List<ApplicationUser> usersTAG = new List<ApplicationUser>();

            foreach (DataRow d in roleData)
            {
                applicationRoles.Add(new ApplicationRole
                {
                    Name = d["Name"].ToString(),
                    Id = d["Id"].ToString(),
                });
            }

            foreach (DataRow d in applicationUserPMData)
            {
                usersPM.Add(new ApplicationUser
                {
                    Id = d["ApplicationUserId"].ToString(),
                    Email = d["Email"].ToString(),
                    UserName = d["Email"].ToString()
                });
            }

            foreach (DataRow d in applicationUserTAGData)
            {
                usersTAG.Add(new ApplicationUser
                {
                    Id = d["ApplicationUserId"].ToString(),
                    Email = d["Email"].ToString(),
                    UserName = d["Email"].ToString()
                });
            }

            foreach (ApplicationUser au in usersPM)
            {
                ApplicationRole ar = applicationRoles.Where(x => x.Name == "PM").Single();
                IdentityUserRole idr = new IdentityUserRole
                {
                    RoleId = ar.Id,
                    UserId = au.Id
                };
                au.Roles.Add(idr);
                applicationRoles.Where(x => x.Name == "PM").Single().Users.Add(idr);
            }

            foreach (ApplicationUser au in usersTAG)
            {
                ApplicationRole ar = applicationRoles.Where(x => x.Name == "TAG").Single();
                IdentityUserRole idr = new IdentityUserRole
                {
                    RoleId = ar.Id,
                    UserId = au.Id
                };
                au.Roles.Add(idr);
                applicationRoles.Where(x => x.Name == "TAG").Single().Users.Add(idr);
            }

            List<IdentityRole> identityRoles = new List<IdentityRole>();
            identityRoles.AddRange(applicationRoles);

            List<ApplicationUser> allUsers = new List<ApplicationUser>();
            allUsers.AddRange(usersPM);
            allUsers.AddRange(usersTAG);

            var applicationUserSet = new Mock<DbSet<ApplicationUser>>().SetupData(allUsers);
            var rolesSet = new Mock<DbSet<IdentityRole>>().SetupData(identityRoles);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Roles).Returns(rolesSet.Object);
            context.Setup(c => c.Users).Returns(applicationUserSet.Object);

            UserHelper userHelper = new UserHelper(context.Object);

            var result = userHelper.GetByRoles(new List<string> { "PM", "TAG" });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<ApplicationUser>));
            Assert.AreEqual(allUsers.Count, result.Count);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
              "Resources\\Tests\\UserHelperTestData.xml",
              "ChangeImageProfileUserHelperTest",
              DataAccessMethod.Sequential)]
        public void ChangeImageProfileUserHelperTest()
        {
            DataRow applicationUserData = TestContext.DataRow.GetChildRows("ChangeImageProfileUserHelperTest_ApplicationUser").Single();

            byte[] originalImage = File.ReadAllBytes(Path.GetFullPath(@applicationUserData["OriginalImagePath"].ToString()));
            byte[] editedImage = File.ReadAllBytes(Path.GetFullPath(@applicationUserData["EditedImagePath"].ToString()));

            ApplicationUser user = new ApplicationUser
            {
                Id = applicationUserData["ApplicationUserId"].ToString(),
                Email = applicationUserData["Email"].ToString(),
                UserName = applicationUserData["Email"].ToString(),
                ImageProfile = originalImage
            };

            var applicationUserSet = new Mock<DbSet<ApplicationUser>>().SetupData(new List<ApplicationUser> { user });

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Users).Returns(applicationUserSet.Object);

            //change image
            user.ImageProfile = editedImage;

            UserHelper userHelper = new UserHelper(context.Object);

            var result = userHelper.ChangeImageProfile(user);

            Assert.IsNotNull(result);
            context.Verify(m => m.SaveChanges(), Times.AtLeastOnce());
            Assert.AreEqual(user, applicationUserSet.Object.Where(x=>x.Id == user.Id).Single());
        }
    }
}

