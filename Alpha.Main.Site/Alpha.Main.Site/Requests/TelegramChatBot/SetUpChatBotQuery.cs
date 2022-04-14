namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using Alpha.Web.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class SetUpChatBotQuery : IAsyncRequest<ChatBotViewModel>
    {
        public SetUpChatBotQuery() : this(null)
        {
        }

        public SetUpChatBotQuery(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            CustLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            SubAccId = userSession.SubAccId;
            Language = userSession.Language;
            IsViewReports = userSession.SubPageAccess.ViewReports;
            IsViewMemberInfo = userSession.SubPageAccess.ViewMemberInfo;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public int SubAccId { get; }

        public string Language { get; }

        public bool IsViewReports { get; }

        public bool IsViewMemberInfo { get; }
    }
}