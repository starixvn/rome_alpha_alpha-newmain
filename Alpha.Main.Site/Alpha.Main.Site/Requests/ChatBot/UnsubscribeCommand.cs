namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class UnsubscribeCommand : IAsyncRequest<Result>
    {
        private readonly IUserSession userSession;

        public UnsubscribeCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public string IP => userSession.UserLoginIp;
    }
}