using System.Web;
using Alpha.Main.Helpers;

namespace Alpha.Main.Site.ViewModels.Security
{
    public class BaseSecurityCodeViewModel
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string HashKey { get; set; }

        public string OldMainRootPath
        {
            get
            {
                return MicroSites.OldMainUrl("~/");
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