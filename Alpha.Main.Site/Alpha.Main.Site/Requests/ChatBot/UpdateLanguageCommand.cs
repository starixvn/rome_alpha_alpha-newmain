namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;

    public class UpdateLanguageCommand : IAsyncRequest<Result>
    {
        public string Language { get; set; }
    }
}