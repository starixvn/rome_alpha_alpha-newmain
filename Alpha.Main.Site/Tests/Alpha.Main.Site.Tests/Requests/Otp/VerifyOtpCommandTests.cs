namespace Alpha.Main.Site.Tests.Requests.Otp
{
    using Alpha.Main.Site.Requests.Otp;
    using Alpha.Main.Site.Tests.Constants;
    using AlphaPlus.User.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class VerifyOtpCommandTests
    {
        private IUserSession userSession;
        private VerifyOtpCommand command;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.SubAccId.Returns(0);
            userSession.Language.Returns("en");

            command = new VerifyOtpCommand(userSession);
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
            var expectedValue = userSession.Language;

            // Act
            var actualValue = command.Language;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}