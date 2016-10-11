namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MediatR;
    using Requests.Message;
    using Services;
    using ViewModels.Message;

    public class MessageHandler : IRequestHandler<MessageQuery, MessagesViewModel>
    {
        private readonly MessageService messageService;

        public MessageHandler(MessageService messageService)
        {
            this.messageService = messageService;
        }

        public MessagesViewModel Handle(MessageQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var messages = messageService.GetMessages(
                                message.CustId,
                                message.ParentSites,
                                message.SpecialType,
                                message.TotalMessage,
                                message.StartDate,
                                message.EndDate,
                                message.IsTest,
                                message.MessageType);

            var messageWrappers = new List<MessageWrapper>(messages.Select(m => new MessageWrapper(m, message.CurrentLang)));

            var viewModel = new MessagesViewModel(messageWrappers, message.StartDate, message.EndDate, message.MessageType);

            return viewModel;
        }
    }
}