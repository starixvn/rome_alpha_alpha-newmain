namespace Alpha.Main.Tests.Criteria.Nickname
{
    using Alpha.Main.Criteria.Nickname;
    using Alpha.Main.Params.Nickname;
    using NUnit.Framework;

    [TestFixture]
    public class CreateNicknameCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var param = new CreateNicknameParam(1, 0, string.Empty, string.Empty, "en", false);
            var command = new CreateNicknameCommand(param);
            var expectedSettingKey = "Age_CustomerInsNickName";

            // Act
            var actualSettingKey = command.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }
    }
}
