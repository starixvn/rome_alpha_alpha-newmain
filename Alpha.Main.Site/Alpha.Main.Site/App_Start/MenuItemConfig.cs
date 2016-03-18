namespace Alpha.Main.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Models.Menus;
    using Services;
    using SimpleInjector;
    using Sunplus.Agent.Model.Entities;
    using static Sunplus.Agent.Model.Entities.Permission.UserPermission;
    using static Sunplus.Agent.Model.Entities.UserSession;

    public static class MenuItemConfig
    {
        public static void Register(Container container)
        {
            container.Register<MenuItem>(() => new MenuItem(checkVisibleFunc: null));

            container.Register<ReportMenuItem>(
                () => new ReportMenuItem(() => Current.SubPageAccess.ViewReports));

            container.Register<TotalBetsMenuItem>(
                () => new TotalBetsMenuItem(() => Current.SubPageAccess.ViewTotalBets));

            container.Register<MemberInfoMenuItem>(
                () => new MemberInfoMenuItem(() => Current.SubPageAccess.ViewMemberInfo));

            container.Register<TransferMenuItem>(
               () => new TransferMenuItem(() => Current.SubPageAccess.Transfer));

            container.Register<ViewLogMenuItem>(
               () => new ViewLogMenuItem(() => Current.SubPageAccess.ViewLog));

            container.Register<SubAccountMenuItem>(() => new SubAccountMenuItem(() => !Current.IsSubAcc));
            container.Register<RacingMenuItem>(() => new RacingMenuItem(() => IsShowRacing));
            container.Register<CasinoMenuItem>(() => new CasinoMenuItem(() => IsShowCasino));
            container.Register<BingoMenuItem>(() => new BingoMenuItem(() => IsShowBingoReport));
            container.Register<NumberGameMenuItem>(() => new NumberGameMenuItem(() => IsShowNumberGame));
            container.Register<LiveCasinoMenuItem>(() => new LiveCasinoMenuItem(() => IsShowLiveCasino));
            container.Register<ConsolidatedReportMenuItem>(
                () => new ConsolidatedReportMenuItem(() => UserSession.Current.IsShowConsolidatedReport));

            container.Register<FoobetMenuItem>(
                () => new FoobetMenuItem(() => UserSession.Current.CustId != AppSettings.CustIdFoobet));

            container.Register<MemberWinLimitMenuItem>(() => new MemberWinLimitMenuItem(
              () => UserSession.Current.IsLiveCasinoSync || !UserSession.Current.IsHideGoldDeluxe));
        }                          
    }
}