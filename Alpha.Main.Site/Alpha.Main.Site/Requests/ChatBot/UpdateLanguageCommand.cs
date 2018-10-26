namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class UpdateLanguageCommand : WriteLogCommandBase, IAsyncRequest<Result>
    {
        public UpdateLanguageCommand()
            : this(null)
        {
        }

        internal UpdateLanguageCommand(IUserSession userSession)
            : base(userSession)
        {
        }

        public string Language { get; set; }
    }
}