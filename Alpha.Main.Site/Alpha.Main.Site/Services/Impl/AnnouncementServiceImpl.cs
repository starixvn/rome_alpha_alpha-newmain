namespace Alpha.Main.Site.Services.Impl
{
    using App_GlobalResources;
    using Constants;
    using Criteria.Announcement;
    using Fanex.Common;
    using Fanex.Data.Repository;
    using Helpers;
    using Models.Announcements;
    using Models.Enumerations;
    using Models.Requests.Announcement;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class AnnouncementServiceImpl : AnnouncementService
    {
        public const int PositionTakingChanged = 1;
        public const int BettingLimitChanged = 2;

        private readonly IDynamicRepository dynamicRepository;
        private readonly SecurityCodeService securityCodeService;

        public AnnouncementServiceImpl(
            IDynamicRepository dynamicRepository,
            SecurityCodeService securityCodeService)
        {
            this.dynamicRepository = dynamicRepository;
            this.securityCodeService = securityCodeService;
        }

        public HotAnnouncement GetHotAnnouncement(HotAnnouncementQuery query)
        {
            var hotAnnouncement = new HotAnnouncement();
            if (query.IsSecurityCodeActive &&
                query.IsRequireSecurityCode)
            {
                return hotAnnouncement;
            }

            IEnumerable<string> messageTypes = null;
            if (Enumeration.FromValue<UserLevel>(query.RoleId).IsAgentLevel() &&
                query.AdminId == 0)
            {
                messageTypes = dynamicRepository.Fetch<string>(new GetMessageTypeListCriteria
                {
                    CustId = query.CustId
                });
            }

            if (messageTypes != null)
            {
                hotAnnouncement.Content = GetSettingChangedMessage(messageTypes, query);
            }

            if (string.IsNullOrEmpty(hotAnnouncement.Content))
            {
                hotAnnouncement = GetHotAnnouncementFromLocalFiles(query);
            }

            hotAnnouncement.Languague = query.Language;

            return hotAnnouncement;
        }

        private string GetSettingChangedMessage(IEnumerable<string> messageTypes, HotAnnouncementQuery query)
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
                InsertPersonalMessage(new InsertPersonalMessageCriteria(content, PositionTakingChanged), query);
            }

            return messageContent;
        }

        private void InsertPersonalMessage(InsertPersonalMessageCriteria criteria, HotAnnouncementQuery query)
        {
            var rawData = dynamicRepository.Fetch<dynamic>(criteria).FirstOrDefault();
            if (rawData == null)
            {
                return;
            }

            dynamicRepository.Fetch<bool>(new InsertPersonalMessageTargetCriteria
            {
                MessageId = rawData.Id,
                TargetId = query.UserId,
                TargetName = query.UserName,
                IsRead = false,
                BetTransId = "0"
            });

            dynamicRepository.Fetch<bool>(new UpdateViewStatus
            {
                CustId = query.CustId,
                IsViewed = true
            });
        }

        private List<long> GetShowedMessageIds(HotAnnouncementQuery query)
        {
            var result = new List<long>();

            if (!string.IsNullOrEmpty(query.MessageIds))
            {
                long id = 0;
                result = query.MessageIds
                    .Split(new char[] { ',', ';', ':' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(str => long.TryParse(str, out id))
                    .Select(str => long.Parse(str))
                    .ToList();
            }

            bool isForceUseSecurityCode = securityCodeService.IsForceUseSecurityCode(query.Site, query.SecurityCodeResult, query.SecurityCodeSession);
            if (!isForceUseSecurityCode)
            {
                result.Add(HotAnnouncementQuery.ForceUseSecurityCode);
            }

            foreach (var productStatus in query.ProductsStatus)
            {
                if (!productStatus.Value)
                {
                    result.Add(productStatus.Key);
                }
            }

            return result;
        }

        private HotAnnouncement GetHotAnnouncementFromLocalFiles(HotAnnouncementQuery query)
        {
            var showedMessages = GetShowedMessageIds(query);
            var xmlPrivateMsg = XMLDocumentHelper.LoadDocumentFromPath(query.PrivateMessagePath);
            var xmlCommonMsg = XMLDocumentHelper.LoadDocumentFromPath(query.CommonAnnouncementPath);
            XMLDocumentHelper.AppendDocument(xmlPrivateMsg, xmlCommonMsg);

            var language = query.Language;
            var announcementNodes = xmlCommonMsg.SelectNodes("FileDB/PopupAnnouncement");
            var hotAnnouncement = new HotAnnouncement();
            foreach (XmlNode announcementNode in announcementNodes)
            {
                var nodeIds = announcementNode.Attributes["id"].Value;
                long nodeId = 0;
                long.TryParse(nodeIds, out nodeId);
                if (nodeId == 0 ||
                    showedMessages.Contains(nodeId) ||
                    IsExcludedHotMessageSite(announcementNode, query.Site) ||
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
            DateTime? currentDate = DateTime.Now;
            return !startDate.HasValue ||
                currentDate < startDate ||
                (endDate.HasValue && endDate < currentDate);
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
    }
}