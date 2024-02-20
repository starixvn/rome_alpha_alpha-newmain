namespace Alpha.Main.Tests.Criteria.Nickname
{
    using Alpha.Main.Criteria.Nickname;
    using NUnit.Framework;

    [TestFixture]
    public class CreateNicknameCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var command = new CreateNicknameCommand(1, string.Empty, "EN", "::1");
            var expectedSettingKey = "Age_CustomerInsNickName";

            // Act
            var actualSettingKey = command.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }
    }
}
