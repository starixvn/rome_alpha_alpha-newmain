using System.Web;
using Alpha.Main.Helpers;
using Alpha.Main.Models.Enumerations;

namespace Alpha.Main.Site.ViewModels.Security
{
    public class BaseSecurityCodeViewModel
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string HashKey { get; set; }

        public byte CustRoleId { get; set; }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.CustRoleId).DisplayName.ToLower();
            }
        }

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