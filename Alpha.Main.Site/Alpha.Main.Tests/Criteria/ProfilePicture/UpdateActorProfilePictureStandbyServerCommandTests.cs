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
            var ip = "192.168.1.1";
            var command = new UpdateActorProfilePictureStandbyServerCommand(1, 1, ip);
            var expectedSettingKey = "Age_CustomerUpdProfilePicture_Standby";

            // Act
            var actualSettingKey = command.GetSettingKey();

            // Assert
            Assert.AreEqual(command.ActorId, 1);
            Assert.AreEqual(command.IsDefault, true);
            Assert.AreEqual(command.IsMainServer, false);
            Assert.AreEqual(command.Mode, 1);
            Assert.AreEqual(command.IP, ip);
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }
    }
}
