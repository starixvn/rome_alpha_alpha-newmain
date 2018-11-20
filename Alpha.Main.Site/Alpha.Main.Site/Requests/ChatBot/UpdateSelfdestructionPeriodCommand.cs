namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class UpdateSelfdestructionPeriodCommand : WriteLogCommandBase, IAsyncRequest<Result>
    {
        public UpdateSelfdestructionPeriodCommand()
            : this(null)
        {
        }

        internal UpdateSelfdestructionPeriodCommand(IUserSession userSession)
            : base(userSession)
        {
        }

        public int Minutes { get; set; }
    }
}