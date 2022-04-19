namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using Alpha.Main.Configurations;
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using Alpha.Web.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Linq;

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
            IsShowLetstalkChatBot = !userSession.IsSubAcc
                && (AppSettings.OfferChatbotSites.Contains("*")
                    || AppSettings.OfferChatbotSites.Contains(userSession.Site, StringComparer.OrdinalIgnoreCase));
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public int SubAccId { get; }

        public string Language { get; }

        public bool IsViewReports { get; }

        public bool IsViewMemberInfo { get; }

        public bool IsShowLetstalkChatBot { get; }
    }
}