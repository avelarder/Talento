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

            var candidateSet = new Mock<DbSet<Candidate>>().SetupData(new List<Candidate>());
            var logSet = new Mock<DbSet<Log>>();
            var positionCandidateSet = new Mock<DbSet<PositionCandidates>>();
            var fileblobSet = new Mock<DbSet<FileBlob>>();


            // mock dbContext
            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Candidates).Returns(candidateSet.Object);
            context.Setup(c => c.PositionLogs).Returns(logSet.Object);
            context.Setup(c => c.PositionCandidates).Returns(positionCandidateSet.Object);
            context.Setup(c => c.FileBlobs).Returns(fileblobSet.Object);
            context.Setup(x => x.SaveChanges()).Verifiable();

            // mock helpers
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<IPositionLog> mockPositionLogHelper = mocks.Create<IPositionLog>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            mockPositionHelper.Setup(x => x.Get(1)).Returns(position);

            CandidateHelper candidateHelper = new CandidateHelper(context.Object, mockPositionLogHelper.Object, mockUserHelper.Object, mockPositionHelper.Object);

            // execute helper query
            candidateHelper.Create(candidate);

            Assert.AreEqual(candidate, candidateSet.Object.Where(c => c.CandidateId == candidate.CandidateId).Single());
            candidateSet.Verify(m => m.Add(It.IsAny<Candidate>()), Times.Once());
            context.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\CandidateHelperTestData.xml",
                "EditCandidateHelperTest",
                DataAccessMethod.Sequential)]
        public void EditCandidateHelperTest()
        {
            DataRow originalCandidateData = TestContext.DataRow.GetChildRows("EditCandidateHelperTest_OriginalCandidate").Single();
            DataRow editedCandidateData = TestContext.DataRow.GetChildRows("EditCandidateHelperTest_EditedCandidate").Single();
            DataRow positionData = TestContext.DataRow.GetChildRows("EditCandidateHelperTest_Position").Single();
            DataRow applicationUserData = TestContext.DataRow.GetChildRows("EditCandidateHelperTest_ApplicationUser").Single();

            Candidate originalCandidate = new Candidate
            {
                CandidateId = Convert.ToInt32(originalCandidateData["CandidateId"]),
                Competencies = originalCandidateData["Competencies"].ToString(),
                CreatedBy = new ApplicationUser { Id = originalCandidateData["CreatedBy_Id"].ToString(), Email = originalCandidateData["CreatedBy_Email"].ToString(), UserName = originalCandidateData["CreatedBy_Email"].ToString() },
                CreatedBy_Id = originalCandidateData["CreatedBy_Id"].ToString(),
                CreatedOn = Convert.ToDateTime(originalCandidateData["CreatedOn"].ToString()),
                Description = originalCandidateData["Description"].ToString(),
                Email = originalCandidateData["Email"].ToString(),
                FileBlobs = new List<FileBlob> { new FileBlob { Blob = new byte[0], FileName = originalCandidateData["FileBlob_Name"].ToString() } },
                IsTcsEmployee = Convert.ToBoolean(originalCandidateData["IsTcsEmployee"].ToString()),
                Name = originalCandidateData["Name"].ToString(),
                PositionCandidates = new List<PositionCandidates> { new PositionCandidates { Position = new Position { PositionId = Convert.ToInt32(originalCandidateData["Position_Id"].ToString()) }, Status = (PositionCandidatesStatus)Enum.Parse(typeof(PositionCandidatesStatus), originalCandidateData["PositionCandidate_Status"].ToString()) } }
            };

            Candidate editedCandidate = new Candidate
            {
                CandidateId = Convert.ToInt32(editedCandidateData["CandidateId"]),
                Competencies = editedCandidateData["Competencies"].ToString(),
                CreatedBy = new ApplicationUser { Id = editedCandidateData["CreatedBy_Id"].ToString(), Email = editedCandidateData["CreatedBy_Email"].ToString(), UserName = editedCandidateData["CreatedBy_Email"].ToString() },
                CreatedBy_Id = editedCandidateData["CreatedBy_Id"].ToString(),
                CreatedOn = Convert.ToDateTime(editedCandidateData["CreatedOn"].ToString()),
                Description = editedCandidateData["Description"].ToString(),
                Email = editedCandidateData["Email"].ToString(),
                FileBlobs = new List<FileBlob> { new FileBlob { Blob = new byte[0], FileName = editedCandidateData["FileBlob_Name"].ToString() } },
                IsTcsEmployee = Convert.ToBoolean(editedCandidateData["IsTcsEmployee"].ToString()),
                Name = editedCandidateData["Name"].ToString(),
                PositionCandidates = new List<PositionCandidates> { new PositionCandidates { Position = new Position { PositionId = Convert.ToInt32(editedCandidateData["Position_Id"].ToString()) }, Status = (PositionCandidatesStatus)Enum.Parse(typeof(PositionCandidatesStatus), editedCandidateData["PositionCandidate_Status"].ToString()) } }
            };

            Position position = new Position
            {
                PositionId = Convert.ToInt32(positionData["PositionId"]),
                Title = positionData["Title"].ToString(),
                Description = positionData["Description"].ToString(),
                CreationDate = Convert.ToDateTime(positionData["CreationDate"].ToString()),
                Area = positionData["Area"].ToString(),
                EngagementManager = positionData["EngagementManager"].ToString(),
                Owner = new ApplicationUser { Id = positionData["Owner_Id"].ToString(), Email = positionData["Owner_Email"].ToString() },
                ApplicationUser_Id = positionData["Owner_Id"].ToString(),
                PortfolioManager = new ApplicationUser { Id = positionData["PortfolioManager_Id"].ToString(), Email = positionData["PortfolioManager_Email"].ToString() },
                RGS = positionData["RGS"].ToString(),
                Status = (PositionStatus)Enum.Parse(typeof(PositionStatus), positionData["Status"].ToString()),
                OpenStatus = (PositionOpenStatus)Enum.Parse(typeof(PositionOpenStatus), positionData["OpenStatus"].ToString()),
                PositionCandidates = new List<PositionCandidates>(),
            };

            ApplicationUser currentUser = new ApplicationUser
            {
                Id = applicationUserData["ApplicationUserId"].ToString(),
                Email = applicationUserData["Email"].ToString()
            };

            var candidateSet = new Mock<DbSet<Candidate>>().SetupData(new List<Candidate> { originalCandidate });
            var logSet = new Mock<DbSet<Log>>();
            var positionCandidateSet = new Mock<DbSet<PositionCandidates>>().SetupData(new List<PositionCandidates> { new PositionCandidates { Candidate = originalCandidate, Position = position, Status = (PositionCandidatesStatus)Enum.Parse(typeof(PositionCandidatesStatus), editedCandidateData["PositionCandidate_Status"].ToString()) } });
            var fileblobSet = new Mock<DbSet<FileBlob>>();

            // mock dbContext
            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Candidates).Returns(candidateSet.Object);
            context.Setup(c => c.PositionLogs).Returns(logSet.Object);
            context.Setup(c => c.PositionCandidates).Returns(positionCandidateSet.Object);
            context.Setup(c => c.FileBlobs).Returns(fileblobSet.Object);
            context.Setup(x => x.SaveChanges()).Verifiable();

            // mock helpers
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<IPositionLog> mockPositionLogHelper = mocks.Create<IPositionLog>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();
            mockPositionHelper.Setup(x => x.Get(1)).Returns(position);

            CandidateHelper candidateHelper = new CandidateHelper(context.Object, mockPositionLogHelper.Object, mockUserHelper.Object, mockPositionHelper.Object);

            // execute helper query
            candidateHelper.Edit(editedCandidate, new HashSet<FileBlob>(), currentUser);
            context.Verify(m => m.SaveChanges(), Times.AtLeastOnce());
        }


        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\CandidateHelperTestData.xml",
                "GetCandidateHelperTest",
                DataAccessMethod.Sequential)]
        public void GetCandidateHelperTest()
        {
            DataRow candidateData = TestContext.DataRow.GetChildRows("GetCandidateHelperTest_Candidate").Single();

            Candidate candidate = new Candidate
            {
                CandidateId = Convert.ToInt32(candidateData["CandidateId"]),
                Competencies = candidateData["Competencies"].ToString(),
                CreatedBy = new ApplicationUser { Id = candidateData["CreatedBy_Id"].ToString(), Email = candidateData["CreatedBy_Email"].ToString(), UserName = candidateData["CreatedBy_Email"].ToString() },
                CreatedBy_Id = candidateData["CreatedBy_Id"].ToString(),
                CreatedOn = Convert.ToDateTime(candidateData["CreatedOn"].ToString()),
                Description = candidateData["Description"].ToString(),
                Email = candidateData["Email"].ToString(),
                FileBlobs = new List<FileBlob> { new FileBlob { Blob = new byte[0], FileName = candidateData["FileBlob_Name"].ToString() } },
                IsTcsEmployee = Convert.ToBoolean(candidateData["IsTcsEmployee"].ToString()),
                Name = candidateData["Name"].ToString(),
                PositionCandidates = new List<PositionCandidates> { new PositionCandidates { Position = new Position { PositionId = Convert.ToInt32(candidateData["Position_Id"].ToString()) }, Status = (PositionCandidatesStatus)Enum.Parse(typeof(PositionCandidatesStatus), candidateData["PositionCandidate_Status"].ToString()) } }
            };

            var candidateSet = new Mock<DbSet<Candidate>>().SetupData(new List<Candidate> { candidate });
            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Candidates).Returns(candidateSet.Object);
            context.Setup(x => x.SaveChanges()).Verifiable();

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<IPositionLog> mockPositionLogHelper = mocks.Create<IPositionLog>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();

            CandidateHelper candidateHelper = new CandidateHelper(context.Object, mockPositionLogHelper.Object, mockUserHelper.Object, mockPositionHelper.Object);

            var resultCandidate = candidateHelper.Get(candidate.CandidateId);

            Assert.IsNotNull(resultCandidate);
            Assert.IsInstanceOfType(resultCandidate, typeof(Candidate));
            Assert.AreEqual(candidate, (Candidate)resultCandidate);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\CandidateHelperTestData.xml",
                "AddTechnicalInterviewCandidateHelperTest",
                DataAccessMethod.Sequential)]
        public void AddTechnicalInterviewCandidateHelperTest()
        {
            DataRow candidateData = TestContext.DataRow.GetChildRows("AddTechnicalInterviewCandidateHelperTest_Candidate").Single();
            DataRow positionData = TestContext.DataRow.GetChildRows("AddTechnicalInterviewCandidateHelperTest_Position").Single();
            DataRow applicationUserData = TestContext.DataRow.GetChildRows("AddTechnicalInterviewCandidateHelperTest_ApplicationUser").Single();
            DataRow technicalInterviewData = TestContext.DataRow.GetChildRows("AddTechnicalInterviewCandidateHelperTest_TechnicalInterview").Single();

            Candidate candidate = new Candidate
            {
                CandidateId = Convert.ToInt32(candidateData["CandidateId"]),
                Competencies = candidateData["Competencies"].ToString(),
                CreatedBy = new ApplicationUser { Id = candidateData["CreatedBy_Id"].ToString(), Email = candidateData["CreatedBy_Email"].ToString(), UserName = candidateData["CreatedBy_Email"].ToString() },
                CreatedBy_Id = candidateData["CreatedBy_Id"].ToString(),
                CreatedOn = Convert.ToDateTime(candidateData["CreatedOn"].ToString()),
                Description = candidateData["Description"].ToString(),
                Email = candidateData["Email"].ToString(),
                FileBlobs = new List<FileBlob> { new FileBlob { Blob = new byte[0], FileName = candidateData["FileBlob_Name"].ToString() } },
                IsTcsEmployee = Convert.ToBoolean(candidateData["IsTcsEmployee"].ToString()),
                Name = candidateData["Name"].ToString(),
                PositionCandidates = new List<PositionCandidates> { new PositionCandidates { Position = new Position { PositionId = Convert.ToInt32(candidateData["Position_Id"].ToString()) }, Status = (PositionCandidatesStatus)Enum.Parse(typeof(PositionCandidatesStatus), candidateData["PositionCandidate_Status"].ToString()) } }
            };
            Position position = new Position
            {
                PositionId = Convert.ToInt32(positionData["PositionId"]),
                Title = positionData["Title"].ToString(),
                Description = positionData["Description"].ToString(),
                CreationDate = Convert.ToDateTime(positionData["CreationDate"].ToString()),
                Area = positionData["Area"].ToString(),
                EngagementManager = positionData["EngagementManager"].ToString(),
                Owner = new ApplicationUser { Id = positionData["Owner_Id"].ToString(), Email = positionData["Owner_Email"].ToString() },
                ApplicationUser_Id = positionData["Owner_Id"].ToString(),
                PortfolioManager = new ApplicationUser { Id = positionData["PortfolioManager_Id"].ToString(), Email = positionData["PortfolioManager_Email"].ToString() },
                RGS = positionData["RGS"].ToString(),
                Status = (PositionStatus)Enum.Parse(typeof(PositionStatus), positionData["Status"].ToString()),
                OpenStatus = (PositionOpenStatus)Enum.Parse(typeof(PositionOpenStatus), positionData["OpenStatus"].ToString()),
                PositionCandidates = new List<PositionCandidates>(),
            };
            ApplicationUser currentUser = new ApplicationUser
            {
                Id = applicationUserData["ApplicationUserId"].ToString(),
                Email = applicationUserData["Email"].ToString()
            };
            PositionCandidates positionCandidate = new PositionCandidates
            {
                Candidate = candidate,
                Position = position,
                CandidateID = candidate.CandidateId,
                PositionID = position.PositionId,
                Status = PositionCandidatesStatus.New
            };
            TechnicalInterview technicalInterview = new TechnicalInterview
            {
                TechnicalInterviewId = Convert.ToInt32(technicalInterviewData["TechnicalInterviewId"].ToString()),
                Date = Convert.ToDateTime(technicalInterviewData["Date"].ToString()),
                IsAccepted = Convert.ToBoolean(technicalInterviewData["IsAccepted"].ToString()),
                Comment = technicalInterviewData["Comment"].ToString(),
                InterviewerId = technicalInterviewData["InterviewerId"].ToString(),
                InterviewerName = technicalInterviewData["InterviewerId"].ToString(),
                PositionCandidate = positionCandidate,
                FeedbackFile = new FileBlob()
            };

            var candidateSet = new Mock<DbSet<Candidate>>().SetupData(new List<Candidate> { candidate });
            var logSet = new Mock<DbSet<Log>>();
            var positionCandidateSet = new Mock<DbSet<PositionCandidates>>().SetupData(new List<PositionCandidates> { positionCandidate });
            var fileblobSet = new Mock<DbSet<FileBlob>>();
            var technicalInterviewSet = new Mock<DbSet<TechnicalInterview>>().SetupData(new List<TechnicalInterview>());

            // mock dbContext
            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Candidates).Returns(candidateSet.Object);
            context.Setup(c => c.PositionLogs).Returns(logSet.Object);
            context.Setup(c => c.PositionCandidates).Returns(positionCandidateSet.Object);
            context.Setup(c => c.FileBlobs).Returns(fileblobSet.Object);
            context.Setup(c => c.TechnicalInterviews).Returns(technicalInterviewSet.Object);
            context.Setup(x => x.SaveChanges()).Verifiable();

            // mock helpers
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<IPositionLog> mockPositionLogHelper = mocks.Create<IPositionLog>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();

            CandidateHelper candidateHelper = new CandidateHelper(context.Object, mockPositionLogHelper.Object, mockUserHelper.Object, mockPositionHelper.Object);

            candidateHelper.AddTechnicalInterview(technicalInterview, currentUser, position.PositionId, candidate.Email);

            Assert.AreEqual(technicalInterview, technicalInterviewSet.Object.Where(c => c.TechnicalInterviewId == technicalInterview.TechnicalInterviewId).Single());
            technicalInterviewSet.Verify(m => m.Add(It.IsAny<TechnicalInterview>()), Times.Once());
            context.Verify(m => m.SaveChanges(), Times.Once());

            if (technicalInterview.IsAccepted)
            {
                Assert.AreEqual(positionCandidate.Status, PositionCandidatesStatus.Interview_Accepted);
            }
            else
            {
                Assert.AreEqual(positionCandidate.Status, PositionCandidatesStatus.Interview_Rejected);
            }
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\CandidateHelperTestData.xml",
                "ChangeStatusCandidateHelperTest",
                DataAccessMethod.Sequential)]
        public void ChangeStatusCandidateHelperTest()
        {
            DataRow candidateData = TestContext.DataRow.GetChildRows("ChangeStatusCandidateHelperTest_Candidate").Single();
            DataRow positionData = TestContext.DataRow.GetChildRows("ChangeStatusCandidateHelperTest_Position").Single();
            DataRow applicationUserData = TestContext.DataRow.GetChildRows("ChangeStatusCandidateHelperTest_ApplicationUser").Single();
            DataRow newPositionCandidateStatusData = TestContext.DataRow.GetChildRows("ChangeStatusCandidateHelperTest_NewPositionCandidateStatus").Single();

            Candidate candidate = new Candidate
            {
                CandidateId = Convert.ToInt32(candidateData["CandidateId"]),
                Competencies = candidateData["Competencies"].ToString(),
                CreatedBy = new ApplicationUser { Id = candidateData["CreatedBy_Id"].ToString(), Email = candidateData["CreatedBy_Email"].ToString(), UserName = candidateData["CreatedBy_Email"].ToString() },
                CreatedBy_Id = candidateData["CreatedBy_Id"].ToString(),
                CreatedOn = Convert.ToDateTime(candidateData["CreatedOn"].ToString()),
                Description = candidateData["Description"].ToString(),
                Email = candidateData["Email"].ToString(),
                FileBlobs = new List<FileBlob> { new FileBlob { Blob = new byte[0], FileName = candidateData["FileBlob_Name"].ToString() } },
                IsTcsEmployee = Convert.ToBoolean(candidateData["IsTcsEmployee"].ToString()),
                Name = candidateData["Name"].ToString(),
                PositionCandidates = new List<PositionCandidates> { new PositionCandidates { Position = new Position { PositionId = Convert.ToInt32(candidateData["Position_Id"].ToString()) }, Status = (PositionCandidatesStatus)Enum.Parse(typeof(PositionCandidatesStatus), candidateData["PositionCandidate_Status"].ToString()) } }
            };
            Position position = new Position
            {
                PositionId = Convert.ToInt32(positionData["PositionId"]),
                Title = positionData["Title"].ToString(),
                Description = positionData["Description"].ToString(),
                CreationDate = Convert.ToDateTime(positionData["CreationDate"].ToString()),
                Area = positionData["Area"].ToString(),
                EngagementManager = positionData["EngagementManager"].ToString(),
                Owner = new ApplicationUser { Id = positionData["Owner_Id"].ToString(), Email = positionData["Owner_Email"].ToString() },
                ApplicationUser_Id = positionData["Owner_Id"].ToString(),
                PortfolioManager = new ApplicationUser { Id = positionData["PortfolioManager_Id"].ToString(), Email = positionData["PortfolioManager_Email"].ToString() },
                RGS = positionData["RGS"].ToString(),
                Status = (PositionStatus)Enum.Parse(typeof(PositionStatus), positionData["Status"].ToString()),
                OpenStatus = (PositionOpenStatus)Enum.Parse(typeof(PositionOpenStatus), positionData["OpenStatus"].ToString()),
                PositionCandidates = new List<PositionCandidates>(),
            };
            ApplicationUser currentUser = new ApplicationUser
            {
                Id = applicationUserData["ApplicationUserId"].ToString(),
                Email = applicationUserData["Email"].ToString()
            };
            PositionCandidates positionCandidate = new PositionCandidates
            {
                Candidate = candidate,
                Position = position,
                CandidateID = candidate.CandidateId,
                PositionID = position.PositionId,
                Status = (PositionCandidatesStatus)Enum.Parse(typeof(PositionCandidatesStatus), positionData["PositionCandidate_Status"].ToString())
            };
            PositionCandidatesStatus newStatus = (PositionCandidatesStatus)Enum.Parse(typeof(PositionCandidatesStatus), newPositionCandidateStatusData["PositionCandidate_Status"].ToString());

            var candidateSet = new Mock<DbSet<Candidate>>().SetupData(new List<Candidate> { candidate });
            var positionCandidateSet = new Mock<DbSet<PositionCandidates>>().SetupData(new List<PositionCandidates> { positionCandidate });

            // mock dbContext
            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.Candidates).Returns(candidateSet.Object);
            context.Setup(c => c.PositionCandidates).Returns(positionCandidateSet.Object);
            context.Setup(x => x.SaveChanges()).Verifiable();

            // mock helpers
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPosition> mockPositionHelper = mocks.Create<IPosition>();
            Mock<IPositionLog> mockPositionLogHelper = mocks.Create<IPositionLog>();
            Mock<ICustomUser> mockUserHelper = mocks.Create<ICustomUser>();

            CandidateHelper candidateHelper = new CandidateHelper(context.Object, mockPositionLogHelper.Object, mockUserHelper.Object, mockPositionHelper.Object);

            candidateHelper.ChangeStatus(candidate.CandidateId, newStatus, currentUser);

            Assert.AreEqual(positionCandidate.Status, newStatus);
            context.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}
