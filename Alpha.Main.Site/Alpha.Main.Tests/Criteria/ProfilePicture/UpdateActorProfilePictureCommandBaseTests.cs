namespace Alpha.Main.Tests.Criteria.MemberRequests
{
    using Alpha.Main.ProfilePicture.Commands;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateActorProfilePictureCommandBaseTests
    {
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void IsValid_InvalidCustId_ReturnsFalse(int actorId, bool expectedResult)
        {
            // Arrange
            var criteria = new UpdateActorProfilePictureCommandBase(actorId ,1, "192.168.1.1", true);

            // Act
            var actualResult = criteria.IsValid();

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void GetSettingKey_Always_ReturnsemptySettingKey()
        {
            var criteria = new UpdateActorProfilePictureCommandBase(1, 1, "192.168.1.1", true);

            var actualSettingKey = criteria.GetSettingKey();

            Assert.AreEqual(actualSettingKey, string.Empty);
        }
    }
}
