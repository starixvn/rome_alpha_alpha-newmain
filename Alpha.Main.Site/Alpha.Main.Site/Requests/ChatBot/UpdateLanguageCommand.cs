namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class UpdateLanguageCommand : IAsyncRequest<Result>
    {
        private readonly IUserSession userSession;

        public UpdateLanguageCommand()
            : this(null)
        {
        }

        internal UpdateLanguageCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public string IP => userSession.UserLoginIp;

        public string Language { get; set; }
    }
}