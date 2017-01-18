namespace Alpha.Main.Site.Tests.ViewModels.Dashboard.BalanceInfo
{
    using System;
    using Constants;
    using Main.Models.BalanceInfo;
    using Main.Models.Enumerations;
    using Main.Tests.TestData.Helper;
    using NUnit.Framework;
    using Site.ViewModels.Dashboard.BalanceInfo;

    [TestFixture]
    public class BalanceViewModelTests
    {
        private BalanceInfo balanceInfo;

        [SetUp]
        public void Setup()
        {
            balanceInfo = FakeDataHelper.Create<BalanceInfo>();
        }

        [TestCase(RoleId.Super, "Super")]
        [TestCase(RoleId.Master, "Master")]
        [TestCase(RoleId.Agent, "Agent")]
        public void UserLevelLabel_ByUserLevel_ReturnCorrectLabel(byte roleId, string expectedLabel)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);

            // Act
            var viewModel = new BalanceInfoViewModel(
                balanceInfo,
                userLevel,
                "UUS",
                1);

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
            var viewModel = new BalanceInfoViewModel(
                balanceInfo,
                userLevel,
                "UUS",
                1);

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
            var viewModel = new BalanceInfoViewModel(
                balanceInfo,
                userLevel,
                "UUS",
                1);

            // Assert
            Assert.AreEqual(expectedLabel, viewModel.TotalDownlineCreditLabel);
        }
    }
}