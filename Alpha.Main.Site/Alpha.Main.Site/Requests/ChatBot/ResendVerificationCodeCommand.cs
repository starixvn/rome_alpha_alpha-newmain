namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using Configurations;
    using MediatR;

    public class ResendVerificationCodeCommand : IAsyncRequest<Result>
    {
        public string MessengerUsername { get; set; }

        public int IntervalThresholdToSendRequestInSeconds => AppSettings.IntervalThresholdToSendRequestInSeconds;
    }
}