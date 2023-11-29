namespace Alpha.Main.Site.Requests.Otp
{
    using AlphaPlus.User.Models;
    using MediatR;
    using ViewModels.Security;

    public class ResetOtpCommand : IAsyncRequest<ResetOtpViewModel>
    {
        private readonly IUserSession userSession;

        public ResetOtpCommand() : this(null)
        {
        }

        public ResetOtpCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public string Username { get; set; }

        public int ActorId
        {
            get
            {
                return this.userSession.UserId;
            }
        }

        public string Language
        {
            get
            {
                return this.userSession.LangKey;
            }
        }
    }
}