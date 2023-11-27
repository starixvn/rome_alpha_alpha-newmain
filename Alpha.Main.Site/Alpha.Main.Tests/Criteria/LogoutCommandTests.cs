namespace Alpha.Main.Tests.Criteria
{
    using Main.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class LogoutCommandTests
    {
        private const int OnlineUserId = 1;
        private const string Username = "master1p1";
        private const byte CustRoleId = 4;
        private const int ServerId = 30001;
        private const string Nickname = "";
        private const string Reason = " Kicked out: Session Changed";

        [Test]
        public void GetSettingKey_Always_Return_Age_CheckOut()
        {
            // Arrange
            var criteria = new LogoutCommand(OnlineUserId, Username, CustRoleId, ServerId, Nickname, Reason);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_CheckOut", settingKey);
        }

        [Test]
        public void Constructor_Always_SetCorrectProperties()
        {
            // Arrange
            var criteria = new LogoutCommand(OnlineUserId, Username, CustRoleId, ServerId, Nickname, Reason);

            // Act & Assert
            Assert.True(
                criteria.OnlineUserId == 1
                && criteria.UserName == "master1p1"
                && criteria.RoleId == 4
                && criteria.ServerId == 30001
                && criteria.Nickname == string.Empty);
        }

        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(8, true)]
        public void IsValid_OnlineUserId_ReturnCorrectValue(int onUserId, bool expected)
        {
            // Arrange
            var criteria = new LogoutCommand(onUserId, Username, CustRoleId, ServerId, Nickname, Reason);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, false)]
        [TestCase("  ", false)]
        [TestCase("master1", true)]
        public void IsValid_Username_ReturnCorrectValue(string username, bool expected)
        {
            // Arrange
            var criteria = new LogoutCommand(OnlineUserId, username, CustRoleId, ServerId, Nickname, Reason);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, false)]
        [TestCase(3, true)]
        public void IsValid_RoleId_ReturnCorrectValue(byte roleId, bool expected)
        {
            // Arrange
            var criteria = new LogoutCommand(OnlineUserId, Username, roleId, ServerId, Nickname, Reason);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(100, true)]
        public void IsValid_ServerId_ReturnCorrectValue(int serverId, bool expected)
        {
            // Arrange
            var criteria = new LogoutCommand(OnlineUserId, Username, CustRoleId, serverId, Nickname, Reason);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
