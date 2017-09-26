namespace Alpha.Main.Site.Tests.Requests.Otp
{
    using Alpha.Main.Site.Requests.Otp;
    using Alpha.Main.Site.Tests.Constants;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class SetUpOtpCommandTests
    {
        private IUserSession userSession;
        private SetUpOtpCommand command;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.SubAccId.Returns(0);
            userSession.LangKey.Returns("en");
            userSession.UserLoginIp.Returns("127.0.0.1");

            command = new SetUpOtpCommand(userSession);
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
        public void SubAccountId_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.SubAccId;

            // Act
            var actualValue = command.SubAccountId;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void Language_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.LangKey;

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
