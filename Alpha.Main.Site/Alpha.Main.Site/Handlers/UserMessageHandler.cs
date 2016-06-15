namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Models.Requests.UserMessages;
    using Services;
    using Services.Impl;

    public class UserMessageHandler :
        IRequestHandler<HotMessagesQuery, IEnumerable<string>>
    {
        /// <summary>
        /// Do not change this session name, because it is used by old site
        /// </summary>
        private const string IsShowedHotMessageSessionKey = "IsShowedHotMessageSessionKey";

        private readonly UserMessageService userMessageService;
        private readonly SessionManager sessionManager;

        public UserMessageHandler(UserMessageService userMessageService, SessionManagerImpl sessionManager)
        {
            this.userMessageService = userMessageService;
            this.sessionManager = sessionManager;
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