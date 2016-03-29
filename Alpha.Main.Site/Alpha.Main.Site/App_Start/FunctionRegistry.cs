namespace Alpha.Main.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using Models.Menus;
    using Sunplus.Agent.Model.BL.Definitions;
    using static Helpers.AppPathHelper;
    using static Sunplus.Agent.Model.Entities.Permission.UserPermission;
    using static Sunplus.Agent.Model.Entities.UserSession;

    public static class FunctionRegistry
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
            ["ConsolidatedReportMenuItem"] = () => new MenuStatus(Current.IsShowConsolidatedReport),
            ["FoobetMenuItem"] = () => new MenuStatus(Current.CustId != Services.AppSettings.CustIdFoobet),
            ["MemberWinLimitMenuItem"] = () => new MenuStatus(Current.IsLiveCasinoSync || !Current.IsHideGoldDeluxe),
            ["LastBetMonitorMenuItem"] = () => new MenuStatus(null, () =>
            {
                if (Current.IsInternal || Services.AppSettings.EnableBetsListMonitoring)
                {
                    return false;
                }

                var usersAlwaysToAccessThisFunction = Services.AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;
                var disabled = string.IsNullOrWhiteSpace(usersAlwaysToAccessThisFunction) ||
                    !usersAlwaysToAccessThisFunction.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                    .Contains(Current.CustId.ToString());

                return disabled;

            })
        };

        public static Func<bool> ShowMenuBar = () =>
        {
            const int ForceChangeSecurityCode = 4;
            const int ForceChangeSecurityCodeWithoutValidation = 3;

            if (!Current.IsLogged || Current.IsPasswordExpired)
            {
                return false;
            }

            var securityCodeSession = CookieLessSessionFactory.SecurityCodeSession;
            var newFlowOfSC = SecurityCodeConfig.IsActive && securityCodeSession.IsRequireSecurityCode;
            if (newFlowOfSC)
            {
                return false;
            }

            var oldStatus = Convert.ToInt32(Current.SecurityCodeResult);
            if (!securityCodeSession.IsInternal &&
                (oldStatus == ForceChangeSecurityCode || oldStatus == ForceChangeSecurityCodeWithoutValidation))
            {
                return false;
            }

            return true;
        };

        public static Func<bool> ShowAccountInfo = () =>
        {
            return Current.SubPageAccess.ViewMemberInfo;
        };

        public static Func<string> GetDefaultUrl = () =>
         {

             var _defaultMainPage = "/" + GetOldMainPath("_Reports/PortalPage/PortalPage.aspx");

             if (!Current.IsLogged)
             {
                 return GetOldMainPath("_MemberInfo/ChangePassword/ChangePassword.aspx?firstLogin=true");
             }

             if (Current.IsPasswordExpired)
             {
                 return GetOldMainPath("_MemberInfo/ChangePassword/ChangePassword.aspx?expired=true");
             }

             ISecurityCodeSession securityCodeSession = CookieLessSessionFactory.SecurityCodeSession;
             bool newFlowOfSC = SecurityCodeConfig.IsActive && securityCodeSession.IsRequireSecurityCode;
             var status = (SecurityCodeStatus)securityCodeSession.SecurityCodeResult;
             var oldStatus = Current.SecurityCodeResult;

             var mainPage = string.Empty;
             if (newFlowOfSC)
             {
                 switch (status)
                 {
                     case SecurityCodeStatus.FORCED_CHANGE_SC:
                         return GetOldMainPath("Security/SecurityCode/ForceChangeSecurityCode");

                     case SecurityCodeStatus.FORCED_CHANGE_SC_WITHOUT_VALIDATION:
                         return GetOldMainPath("Security/SecurityCode/ForceChangeSecurityCodeWithoutValidation");

                     default:
                         return GetOldMainPath("Security/SecurityCode/VerifySecurityCode");
                 }
             }

             if (!securityCodeSession.IsInternal &&
                 (oldStatus == SCResult.FORCED_CHANGE_SC || oldStatus == SCResult.FORCED_CHANGE_SC_WITHOUT_VALIDATION))
             {
                // Use the OLD flow of Security Code.
                return GetOldMainPath("_MemberInfo/SecurityCode/ChangeSecurityCode.aspx");
             }

             return _defaultMainPage;
         };
    }
}