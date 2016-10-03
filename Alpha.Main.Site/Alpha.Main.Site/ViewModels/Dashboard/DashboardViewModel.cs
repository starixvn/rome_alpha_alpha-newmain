namespace Alpha.Main.Site.ViewModels.Dashboard
{
    using Main.Models.Enumerations;

    public class DashboardViewModel
    {
        private byte roleId;

        public DashboardViewModel(byte roleId)
        {
            this.roleId = roleId;
        }

        public string UserLevelName
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.roleId).DisplayName.ToLower();
            }
        }
    }
}