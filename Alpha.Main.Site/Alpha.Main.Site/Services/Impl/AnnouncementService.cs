namespace Alpha.Main.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using Constants;
    using Criteria.Announcement;
    using Fanex.Age.Utility;
    using Fanex.Common;
    using Fanex.Data.Repository;
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using Models.Dtos.Announcement;
    using Models.Requests.Announcement;
    using NewMain.Site.Models.Dtos.Announcement;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Utilities;

    public class AnnouncementService : IAnnouncementService
    {
        private const string AnnouncementResourcePath = "../Content/Announcement/Resources/";

        private readonly IDynamicRepository _dynamicRepository;
        private readonly IUserSession _userSession;

        public AnnouncementService(IDynamicRepository dynamicRepository, IUserSession userSession)
        {
            _dynamicRepository = dynamicRepository;
            _userSession = userSession;
        }

        public HotAnnouncement GetHotAnnouncement(HotAnnouncementQuery message)
        {
            var hotAnnouncement = new HotAnnouncement();
            hotAnnouncement.CookieId = Encryption.PearsonEncode(_userSession.UserId.ToString() + _userSession.SubAccId.ToString());
            var securityCodeSession = CookieLessSessionFactory.SecurityCodeSession;
            if (SecurityCodeConfig.IsActive && securityCodeSession.IsRequireSecurityCode)
            {
                return hotAnnouncement;
            }

            IEnumerable<string> messageTypes = null;
            if (_userSession.RoleId == RoleIdConstant.Agent &&
                _userSession.AdminId == 0)
            {
                messageTypes = _dynamicRepository.Fetch<string>(new GetMessageTypeListCriteria
                {
                    CustId = _userSession.CustId
                });
            }

            if (messageTypes != null)
            {
                var settingChangedMessage = GetSettingChangedMessage(messageTypes);
                hotAnnouncement.Content = settingChangedMessage.Content;
            }

            if (string.IsNullOrEmpty(hotAnnouncement.Content))
            {
                hotAnnouncement = GetHotAnnouncementFromLocalFiles(message);
            }

            hotAnnouncement.LanguagueCode = GetLanguageCode();

            return hotAnnouncement;
        }

        private string GetLanguageCode()
        {
            switch (_userSession.Language)
            {
                case "th-TH":
                    return "th";
                case "zh-TW":
                case "zh-CN":
                    return "tw";
                case "ko-KR":
                    return "ko";
                case "vi-VN":
                    return "vn";
                default:
                    return "en";
            }
        }

        private string GetBriefContent(string fullContent, int maxLength)
        {
            int length = Math.Min(maxLength, fullContent.Length);
            return fullContent.Length <= length ? fullContent : fullContent.Substring(0, length) + " ...";
        }

        private int GetSiteId()
        {
            switch (_userSession.RoleId)
            {
                case RoleIdConstant.Super:
                    return 3100;
                case RoleIdConstant.Master:
                    return 3200;
                case RoleIdConstant.Agent:
                    return 3300;
                default:
                    return 3000;
            }
        }

        private string GetParentSites(string siteId)
        {
            var list = new List<string>();
            var startSubSiteIndex = siteId.Length - 1;
            while (siteId[startSubSiteIndex] == '0' &&
                startSubSiteIndex >= 0)
            {
                startSubSiteIndex--;
            }

            for (int j = 1; j <= startSubSiteIndex; j++)
            {
                var subSiteSuffix = string.Join(",", Enumerable.Repeat('0', siteId.Length - j));
                list.Add(siteId.Remove(j) + subSiteSuffix);
            }

            list.Add(siteId);
            return string.Join(",", list);
        }

        private string GetMessageType(AnnouncementModel item)
        {
            if (!item.IsRead)
            {
                return MessageTypeNameConstant.PrivateUnread;
            }

            if (item.Type == "1" && item.IsSticky != true)
            {
                return MessageTypeNameConstant.Private;
            }

            if ((item.Type == "1" || item.Type == "3") && item.IsSticky == true)
            {
                return MessageTypeNameConstant.Special;
            }

            if (item.Type == "3" && item.IsSticky == false)
            {
                return MessageTypeNameConstant.Public;
            }

            return MessageTypeNameConstant.Undefined;
        }

        private string GetAnnouncementContent(AnnouncementModel item, AnnouncementModel announcement)
        {
            var content = string.Empty;
            var language = _userSession.Language;
            var contentField = "ContentEn";
            switch (language)
            {
                case "zh-CN":
                    contentField = "ContentCs";
                    break;
                case "zh-TW":
                    contentField = "ContentCh";
                    break;
                case "th-TH":
                    contentField = "ContentTh";
                    break;
                case "ja-JP":
                    contentField = "ContentJp";
                    break;
                case "ko-KR":
                    contentField = "ContentKo";
                    break;
                default:
                    contentField = "ContentEn";
                    break;
            }

            var value = (string)ReflectionUtil.GetValue(item, contentField);
            if (string.IsNullOrEmpty(value) && !language.Contains("zh"))
            {
                contentField = "ContentEn";
            }

            if (language.Contains("zh"))
            {
                var contentCs = item.ContentCs;
                var contentCh = item.ContentCh;
                if (string.IsNullOrEmpty(contentCs.ToString()) &&
                    string.IsNullOrEmpty(contentCh.ToString()))
                {
                    contentField = "ContentEn";
                }
                else
                {
                    if (language.Equals("zh-CN"))
                    {
                        value = item.ContentCs;
                        if (string.IsNullOrEmpty(value.ToString()))
                        {
                            contentField = "ContentCh";
                        }
                    }
                    else
                    {
                        value = item.ContentCh;
                        if (string.IsNullOrEmpty(value.ToString()))
                        {
                            contentField = "ContentCs";
                        }
                    }
                }
            }

            content = Converter.ToString2(ReflectionUtil.GetValue(item, contentField));
            if (!string.IsNullOrEmpty(item.BetTransId) &&
            (announcement.Type == MessageTypeNameConstant.Private || 
            announcement.Type == MessageTypeNameConstant.PrivateUnread))
            {
                content = content.Replace("@@", item.BetTransId);
            }

            return content;
        }

        private MessageModel GetSettingChangedMessage(IEnumerable<string> messageTypes)
        {
            var isPositionTakingChanged = messageTypes.Contains(MessageTypeConstant.PositionTakingChanged.ToString());
            var isBettingLimitChanged = messageTypes.Contains(MessageTypeConstant.BettingLimitChanged.ToString());
            var result = new MessageModel();
            if (isPositionTakingChanged && isBettingLimitChanged)
            {
                result.Content = App_GlobalResources.Resources.modifiedMinPTBettingLimitValue;
            }
            else if (isPositionTakingChanged)
            {
                result.Content = App_GlobalResources.Resources.modifiedMinPTValueText;
            }
            else if (isBettingLimitChanged)
            {
                result.Content = App_GlobalResources.Resources.modifiedBettingLimitValue;
            }

            if (!string.IsNullOrEmpty(result.Content))
            {
                var content = result.Content.Replace("<br>", " ");
                result.Id = InsertPersonalMessage(new InsertPersonalMessageCriteria
                {
                    ContentEn = content,
                    ContentCh = content,
                    ContentCs = content,
                    ContentJp = content,
                    ContentIt = string.Empty,
                    ContentTh = content,
                    ContentKo = content,
                    ContentEs = string.Empty,
                    Type = MessageTypeConstant.PositionTakingChanged,
                    IsSticky = false,
                    MatchId = 0,
                    IsBatch = true
                });
            }

            return result;
        }

        private long InsertPersonalMessage(InsertPersonalMessageCriteria criteria)
        {
            var rawData = _dynamicRepository.Fetch<MessageModel>(criteria).FirstOrDefault();
            if (rawData == null)
            {
                return 0;
            }

            _dynamicRepository.Fetch<bool>(new InsertPersonalMessageTargetCriteria
            {
                MessageId = rawData.Id,
                TargetId = _userSession.UserId,
                TargetName = _userSession.UserName,
                IsRead = false,
                BetTransId = "0"
            });

            _dynamicRepository.Fetch<bool>(new UpdateViewStatus
            {
                CustId = _userSession.CustId,
                IsViewed = true
            });

            return rawData.Id;
        }

        private List<long> GetShowedMessageIds(HotAnnouncementQuery message)
        {
            var result = new List<long>();
            if (!string.IsNullOrEmpty(message.MessageIds))
            {
                var ids = message.MessageIds.Split(new char[] { ',', ';', ':' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string element in ids)
                {
                    int i = 0;
                    if (!int.TryParse(element, out i))
                    {
                        continue;
                    }

                    result.Add(i);
                }
            }

            var securityCodeSession = CookieLessSessionFactory.SecurityCodeSession;
            var status = (SecurityCodeStatus)securityCodeSession.SecurityCodeResult;
            bool isForceUseSecurityCode = SecurityCodeConfig.IsActive &&
               (securityCodeSession.IsRequireSecurityCode ||
               (status == SecurityCodeStatus.FORCED_CHANGE_SC) ||
               (status == SecurityCodeStatus.PROVIDED_SC_SUCCESS && !securityCodeSession.IsInternal));

            if (!isForceUseSecurityCode ||
                _userSession.Site.ToLower().ToLower().Equals("789y"))
            {
                result.Add(MessageIdConstant.ForceUseSecurityCode);
            }

            if (!UserPermission.IsShowGoldDeluxeSettingForCredit)
            {
                result.Add(MessageIdConstant.OfferedGoldDeluxe);
                result.Add(MessageIdConstant.NotOfferedGoldDeluxe);
            }
            else if (UserPermission.IsShowLiveCasino)
            {
                result.Add(MessageIdConstant.NotOfferedGoldDeluxe);
            }
            else
            {
                result.Add(MessageIdConstant.OfferedGoldDeluxe);
            }

            if (!UserPermission.IsShowSuperLive)
            {
                result.Add(MessageIdConstant.OfferedSuperLive);
            }

            if (!UserPermission.IsShowColossusBets)
            {
                result.Add(MessageIdConstant.OfferedColossusBets);
            }

            return result;
        }

        private HotAnnouncement GetHotAnnouncementFromLocalFiles(HotAnnouncementQuery message)
        {
            var showedMessages = GetShowedMessageIds(message);
            var xmlPrivateMsg = GetPrivateAnnouncement();
            var xmlCommonMsg = GetCommonAnnouncement();
            JoinTheTwoAnnouncementTypes(xmlPrivateMsg, xmlCommonMsg);

            string language = _userSession.Language;
            XmlNodeList root = xmlCommonMsg.SelectNodes("FileDB/PopupAnnouncement");
            var hotAnnouncement = new HotAnnouncement();
            foreach (XmlNode node in root)
            {
                var nodeIds = node.Attributes["id"].Value;
                long nodeId = 0;
                long.TryParse(nodeIds, out nodeId);
                if (nodeId == 0 ||
                    showedMessages.Contains(nodeId) ||
                    IsExcludedSiteHotMessage(node, _userSession.Site) ||
                    IsExpiredHotMessage(node))
                {
                    continue;
                }

                hotAnnouncement.Id = nodeId;

                int top;
                int.TryParse(node.Attributes["top"].Value, out top);
                hotAnnouncement.Top = top;

                int left;
                int.TryParse(node.Attributes["left"].Value, out left);
                hotAnnouncement.Left = left;

                int width;
                int.TryParse(node.Attributes["width"].Value, out width);
                hotAnnouncement.Width = width;

                int height;
                int.TryParse(node.Attributes["height"].Value, out height);
                hotAnnouncement.Height = height;

                XmlNodeList choosedNodes = node.SelectNodes(language);
                if (choosedNodes.Count == 0)
                {
                    choosedNodes = node.SelectNodes("en-US");
                }

                if (choosedNodes.Count == 0)
                {
                    continue;
                }

                hotAnnouncement.Source = choosedNodes[0].Attributes["src"].Value;
                hotAnnouncement.Content = choosedNodes[0].InnerXml.Replace("{effectdate}", Converter.ToString2(node.Attributes["effectdate"].Value));
                break;
            }

            return hotAnnouncement;
        }

        private XmlDocument GetPrivateAnnouncement()
        {
            var pathToPrivateMsg = "PopupAdAgentNeutral.xml";
            if (_userSession.IsSuper)
            {
                pathToPrivateMsg = "PopupAdSuperNeutral.xml";
            }
            else if (_userSession.IsMaster)
            {
                pathToPrivateMsg = "PopupAdMasterNeutral.xml";
            }

            var xmlPrivateMsg = new XmlDocument();
            xmlPrivateMsg.Load(HttpContext.Current.Server.MapPath(AnnouncementResourcePath + pathToPrivateMsg));
            return xmlPrivateMsg;
        }

        private XmlDocument GetCommonAnnouncement()
        {
            string pathToCommonMsg = HttpContext.Current.Server.MapPath(AnnouncementResourcePath + "PopupAdCommon.xml");
            var xmlCommonMsg = new XmlDocument();
            xmlCommonMsg.Load(pathToCommonMsg);
            return xmlCommonMsg;
        }

        private void JoinTheTwoAnnouncementTypes(XmlDocument xmlPrivateMsg, XmlDocument xmlCommonMsg)
        {
            if (xmlPrivateMsg.DocumentElement != null)
            {
                foreach (XmlNode childNode in xmlPrivateMsg.DocumentElement.ChildNodes)
                {
                    var newNode = xmlCommonMsg.ImportNode(childNode, true);
                    if (xmlCommonMsg.DocumentElement != null)
                    {
                        xmlCommonMsg.DocumentElement.AppendChild(newNode);
                    }
                }
            }
        }

        private bool IsExpiredHotMessage(XmlNode node)
        {
            XmlAttribute startDateAttribute = node.Attributes["startdate"];
            XmlAttribute endDateAttribute = node.Attributes["enddate"];
            XmlAttribute effectDateAttribute = node.Attributes["effectdate"];
            DateTime? startDate = startDateAttribute == null ? (DateTime?)null : Converter.ToDateTime(startDateAttribute.Value);
            DateTime? endDate = endDateAttribute == null ? (DateTime?)null : Converter.ToDateTime(endDateAttribute.Value);
            string effectDate = effectDateAttribute == null ? string.Empty :
                Converter.ToString2(effectDateAttribute.Value);
            DateTime? currentDate = DateTime.Now;
            return !startDate.HasValue ||
                (!endDate.HasValue && currentDate < startDate) ||
                (endDate.HasValue && (currentDate < startDate || endDate < currentDate));
        }

        private bool IsExcludedSiteHotMessage(XmlNode node, string site)
        {
            if (node.Attributes["privatesystem"] != null &&
                (!node.Attributes["privatesystem"].Value.Equals("*") ||
                !string.IsNullOrEmpty(node.Attributes["privatesystem"].Value)))
            {
                if (!node.Attributes["privatesystem"].Value.Equals(site, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            if (node.Attributes["excludesites"] != null &&
                !string.IsNullOrEmpty(node.Attributes["excludesites"].Value))
            {
                var excludeSites = node.Attributes["excludesites"].Value.ToUpper().Split(',');
                if (excludeSites != null && excludeSites.Contains(site.ToUpper()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
