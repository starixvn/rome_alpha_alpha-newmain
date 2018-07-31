namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;

    public class UpdateLanguageCommand : IAsyncRequest<Result>
    {
        public int UserId { get; set; }

        public string Language { get; set; }
    }
}