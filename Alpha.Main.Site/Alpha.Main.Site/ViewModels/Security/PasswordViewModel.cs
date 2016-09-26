namespace Alpha.Main.Site.ViewModels.Security
{
    using System.Web;
    using Alpha.Main.Helpers;

    public class PasswordViewModel
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public int CustRoleId { get; set; }

        public string HashKey { get; set; }

        public bool IsCasinoSync { get; set; }

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