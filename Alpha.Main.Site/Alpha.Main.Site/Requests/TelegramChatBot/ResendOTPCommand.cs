namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using Alpha.Main.Models;
    using Configurations;
    using MediatR;

    public class ResendOTPCommand : IAsyncRequest<Result>
    {
        public int IntervalThresholdToSendRequestInSeconds => AppSettings.IntervalThresholdToSendRequestInSeconds;
    }
}