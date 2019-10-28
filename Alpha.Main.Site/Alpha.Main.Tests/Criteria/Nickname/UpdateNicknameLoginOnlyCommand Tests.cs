namespace Alpha.Main.Tests.Criteria.Nickname
{
    using Alpha.Main.Criteria.Nickname;
    using Alpha.Main.Params.Nickname;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateNicknameLoginOnlyCommand_Tests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var createNicknameParam = new CreateNicknameParam(1, 0, string.Empty, string.Empty, "en", false);
            var param = new UpdateNicknameLoginOnlyParam(createNicknameParam, false);
            var command = new UpdateNicknameLoginOnlyCommand(param);
            var expectedSettingKey = "Age_CustomerUpdNicknameLoginOnly";

            // Act
            var actualSettingKey = command.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }
    }
}
