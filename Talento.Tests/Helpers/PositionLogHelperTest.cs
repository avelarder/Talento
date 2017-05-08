using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talento.Core.Data;
using Talento.Core.Helpers;
using Talento.Entities;

namespace Talento.Tests.Helpers
{
    [TestClass]
    public class PositionLogHelperTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                "Resources\\Tests\\PositionLogHelperTestData.xml",
                "AddPositionLogHelperTest",
                DataAccessMethod.Sequential)]
        public void AddPositionLogHelperTest()
        {
            //Test data
            DataRow[] applicationSettingsData = TestContext.DataRow.GetChildRows("AddPositionLogHelperTest_Log");

            Log log = new Log
            {
                Id = Convert.ToInt32(applicationSettingsData[0]["Id"]),
                Action = (Entities.Action)Enum.Parse(typeof(Entities.Action), applicationSettingsData[0]["Action"].ToString()),
                ActualStatus = (PositionStatus)Enum.Parse(typeof(PositionStatus), applicationSettingsData[0]["ActualStatus"].ToString()),
                PreviousStatus = (PositionStatus)Enum.Parse(typeof(PositionStatus), applicationSettingsData[0]["PreviousStatus"].ToString()),
                User = new ApplicationUser { Email = applicationSettingsData[0]["User_Email"].ToString(), Id = applicationSettingsData[0]["ApplicationUser_Id"].ToString(), UserName = applicationSettingsData[0]["User_Email"].ToString() },
                ApplicationUser_Id = applicationSettingsData[0]["ApplicationUser_Id"].ToString(),
                Date = Convert.ToDateTime(applicationSettingsData[0]["Date"].ToString()),
                Description = applicationSettingsData[0]["Description"].ToString(),
                Position = new Position { PositionId = Convert.ToInt32(applicationSettingsData[0]["Position_Id"].ToString()) }
            };

            var set = new Mock<DbSet<Log>>();

            // mock dbContext
            var context = new Mock<ApplicationDbContext>();
            context.Setup(c => c.PositionLogs).Returns(set.Object);
            context.Setup(x => x.SaveChanges()).Returns(1);

            PositionLogHelper positionLogHelper = new PositionLogHelper(context.Object);

            // execute helper query
            int result = positionLogHelper.Add(log);

            Assert.IsTrue(result == 1);
        }
    }
}
