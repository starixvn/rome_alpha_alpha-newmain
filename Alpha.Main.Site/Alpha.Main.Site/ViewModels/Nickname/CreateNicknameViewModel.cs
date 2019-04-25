namespace Alpha.Main.Site.ViewModels.Nickname
{
    using AppResources;
    using Helpers;
    using Main.Models;
    using Sunplus.Agent.Model.Entities;
    using Web.Helpers;

    public class CreateNicknameViewModel
    {
        private readonly IUserSession userSession;
        private readonly NicknameChecker nicknameChecker;

        public CreateNicknameViewModel(
            IUserSession userSession,
            string nickname = "",
            NicknameChecker nicknameChecker = null)
        {
            this.userSession = userSession;
            Nickname = string.IsNullOrWhiteSpace(this.userSession.NickName) ? nickname : this.userSession.NickName;
            this.nicknameChecker = nicknameChecker ?? new NicknameCheckerImpl(userSession: this.userSession);
        }

        public string Nickname { get; }

        public string NicknameEffectedFrom
            => string.Format(Resources.NicknameEffectedFrom, nicknameChecker.ForceLoginDate);

        public bool IsRecommendLoginByNickname
            => nicknameChecker.IsRecommendLoginByNickname;

        public bool IsDisableUpdateNickname
            => !string.IsNullOrWhiteSpace(userSession.NickName);

        public string Username
            => userSession.UserName;

        public static string OldMainRootPath
            => UrlHelper.MapOldMainPath("~");

        public Result Result { get; set; }

        public bool IsValid()
            => nicknameChecker.IsOfferForceLoginFlow;
    }
}