using Alpha.Main.Site.Models.Request.SecurityCode;
using Alpha.Main.Site.Tests.Constants;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Main.Site.Tests.Requests.SecurityCode
{
    [TestFixture]
    public class ChangeSecurityCodeByForcingCommandTests
    {
        private IUserSession userSession;
        private ChangeSecurityCodeByForcingCommand command;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.UserId.Returns(2);
            userSession.SubAccId.Returns(0);
            userSession.IsSubAcc.Returns(false);
            userSession.SecurityCodeToken.Returns("SecurityCodeToken");
            userSession.Language.Returns("en");
            userSession.UserLoginIp.Returns("127.0.0.1");

            command = new ChangeSecurityCodeByForcingCommand(userSession);
        }

        [Test]
        public void CustId_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.CustId;

            // Act
            var actualValue = command.CustId;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
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
        public void SubAccId_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.SubAccId;

            // Act
            var actualValue = command.SubAccId;

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
        public void SecurityCodeToken_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.SecurityCodeToken;

            // Act
            var actualValue = command.SecurityCodeToken;

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
