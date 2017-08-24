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
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;

    public class AnnouncementServiceImpl : AnnouncementService
    {
        public const int ForceUseSecurityCode = 20140512;
        public const int FishingWorldNotice = 3;

        private readonly IDynamicRepository dynamicRepository;
        private readonly IDateTime dateTimeWrapper;

        public AnnouncementServiceImpl(
            IDynamicRepository dynamicRepository = null,
            IDateTime dateTimeWrapper = null)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
            this.dateTimeWrapper = dateTimeWrapper ?? new DateTimeWrap();
        }

        public static bool IsShowForCustomerIds(IXPathNavigable nav, int custId)
        {
            XmlNode node = (XmlNode)nav.CreateNavigator().UnderlyingObject;
            var custIds = node.Attributes["custIds"];
            return custIds == null
                || string.IsNullOrEmpty(custIds.Value)
                || custIds.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().Contains(custId.ToString());
        }

        private static bool IsExcludedByUplineIds(IXPathNavigable nav, int custId)
        {
            XmlNode node = (XmlNode)nav.CreateNavigator().UnderlyingObject;
            var listExcludedUplineIds = node.Attributes["excludedCustIds"];
            return !(listExcludedUplineIds == null || string.IsNullOrEmpty(listExcludedUplineIds.Value)) && listExcludedUplineIds.Value.Contains(custId.ToString());
        }

        public HotAnnouncement GetHotAnnouncement(GetHotAnnouncementsParams getParams)
        {
            if (getParams == null)
            {
                throw new ArgumentNullException(nameof(getParams), "Argument cannot be null");
            }

            var hotAnnouncement = new HotAnnouncement();

            List<string> messageTypes = null;
            if (getParams.AdminId == 0)
            {
                messageTypes = dynamicRepository
                    .Fetch<byte>(new GetMessageTypeListCriteria(getParams.CustId))
                    .Select(messageType => messageType.ToString())
                    .ToList();

                if (messageTypes != null &&
                    !Enumeration.FromValue<UserLevel>(getParams.RoleId).IsAgentLevel())
                {
                    if (messageTypes.Contains(FishingWorldNotice.ToString()))
                    {
                        messageTypes = new List<string> { FishingWorldNotice.ToString() };
                    }
                    else
                    {
                        messageTypes = new List<string>();
                    }
                }
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

        private static bool IsExcludedHotMessageSite(XmlNode node, string site)
        {
            var isPublicSystem = node.Attributes["privatesystem"] != null &&
                (!node.Attributes["privatesystem"].Value.Equals("*") ||
                !string.IsNullOrEmpty(node.Attributes["privatesystem"].Value)) &&
                !node.Attributes["privatesystem"].Value.ToUpperInvariant().Split(',').Contains(site.ToUpperInvariant());

            if (isPublicSystem)
            {
                return true;
            }

            if (node.Attributes["excludesites"] != null &&
                !string.IsNullOrEmpty(node.Attributes["excludesites"].Value))
            {
                var isExcludeSite = node.Attributes["excludesites"].Value.ToUpperInvariant().Split(',').Contains(site.ToUpperInvariant());

                if (isExcludeSite)
                {
                    return true;
                }
            }

            return false;
        }

        internal static string ReplaceProductListToHtml(string innerXml, IEnumerable<string> thirdPartyProductNames)
        {
            if (!thirdPartyProductNames.Any())
            {
                return string.Empty;
            }

            if (!innerXml.Contains("{product-list}"))
            {
                return innerXml;
            }

            int index = 0;
            var productNamesBuilder = new StringBuilder("<ol>");

            foreach (var name in thirdPartyProductNames)
            {
                productNamesBuilder.Append("<li>")
                    .Append(++index)
                    .Append(". ")
                    .Append(name)
                    .Append("</li>");
            }

            productNamesBuilder.Append("</ol>");

            return innerXml.Replace("{product-list}", productNamesBuilder.ToString());
        }

        private string GetSettingChangedMessage(IEnumerable<string> messageTypes, GetHotAnnouncementsParams getParams)
        {
            var noticeTypes = getParams.NoticeTypes;

            var correctType = noticeTypes.FirstOrDefault(noticeType => ShowNotice(noticeType, messageTypes));
            var messageContent = string.Empty;

            if (correctType != null)
            {
                var resourceKey = "Notice_" + correctType;
                messageContent = Resources.ResourceManager.GetString(resourceKey);
                InsertPersonalMessage(new InsertPersonalMessageCriteria(resourceKey), getParams);
                UpdateViewStatus(getParams.CustId, correctType);
            }

            return messageContent;
        }

        private bool ShowNotice(string noticeType, IEnumerable<string> messageTypes)
        {
            var splitChars = new[] { '_' };

            return noticeType.Split(splitChars, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(subMessageType => !messageTypes.Contains(subMessageType)) == null;
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
        }

        private void UpdateViewStatus(int custId, string noticeType)
        {
            var subTypes = noticeType.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var subType in subTypes)
            {
                dynamicRepository.Fetch<bool>(new UpdateViewStatus(custId, true, Converter.ToInt(subType)));
            }
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
                if (!long.TryParse(nodeIds, out nodeId) ||
                    nodeId == 0 ||
                    showedMessages.Contains(nodeId) ||
                    IsExcludedHotMessageSite(announcementNode, getParams.Site) ||
                    !IsShowForCustomerIds(announcementNode.CreateNavigator(), getParams.CustId) ||
                    IsExcludedByUplineIds(announcementNode.CreateNavigator(), getParams.SRecommendId) ||
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

                if (choosedNodes.Count > 0)
                {
                    hotAnnouncement.Source = choosedNodes[0].Attributes["src"].Value;
                    var effectDateAttr = announcementNode.Attributes["effectdate"];
                    hotAnnouncement.Content = effectDateAttr != null ?
                        choosedNodes[0].InnerXml
                            .Replace("{effectdate}", Converter.ToString2(effectDateAttr.Value))
                            .Replace("{site-main}", getParams.SiteMainReplacement) :
                        ReplaceProductListToHtml(choosedNodes[0].InnerXml, getParams.ThirdPartyProductNames);

                    break;
                }
            }

            return hotAnnouncement;
        }        

        private bool IsExpiredHotMessage(XmlNode node)
        {
            XmlAttribute startDateAttribute = node.Attributes["startdate"];
            XmlAttribute endDateAttribute = node.Attributes["enddate"];
            DateTime? startDate = startDateAttribute == null ? (DateTime?)null : Converter.ToDateTime(startDateAttribute.Value);
            DateTime? endDate = endDateAttribute == null ? (DateTime?)null : Converter.ToDateTime(endDateAttribute.Value);
            DateTime currentDate = dateTimeWrapper.Now;

            return !startDate.HasValue ||
                currentDate < startDate ||
                (endDate.HasValue && endDate < currentDate);
        }
    }
}