namespace Alpha.Main.Site.Tests.Requests.SecurityCode
{
    using Alpha.Main.Site.Models.Request.SecurityCode;
    using AlphaPlus.User.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ResetSecurityCodeCommandTests
    {
        private IUserSession userSession;
        private ResetSecurityCodeCommand command;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.UserId.Returns(2);
            userSession.IsSubAcc.Returns(false);
            userSession.Language.Returns("en");
            userSession.UserLoginIp.Returns("127.0.0.1");

            command = new ResetSecurityCodeCommand(userSession);
        }

        [Test]
        public void UserId_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.UserId;

            // Act
            var actualValue = command.UserId;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void IsSubAcc_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.IsSubAcc;

            // Act
            var actualValue = command.IsSubAcc;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void Language_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.Language;

            // Act
            var actualValue = command.Language;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void IP_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.UserLoginIp;

            // Act
            var actualValue = command.IP;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}