namespace Alpha.Main.Site.ViewModels
{
    using Configurations;
    using Helpers;

    public class HeaderNavViewModel
    {
        public HeaderNavViewModel(int custId, bool enableSecurityCode, bool isViewedMemberInfo)
        {
            CustId = custId;
            EnableSecurityCode = enableSecurityCode;
            IsViewedMemberInfo = isViewedMemberInfo;
        }

        public int CustId { get; }

        public bool EnableSecurityCode { get; }

        public string OldMainRootPath => UrlHelper.MapOldMainPath("~/");

        public string HowToUseRootPath => UrlHelper.MapHowToUsePath("~/");

        public bool IsViewedMemberInfo { get; }

        public string MainRootPath { get; } = UrlHelper.MapMainPath("~/");

        public bool IsDisabledContactUsLink { get; } = AppSettings.IsDisabledContactUsLink;

        public bool ShowHelpLink { get; } = AppSettings.ShowHelpLink;
    }
}
