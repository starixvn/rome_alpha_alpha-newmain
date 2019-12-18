namespace Alpha.Main.Tests.Criteria.MemberRequests
{
    using Alpha.Main.ProfilePicture.Commands;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateActorProfilePictureCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new UpdateActorProfilePictureCommand(1, 1, "192.168.1.1");
            var expectedSettingKey = "Age_CustomerUpdProfilePicture";

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
            var criteria = new UpdateActorProfilePictureCommand(actorId ,1, "192.168.1.1");

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
