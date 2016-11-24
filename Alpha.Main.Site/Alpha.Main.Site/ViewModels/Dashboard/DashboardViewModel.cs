namespace Alpha.Main.Site.ViewModels.Dashboard
{
    using System.Web;
    using Main.Helpers;
    using Main.Models.Enumerations;

    public class DashboardViewModel
    {
        public DashboardViewModel(byte roleId)
        {
            UserLevelName = Enumeration.FromValue<UserLevel>(roleId).DisplayName.ToLower();
        }

        public string UserLevelName { get; }

        public string OldMainRootPath => MicroSites.OldMainUrl("~/");

        public string MainRootPath => AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, "~/");
    }
}