namespace Alpha.Main.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fanex.Data.Repository;
    using Main.Models.Message;
    using Main.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;
    using TestData.Helper;

    [TestFixture]
    public class MessageServiceImplTests
    {
        private IDynamicRepository dynamicRepository;

        private MessageServiceImpl messageService;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();

            messageService = new MessageServiceImpl(dynamicRepository);
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
            Assert.AreEqual(messages, new List<Message>());
        }

        [Test]
        public void GetMessages_CustIdIsValidAndParentSitesIsNotNullorEmpty_ReturnMessages()
        {
            // Arrange
            var messages = FakeDataHelper.CreateMany<Message>();

            dynamicRepository
                .Fetch<Message>(Arg.Any<object>())
                .Returns(messages);

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