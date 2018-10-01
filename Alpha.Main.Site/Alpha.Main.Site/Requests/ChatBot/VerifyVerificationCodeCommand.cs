namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;

    public class VerifyVerificationCodeCommand : IAsyncRequest<VerificationCodeResult>
    {
        public string VerificationCode { get; set; }

        public bool IsShowCaptcha { get; set; }
    }
}