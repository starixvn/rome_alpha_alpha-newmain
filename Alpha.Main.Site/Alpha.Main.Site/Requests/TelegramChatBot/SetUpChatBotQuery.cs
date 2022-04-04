namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
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
            SubAccId = userSession.SubAccId;
            Language = userSession.Language;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Language { get; }
    }
}