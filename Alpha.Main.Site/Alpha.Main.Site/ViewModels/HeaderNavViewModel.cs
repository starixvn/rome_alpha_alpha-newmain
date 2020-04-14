namespace Alpha.Main.Site.ViewModels
{
    using Configurations;
    using Helpers;

    public class HeaderNavViewModel
    {
        public HeaderNavViewModel(
            int custId, 
            string username,
            string nickname,
            bool enableSecurityCode, 
            bool isViewedMemberInfo,
            bool isEnabledOtp,
            bool isDefaultAvatar,
            string embedImage,
            bool showNicknameMenu,
            string language)
        {
            CustId = custId;
            EnableSecurityCode = enableSecurityCode;
            IsViewedMemberInfo = isViewedMemberInfo;
            Username = username;
            NickName = nickname;
            IsEnabledOtp = isEnabledOtp;
            IsDefaultAvatar = isDefaultAvatar;
            EmbedImage = embedImage;
            ShowNicknameMenu = showNicknameMenu;
            DesktopHeaderBanner = new HeaderBannerViewModel(language);
            MobileHeaderBanner = new HeaderBannerViewModel(language, isMobile: true);
        }

        public int CustId { get; }

        public string Username { get; }

        public string NickName { get; }

        public bool IsEnabledOtp { get; }

        public bool EnableSecurityCode { get; }

        public string OldMainRootPath => UrlHelper.MapOldMainPath("~/");

        public string HowToUseRootPath => UrlHelper.MapHowToUsePath("~/");

        public bool IsViewedMemberInfo { get; }

        public string MainRootPath { get; } = UrlHelper.MapMainPath("~/");

        public string AccountInfoRootPath { get; } = UrlHelper.MapAccountInfoPath("~/");

        public bool IsDisabledContactUsLink { get; } = AppSettings.IsDisabledContactUsLink;

        public bool ShowHelpLink { get; } = AppSettings.ShowHelpLink;

        public bool IsDefaultAvatar { get; }

        public string EmbedImage { get;}

        public bool ShowNicknameMenu { get; }

        public HeaderBannerViewModel DesktopHeaderBanner { get; }

        public HeaderBannerViewModel MobileHeaderBanner { get; }
    }
}
