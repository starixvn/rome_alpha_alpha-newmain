namespace Alpha.NewMain.Site.Services.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Constants;
    using Criteria.Report;
    using Fanex.Age.Utility;
    using Fanex.Common;
    using Fanex.Data.Repository;
    using Models.Dtos;
    using Models.Requests.Report;
    using Models.Views.Report;
    using Sunplus.Agent.Model.Entities;
    public class AnnouncementService : IAnnouncementService
    {
        private readonly DateTime MaxDateTime = new DateTime(9999, 12, 31);
        private readonly DateTime MinDateTime = new DateTime(1900, 1, 1);
        private const int TotalMessage = 10;

        private readonly IDynamicRepository _dynamicRepository;

        public AnnouncementService(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }

        public AnnouncementViewModel GetAnnouncements(string lastPostTimeRequest)
        {
            var lastPostTime = string.IsNullOrEmpty(lastPostTimeRequest) ? MaxDateTime : Convert.ToDateTime(lastPostTimeRequest);
            if (lastPostTime == MinDateTime)
            {
                lastPostTime = MaxDateTime;
            }

            var siteId = GetSiteId();
            var criteria = new GetAnnouncementsCriteria
            {
                CustId = UserSession.Current.CustId,
                ParentSites = GetParentSites(siteId.ToString()),
                LastPostTime = lastPostTime,
                TotalMessage = TotalMessage,
                IsTest = false
            };

            var rawData = _dynamicRepository.Fetch<AnnouncementModel>(criteria);
            var result = new AnnouncementViewModel
            {
                IsShowAnnouncementTitle = IsShowAnnouncementTitle(lastPostTime),
                AnnouncementGroup = new List<AnnouncementGroupViewModel>()
            };

            var newLastPostTime = MinDateTime;
            if (rawData.Any())
            {
                var datesQuery = rawData
                    .Select(p => p.PostTime.Date)
                    .Distinct()
                    .OrderByDescending(p => p);
                foreach (var date in datesQuery)
                {
                    var announcementGroup = new AnnouncementGroupViewModel
                    {
                        AnnouncementItems = new List<AnnouncementItemViewModel>()
                    };

                    if (date != lastPostTime.Date)
                    {
                        announcementGroup.ShowTitle = true;
                        announcementGroup.GroupId = date.ToShortDateString();
                    }

                    var dateItemsQuery = rawData
                        .Where(p => p.PostTime.Date == date)
                        .OrderByDescending(p => p.PostTime);
                    foreach (var item in dateItemsQuery)
                    {
                        var announcement = new AnnouncementItemViewModel
                        {
                            Id = item.Id.ToString(),
                            PostTime = item.PostTime.ToLongTimeString(),
                            Type = GetMessageType(item),
                            IsRead = item.IsRead
                        };

                        var maxLength = item.IsRead ? 100 : 80;
                        var content = GetAnnouncementContent(item, announcement);
                        announcement.FullContent = content;
                        announcement.BriefContent = GetBriefContent(content, maxLength);
                        newLastPostTime = item.PostTime;
                        announcementGroup.AnnouncementItems.Add(announcement);
                    }

                    result.AnnouncementGroup.Add(announcementGroup);
                }
            }

            if (rawData.Count() == TotalMessage)
            {
                result.IsShowMoreLink = true;
            }

            result.LastPostTime = newLastPostTime;

            HttpContext.Current.Response.SetCookie(new HttpCookie("lastPostTime", result.LastPostTime.ToString()));
            HttpContext.Current.Response.SetCookie(new HttpCookie("moreIsVisible", result.IsShowMoreLink.ToString()));
            return result;
        }

        private bool IsShowAnnouncementTitle(DateTime postTime)
        {
            return postTime == MaxDateTime;
        }

        private string GetBriefContent(string fullContent, int maxLength)
        {
            int length = Math.Min(maxLength, fullContent.Length);
            return fullContent.Length <= length ? fullContent : fullContent.Substring(0, length) + " ...";
        }

        private int GetSiteId()
        {
            switch (UserSession.Current.RoleId)
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

        private string GetAnnouncementContent(AnnouncementModel item, AnnouncementItemViewModel announcement)
        {
            var content = string.Empty;
            var language = UserSession.Current.Language;
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
            if (announcement.Type == MessageTypeNameConstant.Private &&
            !string.IsNullOrEmpty(item.BetTransId))
            {
                content = content.Replace("@@", item.BetTransId);
            }

            return content;
        }
    }
}