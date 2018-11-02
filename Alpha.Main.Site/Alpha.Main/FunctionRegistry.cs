#pragma warning disable 612, 618

namespace Alpha.Main
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Configurations;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Products;
    using Helpers;
    using Models;
    using Models.Menus;
    using Sunplus.Agent.Model.Definitions;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using static Sunplus.Agent.Model.Entities.Permission.UserPermission;
    using static Sunplus.Agent.Model.Entities.UserSession;

    public static class FunctionRegistry
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed!")]
        public static readonly Dictionary<string, Func<MenuStatus>> MenuItemTypeStatusRules = new Dictionary<string, Func<MenuStatus>>
        {
            [string.Empty] = null,
            ["CreateAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.GetPermission(PageAccessType.CreateAccount)),
            ["ReportMenuItem"] = () => new MenuStatus(() => !Current.IsSubAcc || Current.SubPageAccess.ViewReports, null),
            ["TotalBetsMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewTotalBets),
            ["MemberInfoMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewMemberInfo),
            ["TransferMenuItem"] = () => new MenuStatus((!Current.IsSubAcc || Current.SubPageAccess.Transfer) && !AppSettings.OfferClearGivenCredit.Contains(Current.Site.ToUpper())),
            ["TransferMenuItemBySite"] = () => new MenuStatus((!Current.IsSubAcc || Current.SubPageAccess.Transfer) && AppSettings.OfferClearGivenCredit.Contains(Current.Site.ToUpper())),
            ["BetListsMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.GetPermission(PageAccessType.ViewBetLists)),
            ["ViewLogMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewLog),
            ["SubAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc),
            ["CasinoMenuItem"] = () => new MenuStatus(IsShowCasino),
            ["NumberGameMenuItem"] = () => new MenuStatus(IsShowNumberGame),
            ["ConsolidatedReportMenuItem"] = () => new MenuStatus(Current.IsShowConsolidatedReport),
            ["MemberWinLimitMenuItem"] = () => new MenuStatus(!Current.IsHideGoldDeluxe),
            ["LastBetMonitorMenuItem"] = () => new MenuStatus(
                null,
                () =>
                {
                    if (Current.IsInternal || Configurations.AppSettings.EnableBetsListMonitoring)
                    {
                        return false;
                    }

                    var usersAlwaysToAccessThisFunction = Configurations.AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;
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

                    if (!string.IsNullOrWhiteSpace(Configurations.AppSettings.AllowedSitesToViewTBF)
                        && !Configurations.AppSettings.AllowedSitesToViewTBF.ToLower().Contains(Current.Site.ToLower()))
                    {
                        return false;
                    }

                    return true;
                }),
            ["BetChoiceLimitMenuItem"] = () => new MenuStatus(
                () =>
                {
                    return Current.IsShowBetChoiceLimit;
                }),
            ["NG2MenuItem"] = () => new MenuStatus(
                () =>
                {
                    return ProductManager.IsShowSettings(ProductIdConstants.NG2);
                }),
            ["BullfightingMenuItem"] = () => new MenuStatus(
                () => HasPermissionOnProduct(ProductIdConstants.BullFighting)),
            ["ProgressiveContributionMenuItem"] = () => new MenuStatus(() => new User().IsEnabledJackpot),
            ["ProgressiveJackpotMenuItem"] = () => new MenuStatus(() => new User().IsEnabledJackpot),
            ["PokerGamesMenuItem"] = () => new MenuStatus(() => !Current.GetHideProduct(ProductIdConstants.PokerGames)),
            ["ChatBotMenuItem"] = () => new MenuStatus(
                () => !Current.IsSubAcc
                    && (Configurations.AppSettings.OfferChatbotSites.Contains("*")
                        || Configurations.AppSettings.OfferChatbotSites.Contains(Current.Site, StringComparer.OrdinalIgnoreCase)))
        };

        private static Func<bool> showAccountInfo = () =>
        {
            return Current == null ? false : Current.SubPageAccess.ViewMemberInfo;
        };

        private static Func<string> getDefaultUrl = () =>
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }

            var defaultMainPage = HttpContext.Current.Response.ApplyAppPathModifier("~/Dashboard/Index");
            var currentUrlCookieName = CookieHelper.GetCookieNameOfCurrentPage(Current.CustId, Current.SubAccId, Encryption.PearsonEncode);

            var currentURLCookie = HttpContext.Current.Request.Cookies[currentUrlCookieName];
            var ignoreUrls = new List<string>
            {
                "Error.aspx",
                "SignIn.aspx",
                "ChangePassword.aspx",
                "SecurityCode"
            };

            if (currentURLCookie != null &&
                !string.IsNullOrWhiteSpace(currentURLCookie.Value) &&
                !Current.IsInternal &&
                !ignoreUrls.Any(str => currentURLCookie.Value.Contains(str)))
            {
                if (!string.IsNullOrWhiteSpace(currentURLCookie.Value)
                && (currentURLCookie.Value.ToLower().Contains("http") || currentURLCookie.Value.ToLower().Contains("https")))
                {
                    try
                    {
                        Uri currentUrl = new Uri(HttpUtility.UrlDecode(currentURLCookie.Value));

                        if (HttpContext.Current.Request.Url.Host == currentUrl.Host)
                        {
                            return currentUrl.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.Handle(ex, TraceEventType.Information);
                    }
                }
            }

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

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed!")]
        public static readonly IDictionary<string, Func<string>> MenuItemDisplayNameRules = new Dictionary<string, Func<string>>
        {
            ["ng2"] = () => { return ProductManager.GetProductName(ProductIdConstants.NG2); }
        };
    }
}