namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;

    public class UnsubscribeCommand : IAsyncRequest<Result>
    {
    }
}