namespace Alpha.Main.Tests.DataAccesses
{
    using System;
    using Alpha.Main.DataAccesses.Impl;
    using Fanex.Data.Repository;
    using NUnit.Framework;

    [TestFixture]
    public class SecurityCodeDataAccessImplTests
    {
        private SecurityCodeDataAccessImpl securityCodeDataAccess;

        [SetUp]
        public void SetUp()
        {
            securityCodeDataAccess = new SecurityCodeDataAccessImpl();
        }

        [Test]
        public void GetSecurityCode_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeDataAccess.GetSecurityCode(criteria));
        }

        [Test]
        public void UpdateSecurityCode_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => securityCodeDataAccess.UpdateSecurityCode(criteria));
        }
    }
}
