namespace Alpha.Main.Tests.Models.Dashboard.Statistics
{
    using Helpers;
    using Main.Models.Statistics;
    using NUnit.Framework;

    [TestFixture]
    public class TopWinningMemberTests
    {
        [Test]
        public void CustId_Always_SetValueFromCtor()
        {
            // Arrange
            int custId = 100;

            // Act
            var member = FakeDataHelper.Create<TopWinningMember>("custId", custId);

            // Assert
            Assert.AreEqual(custId, member.CustId);
        }

        [Test]
        public void UserName_Always_SetValueFromCtor()
        {
            // Arrange
            string userName = "member";

            // Act
            var member = FakeDataHelper.Create<TopWinningMember>("userName", userName);

            // Assert
            Assert.AreEqual(userName, member.UserName);
        }

        [Test]
        public void WinLoss_Always_SetValueFromCtor()
        {
            // Arrange
            decimal winLoss = 10;

            // Act
            var member = FakeDataHelper.Create<TopWinningMember>("winLoss", winLoss);

            // Assert
            Assert.AreEqual(winLoss, member.WinLoss);
        }
    }
}