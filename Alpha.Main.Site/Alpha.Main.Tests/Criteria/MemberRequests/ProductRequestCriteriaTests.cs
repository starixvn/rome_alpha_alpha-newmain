namespace Alpha.Main.Tests.Criteria.MemberRequests
{
    using Main.Criteria.MemberRequests;
    using NUnit.Framework;

    [TestFixture]
    public class ProductRequestCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var productRequestCriteria = new ProductRequestCriteria(1);
            var expectedSettingKey = "Age_PlayForFun_MemberRequest";

            // Act
            var actualSettingKey = productRequestCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_InValidCustId_ReturnsFalse()
        {
            // Arrange
            var productRequestCriteria = new ProductRequestCriteria(0);

            // Act
            var actualIsValid = productRequestCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Test]
        public void IsValid_ValidCustId_ReturnsFalse()
        {
            // Arrange
            var productRequestCriteria = new ProductRequestCriteria(1);

            // Act
            var actualIsValid = productRequestCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
