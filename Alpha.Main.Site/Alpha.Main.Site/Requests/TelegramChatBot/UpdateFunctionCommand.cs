namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using AlphaPlus.User.Models;
    using MediatR;
    using TelegramBot.Client.Models;

    public class UpdateFunctionCommand : IAsyncRequest<Result>
    {
        public UpdateFunctionCommand() : this(null)
        {
        }

        public UpdateFunctionCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            Language = userSession.Language;
            IP = userSession.UserLoginIp;
        }

        public short FunctionId { get; set; }

        public string FunctionName { get; set; }

        public byte Type { get; set; }

        public bool Enabled { get; set; }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Language { get; }

        public string IP { get; }
    }
}