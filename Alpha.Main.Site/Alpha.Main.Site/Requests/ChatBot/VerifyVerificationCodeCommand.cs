namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class VerifyVerificationCodeCommand : WriteLogCommandBase, IAsyncRequest<VerificationCodeResult>
    {
        public VerifyVerificationCodeCommand()
            : this(null)
        {
        }

        internal VerifyVerificationCodeCommand(IUserSession userSession)
                : base(userSession)
        {
        }

        public string VerificationCode { get; set; }

        public bool IsShowCaptcha { get; set; }
    }
}