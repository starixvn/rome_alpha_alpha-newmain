#pragma warning disable 612, 618

namespace Alpha.Main
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Alpha.Main.Constants;
    using Alpha.Main.Services;
    using Alpha.Main.Services.Impl;
    using Alpha.Web.Models;
    using AlphaPlus.User.Models;
    using Configurations;
    using Fanex.Data.Repository;
    using Models;
    using Models.Menus;
    using static AlphaPlus.User.Models.UserPermission;
    using static AlphaPlus.User.Models.UserSession;
    using AuthAppSettings = Alpha.Auth.Configurations.AppSettings;

    public static class FunctionRegistry
    {
        private static TransferService transferService = new TransferServiceImpl(new DynamicRepository());

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed!")]
        public static readonly Dictionary<string, Func<MenuStatus>> MenuItemTypeStatusRules = new Dictionary<string, Func<MenuStatus>>
        {
            [string.Empty] = null,
            ["CreateAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.GetPermission(PageAccessType.CreateAccount)),
            ["PTTemplateMenuItem"] = () => new MenuStatus(AppSettings.IsOfferCreateAccountLiteMode && (!Current.IsSubAcc || Current.SubPageAccess.GetPermission(PageAccessType.CreateAccount))),
            ["ReportMenuItem"] = () => new MenuStatus(() => !Current.IsSubAcc || Current.SubPageAccess.ViewReports, null),
            ["BetStatisticsMenuItem"] = () => new MenuStatus(() => !Current.IsSubAcc || Current.SubPageAccess.ViewReports, null),
            ["TotalBetsMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewTotalBets),
            ["MemberInfoMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewMemberInfo),

            ["SingleTransferMenuItem"] = () => new MenuStatus(SingleTransferMenuItemVisible),
            ["TransferMenuItem"] = () => new MenuStatus(!SingleTransferMenuItemVisible && TransferMenuItemVisible),
            ["AutoTransferMonitoringMenuItem"] = () => new MenuStatus(TransferMenuItemVisible && AutoTransferMonitoringMenuItemVisible),
            ["TransferAllAndClearCreditMenuItem"] = () => new MenuStatus(TransferMenuItemVisible && TransferAllAndClearCreditMenuItemVisible),
            ["TransferDownlineHistoryMenuItem"] = () => new MenuStatus(TransferMenuItemVisible && TransferToDownlineHistoryMenuItemVisible),

            ["BetListsMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.GetPermission(PageAccessType.ViewBetLists)),
            ["ViewLogMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewLog),
            ["SubAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc),
            ["NumberGameMenuItem"] = () => new MenuStatus(IsEnableProduct(ProductId.NumberGame)),
            ["ConsolidatedReportMenuItem"] = () => new MenuStatus(Current.IsShowConsolidatedReport),
            ["LastBetMonitorMenuItem"] = () => new MenuStatus(
                null,
                () =>
                {
                    if (Current.IsInternal || AppSettings.EnableBetsListMonitoring)
                    {
                        return false;
                    }

                    var usersAlwaysToAccessThisFunction = AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;
                    var disabled = string.IsNullOrWhiteSpace(usersAlwaysToAccessThisFunction) ||
                        !usersAlwaysToAccessThisFunction.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList()
                        .Contains(Current.CustId.ToString());

                    return disabled;
                }),
            ["TotalBetsForecast"] = () => new MenuStatus(
                () =>
                {
                    if (Current.IsSubAcc && !Current.SubPageAccess.ViewTotalBets)
                    {
                        return false;
                    }

                    if (AppSettings.NewTBFAllowedSites.Any(site => string.Equals(site, "*") || string.Equals(site, Current.Site, StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }

                    return false;
                }),
            ["BetChoiceLimitMenuItem"] = () => new MenuStatus(
                () => Current.IsShowBetChoiceLimit),
            ["ProgressiveContributionMenuItem"] = () => new MenuStatus(() => new User().IsEnabledJackpot),
            ["ProgressiveJackpotMenuItem"] = () => new MenuStatus(() => new User().ShowProgressiveJackpotGames),
            ["IndirectLoginAccountMenuItem"] = () => new MenuStatus(
                () => AppSettings.IsAllowedIndirectLogin(Current.SRecommendId) && !Current.IsIndirectLoggedIn && (Current.IsSuper || Current.IsMaster)),
            ["DormantAccountListMenuItem"] = () => new MenuStatus(() => Current.IsSuper || Current.IsMaster),
            ["ReportsMenuItem"] = () => new MenuStatus(
                () => !AppSettings.IsUsedNewReports(Current.Site, Current.SRecommendId)),
            ["NewReportsMenuItem"] = () => new MenuStatus(
                () => AppSettings.IsUsedNewReports(Current.Site, Current.SRecommendId)),
            ["BonusDetailMenuItem"] = () => new MenuStatus(
                () => AppSettings.AllowedSitesToViewBonusDetail.Contains(Current.Site.ToUpperInvariant())),
            ["WhiteLabelMenuItem"] = () => new MenuStatus(AppSettings.IsOfferedWhiteLabel(Current.Site))
        };

        private static bool TransferMenuItemVisible
            => !Current.IsSubAcc || Current.SubPageAccess.Transfer;

        private static bool TransferAllAndClearCreditMenuItemVisible
        {
            get
            {
                if (Current.RoleId != UserLevel.Super.Value
                    && Current.RoleId != UserLevel.Master.Value)
                {
                    return false;
                }

                return AuthAppSettings.OfferClearGivenCreditSites.Any(s
                    => string.Equals(s, Current.Site, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(s, "*", StringComparison.OrdinalIgnoreCase));
            }
        }

        private static bool TransferToDownlineHistoryMenuItemVisible
            => AppSettings.IsAllowedTransferToDownlineHistorySite(Current.Site)
            && transferService.IsAllDayBalanceTransfer(Current.CustId);

        private static bool AutoTransferMonitoringMenuItemVisible
            => transferService.IsEnableAutoTransfer(Current.Currency, Current.CustId, AppSettings.Settings["OfferAutoTransfer.Currencies"]);

        private static bool SingleTransferMenuItemVisible
        {
            get
            {
                if (!TransferMenuItemVisible)
                {
                    return false;
                }

                var menu = new List<bool> {
                    TransferAllAndClearCreditMenuItemVisible,
                    TransferToDownlineHistoryMenuItemVisible,
                    AutoTransferMonitoringMenuItemVisible
                };

                return !menu.Any(m => m);
            }
        }

        private static Func<bool> showAccountInfo = () =>
          {
              return Current == null ? false : Current.SubPageAccess.ViewMemberInfo && !Current.IsProtectedMode;
          };

        private static Func<string> getDefaultUrl = () =>
          {
              if (HttpContext.Current == null)
              {
                  return string.Empty;
              }

              var defaultMainPage = HttpContext.Current.Response.ApplyAppPathModifier("~/Dashboard/Index");

              return defaultMainPage;
          };

        public static Func<bool> ShowAccountInfo
        {
            get
            {
                return showAccountInfo;
            }
        }

        public static Func<string> GetDefaultUrl
        {
            get
            {
                return getDefaultUrl;
            }
        }

        public static Func<bool> IsProtectedModeFunc => () => Current.IsProtectedMode;

        public static Func<bool> IsInternalFunc => () => Current.IsInternal;

        public static Func<string, string> BuildInvalidSessionUniqueKey => (string url) =>
          {
              if (url.IndexOf("/winlossdetail/", StringComparison.OrdinalIgnoreCase) > -1
              && AppSettings.EnableLogDifferentSessionBetweenMainAndReports)
              {
                  return $"{Current.CustId}-{Current.SubAccId}";
              }

              return string.Empty;
          };

        public static Func<string> BuildLanguageQueryString => () => $"lng={Current.Language}";

        public static Func<string> BuildDarkModeQueryString => () => $"isDarkMode={(!string.IsNullOrWhiteSpace(Current.DarkModeName)).ToString().ToLowerInvariant()}";
    }
}