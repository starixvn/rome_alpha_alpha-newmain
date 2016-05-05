namespace Alpha.Main.Services.Impl
{
    using AppResources;
    using Constants;
    using Criteria.Announcement;
    using Fanex.Common;
    using Fanex.Data.Repository;
    using Helpers;
    using Models.Announcements;
    using Models.Enumerations;
    using Params.Announcements;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class AnnouncementServiceImpl : AnnouncementService
    {
        public const int ForceUseSecurityCode = 20140512;

        public const int PositionTakingChanged = 1;
        public const int BettingLimitChanged = 2;

        private readonly IDynamicRepository dynamicRepository;
        private readonly SecurityCodeService securityCodeService;
        private readonly IDateTime dateTimeWrapper;

        public AnnouncementServiceImpl(
            IDynamicRepository dynamicRepository = null,
            SecurityCodeService securityCodeService = null,
            IDateTime dateTimeWrapper = null)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
            this.securityCodeService = securityCodeService ?? new SecurityCodeServiceImpl();
            this.dateTimeWrapper = dateTimeWrapper ?? new DateTimeWrap();
        }

        public HotAnnouncement GetHotAnnouncement(GetHotAnnouncementsParams getParams)
        {
            var hotAnnouncement = new HotAnnouncement();
            if (getParams.IsSecurityCodeActive &&
                getParams.IsRequireSecurityCode)
            {
                return hotAnnouncement;
            }

            IEnumerable<string> messageTypes = null;
            if (Enumeration.FromValue<UserLevel>(getParams.RoleId).IsAgentLevel() &&
                getParams.AdminId == 0)
            {
                messageTypes = dynamicRepository.Fetch<string>(new GetMessageTypeListCriteria
                {
                    CustId = getParams.CustId
                });
            }

            if (messageTypes != null)
            {
                hotAnnouncement.Content = GetSettingChangedMessage(messageTypes, getParams);
            }

            if (string.IsNullOrEmpty(hotAnnouncement.Content))
            {
                hotAnnouncement = GetHotAnnouncementFromLocalFiles(getParams);
            }

            hotAnnouncement.Languague = getParams.Language;

            return hotAnnouncement;
        }

        private string GetSettingChangedMessage(IEnumerable<string> messageTypes, GetHotAnnouncementsParams getParams)
        {
            var isPositionTakingChanged = messageTypes.Contains(PositionTakingChanged.ToString());
            var isBettingLimitChanged = messageTypes.Contains(BettingLimitChanged.ToString());
            var messageContent = string.Empty;
            if (isPositionTakingChanged && isBettingLimitChanged)
            {
                messageContent = Resources.modifiedMinPTBettingLimitValue;
            }
            else if (isPositionTakingChanged)
            {
                messageContent = Resources.modifiedMinPTValueText;
            }
            else if (isBettingLimitChanged)
            {
                messageContent = Resources.modifiedBettingLimitValue;
            }

            if (!string.IsNullOrEmpty(messageContent))
            {
                var content = messageContent.Replace("<br>", " ");
                InsertPersonalMessage(new InsertPersonalMessageCriteria(content, PositionTakingChanged), getParams);
            }

            return messageContent;
        }

        private void InsertPersonalMessage(InsertPersonalMessageCriteria criteria, GetHotAnnouncementsParams getParams)
        {
            var rawData = dynamicRepository.Fetch<PersonalMessage>(criteria);
            if (rawData == null || !rawData.Any())
            {
                return;
            }

            var firstMessage = rawData.FirstOrDefault();
            dynamicRepository.Fetch<bool>(new InsertPersonalMessageTargetCriteria
            {
                MessageId = firstMessage.Id,
                TargetId = getParams.UserId,
                TargetName = getParams.UserName,
                IsRead = false,
                BetTransId = "0"
            });

            dynamicRepository.Fetch<bool>(new UpdateViewStatus
            {
                CustId = getParams.CustId,
                IsViewed = true
            });
        }

        private List<long> GetShowedMessageIds(GetHotAnnouncementsParams getParams)
        {
            var result = new List<long>();

            if (!string.IsNullOrEmpty(getParams.MessageIds))
            {
                long id = 0;
                result = getParams.MessageIds
                    .Split(new char[] { ',', ';', ':' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(str => long.TryParse(str, out id))
                    .Select(str => long.Parse(str))
                    .ToList();
            }

            bool isForceUseSecurityCode = securityCodeService.IsForceUseSecurityCode(getParams.Site, getParams.SecurityCodeResult, getParams.SecurityCodeSession);
            if (!isForceUseSecurityCode)
            {
                result.Add(ForceUseSecurityCode);
            }

            foreach (var productStatus in getParams.AnnouncementEnableStatus)
            {
                if (!productStatus.Value)
                {
                    result.Add(productStatus.Key);
                }
            }

            return result;
        }

        private HotAnnouncement GetHotAnnouncementFromLocalFiles(GetHotAnnouncementsParams getParams)
        {
            var showedMessages = GetShowedMessageIds(getParams);
            var xmlPrivateMsg = XMLDocumentHelper.LoadDocumentFromPath(getParams.PrivateMessagePath);
            var xmlCommonMsg = XMLDocumentHelper.LoadDocumentFromPath(getParams.CommonAnnouncementPath);
            XMLDocumentHelper.AppendDocument(xmlPrivateMsg, xmlCommonMsg);

            var language = getParams.Language;
            var announcementNodes = xmlCommonMsg.SelectNodes("FileDB/PopupAnnouncement");
            var hotAnnouncement = new HotAnnouncement();
            foreach (XmlNode announcementNode in announcementNodes)
            {
                var nodeIds = announcementNode.Attributes["id"].Value;
                long nodeId = 0;
                long.TryParse(nodeIds, out nodeId);
                if (nodeId == 0 ||
                    showedMessages.Contains(nodeId) ||
                    IsExcludedHotMessageSite(announcementNode, getParams.Site) ||
                    IsExpiredHotMessage(announcementNode))
                {
                    continue;
                }

                hotAnnouncement.Id = nodeId;
                var choosedNodes = announcementNode.SelectNodes(language);

                if (choosedNodes.Count == 0)
                {
                    choosedNodes = announcementNode.SelectNodes(Language.Default);
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

        private bool IsExpiredHotMessage(XmlNode node)
        {
            XmlAttribute startDateAttribute = node.Attributes["startdate"];
            XmlAttribute endDateAttribute = node.Attributes["enddate"];
            XmlAttribute effectDateAttribute = node.Attributes["effectdate"];
            DateTime? startDate = startDateAttribute == null ? (DateTime?)null : Converter.ToDateTime(startDateAttribute.Value);
            DateTime? endDate = endDateAttribute == null ? (DateTime?)null : Converter.ToDateTime(endDateAttribute.Value);
            string effectDate = effectDateAttribute == null ? string.Empty :
                Converter.ToString2(effectDateAttribute.Value);
            DateTime currentDate = dateTimeWrapper.Now;

            return !startDate.HasValue ||
                currentDate < startDate ||
                (endDate.HasValue && endDate < currentDate);
        }

        private bool IsExcludedHotMessageSite(XmlNode node, string site)
        {
            var isPublicSystem = node.Attributes["privatesystem"] != null &&
                (!node.Attributes["privatesystem"].Value.Equals("*") ||
                !string.IsNullOrEmpty(node.Attributes["privatesystem"].Value)) &&
                !node.Attributes["privatesystem"].Value.ToUpper().Split(',').Contains(site.ToUpper());

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
    }
}