namespace Alpha.Main.Site.ViewModels.Dashboard
{
    using System.Web;
    using Configurations;
    using Main.Helpers;
    using Main.Models.Enumerations;

    public class DashboardViewModel
    {
        public DashboardViewModel(byte roleId)
        {
            UserLevelName = Enumeration.FromValue<UserLevel>(roleId).DisplayName.ToLower();
        }

        public string UserLevelName { get; }

        public bool EnablePlayForFun => AppSettings.EnablePlayForFun;

        public string OldMainRootPath => MicroSites.OldMainPath("~/");

        public string MainRootPath => AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, "~/");
    }
}