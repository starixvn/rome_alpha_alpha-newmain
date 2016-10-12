namespace Alpha.Main.Tests.Models.Dashboard.BalanceInfo
{
    using Helpers;
    using Main.Models.BalanceInfo;
    using NUnit.Framework;

    [TestFixture]
    public class BalanceInfoTests
    {
        [Test]
        public void UserName_Always_SetValueFromCtor()
        {
            // Arrange
            string userName = "user";

            // Act
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>("userName", userName);

            // Assert
            Assert.AreEqual(userName, balanceInfo.UserName);
        }

        [Test]
        public void CashBalance_Always_SetValueFromCtor()
        {
            // Arrange
            decimal cashBalance = 100;

            // Act
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>("cashBalance", cashBalance);

            // Assert
            Assert.AreEqual(cashBalance, balanceInfo.CashBalance);
        }

        [Test]
        public void YesterdayCashBalance_Always_SetValueFromCtor()
        {
            // Arrange
            decimal yesterdayCashBalance = 100;

            // Act
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>("yesterdayCashBalance", yesterdayCashBalance);

            // Assert
            Assert.AreEqual(yesterdayCashBalance, balanceInfo.YesterdayCashBalance);
        }

        [Test]
        public void TotalBalance_Always_SetValueFromCtor()
        {
            // Arrange
            decimal totalBalance = 100;

            // Act
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>("totalBalance", totalBalance);

            // Assert
            Assert.AreEqual(totalBalance, balanceInfo.TotalBalance);
        }

        [Test]
        public void YesterdayTotalBalance_Always_SetValueFromCtor()
        {
            // Arrange
            decimal yesterdayTotalBalance = 100;

            // Act
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>("yesterdayTotalBalance", yesterdayTotalBalance);

            // Assert
            Assert.AreEqual(yesterdayTotalBalance, balanceInfo.YesterdayTotalBalance);
        }

        [Test]
        public void TodayWinLoss_Always_SetValueFromCtor()
        {
            // Arrange
            decimal todayWinLoss = 100;

            // Act
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>("todayWinLoss", todayWinLoss);

            // Assert
            Assert.AreEqual(todayWinLoss, balanceInfo.TodayWinLoss);
        }

        [Test]
        public void YesterdayWinLoss_Always_SetValueFromCtor()
        {
            // Arrange
            decimal yesterdayWinLoss = 100;

            // Act
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>("yesterdayWinLoss", yesterdayWinLoss);

            // Assert
            Assert.AreEqual(yesterdayWinLoss, balanceInfo.YesterdayWinLoss);
        }

        [Test]
        public void GivenCredit_Always_SetValueFromCtor()
        {
            // Arrange
            decimal givenCredit = 100;

            // Act
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>("givenCredit", givenCredit);

            // Assert
            Assert.AreEqual(givenCredit, balanceInfo.GivenCredit);
        }

        [Test]
        public void MemberCredit_Always_SetValueFromCtor()
        {
            // Arrange
            decimal memberCredit = 100;

            // Act
            var balanceInfo = FakeDataHelper.Create<BalanceInfo>("memberCredit", memberCredit);

            // Assert
            Assert.AreEqual(memberCredit, balanceInfo.MemberCredit);
        }
    }
}