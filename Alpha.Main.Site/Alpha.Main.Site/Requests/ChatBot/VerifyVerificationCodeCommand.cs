namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class VerifyVerificationCodeCommand : IAsyncRequest<VerificationCodeResult>
    {
        private readonly IUserSession userSession;

        public VerifyVerificationCodeCommand()
            : this(null)
        {
        }

        internal VerifyVerificationCodeCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public string IP => userSession.UserLoginIp;

        public string VerificationCode { get; set; }

        public bool IsShowCaptcha { get; set; }
    }
}