namespace Alpha.Main.Tests.Criteria.Nickname
{
    using Alpha.Main.Criteria.Nickname;
    using NUnit.Framework;

    [TestFixture]
    public class UpdateSubAccountNicknameLoginOnlyCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var command = new UpdateSubAccountNicknameLoginOnlyCommand(1, 1, true, "EN", "::1");
            var expectedSettingKey = "Age_CustSubUpdNicknameLoginOnly";

            // Act
            var actualSettingKey = command.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }
    }
}
