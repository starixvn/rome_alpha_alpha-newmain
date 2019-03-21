namespace Alpha.Main.Tests.Criteria
{
    using Main.Criteria;
    using Main.Criteria.Statistics;
    using NUnit.Framework;
    using Params;

    [TestFixture]
    public class LogoutCommandTests
    {
        private const int OnlineUserId = 1;
        private const string Username = "master1p1";
        private const string SubUserName = "";
        private const byte CustRoleId = 4;
        private const int ServerId = 30001;
        private const string Nickname = "";

        [Test]
        public void GetSettingKey_Always_Return_Age_CheckOut()
        {
            // Arrange
            var criteria = new LogoutCommand(OnlineUserId, Username, CustRoleId, ServerId, Nickname);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_CheckOut", settingKey);
        }

        [Test]
        public void GetSettingKey_LogoutParam_Return_Age_CheckOut()
        {
            // Arrange
            var criteria = new LogoutCommand(
                new LogoutParam(OnlineUserId, Username, SubUserName, Nickname, CustRoleId, ServerId));

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Age_CheckOut", settingKey);
        }

        [Test]
        public void Constructor_Always_SetCorrectProperties()
        {
            // Arrange
            var criteria = new LogoutCommand(OnlineUserId, Username, CustRoleId, ServerId, Nickname);

            // Act & Assert
            Assert.True(
                criteria.OnlineUserId == 1
                && criteria.UserName == "master1p1"
                && criteria.RoleId == 4
                && criteria.ServerId == 30001
                && criteria.Nickname == string.Empty);
        }

        [Test]
        public void Constructor_LogoutParam_SetCorrectProperties()
        {
            // Arrange
            var criteria = new LogoutCommand(
                new LogoutParam(OnlineUserId, Username, SubUserName, Nickname, CustRoleId, ServerId));

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
            var criteria = new LogoutCommand(
                new LogoutParam(onUserId, Username, SubUserName, Nickname, CustRoleId, ServerId));

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
            var criteria = new LogoutCommand(
                new LogoutParam(OnlineUserId, username, SubUserName, Nickname, CustRoleId, ServerId));

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
            var criteria = new LogoutCommand(
                new LogoutParam(OnlineUserId, Username, SubUserName, Nickname, roleId, ServerId));

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
            var criteria = new LogoutCommand(
                new LogoutParam(OnlineUserId, Username, SubUserName, Nickname, CustRoleId, serverId));

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
