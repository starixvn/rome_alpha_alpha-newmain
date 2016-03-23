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
        public static readonly Dictionary<string, Func<MenuStatus>> MenuItemTypeStatusRules = new Dictionary<string, Func<MenuStatus>>
        {
            [string.Empty] = null,
            ["ReportMenuItem"] = () => new MenuStatus(() => !Current.IsSubAcc || Current.SubPageAccess.ViewReports, null),
            ["TotalBetsMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewTotalBets),
            ["MemberInfoMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewMemberInfo),
            ["TransferMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.Transfer),
            ["ViewLogMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewLog),
            ["SubAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc),
            ["RacingMenuItem"] = () => new MenuStatus(IsShowRacing),
            ["CasinoMenuItem"] = () => new MenuStatus(IsShowCasino),
            ["BingoMenuItem"] = () => new MenuStatus(IsShowBingoReport),
            ["BingoPositionTakingMenuItem"] = () => new MenuStatus(IsShowBingo),
            ["NumberGameMenuItem"] = () => new MenuStatus(IsShowNumberGame),
            ["LiveCasinoMenuItem"] = () => new MenuStatus(IsShowLiveCasino),
            ["ConsolidatedReportMenuItem"] = () => new MenuStatus(UserSession.Current.IsShowConsolidatedReport),
            ["FoobetMenuItem"] = () => new MenuStatus(UserSession.Current.CustId != AppSettings.CustIdFoobet),
            ["MemberWinLimitMenuItem"] = () => new MenuStatus(UserSession.Current.IsLiveCasinoSync || !UserSession.Current.IsHideGoldDeluxe),
            ["LastBetMonitorMenuItem"] = () => new MenuStatus(null, () =>
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

            })
        };
    }
}