namespace Alpha.Main.Site.Services.Impl
{
    using Sunplus.Agent.Model.Entities.Permission;

    public class UserPermissionServiceImpl : UserPermissionService
    {
        public bool IsShowGoldDeluxeSettingForCredit
        {
            get
            {
                return UserPermission.IsShowGoldDeluxeSettingForCredit;
            }
        }

        public bool IsShowLiveCasino
        {
            get
            {
                return UserPermission.IsShowLiveCasino;
            }
        }

        public bool IsShowSuperLive
        {
            get
            {
                return UserPermission.IsShowSuperLive;
            }
        }

        public bool IsShowColossusBets
        {
            get
            {
                return UserPermission.IsShowColossusBets;
            }
        }
    }
}