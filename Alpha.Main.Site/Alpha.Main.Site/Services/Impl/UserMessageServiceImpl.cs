namespace Alpha.Main.Site.Services.Impl
{
    using Constants;
    using Fanex.Common;
    using Models.Requests.UserMessages;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    public class UserMessageServiceImpl : UserMessageService
    {
        public IEnumerable<string> GetHotMessages(HotMessagesQuery query)
        {
            List<string> hotMessages = new List<string>();

            if (!File.Exists(query.HotMessageFilePath))
            {
                throw new FileNotFoundException("Cannot find user tooltip message.");
            }

            var xmlNormalAnnouncement = new XmlDocument();

            xmlNormalAnnouncement.Load(query.HotMessageFilePath);

            if (!xmlNormalAnnouncement.DocumentElement.HasChildNodes)
            {
                return Enumerable.Empty<string>();
            }

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

                var hotMessage = GetHotMessageByLanguage(query.RoleId, query.Language.ToLower(), announcementXmlNode);

                if (!string.IsNullOrWhiteSpace(hotMessage))
                {
                    hotMessages.Add(hotMessage);
                }
            }

            return hotMessages;
        }

        private string GetHotMessageByLanguage(byte roleId, string language, XmlNode announcementXmlNode)
        {
            var offerForRoleId = Converter.ToInt(announcementXmlNode.Attributes["role"].Value);
            var startDate = Converter.ToDateTime(announcementXmlNode.Attributes["startdate"].Value);
            var endDate = Converter.ToDateTime(announcementXmlNode.Attributes["enddate"].Value);
            var currentDate = DateTime.Now;

            if (startDate <= currentDate && currentDate <= endDate &&
                (roleId == offerForRoleId || offerForRoleId == 0))
            {
                switch (language)
                {
                    case "en-us":
                        return announcementXmlNode.SelectNodes(Language.English)[0].InnerText;

                    case "zh-tw":
                        return announcementXmlNode.SelectNodes(Language.ChineseTraditional)[0].InnerText;

                    case "zh-cn":
                        return announcementXmlNode.SelectNodes(Language.ChineseSimplified)[0].InnerText;

                    case "ja-jp":
                        return announcementXmlNode.SelectNodes(Language.Japan)[0].InnerText;

                    case "th-th":
                        return announcementXmlNode.SelectNodes(Language.Thailand)[0].InnerText;

                    case "ko-kr":
                        return announcementXmlNode.SelectNodes(Language.Korean)[0].InnerText;

                    case "vi-vn":
                        break;

                    default:
                        return announcementXmlNode.SelectNodes(Language.Default)[0].InnerText;
                }
            }

            return string.Empty;
        }
    }
}