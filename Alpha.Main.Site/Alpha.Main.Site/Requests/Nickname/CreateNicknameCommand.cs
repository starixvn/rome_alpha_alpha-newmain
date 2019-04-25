namespace Alpha.Main.Site.Requests.Nickname
{
    using Main.Models;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Web.Helpers;

    public class CreateNicknameCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;
        private readonly NicknameChecker nicknameChecker;

        public CreateNicknameCommand(
            IUserSession userSession,
            NicknameChecker nicknameChecker = null)
        {
            this.userSession = userSession;
            this.nicknameChecker = nicknameChecker ?? new NicknameCheckerImpl(userSession: this.userSession);
        }

        public string Nickname { get; set; }

        public bool IsDisableUpdateNickname
            => !string.IsNullOrWhiteSpace(userSession.NickName);

        public int ActorId => userSession.CustId;

        public string IP => userSession.UserLoginIp;

        public string LanguageKey => userSession.LangKey;

        public bool IsValid()
            => nicknameChecker.IsOfferForceLoginFlow;
    }
}