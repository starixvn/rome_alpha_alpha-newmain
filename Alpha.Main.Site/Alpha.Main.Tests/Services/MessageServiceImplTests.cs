namespace Alpha.Main.Tests.Services
{
    using System;
    using System.Linq;
    using Constants;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Main.Models.Message;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using TestData.Helper;
    using Assert = NUnit.Framework.Assert;

    [TestFixture]
    public class MessageServiceImplTests
    {
        private MessageDataAccess messageDataAccess;
        private MessageServiceImpl messageService;

        [SetUp]
        public void SetUp()
        {
            messageDataAccess = Substitute.For<MessageDataAccess>();

            messageService = new MessageServiceImpl(messageDataAccess);
        }

        [Test]
        public void GetMessages_CustIdIsInvalidAndParentSitesIsNullOrEmpty_ReturnEmpty()
        {
            // Arrange
            var messages = messageService.GetMessages(
                CustId.InvalidId,
                string.Empty,
                1,
                1,
                DateTime.Today,
                DateTime.Today,
                false,
                1);

            // Act & Assert
            Assert.AreEqual(messages, null);
        }

        [Test]
        public void GetMessages_CustIdIsValidAndParentSitesIsNotNullorEmpty_ReturnMessages()
        {
            // Arrange
            var messages = FakeDataHelper.CreateMany<Message>().ToList();

            messageDataAccess.GetMessages(Arg.Any<ICriteria>(), Arg.Any<int>()).Returns(messages);

            // Act
            var actualMessages = messageService.GetMessages(
                CustId.ValidId,
                "1",
                1,
                1,
                DateTime.Today,
                DateTime.Today,
                false,
                1);

            // Assert
            Assert.AreEqual(messages, actualMessages);
        }
    }
}