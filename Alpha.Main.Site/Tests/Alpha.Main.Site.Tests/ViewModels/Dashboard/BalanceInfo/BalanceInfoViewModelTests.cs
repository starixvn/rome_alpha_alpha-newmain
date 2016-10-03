namespace Alpha.Main.Site.Tests.ViewModels.Dashboard.Notice
{
    using Constants;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;
    using NSubstitute;
    using NUnit.Framework;
    using Site.ViewModels.Dashboard;

    [TestFixture]
    public class BalanceViewModelTests
    {
        [TestCase(RoleId.Super, "Super")]
        [TestCase(RoleId.Master, "Master")]
        [TestCase(RoleId.Agent, "Agent")]
        public void UserLevelLabel_ByUserLevel_ReturnCorrectLabel(byte roleId, string expectedLabel)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);

            // Act
            var viewModel = new BalanceInfoViewModel(Arg.Any<BalanceInfo>(), userLevel, Arg.Any<string>());

            // Assert
            Assert.AreEqual(expectedLabel, viewModel.UserLevelLabel);
        }

        [TestCase(RoleId.Super, "Super Given")]
        [TestCase(RoleId.Master, "Master Given")]
        [TestCase(RoleId.Agent, "Agent Given")]
        public void GivenCreditLabel_ByUserLevel_ReturnCorrectLabel(byte roleId, string expectedLabel)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);

            // Act
            var viewModel = new BalanceInfoViewModel(Arg.Any<BalanceInfo>(), userLevel, Arg.Any<string>());

            // Assert
            Assert.AreEqual(expectedLabel, viewModel.GivenCreditLabel);
        }

        [TestCase(RoleId.Super, "Total Master")]
        [TestCase(RoleId.Master, "Total Agent")]
        [TestCase(RoleId.Agent, "Total Member")]
        public void TotalDownlineCreditLabel_ByUserLevel_ReturnCorrectLabel(byte roleId, string expectedLabel)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);

            // Act
            var viewModel = new BalanceInfoViewModel(Arg.Any<BalanceInfo>(), userLevel, Arg.Any<string>());

            // Assert
            Assert.AreEqual(expectedLabel, viewModel.TotalDownlineCreditLabel);
        }
    }
}