namespace Alpha.Main.Tests.Criteria.Statistics
{
    using System;
    using Main.Constants;
    using Main.Criteria.Statistics;
    using Main.Models.Enumerations;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class GetTopWinningMembersCriteriaTests
    {
        [TestCase(RoleId.Agent, "Age_CustomerSelTopWinningMembers_Ag")]
        [TestCase(RoleId.Master, "Age_CustomerSelTopWinningMembers_MAg")]
        [TestCase(RoleId.Super, "Age_CustomerSelTopWinningMembers_SAg")]
        public void GetSettingKey_Always_ReturnValidSettingKeyByLevel(byte userLevelValue, string settingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelValue);
            var criteria = new GetTopWinningMembersCriteria(
                userLevel,
                Arg.Any<int>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>());

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(settingKey, actualSettingKey);
        }

        [Test]
        public void CustId_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var custId = 69;
            var criteria = new GetTopWinningMembersCriteria(
                UserLevel.Agent,
                custId,
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>());

            // Act & Assert
            Assert.AreEqual(custId, criteria.CustId);
        }

        [Test]
        public void IsValid_CustIdIsGreaterThanZero_ReturnTrue()
        {
            // Arrange
            int custId = 69;
            var criteria = new GetTopWinningMembersCriteria(
                UserLevel.Agent,
                custId,
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>());

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(true, actual);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_CustIdIsLessThanOrEqualToZero_ReturnFalse(int custId)
        {
            // Arrange
            var criteria = new GetTopWinningMembersCriteria(
                UserLevel.Agent,
                custId,
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>());

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(false, actual);
        }

        [Test]
        public void BalanceUpDay_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var balanceUpDay = DateTime.Today;
            var criteria = new GetTopWinningMembersCriteria(
                UserLevel.Agent,
                Arg.Any<int>(),
                balanceUpDay,
                Arg.Any<DateTime>());

            // Act
            var actualBalanceUpday = criteria.BalanceUpDay;

            // Assert
            Assert.AreEqual(balanceUpDay, actualBalanceUpday);
        }

        [Test]
        public void MoveBetsDay_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var moveBetsDay = DateTime.Today;
            var criteria = new GetTopWinningMembersCriteria(
                UserLevel.Agent,
                Arg.Any<int>(),
                Arg.Any<DateTime>(),
                moveBetsDay);

            // Act
            var actualMoveBetsDay = criteria.MoveBetsDay;

            // Assert
            Assert.AreEqual(moveBetsDay, actualMoveBetsDay);
        }
    }
}