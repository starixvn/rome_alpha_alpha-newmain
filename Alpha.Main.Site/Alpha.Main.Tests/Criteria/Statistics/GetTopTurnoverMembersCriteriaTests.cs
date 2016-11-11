namespace Alpha.Main.Tests.Criteria.Statistics
{
    using System;
    using Main.Constants;
    using Main.Criteria.Statistics;
    using Main.Models.Enumerations;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class GetTopTurnoverMembersCriteriaTests
    {
        [TestCase(RoleId.Agent, "Age_CustomerSelTopTurnoverMembers_Ag")]
        [TestCase(RoleId.Master, "Age_CustomerSelTopTurnoverMembers_MAg")]
        [TestCase(RoleId.Super, "Age_CustomerSelTopTurnoverMembers_SAg")]
        public void GetSettingKey_Always_ReturnValidSettingKeyByLevel(byte userLevelValue, string settingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelValue);
            var criteria = new GetTopTurnoverMembersCriteria(
                userLevel,
                1,
                DateTime.Today,
                DateTime.Today);

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
            var criteria = new GetTopTurnoverMembersCriteria(
                UserLevel.Agent,
                custId,
                DateTime.Today,
                DateTime.Today);

            // Act & Assert
            Assert.AreEqual(custId, criteria.CustId);
        }

        [Test]
        public void IsValid_CustIdIsGreaterThanZero_ReturnTrue()
        {
            // Arrange
            int custId = 69;
            var criteria = new GetTopTurnoverMembersCriteria(
                UserLevel.Agent,
                custId,
                DateTime.Today,
                DateTime.Today);

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
            var criteria = new GetTopTurnoverMembersCriteria(
                UserLevel.Agent,
                custId,
                DateTime.Today,
                DateTime.Today);

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
            var criteria = new GetTopTurnoverMembersCriteria(
                UserLevel.Agent,
                1,
                balanceUpDay,
                DateTime.Today);

            // Act
            var actualBalanceUpday = criteria.BalanceUpDay;

            // Assert
            Assert.AreEqual(DateTime.Today, actualBalanceUpday);
        }

        [Test]
        public void MoveBetsDay_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var moveBetsDay = DateTime.Today;
            var criteria = new GetTopTurnoverMembersCriteria(
                UserLevel.Agent,
                1,
                DateTime.Today,
                moveBetsDay);

            // Act
            var actualMoveBetsDay = criteria.MoveBetsDay;

            // Assert
            Assert.AreEqual(DateTime.Today, actualMoveBetsDay);
        }
    }
}