namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Models.Requests.UserMessages;
    using Services;

    public class UserMessageHandler :
        IRequestHandler<HotMessagesQuery, IEnumerable<string>>
    {
        private readonly UserMessageService userMessageService;

        public UserMessageHandler(UserMessageService userMessageService)
        {
            this.userMessageService = userMessageService;
        }

        public IEnumerable<string> Handle(HotMessagesQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var hotMessages = userMessageService.GetHotMessages(message.HotMessageFilePath, message.RoleId, message.Language);

            return hotMessages;
        }
    }
}