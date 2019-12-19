namespace Alpha.Main.Tests.Criteria.MemberRequests
{
    using Alpha.Main.ProfilePicture.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class SelectActorProfilePictureCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SelectActorProfilePictureCriteria(1);
            var expectedSettingKey = "Age_CustomerSelProfilePicture";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void IsValid_InvalidCustId_ReturnsFalse(int actorId, bool expectedResult)
        {
            // Arrange
            var criteria = new SelectActorProfilePictureCriteria(actorId);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
