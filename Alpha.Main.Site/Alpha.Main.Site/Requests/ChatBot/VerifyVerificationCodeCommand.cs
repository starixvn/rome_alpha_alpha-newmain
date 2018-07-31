namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using Constants;
    using MediatR;

    public class VerifyVerificationCodeCommand : IAsyncRequest<VerificationCodeResult>
    {
        public int UserId { get; set; }

        public string VerificationCode { get; set; }

        public string MessengerAccount { get; set; }

        public int MessengerType => MessengerTypeConstants.Letstalk;

        public bool IsShowCaptcha { get; set; }
    }
}