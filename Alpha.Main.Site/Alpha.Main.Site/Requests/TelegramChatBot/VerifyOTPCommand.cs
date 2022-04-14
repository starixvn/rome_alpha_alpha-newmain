namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using TelegramBot.Client.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Base;

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
            IP = userSession.UserLoginIp;
        }

        public string OTP { get; set; }

        public bool IsShowCaptcha { get; set; }

        public int CustId { get; }

        public int SubAccId { get; }

        public string IP { get; }
    }
}