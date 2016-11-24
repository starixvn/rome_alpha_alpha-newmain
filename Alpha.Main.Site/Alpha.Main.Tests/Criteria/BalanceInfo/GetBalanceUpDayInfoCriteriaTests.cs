namespace Alpha.Main.Tests.Criteria.BalanceInfo
{
    using Main.Criteria.BalanceInfo;
    using NUnit.Framework;

    [TestFixture]
    public class GetBalanceUpDayInfoCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_Return_Age_SelBalanceUpDay()
        {
            // Arrange
            var criteria = new GetBalanceUpDayInfoCriteria();

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_SelBalanceUpDay", actualSettingKey);
        }

        [Test]
        public void IsValid_Always_ReturnTrue()
        {
            // Arrange
            var criteria = new GetBalanceUpDayInfoCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(true, actual);
        }
    }
}