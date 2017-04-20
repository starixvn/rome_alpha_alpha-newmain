#pragma warning disable 612, 618

namespace Alpha.Main
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.Products;
    using Helpers;
    using Models.Menus;
    using static Sunplus.Agent.Model.Entities.Permission.UserPermission;
    using static Sunplus.Agent.Model.Entities.UserSession;
    using Sunplus.Agent.Model.Definitions;
    using Sunplus.Agent.Utilities;

    public static class FunctionRegistry
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed!")]
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
            ["MemberWinLimitMenuItem"] = () => new MenuStatus(Current.IsLiveCasinoSync || !Current.IsHideGoldDeluxe),
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
                })
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
            var currentUrlCookieName = CookieHelper.GetCurrentUrlCookieName(Current.CustId, Current.SubAccId, Encryption.PearsonEncode);

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
                        Logs.WriteInfo(ex);
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