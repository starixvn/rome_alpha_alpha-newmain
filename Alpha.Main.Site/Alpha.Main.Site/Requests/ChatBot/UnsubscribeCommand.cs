namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;

    public class UnsubscribeCommand : IAsyncRequest<Result>
    {
        public int UserId { get; set; }

        public int MessengerType { get; set; }
    }
}