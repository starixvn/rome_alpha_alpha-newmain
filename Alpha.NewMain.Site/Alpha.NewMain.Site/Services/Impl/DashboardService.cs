namespace Alpha.NewMain.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Xml;
    using Alpha.NewMain.Site.Repositories;
    using App_GlobalResources;
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
    using Models.Views.CustomerProfile;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Common;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using AlphaDefinitions = Sunplus.Agent.Model.BL.Definitions;

    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        private readonly IFileUploadService _fileUploadService;

        private const string Default_MainPage = "_Reports/PortalPage/PortalPage.aspx";

        public DashboardService(
            IDashboardRepository repository,
            IFileUploadService fileUploadService)
        {
            _repository = repository;
            _fileUploadService = fileUploadService;
        }

        public List<LanguageModel> GetLanguages()
        {
            return _repository.GetLanguages();
        }

        public ViewProfileViewModel GetProfile(ViewProfileQuery message)
        {
            var avatar = GetAvatar(new ViewProfileCriteria
            {
                CustId = message.CustId
            });

            var result = new ViewProfileViewModel
            {
                IsDefaultAvatar = avatar.IsDefaultAvatar,
                EmbedImage = GenerateImage(avatar)
            };

            return result;
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
            string name = string.Join("_", new string[] {
               UserSession.Current.CustId.ToString(),
               UserSession.Current.SubAccId.ToString(),
               "URL"
            });

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
            if (!UserSession.Current.IsLogged)
            {
                return "_MemberInfo/ChangePassword/ChangePassword.aspx?firstLogin=true";
            }

            if (UserSession.Current.IsPasswordExpired)
            {
                return "_MemberInfo/ChangePassword/ChangePassword.aspx?expired=true";
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
                        return "Security/SecurityCode/ForceChangeSecurityCode";
                    case SecurityCodeStatus.FORCED_CHANGE_SC_WITHOUT_VALIDATION:
                        return "Security/SecurityCode/ForceChangeSecurityCodeWithoutValidation";
                    default:
                        return "Security/SecurityCode/VerifySecurityCode";
                }
            }

            if (!securityCodeSession.IsInternal &&
                (oldStatus == SCResult.FORCED_CHANGE_SC || oldStatus == SCResult.FORCED_CHANGE_SC_WITHOUT_VALIDATION))
            {
                // Use the OLD flow of Security Code.
                return "_MemberInfo/SecurityCode/ChangeSecurityCode.aspx";
            }

            var cookieCurrentUrl = GetCookieCurrentURL();
            var currentURLCookie = HttpContext.Current.Request.Cookies[cookieCurrentUrl];
            var url = Default_MainPage;

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

        private string GenerateImage(ProfilePictureModel avatar)
        {
            try
            {
                byte[] binaryAvatar = avatar.BinaryAvatar;
                if (binaryAvatar != null)
                {
                    return "data:image/jpeg;base64," + Convert.ToBase64String(binaryAvatar);
                }
            }
            catch (Exception ex)
            {
                Logs.Write(ex);
            }

            return "data:image/jpeg;base64," + string.Empty;
        }
    }
}