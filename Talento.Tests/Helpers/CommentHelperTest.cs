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
    public class CommentHelperTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\CommentHelperTestData.xml",
                "CreateCommentHelperTest",
                DataAccessMethod.Sequential)]
        public void CreateCommentHelperTest()
        {
            DataRow[] positionData = TestContext.DataRow.GetChildRows("CreateCommentHelperTest_Position");
            DataRow[] commentData = TestContext.DataRow.GetChildRows("CreateCommentHelperTest_Comment");

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

            Comment comment = new Comment
            {
                CommentId = Convert.ToInt32(commentData[0]["CommentId"].ToString()),
                Content = commentData[0]["Content"].ToString(),
                Date = Convert.ToDateTime(commentData[0]["Date"].ToString()),
                PositionId = position.PositionId,
                User = new ApplicationUser { Id = commentData[0]["UserId"].ToString(), Email = commentData[0]["UserEmail"].ToString() },
                UserId = commentData[0]["UserId"].ToString()
            };

            var positionSet = new Mock<DbSet<Position>>().SetupData(new List<Position> { position });
            var commentSet = new Mock<DbSet<Comment>>().SetupData(new List<Comment>());

            var context = new Mock<ApplicationDbContext>();
            var mockPositionHelper = new Mock<IPosition>();
            var mockCandidateHelper = new Mock<ICandidate>();
            var mockPositionLogHelper = new Mock<IPositionLog>();
            context.Setup(p => p.Positions).Returns(positionSet.Object);
            context.Setup(p => p.Comments).Returns(commentSet.Object);
            mockPositionHelper.Setup(h => h.Get(position.PositionId)).Returns(position);

            var commentHelper = new CommentHelper(context.Object, mockPositionLogHelper.Object, mockPositionHelper.Object, mockCandidateHelper.Object);
            var result = commentHelper.Create(comment);

            Assert.AreEqual(comment, commentSet.Object.Where(c => c.CommentId == comment.CommentId).Single());
            commentSet.Verify(m => m.Add(It.IsAny<Comment>()), Times.Once());
            context.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\CommentHelperTestData.xml",
                "GetByPositionIdCommentHelperTest",
                DataAccessMethod.Sequential)]
        public void GetByPositionIdCommentHelperTest()
        {
            DataRow[] idsData = TestContext.DataRow.GetChildRows("GetByPositionIdCommentHelperTest_Ids");
            DataRow[] commentData = TestContext.DataRow.GetChildRows("GetByPositionIdCommentHelperTest_Comment");

            int positionId = Convert.ToInt32(idsData[0]["PositionId"].ToString());
            int candidateId = Convert.ToInt32(idsData[0]["CandidateId"].ToString());

            List<Comment> commentList = new List<Comment>();
            foreach (DataRow d in commentData)
            {
                commentList.Add(new Comment
                {
                    PositionId = Convert.ToInt32(d["PositionId"].ToString()),
                    Content = d["Content"].ToString(),
                    User = new ApplicationUser { Id = d["UserId"].ToString(), Email = d["UserEmail"].ToString() },
                    UserId = d["UserId"].ToString(),
                    CommentId = Convert.ToInt32(d["CommentId"].ToString()),
                    CandidateId = null
                });
            }

            var commentSet = new Mock<DbSet<Comment>>().SetupData(commentList);

            var context = new Mock<ApplicationDbContext>();
            var mockPositionHelper = new Mock<IPosition>();
            var mockCandidateHelper = new Mock<ICandidate>();
            var mockPositionLogHelper = new Mock<IPositionLog>();
            context.Setup(p => p.Comments).Returns(commentSet.Object);

            var commentHelper = new CommentHelper(context.Object, mockPositionLogHelper.Object, mockPositionHelper.Object, mockCandidateHelper.Object);
            var result = commentHelper.Get(positionId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Comment>));
            Assert.AreEqual(result.Count, commentList.Count());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\CommentHelperTestData.xml",
                "GetCommentHelperTest",
                DataAccessMethod.Sequential)]
        public void GetCommentHelperTest()
        {
            DataRow[] idsData = TestContext.DataRow.GetChildRows("GetCommentHelperTest_Ids");
            DataRow[] commentData = TestContext.DataRow.GetChildRows("GetCommentHelperTest_Comment");

            int positionId = Convert.ToInt32(idsData[0]["PositionId"].ToString());
            int candidateId = Convert.ToInt32(idsData[0]["CandidateId"].ToString());

            List<Comment> commentList = new List<Comment>();
            foreach (DataRow d in commentData)
            {
                commentList.Add(new Comment
                {
                    CandidateId = Convert.ToInt32(d["CandidateId"].ToString()),
                    PositionId = Convert.ToInt32(d["PositionId"].ToString()),
                    Content = d["Content"].ToString(),
                    User = new ApplicationUser { Id = d["UserId"].ToString(), Email = d["UserEmail"].ToString() },
                    UserId = d["UserId"].ToString(),
                    CommentId = Convert.ToInt32(d["CommentId"].ToString())
                });
            }

            var commentSet = new Mock<DbSet<Comment>>().SetupData(commentList);

            var context = new Mock<ApplicationDbContext>();
            var mockPositionHelper = new Mock<IPosition>();
            var mockCandidateHelper = new Mock<ICandidate>();
            var mockPositionLogHelper = new Mock<IPositionLog>();
            context.Setup(p => p.Comments).Returns(commentSet.Object);

            var commentHelper = new CommentHelper(context.Object, mockPositionLogHelper.Object, mockPositionHelper.Object, mockCandidateHelper.Object);
            var result = commentHelper.Get(candidateId, positionId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Comment>));
            Assert.AreEqual(result.Count, commentList.Count());

            result = commentHelper.GetAll(positionId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Comment>));
            Assert.AreEqual(result.Count, commentList.Count());
        }
    }
}
