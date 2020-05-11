namespace Alpha.Main.Site.ViewModels.Dashboard
{
    using Alpha.Web.Models;
    using Configurations;
    using Helpers;

    public class DashboardViewModel
    {
        public DashboardViewModel(byte roleId)
        {
            UserLevelName = Enumeration.FromValue<UserLevel>(roleId).DisplayName.ToLower();
        }

        public string UserLevelName { get; }

        public bool EnablePlayForFun => AppSettings.EnablePlayForFun;

        public string OldMainRootPath => UrlHelper.MapOldMainPath("~/");

        public string MainRootPath => UrlHelper.MapMainPath("~/");
    }
}
