namespace Alpha.Main.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using Fanex.Common;
    using Sunplus.Agent.Model.Entities;

    public class UserMessageServiceImpl : UserMessageService
    {
        private const string IsShowedMessageSessionKey = "messageisshowed";
        private IUserSession _userSession;

        public UserMessageServiceImpl(IUserSession userSession)
        {
            _userSession = userSession;
        }

        private bool IsShowedMessage
        {
            get
            {
                return false;
                //// Use it because we want to consistency with old site
                return Converter.ToString2(HttpContext.Current.Session[IsShowedMessageSessionKey]).Equals("1");
            }
            set
            {
                HttpContext.Current.Session[IsShowedMessageSessionKey] = value ? 1 : 0;
            }
        }

        public IEnumerable<string> GetHotMessages(string hotMessageFilePath)
        {
            List<string> hotMessages = new List<string>();

            if (!File.Exists(hotMessageFilePath))
            {
                throw new FileNotFoundException("Cannot find user tooltip message.");
            }

            var xmlNormalAnnouncement = new XmlDocument();

            xmlNormalAnnouncement.Load(hotMessageFilePath);

            if (!xmlNormalAnnouncement.DocumentElement.HasChildNodes)
            {
                return Enumerable.Empty<string>();
            }

            if (!IsShowedMessage)
            {
                IsShowedMessage = true;

                foreach (XmlNode announcementXmlNode in xmlNormalAnnouncement.DocumentElement.ChildNodes)
                {
                    if (announcementXmlNode == null ||
                        announcementXmlNode.Attributes["id"] == null ||
                        string.IsNullOrEmpty(announcementXmlNode.Attributes["id"].Value) ||
                        announcementXmlNode.Attributes["role"] == null ||
                        string.IsNullOrEmpty(announcementXmlNode.Attributes["role"].Value) ||
                        announcementXmlNode.Attributes["startdate"] == null ||
                        string.IsNullOrEmpty(announcementXmlNode.Attributes["startdate"].Value) ||
                        announcementXmlNode.Attributes["enddate"] == null ||
                        string.IsNullOrEmpty(announcementXmlNode.Attributes["enddate"].Value))
                    {
                        continue;
                    }

                    var hotMessage = GetHotMessageByLanguage(_userSession.Language.ToLower(), announcementXmlNode);

                    if (!string.IsNullOrWhiteSpace(hotMessage))
                    {
                        hotMessages.Add(hotMessage);
                    }
                }
            }

            return hotMessages;
        }

        private string GetHotMessageByLanguage(string language, XmlNode announcementXmlNode)
        {
            var offerForLevel = Converter.ToInt(announcementXmlNode.Attributes["role"].Value);
            var startDate = Converter.ToDateTime(announcementXmlNode.Attributes["startdate"].Value);
            var endDate = Converter.ToDateTime(announcementXmlNode.Attributes["enddate"].Value);
            var currentDate = DateTime.Now;

            if (startDate <= currentDate && currentDate <= endDate &&
                (_userSession.RoleId == offerForLevel || offerForLevel == 0))
            {
                switch (language)
                {
                    case "zh-tw":
                        return announcementXmlNode.SelectNodes("zh-TW")[0].InnerText;

                    case "zh-cn":
                        return announcementXmlNode.SelectNodes("zh-CN")[0].InnerText;

                    case "ja-jp":
                        return announcementXmlNode.SelectNodes("ja-JP")[0].InnerText;

                    case "th-th":
                        return announcementXmlNode.SelectNodes("th-TH")[0].InnerText;

                    case "ko-kr":
                        return announcementXmlNode.SelectNodes("ko-KR")[0].InnerText;

                    case "vi-vn":
                        break;

                    default:
                        return announcementXmlNode.SelectNodes("en-US")[0].InnerText;
                }
            }

            return string.Empty;
        }
    }
}