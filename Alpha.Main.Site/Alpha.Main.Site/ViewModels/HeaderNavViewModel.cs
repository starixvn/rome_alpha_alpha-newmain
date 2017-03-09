namespace Alpha.Main.Site.ViewModels
{
    using System.Web;
    using Configurations;
    using Main.Helpers;

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

        public string OldMainRootPath => MicroSites.OldMainUrl("~/");

        public string HowToUseRootPath => MicroSites.HowToUseUrl("~/");

        public bool IsViewedMemberInfo { get; }

        public string MainRootPath { get; } = AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, "~/");

        public bool IsDisabledContactUsLink { get; } = AppSettings.IsDisabledContactUsLink;
    }
}