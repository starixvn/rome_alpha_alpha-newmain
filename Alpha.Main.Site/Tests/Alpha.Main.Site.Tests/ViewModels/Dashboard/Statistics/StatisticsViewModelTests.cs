namespace Alpha.Main.Site.Tests.ViewModels.Dashboard.Notice
{
    using Constants;
    using Main.Models.Enumerations;
    using Main.Models.Statistics;
    using NSubstitute;
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
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);

            // Act
            var viewModel = new StatisticsViewModel(Arg.Any<Statistics>(), userLevel);

            // Assert
            Assert.AreEqual(expectedLabel, viewModel.OutstandingLabel);
        }
    }
}