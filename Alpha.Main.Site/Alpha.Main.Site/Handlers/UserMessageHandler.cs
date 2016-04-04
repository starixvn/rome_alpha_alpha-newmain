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
        /// <summary>
        /// Do not change this session name, because it is used by old site
        /// </summary>
        private const string IsShowedMessageSessionKey = "messageisshowed";
        private readonly UserMessageService userMessageService;
        private IUserSession userSession;

        private bool IsShowedHotMessages
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
            UserMessageService userMessageService,
            IUserSession userSession)
        {
            this.userMessageService = userMessageService;
            this.userSession = userSession;
        }

        public IEnumerable<string> Handle(HotMessagesQuery message)
        {
            if (IsShowedHotMessages)
            {
                return Enumerable.Empty<string>();
            }

            message.Language = userSession.Language;
            message.RoleId = userSession.RoleId;

            var hotMessages = userMessageService.GetHotMessages(message);

            IsShowedHotMessages = true;

            return hotMessages;
        }
    }
}