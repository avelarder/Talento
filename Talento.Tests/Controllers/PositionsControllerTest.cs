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
using Talento.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Talento.Core.Data;
using System.Net;

namespace Talento.Tests.Controllers
{
    [TestClass]
    public class PositionsControllerTest
    {
        [TestMethod]
        public void EditAPositionWithoutValidId()
        {
            // Create a mock set and context
            ApplicationUser userPM = new ApplicationUser() { Email = "somecoolpm@test.com", EmailConfirmed = true };
            ApplicationRole pmrole = new ApplicationRole() { Id = "PM", Description = "You have to manage projects", Name = "Project Manager" };
            ApplicationUser userHR = new ApplicationUser() { Email = "somecoolhr@test.com", EmailConfirmed = true };
            ApplicationRole hrrole = new ApplicationRole() { Id = "HR", Description = "You have to manage people", Name = "Human Resources" };
            ApplicationUser userEditing = new ApplicationUser() { Email = "someone@test.com", EmailConfirmed = true };
            hrrole.Users.Add(new IdentityUserRole() { RoleId = hrrole.Id, UserId = userHR.Id });
            pmrole.Users.Add(new IdentityUserRole() { RoleId = pmrole.Id, UserId = userPM.Id });
            var positions = new List<Position>{
                new Position(){Area = "Dev",CreationDate = DateTime.Now,Description = "We need a dev, please!",RGS = "666",Title = "Dev Mega Senior",Status = Status.Open,EngagementManager = "Some Genious EM",PortfolioManager = userPM,PortfolioManager_Id = userPM.Id},
            };
            var roles = new List<ApplicationRole> { hrrole, pmrole };
            var users = new List<ApplicationUser>{userPM,userHR,userEditing};
            var setUsers = new Mock<DbSet<ApplicationUser>>().SetupData<ApplicationUser>(users);
            var setPositions= new Mock<DbSet<Position>>().SetupData<Position>(positions);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Users).Returns(setUsers.Object);
            context.Setup(c => c.Positions).Returns(setPositions.Object);

            // Create a BlogsController and invoke the Index action
            Mock<IPosition> mPositions = new Mock<IPosition>();
            var controller = new PositionsController(mPositions.Object);
            var result = controller.Edit(0);

            // Check the results
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void EditANonExistingPosition()
        {
            // Create a mock set and context
            ApplicationUser userPM = new ApplicationUser() { Email = "somecoolpm@test.com", EmailConfirmed = true };
            ApplicationRole pmrole = new ApplicationRole() { Id = "PM", Description = "You have to manage projects", Name = "Project Manager" };
            ApplicationUser userHR = new ApplicationUser() { Email = "somecoolhr@test.com", EmailConfirmed = true };
            ApplicationRole hrrole = new ApplicationRole() { Id = "HR", Description = "You have to manage people", Name = "Human Resources" };
            ApplicationUser userEditing = new ApplicationUser() { Email = "someone@test.com", EmailConfirmed = true };
            hrrole.Users.Add(new IdentityUserRole() { RoleId = hrrole.Id, UserId = userHR.Id });
            pmrole.Users.Add(new IdentityUserRole() { RoleId = pmrole.Id, UserId = userPM.Id });
            var positions = new List<Position>{
                
            };
            var roles = new List<ApplicationRole> {hrrole,pmrole};
            var users = new List<ApplicationUser>{userPM,userHR,userEditing};
            var setUsers = new Mock<DbSet<ApplicationUser>>().SetupData<ApplicationUser>(users);
            var setRoles = new Mock<DbSet<ApplicationRole>>().SetupData<ApplicationRole>(roles);
            var setPositions = new Mock<DbSet<Position>>().SetupData<Position>(positions);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Users).Returns(setUsers.Object);
            context.Setup(c => c.Positions).Returns(setPositions.Object);

            // Create a BlogsController and invoke the Index action
            Mock<IPosition> mPositions = new Mock<IPosition>();
            var controller = new PositionsController(mPositions.Object);
            var result = controller.Edit(1);

            // Check the results
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }


        [TestMethod]
        public void EditAnExistingPosition()
        {
            // Create a mock set and context
            ApplicationUser userPM = new ApplicationUser() { Email = "somecoolpm@test.com", EmailConfirmed = true };
            ApplicationUser userHR = new ApplicationUser() { Email = "somecoolhr@test.com", EmailConfirmed = true };
            ApplicationUser userEditing = new ApplicationUser() { Email = "someone@test.com", EmailConfirmed = true };

            ApplicationRole pmrole = new ApplicationRole() { Id = "PM", Description = "You have to manage projects", Name = "Project Manager" };
            ApplicationRole hrrole = new ApplicationRole() { Id = "HR", Description = "You have to manage people", Name = "Human Resources" };

            hrrole.Users.Add(new IdentityUserRole() { RoleId = hrrole.Id, UserId = userHR.Id });
            pmrole.Users.Add(new IdentityUserRole() { RoleId = pmrole.Id, UserId = userPM.Id });

            Position somePosition = new Position()
            {
                Area = "Dev",
                CreationDate = DateTime.Now,
                Description = "We need a dev, please!",
                RGS = "666",
                Title = "Dev Mega Senior",
                Status = Status.Open,
                EngagementManager = "Some Genious EM",
                PortfolioManager = userPM,
                PortfolioManager_Id = userPM.Id,
                Owner = userHR,
                ApplicationUser_Id = userHR.Id,
                Id=1,
                Tags=null
            };

            var positions = new List<Position>{somePosition};
            var users = new List<ApplicationUser>{userPM,userHR,userEditing};
            var roles = new List<ApplicationRole> { hrrole, pmrole };
            var setRoles = new Mock<DbSet<ApplicationRole>>().SetupData<ApplicationRole>(roles);
            var setUsers = new Mock<DbSet<ApplicationUser>>().SetupData<ApplicationUser>(users);
            var setPositions = new Mock<DbSet<Position>>().SetupData<Position>(positions);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Users).Returns(setUsers.Object);
            context.Setup(c => c.Positions).Returns(setPositions.Object);

            // Create a BlogsController and invoke the Index action
            Mock<IPosition> mPositions = new Mock<IPosition>();
            mPositions.Setup(m => m.Get(1)).Returns(somePosition);
            var controller = new PositionsController(mPositions.Object);
            var result = controller.Edit(1);

            // Check the results
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
