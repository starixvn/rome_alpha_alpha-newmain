using Alpha.Main.Site.Models.Request.SecurityCode;
using Alpha.Main.Site.Tests.Constants;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Main.Site.Tests.Requests.SecurityCode
{
    [TestFixture]
    public class SecurityCodeQueryTests
    {
        private IUserSession userSession;
        private SecurityCodeQuery query;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.IsSubAcc.Returns(false);
            userSession.RoleId.Returns(RoleId.Agent);

            query = new SecurityCodeQuery(userSession);
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
        public void SubAccId_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.SubAccId;

            // Act
            var actualValue = query.SubAccId;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void IsSubAcc_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.IsSubAcc;

            // Act
            var actualValue = query.IsSubAcc;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void RoleId_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.RoleId;

            // Act
            var actualValue = query.RoleId;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
