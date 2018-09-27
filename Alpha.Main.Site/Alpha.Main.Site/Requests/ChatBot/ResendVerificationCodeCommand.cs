namespace Alpha.Main.Site.Requests.ChatBot
{
    using Chatbot.Client.Models;
    using Configurations;
    using MediatR;

    public class ResendVerificationCodeCommand : IAsyncRequest<Result>
    {
        public int IntervalThresholdToSendRequestInSeconds => AppSettings.IntervalThresholdToSendRequestInSeconds;
    }
}