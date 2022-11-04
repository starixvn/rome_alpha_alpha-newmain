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
            IsShowLetstalkChatBot = !userSession.IsSubAcc
                && (AppSettings.OfferChatbotSites.Contains("*")
                    || AppSettings.OfferChatbotSites.Contains(userSession.Site, StringComparer.OrdinalIgnoreCase));
        }

        public int CustId { get; set; }

        public UserLevel CustLevel { get; set; }

        public int SubAccId { get; set; }

        public string Language { get; set; }

        public bool IsShowLetstalkChatBot { get; set; }
    }
}