namespace Alpha.Main.Site.Tests.Requests.Otp
{
    using Alpha.Main.Site.Requests.Otp;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class ResetOtpCommandTests
    {
        private IUserSession userSession;
        private ResetOtpCommand command;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.UserId.Returns(1);
            userSession.LangKey.Returns("en");

            command = new ResetOtpCommand(userSession);
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
        public void ActorId_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.UserId;

            // Act
            var actualValue = command.ActorId;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
