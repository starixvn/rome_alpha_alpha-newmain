namespace Alpha.Main.Site.ViewModels
{
    using System.Web;
    using Main.Helpers;

    public class HeaderNavViewModel
    {
        public HeaderNavViewModel(int custId, bool enableSecurityCode)
        {
            this.CustId = custId;
            this.EnableSecurityCode = enableSecurityCode;
        }

        public int CustId { get; private set; }

        public bool EnableSecurityCode { get; private set; }

        public string OldMainRootPath
        {
            get
            {
                return MicroSites.OldMainUrl("~/");
            }
        }

        public string HowToUseRootPath
        {
            get
            {
                return MicroSites.HowToUseUrl("~/");
            }
        }

        public string MainRootPath
        {
            get
            {
                return AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, "~/");
            }
        }
    }
}