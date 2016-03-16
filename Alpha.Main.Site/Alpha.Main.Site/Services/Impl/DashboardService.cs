namespace Alpha.Main.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;
    using Alpha.Main.Site.Repositories;
    using App_GlobalResources;
    using Auth.Web.Validators;
    using Constants;
    using Criteria.CustomerProfile;
    using Extensions;
    using Fanex.Age.UI;
    using Fanex.Common;
    using Fanex.CustomerSetting.Module.Core.Web;
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using Models.Dtos;
    using Models.Requests;
    using Models.Requests.CustomerProfile;
    using Models.Views;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Common;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Utilities;
    using AlphaDefinitions = Sunplus.Agent.Model.BL.Definitions;
    using SiteServices = Alpha.Main.Site.Services;
    using System.Linq;
    using Models.Enumerations;
    using System.Threading;
    using System.Globalization;
    using Models;
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        private readonly IFileUploadService _fileUploadService;

        private readonly ISessionValidationService _sessionValidationService;

        private string _defaultMainPage;

        private AppPathService _appPathService;

        public DashboardService(
            IDashboardRepository repository,
            IFileUploadService fileUploadService,
            ISessionValidationService sessionValidationService,
            AppPathService appPathService)
        {
            _repository = repository;
            _fileUploadService = fileUploadService;
            _sessionValidationService = sessionValidationService;
            _appPathService = appPathService;
        }

        public List<LanguageModel> GetLanguages()
        {
            return _repository.GetLanguages();
        }

        public ProfilePictureModel GetProfile(ViewProfileQuery message)
        {
            var avatar = GetAvatar(new ViewProfileCriteria
            {
                CustId = message.CustId
            });

            return avatar;
        }

        public ProfilePictureModel GetAvatar(ViewProfileCriteria criteria)
        {
            var avatar = _repository.GetAvatar(criteria);
            if (avatar.IsDefaultAvatar)
            {
                avatar.BinaryAvatar = _fileUploadService.GetDefaultAvatar();
            }

            SessionExtension.IsDefaultAvatar = avatar.IsDefaultAvatar;
            SessionExtension.BinaryAvatar = avatar.BinaryAvatar;
            return avatar;
        }

        public List<string> GetMonthNames()
        {
            var result = new List<string>();
            for (var i = 1; i <= 12; i++)
            {
                result.Add(App_GlobalResources.Resources.ResourceManager.GetString("monthName" + i));
            }

            return result;
        }

        public List<string> GetHotMessages()
        {
            List<string> msgList = new List<string>();
            var filePath = HttpContext.Current.Server.MapPath("~/_Announcement/Resources/ToolTipMessage.xml");
            if (!File.Exists(filePath))
            {
                return msgList;
            }

            var xmlNormalAnnouncement = new XmlDocument();
            xmlNormalAnnouncement.Load(filePath);

            if (xmlNormalAnnouncement.DocumentElement == null ||
                xmlNormalAnnouncement.DocumentElement.ChildNodes == null ||
                xmlNormalAnnouncement.DocumentElement.ChildNodes.Count <= 0)
            {
                return msgList;
            }

            var isShowed = Converter.ToString2(HttpContext.Current.Session["messageisshowed"]).Equals("1");
            var currentDate = DateTime.Now;
            var language = UserSession.Current.Language.ToLower();
            if (!isShowed)
            {
                HttpContext.Current.Session["messageisshowed"] = 1;

                foreach (XmlNode xmlNodeAnnouncement in xmlNormalAnnouncement.DocumentElement.ChildNodes)
                {
                    if (xmlNodeAnnouncement == null ||
                        xmlNodeAnnouncement.ChildNodes == null ||
                        xmlNodeAnnouncement.ChildNodes.Count == 0 ||
                        xmlNodeAnnouncement.Attributes == null ||
                        xmlNodeAnnouncement.Attributes["id"] == null ||
                        string.IsNullOrEmpty(xmlNodeAnnouncement.Attributes["id"].Value) ||
                        xmlNodeAnnouncement.Attributes["role"] == null ||
                        string.IsNullOrEmpty(xmlNodeAnnouncement.Attributes["role"].Value) ||
                        xmlNodeAnnouncement.Attributes["startdate"] == null ||
                        string.IsNullOrEmpty(xmlNodeAnnouncement.Attributes["startdate"].Value) ||
                        xmlNodeAnnouncement.Attributes["enddate"] == null ||
                        string.IsNullOrEmpty(xmlNodeAnnouncement.Attributes["enddate"].Value))
                    {
                        continue;
                    }

                    var offerForLevel = Converter.ToInt(xmlNodeAnnouncement.Attributes["role"].Value);
                    var startDate = Converter.ToDateTime(xmlNodeAnnouncement.Attributes["startdate"].Value);
                    var endDate = Converter.ToDateTime(xmlNodeAnnouncement.Attributes["enddate"].Value);

                    if (startDate <= currentDate &&
                        currentDate <= endDate &&
                        (UserSession.Current.RoleId == offerForLevel || offerForLevel == 0))
                    {
                        switch (language)
                        {
                            case "zh-tw":
                                msgList.Add(xmlNodeAnnouncement.SelectNodes("zh-TW")[0].InnerText);
                                break;
                            case "zh-cn":
                                msgList.Add(xmlNodeAnnouncement.SelectNodes("zh-CN")[0].InnerText);
                                break;
                            case "ja-jp":
                                msgList.Add(xmlNodeAnnouncement.SelectNodes("ja-JP")[0].InnerText);
                                break;
                            case "th-th":
                                msgList.Add(xmlNodeAnnouncement.SelectNodes("th-TH")[0].InnerText);
                                break;
                            case "ko-kr":
                                msgList.Add(xmlNodeAnnouncement.SelectNodes("ko-KR")[0].InnerText);
                                break;
                            case "vi-vn":
                                break;
                            default:
                                msgList.Add(xmlNodeAnnouncement.SelectNodes("en-US")[0].InnerText);
                                break;
                        }
                    }
                }
            }

            return msgList;
        }

        public ResultModel ChangeLanguage(ChangeLanguageQuery message)
        {
            var result = new ResultModel();
            if (!Validation.CheckValidLanguage(message.Language))
            {
                result.ResultCode = 1;
                result.ResultMessage = Resources.strNotAccessPage;
            }
            else
            {
                UserSession.Current.Language = message.Language;
                UserSession.Current.LangKey = Language.LangKeyFromCulture(message.Language);
                AppSession.Language = message.Language;
            }

            return result;
        }

        public string GetCookieCurrentURL()
        {
            string name = string.Join(
                "_",
                new string[] { UserSession.Current.CustId.ToString(), UserSession.Current.SubAccId.ToString(), "URL_Main" });

            return Encryption.PearsonEncode(name);
        }

        public string GetBaseUrl()
        {
            return UserSession.Current.IsInternalWebVPN ?
                HttpContext.Current.Request.ApplicationPath + "/" + AlphaDefinitions.AppSettings.CookielessPath :
                AlphaDefinitions.AppSettings.BaseUrl + AlphaDefinitions.AppSettings.CookielessPath;
        }

        public string GetMainPage()
        {
            _defaultMainPage = "/" + _appPathService.GetAppPath(SiteServices.AppSettings.OldMainApp, "_Reports/PortalPage/PortalPage.aspx");
            var oldMainAppPath = GetOldMainAppPath();
            if (!UserSession.Current.IsLogged)
            {
                return oldMainAppPath + "_MemberInfo/ChangePassword/ChangePassword.aspx?firstLogin=true";
            }

            if (UserSession.Current.IsPasswordExpired)
            {
                return oldMainAppPath + "_MemberInfo/ChangePassword/ChangePassword.aspx?expired=true";
            }

            ISecurityCodeSession securityCodeSession = CookieLessSessionFactory.SecurityCodeSession;
            bool newFlowOfSC = SecurityCodeConfig.IsActive && securityCodeSession.IsRequireSecurityCode;
            var status = (SecurityCodeStatus)securityCodeSession.SecurityCodeResult;
            var oldStatus = UserSession.Current.SecurityCodeResult;

            var mainPage = string.Empty;
            if (newFlowOfSC)
            {
                switch (status)
                {
                    case SecurityCodeStatus.FORCED_CHANGE_SC:
                        return oldMainAppPath + "Security/SecurityCode/ForceChangeSecurityCode";
                    case SecurityCodeStatus.FORCED_CHANGE_SC_WITHOUT_VALIDATION:
                        return oldMainAppPath + "Security/SecurityCode/ForceChangeSecurityCodeWithoutValidation";
                    default:
                        return oldMainAppPath + "Security/SecurityCode/VerifySecurityCode";
                }
            }

            if (!securityCodeSession.IsInternal &&
                (oldStatus == SCResult.FORCED_CHANGE_SC || oldStatus == SCResult.FORCED_CHANGE_SC_WITHOUT_VALIDATION))
            {
                // Use the OLD flow of Security Code.
                return oldMainAppPath + "_MemberInfo/SecurityCode/ChangeSecurityCode.aspx";
            }

            var cookieCurrentUrl = GetCookieCurrentURL();
            var currentURLCookie = HttpContext.Current.Request.Cookies[cookieCurrentUrl];
            var url = _defaultMainPage;

            if (IsMainPageFromCookie())
            {
                url = currentURLCookie.Value;
            }

            mainPage = url;
            return mainPage;
        }

        public bool IsMainPageFromCookie()
        {
            var cookieCurrentUrl = GetCookieCurrentURL();
            var currentURLCookie = HttpContext.Current.Request.Cookies[cookieCurrentUrl];
            return currentURLCookie != null &&
                !string.IsNullOrEmpty(currentURLCookie.Value) &&
                !UserSession.Current.IsInternal &&
                !currentURLCookie.Value.Contains("Error.aspx") &&
                !currentURLCookie.Value.Contains("SignIn.aspx") &&
                !currentURLCookie.Value.Contains("ChangePassword.aspx") &&
                !currentURLCookie.Value.Contains("SecurityCode");
        }

        public List<MenuItem> GetLeftMenu()
        {
            var siteNode = _repository.GetSiteMapNode();
            var menuList = new List<MenuItem>();
            if (!siteNode.HasChildNodes)
            {
                return menuList;
            }

            var pageAccess = UserSession.Current.SubPageAccess;
            var isSubAcc = UserSession.Current.IsSubAcc;
            var subMenuCount = siteNode.ChildNodes.Count;


            var siteMenuService = new SiteMenuServiceImpl(new CachingServiceImpl());

            var user = new Models.User(pageAccess);
            user.Level = Enumeration.FromValue<UserLevel>(2);
            menuList = siteMenuService.GetMenuByUser(user).ToList();

            //for (int i = 0; i < subMenuCount; i++)
            //{
            //    var subMenu = siteNode.ChildNodes[i];
            //    var canViewMenu = CanViewMenu(subMenu.Title, isSubAcc, pageAccess);
            //    if (!canViewMenu
            //        || (subMenu.Roles.Count > 0 && !subMenu.Roles.Contains(UserSession.Current.RoleId)))
            //    {
            //        continue;
            //    }

            //    var showSubMenu = subMenu.GetAttribute("expand") == "true";
            //    if (!subMenu.HasChildNodes && string.IsNullOrWhiteSpace(subMenu.Url))
            //    {
            //        continue;
            //    }

            //    menuList.Add(GetMenu(subMenu, i, showSubMenu));
            //}

            return menuList;
        }

        public string GetPasswordHash()
        {
            return Encryption.RandomString(new Random().Next(4, 8));
        }

        public void StoreSession(MainIndexViewModel dashboardViewModel)
        {
            HttpContext.Current.Session["ph"] = dashboardViewModel.PasswordHash;
            //_sessionValidationService.AddUsernameTokenToCookie();
            //SessionValidator.AddSessionVerificationToken();
        }

        public bool IsShowLeftMenu()
        {
            if (!UserSession.Current.IsLogged || UserSession.Current.IsPasswordExpired)
            {
                return false;
            }

            var securityCodeSession = CookieLessSessionFactory.SecurityCodeSession;
            var newFlowOfSC = SecurityCodeConfig.IsActive && securityCodeSession.IsRequireSecurityCode;
            if (newFlowOfSC)
            {
                return false;
            }

            var oldStatus = UserSession.Current.SecurityCodeResult;
            if (!securityCodeSession.IsInternal &&
                (oldStatus == SCResult.FORCED_CHANGE_SC || oldStatus == SCResult.FORCED_CHANGE_SC_WITHOUT_VALIDATION))
            {
                return false;
            }

            return true;
        }

        private bool CanViewMenu(string menuTitle, bool isSubAcc, PageAccess pageAccess)
        {
            var deniedView = (menuTitle == "reports" && !pageAccess.ViewReports) ||
                (menuTitle == "totalbets" && !pageAccess.ViewTotalBets) ||
                (menuTitle == "memberInfo" && !pageAccess.ViewMemberInfo) ||
                (menuTitle == "transfer" && !pageAccess.Transfer) ||
                (menuTitle == "viewLog" && !pageAccess.ViewLog);
            return !isSubAcc || !deniedView;
        }

        private MenuItem GetMenu(SiteMapNode siteMapNode, int index, bool showSubMenu)
        {
            var menu = new MenuItem
            {
                Url = siteMapNode.Url,
                SubMenus = new List<MenuItem>(),
                Name = siteMapNode.Title
            };

            var isSubAcc = UserSession.Current.IsSubAcc;
            var authorize = new AuthorizationBO(UserSession.Current.NetSessionId);
            var itemCount = siteMapNode.ChildNodes.Count;

            for (var j = 0; j < itemCount; j++)
            {
                SiteMapNode item = siteMapNode.ChildNodes[j];
                var isDeniedView = IsDeniedViewSubMenu(isSubAcc, item, authorize);
                if (isDeniedView)
                {
                    continue;
                }

                menu.SubMenus.Add(GetMenuItem(item, j, index));
            }

            return menu;
        }

        private string GetLeftPanelResource(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    return string.Empty;
                }

                object val = Alpha_sitemap.ResourceManager.GetString(key);
                if (val != null)
                {
                    return val.ToString();
                }
            }
            catch (Exception ex)
            {
                ex.Data["CUR-CULTURE"] = System.Threading.Thread.CurrentThread.CurrentCulture;
                ex.Data["RESOURKEY"] = key;
                Logs.Write(ex);
            }

            return string.Empty;
        }

        private bool IsDeniedViewSubMenu(bool isSubAcc, SiteMapNode subMenu, AuthorizationBO authorize)
        {
            return (isSubAcc && subMenu.Description == RoleDescriptionConstant.SubAcc) ||
                !authorize.AuthorizeUrl(0, 0, false, subMenu.GetAttribute("href")) ||

                (!UserPermission.IsShowRacing && subMenu.Description == RoleDescriptionConstant.HorseRacing) ||
                (UserSession.Current.IsDisabledFI && subMenu.Description == RoleDescriptionConstant.Financial) ||
                (!UserPermission.IsShowCasino && (subMenu.Description == RoleDescriptionConstant.Casino)) ||
                (!UserPermission.IsShowBingoReport && subMenu.Description == RoleDescriptionConstant.Bingo) ||
                (!UserPermission.IsShowNumberGame && subMenu.Title == RoleDescriptionConstant.NumberGame) ||
                (!UserPermission.IsShowLiveCasino && subMenu.Title == RoleDescriptionConstant.LiveCasino) ||
                (!UserSession.Current.IsShowConsolidatedReport && subMenu.Title == RoleDescriptionConstant.ConsolidatedReport) ||
                (UserSession.Current.CustId != SiteServices.AppSettings.CustIdFoobet && subMenu.Title.Contains("foobet")) ||
                (subMenu.Title.Contains("MemberWinLimit") && !UserSession.Current.IsLiveCasinoSync && UserSession.Current.IsHideGoldDeluxe);
        }

        private MenuItem GetMenuItem(SiteMapNode menuItem, int index, int parentIndex)
        {
            var menu = new MenuItem();
            var title = menuItem.Title;
            var bettypeId = 0;

            if (!string.IsNullOrEmpty(menuItem.GetAttribute("microSite")))
            {
                var microSiteSettingKey = menuItem.GetAttribute("microSiteNameSettingKey");
                var microSiteName = AppSettings.GetValue(microSiteSettingKey);
                var microSiteUrl = HttpContext.Current.Response.ApplyAppPathModifier(menuItem.Url);
                microSiteUrl = Regex.Replace(microSiteUrl, HttpRuntime.AppDomainAppVirtualPath, microSiteName, RegexOptions.IgnoreCase);
                menu.Url = microSiteUrl;
            }
            else
            {
                menu.Url = "/" + menuItem.Url;
            }

            if (title.Contains("statement"))
            {
                menu.Url = "/" + menu.Url.Replace("custidParam", UserSession.Current.CustId.ToString());
            }

            //menu.IsImportant = title.Contains("monitor") && !CheckingLastsBetMonitoringFunctionStatus();
            return menu;
        }

        private bool CheckingLastsBetMonitoringFunctionStatus()
        {
            if (UserSession.Current.IsInternal)
            {
                return true;
            }

            var isEnableMonitoringBetsList = SiteServices.AppSettings.EnableBetsListMonitoring;
            if (isEnableMonitoringBetsList)
            {
                return true;
            }

            var usersAlwaysToAccessThisFunction = SiteServices.AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;
            if (string.IsNullOrEmpty(usersAlwaysToAccessThisFunction))
            {
                return false;
            }

            var customerIdArray = usersAlwaysToAccessThisFunction.Split(',');
            for (int i = 0; i < customerIdArray.Length; i++)
            {
                if (UserSession.Current.CustId == Converter.ToInt(customerIdArray[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private string GetOldMainAppPath()
        {
            return "/" + _appPathService.GetAppPath(SiteServices.AppSettings.OldMainApp, "~");
        }
    }
}