namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class UnsubscribeCommand : WriteLogCommandBase, IAsyncRequest<Result>
    {
        public UnsubscribeCommand(IUserSession userSession)
            : base(userSession)
        {
        }
    }
}