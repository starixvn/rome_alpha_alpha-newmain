namespace Alpha.Main.Site.Services
{
    public interface UserPermissionService
    {
        bool IsShowGoldDeluxeSettingForCredit { get; }

        bool IsShowLiveCasino { get; }

        bool IsShowSuperLive { get; }

        bool IsShowColossusBets { get; }
    }
}
