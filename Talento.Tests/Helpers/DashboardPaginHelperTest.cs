using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Collections.Generic;
using Talento.Entities;
using System.Data.Entity;
using Moq;
using Talento.Core.Data;
using Talento.Core.Helpers;
using Talento.Core;

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
            Mock<IComment> mockCommenthelper = new Mock<IComment>();
            context.Setup(c => c.Positions).Returns(positionSet.Object);

            DashboardPagingHelper dashboardPagingHelper = new DashboardPagingHelper(context.Object, mockCommenthelper.Object);

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
            Mock<IComment> mockCommentHelper = new Mock<IComment>();
            context.Setup(c => c.Positions).Returns(positionSet.Object);

            DashboardPagingHelper dashboardPagingHelper = new DashboardPagingHelper(context.Object, mockCommentHelper.Object);

            var result = dashboardPagingHelper.GetBasicTable("", "", "", "", 1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Position>));
            Assert.AreEqual(expectedData.Count, result.Count);
        }

        //This test was commented out because we were unable to integrate office library into continuous integration
        #region Test for xml creator helper
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\DashboardPaginHelperTestData.xml",
                "CreateXlDashboardPaginHelperTestData",
                DataAccessMethod.Sequential)]
        public void CreateXlDashboardPaginHelperTestData()
        {
            DataRow[] positionsData = TestContext.DataRow.GetChildRows("CreateXlDashboardPaginHelperTestData_Position");
            DataRow[] commentsData = TestContext.DataRow.GetChildRows("CreateXlDashboardPaginHelperTestData_Comment");

            List<Position> expectedPositionsData = new List<Position>();
            foreach (DataRow r in positionsData)
            {
                expectedPositionsData.Add(new Position
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
                    LastOpenedDate = DateTime.Today,
                    LastClosedDate = DateTime.Today,
                    LastCancelledDate = DateTime.Today
                });
            }

            List<Comment> expectedCommentsData = new List<Comment>();
            foreach (DataRow r in commentsData)
            {
                expectedCommentsData.Add(new Comment
                {
                    CommentId = Convert.ToInt32(r["CommentId"]),
                    Content = r["Content"].ToString(),
                    CandidateId = Convert.ToInt32(r["CandidateId"]),
                    Candidate = new Candidate(),
                    PositionId = Convert.ToInt32(r["PositionId"]),
                    Position = new Position(),
                    Date = Convert.ToDateTime(r["Date"].ToString()),
                    UserId = r["UserId"].ToString(),
                    User = new ApplicationUser()
                });
            }
            var positionSet = new Mock<DbSet<Position>>().SetupData(expectedPositionsData);
            var commentSet = new Mock<DbSet<Comment>>().SetupData(expectedCommentsData);

            var context = new Mock<ApplicationDbContext>();
            Mock<IComment> mockCommentHelper = new Mock<IComment>();
            context.Setup(c => c.Positions).Returns(positionSet.Object);
            context.Setup(c => c.Comments).Returns(commentSet.Object);
            mockCommentHelper.Setup(c => c.GetAll(1)).Returns(expectedCommentsData);
            mockCommentHelper.Setup(c => c.GetAll(2)).Returns(expectedCommentsData);
            mockCommentHelper.Setup(c => c.GetAll(3)).Returns(expectedCommentsData);
            mockCommentHelper.Setup(c => c.GetAll(4)).Returns(expectedCommentsData);
            mockCommentHelper.Setup(c => c.GetAll(5)).Returns(expectedCommentsData);
            mockCommentHelper.Setup(c => c.GetAll(6)).Returns(expectedCommentsData);
            mockCommentHelper.Setup(c => c.GetAll(7)).Returns(expectedCommentsData);
            mockCommentHelper.Setup(c => c.GetAll(8)).Returns(expectedCommentsData);

            DashboardPagingHelper dashboardPagingHelper = new DashboardPagingHelper(context.Object, mockCommentHelper.Object);

            var result = dashboardPagingHelper.CreateXl("id_desc", "Status", null, "", null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        #endregion
    }
}
