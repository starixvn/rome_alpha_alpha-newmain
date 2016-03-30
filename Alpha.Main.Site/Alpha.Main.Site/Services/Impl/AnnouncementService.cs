namespace Alpha.Main.Site.Services.Impl
{
    using Constants;
    using Criteria.Announcement;
    using Fanex.Common;
    using Fanex.Data.Repository;
    using Helpers;
    using Models.Dtos.Announcement;
    using Models.Requests.Announcement;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class AnnouncementService : IAnnouncementService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IUserSession userSession;
        private readonly SecurityCodeService securityCodeService;
        private readonly UserPermissionService userPermissionService;

        public AnnouncementService(
            IDynamicRepository dynamicRepository,
            IUserSession userSession,
            SecurityCodeService securityCodeService,
            UserPermissionService userPermissionService)
        {
            this.dynamicRepository = dynamicRepository;
            this.userSession = userSession;
            this.securityCodeService = securityCodeService;
            this.userPermissionService = userPermissionService;
        }

        public HotAnnouncement GetHotAnnouncement(HotAnnouncementQuery message)
        {
            var hotAnnouncement = new HotAnnouncement();
            if (securityCodeService.IsActive &&
                securityCodeService.IsRequireSecurityCode)
            {
                return hotAnnouncement;
            }

            IEnumerable<string> messageTypes = null;
            if (userSession.RoleId == RoleIdConstant.Agent &&
                userSession.AdminId == 0)
            {
                messageTypes = dynamicRepository.Fetch<string>(new GetMessageTypeListCriteria
                {
                    CustId = userSession.CustId
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

            hotAnnouncement.LanguagueCode = LanguageHelper.GetShortLanguageCode(userSession.Language);
            return hotAnnouncement;
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
            var rawData = dynamicRepository.Fetch<MessageModel>(criteria).FirstOrDefault();
            if (rawData == null)
            {
                return 0;
            }

            dynamicRepository.Fetch<bool>(new InsertPersonalMessageTargetCriteria
            {
                MessageId = rawData.Id,
                TargetId = userSession.UserId,
                TargetName = userSession.UserName,
                IsRead = false,
                BetTransId = "0"
            });

            dynamicRepository.Fetch<bool>(new UpdateViewStatus
            {
                CustId = userSession.CustId,
                IsViewed = true
            });

            return rawData.Id;
        }

        private List<long> GetShowedMessageIds(HotAnnouncementQuery message)
        {
            var result = new List<long>();

            if (!string.IsNullOrEmpty(message.MessageIds))
            {
                long id = 0;
                result = message.MessageIds
                    .Split(new char[] { ',', ';', ':' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(str => long.TryParse(str, out id))
                    .Select(str => long.Parse(str))
                    .ToList();
            }

            bool isForceUseSecurityCode = securityCodeService.IsForceUseSecurityCode(userSession.Site);
            var messageVisibilities = MessageVisibilities(isForceUseSecurityCode);

            foreach (var messageVisibility in messageVisibilities)
            {
                if (!messageVisibility.Value)
                {
                    result.Add(messageVisibility.Key);
                }
            }

            return result;
        }

        private HotAnnouncement GetHotAnnouncementFromLocalFiles(HotAnnouncementQuery message)
        {
            var showedMessages = GetShowedMessageIds(message);
            var xmlPrivateMsg = GetPrivateAnnouncement(message);
            var xmlCommonMsg = GetCommonAnnouncement(message);
            JoinTheTwoAnnouncementTypes(xmlPrivateMsg, xmlCommonMsg);

            var language = userSession.Language;
            var announcementNodes = xmlCommonMsg.SelectNodes("FileDB/PopupAnnouncement");
            var hotAnnouncement = new HotAnnouncement();
            foreach (XmlNode announcementNode in announcementNodes)
            {
                var nodeIds = announcementNode.Attributes["id"].Value;
                long nodeId = 0;
                long.TryParse(nodeIds, out nodeId);
                if (nodeId == 0 ||
                    showedMessages.Contains(nodeId) ||
                    IsExcludedHotMessageSite(announcementNode, userSession.Site) ||
                    IsExpiredHotMessage(announcementNode))
                {
                    continue;
                }

                hotAnnouncement.Id = nodeId;
                AssignAnnouncementPopupApperance(announcementNode, hotAnnouncement);

                var choosedNodes = announcementNode.SelectNodes(language);
                if (choosedNodes.Count == 0)
                {
                    choosedNodes = announcementNode.SelectNodes(LanguageConstant.DefaultLanguage);
                }

                if (choosedNodes.Count == 0)
                {
                    continue;
                }

                hotAnnouncement.Source = choosedNodes[0].Attributes["src"].Value;
                hotAnnouncement.Content = choosedNodes[0].InnerXml.Replace("{effectdate}", Converter.ToString2(announcementNode.Attributes["effectdate"].Value));
                break;
            }

            return hotAnnouncement;
        }

        private XmlDocument GetPrivateAnnouncement(HotAnnouncementQuery message)
        {
            var pathToPrivateMsg = message.AgentPrivateMessagePath;
            switch (userSession.RoleId)
            {
                case RoleIdConstant.Super:
                    pathToPrivateMsg = message.SuperPrivateMessagePath;
                    break;
                case RoleIdConstant.Master:
                    pathToPrivateMsg = message.SuperPrivateMessagePath;
                    break;
                default:
                    break;
            }

            var xmlPrivateMsg = new XmlDocument();
            xmlPrivateMsg.Load(pathToPrivateMsg);
            return xmlPrivateMsg;
        }

        private XmlDocument GetCommonAnnouncement(HotAnnouncementQuery message)
        {
            var xmlCommonMsg = new XmlDocument();
            xmlCommonMsg.Load(message.CommonAnnouncementPath);
            return xmlCommonMsg;
        }

        private void JoinTheTwoAnnouncementTypes(XmlDocument xmlPrivateMsg, XmlDocument xmlCommonMsg)
        {
            if (xmlPrivateMsg.DocumentElement == null)
            {
                return;
            }

            foreach (XmlNode childNode in xmlPrivateMsg.DocumentElement.ChildNodes)
            {
                var newNode = xmlCommonMsg.ImportNode(childNode, true);
                if (xmlCommonMsg.DocumentElement == null)
                {
                    continue;
                }

                xmlCommonMsg.DocumentElement.AppendChild(newNode);
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

        private bool IsExcludedHotMessageSite(XmlNode node, string site)
        {
            var isPublicSystem = node.Attributes["privatesystem"] != null &&
                (!node.Attributes["privatesystem"].Value.Equals("*") ||
                !string.IsNullOrEmpty(node.Attributes["privatesystem"].Value)) &&
                !node.Attributes["privatesystem"].Value.Equals(site, StringComparison.OrdinalIgnoreCase);
            if (isPublicSystem)
            {
                return true;
            }

            if (node.Attributes["excludesites"] != null &&
                !string.IsNullOrEmpty(node.Attributes["excludesites"].Value))
            {
                var isExcludeSite = node.Attributes["excludesites"].Value.ToUpper().Split(',').Contains(site.ToUpper());
                if (isExcludeSite)
                {
                    return true;
                }
            }

            return false;
        }

        private Dictionary<int, bool> MessageVisibilities(bool isForceUseSecurityCode)
        {
            var messageVisibilities = new Dictionary<int, bool>();
            messageVisibilities.Add(MessageIdConstant.ForceUseSecurityCode, isForceUseSecurityCode);
            messageVisibilities.Add(
                MessageIdConstant.NotOfferedGoldDeluxe,
                userPermissionService.IsShowGoldDeluxeSettingForCredit && !userPermissionService.IsShowLiveCasino);
            messageVisibilities.Add(
                MessageIdConstant.OfferedGoldDeluxe,
                userPermissionService.IsShowGoldDeluxeSettingForCredit && userPermissionService.IsShowLiveCasino);
            messageVisibilities.Add(MessageIdConstant.OfferedSuperLive, userPermissionService.IsShowSuperLive);
            messageVisibilities.Add(MessageIdConstant.OfferedColossusBets, userPermissionService.IsShowColossusBets);

            return messageVisibilities;
        }

        private void AssignAnnouncementPopupApperance(XmlNode announcementNode, HotAnnouncement hotAnnouncement)
        {
            int top;
            int.TryParse(announcementNode.Attributes["top"].Value, out top);
            hotAnnouncement.Top = top;

            int left;
            int.TryParse(announcementNode.Attributes["left"].Value, out left);
            hotAnnouncement.Left = left;

            int width;
            int.TryParse(announcementNode.Attributes["width"].Value, out width);
            hotAnnouncement.Width = width;

            int height;
            int.TryParse(announcementNode.Attributes["height"].Value, out height);
            hotAnnouncement.Height = height;
        }
    }
}
