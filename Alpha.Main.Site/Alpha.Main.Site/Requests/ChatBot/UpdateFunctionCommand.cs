namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class UpdateFunctionCommand : IAsyncRequest<Result>
    {
        private readonly IUserSession userSession;

        public UpdateFunctionCommand()
            : this(null)
        {
        }

        internal UpdateFunctionCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public string IP => userSession.UserLoginIp;

        public int FunctionId { get; set; }

        public string FunctionName { get; set; }

        public string Command { get; set; }

        public bool IsEnable { get; set; }
    }
}