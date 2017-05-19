using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Collections.Generic;
using Talento.Entities;
using System.Data.Entity;
using Moq;
using Talento.Core.Data;
using Talento.Core.Helpers;

namespace Talento.Tests.Helpers
{
    [TestClass]
    public class DashboardPaginHelperTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\DashboardPaginHelperTestData.xml",
                "GetAdminTableDashboardPaginHelperTestData",
                DataAccessMethod.Sequential)]
        public void GetAdminTableDashboardPaginHelperTestData()
        {
            DataRow[] positionsData = TestContext.DataRow.GetChildRows("GetAdminTableDashboardPaginHelperTestData_Position");

            List<Position> expectedData = new List<Position>();
            foreach (DataRow r in positionsData)
            {
                expectedData.Add(new Position
                {
                    PositionId = Convert.ToInt32(r["PositionId"]),
                    Title = r["Title"].ToString(),
                    Description = r["Description"].ToString(),
                    CreationDate = Convert.ToDateTime(r["CreationDate"].ToString()),
                    Area = r["Area"].ToString(),
                    EngagementManager = r["EngagementManager"].ToString(),
                    Owner = new ApplicationUser { Id = r["Owner_Id"].ToString(), Email = r["Owner_Email"].ToString() },
                    ApplicationUser_Id = r["Owner_Id"].ToString(),
                    PortfolioManager = new ApplicationUser { Id = r["PortfolioManager_Id"].ToString(), Email = r["PortfolioManager_Email"].ToString() },
                    RGS = r["RGS"].ToString(),
                    Status = (PositionStatus)Enum.Parse(typeof(PositionStatus), r["Status"].ToString()),
                    OpenStatus = (PositionOpenStatus)Enum.Parse(typeof(PositionOpenStatus), r["OpenStatus"].ToString()),
                    PositionCandidates = new List<PositionCandidates>(),
                });
            }

            var positionSet = new Mock<DbSet<Position>>().SetupData(expectedData);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Positions).Returns(positionSet.Object);

            DashboardPagingHelper dashboardPagingHelper = new DashboardPagingHelper(context.Object);

            var result = dashboardPagingHelper.GetAdminTable("", "", "", "", 1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Position>));
            Assert.AreEqual(expectedData.Count, result.Count);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\DashboardPaginHelperTestData.xml",
                "GetBasicTableDashboardPaginHelperTestData",
                DataAccessMethod.Sequential)]
        public void GetBasicTableDashboardPaginHelperTestData()
        {
            DataRow[] positionsData = TestContext.DataRow.GetChildRows("GetBasicTableDashboardPaginHelperTestData_Position");

            List<Position> expectedData = new List<Position>();
            foreach (DataRow r in positionsData)
            {
                expectedData.Add(new Position
                {
                    PositionId = Convert.ToInt32(r["PositionId"]),
                    Title = r["Title"].ToString(),
                    Description = r["Description"].ToString(),
                    CreationDate = Convert.ToDateTime(r["CreationDate"].ToString()),
                    Area = r["Area"].ToString(),
                    EngagementManager = r["EngagementManager"].ToString(),
                    Owner = new ApplicationUser { Id = r["Owner_Id"].ToString(), Email = r["Owner_Email"].ToString() },
                    ApplicationUser_Id = r["Owner_Id"].ToString(),
                    PortfolioManager = new ApplicationUser { Id = r["PortfolioManager_Id"].ToString(), Email = r["PortfolioManager_Email"].ToString() },
                    RGS = r["RGS"].ToString(),
                    Status = (PositionStatus)Enum.Parse(typeof(PositionStatus), r["Status"].ToString()),
                    OpenStatus = (PositionOpenStatus)Enum.Parse(typeof(PositionOpenStatus), r["OpenStatus"].ToString()),
                    PositionCandidates = new List<PositionCandidates>(),
                });
            }

            var positionSet = new Mock<DbSet<Position>>().SetupData(expectedData);

            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Positions).Returns(positionSet.Object);

            DashboardPagingHelper dashboardPagingHelper = new DashboardPagingHelper(context.Object);

            var result = dashboardPagingHelper.GetBasicTable("", "", "", "", 1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Position>));
            Assert.AreEqual(expectedData.Count, result.Count);
        }
    }
}
