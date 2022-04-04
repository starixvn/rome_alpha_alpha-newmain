namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using TelegramBot.Client.Models;

    public class UnsubscribeCommand : IAsyncRequest<Result>
    {
        public UnsubscribeCommand() : this(null)
        {
        }

        public UnsubscribeCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

    }
}