namespace Alpha.Main.Site.Requests.Nickname
{
    using Alpha.Web.Helpers;
    using AlphaPlus.User.Models;
    using Main.Models;
    using MediatR;

    public class CreateNicknameCommand : IAsyncRequest<Result>
    {
        private readonly IUserSession userSession;
        private readonly NicknameChecker nicknameChecker;

        public CreateNicknameCommand(
            IUserSession userSession,
            NicknameChecker nicknameChecker = null)
        {
            this.userSession = userSession;
            this.nicknameChecker = nicknameChecker ?? new NicknameCheckerImpl(this.userSession);
        }

        public string Nickname { get; set; }

        public bool IsDisableUpdateNickname
            => !string.IsNullOrWhiteSpace(userSession.NickName);

        public int ActorId => userSession.CustId;

        public int SubAccountId => userSession.SubAccId;

        public string IP => userSession.UserLoginIp;

        public string Language => userSession.Language;

        public string Site => userSession.Site;

        public int CustDBGroupId => userSession.CustDBGroupId;

        public bool IsValid()
            => nicknameChecker.IsOfferForceLoginFlow;
    }
}