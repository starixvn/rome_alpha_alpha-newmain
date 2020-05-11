namespace Alpha.Main.Tests.Criteria.Statistics
{
    using Alpha.Web.Models;
    using Main.Constants;
    using Main.Criteria.Statistics;
    using NUnit.Framework;

    [TestFixture]
    public class GetStatisticsCriteriaTests
    {
        [TestCase(RoleId.Agent, "Age_CustomerSelStatisticsInfo_Ag")]
        [TestCase(RoleId.Master, "Age_CustomerSelStatisticsInfo_MAg")]
        [TestCase(RoleId.Super, "Age_CustomerSelStatisticsInfo_SAg")]
        public void GetSettingKey_Always_ReturnValidSettingKeyByLevel(byte userLevelValue, string settingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelValue);
            var criteria = new GetStatisticsCriteria(userLevel, 1);

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
            var criteria = new GetStatisticsCriteria(UserLevel.Agent, custId);

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
            var criteria = new GetStatisticsCriteria(UserLevel.Agent, custId);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(false, actual);
        }

        [Test]
        public void CustId_Always_ReturnValueSetFromCtor()
        {
            // Arrange
            var custId = 69;
            var criteria = new GetStatisticsCriteria(UserLevel.Agent, custId);

            // Act & Assert
            Assert.AreEqual(custId, criteria.CustId);
        }
    }
}