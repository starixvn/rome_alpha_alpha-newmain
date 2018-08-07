namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class AddAccountCommand : IAsyncRequest<AddingUserResult>
    {
        private readonly IUserSession userSession;

        public AddAccountCommand()
            : this(null)
        {
        }

        internal AddAccountCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public int SubAccountId => userSession.SubAccId;

        public string CustName => userSession.UserName;

        public string Language => userSession.Language;

        public string MessengerAccount { get; set; }
    }
}