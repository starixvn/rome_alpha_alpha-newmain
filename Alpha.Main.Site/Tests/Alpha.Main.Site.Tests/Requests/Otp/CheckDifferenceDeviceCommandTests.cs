namespace Alpha.Main.Site.Tests.Requests.Otp
{
    using Alpha.Main.Site.Requests.Otp;
    using Alpha.Main.Site.Tests.Constants;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class CheckDifferenceDeviceCommandTests
    {
        private IUserSession userSession;
        private CheckDifferenceDeviceCommand command;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.SubAccId.Returns(0);

            command = new CheckDifferenceDeviceCommand(userSession);
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
    }
}
