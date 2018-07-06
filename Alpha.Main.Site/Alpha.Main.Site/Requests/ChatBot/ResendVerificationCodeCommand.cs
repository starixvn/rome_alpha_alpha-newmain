namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;

    public class ResendVerificationCodeCommand : IAsyncRequest<Result>
    {
        public int UserId { get; set; }

        public string MessengerAccount { get; set; }

        public int MessengerType { get; set; }
    }
}