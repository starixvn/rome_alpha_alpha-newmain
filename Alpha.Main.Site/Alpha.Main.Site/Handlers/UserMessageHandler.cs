namespace Alpha.Main.Site.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.Common;
    using MediatR;
    using Models.Requests.UserMessages;
    using Services;
    using Sunplus.Agent.Model.Entities;

    public class UserMessageHandler :
        IRequestHandler<HotMessagesQuery, IEnumerable<string>>
    {
        private const string IsShowedMessageSessionKey = "messageisshowed";
        private readonly UserMessageService userMessageService;
        private IUserSession _userSession;

        private bool IsShowedMessage
        {
            get
            {
                return Converter.ToString2(HttpContext.Current.Session[IsShowedMessageSessionKey]).Equals("1");
            }
            set
            {
                HttpContext.Current.Session[IsShowedMessageSessionKey] = value ? 1 : 0;
            }
        }

        public UserMessageHandler(
            UserMessageService userMessageServiceImpl,
            IUserSession userSession)
        {
            userMessageService = userMessageServiceImpl;
            _userSession = userSession;
        }

        public IEnumerable<string> Handle(HotMessagesQuery message)
        {
            if (IsShowedMessage)
            {
                return Enumerable.Empty<string>();
            }

            message.Language = _userSession.Language;
            message.RoleId = _userSession.RoleId;

            var userMessagViewModel = userMessageService.GetHotMessages(message);

            IsShowedMessage = true;

            return userMessagViewModel;
        }
    }
}