namespace Alpha.Main.Tests.Criteria.Nickname
{
    using Alpha.Main.Criteria.Nickname;
    using NUnit.Framework;

    [TestFixture]
    public class CreateSubAccountNicknameCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var command = new CreateSubAccountNicknameCommand(1, 1, string.Empty, "en", "::1");
            var expectedSettingKey = "Age_CustSubInsNickname";

            // Act
            var actualSettingKey = command.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }
    }
}
