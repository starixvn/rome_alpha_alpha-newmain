namespace Alpha.Main.Tests.DataAccesses
{
    using System;
    using Alpha.Main.Constants;
    using Alpha.Main.DataAccesses.Impl;
    using Fanex.Data.Repository;
    using NUnit.Framework;

    [TestFixture]
    public class MessageDataAccessImplTests
    {
        private MessageDataAccessImpl messageDataAccess;

        [SetUp]
        public void SetUp()
        {
            messageDataAccess = new MessageDataAccessImpl();
        }

        [Test]
        public void GetMessages_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            ICriteria criteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => messageDataAccess.GetMessages(criteria, MessageType.Private));
        }
    }
}
