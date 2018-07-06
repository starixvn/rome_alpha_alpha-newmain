namespace Alpha.Main.Site.Requests.ChatBot
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.ChatBot;

    public class SetUpChatBotQuery : IAsyncRequest<ChatBotViewModel>
    {
        private readonly IUserSession userSession;

        public SetUpChatBotQuery()
            : this(null)
        {
        }

        internal SetUpChatBotQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public int SubAccountId => userSession.SubAccId;

        public string Language => userSession.Language;
    }
}