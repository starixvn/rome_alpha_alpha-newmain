namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using Configurations;
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using TelegramBot.Client.Models;

    public class ResendOTPCommand : IAsyncRequest<Result>
    {
        public ResendOTPCommand() : this(null)
        {
        }

        public ResendOTPCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }
    }
}