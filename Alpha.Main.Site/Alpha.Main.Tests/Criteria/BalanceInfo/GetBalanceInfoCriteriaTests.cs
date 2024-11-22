namespace Alpha.Main.Tests.Criteria.BalanceInfo
{
    using Alpha.Web.Models;
    using Main.Constants;
    using Main.Criteria.BalanceInfo;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class GetBalanceInfoCriteriaTests
    {
        [TestCase(RoleId.Agent, "Age_CustomerSelBalanceInfo_Ag")]
        [TestCase(RoleId.Master, "Age_CustomerSelBalanceInfo_MAg")]
        [TestCase(RoleId.Super, "Age_CustomerSelBalanceInfo_SAg")]
        public void GetSettingKey_Always_ReturnValidSettingKeyByLevel(byte userLevelValue, string settingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelValue);
            var criteria = new GetBalanceInfoCriteria(userLevel, 100);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(settingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_CustIdIsGreaterThanZero_ReturnTrue()
        {
            // Arrange
            var custId = 69;
            var criteria = new GetBalanceInfoCriteria(UserLevel.Agent, custId);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.IsTrue(actual);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_CustIdIsLessThanOrEqualToZero_ReturnFalse(int custId)
        {
            // Arrange
            var criteria = new GetBalanceInfoCriteria(UserLevel.Agent, custId);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void CustId_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var custId = 69;
            var criteria = new GetBalanceInfoCriteria(UserLevel.Agent, custId);

            // Act & Assert
            Assert.AreEqual(custId, criteria.CustId);
        }
    }
}