#pragma warning disable 612, 618

namespace Alpha.Main.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Main.Models.Message;
    using Main.Tests.TestData.Helper;
    using NSubstitute;
    using NUnit.Framework;
    using Services;
    using Site.Handlers;
    using Site.Requests.Message;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class MessageHanlderTests
    {
        private MessageService messageService;
        private MessageHandler messageHandler;
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            messageService = Substitute.For<MessageService>();
            messageHandler = new MessageHandler(messageService);

            userSession = Substitute.For<IUserSession>();
            userSession.Language.Returns(Language.English);
            userSession.CustId.Returns(CustId.ValidId);
            userSession.RoleId.Returns(RoleId.Agent);
        }

        [Test]
        public void HandleMessageQuery_NullQuery_ThrowArgumentNullException()
        {
            // Arrange
            MessageQuery nullQuery = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => messageHandler.Handle(nullQuery));
        }

        [Test]
        public void HandleMessageQuery_NotNullQuery_CallGetMessageService()
        {
            // Arrange
            var messages = FakeDataHelper.CreateMany<Message>().ToList();
            FakeReturnGetMessagesService(messages);

            var query = new MessageQuery(
                userSession,
                1,
                DateTime.Today,
                DateTime.Today,
                false);

            // Act
            messageHandler.Handle(query);

            // Assert
            messageService
                .Received()
                .GetMessages(
                    Arg.Is<int>(cud => cud == query.CustId),
                    Arg.Is<string>(ps => ps == query.ParentSites),
                    Arg.Is<int>(st => st == query.SpecialType),
                    Arg.Is<int>(tm => tm == query.TotalMessage),
                    Arg.Is<DateTime>(st => st == query.StartDate),
                    Arg.Is<DateTime>(ed => ed == query.EndDate),
                    Arg.Is<bool>(it => it == query.IsTest),
                    Arg.Is<int>(mt => mt == query.MessageType));
        }

        [Test]
        public void HandleMessageQuery_NotNullQuery_ReturnMessagesViewModel()
        {
            // Arrange
            var messages = FakeDataHelper.CreateMany<Message>().ToList();
            FakeReturnGetMessagesService(messages);

            var query = new MessageQuery(
                    userSession,
                    Arg.Any<int>(),
                    Arg.Any<DateTime>(),
                    Arg.Any<DateTime>(),
                    Arg.Any<bool>(),
                    Arg.Any<int>());

            // Act
            var viewModel = messageHandler.Handle(query);

            // Assert
            Assert.AreEqual(messages.Count, viewModel.Messages.Count);
        }

        private void FakeReturnGetMessagesService(List<Message> messages)
        {
            messageService.GetMessages(
                        Arg.Any<int>(),
                        Arg.Any<string>(),
                        Arg.Any<int>(),
                        Arg.Any<int>(),
                        Arg.Any<DateTime>(),
                        Arg.Any<DateTime>(),
                        Arg.Any<bool>(),
                        Arg.Any<int>())
                  .Returns(messages);

            messageService.GetMessagesFromXml(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<byte>())
                .Returns(messages);
        }
    }
}