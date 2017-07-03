using Alpha.Main.Site.Requests.Otp;
using Alpha.Main.Site.Tests.Constants;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Main.Site.Tests.Requests.Otp
{
    public class SetUpOtpQueryTests
    {
        private IUserSession userSession;
        private SetUpOtpQuery query;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.SubAccId.Returns(0);

            query = new SetUpOtpQuery(userSession);
        }

        [Test]
        public void CustId_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.CustId;

            // Act
            var actualValue = query.CustId;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void SubAccountId_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.SubAccId;

            // Act
            var actualValue = query.SubAccountId;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
