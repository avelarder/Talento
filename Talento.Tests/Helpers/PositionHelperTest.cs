using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using Talento.Entities;
using Moq;
using System.Data.Entity;
using Talento.Core.Data;
using Talento.Core.Helpers;
using Talento.Core;

namespace Talento.Tests.Helpers
{
    [TestClass]
    public class PositionHelperTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\PositionHelperTestData.xml",
                "CreatePositionHelperTest",
                DataAccessMethod.Sequential)]
        public void CreatePositionHelperTest()
        {
            DataRow[] applicatinUserData = TestContext.DataRow.GetChildRows("CreatePositionHelperTest_ApplicationUser");
            DataRow[] positionData = TestContext.DataRow.GetChildRows("CreatePositionHelperTest_Position");

            Position position = new Position
            {
                PositionId = Convert.ToInt32(positionData[0]["PositionId"]),
                Title = positionData[0]["Title"].ToString(),
                Description = positionData[0]["Description"].ToString(),
                CreationDate = Convert.ToDateTime(positionData[0]["CreationDate"].ToString()),
                Area = positionData[0]["Area"].ToString(),
                EngagementManager = positionData[0]["EngagementManager"].ToString(),
                Owner = new ApplicationUser { Id = positionData[0]["Owner_Id"].ToString(), Email = positionData[0]["Owner_Email"].ToString() },
                ApplicationUser_Id = positionData[0]["Owner_Id"].ToString(),
                PortfolioManager = new ApplicationUser { Id = positionData[0]["PortfolioManager_Id"].ToString(), Email = positionData[0]["PortfolioManager_Email"].ToString() },
                RGS = positionData[0]["RGS"].ToString(),
                Status = (PositionStatus)Enum.Parse(typeof(PositionStatus), positionData[0]["Status"].ToString()),
                OpenStatus = (PositionOpenStatus)Enum.Parse(typeof(PositionOpenStatus), positionData[0]["OpenStatus"].ToString()),
                PositionCandidates = new List<PositionCandidates>(),
            };

            var logSet = new Mock<DbSet<Log>>();
            var positionSet = new Mock<DbSet<Position>>().SetupData(new List<Position>());
            var DbContext = new Mock<ApplicationDbContext>();
            var mockHelper = new Mock<IPositionLog>();
            DbContext.Setup(p => p.Positions).Returns(positionSet.Object);
            DbContext.Setup(c => c.PositionLogs).Returns(logSet.Object);
            DbContext.Setup(x => x.SaveChanges()).Verifiable();

            PositionHelper poshelper = new PositionHelper(DbContext.Object, mockHelper.Object);

            poshelper.Create(position);

            Assert.AreEqual(position, positionSet.Object.Where(p => p.PositionId == position.PositionId).Single());
            positionSet.Verify(m => m.Add(It.IsAny<Position>()), Times.Once());
            DbContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\PositionHelperTestData.xml",
                "DeletePositionHelperTest",
                DataAccessMethod.Sequential)]
        public void DeletePositionHelperTest()
        {
            DataRow[] applicatinUserData = TestContext.DataRow.GetChildRows("DeletePositionHelperTest_ApplicationUser");
            DataRow[] positionData = TestContext.DataRow.GetChildRows("DeletePositionHelperTest_Position");

            ApplicationUser user = new ApplicationUser
            {
                Id = positionData[0]["PortfolioManager_Id"].ToString(),
                Email = positionData[0]["PortfolioManager_Email"].ToString()
            };

            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(user);
            List<Position> list = new List<Position>
            {
                new Position
                {
                    PositionId = Convert.ToInt32(positionData[0]["PositionId"]),
                    Title = positionData[0]["Title"].ToString(),
                    Description = positionData[0]["Description"].ToString(),
                    CreationDate = Convert.ToDateTime(positionData[0]["CreationDate"].ToString()),
                    Area = positionData[0]["Area"].ToString(),
                    EngagementManager = positionData[0]["EngagementManager"].ToString(),
                    Owner = new ApplicationUser { Id = positionData[0]["Owner_Id"].ToString(), Email = positionData[0]["Owner_Email"].ToString() },
                    ApplicationUser_Id = positionData[0]["Owner_Id"].ToString(),
                    PortfolioManager = user,
                    RGS = positionData[0]["RGS"].ToString(),
                    Status = (PositionStatus)Enum.Parse(typeof(PositionStatus), positionData[0]["Status"].ToString()),
                    OpenStatus = (PositionOpenStatus)Enum.Parse(typeof(PositionOpenStatus), positionData[0]["OpenStatus"].ToString()),
                    PositionCandidates = new List<PositionCandidates>(),
                }
            };
        
            var logSet = new Mock<DbSet<Log>>();
            var positionSet = new Mock<DbSet<Position>>().SetupData(list);
            var usersSet = new Mock<DbSet<ApplicationUser>>().SetupData(users);
            var DbContext = new Mock<ApplicationDbContext>();
            var mockHelper = new Mock<IPositionLog>();
            DbContext.Setup(p => p.Positions).Returns(positionSet.Object);
            DbContext.Setup(c => c.PositionLogs).Returns(logSet.Object);
            DbContext.Setup(x => x.SaveChanges()).Verifiable();
            DbContext.Setup(x => x.Users).Returns(usersSet.Object);

            PositionHelper poshelper = new PositionHelper(DbContext.Object, mockHelper.Object);

            poshelper.Delete(1, user.Id);

            Assert.IsTrue(list.Where(x => x.PositionId == 1).Single().Status == PositionStatus.Removed);
            DbContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\PositionHelperTestData.xml",
                "EditPositionHelperTest",
                DataAccessMethod.Sequential)]
        public void EditPositionHelperTest()
        {
            DataRow[] applicatinUserData = TestContext.DataRow.GetChildRows("EditPositionHelperTest_ApplicationUser");
            DataRow[] positionData = TestContext.DataRow.GetChildRows("EditPositionHelperTest_Position");

            ApplicationUser user = new ApplicationUser
            {
                Id = positionData[0]["PortfolioManager_Id"].ToString(),
                Email = positionData[0]["PortfolioManager_Email"].ToString()
            };

            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(user);
            List<Position> list = new List<Position>
            {
                new Position
                {
                    PositionId = Convert.ToInt32(positionData[0]["PositionId"]),
                    Title = positionData[0]["Title"].ToString(),
                    Description = positionData[0]["Description"].ToString(),
                    CreationDate = Convert.ToDateTime(positionData[0]["CreationDate"].ToString()),
                    Area = positionData[0]["Area"].ToString(),
                    EngagementManager = positionData[0]["EngagementManager"].ToString(),
                    Owner = new ApplicationUser { Id = positionData[0]["Owner_Id"].ToString(), Email = positionData[0]["Owner_Email"].ToString() },
                    ApplicationUser_Id = positionData[0]["Owner_Id"].ToString(),
                    PortfolioManager = user,
                    RGS = positionData[0]["RGS"].ToString(),
                    Status = (PositionStatus)Enum.Parse(typeof(PositionStatus), positionData[0]["Status"].ToString()),
                    OpenStatus = (PositionOpenStatus)Enum.Parse(typeof(PositionOpenStatus), positionData[0]["OpenStatus"].ToString()),
                    PositionCandidates = new List<PositionCandidates>(),
                }
            };

            var logSet = new Mock<DbSet<Log>>();
            var positionSet = new Mock<DbSet<Position>>().SetupData(list);
            var usersSet = new Mock<DbSet<ApplicationUser>>().SetupData(users);
            var DbContext = new Mock<ApplicationDbContext>();
            var mockHelper = new Mock<IPositionLog>();
            DbContext.Setup(p => p.Positions).Returns(positionSet.Object);
            DbContext.Setup(c => c.PositionLogs).Returns(logSet.Object);
            DbContext.Setup(x => x.SaveChanges()).Verifiable();
            DbContext.Setup(x => x.Users).Returns(usersSet.Object);
        }
    }
}
