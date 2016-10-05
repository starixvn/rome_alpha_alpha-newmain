namespace Alpha.Main.Site.ViewModels
{
    using System.Web;
    using Main.Helpers;

    public class HeaderNavViewModel
    {
        public HeaderNavViewModel(int custId, bool enableSecurityCode)
        {
            CustId = custId;
            EnableSecurityCode = enableSecurityCode;
        }

        public int CustId { get; private set; }

        public bool EnableSecurityCode { get; private set; }

        public string OldMainRootPath => MicroSites.OldMainUrl("~/");

        public string HowToUseRootPath => MicroSites.HowToUseUrl("~/");
            }
        }

        public string MainRootPath
        {
            get
            {
                return AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, "~/");
    }
}