namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using MediatR;

    public class UpdateFunctionCommand : IAsyncRequest<Result>
    {
        public int UserId { get; set; }

        public int FunctionId { get; set; }

        public string Command { get; set; }

        public bool IsEnable { get; set; }
    }
}