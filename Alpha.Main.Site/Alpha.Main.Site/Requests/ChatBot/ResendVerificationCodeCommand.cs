namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class ResendVerificationCodeCommand : IAsyncRequest<Result>
    {
        private readonly IUserSession userSession;

        public ResendVerificationCodeCommand()
            : this(null)
        {
        }

        internal ResendVerificationCodeCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public string CustName => userSession.UserName;

        public int UserId { get; set; }

        public string MessengerUsername { get; set; }
    }
}