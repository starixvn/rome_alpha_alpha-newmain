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
            bool isEnabledOtp)
        {
            CustId = custId;
            EnableSecurityCode = enableSecurityCode;
            IsViewedMemberInfo = isViewedMemberInfo;
            Username = username;
            NickName = nickname;
            IsEnabledOtp = isEnabledOtp;
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

        public string LoginOptionLink => $"{OldMainRootPath}LoginOption?custId={CustId}";
    }
}
