namespace Alpha.Main.Tests.Criteria.Nickname
{
    using Alpha.Main.Criteria.Nickname;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateNicknameLoginOnlyCommand_Tests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var command = new UpdateNicknameLoginOnlyCommand(1, true, "EN", "::1");
            var expectedSettingKey = "Age_CustomerUpdNicknameLoginOnly";

            // Act
            var actualSettingKey = command.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }
    }
}
