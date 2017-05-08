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
    public class CandidateHelperTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\CandidateHelperTestData.xml",
                "CreateCandidateHelperTest",
                DataAccessMethod.Sequential)]
        public void CreateCandidateHelperTest()
        {
            //Test data
            DataRow[] candidateData = TestContext.DataRow.GetChildRows("CreateCandidateHelperTest_Candidate");
            DataRow[] positionData = TestContext.DataRow.GetChildRows("CreateCandidateHelperTest_Position");

            Candidate candidate = new Candidate
            {
                CandidateId = Convert.ToInt32(candidateData[0]["CandidateId"]),
                Competencies = candidateData[0]["Competencies"].ToString(),
                CreatedBy = new ApplicationUser { Id = candidateData[0]["CreatedBy_Id"].ToString(), Email = candidateData[0]["CreatedBy_Email"].ToString(), UserName = candidateData[0]["CreatedBy_Email"].ToString() },
                CreatedBy_Id = candidateData[0]["CreatedBy_Id"].ToString(),
                CreatedOn = Convert.ToDateTime(candidateData[0]["CreatedOn"].ToString()),
                Description = candidateData[0]["Description"].ToString(),
                Email = candidateData[0]["Email"].ToString(),
                FileBlobs = new List<FileBlob> { new FileBlob { Blob = new byte[0], FileName = candidateData[0]["FileBlob_Name"].ToString() } },
                IsTcsEmployee = Convert.ToBoolean(candidateData[0]["IsTcsEmployee"].ToString()),
                Name = candidateData[0]["Name"].ToString(),
                PositionCandidates = new List<PositionCandidates> { new PositionCandidates { Position = new Position { PositionId = Convert.ToInt32(candidateData[0]["Position_Id"].ToString()) }, Status = (PositionCandidatesStatus)Enum.Parse(typeof(PositionCandidatesStatus), candidateData[0]["PositionCandidate_Status"].ToString()) } }
            };

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

            var candidateSet = new Mock<DbSet<Candidate>>();
            var logSet = new Mock<DbSet<Log>>();
            var positionCandidateSet = new Mock<DbSet<PositionCandidates>>();
            var fileblobSet = new Mock<DbSet<FileBlob>>();


            // mock dbContext
            var expected = 1;
            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Candidates).Returns(candidateSet.Object);
            context.Setup(c => c.PositionLogs).Returns(logSet.Object);
            context.Setup(c => c.PositionCandidates).Returns(positionCandidateSet.Object);
            context.Setup(c => c.FileBlobs).Returns(fileblobSet.Object);
            context.Setup(x => x.SaveChanges()).Returns(1);

            // mock helpers
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<IPositionLog> mockPositionLogHelper = mocks.Create<IPositionLog>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            mockPositionHelper.Setup(x => x.Get(1)).Returns(position);
            
            CandidateHelper candidateHelper = new CandidateHelper(context.Object, mockPositionLogHelper.Object, mockUserHelper.Object, mockPositionHelper.Object);

            // execute helper query
            int result = candidateHelper.Create(candidate);

            Assert.AreEqual(expected, result);
        }

    }
}
