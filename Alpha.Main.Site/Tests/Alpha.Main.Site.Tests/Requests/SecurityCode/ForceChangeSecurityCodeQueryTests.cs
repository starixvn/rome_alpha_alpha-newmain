using Alpha.Main.Site.Models.Request.SecurityCode;
using Alpha.Main.Site.Tests.Constants;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Main.Site.Tests.Requests.SecurityCode
{
    [TestFixture]
    public class ForceChangeSecurityCodeQueryTests
    {
        private IUserSession userSession;
        private ForceChangeSecurityCodeQuery query;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(CustId.ValidId);
            userSession.SubAccId.Returns(0);
            userSession.IsSubAcc.Returns(false);
            userSession.RoleId.Returns(RoleId.Agent);
            userSession.UserName.Returns("UserName");
            userSession.NickName.Returns("NickName");
            userSession.Language.Returns("en");
            userSession.SubUserName.Returns("SubUserName");

            query = new ForceChangeSecurityCodeQuery(userSession);
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

        [Test]
        public void UserName_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.UserName;

            // Act
            var actualValue = query.UserName;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void NickName_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.NickName;

            // Act
            var actualValue = query.NickName;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void Language_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.Language;

            // Act
            var actualValue = query.Language;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void SubUserName_Always_EqualsValueOfUserSession()
        {
            // Arrange
            var expectedValue = userSession.SubUserName;

            // Act
            var actualValue = query.SubUserName;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
