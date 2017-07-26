using Alpha.Main.DataAccesses.Impl;
using Fanex.Data.Repository;
using NUnit.Framework;
using System;

namespace Alpha.Main.Tests.DataAccesses
{
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
