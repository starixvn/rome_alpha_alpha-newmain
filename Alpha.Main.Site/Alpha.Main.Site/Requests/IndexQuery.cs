#pragma warning disable 612, 618

namespace Alpha.Main.Site.Models.Requests
{
    using Alpha.Main.Site.Helpers;
    using Alpha.Web.Models;
    using Configurations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;
    using Web.Helpers;

    public class IndexQuery : IRequest<IndexViewModel>
    {
        private readonly bool isEnableSecurityCode;
        private readonly IUserSession userSession;
        private readonly bool disableMessages;
        private readonly NicknameChecker nicknameChecker;

        public IndexQuery(
            IUserSession userSession,
            bool isEnableSecurityCode,
            bool? disableMessages,
            NicknameChecker nicknameChecker)
        {
            this.userSession = userSession;
            this.isEnableSecurityCode = isEnableSecurityCode;
            this.disableMessages = disableMessages ?? AppSettings.DisableMessages;
            this.nicknameChecker = nicknameChecker;
            IsProtectedMode = userSession.IsProtectedMode;
            IsOfferProtectedMode = userSession.IsOfferProtectedMode;
        }

        public int CustId => userSession.CustId;

        public string UserName => userSession.UserName;

        public string NickName => userSession.NickName;

        public int OnUserId => userSession.OnUserId;

        public bool IsSubAcc => userSession.IsSubAcc;

        public int SubAccId => userSession.SubAccId;

        public string SubUserName => userSession.SubUserName;

        public byte RoleId => userSession.RoleId;

        public string Language => userSession.Language;

        public string LanguageKey => userSession.LangKey;

        public string SubPageAccess => userSession.SubPageAccess;

        public bool IsSubAccount => userSession.IsSubAcc;

        public bool IsInternalWebVpn => userSession.IsInternalWebVPN;

        public bool IsInternal => userSession.IsInternal;

        public string AdminName => userSession.AdminName;

        public bool EnableSecurityCode => userSession.IsLogged &&
                    !userSession.IsPasswordExpired &&
                    this.isEnableSecurityCode;

        public bool ShowSearchAccount
            => userSession.SubPageAccess.ViewMemberInfo && !IsProtectedMode;

        public bool DisableMessages => disableMessages;

        public bool ShowNicknameMenu
            => !nicknameChecker.IsOfferForceLoginAndOverDueDate;

        public bool ShowCreateMemberByQRCode =>
            userSession.SubPageAccess.CreateAccount 
            && Enumeration.FromValue<UserLevel>(userSession.RoleId).IsAgentLevel()
            && !IsProtectedMode;

        public bool IsIndirectLoggedIn => userSession.IsIndirectLoggedIn;

        public string DarkModeName => DarkModeHelper.GetDarkModeName(userSession);
        
        public bool IsProtectedMode { get; }

        public bool IsOfferProtectedMode { get; }
    }
}