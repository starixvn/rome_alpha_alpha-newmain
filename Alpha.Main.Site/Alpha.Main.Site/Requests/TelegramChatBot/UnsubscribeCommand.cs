namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using AlphaPlus.User.Models;
    using MediatR;
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
            IP = userSession.UserLoginIp;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string IP { get; }
    }
}