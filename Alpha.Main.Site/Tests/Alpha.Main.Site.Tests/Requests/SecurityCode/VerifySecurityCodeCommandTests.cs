using Alpha.Main.Site.Models.Request.SecurityCode;
using Alpha.Main.Site.Tests.Constants;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Main.Site.Tests.Requests.SecurityCode
{
    [TestFixture]
    public class VerifySecurityCodeCommandTests
    {
        private IUserSession userSession;
        private VerifySecurityCodeCommand command;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.SubAccId.Returns(0);
            userSession.SecurityCodeToken.Returns("SecurityCodeToken");
            userSession.Language.Returns("en");
            userSession.UserLoginIp.Returns("127.0.0.1");

            command = new VerifySecurityCodeCommand(userSession);
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
