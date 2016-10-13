namespace Alpha.Main.Site.ViewModels.Security
{
    using System.Web;
    using Alpha.Main.Helpers;
    using Main.Models.Enumerations;

    public class PasswordViewModel
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public byte CustRoleId { get; set; }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.CustRoleId).DisplayName.ToLower();
            }
        }

        public string HashKey { get; set; }

        public bool IsCasinoSync { get; set; }

        public bool EnableSecurityCode { get; set; }

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

        public bool IsChangeMemberPassword
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.CustRoleId).IsMemberLevel();
            }
        }

        public string Title { get; set; }
    }
}