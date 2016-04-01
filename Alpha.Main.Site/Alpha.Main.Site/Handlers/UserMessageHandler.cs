namespace Alpha.Main.Site.Handlers
{
    using System.Collections.Generic;
    using System.Web;
    using MediatR;
    using Models.Requests.UserMessages;
    using Services;

    public class UserMessageHandler :
        IRequestHandler<HotMessagesQuery, IEnumerable<string>>
    {
        private readonly UserMessageService userMessageService;

        public UserMessageHandler(IDashboardService dashboardService, UserMessageService userMessageServiceImpl)
        {
            userMessageService = userMessageServiceImpl;
        }

        public IEnumerable<string> Handle(HotMessagesQuery message)
        {
            var hotMessageFilePath = HttpContext.Current.Server.MapPath("~/App_Data/UserMessage/ToolTipMessage.xml");

            return userMessageService.GetHotMessages(hotMessageFilePath);
        }
    }
}