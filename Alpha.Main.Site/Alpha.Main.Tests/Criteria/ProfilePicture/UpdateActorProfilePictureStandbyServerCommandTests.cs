namespace Alpha.Main.Tests.Criteria.MemberRequests
{
    using Alpha.Main.ProfilePicture.Commands;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateActorProfilePictureStandbyServerCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new UpdateActorProfilePictureStandbyServerCommand(1, 1, "192.168.1.1");
            var expectedSettingKey = "Age_CustomerUpdProfilePicture_Standby";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }
    }
}
