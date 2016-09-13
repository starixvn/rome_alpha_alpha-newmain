namespace Alpha.Main.Site.ViewModels
{
    using System.Web;
    using Configurations;
    using Main.Helpers;

    public class HeaderNavViewModel
    {
        public int CustId { get; set; }

        public bool EnableSecurityCode { get; set; }

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

        public bool IsShowOldViewLink
        {
            get
            {
                return AppSettings.IsShowOldViewLink;
            }
        }

    }
}