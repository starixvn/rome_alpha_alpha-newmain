namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using AlphaPlus.User.Models;
    using MediatR;
    using TelegramBot.Client.Models;

    public class VerifyOTPCommand : IAsyncRequest<Result>
    {
        public VerifyOTPCommand() : this(null)
        {
        }

        public VerifyOTPCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            Username = userSession.UserName;
            SubUserName = userSession.SubUserName;
            IsSubAcc = userSession.IsSubAcc;
            IP = userSession.UserLoginIp;
        }

        public string OTP { get; set; }

        public bool IsShowCaptcha { get; set; }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Username { get; }

        public string SubUserName { get; }

        public bool IsSubAcc { get; }

        public string IP { get; }
    }
}