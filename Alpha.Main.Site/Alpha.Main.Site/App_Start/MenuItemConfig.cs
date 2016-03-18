namespace Alpha.Main.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
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
            container.Register<MenuItem>(() => new MenuItem(getMenuStatusFunc: null));


            container.Register<ReportMenuItem>(
                () => new ReportMenuItem(() => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewReports)));

            container.Register<TotalBetsMenuItem>(
                () => new TotalBetsMenuItem(() => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewTotalBets)));

            container.Register<MemberInfoMenuItem>(
                () => new MemberInfoMenuItem(() => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewMemberInfo)));

            container.Register<TransferMenuItem>(
               () => new TransferMenuItem(() => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.Transfer)));

            container.Register<ViewLogMenuItem>(
               () => new ViewLogMenuItem(() => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewLog)));

            container.Register<SubAccountMenuItem>(
                () => new SubAccountMenuItem(() => new MenuStatus(!Current.IsSubAcc)));

            container.Register<RacingMenuItem>(
                () => new RacingMenuItem(() => new MenuStatus(IsShowRacing)));

            container.Register<CasinoMenuItem>(
                () => new CasinoMenuItem(() => new MenuStatus(IsShowCasino)));

            container.Register<BingoMenuItem>(
                () => new BingoMenuItem(() => new MenuStatus(IsShowBingoReport)));

            container.Register<NumberGameMenuItem>(
                () => new NumberGameMenuItem(() => new MenuStatus(IsShowNumberGame)));

            container.Register<LiveCasinoMenuItem>(
                () => new LiveCasinoMenuItem(() => new MenuStatus(IsShowLiveCasino)));

            container.Register<ConsolidatedReportMenuItem>(
                () => new ConsolidatedReportMenuItem(() => new MenuStatus(UserSession.Current.IsShowConsolidatedReport)));

            container.Register<FoobetMenuItem>(
                () => new FoobetMenuItem(() => new MenuStatus(UserSession.Current.CustId != AppSettings.CustIdFoobet)));

            container.Register<MemberWinLimitMenuItem>(() => new MemberWinLimitMenuItem(
              () => new MenuStatus(UserSession.Current.IsLiveCasinoSync || !UserSession.Current.IsHideGoldDeluxe)));

            container.Register<LastBetMonitorMenuItem>(() => new LastBetMonitorMenuItem(
              () => new MenuStatus(() =>
              {
                  if (UserSession.Current.IsInternal || AppSettings.EnableBetsListMonitoring)
                  {
                      return false;
                  }

                  var usersAlwaysToAccessThisFunction = AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;
                  var isDisabled = string.IsNullOrWhiteSpace(usersAlwaysToAccessThisFunction) ||
                      !usersAlwaysToAccessThisFunction.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                      .ToList()
                      .Contains(UserSession.Current.CustId.ToString());

                  return isDisabled;

              })));            
        }
    }
}