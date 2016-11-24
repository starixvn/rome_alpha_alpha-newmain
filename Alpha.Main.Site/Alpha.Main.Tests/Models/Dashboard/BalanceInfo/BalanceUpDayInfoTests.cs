namespace Alpha.Main.Tests.Models.Dashboard.BalanceInfo
{
    using System;
    using Main.Models.BalanceInfo;
    using NUnit.Framework;
    using TestData.Helper;

    public class BalanceUpDayInfoTests
    {
        [Test]
        public void BalanceUpDay_Always_SetValueFromCtor()
        {
            // Arrange
            DateTime balanceUpDay = DateTime.Today;

            // Act
            var balanceUpDayInfo = FakeDataHelper.Create<BalanceUpDayInfo>("balanceUpDay", balanceUpDay);

            // Assert
            Assert.AreEqual(balanceUpDay, balanceUpDayInfo.BalanceUpDay);
        }

        [Test]
        public void MoveBetsDay_Always_SetValueFromCtor()
        {
            // Arrange
            DateTime moveBetsDay = DateTime.Today;

            // Act
            var balanceUpDayInfo = FakeDataHelper.Create<BalanceUpDayInfo>("moveBetsDay", moveBetsDay);

            // Assert
            Assert.AreEqual(moveBetsDay, balanceUpDayInfo.MoveBetsDay);
        }
    }
}