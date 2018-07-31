namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using Constants;
    using MediatR;

    public class UnsubscribeCommand : IAsyncRequest<Result>
    {
        public int UserId { get; set; }

        public int MessengerType => MessengerTypeConstants.Letstalk;

        public string MessengerUserName { get; set; }
    }
}