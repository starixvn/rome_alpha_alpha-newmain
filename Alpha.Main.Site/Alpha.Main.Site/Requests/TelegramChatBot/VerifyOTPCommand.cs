namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using Alpha.Main.Models;
    using Alpha.Main.Site.Requests.ChatBot;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class VerifyOTPCommand : WriteLogCommandBase, IAsyncRequest<Result>
    {
        public VerifyOTPCommand()
           : this(null)
        {
        }

        internal VerifyOTPCommand(IUserSession userSession)
                : base(userSession)
        {
        }

        public string OTP { get; set; }

        public bool IsShowCaptcha { get; set; }
    }
}