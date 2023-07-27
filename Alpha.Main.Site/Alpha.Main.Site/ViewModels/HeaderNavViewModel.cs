namespace Alpha.Main.Site.ViewModels
{
    using System.Linq;
    using Configurations;
    using Helpers;

    public class HeaderNavViewModel
    {
        public HeaderNavViewModel(
            int custId,
            string username,
            string nickname,
            bool enableSecurityCode,
            bool showSearchAccount,
            bool isEnabledOtp,
            bool isDefaultAvatar,
            string embedImage,
            bool showNicknameMenu,
            bool showCreateMemberByQRCode,
            bool isDarkMode,
            bool isProtectedMode,
            bool isOfferProtectedMode,
            bool isSubAccount,
            int superId)
        {
            CustId = custId;
            EnableSecurityCode = enableSecurityCode;
            ShowSearchAccount = showSearchAccount;
            Username = username;
            NickName = nickname;
            IsEnabledOtp = isEnabledOtp;
            IsDefaultAvatar = isDefaultAvatar;
            EmbedImage = embedImage;
            ShowNicknameMenu = showNicknameMenu;
            ShowCreateMemberByQRCode = showCreateMemberByQRCode;
            IsDarkMode = isDarkMode;
            IsProtectedMode = isProtectedMode;
            IsOfferProtectedMode = isOfferProtectedMode;
            IsSubAccount = isSubAccount;
            SuperId = superId;
        }

        public int CustId { get; }

        public string Username { get; }

        public string NickName { get; }

        public bool IsEnabledOtp { get; }

        public bool EnableSecurityCode { get; }

        public string OldMainRootPath => UrlHelper.MapOldMainPath("~/");

        public string HowToUseRootPath => UrlHelper.MapHowToUsePath("~/");

        public bool ShowSearchAccount { get; }

        public string MainRootPath { get; } = UrlHelper.MapMainPath("~/");

        public string AccountInfoRootPath { get; } = UrlHelper.MapAccountInfoPath("~/");

        public bool IsDisabledContactUsLink { get; } =
            AppSettings.IsDisabledContactUsLink
            && !AppSettings.IsViva88Domain;

        public bool ShowHelpLink { get; } = AppSettings.ShowHelpLink;

        public bool IsDefaultAvatar { get; }

        public string EmbedImage { get; }

        public bool ShowNicknameMenu { get; }

        public bool ShowCreateMemberByQRCode { get; }

        public bool IsOfferLiveChat { get; } = AppSettings.IsOfferLiveChat;

        public bool IsDarkMode { get; }

        public bool IsProtectedMode { get; }

        public bool IsOfferProtectedMode { get; }

        public bool IsSubAccount { get; }

        public int SuperId { get; }

        public bool ShowAPIKeyFunction
            => !IsSubAccount
            && (AppSettings.APISuperIds.Any(s => string.Equals(s, "*"))
                || AppSettings.APISuperIds.Any(s => string.Equals(s, SuperId.ToString())));
    }
}
