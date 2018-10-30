namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class UpdateFunctionCommand : WriteLogCommandBase, IAsyncRequest<Result>
    {
        public UpdateFunctionCommand()
            : this(null)
        {
        }

        internal UpdateFunctionCommand(IUserSession userSession)
            : base(userSession)
        {
        }

        public int FunctionId { get; set; }

        public string FunctionName { get; set; }

        public string Command { get; set; }

        public bool IsEnable { get; set; }
    }
}