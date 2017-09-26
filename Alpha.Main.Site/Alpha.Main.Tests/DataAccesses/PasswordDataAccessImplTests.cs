namespace Alpha.Main.Tests.DataAccesses
{
    using System;
    using Alpha.Main.DataAccesses.Impl;
    using Fanex.Data.Repository;
    using NUnit.Framework;

    [TestFixture]
    public class PasswordDataAccessImplTests
    {
        private PasswordDataAccessImpl passwordDataAccess;

        [SetUp]
        public void SetUp()
        {
            passwordDataAccess = new PasswordDataAccessImpl();
        }

        [Test]
        public void UpdatePassword_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => passwordDataAccess.UpdatePassword(criteria));
        }
    }
}
