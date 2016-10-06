namespace Alpha.Main.Site.ViewModels.Dashboard
{
    using Main.Models.Enumerations;

    public class DashboardViewModel
    {
        public DashboardViewModel(byte roleId)
        {
            UserLevelName = Enumeration.FromValue<UserLevel>(roleId).DisplayName.ToLower();
        }

        public string UserLevelName { get; }
    }
}