namespace Alpha.Main.Site.Tests.ViewModels.Dashboard.Notice
{
    using Constants;
    using Main.Models.Enumerations;
    using Main.Models.Statistics;
    using Main.Tests.TestData.Helper;
    using NUnit.Framework;
    using Site.ViewModels.Dashboard.Statistics;

    [TestFixture]
    public class StatisticsViewModelTests
    {
        [TestCase(RoleId.Super, "Super")]
        [TestCase(RoleId.Master, "Master")]
        [TestCase(RoleId.Agent, "Agent")]
        public void OutstandingLabel_ByUserLevel_ReturnCorrectLabel(byte roleId, string expectedLabel)
        {
            // Arrange
            var stats = FakeDataHelper.Create<Statistics>();
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);

            // Act
            var viewModel = new StatisticsViewModel(stats, userLevel, false);

            // Assert
            Assert.AreEqual(expectedLabel, viewModel.OutstandingLabel);
        }

        [TestCase(RoleId.Super, "master")]
        [TestCase(RoleId.Master, "agent")]
        [TestCase(RoleId.Agent, "member")]
        public void GetOutstandingDefaultReportLevel_ByViewerLevel_ReturnCorrectLevel(byte roleId, string expectedDefaultLevel)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);

            // Act
            var viewModel = new StatisticsViewModel(null, userLevel, false);

            // Assert
            Assert.AreEqual(expectedDefaultLevel, viewModel.GetOutstandingDefaultReportLevel(userLevel));
        }
    }
}