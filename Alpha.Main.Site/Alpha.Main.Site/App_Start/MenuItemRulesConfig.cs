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

    public static class MenuItemRulesConfig
    {
        public static readonly Dictionary<string, Func<IMenuItem>> MenuContainer = new Dictionary<string, Func<IMenuItem>>
        {
            [string.Empty] = () => new MenuItem(getMenuStatusFunc: null),
            ["ReportMenuItem"] = () => new MenuItem(() => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewReports)),
            ["TotalBetsMenuItem"] = () => new MenuItem(() => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewTotalBets)),
            ["MemberInfoMenuItem"] = () => new MenuItem(() => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewMemberInfo)),
            ["TransferMenuItem"] = () => new MenuItem(() => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.Transfer)),
            ["ViewLogMenuItem"] = () => new MenuItem(() => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewLog)),
            ["SubAccountMenuItem"] = () => new MenuItem(() => new MenuStatus(!Current.IsSubAcc)),
            ["RacingMenuItem"] = () => new MenuItem(() => new MenuStatus(IsShowRacing)),
            ["CasinoMenuItem"] = () => new MenuItem(() => new MenuStatus(IsShowCasino)),
            ["BingoMenuItem"] = () => new MenuItem(() => new MenuStatus(IsShowBingoReport)),
            ["NumberGameMenuItem"] = () => new MenuItem(() => new MenuStatus(IsShowNumberGame)),
            ["LiveCasinoMenuItem"] = () => new MenuItem(() => new MenuStatus(IsShowLiveCasino)),
            ["ConsolidatedReportMenuItem"] = () => new MenuItem(() => new MenuStatus(UserSession.Current.IsShowConsolidatedReport)),            
            ["FoobetMenuItem"] = () => new MenuItem(() => new MenuStatus(UserSession.Current.CustId != AppSettings.CustIdFoobet)),
            ["MemberWinLimitMenuItem"] = () => new MenuItem(() => new MenuStatus(UserSession.Current.IsLiveCasinoSync || !UserSession.Current.IsHideGoldDeluxe)),
            ["LastBetMonitorMenuItem"] = () => new MenuItem(() => new MenuStatus(() =>
            {
                if (UserSession.Current.IsInternal || AppSettings.EnableBetsListMonitoring)
                {
                    return false;
                }

                var usersAlwaysToAccessThisFunction = AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;
                var disabled = string.IsNullOrWhiteSpace(usersAlwaysToAccessThisFunction) ||
                    !usersAlwaysToAccessThisFunction.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                    .Contains(UserSession.Current.CustId.ToString());

                return disabled;

            }))
        };
    }
}