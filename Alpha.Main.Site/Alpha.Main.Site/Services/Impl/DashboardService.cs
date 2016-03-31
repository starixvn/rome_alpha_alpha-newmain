namespace Alpha.Main.Site.Services.Impl
{
    using App_GlobalResources;
    using Fanex.Age.UI;
    using Fanex.Common;
    using Fanex.CustomerSetting.Module.Core.Web;
    using Models.Dtos;
    using Models.Requests;
    using Models.Views;
    using Sunplus.Agent.Model.Common;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Xml;
    using SiteServices = Alpha.Main.Site.Services;

    public class DashboardService : IDashboardService
    {
        private readonly SiteMenuService siteMenuService;

        private AppPathService _appPathService;

        public DashboardService(
            AppPathService appPathService,
            SiteMenuService siteMenuService)
        {
            _appPathService = appPathService;
            this.siteMenuService = siteMenuService;
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

        public void StoreSession(MainIndexViewModel dashboardViewModel)
        {
            HttpContext.Current.Session["ph"] = dashboardViewModel.PasswordHash;

            // _sessionValidationService.AddUsernameTokenToCookie();
            // SessionValidator.AddSessionVerificationToken();
        }

        private string GetOldMainAppPath()
        {
            return _appPathService.GetAppPath(SiteServices.AppSettings.OldMainAppPath, "~/");
        }
    }
}