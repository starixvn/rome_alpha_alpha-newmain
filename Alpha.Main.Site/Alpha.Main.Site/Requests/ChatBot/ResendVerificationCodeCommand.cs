namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;

    public class ResendVerificationCodeCommand : IAsyncRequest<Result>
    {
        public int UserId { get; set; }

        public string MessengerUsername { get; set; }
    }
}